using Dialogue;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using System.Linq;
using Utils;

#nullable enable

namespace Dialogue
{
    // TODO Figure out how to add assets to localization tables, and do it

    public class NpcWithNoMemory : MonoBehaviour
    {
        public AudioSource AudioSource;
        public NoMemoryDialogueBox FirstDialogue;

        [Serializable]
        public struct NoMemoryDialogueBox
        {
            public LocalizedString Text;

            public LocalizedAsset<AudioClip> Audio;

            public NoMemoryResponseChoice[] PlayerResponseChoices;
        }

        [Serializable]
        public struct NoMemoryResponseChoice
        {
            public LocalizedString Text;

            public NoMemoryDialogueBox NpcResponse;
        }

        // Script for NPCs that, when right-clicked, go through the same dialogue tree every time
        // Fully customizable through Unity's Inspector, no code needed

        private (string textLocalizationTable, string textLocalizationKey, string audioLocalizationTable, string audioLocalizationKey,
            IList<ResponseChoice> responseChoices) whenDialogueBoxAppears(NoMemoryDialogueBox dialogueBox)
        {
            var realResponseChoices = from i in dialogueBox.PlayerResponseChoices
                                      select new ResponseChoice(i.Text.GetTableName(),
                                                                i.Text.GetKeyName(),
                                                                () => whenDialogueBoxAppears(i.NpcResponse));

            return (dialogueBox.Text.GetTableName(),
                    dialogueBox.Text.GetKeyName(),
                    dialogueBox.Audio.GetTableName(),
                    dialogueBox.Audio.GetKeyName(),
                    realResponseChoices.ToList<ResponseChoice>());
        }

        // When NPC is right-clicked:

        public void OnClick(BaseEventData baseEvent)
        {
            PointerEventData pointerEventData = (PointerEventData)baseEvent;

            if (pointerEventData.button == PointerEventData.InputButton.Right)
            {

                // Open dialogue menu
                DialogueManager.OpenMenu(() => whenDialogueBoxAppears(FirstDialogue), AudioSource);
            }
        }
    }
}