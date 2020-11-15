using UnityEngine;

namespace UI
{
    public class UiSwitcher : MonoBehaviour
    {
        public UiStartMenu  StartMenu;
        public UiFinishMenu FinishMenu;

        public void SetStartMode()
        {
            StartMenu.Show();
            FinishMenu.Hide();
        }

        public void SetGameMode()
        {
            StartMenu.Hide();
            FinishMenu.Hide();
        }

        public void SetFinishMode()
        {
            StartMenu.Hide();
            FinishMenu.Show();
        }
    }
}

