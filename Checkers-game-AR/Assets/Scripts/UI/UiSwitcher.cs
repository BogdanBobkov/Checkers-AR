using UnityEngine;

namespace UI
{
    public class UiSwitcher : MonoBehaviour
    {
        public UiStartMenu  StartMenu;
        public UiFinishMenu FinishMenu;

        private void Awake() => SetStartMode();

        public void SetStartMode()
        {
            StartMenu.Show();
            FinishMenu.Hide();
        }
    }
}

