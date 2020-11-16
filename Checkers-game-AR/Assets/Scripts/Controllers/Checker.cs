using UnityEngine;
using Internals;
using Enums;

namespace Controllers
{
    public class Checker : MonoBehaviour
    {
        [SerializeField] private Transform _StartPoint;
        [SerializeField] private ColorChecker _Color;

        private void Start()
        {
            Locator.GameplayControl.onStartGame += SetStartPosition;
        }

        private void SetStartPosition()
        {
            transform.SetParent(_StartPoint);
            transform.localPosition = Vector3.zero;
        }
    }
}
