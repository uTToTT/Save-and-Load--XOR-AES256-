using Newtonsoft.Json;

public class PlayerData
{
    private int _score;

    #region ==== Constructors ====

    public PlayerData()
    {
        _score = 0;
    }

    [JsonConstructor]
    public PlayerData(int score)
    {
        _score = score;
    }

    #endregion ===================

    public int Score
    {
        get => _score; set
        {
            if (value < 0)
                throw new System.ArgumentException(nameof(_score));

            _score = value;
        }
    }
}
