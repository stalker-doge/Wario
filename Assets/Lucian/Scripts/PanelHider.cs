using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelHider : MonoBehaviour
{
    public GameObject panel;

    public void HidePanel()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }
}
