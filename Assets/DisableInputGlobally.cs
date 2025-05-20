using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DisableInputGlobally : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
        {
           
        }
    }
}