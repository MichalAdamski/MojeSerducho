using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyHeart
{
    public class TaskBar : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Toggle isDoneToggle;
        public event Action<Task> onClickEvent;
        public Task Task { get; set; }

        private string taskName;
        private bool isDone;

        public TextMeshProUGUI NameText
        {
            get => nameText;
            set => nameText = value;
        }

        public Toggle IsDoneToggle
        {
            get => isDoneToggle;
            set => isDoneToggle = value;
        }

        public void SetTaskBar(Task task)
        {
            Task = task;
            taskName = task.Name;
            isDone = Convert.ToBoolean(task.IsDone);
            nameText.text = taskName;
            isDoneToggle.isOn = isDone;
            GetComponent<Button>().onClick.AddListener(()=>{onClickEvent?.Invoke(Task);});
        }
    }
}
