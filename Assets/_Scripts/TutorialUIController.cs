using System;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUIController : MonoBehaviour
{
    [SerializeField] private Button m_OkayButton;

    private void OnEnable()
    {
        m_OkayButton.onClick.AddListener(OnOkayButtonPressed);
    }

    private void OnDisable()
    {
        m_OkayButton.onClick.RemoveListener(OnOkayButtonPressed);
    }

    private void OnOkayButtonPressed()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
