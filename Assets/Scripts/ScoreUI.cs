using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private Text scoreText;

    void Update()
    {
        scoreText.text = $"SCORE : {GameManager.Instance.score}";
    }
}
