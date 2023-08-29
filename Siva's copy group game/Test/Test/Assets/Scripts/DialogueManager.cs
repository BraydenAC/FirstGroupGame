using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using Utils;

#nullable enable

namespace Dialogue
{

    public delegate (string textLocalizationTable, string textLocalizationKey, string audioLocalizationTable, string audioLocalizationKey,
        IList<ResponseChoice> responseChoices) DialogueBox();
    
    public record ResponseChoice(string textLocalizationTable, string textLocalizationKey, DialogueBox? response) { }

    public class DialogueManager : MonoBehaviour
    {

        [SerializeField]
        GameObject canvas;

        [SerializeField]
        GameObject responseChoiceButton;

        [SerializeField]
        GameObject npcDialogueText;

        static DialogueManager instance;

        public void Start()
        {
            instance = this;
            instance.canvas.SetActive(false);
        }

        public static bool IsMenuOpen
        {
            get;
            private set;
        } = false;

        public static IList<ResponseChoice> CurrentResponseChoices = new List<ResponseChoice>();
        public static AudioSource? CurrentAudioSource;

        public static void OpenMenu(DialogueBox dialogue, AudioSource whereToPlayAudio)
        {
            // Make sure the player didn't try to open two dialogue menus at once
            if (IsMenuOpen)
            {
                Debug.LogWarning("Attempted to open dialogue menu when it was already open");
                return;
            }
            // Mark the menu as open
            IsMenuOpen = true;
            instance.canvas.SetActive(true);

            // Run the NpcDialogue, so we can use the results
            (string textLocalizationTable, string textLocalizationKey, string audioLocalizationTable, string audioLocalizationKey,
            IList<ResponseChoice> responseChoices) = dialogue();

            // Show NPC dialogue text
            TextMeshProUGUI npcText = instance.npcDialogueText.GetComponentInChildren<TextMeshProUGUI>();
            npcText.text = LocalizationSettings.StringDatabase.GetLocalizedString(textLocalizationTable, textLocalizationKey);

            // TODO Play NPC dialogue audio
            // whereToPlayAudio.clip = LocalizationSettings.AssetDatabase.GetLocalizedAsset<AudioClip>(audioLocalizationTable, audioLocalizationKey);
            // whereToPlayAudio.Play();
            CurrentAudioSource = whereToPlayAudio;

            // Make the response choice buttons
            responseChoices.Enumerate((responseIndex, responseChoice) =>
            {
                GameObject buttonAsObject = Instantiate(instance.responseChoiceButton, new Vector3(540, 960), Quaternion.identity, instance.canvas.transform);

                Button button = buttonAsObject.GetComponent<Button>();
                TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();
                text.text = LocalizationSettings.StringDatabase.GetLocalizedString(responseChoice.textLocalizationTable, responseChoice.textLocalizationKey);
                text.richText = true;
                text.enableWordWrapping = true;

                var buttonScript = button.GetComponent<DialogueButton>();
                buttonScript.ResponseIndex = responseIndex;

                // TODO expand button to fit text, using text.preferredHeight, or ContentSizeFitter or whatever it's called
                // https://stackoverflow.com/questions/29166380/how-to-change-width-of-a-ui-button-by-code
            });
            CurrentResponseChoices = responseChoices;
        }

        public static void CloseMenu()
        {
            // Remove all dialogue options
            Button[] buttons = instance.canvas.GetComponentsInChildren<Button>();
            foreach (var button in buttons)
            {
                // TODO properly destroy them
                Destroy(button);
            }

            // Mark the menu as closed
            IsMenuOpen = false;
            instance.canvas.SetActive(false);
        }

        public static void ResponseChoiceClicked(int responseIndex)
        {
            CloseMenu();

            ResponseChoice response = CurrentResponseChoices[responseIndex];
            if (response.response != null)
            {
                if (CurrentAudioSource == null)
                {
                    throw new NullReferenceException();
                }
                OpenMenu(response.response, CurrentAudioSource);
            }
        }
    }
}

    // Hacky fix to a weird error. Ignore this
    namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit { }
}