using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using MyHeart;
using MyHeart.VoiceRecognition;
using UnityEngine;

namespace MyHeart
{
    public class VoiceCommandManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private CommandLanguage language;
        [SerializeField] private RecognitionEngine recognitionEngine;
        [Space]

        [Header("Commands lists")]
        [SerializeField, NotNull] private VoiceCommandsList englishCommands;
        [SerializeField, NotNull] private VoiceCommandsList polishCommands;
        [Space]

        [Header("Engine manager")]
        [SerializeField, NotNull] private RecognitionEngineManager engineManager;

        public static Dictionary<string, VoiceCommand> Commands { get; set; }
        private ICommandRecognitionEngine engine;

        public static VoiceCommandManager ManagerInstance;

        public VoiceCommandsList EnglishCommands
        {
            get => englishCommands;
            set => englishCommands = value;
        }

        public VoiceCommandsList PolishCommands
        {
            get => polishCommands;
            set => polishCommands = value;
        }

        public RecognitionEngine RecognitionEngine
        {
            get => recognitionEngine;
            set => recognitionEngine = value;
        }

        public CommandLanguage Language
        {
            get => language;
            set => language = value;
        }

        public RecognitionEngineManager EngineManager
        {
            get => engineManager;
            set => engineManager = value;
        }

        private void Awake()
        {
            ManagerInstance = this;
            Commands = Commands ?? new Dictionary<string, VoiceCommand>();
            Commands.Clear();
            SetUpEngine(recognitionEngine);
            SetUpCommands(language);
        }

        private void SetUpCommands(CommandLanguage l)
        {
            switch (l)
            {
                case CommandLanguage.En:
                    SetUpCommands(englishCommands);
                    break;
                case CommandLanguage.Pl:
                    SetUpCommands(polishCommands);
                    break;
                default:
                    SetUpCommands(englishCommands);
                    break;
            }
        }

        private void SetUpCommands(VoiceCommandsList list)
        {
            foreach (var command in list.listOfCommands)
            {
                Commands.Add(command.Id, command);
                command.OnDisableAction = DisableCommand;
                command.OnEnableAction = EnableCommand;
                command.DisableCommand();
            }
        }

        private void SetUpEngine(RecognitionEngine rEngine)
        {
            engine = EngineManager.CreateEngine(rEngine);
        }

        private void DisableCommand(VoiceCommand command)
        {
            engine.DisableCommand(command);
        }

        private void EnableCommand(VoiceCommand command)
        {
            engine.EnableCommand(command);
        }

        public VoiceCommand AddCustomCommand(string id, string commandName, bool isActive = true, Action onCommandAction = default)
        {
            if (Commands.ContainsKey(id))
                return null;

            if (onCommandAction == default)
                onCommandAction = () => { };

            var command = new VoiceCommand
            {
                OnEnableAction = EnableCommand,
                OnDisableAction = DisableCommand,
                Id = id,
                CommandName = commandName,
                CommandAction = onCommandAction
            };

            Commands.Add(id, command);

            if (isActive)
            {
                command.EnableCommand();
            }

            return command;
        }
    }

    public enum CommandLanguage
    {
        En,
        Pl
    }
}

