using System;
using UnityEngine;
using UnityEngine.Android;

public class VoiceController : MonoBehaviour {

    public static event Action<string> OnResultReceived;

    public delegate void OnErrorReceived(string result);
    public static OnErrorReceived ErrorReceived;

    public delegate void OnMessageReceived(string result);
    public static OnMessageReceived MessageReceived;

    AndroidJavaObject activity;
    AndroidJavaObject plugin;

    private void OnEnable() {
        InitPlugin();
    }

    private void InitPlugin() {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
#endif
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

        activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        
        activity.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                plugin = new AndroidJavaObject(
                "com.nsflow.sphinxplugin.MagicWordBridge");
        }));

        activity.Call("runOnUiThread", new AndroidJavaRunnable(() => {
            plugin.Call("StartPlugion");
        }));
    }

    /// <summary>
    /// gets called via SendMessage from the android plugin
    /// </summary>
    /// <param name="recognizedText">recognizedText.</param>
    public void OnVoiceResult(string recognizedText) {
        Debug.Log(recognizedText);
        OnResultReceived?.Invoke(recognizedText);
        StartListening();
    }

    /// <summary>
    /// gets called via SendMessage from the android plugin
    /// </summary>
    /// <param name="error">Error.</param>
    public void OnErrorResult(string error) {
        Debug.Log(error);
    }

    public void OnMessageResult(string message)
    {
        MessageReceived?.Invoke(message);
    }
    /// <summary>
    /// manual speech recognition starter, disabled - now there is magic word "nsflow" inside plugin which starts recognition process 
    /// </summary>
    public void GetSpeech() {
        // Calls the function from the jar file
        /*activity.Call("runOnUiThread", new AndroidJavaRunnable(() => {
            plugin.Call("StartSpeaking");
        }));*/
        InitPlugin();
    }

    public void StartListening()
    {
        activity.Call("runOnUiThread", new AndroidJavaRunnable(() => {plugin.Call("StartListening");}));
    }
}
