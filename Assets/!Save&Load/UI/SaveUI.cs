using System.Collections;
using UnityEngine;

public class SaveUI : MonoBehaviour
{
    [SerializeField] private GameObject _proxy;
    [SerializeField] private GameObject _savedIcon;
    [Space]
    [SerializeField] private GameEvent _onDataLoaded;
    [SerializeField] private GameEvent _onDataSaved;
    [SerializeField] private GameEvent _onSaveSlotSelected;
    [Space]
    [SerializeField] private SaveSelectionButton _firstSaveButton;
    [SerializeField] private SaveSelectionButton _secondSaveButton;
    [SerializeField] private SaveSelectionButton _thirdSaveButton;

    private Coroutine _onSavedRoutine;

    public SaveSelectionButton FirstSaveButton => _firstSaveButton;
    public SaveSelectionButton SecondSaveButton => _secondSaveButton;
    public SaveSelectionButton ThirdSaveButton => _thirdSaveButton;

    #region ==== Unity API ====

    private void Start()
    {
        _firstSaveButton.Unselect();
        _secondSaveButton.Unselect();
        _thirdSaveButton.Unselect();
    }

    private void OnEnable()
    {
        _onSaveSlotSelected.Register(DisableUIProxy);
        _onDataSaved.Register(OnDataSaved);

        _firstSaveButton.Click += FirstSlotSelect;
        _secondSaveButton.Click += SecondSlotSelect;
        _thirdSaveButton.Click += ThirdSlotSelect;
    }

    private void OnDisable()
    {
        _onSaveSlotSelected.Unregister(DisableUIProxy);
        _onDataSaved.Unregister(OnDataSaved);

        _firstSaveButton.Click -= FirstSlotSelect;
        _secondSaveButton.Click -= SecondSlotSelect;
        _thirdSaveButton.Click -= ThirdSlotSelect;
    }

    #endregion ================

    private void FirstSlotSelect()
    {
        _firstSaveButton.Select();
        _secondSaveButton.Unselect();
        _thirdSaveButton.Unselect();

        OnSaveSlotSelected();
    }

    private void SecondSlotSelect()
    {
        _firstSaveButton.Unselect();
        _secondSaveButton.Select();
        _thirdSaveButton.Unselect();

        OnSaveSlotSelected();
    }

    private void ThirdSlotSelect()
    {
        _firstSaveButton.Unselect();
        _secondSaveButton.Unselect();
        _thirdSaveButton.Select();

        OnSaveSlotSelected();
    }

    private void DisableUIProxy() => _proxy.SetActive(false);

    private void OnSaveSlotSelected() => _onSaveSlotSelected.Raise();
    private void OnDataSaved() => StartRoutine();

    #region ==== Routine ====

    private void StartRoutine()
    {
        if (_onSavedRoutine != null)
            return;

        _onSavedRoutine = StartCoroutine(ShowSavedIconRoutine());
    }

    private void StopRoutine()
    {
        if (_onSavedRoutine == null)
            return;

        StopCoroutine(_onSavedRoutine);
        _onSavedRoutine = null; 
    }

    private IEnumerator ShowSavedIconRoutine()
    {
        _savedIcon.SetActive(true);
        yield return new WaitForSeconds(2f);
        _savedIcon.SetActive(false);

        _onSavedRoutine = null;
    }

    #endregion ==============
}
