using UnityEngine;
using UnityEngine.UI;

public class ClickerBootstrap : MonoBehaviour
{
    [SerializeField] private Button _clickerButton;
    [SerializeField] private ScoreUI _scoreUI;

    private Clicker _clicker;

    public void Init(IPersistentData persistentData, GameEvent onDataLoadedEvent)
    {
        _clicker = new Clicker(persistentData, onDataLoadedEvent);

        _clickerButton.onClick.AddListener(_clicker.OnClick);
        _clicker.OnScoreChanged += _scoreUI.UpdateDisplay;
    }
}

