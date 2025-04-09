using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainUIController: MonoBehaviour
{
    [SerializeField] private MultiTextController m_StarText;
    [SerializeField] private MultiTextController m_DepthText;
    [SerializeField] private MultiTextController m_FoodSoldText;
    [SerializeField] private Button m_TutButton;
    [SerializeField] private GameObject m_TutorialUI;

    private PlayerManager _pm;

    private void OnEnable()
    {
        Initialize();
        m_TutButton.onClick.AddListener(OnTutButtonPressed);
    }

    private void Initialize()
    {
        _pm = PlayerManager.Instance;
        _pm.EarnedStars += OnStarTextUpdated;
        _pm.UpdatedDepth += OnDepthTextUpdated;
        _pm.SoldFood += OnFoodSoldTextUpdated;
    }

    private void OnDisable()
    {
        _pm.EarnedStars -= OnStarTextUpdated;
        _pm.UpdatedDepth -= OnDepthTextUpdated;
        _pm.SoldFood -= OnFoodSoldTextUpdated;
        m_TutButton.onClick.RemoveListener(OnTutButtonPressed);
    }

    private void OnTutButtonPressed()
    {
        m_TutorialUI.SetActive(true);
        Time.timeScale = 0;
    }

    private void OnStarTextUpdated(int i)
    {
        m_StarText.UpdateText(_pm.StarCount.ToString());
    }
    
    private void OnDepthTextUpdated(int i)
    {
        m_DepthText.UpdateText(_pm.Depth.ToString());

    }
    
    private void OnFoodSoldTextUpdated(int i)
    {
        m_FoodSoldText.UpdateText(_pm.FoodSold.ToString());

    }
}