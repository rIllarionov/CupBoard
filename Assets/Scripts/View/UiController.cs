using System;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

public class UiController : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _title;

    public Action OnButtonClick;

    private Animator _buttonAnimator;
    private Animator _titleAnimator;

    private static readonly int Show = Animator.StringToHash("show");
    private static readonly int Hide = Animator.StringToHash("hide");

    public void ShowButton()
    {
        _buttonAnimator.SetTrigger(Show);
    }

    public void HideButton()
    {
        _buttonAnimator.SetTrigger(Hide);
        OnButtonClick?.Invoke();
    }

    public void ShowTitle()
    {
        _titleAnimator.SetTrigger(Show);
    }

    private void Awake()
    {
        _buttonAnimator = _button.GetComponent<Animator>();
        _titleAnimator = _title.GetComponent<Animator>();
    }
}