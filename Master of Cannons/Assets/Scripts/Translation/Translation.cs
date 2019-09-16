using System;
using System.Collections.Generic;
using UnityEngine;

public class Translation
{
    public static Dictionary<byte, string> idToLanguage = new Dictionary<byte, string>{ { 0, "en" }, { 1, "es" }, { 2, "zh" }, {3, "fr" }, { 4, "ja" }, { 5, "pt" }};
    public static Dictionary<string, string> Fields { get; private set; } = new Dictionary<string, string>();

    public static byte currentLanguageId = 0;

    public static event Action OnLanguageLoaded = null;

    /// <summary>
    /// Get the current language
    /// </summary>
    /// <returns></returns>
    public static LanguageType GetCurrentLanguage()
    {
        LanguageType currentLanguage = LanguageType.UNKNOWN;

        switch (idToLanguage[currentLanguageId])
        {
            case "en":
                currentLanguage = LanguageType.en;
                break;
            case "es":
                currentLanguage = LanguageType.es;
                break;
            case "zh":
                currentLanguage = LanguageType.zh;
                break;
            case "fr":
                currentLanguage = LanguageType.fr;
                break;
            case "ja":
                currentLanguage = LanguageType.ja;
                break;
            case "pt":
                currentLanguage = LanguageType.pt;
                break;
            default:
                currentLanguage = LanguageType.UNKNOWN;
                break;
        }

        return currentLanguage;
    }

    /// <summary>
    /// Change the language ordinally
    /// </summary>
    public static void ChangeLanguage()
    {
        currentLanguageId++;

        if (currentLanguageId > idToLanguage.Count - 1) currentLanguageId = 0;

        LoadLanguage(idToLanguage[currentLanguageId]);
    }

    /// <summary>
    /// Load language and fill the dictionary
    /// </summary>
    /// <param name="_Language"></param>
    public static void LoadLanguage(string _Language)
    {
        if (Fields == null) Fields = new Dictionary<string, string>();
        Fields.Clear();

        string lang = _Language;
        TextAsset textAsset = Resources.Load<TextAsset>(@"Languages/" + lang);
        string allText = "";

        if (textAsset == null) textAsset = Resources.Load<TextAsset>(@"Languages/en");
        if (textAsset == null) Debug.LogError("File not found: Assets/Resources/Languages/" + lang + ".txt");

        allText = textAsset.text;

        string[] lines = allText.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
        string key, value;

        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].IndexOf("=") >= 0 && !lines[i].StartsWith("#"))
            {
                key = lines[i].Substring(0, lines[i].IndexOf("="));
                value = lines[i].Substring(lines[i].IndexOf("=") + 1, lines[i].Length - lines[i].IndexOf("=") - 1).Replace("\\n", Environment.NewLine);
                Fields.Add(key, value);
            }
        }

        OnLanguageLoaded?.Invoke();
    }
}
