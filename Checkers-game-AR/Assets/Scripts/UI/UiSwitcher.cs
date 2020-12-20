using UnityEngine;

namespace UI
{
    public class UiSwitcher : MonoBehaviour
    {
        public UiStartMenu  StartMenu;
        public WinnerText FinishMenu;
        public UiGame       GameMenu;

        private void Awake() => SetStartMode();

        public void SetStartMode()
        {
            GameMenu.Hide();
            StartMenu.Show();
            FinishMenu.Hide();
        }
    }
}

