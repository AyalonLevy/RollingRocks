// Inspired by Nick Hwang https://github.com/rioter00/UnityExamples/blob/master/typewriterUI.cs

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GepetoAI : MonoBehaviour
{
    [Header("Typewriter Settings")]
    [SerializeField] private TMP_Text _tmpProText;
    [SerializeField] private float delayBeforeStart = 0.0f;
    [SerializeField] private float timeBetweenChars = 0.1f;
    [SerializeField] private string leadingChar = "";
    [SerializeField] private bool leadingCharBeforeDelay = false;
    [SerializeField] private ScrollRect scrollRect;

    private string writer;

    private List<string> chatTexts = new();
    private string introChatText;

    // Chat files location
    private const string ResourcesBasePath = "AI_Text"; // The folder inside Resources that has all the Chat text
    private const string IntroFileName = "Intro";       // The text that will be displayed at the start of the level
    private const string SequentialPrefix = "Chat_";    // The prefix to all the different chat text files

    public void InitializeAI(string levelName)
    {
        LoadLevelChats(levelName);

        TypeChatText(introChatText);
    }

    private void LoadLevelChats(string levelFolderName)
    {
        string levelPath = $"{ResourcesBasePath}/{levelFolderName}";

        introChatText = LoadTextFile($"{levelPath}/{IntroFileName}");

        chatTexts.Clear();
        int idx = 1;

        while (true)
        {
            string fileName = $"{SequentialPrefix}{idx:D2}";
            TextAsset textAsset = LoadTextAssetFile($"{levelPath}/{fileName}");

            // Stop when the file doesn't exist
            if (textAsset == null) break;

            chatTexts.Add(textAsset.text);
            idx++;
        }
    }

    private string LoadTextFile(string path)
    {
        TextAsset asset = LoadTextAssetFile(path);

        if (asset == null)
        {
            return string.Empty;
        }

        return asset.text;
    }

    private TextAsset LoadTextAssetFile(string path)
    {
        TextAsset asset = Resources.Load<TextAsset>(path);

        if (asset == null)
        {
            Debug.Log($"[GepetoAI] Could not find asset at: {path}");
            return null;
        }

        return asset;
    }

    public void TypeChatText(string text)
    {
        if (_tmpProText != null)
        {
            writer = text;
            _tmpProText.text = "";

            StartCoroutine(TypeWriterTMP());
        }
    }

    IEnumerator TypeWriterTMP()
    {
        _tmpProText.text = leadingCharBeforeDelay ? leadingChar : "";

        yield return new WaitForSeconds(delayBeforeStart);

        bool removedLead = false;
        bool skipChars = false;
        string richTextSection = "";

        foreach (char c in writer)
        {
            if (c == '<')
            {
                skipChars = true;

                if (_tmpProText.text.Length > 0 && removedLead)
                {
                    _tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length);
                    removedLead = false;
                }
            }
            
            if (c == '>')
            {
                skipChars = false;
                _tmpProText.text += richTextSection;
                richTextSection = "";
            }

            if (skipChars)
            {
                richTextSection += c;

                continue;
            }

            if (_tmpProText.text.Length > 0 && removedLead)
            {
                _tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length);
                removedLead = false;
            }

            _tmpProText.text += c;
            _tmpProText.text += leadingChar;
            removedLead = true;
            yield return new WaitForSeconds(timeBetweenChars);

            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0.0f;
        }

        if (leadingChar != "")
        {
            _tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length);
        }
    }

    public void HelpRequired()
    {
        // When the help button is pressed it will call this function

        Debug.Log("[GepetoAI] Help is coming!");
    }
}
