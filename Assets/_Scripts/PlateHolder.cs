using System;
using DG.Tweening;
using UnityEngine;

public class PlateHolder: MonoBehaviour
{
    [SerializeField] private PlateUIController m_PlateUI;
    public bool IsAvailable { get; set; }
    private int _position;
    private PlateController _plate;

    public int Position
    {
        get => _position;
        set => _position = value;
    }

    public void Awake()
    {
        IsAvailable = true;
    }

    public void Initialize(PlateController plateController)
    {
        _plate = plateController;
        m_PlateUI.Initialize(_plate.ItemRequest, _plate.PlateDuration);
        _plate.PlateSuccess += OnPlateSuccess;
        _plate.PlateFail += OnPlateFailed;
        _plate.PlateUpdate += OnPlateUpdate;
        _plate.PlateMisserved += OnPlateMisServed;
        IsAvailable = false;
    }
    
    private void Update()
    {
        if (!_plate) return;
        m_PlateUI.UpdateTimer(_plate.PlateDuration);
    }

    private void DeInitialize()
    {
        if (!_plate) return;
    
        m_PlateUI.DeInitialize();
        _plate.PlateSuccess -= OnPlateSuccess;
        _plate.PlateFail -= OnPlateFailed;
        _plate.PlateUpdate -= OnPlateUpdate;
        _plate.PlateMisserved -= OnPlateMisServed;

        Destroy(_plate.gameObject);
        _plate = null;
        DOVirtual.DelayedCall(0.5f,()=> IsAvailable = true);
    }

    private void OnPlateFailed(int value)
    {
        if (_plate.CurrentItems.Count <= 0)
        {
            StarUIManager.Instance.PlayNotif(_position,NotifType.EmptyOrder);
            PlayerManager.Instance.AddStars(-10);
        }

        DeInitialize();
        StarUIManager.Instance.PlayStarAnimation(_position,value,() =>
        {
            PlayerManager.Instance.AddStars(value);
        });
        
    }
    
    private void OnPlateMisServed(int value)
    {
        StarUIManager.Instance.PlayNotif(_position,NotifType.WrongOrder);
        PlayerManager.Instance.AddStars(-20);

        DeInitialize();
        StarUIManager.Instance.PlayStarAnimation(_position,value,() =>
        {
            PlayerManager.Instance.AddStars(value);
        });
        
    }

    private void OnPlateSuccess(int value)
    {
        
        if (_plate.CurrentItems.Count == 3)
        {
            StarUIManager.Instance.PlayNotif(_position,NotifType.FullPlate);
            PlayerManager.Instance.AddStars(10);
        }

        if (StreakManager.Instance.IsStreakActive())
        {
            StarUIManager.Instance.PlayNotif(_position, NotifType.Double);
        }
        
        DeInitialize();
        StarUIManager.Instance.PlayStarAnimation(_position,value,() =>
        {
            PlayerManager.Instance.AddStars(value);
            PlayerManager.Instance.AddFoodSold(value);
        });
    }

    private void OnPlateUpdate(int value)
    {
        StarUIManager.Instance.PlayStarNotification(_position, value);
    }
}