using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType { Weapon, Item, none }

public class Card : MonoBehaviour
{
    WeaponData weaponData;
    Item itemData;
    Gear gear;
    CardData myCardData;
    CardType cardType;
    string ID, Name;
    int exp;
    int level;
    Grade Grade;

    int To_Level_Up_Card
    {
        get
        {
            return (int)(Mathf.Pow((level) / 3.5f, 2)) * 1000 + (100 * level);
        }
    }

    public void SetWeaponCardData(WeaponData _weaponData, CardData cardData)
    {
        if(_weaponData == null) Debug.Log("weaponData가 Null입니다.");
        this.weaponData = _weaponData;
        this.myCardData = cardData;
        ID = cardData.ID;
        cardType = CardType.Weapon;
        Name = _weaponData.Name;
        Grade = _weaponData.grade;
        level = 1;
        GetComponent<CardDisplay>().InitWeaponCardDisplay(this.weaponData);
    }
    public void SetItemCardData(Item _itemData, CardData cardData)
    {
        this.itemData = _itemData;
        this.myCardData = cardData;
        ID = cardData.ID;
        cardType = CardType.Item;
        Name = _itemData.Name;
        Grade = _itemData.grade;
        level = 1;
        GetComponent<CardDisplay>().InitItemCardDisplay(this.itemData);
    }

    public CardData GetCardData()
    {
        return myCardData;
    }
    public string GetCardID()
    {
        return ID;
    }

    public string GetCardName()
    {
        return Name;
    }

    public CardType GetCardType()
    {
        if (cardType != CardType.Weapon && cardType != CardType.Item)
        {
            Debug.Log("카드 타입이 정해지지 않았습니다.");
            return CardType.none;
        }
        return cardType;
    }

    public Grade GetCardGrade()
    {
        return Grade;
    }

    public void AddExp(int expToAdd)
    {
        exp += expToAdd;
    }

    void LevelUp()
    {
        if(level < 30)
        {
            level++;
            GetComponent<CardDisplay>().UpdateCard(level);
        }
    }
}
