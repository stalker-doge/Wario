//Mairaj Muhammad ->2415831
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System.Collections;

public class LanguageManager : MonoBehaviour
{
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
