using System;

public class Clicker
{
    public event Action<int> OnScoreChanged;

    private GameEvent _onDataLoadedEvent;

    private int _score;

    private IPersistentData _persistentData;

    public Clicker(IPersistentData persistentData, GameEvent onDataLoadedEvent)
    {
        _persistentData = persistentData;
        _onDataLoadedEvent = onDataLoadedEvent;

        _onDataLoadedEvent.Register(LoadScore);
    }

    public void OnClick()
    {
        _score++;
        _persistentData.PlayerData.Score = _score;
        RaiseScore();
    }

    private void RaiseScore()
    {
        OnScoreChanged?.Invoke(_score);
    }

    private void LoadScore()
    {
        _score = _persistentData.PlayerData.Score;
        RaiseScore();
    }
}
