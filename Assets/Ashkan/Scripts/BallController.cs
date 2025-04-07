using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 5f;
    public float maxX = 1.5f; // Set this based on screen width or use dynamic value later

    void Update()
    {
        Vector3 accel = Input.acceleration;
        float tiltX = 0f;

        // Adjust input based on screen orientation
        switch (Screen.orientation)
        {
            case ScreenOrientation.Portrait:
                tiltX = accel.x;
                break;

            case ScreenOrientation.LandscapeLeft:
                tiltX = accel.y;
                break;

            case ScreenOrientation.LandscapeRight:
                tiltX = -accel.y;
                break;

            case ScreenOrientation.PortraitUpsideDown:
                tiltX = -accel.x;
                break;
        }

        Vector3 move = new Vector3(tiltX, 0f, 0f);
        transform.Translate(move * speed * Time.deltaTime);

        // Clamp position so ball stays within screen bounds
        float clampedX = Mathf.Clamp(transform.position.x, -maxX, maxX);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            //GameManager Current mini game finished
            
            Debug.Log("Game Over");
        }
    }
}