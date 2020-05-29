using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationUtilities : MonoBehaviour
{
    public static IEnumerator WaitForAnimationEnd(Animation animation)
    {
        do
        {
            yield return null;
            Debug.Log("Waiting");
        } while (animation.isPlaying);

    }
}
