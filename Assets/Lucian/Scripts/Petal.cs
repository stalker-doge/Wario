using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Petal : MonoBehaviour, IPointerClickHandler
{
    private bool isPicked = false;

    public void Pick()
    {
        if (isPicked) return;

        isPicked = true;
        PetalGameManager.Instance.PetalPicked();
        gameObject.SetActive(false); // Or play fade-out animation
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Pick();
    }
}