using System;
using System.Collections;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace MyHeart
{
    [Serializable]
    public class UnityTaskEvent : UnityEvent<Task.TaskList>
    {
    }

    public class ApiConnectionManager : MonoBehaviour
    {
        [SerializeField] private string apiUrl = "https://7eylhg26k2.execute-api.us-east-1.amazonaws.com/prod";
        [SerializeField] private string apiGetTask = "/task/{0}";
        [SerializeField] private string apiLogin = "/login";
        [SerializeField] private string apiEndTask = "/task/management/{0}/isdone";
        [SerializeField] public event Action<Task.TaskList> onResponse;
        [SerializeField] public event Action onTaskDone; 

        private static UserInfo userInfo;

        public string ApiUrl
        {
            get => apiUrl;
            set => apiUrl = value;
        }

        public string ApiGetTask
        {
            get => apiGetTask;
            set => apiGetTask = value;
        }

        public string ApiLogin
        {
            get => apiLogin;
            set => apiLogin = value;
        }

        public void GetTasks()
        {
            GetTasksAsync(userInfo.UserId, userInfo.Token);
        }

        public IEnumerator GetTaskRoutine()
        {
            yield return GetTasks(userInfo.UserId, userInfo.Token);
        }

        private void GetTasksAsync(string userId, string token)
        {
            StartCoroutine(GetTasks(userId, token));
        }

        private IEnumerator GetTasks(string userId, string token)
        {
            Debug.Log("Getting tasks...");
            using (var webRequest = GetTaskRequest(userId, token))
            {
                yield return webRequest.SendWebRequest();
                if (webRequest.error == null)
                {
                    Debug.Log("Getting tasks - no error");
                    Debug.Log(webRequest.responseCode + " : " + webRequest.downloadHandler.text);
                    onResponse?.Invoke(Task.TaskList.FromJson(webRequest.downloadHandler.text));
                }
                else
                    Debug.Log(webRequest.responseCode + " : " + webRequest.downloadHandler.text);
            }
            Debug.Log("Getting tasks ended");
        }

        private UnityWebRequest GetTaskRequest(string userId, string token)
        {

            var webRequest = new UnityWebRequest(GetTaskUri(userId), "GET")
            {
                downloadHandler = new DownloadHandlerBuffer()
            };

            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Authentication", token);

            return webRequest;
        }

        private string GetTaskUri(string userId)
        {
            return apiUrl + string.Format(apiGetTask, userId);
        }

        public bool LoginSync(UserLoginData userData)
        {
            return Login(userData);
        }

        private bool Login(UserLoginData userData)
        {
            using (var webRequest = GetLoginRequest(userData))
            {
                webRequest.SendWebRequest();
                while (!webRequest.isDone) { }
                Debug.Log(webRequest.responseCode + " : " + webRequest.downloadHandler.text);

                if (webRequest.error != null) return false;
                if (webRequest.responseCode != 200) return false;

                userInfo = UserInfo.FromJson(webRequest.downloadHandler.text);
                return true;
            }
        }

        private UnityWebRequest GetLoginRequest(UserLoginData userData)
        {
            var postData = userData.ToJson();
            var bytePostData = Encoding.UTF8.GetBytes(postData);

            Debug.Log(postData);

            var webRequest = new UnityWebRequest(apiUrl + apiLogin, "POST")
            {
                uploadHandler = new UploadHandlerRaw(bytePostData),
                downloadHandler = new DownloadHandlerBuffer()
            };

            webRequest.SetRequestHeader("Content-Type", "application/json");

            return webRequest;
        }

        public void EndTaskAsync(Task task)
        {
            StartCoroutine(EndTask(task, userInfo.Token));
        }

        private IEnumerator EndTask(Task task, string token)
        {
            using (var webRequest = GetTaskEndRequest(task.TaskId, token))
            {
                Debug.Log(webRequest.uri);
                yield return webRequest.SendWebRequest();
                if (webRequest.error == null)
                {
                    Debug.Log("End tasks - no error");
                    Debug.Log(webRequest.responseCode + " : " + webRequest.downloadHandler.text);
                    onTaskDone?.Invoke();
                }
                else
                    Debug.Log(webRequest.responseCode + " : " + webRequest.downloadHandler.text);
            }
        }

        private UnityWebRequest GetTaskEndRequest(string id, string token)
        {
            var webRequest = new UnityWebRequest(GetTaskEndUri(id), "GET")
            {
                downloadHandler = new DownloadHandlerBuffer()
            };

            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Authentication", token);
            Debug.Log(token);
            return webRequest;
        }

        private string GetTaskEndUri(string taskId)
        {
            return apiUrl + string.Format(apiEndTask, taskId);
        }

        public class UserInfo
        {
            [JsonProperty("id_token", NullValueHandling = NullValueHandling.Ignore)]
            public string Token;
            [JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
            public string UserId;

            public static UserInfo FromJson(string json)
            {
                return JsonConvert.DeserializeObject<UserInfo>(json, UserLoginData.Converter.Settings);
            }
        }
    }
}
