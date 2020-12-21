using UnityEngine;
using System;
using Vuforia;
using Internals;
using Enums;
using Configs;
using System.Linq;

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

        [SerializeField] private Material   _YellowMaterial;
        [SerializeField] private GameObject _Chessboard;
        [SerializeField] private Point[]    _Points;

        private Player[] _Players = new Player[2];
        private ColorChecker                     _CurrentMovingPlayer = ColorChecker.Blue;
        private bool                             _IsGame              = false;
        private bool                             _IsMoveAgain;
        private Point                            _PointFromMove;
        private Material                         _startMaterialChecker;

        private bool                             _IsRedPlayerComputer = false;

        public event Action onStartGame;

        private void Start()
        {
            Locator.DefaultTrackableEventHandler.onChangedStatus += ChangedStatus;
        }

        private void Update()
        {
            if (_IsGame)
            {
                if (!_IsRedPlayerComputer || _CurrentMovingPlayer == ColorChecker.Blue)
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
                } else if(_CurrentMovingPlayer == ColorChecker.Red)
                {
                    foreach(var pointFrom in _Points)
                    {
                        if(pointFrom.checker != null && pointFrom.checker.Color == ColorChecker.Red)
                        {
                            foreach(var pointTo in _Points)
                            {
                                if (pointTo.checker == null || pointTo.checker.IsDead)
                                {
                                    var distanceBetweenPoints = (int)pointTo.position - (int)pointFrom.position;
                                    if (distanceBetweenPoints == (int)_CurrentMovingPlayer * 9 ||
                                        distanceBetweenPoints == (int)_CurrentMovingPlayer * 11)
                                    {
                                        _PointFromMove = pointFrom;
                                        _PointFromMove.checker.transform.position = pointTo.transform.position;
                                        MoveTo(pointTo);
                                        _CurrentMovingPlayer = (_CurrentMovingPlayer == ColorChecker.Blue ? ColorChecker.Red : ColorChecker.Blue);
                                        return;
                                    }
                                    else
                                    if (Math.Abs(distanceBetweenPoints) == 18 ||
                                        Math.Abs(distanceBetweenPoints) == 22)
                                    {
                                        _PointFromMove = pointFrom;
                                        _startMaterialChecker = _PointFromMove.checker.GetComponent<MeshRenderer>().material;
                                        EatEnemyChecker(distanceBetweenPoints, pointTo, true);
                                        if (_CurrentMovingPlayer != ColorChecker.Red) return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void SetPlayers(string Player1, string Player2)
        {
            _Players[0] = new Player(Player1, ColorChecker.Blue);
            _Players[1] = new Player(Player2, ColorChecker.Red);

            if(Player2 == "КОМПЬЮТЕР") _IsRedPlayerComputer = true;
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
                        if (_PointFromMove != null) _PointFromMove.checker.GetComponent<MeshRenderer>().material = _startMaterialChecker;
                        _PointFromMove = point;
                        var renderer = _PointFromMove.checker.GetComponent<MeshRenderer>();
                        _startMaterialChecker = renderer.material;
                        _PointFromMove.checker.GetComponent<MeshRenderer>().material = _YellowMaterial;
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
                                var renderer = _PointFromMove.checker.GetComponent<MeshRenderer>();
                                _PointFromMove.checker.transform.position = point.transform.position;
                                Locator.UiSwitcher.GameMenu.Show(() =>
                                {
                                    MoveTo(point);
                                    _CurrentMovingPlayer = (_CurrentMovingPlayer == ColorChecker.Blue ? ColorChecker.Red : ColorChecker.Blue);
                                    renderer.material = _startMaterialChecker;
                                }, () => { 
                                    _PointFromMove.checker.transform.localPosition = Vector3.zero;
                                    renderer.material = _startMaterialChecker;
                                });
                            }
                            else 
                            if (Math.Abs(distanceBetweenPoints) == 18 ||
                                Math.Abs(distanceBetweenPoints) == 22)
                            {
                                EatEnemyChecker(distanceBetweenPoints, point);
                            }
                        }
                    }
                    return;
                }
            }
        }

        private void EatEnemyChecker(int distanceBetweenPoints, Point pointTo, bool isYes = false)
        {
            var element = GetPoint((BoardPosition)((int)_PointFromMove.position + distanceBetweenPoints / 2));
            if (element != null && element.checker != null && element.checker.Color != _PointFromMove.checker.Color)
            {
                _PointFromMove.checker.transform.position = pointTo.transform.position;
                _PointFromMove.checker.GetComponent<MeshRenderer>().material = _YellowMaterial;
                Locator.UiSwitcher.GameMenu.Show(() =>
                {
                    element.checker.IsDead = true;
                    foreach (var player in _Players)
                        if (player.Color != _CurrentMovingPlayer)
                        {
                            player.NumOfChackers--;
                            if (player.NumOfChackers == 0)
                            {
                                string name = _Players.FirstOrDefault(t => t.Color == _CurrentMovingPlayer).Name;
                                OnFinish(name);
                                _IsGame = false;
                            }
                        }

                    MoveTo(pointTo);
                    pointTo.checker.GetComponent<MeshRenderer>().material = _startMaterialChecker;

                    foreach (var p in _Points)
                    {
                        if (p.checker != null && p.checker.Color != _CurrentMovingPlayer)
                        {
                            if (p.position == pointTo.position + 9 && !p.checker.IsDead ||
                                p.position == pointTo.position - 9 && !p.checker.IsDead ||
                                p.position == pointTo.position + 11 && !p.checker.IsDead ||
                                p.position == pointTo.position - 11 && !p.checker.IsDead)
                            {
                                var nextPoint = GetPoint((BoardPosition)(2 * (int)p.position - (int)pointTo.position));
                                if (nextPoint != null && nextPoint.checker == null)
                                {
                                    if (!isYes)
                                    {
                                        _PointFromMove = pointTo;
                                        _IsMoveAgain = true;
                                    }
                                    return;
                                }
                            }
                        }
                    }
                    _IsMoveAgain = false;
                    _CurrentMovingPlayer = (_CurrentMovingPlayer == ColorChecker.Blue ? ColorChecker.Red : ColorChecker.Blue);
                }, () =>
                {
                    _PointFromMove.checker.transform.localPosition = Vector3.zero;
                    _PointFromMove.checker.GetComponent<MeshRenderer>().material = _startMaterialChecker;
                });

                if (isYes) Locator.UiSwitcher.GameMenu.ButtonYes();
            }
        }

        private Point GetPoint(BoardPosition boardPosition)
        {
            foreach (var element in _Points) if(element.position == boardPosition) return element;

            return null;
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
            Locator.UiSwitcher.FinishMenu.Show(PlayerWon);
        }
    }
}
