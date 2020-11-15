using UnityEngine;
using System;
using TMPro;
using Vuforia;
using Internals;

namespace Controllers
{
    public class GameplayControl : MonoBehaviour
    {
        [SerializeField] private Transform[] _Points;

        public string Player1;
        public string Player2;

        public event Action onStartGame;

        public void Start()
        {
            Locator.DefaultTrackableEventHandler.onChangedStatus += ChangedStatus;
        }

        public void StartGame()
        {
            onStartGame?.Invoke();
        }

        private void ChangedStatus(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
        {

        }
    }
}
