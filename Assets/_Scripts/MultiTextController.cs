using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MultiTextController: MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> m_Texts;

    public void UpdateText(string context)
    {
        foreach (var text in m_Texts)
        {
            text.text = context;
        }
    }
}