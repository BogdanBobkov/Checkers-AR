using UnityEngine;

namespace UI
{
    public class UiStartMenu : MonoBehaviour
    {
        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}
