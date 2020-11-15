using UnityEngine;
using System;
using TMPro;

namespace Controllers
{
    public class GameplayControl : MonoBehaviour
    {
        [SerializeField] private Transform[] _Points;
        private TMP_InputField tm;

        public string Player1 = "Бобков";
        public string Player2 = "Молдовану";

        public event Action onStartGame;

        public void StartGame()
        {
            onStartGame?.Invoke();
        }
    }
}
