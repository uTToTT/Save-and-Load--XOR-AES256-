using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _score;

    public void UpdateDisplay(int score) => _score.text = score.ToString();
}
