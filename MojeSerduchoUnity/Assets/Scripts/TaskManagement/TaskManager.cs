using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyHeart
{
    public class TaskManager : MonoBehaviour
    {
        [Header("Settings")] [SerializeField] private float timeBeforeNotify;
        [SerializeField] private ApiConnectionManager connectionManager;

        public static List<Task> TaskList { get; private set; }
        public Action<Task> OnNotifyBeforeEnd { get; set; }
        public Action<Task> OnNotifyBeforeStart { get; set; }


        public Action<List<Task>> OnRefresh { get; set; }

        private bool areTasksLoaded = false;

        public ApiConnectionManager ConnectionManager
        {
            get => connectionManager;
            set => connectionManager = value;
        }

        public float TimeBeforeNotify
        {
            get => timeBeforeNotify;
            set => timeBeforeNotify = value;
        }

        private void Start()
        {
            connectionManager.OnResponse.AddListener(AddTasks);
            StartCoroutine(GetTasks());
            OnNotifyBeforeEnd = (task) => Debug.Log("Message before end: " + task.Name);
            OnNotifyBeforeStart = (task) => Debug.Log("Message before start: " + task.Name);
        }

        private void Update()
        {
            if (areTasksLoaded)
                CheckTime();
        }

        private void AddTasks(Task.TaskList tasks)
        {
            TaskList = tasks.Tasks;
            foreach (var task in TaskList)
            {
                task.DoNotifyBeforeEnd = true;
                task.DoNotifyBeforeStart = true;
                task.ToDo = true;
            }
        }

        private IEnumerator GetTasks()
        {
            yield return connectionManager.GetTaskRoutine();
            areTasksLoaded = true;
        }

        private void CheckTime()
        {
            foreach (var task in TaskList)
            {
                if (task.StartTime != null)
                {
                    if (task.StartTime < DateTime.Now.TimeOfDay)
                    {
                        if (task.EndTime != null)
                        {
                            if (DateTime.Now.TimeOfDay < task.EndTime)
                            {
                                if (DateTime.Now.TimeOfDay > (task.EndTime - TimeSpan.FromMinutes(timeBeforeNotify)))
                                    NotifyBeforeEndAndRefresh(task);
                            }
                            else
                            {
                                HideAndRefresh(task);
                            }
                        }
                    }
                    else
                    {
                        if (DateTime.Now.TimeOfDay > (task.StartTime - TimeSpan.FromMinutes(timeBeforeNotify)))
                            NotifyBeforeStartAndRefresh(task);
                        else
                            HideAndRefresh(task);
                    }
                }
                else if (task.EndTime != null)
                {
                    if (DateTime.Now.TimeOfDay < task.EndTime)
                    {
                        if (DateTime.Now.TimeOfDay > (task.EndTime - TimeSpan.FromMinutes(timeBeforeNotify)))
                            NotifyBeforeEndAndRefresh(task);
                    }
                    else
                    {
                        HideAndRefresh(task);
                    }
                }
                else
                {
                    NotifyBeforeEndAndRefresh(task);
                }
            }
        }

        private void NotifyBeforeEndAndRefresh(Task task)
        {
            if (task.DoNotifyBeforeEnd && task.IsDone == 0)
            {
                OnNotifyBeforeEnd?.Invoke(task);
                task.DoNotifyBeforeEnd = false;
            }

            if (task.ToDo) return;
            task.ToDo = true;
            OnRefresh?.Invoke(TaskList);
        }

        private void NotifyBeforeStartAndRefresh(Task task)
        {
            if (task.DoNotifyBeforeStart && task.IsDone == 0)
            {
                OnNotifyBeforeStart?.Invoke(task);
                task.DoNotifyBeforeStart = false;
            }

            if (task.ToDo) return;
            task.ToDo = true;
            OnRefresh?.Invoke(TaskList);
        }

        private void HideAndRefresh(Task task)
        {
            if (!task.ToDo) return;
            task.ToDo = false;
            OnRefresh?.Invoke(TaskList);
        }
    }
}
