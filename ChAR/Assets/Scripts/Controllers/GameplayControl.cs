using UnityEngine;
using System;

namespace Controllers
{
    public class GameplayControl : MonoBehaviour
    {
        [SerializeField] private Transform[] _Points;

        public event Action StartGame;
        private void Start()
        {

        }
    }
}
