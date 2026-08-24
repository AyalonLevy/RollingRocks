// Inspired by Nick Hwang https://github.com/rioter00/UnityExamples/blob/master/typewriterUI.cs

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TypewriterUI : MonoBehaviour
{
    [Header("Typewriter Settings")]
    [SerializeField] private TMP_Text _tmpProText;
    [SerializeField] private float delayBeforeStart = 0.0f;
    [SerializeField] private float timeBetweenChars = 0.1f;
    [SerializeField] private string leadingChar = "";
    [SerializeField] private bool leadingCharBeforeDelay = false;

    string writer;

    void Start()
    {
        // TMP for defining everything
        if (_tmpProText != null)
        {
            writer = _tmpProText.text;
            _tmpProText.text = "";

            StartCoroutine(TypeWriterTMP());
        }
        else
        {
            Debug.Log("You forgot your text!");
        }
    }

    public void StartTyping(string textToType)
    {
        if (_tmpProText != null)
        {
            writer = textToType;
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
        }

        if (leadingChar != "")
        {
            _tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length);
        }
    }
}
