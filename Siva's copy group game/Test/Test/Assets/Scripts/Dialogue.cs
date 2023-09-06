using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Localization;
using System;

public class Dialogue: MonoBehaviour
{
    VisualElement root;
    public LocalizedString test;
    public LocalizedAudioClip clip;
    public bool PLAY_AUDIO_ON_HOVER;
    public AudioSource PlayerAudioSource;

    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();

        root = uiDocument.rootVisualElement;
        AddPlayerResponse(test, clip, () => Debug.Log("e"));
    }

    public void AddNpcDialogue(LocalizedString text)
    {
        // Set text
        Label npcText = root.Q("npc-dialogue") as Label;
        npcText.text = text.GetLocalizedString();

        // Delete response choice buttons
        root.Query("response-choice").ForEach(i => i.Remove(i));
    }

    public void AddPlayerResponse(LocalizedString text, LocalizedAudioClip audio, Action whenChosen)
    {
        // TODO does this order work
        // Make button
        Button button = new Button();
        root.Add(button);
        button.text = text.GetLocalizedString();
        button.clicked += whenChosen;
        button.name = "response-choice";

        // When hovered, if PLAY_AUDIO_ON_HOVER is true, play audio
        button.RegisterCallback<MouseOverEvent>(ignored =>
        {
            if (PLAY_AUDIO_ON_HOVER)
            {
                // TODO retrieve audio from localizedasset
                // PlayerAudioSource.clip = audio;
                // PlayerAudioSource.Play();
            }
        });
    }

    private void OnDisable()
    {
        // _button.UnregisterCallback<ClickEvent>(PrintClickMessage);
    }
}