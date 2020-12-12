using UnityEngine;
using System;
using Vuforia;
using Internals;
using Enums;
using Configs;
using System.Collections.Generic;

namespace Controllers
{
    public class GameplayControl : MonoBehaviour
    {
        [Serializable]
        class Point
        {
            public BoardPosition position;
            public Transform     transform;
            public Checker       checker;
        }

        [SerializeField] private GameObject _Chessboard;
        [SerializeField] private Point[]    _Points;

        private Player[] _Players = new Player[2];
        private ColorChecker                     _CurrentMovingPlayer = ColorChecker.Blue;
        private bool                             _IsGame              = false;
        private bool _IsMoveAgain;
        private Point                          _PointFromMove;

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
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        CheckTouchPosition(hit);
                    }
                }
            }
        }

        public void SetPlayers(string Player1, string Player2)
        {
            _Players[0] = new Player(Player1, ColorChecker.Blue);
            _Players[1] = new Player(Player2, ColorChecker.Red);
        }

        public void StartGame()
        {
            Debug.Log("[GameplayControl] [StartGame] Start Game");
            onStartGame?.Invoke();
            _IsGame = true;

            foreach(var point in _Points)
            {
                if (point.checker != null)
                {
                    point.checker.IsDead = false;
                    point.checker.gameObject.SetActive(true);
                }
            }
        }

        private void CheckTouchPosition(RaycastHit hit)
        {
            foreach (var point in _Points)
            {
                if (hit.transform == point.transform)
                {
                    if (point.checker != null && !_IsMoveAgain && !point.checker.IsDead && point.checker.Color == _CurrentMovingPlayer)
                    {
                        Debug.Log($"[GameplayControl] [CheckTouchPosition] You touched on your checker!");
                        _PointFromMove = point;
                        return;
                    }

                    if (_PointFromMove != null)
                    {
                        if (hit.transform == point.transform && point.checker == null || 
                            hit.transform == point.transform && point.checker.IsDead)
                        {
                            var distanceBetweenPoints = (int)point.position - (int)_PointFromMove.position;
                            if (distanceBetweenPoints == (int)_CurrentMovingPlayer * 9 ||
                               distanceBetweenPoints == (int)_CurrentMovingPlayer * 11)
                            {
                                MoveTo(point);
                                _CurrentMovingPlayer = (_CurrentMovingPlayer == ColorChecker.Blue ? ColorChecker.Red : ColorChecker.Blue);
                            }
                            else 
                            if (Math.Abs(distanceBetweenPoints) == 18 ||
                                Math.Abs(distanceBetweenPoints) == 22)
                            {
                                foreach (var element in _Points)
                                {
                                    if ((int)element.position == (int)_PointFromMove.position + distanceBetweenPoints / 2)
                                    {
                                        element.checker.IsDead = true;
                                        foreach (var player in _Players)
                                            if (player.Color != _CurrentMovingPlayer)
                                            {
                                                player.NumOfChackers--;
                                                if(player.NumOfChackers == 0)
                                                {
                                                    Locator.UiSwitcher.FinishMenu.Show();
                                                    _IsGame = false;
                                                }
                                            }

                                        MoveTo(point);

                                        foreach (var p in _Points)
                                        {
                                            if (p.checker != null && p.checker.Color != _CurrentMovingPlayer)
                                            {
                                                if (p.position == point.position + 9 && !p.checker.IsDead ||
                                                    p.position == point.position - 9 && !p.checker.IsDead ||
                                                    p.position == point.position + 11 && !p.checker.IsDead ||
                                                    p.position == point.position - 11 && !p.checker.IsDead)
                                                {
                                                    _PointFromMove = point;
                                                    _IsMoveAgain = true;
                                                    return;
                                                }
                                            }
                                        }
                                        _IsMoveAgain = false;
                                        _CurrentMovingPlayer = (_CurrentMovingPlayer == ColorChecker.Blue ? ColorChecker.Red : ColorChecker.Blue);
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    return;
                }
            }
        }

        private void MoveTo(Point pointTo)
        {
            Debug.Log($"[GameplayControl] [CheckTouchPosition] You touched on point for your checker!");
            pointTo.checker = _PointFromMove.checker;
            _PointFromMove.checker.transform.SetParent(pointTo.transform);
            _PointFromMove.checker.transform.localPosition = Vector3.zero;
            _PointFromMove.checker = null;
            _PointFromMove = null;
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

        public void OnFinish(string PlayerWon)
        {
            Debug.Log("[GameplayControl] [OnLose] Lose");
            _IsGame = false;
            Locator.UiSwitcher.FinishMenu.Show();
        }
    }
}
