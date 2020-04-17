using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SideBarBtnManager : MonoBehaviour
{
    [field: SerializeField]
    public GameObject SideBarBtn { get; set; }

    [field: SerializeField]
    public GameObject SideBarDisabler { get; set; }

    [field: SerializeField]
    public Animator SideBarAnimator { get; set; }

    [field: SerializeField]
    public RectTransform TaskListView { get; set; }

    public void ShowSideBar()
    {
        SideBarAnimator.SetBool("DoShow", true);
        SideBarBtn.SetActive(false);
        SideBarDisabler.SetActive(true);
    }

    public void HideSideBar()
    {
        SideBarAnimator.SetBool("DoShow", false);
        SideBarBtn.SetActive(true);
        SideBarDisabler.SetActive(false);
    }

    public void OnTaskButtonClick()
    {
        TaskListView.anchoredPosition = new Vector2(0, 0);
        HideSideBar();
    }
}
