using TMPro;
using UnityEngine;

public class WinnerText : MonoBehaviour
{
    public void Show(string nameWinner)
    {
        gameObject.SetActive(true);
        GetComponent<TextMeshPro>().text = nameWinner;
    }
    public void Hide() => gameObject.SetActive(false);

    private void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
}
