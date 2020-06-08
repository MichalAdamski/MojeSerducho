using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyHeart
{
    public class TaskUiCreator : MonoBehaviour
    {
        [Header("Tasks")]
        [SerializeField] private TaskBar taskBar;
        [SerializeField] private TaskBar taskBarDone;
        [SerializeField] private TaskBar taskBarNotToDo;

        [SerializeField] private RectTransform taskBarsParentAll;
        [SerializeField] private RectTransform taskBarsParentTodo;
        [SerializeField] private RectTransform taskBarsParentTodoNow;
        [SerializeField] private TaskManager taskManager;
        [SerializeField] private TaskCompletionView taskCompletionView;
        [SerializeField] private GameObject taskList;

        [Header("Heart")] [Space] 
        [SerializeField] private Image heartImage;

        [SerializeField] private Sprite happyHeartSprite;
        [SerializeField] private Sprite sadHeartSprite;
        [SerializeField] private Image heartPointerImage;
        private const int maxPointerPos = 1830;
        private const int minPointerPos = 10;

        private List<TaskBar> taskBarsAll = new List<TaskBar>();
        private List<TaskBar> taskBarsTodo = new List<TaskBar>();
        private List<TaskBar> taskBarsTodoNow = new List<TaskBar>();

        private void Awake()
        {
            taskManager.OnRefresh = CreateTasks;
            taskManager.OnTasksCreated += UpdateUi;
            taskCompletionView.endTaskEvent += taskManager.EndTask;
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

        public TaskCompletionView CompletionView
        {
            get => taskCompletionView;
            set => taskCompletionView = value;
        }

        public GameObject TaskList
        {
            get => taskList;
            set => taskList = value;
        }

        public Image HeartImage
        {
            get => heartImage;
            set => heartImage = value;
        }

        public Sprite HappyHeartSprite
        {
            get => happyHeartSprite;
            set => happyHeartSprite = value;
        }

        public Sprite SadHeartSprite
        {
            get => sadHeartSprite;
            set => sadHeartSprite = value;
        }

        public Image HeartPointerImage
        {
            get => heartPointerImage;
            set => heartPointerImage = value;
        }

        public TaskBar TaskBarDone
        {
            get => taskBarDone;
            set => taskBarDone = value;
        }

        public TaskBar TaskBarNotToDo
        {
            get => taskBarNotToDo;
            set => taskBarNotToDo = value;
        }

        private void UpdateUi(List<Task> tasks, float doneRatio)
        {
            CreateTasks(tasks);
            UpdateHeart(doneRatio);
        }

        private void UpdateHeart(float doneRatio)
        {
            heartImage.sprite = doneRatio < 0.75f ? sadHeartSprite : happyHeartSprite;
            var pointerTrans = heartPointerImage.GetComponent<RectTransform>();
            var yPointerPos = ((maxPointerPos - minPointerPos) * doneRatio) + minPointerPos;
            pointerTrans.anchoredPosition = new Vector2(pointerTrans.anchoredPosition.x, yPointerPos);
        }

        private void CreateTasks(Task.TaskList taskList)
        {
            var tasks = taskList.Tasks;
            CreateTasks(tasks);
        }

        private void CreateTasks(List<Task> taskList)
        {
            ClearTasks();
            foreach (var task in taskList)
            {
                CreateTask(task);
            }
        }

        private void CreateTask(Task task)
        {
            TaskBar taskPrefab;
            if (task.IsDone == 1)
            {
                taskPrefab = taskBarDone;
            }
            else if (!task.ToDo)
            {
                taskPrefab = taskBarNotToDo;
            }
            else
            {
                taskPrefab = taskBar;
            }

            var newTaskBar = Instantiate(taskPrefab, taskBarsParentAll);
            newTaskBar.SetTaskBar(task);
            newTaskBar.onClickEvent += (t) =>
            {
                taskList.SetActive(false);
                taskCompletionView.SetView(t);
                taskCompletionView.gameObject.SetActive(true);
            };
            taskBarsAll.Add(newTaskBar);
            if (task.IsDone == 0)
            {
                newTaskBar = Instantiate(taskPrefab, taskBarsParentTodo);
                newTaskBar.SetTaskBar(task);
                newTaskBar.onClickEvent += (t) =>
                {
                    taskList.SetActive(false);
                    taskCompletionView.SetView(t);
                    taskCompletionView.gameObject.SetActive(true);
                };
                taskBarsTodo.Add(newTaskBar);
                if (task.ToDo)
                {
                    newTaskBar = Instantiate(taskPrefab, taskBarsParentTodoNow);
                    newTaskBar.SetTaskBar(task);
                    newTaskBar.onClickEvent += (t) =>
                    {
                        taskList.SetActive(false);
                        taskCompletionView.SetView(t);
                        taskCompletionView.gameObject.SetActive(true);
                    };
                    taskBarsTodoNow.Add(newTaskBar);
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

