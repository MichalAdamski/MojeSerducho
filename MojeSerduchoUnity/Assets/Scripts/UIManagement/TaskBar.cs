﻿using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyHeart
{
    public class TaskBar : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Toggle isDoneToggle;

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
            taskName = task.Name;
            isDone = Convert.ToBoolean(task.IsDone);

            nameText.text = taskName;
            isDoneToggle.isOn = isDone;
        }
    }
}
