using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

public class LevelManager : MonoSingleton<LevelManager>
{
    [SerializeField] private List<PlateData> m_PlatePresets;
    [SerializeField] private List<TimeDifficultyPair> m_LevelFunnel;

    private List<PlateData> _possiblePlateContext;
    private float _gameTimer;
    private int _currentFunnelIndex;
    private Difficulty _currentDifficulty;
    private float _currentSpawnRate;

    public List<PlateData> PossiblePlateContext
    {
        get => _possiblePlateContext;
        private set => _possiblePlateContext = value;
    }

    public float CurrentSpawnRate
    {
        get => _currentSpawnRate;
        set => _currentSpawnRate = value;
    }

    public PlateData GetRandomPlate()
    {
        if (_possiblePlateContext == null || _possiblePlateContext.Count == 0)
        {
            return default;
        }
        int randomIndex = Random.Range(0, _possiblePlateContext.Count);
        return _possiblePlateContext[randomIndex];
    }

    private void Start()
    {
        _gameTimer = 0f;
        _currentFunnelIndex = 0;
        if (m_LevelFunnel.Count > 0 && m_LevelFunnel[0].Difficulties.Count > 0)
        {
            _currentDifficulty = m_LevelFunnel[0].Difficulties[0];
        }
        UpdatePlateContext();
    }

    private void Update()
    {
        _gameTimer += Time.deltaTime;
        CheckForFunnel();
    }

    private void CheckForFunnel()
    {
        if (_currentFunnelIndex >= m_LevelFunnel.Count - 1) return;
        
        var nextPair = m_LevelFunnel[_currentFunnelIndex + 1];
        if (_gameTimer >= nextPair.Time)
        {
            _currentFunnelIndex++;
            PickNewDifficulty();
            UpdatePlateContext();
        }
    }

    private void PickNewDifficulty()
    {
        var difficulties = m_LevelFunnel[_currentFunnelIndex].Difficulties;
        var randomIndex = UnityEngine.Random.Range(0, difficulties.Count);
        _currentDifficulty = difficulties[randomIndex];
        Debug.Log("Difficulty updated to: " + _currentDifficulty);
    }

    private void UpdatePlateContext()
    {
        var allowedDifficulties = m_LevelFunnel[_currentFunnelIndex].Difficulties;
        PossiblePlateContext = m_PlatePresets.FindAll(p => allowedDifficulties.Contains(p.Difficulty));
        _currentSpawnRate = m_LevelFunnel[_currentFunnelIndex].SpawnRate;
    }
}

[Serializable]
public struct TimeDifficultyPair
{
    public float Time;
    public List<Difficulty> Difficulties;
    public float SpawnRate;
}

[Serializable]
public struct PlateData
{
    public int Size;
    public float Duration;
    public Difficulty Difficulty;
}

public enum Difficulty
{
    Easy,
    Normal,
    Hard,
    Mythic
}
