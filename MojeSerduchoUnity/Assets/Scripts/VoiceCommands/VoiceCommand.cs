using System;
using UnityEngine;

namespace MyHeart.VoiceRecognition
{
    [CreateAssetMenu(fileName = "newVoiceCommand", menuName = "Voice Recognition/Voice Command")]
    public class VoiceCommand : ScriptableObject
    {
        public bool IsActive { get; private set; }
        public Action CommandAction { get; set; }
        public Action<VoiceCommand> OnDisableAction { get; set; }
        public Action<VoiceCommand> OnEnableAction { get; set; }

        [SerializeField] private string id;
        [SerializeField] private string commandName;
        

        public string Id
        {
            get => id;
            set => id = value;
        }

        public string CommandName
        {
            get => commandName;
            set => commandName = value;
        }

        public void DisableCommand()
        {
            IsActive = false;
            OnDisableAction.Invoke(this);
        }

        public void EnableCommand()
        {
            IsActive = true;
            OnEnableAction.Invoke(this);
        }
    }
}
