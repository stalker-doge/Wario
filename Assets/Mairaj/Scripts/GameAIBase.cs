using UnityEngine;

public abstract class GameAIBase
{
    public abstract void PlayAIMove(GameObject game);

    public virtual void ChargeAndShoot(GameObject game) { } // AimAndShootGameAI Exclusive

    public virtual bool IsTakingAPerfectShot { get => false; set { } } // AimAndShootGameAI Exclusive
}