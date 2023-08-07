using Dialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

#nullable enable

public class NpcWithNoMemory : MonoBehaviour
{
    public AudioSource audioSource;

    // Script for NPCs that, when right-clicked, go through the same dialogue tree every time
    // Fully customizable through Unity's Inspector, no code needed

    // When NPC is right-clicked:
    public void OnClick(BaseEventData baseEvent)
    {
        PointerEventData pointerEventData = (PointerEventData)baseEvent;

        if (pointerEventData.button == PointerEventData.InputButton.Right)
        {

            // Open dialogue menu
            DialogueManager.OpenMenu(() => (
                "E cat", "E dog",
                new List<ResponseChoice> {
                    new ResponseChoice("E elep<b>hantEFEWFUEWFUHEWFHE</b>WFHYUEWFYYEFGHEWDHEDHYUEDYGEYFUFHEWDYUREHFDUYGEWFDHYGHYFRGYFHEYFGEWYFGYEWGFYUEWGYGEWFYUF", () =>
                    (
                        "E tapir", "E tapeworm", new List<ResponseChoice>()
                    )),
                    new ResponseChoice("Meta <i>meow</i> meow", null)
                }),
                audioSource);
        }
    }
}
