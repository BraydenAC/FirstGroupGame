using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dialogue
{
    public class DialogueButton : MonoBehaviour
    {
        public int ResponseIndex;

        public void OnClick()
        {
            DialogueManager.ResponseChoiceClicked(ResponseIndex);
        }

        // TODO selector with DOTween
        public void OnHover()
        {
        }

        public void OnUnhover()
        {
        }

        public void OnDestroy()
        {
            Debug.Log("me ded");
        }
    }
}