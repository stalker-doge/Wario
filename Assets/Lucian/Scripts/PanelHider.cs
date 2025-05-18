using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelHider : MonoBehaviour
{
    public GameObject panel;

    public void HidePanel()
    {
        //waits for 1 second before hiding the panel

        HidePanelCoroutine(1f);
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    public IEnumerator HidePanelCoroutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }
}
