using System;
using System.Collections;
using MyHeart.VoiceRecognition;
using UnityEngine;

namespace MyHeart
{
    public class CommandBehaviour : MonoBehaviour
    {
        [Header("Command")]

        [SerializeField] private VoiceCommand command;
        [Header("or set id")]
        [SerializeField] private string commandId;

        public Action OnRecognized { get; set; }

        public string CommandId
        {
            get => commandId;
            set => commandId = value;
        }

        public VoiceCommand Command
        {
            get => command;
            set => command = value;
        }

        protected virtual void OnEnable()
        {
            StartCoroutine(RegisterCommands());
        }

        private IEnumerator RegisterCommands()
        {
            yield return new WaitForEndOfFrame();
            var c = command == null ? VoiceCommandManager.Commands[commandId] : VoiceCommandManager.Commands[command.Id];
            c.EnableCommand();
            c.CommandAction = OnRecognized;
        }

        protected virtual void OnDisable()
        {
            if (VoiceCommandManager.Commands == null) return;
            var c = command == null
                ? VoiceCommandManager.Commands[commandId]
                : VoiceCommandManager.Commands[command.Id];
            c.DisableCommand();
        }
    }
}
