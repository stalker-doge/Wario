using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LoadingSeanCharecters : MonoBehaviour
{
   [SerializeField]GameObject MaleCharecter,FemaleCharecter;

   private void Start()
   {
      MaleCharecter.SetActive(false);
      FemaleCharecter.SetActive(false);
      
      int rnd = Random.Range(0, 10);
      if (rnd%2 == 0)
      {
         MaleCharecter.SetActive(true);
      }
      else
      {
         FemaleCharecter.SetActive(true);
      }
   }
}
