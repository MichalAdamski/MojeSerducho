using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyHeart;
using MyHeart.VoiceRecognition;
using UnityEngine;
using UnityEngine.Events;

namespace MyHeart
{
    [Serializable]
    public class MultiCommand
    {
        [SerializeField] private VoiceCommand command;
        [SerializeField] private string commandId;
        public Action OnRecognized { get; set; }

        public VoiceCommand Command
        {
            get => command;
            set => command = value;
        }

        public string CommandId
        {
            get => commandId;
            set => commandId = value;
        }
    }
    public class MultiCommandBehaviour : MonoBehaviour
    {
        [SerializeField] private List<MultiCommand> multiCommands;

        public List<MultiCommand> Commands
        {
            get => multiCommands ?? (multiCommands = new List<MultiCommand>());
            set => multiCommands = value;
        }

        protected virtual void OnEnable()
        {
            StartCoroutine(RegisterCommands());
        }

        private IEnumerator RegisterCommands()
        {
            yield return new WaitForEndOfFrame();
            foreach (var command in Commands)
            {
                var c = command.Command == null ? VoiceCommandManager.Commands[command.CommandId] : VoiceCommandManager.Commands[command.Command.Id];
                c.EnableCommand();
                c.CommandAction = () => command.OnRecognized?.Invoke();
            }
        }

        protected virtual void OnDisable()
        {
            if (VoiceCommandManager.Commands == null) return;
            foreach (var c in Commands.Select(command =>
                command.Command == null
                    ? VoiceCommandManager.Commands[command.CommandId]
                    : VoiceCommandManager.Commands[command.Command.Id]))
            {
                c.DisableCommand();
            }
        }
    }
}
