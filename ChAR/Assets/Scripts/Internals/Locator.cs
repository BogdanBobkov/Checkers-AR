using UnityEngine;
using Controllers;
using UI;

namespace Internals
{
    public class Locator : MonoBehaviour
    {
        private static Locator _instance;

        [SerializeField] private GameplayControl _gameplayControl;
        public static GameplayControl GameplayControl => _instance._gameplayControl;

        [SerializeField] private UiSwitcher _uiSwitcher;
        public static UiSwitcher UiSwitcher => _instance._uiSwitcher;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(this);
                return;
            }
            _instance = this;
        }
    }
}
