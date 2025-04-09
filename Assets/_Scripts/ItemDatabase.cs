using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "ItemDatabase", order = 100)]
public class ItemDatabase: SingletonScriptableObject<ItemDatabase>
{
    public List<ItemData> ItemDatas;

    public GameObject FindItemPrefabById(int id)
    {
        foreach (var data in ItemDatas)
        {
            if (id == data.ItemID)
            {
                return data.ItemPrefab;
            }
        }
        Debug.LogError("There is no item in database with" + id + "id");
        return null;
    }
    
    public Sprite FindItemIconById(int id)
    {
        foreach (var data in ItemDatas)
        {
            if (id == data.ItemID)
            {
                return data.ItemSprite;
            }
        }
        Debug.LogError("There is no sprite in database with" + id + "id");
        return null;
    }

    public int GetValueByID(int id)
    {
        foreach (var data in ItemDatas)
        {
            if (id == data.ItemID)
            {
                return data.ItemValue;
            }
        }
        Debug.LogError("There is no value in database with" + id + "id");
        return 0;
    }
}

[Serializable]
public struct ItemData
{
    public int ItemID;
    public GameObject ItemPrefab;
    public Sprite ItemSprite;
    public int ItemValue;
}