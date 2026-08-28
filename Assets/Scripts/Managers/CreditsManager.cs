using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    /// The flow is as follows
    /// 1. The basic credits appear, the AI is writing the Intro message.
    /// 2. When the AI finishes, the disclaimer appears
    /// 3. The AI writes a comment regarding the no use of AI,
    /// 4. The credits replay by a personal message from the developer (me) that explains that this AI is an Artificial Idiot
    /// 5. The AI rants and when it is done it "turnes of the game"
    /// 6. black screen -> wait 5 seconds
    /// 7. Back to main menu

    [Header("Game Credits text")]
    [Tooltip("Place credits by order!")]
    [SerializeField] private GameObject[] credits;
    [SerializeField] private GepetoAI gepetoAI;
    [Tooltip("Place AI responses by order!")]
    [SerializeField] private TextAsset[] aiChats;
    [SerializeField] float timeBetweenInteractions = 0.5f;
    [SerializeField] Animator transitionScreen;
    [SerializeField] AudioMixer audioMixer;

    private void Awake()
    {
        if (credits == null)
        {
            Debug.LogError("[CreditsManager] There are no credits to display");
            return;
        }

        if (gepetoAI == null)
        {
            Debug.LogWarning("[CreditsManager] No AI was linked, aborting");
            return;
        }

        foreach (GameObject credit in credits)
        {
            credit.SetActive(false);
        }

        gepetoAI.DisableButton();

        transitionScreen.gameObject.SetActive(false);

        StartCoroutine(StartTheChatter());
    }

    private IEnumerator StartTheChatter()
    {
        int textIndex = 0;
        int aiChatsIndex = 0;

        // Display first 2 text boxes
        if (textIndex < credits.Length) credits[textIndex++].SetActive(true);
        if (textIndex < credits.Length) credits[textIndex++].SetActive(true);

        // First AI chat - Intro -> congratulates the player for finishing the game
        if (aiChatsIndex < aiChats.Length)
        {
            gepetoAI.TypeChatText(aiChats[aiChatsIndex++].text);
        }

        while (aiChatsIndex < aiChats.Length || textIndex < credits.Length)
        {
            // Pause the coroutine until the AI finishes typing
            yield return new WaitUntil(() => gepetoAI.FinishedTyping);

            //// Time to let the user read the text
            //yield return new WaitForSeconds(timeBetweenInteractions);

            if (textIndex < credits.Length)
            {
                credits[textIndex++].SetActive(true);

                yield return new WaitForSeconds(timeBetweenInteractions);
            }

            if (aiChatsIndex < aiChats.Length)
            {
                gepetoAI.TypeChatText(aiChats[aiChatsIndex++].text);
            }

            yield return new WaitUntil(() => gepetoAI.FinishedTyping);

        }

        yield return new WaitForSeconds(timeBetweenInteractions);

        gepetoAI.EnableButton();

        transitionScreen.gameObject.SetActive(true);

        audioMixer.SetFloat("MusicVolume", -80);
        audioMixer.SetFloat("SFXVolume", -80);

        yield return new WaitForSeconds(5);

        SceneManager.LoadScene("MainMenu");
    }
}
