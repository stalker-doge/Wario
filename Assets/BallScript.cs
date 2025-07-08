using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
 
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Goal"))
        {
            Debug.Log("Goal");
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground") && DragController.Instance.shoot)
        {
            StartCoroutine(RespawnWithDelay());
            DragController.Instance.shoot = false;
        }
    }

    IEnumerator RespawnWithDelay()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<Rigidbody2D>().Sleep();
        gameObject.transform.position = DragController.Instance.spawnPoint.position;

    }
}
