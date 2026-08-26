// Typwriter inspired by Nick Hwang https://github.com/rioter00/UnityExamples/blob/master/typewriterUI.cs

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GepetoAI : MonoBehaviour
{
    [Header("Typewriter Settings")]
    [SerializeField] private TMP_Text textField;
    [SerializeField] private float delayBeforeStart = 0.0f;
    [SerializeField] private float timeBetweenChars = 0.1f;
    [SerializeField] private string leadingChar = "";
    [SerializeField] private bool leadingCharBeforeDelay = false;
    [SerializeField] private ScrollRect scrollRect;

    [Header("AI Settings")]
    [SerializeField] private int maxButtonClicks = 10;

    private string writer;

    private List<string> _chatTexts = new();
    private List<string> _angryTexts = new();
    private string _introChatText;
    private int _chatIdx = 0;
    private int _buttonPressedCounter = 0;

    private Coroutine _chatCoroutine;

    // Chat files location
    private const string ResourcesBasePath = "AI_Text"; // The folder inside Resources that has all the Chat text
    private const string IntroFileName = "Intro";       // The text that will be displayed at the start of the level
    private const string SequentialPrefix = "Chat_";    // The prefix to all the different chat text files
    private const string AngryFolder = "Angry";         // The folder where all the angry texts are located
    private const string AngryPrefix = "Angry_";        // The prefix to all the different angry text files

    public void InitializeAI(string levelName)
    {
        LoadLevelChats(levelName);
        if (_angryTexts.Count == 0)
        {
            LoadAngryTexts();
        }

        TypeChatText(_introChatText);
        _buttonPressedCounter = 0;
    }

    private void LoadLevelChats(string levelFolderName)
    {
        string levelPath = $"{ResourcesBasePath}/{levelFolderName}";

        _introChatText = LoadTextFile($"{levelPath}/{IntroFileName}");

        _chatTexts.Clear();
        int idx = 1;

        while (true)
        {
            string fileName = $"{SequentialPrefix}{idx:D2}";
            TextAsset textAsset = LoadTextAssetFile($"{levelPath}/{fileName}");

            // Stop when the file doesn't exist
            if (textAsset == null) break;

            _chatTexts.Add(textAsset.text);
            idx++;
        }
    }

    private void LoadAngryTexts()
    {
        string angryPath = $"{ResourcesBasePath}/{AngryFolder}";

        _angryTexts.Clear();
        int idx = 1;

        while (true)
        {
            string fileName = $"{AngryPrefix}{idx:D2}";
            TextAsset textAsset = LoadTextAssetFile($"{angryPath}/{fileName}");

            // Stop when the file doesn't exist
            if (textAsset == null) break;

            _angryTexts.Add(textAsset.text);
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
        if (textField != null)
        {
            writer = text;
            textField.text = "";

            _chatCoroutine = StartCoroutine(TypeWriterTMP());
        }
    }

    IEnumerator TypeWriterTMP()
    {
        textField.text = leadingCharBeforeDelay ? leadingChar : "";

        yield return new WaitForSeconds(delayBeforeStart);

        bool removedLead = false;
        bool skipChars = false;
        string richTextSection = "";

        foreach (char c in writer)
        {
            if (c == '<')
            {
                skipChars = true;

                if (textField.text.Length > 0 && removedLead)
                {
                    textField.text = textField.text.Substring(0, textField.text.Length - leadingChar.Length);
                    removedLead = false;
                }
            }

            if (c == '>')
            {
                skipChars = false;
                textField.text += richTextSection;
                richTextSection = "";
            }

            if (skipChars)
            {
                richTextSection += c;

                continue;
            }

            if (textField.text.Length > 0 && removedLead)
            {
                textField.text = textField.text.Substring(0, textField.text.Length - leadingChar.Length);
                removedLead = false;
            }

            textField.text += c;
            textField.text += leadingChar;
            removedLead = true;
            yield return new WaitForSeconds(timeBetweenChars);

            ScrollToBottom();
        }

        if (leadingChar != "")
        {
            textField.text = textField.text.Substring(0, textField.text.Length - leadingChar.Length);
        }
    }

    private void ScrollToBottom()
    {
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0.0f;
    }

    public void HelpRequired()
    {
        // If the Coroutine it will stop the old one and start a new one
        if (_chatCoroutine != null)
        {
            StopCoroutine(_chatCoroutine);
        }

        // OR

        // Wait for the coroutine to stop and prevent the button from working - pop-up a angry message after a few attempts

        _buttonPressedCounter++;

        if (_buttonPressedCounter > maxButtonClicks)
        {
            // Send a random angry message
            int randIdx = Random.Range(0, _angryTexts.Count);
            TypeChatText(_angryTexts[randIdx]);
            return;
        }

        if (_chatTexts.Count == 0)
        {
            TypeChatText(_introChatText);
        }
        else
        {
            // Use the modulo to cycle through the chats forever
            int idx = _chatIdx % _chatTexts.Count;
            TypeChatText(_chatTexts[idx]);

            _chatIdx++;
        }
    }
}
