using System;
using System.Collections.Generic;
using DefaultNamespace;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlateUIController: MonoBehaviour
{
    [SerializeField] private List<PlateRequestCell> m_Requests;
    [SerializeField] private GameObject m_TimerParent;
    [SerializeField] private Image m_TimerFill;

    private float _maxDuration;
    private float _currentDuration;
    private Sequence _spawnSeq;
    private Sequence _killSeq;

    private void OnEnable()
    {
        foreach (var request in m_Requests)
        {
            request.gameObject.SetActive(false);
        }
        m_TimerParent.SetActive(false);
    }

    public void Update()
    {
        m_TimerFill.fillAmount = _currentDuration / _maxDuration;
    }

    public void Initialize(List<int> ids, float duration)
    {
        _spawnSeq?.Kill();
        _killSeq?.Kill();
        _spawnSeq = DOTween.Sequence();
        _spawnSeq.Append(m_TimerParent.transform.DOScale(0, 0));
        for (int i = 0; i < ids.Count; i++)
        {
            var nextRequest = m_Requests[i];
            nextRequest.gameObject.SetActive(true);
            var icon = ItemDatabase.Instance.FindItemIconById(ids[i]);
            nextRequest.Initialize(icon);
            _spawnSeq.Append(nextRequest.GetComponent<RectTransform>().DOScale(0, 0));
            _spawnSeq.Append(nextRequest.GetComponent<RectTransform>().DOScale(1, .3f).SetEase(Ease.OutBack));
        }

        m_TimerFill.fillAmount = 1;
        _maxDuration = duration;
        _currentDuration = _maxDuration;
        m_TimerParent.SetActive(true);
        _spawnSeq.Append(m_TimerParent.transform.DOScale(1, 0.2f).SetEase(Ease.OutBack));
    }

    public void DeInitialize()
    {
        _killSeq?.Kill();
        _killSeq = DOTween.Sequence();
        _killSeq.Append(m_TimerParent.transform.DOScale(0, 0.2f).SetEase(Ease.InBack));
        foreach (var request in m_Requests)
        {
            _killSeq.Append(request.transform.DOScale(0, 0.2f).SetEase(Ease.InBack));
            request.gameObject.SetActive(false);
        }

        _killSeq.OnComplete(() =>
        {
            foreach (var request in m_Requests)
            {
                request.gameObject.SetActive(false);
            }
            m_TimerParent.SetActive(false);
        });

    }

    public void UpdateTimer(float duration)
    {
        _currentDuration = duration;
    }
}