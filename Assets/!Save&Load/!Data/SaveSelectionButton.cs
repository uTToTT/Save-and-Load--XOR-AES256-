using System;
using UnityEngine;
using UnityEngine.UI;

public class SaveSelectionButton : MonoBehaviour
{
    public event Action Click;

    [SerializeField] private Image _icon;
    [SerializeField] private Button _button;
    [Space]
    [SerializeField] private Color _selectedColor = Color.white;
    [SerializeField] private Color _unselectedColor = Color.white;

    private void OnEnable() => _button.onClick.AddListener(OnClick);
    private void OnDisable() => _button.onClick.RemoveListener(OnClick);

    public void Select() => _icon.color = _selectedColor;
    public void Unselect() => _icon.color = _unselectedColor;

    private void OnClick() => Click?.Invoke();
}
