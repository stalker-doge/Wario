using DG.Tweening;
using UnityEngine;

public class AimShootGameAI : GameAIBase
{
    private ArrowController arrowController = null;

    private bool isTakingAPerfectShot;

    private bool isPlayingMove = false;

    public override bool IsTakingAPerfectShot
    {
        get => isTakingAPerfectShot;
        set => isTakingAPerfectShot = value;
    }

    public override void PlayAIMove(GameObject game)
    {
        if (isPlayingMove)
            return;

        Debug.Log("XYZ PlayAIMove AimAndShootAI");

        arrowController = game.GetComponent<ArrowController>();

        arrowController?.transform.DOKill();

        if (arrowController)
        {
            isPlayingMove = true;
            bool shouldAimAtTarget = Random.Range(0, 2) == 1;
            IsTakingAPerfectShot = shouldAimAtTarget;
            
            if (shouldAimAtTarget)
            {
                Debug.Log("XYZ Perfect Shot Case");
                // Perfect Shot Case
                PerfectShotCase(game);
            } else
            {
                Debug.Log("XYZ Random Shot Case");
                // Random Shot Case
                RandomShotCase(game);
            }
            
        }
    }

    private void PerfectShotCase(GameObject game)
    {
        if (!DOTween.IsTweening(game.transform))
        {
            FindTarget(game.transform, Random.Range(0, 2) == 1 ? MoveTypeAimAndShoot.AimLeft : MoveTypeAimAndShoot.AimRight);
        }
    }
    private void RandomShotCase(GameObject game)
    {
        if (!DOTween.IsTweening(game.transform))
        {
            FindTarget(game.transform, Random.Range(0, 2) == 1 ? MoveTypeAimAndShoot.AimLeft : MoveTypeAimAndShoot.AimRight);
        }
    }

    public override void ChargeAndShoot(GameObject game)
    {
        base.ChargeAndShoot(game);
        TrajectoryPredictor.IsEligibleToShoot = false;
        StopFindingTarget(game);
        DOVirtual.DelayedCall(Random.Range(0.5f, 1.5f), () => AIShootLogic());

        //Invoke("AIShootLogic", Random.Range(0.5f, 1.5f));
    }
    private void AIShootLogic()
    {
        //Debug.Log("XYZ AIShootLogic");
        Bullet.ShootBulletLogicAICallback?.Invoke();
        arrowController.HandleShot();
        if (!arrowController.HasFiredAllShots())
        {
            DOVirtual.DelayedCall(Random.Range(0.5f, 1.5f), () => TakeTimeBeforeShootingAgain());
        }

        //Invoke("TakeTimeBeforeShootingAgain", Random.Range(0.5f, 1.5f));
    }

    private void StopFindingTarget(GameObject game)
    {
        //arrowController.AIFoundTarget = true;
        game.transform.DOKill();
    }

    private void TakeTimeBeforeShootingAgain()
    {
        //Debug.Log("XYZ TakeTimeBeforeShooting");
        arrowController.gameObject.transform.DORotate(new Vector3(0, 0, Random.Range(70, 90)), Random.Range(2, 4)).SetEase(Ease.Linear).OnComplete(() =>
        {
            isPlayingMove = false;
            TrajectoryPredictor.IsEligibleToShoot = true;
        });
    }
    public void FindTarget(Transform transform, MoveTypeAimAndShoot moveType)
    {
        //Debug.Log("XYZ FindTargetCalled");
        //if (DOTween.IsTweening(transform))
        //    return;

        if (moveType == MoveTypeAimAndShoot.AimLeft)
        {
            transform.DORotate(isTakingAPerfectShot ? new Vector3(0, 0, 180) : new Vector3(0, 0, Random.Range(120, 160)), isTakingAPerfectShot ? Random.Range(6, 10): Random.Range(3, 5)).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (!isTakingAPerfectShot)
                {
                    ChargeAndShoot(arrowController.GetComponent<TrajectoryPredictor>().gameObject);
                }
            });
        }
        else if (moveType == MoveTypeAimAndShoot.AimRight)
        {
            transform.DORotate(isTakingAPerfectShot ? new Vector3(0, 0, 0) : new Vector3(0, 0, Random.Range(30, 70)), isTakingAPerfectShot ? Random.Range(6, 10) : Random.Range(3, 5)).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (!isTakingAPerfectShot)
                {
                    ChargeAndShoot(arrowController.GetComponent<TrajectoryPredictor>().gameObject);
                }
            });
        }
    }
}