using System;
using System.Collections.Generic;
using UnityEngine;

public class Translation
{
    public static Dictionary<int, LanguageType> idToLanguage = new Dictionary<int, LanguageType>{ { 0, LanguageType.en}, { 1, LanguageType.es }, { 2, LanguageType.zh }, {3, LanguageType.fr }, { 4, LanguageType.ja }, { 5, LanguageType.pt }};

    public static event Action OnLanguageLoaded = null;

    public static Dictionary<string, string> Fields { get; private set; } = new Dictionary<string, string>();
    public static int CurrentLanguageId { get; set; } = 0;

    public static LanguageType[] LanguageTypes
    {
        get
        {
            LanguageType[] languageTypes = new LanguageType[idToLanguage.Count];

            for (int i = 0; i < languageTypes.Length; i++)
            {
                languageTypes[i] = idToLanguage[i];
            }

            return languageTypes;
        }
    }
    
    /// <summary>
    /// Get the current language
    /// </summary>
    /// <returns></returns>
    public static LanguageType GetCurrentLanguage()
    {
        return idToLanguage[CurrentLanguageId];
    }

    /// <summary>
    /// Change the language by the index
    /// </summary>
    public static void ChangeLanguage(int _Id)
    {
        CurrentLanguageId = _Id;
        LoadLanguage(idToLanguage[CurrentLanguageId].ToString());
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

    /// <summary>
    /// Returns the text from the key with the current language
    /// </summary>
    /// <param name="_Key"></param>
    /// <returns></returns>
    public static string GetTextTranslated(string _Key)
    {
        return Fields[_Key];
    }
}
