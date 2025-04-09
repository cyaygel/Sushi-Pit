using System;
using UnityEngine;
using Utility;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    public event Action<int> EarnedStars; 
    public event Action<int> SoldFood; 
    public event Action<int> UpdatedDepth; 

    private int _starCount;
    private int _foodSold;
    private int _depth;

    private int _starHighscore;
    private int _foodHighscore;
    private int _depthHighscore;

    public int StarCount => _starCount;
    public int FoodSold => _foodSold;
    public int Depth => _depth;

    public int StarHighscore => _starHighscore;
    public int FoodHighscore => _foodHighscore;
    public int DepthHighscore => _depthHighscore;

    private const string STAR_KEY = "Highscore_Stars";
    private const string FOOD_KEY = "Highscore_Food";
    private const string DEPTH_KEY = "Highscore_Depth";

    private void Start()
    {
        _starHighscore = PlayerPrefs.GetInt(STAR_KEY, 0);
        _foodHighscore = PlayerPrefs.GetInt(FOOD_KEY, 0);
        _depthHighscore = PlayerPrefs.GetInt(DEPTH_KEY, 0);
        AddStars(30);
        AddFoodSold(0);
        UpdateDepth(0);
    }

    public void AddStars(int amount)
    {
        _starCount += amount;
        EarnedStars?.Invoke(amount);
        CheckForStarHighscore(_starCount);
    }

    public void AddStarSpecial(int amount)
    {
        _starCount += amount;
        CheckForStarHighscore(_starCount);
    }

    public void AddFoodSold(int amount)
    {
        _foodSold += amount;
        SoldFood?.Invoke(amount);
        CheckForFoodHighscore(_foodSold);
    }

    public void UpdateDepth(int amount)
    {
        _depth = amount;
        UpdatedDepth?.Invoke(amount);
        CheckForDepthHighscore(_depth);
    }

    private void CheckForStarHighscore(int currentStars)
    {
        if (currentStars > _starHighscore)
        {
            _starHighscore = currentStars;
            PlayerPrefs.SetInt(STAR_KEY, _starHighscore);
            PlayerPrefs.Save();
        }
    }

    private void CheckForFoodHighscore(int currentFood)
    {
        if (currentFood > _foodHighscore)
        {
            _foodHighscore = currentFood;
            PlayerPrefs.SetInt(FOOD_KEY, _foodHighscore);
            PlayerPrefs.Save();
        }
    }

    private void CheckForDepthHighscore(int currentDepth)
    {
        if (currentDepth < _depthHighscore)
        {
            _depthHighscore = currentDepth;
            PlayerPrefs.SetInt(DEPTH_KEY, _depthHighscore);
            PlayerPrefs.Save();
        }
    }
}
