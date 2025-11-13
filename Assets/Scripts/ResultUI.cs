using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    [SerializeField] Text scoreText;

    void Start()
    {
        scoreText.text = "SCORE : " + ScoreData.LastScore;
    }
}
