using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListDropdownController : MonoBehaviour
{
    [SerializeField] private GameObject listAll;
    [SerializeField] private GameObject listTodo;
    [SerializeField] private GameObject listTodoNow;

    public GameObject ListAll
    {
        get => listAll;
        set => listAll = value;
    }

    public GameObject ListTodo
    {
        get => listTodo;
        set => listTodo = value;
    }

    public GameObject ListTodoNow
    {
        get => listTodoNow;
        set => listTodoNow = value;
    }

    public void OnValueChanged(int value)
    {
        switch (value)
        {
            case 0:
                listAll.SetActive(true); listTodo.SetActive(false); listTodoNow.SetActive(false);
                break;
            case 1:
                listAll.SetActive(false); listTodo.SetActive(true); listTodoNow.SetActive(false);
                break;
            case 2:
                listAll.SetActive(false); listTodo.SetActive(false); listTodoNow.SetActive(true);
                break;
        }
    }
}
