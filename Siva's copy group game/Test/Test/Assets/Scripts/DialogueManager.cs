using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable enable

namespace Dialogue
{

    public delegate (string textLocalizationKey, string audioLocalizationKey,
        // TODO Leave this empty for no responses
        IList<ResponseChoice> responseChoices) DialogueBox();
    public record ResponseChoice(string textLocalizationKey, DialogueBox? response) { }

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
        }

        public static bool IsMenuOpen = false;
        public static IList<ResponseChoice>? CurrentResponseChoices;
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

            // Run the NpcDialogue, so we can use the results
            (string textLocalizationKey, string audioLocalizationKey, IList<ResponseChoice> responseChoices) = dialogue();

            // Show NPC dialogue text
            TextMeshProUGUI npcText = instance.npcDialogueText.GetComponentInChildren<TextMeshProUGUI>();
            npcText.text = textLocalizationKey;

            // TODO Play NPC dialogue audio
            // whereToPlayAudio.clip = audioLocalizationKey;
            // whereToPlayAudio.Play();
            CurrentAudioSource = whereToPlayAudio;

            // Make the response choice buttons
            responseChoices.Enumerate((responseIndex, responseChoice) =>
            {
                // TODO localization
                GameObject buttonAsObject = Instantiate(instance.responseChoiceButton, new Vector3(540, 960), Quaternion.identity, instance.canvas.transform);

                Button button = buttonAsObject.GetComponent<Button>();
                TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();
                text.text = responseChoice.textLocalizationKey;
                text.richText = true;
                text.enableWordWrapping = true;

                var buttonScript = button.GetComponent<DialogueButton>();
                buttonScript.ResponseIndex = responseIndex;

                // TODO expand button to fit text

                // TODO skippable dialogue, which automatically plays npc audio / user audio
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
            // TODO hide/show menu when appropriate
        }

        public static void ResponseChoiceClicked(int responseIndex)
        {
            CloseMenu();

            ResponseChoice response = CurrentResponseChoices[responseIndex];
            if (response.response != null)
            {
                OpenMenu(response.response, CurrentAudioSource);
            }
        }
    }

    // TODO refactor these into utils class
    public static class ExtensionMethods
    {
        public static void Enumerate<T>(this IEnumerable<T> enumerable, Action<int, T> action)
        {
            int i = 0;
            foreach (T item in enumerable)
            {
                action(i, item);
                i++;
            }
        }
    }
}

    // Hacky fix to a weird error. Ignore this
    namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit { }
}