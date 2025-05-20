using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashTransition : MonoBehaviour
{
   public void DeactivePage()
   {
      gameObject.SetActive(false);
      transform.parent.gameObject.SetActive(false);
   }
}
