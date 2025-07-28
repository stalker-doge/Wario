using UnityEngine;
using UnityEngine.UI;

public class Petal : MonoBehaviour
{
    private bool isPicked = false;

    public void Pick()
    {
        if (isPicked) return;

        isPicked = true;
        PetalGameManager.Instance.PetalPicked();
        gameObject.SetActive(false); // Or play fade-out animation
    }
}