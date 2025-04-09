using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlateController: MonoBehaviour
{
    public event Action<int> PlateSuccess;
    public event Action<int> PlateFail;
    public event Action<int> PlateUpdate;
    public event Action<int> PlateMisserved; 

    [SerializeField] private GameObject m_ItemGrid;
    [SerializeField] private List<int> m_ItemRequest;
    [SerializeField] private List<int> m_CurrentItems;
    [SerializeField] private GameObject m_SelectedEffect;

    private PlateData _plateData;
    private bool _plateActive;
    private int _plateValue;
    private int _size = 3;
    private float _plateDuration;

    public List<int> ItemRequest => m_ItemRequest;

    public List<int> CurrentItems => m_CurrentItems;

    public float PlateDuration => _plateDuration;

    public int PlateValue => _plateValue;

    private void OnEnable()
    {
        _plateActive = false;
        _plateValue = 0;
    }

    private void Update()
    {
        if (_plateActive)
        {
            _plateDuration = PlateDuration - Time.deltaTime;

            if (PlateDuration <= 0)
            {
                FailPlate();
            }
        }
    }

    public void ActivatePlate(PlateData data)
    {
        _size = data.Size;
        _plateDuration = data.Duration;
        PopulateRequest();
        _plateActive = true;
    }
    public void TryAddItem(int id)
    {
        var db = ItemDatabase.Instance;
        CurrentItems.Add(id);
        VisualizePlate();
        foreach (var i in ItemRequest)
        {
            if (i == id)
            {
                _plateValue = PlateValue + db.GetValueByID(id);
                ItemRequest.Remove(i);
                CheckForCompletePlate();
                PlateUpdate?.Invoke(_plateValue);
                return;
            }
        }
        PlateMisserved?.Invoke(_plateValue);
        FailPlate();
    }

    public void SetSelectedEffect(bool active)
    {
        m_SelectedEffect.SetActive(active);
    }

    private void PopulateRequest()
    {
        ItemRequest.Clear();
        var db = ItemDatabase.Instance;
        for (int i = 0; i < _size; i++)
        {
            var randomID = db.ItemDatas[Random.Range(0, db.ItemDatas.Count)];
            ItemRequest.Add(randomID.ItemID);
        }
    }
    private void SuccessPlate()
    {
        PlateSuccess?.Invoke(PlateValue);
        Debug.Log("plate completed");
        _plateActive = false;
    }

    private void FailPlate()
    {
        PlateFail?.Invoke(PlateValue);
        Debug.Log("plate failed");
        _plateActive = false;
    }

    private void CheckForCompletePlate()
    {
        if (CurrentItems.Count != _size) return;

        if (PlateValue <= 0)
        {
            FailPlate();
        }
        else
        {
            SuccessPlate();
        }
        
    }

    private void VisualizePlate()
    {
        foreach (Transform child in m_ItemGrid.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var id in CurrentItems)
        {
            var prefab = ItemDatabase.Instance.FindItemPrefabById(id);
            var newFood = Instantiate(prefab, m_ItemGrid.transform);
            newFood.GetComponent<Collider>().enabled = false;
        }
    }
    
    
}