using UnityEngine;
using UnityEngine.UI;

public class StarUIVisual: MonoBehaviour
{
    [SerializeField] private Image m_StarImage;
    [SerializeField] private MultiTextController m_StarText;

    public void Init(Color starColor, string starText)
    {
        m_StarImage.color = starColor;
        m_StarText.UpdateText(starText);
    }
}