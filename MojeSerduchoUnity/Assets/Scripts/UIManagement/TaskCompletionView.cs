using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyHeart
{
    public class TaskCompletionView : MonoBehaviour
    {
        [SerializeField] private TaskCompletionManager taskCompletionManager;
        [SerializeField] private TextMeshProUGUI counterText;
        [SerializeField] private Button startButton;
        [SerializeField] private Button endButton;
        [SerializeField] private Image timeFiller;

        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI startText;
        [SerializeField] private TextMeshProUGUI endText;
        [SerializeField] private TextMeshProUGUI durationText;

        [SerializeField] private BackButtonManager backButton;


        public event Action<Task> endTaskEvent;
        private Task currentTask;

        private void Awake()
        {
            taskCompletionManager.OnCompletionAction += OnCounterEnd;
            taskCompletionManager.OnCounterAction += SetCounter;
            startButton.onClick.AddListener(StartTask);
            endButton.onClick.AddListener(EndTask);
            endButton.interactable = false;
        }

        public TaskCompletionManager TaskCompletionManager
        {
            get => taskCompletionManager;
            set => taskCompletionManager = value;
        }

        public TextMeshProUGUI CounterText
        {
            get => counterText;
            set => counterText = value;
        }

        public Button StartButton
        {
            get => startButton;
            set => startButton = value;
        }

        public Button EndButton
        {
            get => endButton;
            set => endButton = value;
        }

        public Image TimeFiller
        {
            get => timeFiller;
            set => timeFiller = value;
        }

        public TextMeshProUGUI TitleText
        {
            get => titleText;
            set => titleText = value;
        }

        public TextMeshProUGUI DescriptionText
        {
            get => descriptionText;
            set => descriptionText = value;
        }

        public TextMeshProUGUI StartText
        {
            get => startText;
            set => startText = value;
        }

        public TextMeshProUGUI EndText
        {
            get => endText;
            set => endText = value;
        }

        public TextMeshProUGUI DurationText
        {
            get => durationText;
            set => durationText = value;
        }

        public BackButtonManager BackButton
        {
            get => backButton;
            set => backButton = value;
        }

        private void SetCounter(int time, int fullTime)
        {
            counterText.text = TimeUtilities.SecToText(time);
            timeFiller.fillAmount = 1 - (time / (float)fullTime);
        }

        private void OnCounterEnd()
        {
            endButton.interactable = true;
            counterText.text = "Zakończ zadanie";
            TimeFiller.fillAmount = 0;
        }

        public void EndTask()
        {
            endTaskEvent?.Invoke(currentTask);
            endButton.interactable = false;
            endButton.gameObject.SetActive(false);
            startButton.gameObject.SetActive(true);
            backButton.GetComponent<Button>().interactable = true;
        }

        public void SetView(Task task)
        {
            currentTask = task;
            if (task.ToDo && task.IsDone == 0)
                startButton.interactable = true;
            else
                startButton.interactable = false;

            titleText.text = task.Name ?? "Zadanie";
            descriptionText.text = task.Description ?? "";
            startText.text = task.StartTime != null ? $"Od: {task.StartTime}" : "Dowolnie";
            endText.text = task.EndTime != null ? $"Do: {task.EndTime}" : "Dowolnie";
            durationText.text = $"Potrzebny czas: {TimeUtilities.SecToText(TimeUtilities.MinToSec(task.Duration))}";
        }

        public void StartTask()
        {
            taskCompletionManager.StartTask(currentTask);
            startButton.gameObject.SetActive(false);
            startButton.interactable = false;
            endButton.gameObject.SetActive(true);
            gameObject.SetActive(true);
            backButton.GetComponent<Button>().interactable = false;
        }

        

    }
}

