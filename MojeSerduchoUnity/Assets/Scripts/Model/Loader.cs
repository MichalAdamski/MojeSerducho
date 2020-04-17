using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public string Text = "{\"name\":\"Nowe zadanie do wykonania\"}";

    void Start()
    {
        var task = Task.FromJson(Text);
        Debug.Log(task.Name);
    }
}
