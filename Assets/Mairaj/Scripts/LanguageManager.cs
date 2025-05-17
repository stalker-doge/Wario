//Mairaj Muhammad ->2415831
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine.UI;
using System;

public class LanguageManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown myDropdown;

    [SerializeField]
    private LanguageData languageData;

    private const string LANGUAGE_INDEX_PREF_KEY = "SelectedDropdownIndexLanguages";

    private const string LANGUAGE_SAVED_PREF_KEY = "SelectedLanguage";
    private void Start()
    {
        // Handles the case where drop down option is set as last selected one instead by default
        int savedValue = PlayerPrefs.GetInt(LANGUAGE_INDEX_PREF_KEY, 0); // 0 is default

        if (myDropdown)
        {
            myDropdown.value = savedValue;
            myDropdown.RefreshShownValue(); // update UI
            myDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        } else
        {
            string savedLanguage = PlayerPrefs.GetString(LANGUAGE_SAVED_PREF_KEY, "en");
            ChangeLanguage(savedLanguage);
        }
    }

    void OnDropdownValueChanged(int selectedIndex)
    {
        PlayerPrefs.SetInt(LANGUAGE_INDEX_PREF_KEY, selectedIndex);

        string selectedText = myDropdown.options[selectedIndex].text;

        string languageCode = languageData.data.Where(data => data.language == selectedText).FirstOrDefault().languageCode;

        PlayerPrefs.SetString(LANGUAGE_SAVED_PREF_KEY, languageCode);

        PlayerPrefs.Save();

        ChangeLanguage(languageCode);
    }

    // Call this to change language, e.g., "en", "es", "fr", etc.
    public void ChangeLanguage(string localeCode)
    {
        StartCoroutine(SetLocale(localeCode));
    }

    private IEnumerator SetLocale(string localeCode)
    {
        // Wait until localization system is initialized
        yield return LocalizationSettings.InitializationOperation;

        var availableLocales = LocalizationSettings.AvailableLocales.Locales;

        // Find the locale by code
        Locale selectedLocale = availableLocales.Find(locale => locale.Identifier.Code == localeCode);

        if (selectedLocale != null)
        {
            LocalizationSettings.SelectedLocale = selectedLocale;
            Debug.Log($"Language changed to: {selectedLocale.LocaleName}");
        }
        else
        {
            Debug.LogWarning($"Locale '{localeCode}' not found in AvailableLocales.");
        }
    }
}
