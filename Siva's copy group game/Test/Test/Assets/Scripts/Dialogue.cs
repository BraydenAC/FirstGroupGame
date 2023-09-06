using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Localization;
using System;
using System.Collections.Generic;

public class Dialogue: MonoBehaviour
{
    static VisualElement root;
    static Dialogue instance;

    public LocalizedString test;
    public LocalizedAudioClip clip;
    public AudioSource PlayerAudioSource;
    public bool PLAY_AUDIO_ON_HOVER;

    private static List<DialogueButton> ResponseChoices = new List<DialogueButton>();
    private static int ResponseHoveredIndex;

    static void HoverResponse(int index)
    {
        ResponseHoveredIndex = index;
        ResponseChoices[index].OnHover();
    }

    private void OnEnable()
    {
        instance = this;

        var uiDocument = GetComponent<UIDocument>();

        root = uiDocument.rootVisualElement;
        AddPlayerResponse(test, clip, () => AddNpcDialogue(test));
        AddPlayerResponse(test, clip, () => AddNpcDialogue(test));
    }

    public static void AddNpcDialogue(LocalizedString text)
    {
        // Opens dialogue UI, clears existing player response choices, and sets NPC dialogue

        // Open menu
        instance.gameObject.SetActive(true);

        // Set text
        Label npcText = root.Q("npc-dialogue") as Label;
        npcText.text = text.GetLocalizedString();

        // Delete response choice buttons
        root.Query(classes:"response-choice").ForEach(button => root.Remove(button));
        ResponseChoices.Clear();
    }

    public static void AddPlayerResponse(LocalizedString text, LocalizedAudioClip audio, Action whenChosen)
    {
        root.Add(new DialogueButton(text, audio, whenChosen));
    }

    private class DialogueButton : Button
    {
        private int index;
        private LocalizedString localizedText;
        public LocalizedAudioClip localizedAudio;
        private Action whenChosen;

        public DialogueButton(LocalizedString text, LocalizedAudioClip audio, Action whenChosen)
        {
            this.index = ResponseChoices.Count;
            ResponseChoices.Add(this);
            localizedText = text;
            localizedAudio = audio;
            this.clicked += whenChosen;

            this.AddToClassList("response-choice");

            this.RegisterCallback<MouseOverEvent>(i => HoverResponse(index));
        }

        public void OnHover()
        {
            if (instance.PLAY_AUDIO_ON_HOVER)
            {
                instance.PlayerAudioSource.clip = localizedAudio.LoadAsset();
                instance.PlayerAudioSource.Play();
            }
            Debug.Log(index);
        }
    }

    public static void CloseMenu()
    {
        // Closes dialogue UI, and stops audio from all participants in the conversation

        // Close UI
        if (!instance.gameObject.activeSelf)
        {
            Debug.LogWarning("Attempted to close dialogue UI when it was already closed. Or maybe not; this warning appears at wrong times sometimes");
        }
        instance.gameObject.SetActive(false);

        // Stop audio from all interlocutors
        instance.PlayerAudioSource.Stop();
        // TODO add NPC audio
    }

    // TODO something wrong around here; the inputs don't work like they should
    public static void MoveSelectorUp()
    {
        if (ResponseHoveredIndex > 0)
        {
            HoverResponse(ResponseHoveredIndex - 1);
        }
    }

    public static void MoveSelectorDown()
    {
        if (ResponseHoveredIndex < ResponseHoveredIndex - 1)
        {
            HoverResponse(ResponseHoveredIndex + 1);
        }
    }
}