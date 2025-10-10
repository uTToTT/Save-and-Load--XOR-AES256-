using UnityEngine;
using UnityEngine.UI;

// temp variant
public class GameBootstrap : MonoBehaviour
{
    [SerializeField] private ClickerBootstrap _clickerBootstrap;
    [SerializeField] private Button _saveButton;
    [SerializeField] private SaveManager _saveManager;

    private void Awake() => Init();

    private void Init()
    {
        _saveManager.Init();

        _clickerBootstrap.Init(_saveManager.PersistentData, _saveManager.OnDataLoadedEvent);
    }

    [ContextMenu(nameof(ClearPlayerPrefs))]
    private void ClearPlayerPrefs() => PlayerPrefs.DeleteAll();
}
