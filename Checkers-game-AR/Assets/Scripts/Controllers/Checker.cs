using UnityEngine;
using Internals;
using Enums;

namespace Controllers
{
    public class Checker : MonoBehaviour
    {
        [SerializeField] private ColorChecker _Color;
        public ColorChecker Color => _Color;

        [HideInInspector] public  bool IsDead = false;

        private Transform _startPoint;

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
