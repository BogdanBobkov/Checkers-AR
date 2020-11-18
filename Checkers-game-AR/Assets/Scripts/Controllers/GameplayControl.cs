using UnityEngine;
using System;
using Vuforia;
using Internals;
using Enums;
using System.Collections.Generic;

namespace Controllers
{
    public class GameplayControl : MonoBehaviour
    {
        [SerializeField] private Transform[] _Points;
        [SerializeField] private GameObject  _Chessboard;
        [SerializeField] private Checker[]   _Checkers;

        private Dictionary<string, ColorChecker> _Players             = new Dictionary<string, ColorChecker>();
        private ColorChecker                     _CurrentMovingPlayer = ColorChecker.Blue;
        private bool                             _IsGame              = false;
        private Checker                          _CurrentCheckerForMoving;

        public event Action onStartGame;

        private void Start()
        {
            Locator.DefaultTrackableEventHandler.onChangedStatus += ChangedStatus;
        }

        private void Update()
        {
            if (_IsGame)
            {
                /* Отслеживание нажатия на экран */
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        foreach (var checker in _Checkers)
                        {
                            if (hit.transform == checker.transform && checker.Color == _CurrentMovingPlayer)
                            {
                                Debug.Log($"[GameplayControl] [Update] You touched on your checker!");
                                _CurrentCheckerForMoving = checker;
                            }
                        }

                        if (_CurrentCheckerForMoving != null)
                        {
                            foreach (var point in _Points)
                            {
                                if (hit.transform == point.transform && point.childCount == 0)
                                {
                                    Debug.Log($"[GameplayControl] [Update] You touched on point for your checker!");
                                    _CurrentCheckerForMoving.transform.SetParent(point);
                                    _CurrentCheckerForMoving.transform.localPosition = Vector3.zero;
                                    _CurrentCheckerForMoving = null;
                                    _CurrentMovingPlayer = (_CurrentMovingPlayer == ColorChecker.Blue ? ColorChecker.Red : ColorChecker.Blue);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void SetPlayers(string Player1, string Player2)
        {
            _Players.Add(Player1, ColorChecker.Blue);
            _Players.Add(Player2, ColorChecker.Red);
        }

        public void StartGame()
        {
            Debug.Log("[GameplayControl] [StartGame] Start Game");
            _IsGame = true;
            onStartGame?.Invoke();
        }

        private void ChangedStatus(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
        {
            switch (newStatus)
            {
                case TrackableBehaviour.Status.TRACKED:
                    _Chessboard.SetActive(true);
                    break;
                case TrackableBehaviour.Status.NO_POSE:
                    _Chessboard.SetActive(false);
                    break;
            }
        }

        public void OnLose(string PlayerWon)
        {
            Debug.Log("[GameplayControl] [OnLose] Lose");
            _IsGame = false;
        }
    }
}
