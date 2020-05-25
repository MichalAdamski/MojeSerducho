using System.Collections.Generic;
using UnityEngine;

namespace MyHeart
{
    public class TaskBarCreator : MonoBehaviour
    {
        [SerializeField] private TaskBar taskBar;
        [SerializeField] private RectTransform taskBarsParentAll;
        [SerializeField] private RectTransform taskBarsParentTodo;
        [SerializeField] private RectTransform taskBarsParentTodoNow;
        [SerializeField] private TaskManager taskManager;

        private List<TaskBar> taskBarsAll = new List<TaskBar>();
        private List<TaskBar> taskBarsTodo = new List<TaskBar>();
        private List<TaskBar> taskBarsTodoNow = new List<TaskBar>();

        private void Awake()
        {
            taskManager.OnRefresh = CreateTasks;
        }

        public TaskBar Bar
        {
            get => taskBar;
            set => taskBar = value;
        }

        public RectTransform TaskBarsParent
        {
            get => taskBarsParentAll;
            set => taskBarsParentAll = value;
        }

        public RectTransform TaskBarsParentTodo
        {
            get => taskBarsParentTodo;
            set => taskBarsParentTodo = value;
        }

        public RectTransform TaskBarsParentTodoNow
        {
            get => taskBarsParentTodoNow;
            set => taskBarsParentTodoNow = value;
        }

        public TaskManager Manager
        {
            get => taskManager;
            set => taskManager = value;
        }

        public void CreateTasks(Task.TaskList taskList)
        {
            var tasks = taskList.Tasks;
            CreateTasks(tasks);
        }

        public void CreateTasks(List<Task> taskList)
        {
            ClearTasks();
            foreach (var task in taskList)
            {
                var newTaskBar = Instantiate(taskBar, taskBarsParentAll);
                newTaskBar.SetTaskBar(task);
                taskBarsAll.Add(newTaskBar);
                if (task.IsDone == 0)
                {
                    newTaskBar = Instantiate(taskBar, taskBarsParentTodo);
                    newTaskBar.SetTaskBar(task);
                    taskBarsTodo.Add(newTaskBar);
                    if (task.ToDo)
                    {
                        newTaskBar = Instantiate(taskBar, taskBarsParentTodoNow);
                        newTaskBar.SetTaskBar(task);
                        taskBarsTodoNow.Add(newTaskBar);
                    }
                }
            }
        }

        private void ClearTasks()
        {
            Debug.Log("Clear");
            foreach (var task in taskBarsAll)
            {
                Destroy(task.gameObject);
            }
            foreach (var task in taskBarsTodo)
            {
                Destroy(task.gameObject);
            }
            foreach (var task in taskBarsTodoNow)
            {
                Destroy(task.gameObject);
            }

            taskBarsAll.Clear();
            taskBarsTodo.Clear();
            taskBarsTodoNow.Clear();
        }
    }
}

