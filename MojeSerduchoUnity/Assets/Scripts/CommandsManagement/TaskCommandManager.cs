using UnityEngine;

namespace MyHeart
{
    public class TaskCommandManager : CommandBehaviour
    {
        [SerializeField] private SideBarBtnManager sideBarBtnManager;

        public SideBarBtnManager BarBtnManager
        {
            get => sideBarBtnManager;
            set => sideBarBtnManager = value;
        }

        protected override void OnEnable()
        {
            CommandId = "Tasks";
            OnRecognized = sideBarBtnManager.OnTaskButtonClick;
            base.OnEnable();
        }

        
    }
}

