using UnityEngine;

public class FollowXYOnly : MonoBehaviour
{
    [SerializeField]
    private Transform target; // Assign this in the Inspector or via script

    [SerializeField]
    private float offset = 1f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 newPosition = transform.position;
        newPosition.x = target.position.x;
        newPosition.y = target.position.y + offset;
        transform.position = newPosition;
    }
}
