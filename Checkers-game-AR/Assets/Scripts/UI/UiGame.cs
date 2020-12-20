using System;
using UnityEngine;

public class UiGame : MonoBehaviour
{
    private Action yesAction;
    private Action noAction;
    public void Show(Action yesAction, Action noAction)
    {
        this.yesAction = yesAction;
        this.noAction = noAction;
        gameObject.SetActive(true);
    }
    public void Hide() => gameObject.SetActive(false);

    public void ButtonYes()
    {
        yesAction?.Invoke();
        Hide();
    }

    public void ButtonNo()
    {
        noAction?.Invoke();
        Hide();
    }
}
