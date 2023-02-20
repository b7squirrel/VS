using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


[Serializable]
public class ItemStats
{
    public int armor;
    public int magnetSize;

    public int hp;
    public int coins;

    // coin과 hp는 Equip 같은 것들을 거치지 않고 바로 Level에서 적용되므로 Sum에 포함되지 않는다.
    internal void Sum(ItemStats stats)
    {
        armor += stats.armor;
        magnetSize += stats.magnetSize;
    }
}

[CreateAssetMenu(fileName = "ItemData", menuName = "SO/ItemData")]
public class Item : ScriptableObject
{
    public string Name;
    public ItemStats stats;
    public List<UpgradeData> upgrades;

    public void Init(string Name)
    {
        this.Name = Name;
        stats = new ItemStats();
        upgrades = new List<UpgradeData>();
    }

    public void Equip(Character character)
    {
        Debug.Log("Armor added");
        character.Armor += stats.armor;
        character.MagnetSize +=stats.magnetSize;
    }

    public void UnEquip(Character character)
    {
        character.Armor -= stats.armor;
    }
}
