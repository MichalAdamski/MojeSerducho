using System.Collections.Generic;
using MyHeart.VoiceRecognition;
using UnityEngine;

namespace MyHeart
{
    [CreateAssetMenu(fileName = "newVoiceCommandsList", menuName = "Voice Recognition/Voice Commands List")]

    public class VoiceCommandsList : ScriptableObject
    {
        public List<VoiceCommand> listOfCommands;
    }
}

