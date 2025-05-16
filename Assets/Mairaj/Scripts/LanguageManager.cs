//Mairaj Muhammad ->2415831
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using System.Linq;

public class LanguageManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown myDropdown;

    [SerializeField]
    private LanguageData languageData;
    private void Start()
    {
        myDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    void OnDropdownValueChanged(int selectedIndex)
    {
        string selectedText = myDropdown.options[selectedIndex].text;

        ChangeLanguage(languageData.data.Where(data => data.language == selectedText).FirstOrDefault().languageCode);
    }

    // Call this to change language, e.g., "en", "es", "fr", etc.
    public void ChangeLanguage(string localeCode)
    {
        Debug.Log("XYZ Code " + localeCode);
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
