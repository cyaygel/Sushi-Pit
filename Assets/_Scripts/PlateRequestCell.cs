using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class PlateRequestCell: MonoBehaviour
    {
        [SerializeField] private Image m_RequestIcon;

        public void Initialize(Sprite icon)
        {
            m_RequestIcon.sprite = icon;
        }
    }
}