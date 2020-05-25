using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginBehaviourManager : MonoBehaviour
{
    public void OnSuccess()
    {
        SceneManager.LoadScene("MainScene");
    }
}
