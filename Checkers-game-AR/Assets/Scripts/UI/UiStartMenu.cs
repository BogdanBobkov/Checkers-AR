using UnityEngine;
using Internals;
using TMPro;

namespace UI
{
    public class UiStartMenu : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _InputFieldPlayer1;
        [SerializeField] private TMP_InputField _InputFieldPlayer2;
        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);

        public void ClickStartButton()
        {
            bool isStartGame = true;

            if (_InputFieldPlayer1.text == "") { isStartGame = false; _InputFieldPlayer1.GetComponent<Animation>().Play(); }
            if (_InputFieldPlayer2.text == "") { isStartGame = false; _InputFieldPlayer2.GetComponent<Animation>().Play(); }

            if (isStartGame)
            {
                Locator.GameplayControl.SetPlayers(_InputFieldPlayer1.text, _InputFieldPlayer2.text);
                Locator.GameplayControl.StartGame();
                Hide();
            }
        }
    }
}
