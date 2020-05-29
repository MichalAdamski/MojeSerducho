using System.Collections;
using UnityEngine;

namespace MyHeart
{
    public class RecognitionEngineManager : MonoBehaviour
    {
        [Header("Set this up if you are using native android voice commands recognition")]
        [SerializeField] private VoiceController bridge;

        public VoiceController Bridge
        {
            get => bridge;
            set => bridge = value;
        }


        public ICommandRecognitionEngine CreateEngine(RecognitionEngine recognitionEngine)
        {
            ICommandRecognitionEngine engine;
            switch (recognitionEngine)
            {
                case RecognitionEngine.Native:
                    engine = new NativeAndroidCommandRecognitionEngine();
                    bridge.gameObject.SetActive(true);
                    break;
                default:
                    engine = new NativeAndroidCommandRecognitionEngine();
                    break;
            }
            engine.InitializeEngine();
            return engine;
        }
    }

    public enum RecognitionEngine
    {
        Rw,
        Native
    }
}

