using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyHeart
{
    public class TaskCompletionManager : MonoBehaviour
    {
        public Action<int, int> OnCounterAction { get; set; }
        public Action OnCompletionAction { get; set; }

        public void StartTask(Task task)
        {
            if (task.Duration <= 0)
                OnCompletionAction?.Invoke();
            else
                StartCoroutine(PerformTask(task.Duration));
        }

        private IEnumerator PerformTask(float duration)
        {
            var secTime = TimeUtilities.MinToSec(duration);
            var wait = new WaitForSeconds(1);
            for (var i = 0; i < secTime; i++)
            {
                OnCounterAction?.Invoke((secTime - i), secTime);
                yield return wait;
            }
            OnCounterAction?.Invoke(0, secTime);
            OnCompletionAction?.Invoke();
        }

        
    }

    public static class TimeUtilities
    {
        public static int MinToSec(float min)
        {
            var fullMin = (int)min;
            var rest = (int)((min - fullMin) * 60);
            return fullMin + rest;
        }

        public static string SecToText(int sec)
        {
            var min = (int)((float)sec / 60);
            var s = sec - (min * 60);
            var h = (int)((float)min / 60);
            var m = min - (h * 60);
            var hString = h < 10 ? $"0{h}" : $"{h}";
            var mString = m < 10 ? $"0{m}" : $"{m}";
            var sString = s < 10 ? $"0{s}" : $"{s}";

            return $"{hString}:{mString}:{sString}";
        }
    }
}

