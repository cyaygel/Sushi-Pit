using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverUIController : MonoBehaviour
{
    [SerializeField] private Button m_ReplayButton;
    [SerializeField] private TextMeshProUGUI starHighscoreText;
    [SerializeField] private TextMeshProUGUI foodHighscoreText;
    [SerializeField] private TextMeshProUGUI depthHighscoreText;
    [SerializeField] private TextMeshProUGUI starScoreText;
    [SerializeField] private TextMeshProUGUI foodScoreText;
    [SerializeField] private TextMeshProUGUI depthScoreText;

    private void OnEnable()
    {
        UpdateScoreUI();
        m_ReplayButton.onClick.AddListener(OnReplayButtonPressed);
    }

    private void OnDisable()
    {
        m_ReplayButton.onClick.RemoveListener(OnReplayButtonPressed);
    }

    private void UpdateScoreUI()
    {
        var playerManager = PlayerManager.Instance;
        starHighscoreText.text = playerManager.StarHighscore.ToString();
        foodHighscoreText.text = playerManager.FoodHighscore.ToString();
        depthHighscoreText.text = playerManager.DepthHighscore.ToString();
        starScoreText.text = playerManager.StarCount.ToString();
        foodScoreText.text = playerManager.FoodSold.ToString();
        depthScoreText.text = playerManager.Depth.ToString();
    }

    private void OnReplayButtonPressed()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
