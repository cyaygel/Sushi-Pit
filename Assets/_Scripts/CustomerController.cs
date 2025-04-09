using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class CustomerController : MonoBehaviour
{
    [SerializeField] private float m_PlateSpawnRate = 2;
    [SerializeField] private List<PlateController> m_PlatePrefabs;
    [SerializeField] private List<PlateHolder> m_PlateHolders;
    [field: SerializeField] public bool CustomersActive { get; set; }

    private float _timer;
    private bool _readyToCheck;

    private void OnEnable()
    {
        for (int i = 0; i < m_PlateHolders.Count; i++)
        {
            m_PlateHolders[i].Position = i;
        }

        _readyToCheck = true;
    }

    private void Update()
    {
        if (CustomersActive)
        {
            if (NoPlatesActive())
            {
                AddPlate();
                _timer = 0;
            }
            
            _timer += Time.deltaTime;
            if (_timer >= m_PlateSpawnRate)
            {
                AddPlate();
                _timer = 0;
            }
        }
        
    }

    private void AddPlate()
    {
        var randomPlate = m_PlatePrefabs[Random.Range(0, m_PlatePrefabs.Count)];
        var emptyHolder = GetEmptyHolder();
        if (emptyHolder != null)
        {
            var newPlate = Instantiate(randomPlate, emptyHolder.transform);
            var randPlateData = LevelManager.Instance.GetRandomPlate();
            m_PlateSpawnRate = LevelManager.Instance.CurrentSpawnRate;
            newPlate.ActivatePlate(randPlateData);
            emptyHolder.Initialize(newPlate);
        }
        else
        {
            Debug.LogWarning("There is no room for a new plate to spawn");
        }
    }

    private bool NoPlatesActive()
    {
        return m_PlateHolders.All(holder => holder.IsAvailable);
    }

    private PlateHolder GetEmptyHolder()
    {
        var emptyHolders = new List<PlateHolder>();
        foreach (var holder in m_PlateHolders)
        {
            if (holder.IsAvailable)
            {
                emptyHolders.Add(holder);
            }
        }
        
        return emptyHolders.Count <= 0 
            ? null 
            : emptyHolders[Random.Range(0, emptyHolders.Count)];
    }
}
