using System;
using DG.Tweening;
using UnityEngine;
using Utility;

public class GameManager: MonoSingleton<GameManager>
{
    [SerializeField] private GameOverUIController m_GameOverUI;
    [SerializeField] private GameObject m_TutorialUi;
    [SerializeField] private CustomerController m_CustomerController;

    private void OnEnable()
    {
        PlayerManager.Instance.EarnedStars += CheckForGameOver;
        m_TutorialUi.gameObject.SetActive(true);
        Time.timeScale = 0;
        DOVirtual.DelayedCall(4, () => m_CustomerController.CustomersActive = true, false);
        
    }

    private void OnDisable()
    {
        PlayerManager.Instance.EarnedStars -= CheckForGameOver;
    }

    private void CheckForGameOver(int i)
    {
        if (PlayerManager.Instance.StarCount < 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        DOVirtual.DelayedCall(1, () =>
        {
            m_GameOverUI.gameObject.SetActive(true);
            Time.timeScale = 0;
        });
    }
}