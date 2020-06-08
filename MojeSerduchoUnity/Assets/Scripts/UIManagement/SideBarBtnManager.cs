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
    public RectTransform TaskListView { get; set; }

    [field: SerializeField]
    public GameObject MainWidnow { get; set; }

    [field: SerializeField]
    public Animation SideBarAnimation { get; set; }

    public void ShowSideBar()
    {
        SideBarAnimation.PlayQueued("SideBarIn");
        SideBarBtn.SetActive(false);
        SideBarDisabler.SetActive(true);
    }

    public void HideSideBar()
    {
        SideBarAnimation.PlayQueued("SideBarOut");
        SideBarBtn.SetActive(true);
        SideBarDisabler.SetActive(false);
    }

    private IEnumerator WaitForHideSideBar()
    {
        SideBarAnimation.PlayQueued("SideBarOut");
        SideBarBtn.SetActive(true);
        SideBarDisabler.SetActive(false);
        yield return AnimationUtilities.WaitForAnimationEnd(SideBarAnimation);
    }

    public void OnTaskButtonClick()
    {
        StartCoroutine(OnTaskButtonClickAfterSideBarHides());
    }

    public IEnumerator OnTaskButtonClickAfterSideBarHides()
    {
        yield return WaitForHideSideBar();
        TaskListView.gameObject.SetActive(true);
        MainWidnow.SetActive(false);
    }
}
