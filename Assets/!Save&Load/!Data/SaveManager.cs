using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    private const string CRYPT_KEY = "it_is_password";

    [SerializeField, Range(5, 300)] private float _autoSaveDelay;
    [Space]
    [SerializeField] private Button _saveButton;
    [Space]
    [SerializeField] private GameEvent _onDataLoadedEvent;
    [SerializeField] private GameEvent _onDataSavedEvent;
    [SerializeField] private GameEvent _onSaveSlotSelected;
    [Space]
    [SerializeField] private SaveUI _saveUI;

    private int _currenSlot = -1;

    private IPersistentData _persistentData;
    private IDataProvider _provider;
    private ICipherProvider _cipherProvider;

    private Coroutine _autoSaveRoutine;

    public IDataProvider Provider => _provider;
    public IPersistentData PersistentData => _persistentData;
    public GameEvent OnDataLoadedEvent => _onDataLoadedEvent;
    public GameEvent OnDataSavedEvent => _onDataSavedEvent;

    #region ==== Init ====

    public void Init()
    {
        InitData();

        _saveUI.FirstSaveButton.Click += OnFirstSlotClick;
        _saveUI.SecondSaveButton.Click += OnSecondSlotClick;
        _saveUI.ThirdSaveButton.Click += OnThirdSlotClick;

        _onSaveSlotSelected.Register(StartAutoSaveRoutine);

        _saveButton.onClick.AddListener(SaveGame);
    }

    private void InitData()
    {
        _persistentData = new PersistentData();
        //_cipherProvider = new XORCipher(CRYPT_KEY);
        _cipherProvider = new AESCipher();
        _provider = new DataLocalProvider(_persistentData, _cipherProvider);
    }

    #endregion ===========

    private void OnFirstSlotClick() => LoadOrInit(1);
    private void OnSecondSlotClick() => LoadOrInit(2);
    private void OnThirdSlotClick() => LoadOrInit(3);

    private void SaveGame()
    {
        _provider.Save(_currenSlot);
        _onDataSavedEvent.Raise();
    }

    private void LoadOrInit(int slotId)
    {
        if (_provider.TryLoad(slotId) == false)
            _persistentData.PlayerData = new PlayerData();

        _currenSlot = slotId;
        _onDataLoadedEvent.Raise();
    }

    #region ==== Routine ====

    private void StartAutoSaveRoutine()
    {
        if (_autoSaveRoutine != null)
            return;

        _autoSaveRoutine = StartCoroutine(AutoSaveRoutine());
    }

    private void StopAutoSaveRoutine()
    {
        if (_autoSaveRoutine == null) 
            return;

        StopCoroutine(_autoSaveRoutine);
        _autoSaveRoutine = null;
    }

    private IEnumerator AutoSaveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_autoSaveDelay);
            SaveGame();
        }
    }

    #endregion ==============
}
