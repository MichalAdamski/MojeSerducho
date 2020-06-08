using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BackButtonManager : MonoBehaviour
{
    [field: SerializeField]
    public RectTransform ActualView { get; set; }
    [field: SerializeField]
    public RectTransform PreviousView { get; set; }

    [field: SerializeField]
    public UnityEvent OnBackEvent { get; set; }
    public void OnBackAction()
    {
        ActualView.gameObject.SetActive(false);
        PreviousView.gameObject.SetActive(true);
        OnBackEvent?.Invoke();
    }
}
