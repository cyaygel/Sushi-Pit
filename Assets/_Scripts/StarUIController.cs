using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using Random = UnityEngine.Random;

public class StarUIManager: MonoSingleton<StarUIManager>
{
    [SerializeField] private List<StarUIVisual> m_Stars;
    [SerializeField] private List<RectTransform> m_StartPoints;
    [SerializeField] private RectTransform m_StreakPoint;
    [SerializeField] private RectTransform m_EndPoint;
    [SerializeField] private AnimationCurve m_PathEase;
    [SerializeField] private GameObject m_FullPlateNotif;
    [SerializeField] private GameObject m_EmptyOrderNotif;
    [SerializeField] private GameObject m_WrongOrderNotif;
    [SerializeField] private GameObject m_StreakNotif;
    [SerializeField] private GameObject m_StreakLostNotif;
    [SerializeField] private GameObject m_DoubleNotif;
    [SerializeField] private TextMeshProUGUI m_StreakText;
    [SerializeField] private Color m_SuccessColor;
    [SerializeField] private Color m_FailColor;

    public void PlayStreakNotification(int streak)
    {
        var seq = DOTween.Sequence();
        var rect = m_StreakNotif.GetComponent<RectTransform>();
        seq.Append(rect.DOScale(0, 0));
        seq.Append(rect.DOMove(m_StreakPoint.position, 0));
        seq.Append(rect.DOScale(1, 0.4f).SetEase(Ease.OutBack));
        seq.AppendCallback((() => { m_StreakText.text = streak.ToString(); }));
        seq.AppendInterval(0.2f);
        seq.Append(rect.DOScale(0, 0.2f).SetEase(Ease.InBack));
    }
    public void PlayStreakLostNotification()
    {
        var seq = DOTween.Sequence();
        var rect = m_StreakLostNotif.GetComponent<RectTransform>();
        seq.Append(rect.DOScale(0, 0));
        seq.Append(rect.DOMove(m_StreakPoint.position, 0));
        seq.Append(rect.DOScale(1, 0.4f).SetEase(Ease.OutBack));
        seq.AppendInterval(0.2f);
        seq.Append(rect.DOScale(0, 0.2f).SetEase(Ease.InBack));
    }
    
    public void PlayStarNotification(int holderPosition, int value)
    {
        var star = m_Stars[holderPosition];
        var starRect = m_Stars[holderPosition].GetComponent<RectTransform>();
        var seq = DOTween.Sequence();
        var color = value <= 0 ? m_FailColor : m_SuccessColor;

        seq.Append(starRect.DOMove(m_StartPoints[holderPosition].position, 0));
        seq.Append(starRect.DOScale(1, 0.2f).SetEase(Ease.OutBack));
        seq.AppendCallback((() =>
        {
            star.Init(color, value.ToString());
        }));
        seq.Append(starRect.DOScale(0.7f, 0.1f).SetEase(Ease.OutBack));
    }

    public void PlayNotif(int holderPosition, NotifType type)
    {
        var context = type switch
        {
            NotifType.FullPlate => m_FullPlateNotif,
            NotifType.EmptyOrder => m_EmptyOrderNotif,
            NotifType.WrongOrder => m_WrongOrderNotif,
            NotifType.Double => m_DoubleNotif,
            _ => m_FullPlateNotif
        };
        var seq = DOTween.Sequence();
        var rect = context.GetComponent<RectTransform>();
        seq.Append(rect.DOScale(0, 0));
        seq.Append(rect.DOMove(m_StartPoints[holderPosition].position + new Vector3(0, 100 , 0), 0));
        seq.Append(rect.DOScale(2, 0.4f).SetEase(Ease.OutBack));
        seq.AppendInterval(0.2f);
        seq.Append(rect.DOScale(0, 0.2f).SetEase(Ease.InBack));
    }

    [Button]
    public void PlayStarAnimation(int holderPosition, int amount, Action onComplete = null)
    {
        var color = amount <= 0 ? m_FailColor : m_SuccessColor;
        var star = m_Stars[holderPosition];
        var starRect = m_Stars[holderPosition].GetComponent<RectTransform>();
        star.Init(color, amount.ToString());

        var startPos = m_StartPoints[holderPosition].position;
        var endPos = m_EndPoint.position;
        var midPos = (startPos + endPos) / 2;
        midPos += new Vector3(0, Random.Range(-300,300), 0);
        Vector3[] path = { startPos, midPos, endPos };

        var seq = DOTween.Sequence();
        seq.Append(starRect.DOScale(0, 0));
        seq.Append(starRect.DOMove(startPos, 0));
        seq.Join(starRect.DOScale(1, 0.1f).SetEase(Ease.OutBack));
        seq.Join(starRect.DOPath(path, 0.5f, PathType.CatmullRom).SetEase(m_PathEase));
        seq.OnComplete(() =>
        {
            starRect.DOScale(0, 0f).SetEase(Ease.InBack);
            onComplete?.Invoke();
        });
    }
}

public enum NotifType
{
    FullPlate,
    EmptyOrder,
    WrongOrder,
    Double,
}