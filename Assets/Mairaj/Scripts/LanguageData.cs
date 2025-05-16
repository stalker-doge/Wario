//Mairaj Muhammad ->2415831
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LanguageData", menuName = "ScriptableObjects/LanguageData")]
public class LanguageData : ScriptableObject
{
    public Data [] data;

    [Serializable]
    public class Data
    {
        public string language;
        public string languageCode;
    }
}
