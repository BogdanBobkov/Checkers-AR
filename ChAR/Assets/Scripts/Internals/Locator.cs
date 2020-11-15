using UnityEngine;
using Controllers;

namespace Internals
{
    public class Locator : MonoBehaviour
    {
        private static Locator _instance;

        [SerializeField] private GameplayControl _gameplayControl;
        public static GameplayControl GameplayControl => _instance._gameplayControl;

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
