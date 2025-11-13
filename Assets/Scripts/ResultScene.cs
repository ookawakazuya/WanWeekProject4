using UnityEngine;
using UnityEngine.UI;

public class ResultScene : MonoBehaviour
{
    [SerializeField] Text scoreText;

    void Start()
    {
        int lastScore = PlayerPrefs.GetInt("LastScore", 0);
        scoreText.text = "SCORE: " + lastScore;
    }

    public void OnRetryButton()
    {
        SceneController.Instance.StartGame();
    }

    public void OnTitleButton()
    {
        SceneController.Instance.GoToTitle();
    }
}
