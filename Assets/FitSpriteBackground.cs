using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FitSpriteBackground : MonoBehaviour
{
    void Start()
    {
        FitToScreen();
    }

    void FitToScreen()
    {
        // Get reference to camera and sprite
        Camera cam = Camera.main;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (cam == null || sr == null || sr.sprite == null)
        {
            Debug.LogWarning("Camera or SpriteRenderer is missing.");
            return;
        }

        // Get sprite size in world units
        float spriteWidth = sr.sprite.bounds.size.x;
        float spriteHeight = sr.sprite.bounds.size.y;

        // Get screen height and width in world units
        float screenHeight = 2f * cam.orthographicSize;
        float screenWidth = screenHeight * cam.aspect;

        // Calculate scale factors to fit
        float scaleX = screenWidth / spriteWidth;
        float scaleY = screenHeight / spriteHeight;

        // Apply scale
        transform.localScale = new Vector3(scaleX, scaleY, 1);
    }
}