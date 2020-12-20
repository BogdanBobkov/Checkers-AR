using UnityEngine;
using Internals;
using Enums;

namespace Controllers
{
    public class Checker : MonoBehaviour
    {
        [SerializeField] private ColorChecker _Color;
        public ColorChecker Color => _Color;
        private bool _isDead;

        private Transform _startPoint;

        public bool IsDead
        {
            set
            {
                _isDead = value;
                gameObject.SetActive(!_isDead);
            }
            get
            {
                return _isDead;
            }
        }

        private void Awake()
        {
            _startPoint = transform.parent;
        }

        private void OnEnable()
        {
            transform.SetParent(_startPoint);
            transform.localPosition = Vector3.zero;
        }
    }
}
