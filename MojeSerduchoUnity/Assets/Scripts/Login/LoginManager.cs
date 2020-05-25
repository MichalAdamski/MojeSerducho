using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace MyHeart
{
    public class LoginManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField usernameInput;
        [SerializeField] private TMP_InputField passwordInput;

        [SerializeField] private ApiConnectionManager connectionManager;

        [SerializeField] private UnityEvent onSuccess;
        [SerializeField] private UnityEvent onFail;

        public TMP_InputField UsernameInput
        {
            get => usernameInput;
            set => usernameInput = value;
        }

        public TMP_InputField PasswordInput
        {
            get => passwordInput;
            set => passwordInput = value;
        }

        public ApiConnectionManager ConnectionManager
        {
            get => connectionManager;
            set => connectionManager = value;
        }

        public UnityEvent OnSuccess
        {
            get => onSuccess;
            set => onSuccess = value;
        }

        public UnityEvent OnFail
        {
            get => onFail;
            set => onFail = value;
        }

        public void Login()
        {
            var userData = new UserLoginData {Username = usernameInput.text, Password = passwordInput.text};
            var result = connectionManager.LoginSync(userData);
            if(result)
                onSuccess?.Invoke();
            else
                onFail?.Invoke();
        }
    }
}

