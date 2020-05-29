using System;
using System.Collections.Generic;
using System.Linq;
using MyHeart.VoiceRecognition;
using UnityEngine;

namespace MyHeart
{
    public interface ICommandRecognitionEngine
    {
        void DisableCommand(VoiceCommand command);
        void EnableCommand(VoiceCommand command);
        void InitializeEngine();
    }

    public class NativeAndroidCommandRecognitionEngine: ICommandRecognitionEngine
    {
        private Dictionary<string, VoiceCommand> activeVoiceCommands;

        public void InitializeEngine()
        {
            activeVoiceCommands = new Dictionary<string, VoiceCommand>();
            VoiceController.OnResultReceived += Search;
        }

        private void Search(string phrase)
        {
            foreach (var command in activeVoiceCommands.Where(command => MatchPhrase(phrase, command.Value.CommandName)))
            {
                command.Value.CommandAction?.Invoke();
            }
        }

        private bool MatchPhrase(string phrase, string command)
        {
            return phrase.ToLower().Contains(command.ToLower());
        }

        public void DisableCommand(VoiceCommand command)
        {
            if (activeVoiceCommands.ContainsKey(command.Id))
                activeVoiceCommands.Remove(command.Id);
        }

        public void EnableCommand(VoiceCommand command)
        {
            if(!activeVoiceCommands.ContainsKey(command.Id))
                activeVoiceCommands.Add(command.Id, command);
        }
    }
}

