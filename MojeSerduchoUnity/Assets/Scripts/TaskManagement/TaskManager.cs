using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyHeart
{
    public class TaskManager : MonoBehaviour
    {
        [Header("Settings")] [SerializeField] private float timeBeforeNotify;
        [SerializeField] private ApiConnectionManager connectionManager;

        public static Dictionary<string, Task> TaskList { get; private set; }
        public Action<Task> OnNotifyBeforeEnd { get; set; }
        public Action<Task> OnNotifyBeforeStart { get; set; }
        public Action<List<Task>, float> OnTasksCreated { get; set; }

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

        private void Awake()
        {
            connectionManager.onResponse += AddTasks;
            connectionManager.onTaskDone += EndTask;
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
            TaskList = TaskList ?? new Dictionary<string, Task>();
            var list = tasks.Tasks;
            foreach (var task in list)
            {
                if (!TaskList.TryGetValue(task.TaskId, out var value))
                {
                    task.DoNotifyBeforeEnd = true;
                    task.DoNotifyBeforeStart = true;
                    task.ToDo = true;
                    TaskList.Add(task.TaskId, task);
                }
                else
                {
                    task.DoNotifyBeforeEnd = value.DoNotifyBeforeEnd;
                    task.DoNotifyBeforeStart = value.DoNotifyBeforeStart;
                    task.ToDo = value.ToDo;
                }

                TaskList[task.TaskId] = task;
            }

            var doneRatio = 0.0f;
            foreach (var task in TaskList.Values.Where(task => task.IsDone == 1))
            {
                doneRatio++;
            }

            doneRatio /= TaskList.Count;
            OnTasksCreated?.Invoke(TaskList.Values.ToList(), doneRatio);
        }

        private void EndTask()
        {
            areTasksLoaded = false;
            StartCoroutine(GetTasks());
        }

        private IEnumerator GetTasks()
        {
            yield return connectionManager.GetTaskRoutine();
            areTasksLoaded = true;
        }

        private void CheckTime()
        {
            foreach (var task in TaskList.Values)
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
            OnRefresh?.Invoke(TaskList.Values.ToList());
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
            OnRefresh?.Invoke(TaskList.Values.ToList());
        }

        private void HideAndRefresh(Task task)
        {
            if (!task.ToDo) return;
            task.ToDo = false;
            OnRefresh?.Invoke(TaskList.Values.ToList());
        }

        public void EndTask(Task task)
        {
            ConnectionManager.EndTaskAsync(task);
        }
    }
}
