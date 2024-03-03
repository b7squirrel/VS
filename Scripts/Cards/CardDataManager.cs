using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[System.Serializable]
public class Serialization<T>
{
    public Serialization(List<T> _target) => Data = _target;
    public List<T> Data;
}

[System.Serializable]
public class CardData
{
    public CardData(string _id, string _Type, string _Grade, string _Name, 
        string _level, string _hp, string _atk, string _equipmentType, 
        string _essectialEquip, string _bindingTo, string _startingMember, 
        string _defaultItem, string _passiveSkill)
    {
        ID = _id;
        Type = _Type;
        Debug.Log(_Grade);
        Grade = int.Parse(_Grade);
        Name = _Name;
        Level = int.Parse(_level);
        Hp = int.Parse(_hp);
        Atk = int.Parse(_atk);
        EquipmentType = _equipmentType;
        EssentialEquip = _essectialEquip;
        BindingTo = _bindingTo;
        StartingMember = _startingMember;
        DefaultItem = _defaultItem;
        PassiveSkill = int.Parse(_passiveSkill);

    }

    public string ID, Type, Name, 
            EquipmentType, EssentialEquip, BindingTo,
            StartingMember, DefaultItem;
    public int Grade, Level, Hp, Atk, PassiveSkill;
}
public class ReadCardData
{
    public List<CardData> cardList;
    public List<CardData> GetCardsList(TextAsset cardDataText)
    {
        cardList = new List<CardData>();

        string[] line = cardDataText.text.Substring(0, cardDataText.text.Length - 1).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');

            List<string> rowList = new();
            for(int j = 0; j < row.Length - 1; j++)
            {
                rowList.Add(row[j]);
            }

            cardList.Add(new CardData(row[0], row[1], row[2], row[3], row[4], 
                                      row[5], row[6], row[7], row[8], row[9], 
                                      row[10], row[11], row[12]));
        }
        return cardList;
    }
}

public class CardDataManager : MonoBehaviour
{
    public TextAsset CardDatabase;
    public TextAsset startingCardData;
    public List<CardData> AllCardsList, MyCardsList;
    string filePath;
    string myCards = "MyCards.txt";

    void Start()
    {
        // 전체 카드 리스트 불러오기
        // AllCardsList = new ReadCardData().GetCardsList(CardDatabase);

        if (Directory.Exists(Application.persistentDataPath + "/PlayerData") == false)
            Directory.CreateDirectory(Application.persistentDataPath + "/PlayerData");

        filePath = Application.persistentDataPath + "/PlayerData/" + myCards;

        Load();
    }

    void Save()
    {
        string jsonData = JsonUtility.ToJson(new Serialization<CardData>(MyCardsList), true);
        File.WriteAllText(filePath, jsonData);

        GetComponent<CardList>().InitCardList();
    }

    void Load()
    {
        if (!File.Exists(filePath))
        {
            ResetCards();
            return;
        }
        string jdata = File.ReadAllText(filePath);
        MyCardsList = JsonUtility.FromJson<Serialization<CardData>>(jdata).Data;
    }

    // 특정 카드를 가지고 시작하도록 만들려고. 아무것도 없이 시작할 수도 있다
    void ResetCards()
    {
        Debug.Log("리셋 카드 데이터");
        MyCardsList.Clear();
        List<CardData> startingCards = new ReadCardData().GetCardsList(startingCardData);
        AddNewCardToMyCardsList(startingCards[0]);
        startingCards[0].StartingMember = StartingMember.Zero.ToString();

        FindObjectOfType<GachaSystem>().AddEssentialEquip(startingCards[0]);

        Save();
        Load();
    }

    public List<CardData> GetMyCardList()
    {
        if (MyCardsList == null) Debug.Log("리스트 널");
        return MyCardsList;
    }
    public List<CardData> GetAllCardList()
    {
        if (AllCardsList == null) AllCardsList = new();
        if (AllCardsList.Count == 0)
            AllCardsList = new ReadCardData().GetCardsList(CardDatabase);
        Debug.Log("All card list numbers = " + AllCardsList.Count);
        return AllCardsList;
    }

    public void RemoveCardFromMyCardList(CardData cardToRemove)
    {
        string mID = cardToRemove.ID;
        for (int i = 0; i < MyCardsList.Count; i++)
        {
            if (MyCardsList[i].ID == mID)
            {
                MyCardsList.Remove(MyCardsList[i]);
                return;
            }
            Save();
        }
    }

    // 새로운 카드는 아이디를 발급받는다
    public void AddNewCardToMyCardsList(CardData _cardData)
    {
        //_cardData.ID = Guid.NewGuid().ToString();
        _cardData.ID = GenerateRandomId().ToString();

        MyCardsList.Add(_cardData);
        Save();
    }
    // 착용되어 있는 장비는 아이디가 바뀌면 안되므로
    public void AddUpgradedCardToMyCardList(CardData _cardData)
    {
        MyCardsList.Add(_cardData);
        Save();
    }
    public void UpgradeCardData(CardData _cardData, int _level, int _hp, int _atk)
    {
        _cardData.Level = _level;
        _cardData.Hp = _hp;
        _cardData.Atk = _atk;
        Save();
    }
    public void UpdateStartingmemberOfCard(CardData cardToUpdate, string orderIndex)
    {
        cardToUpdate.StartingMember = orderIndex;
        Save();
    }
    public void DeleteData()
    {
        ResetCards();
    }

    // 중복되지 않는 랜덤 아이디 생성 함수
    int GenerateRandomId()
    {
        int randomId;
        do
        {
            randomId = UnityEngine.Random.Range(1, 10000);
        } while (ItemIdExists(randomId));

        return randomId;
    }

    // 이미 존재하는 아이디인지 확인하는 함수
    bool ItemIdExists(int id)
    {
        for (int i = 0; i < MyCardsList.Count; i++)
        {
            if (MyCardsList[i].ID == id.ToString())
            {
                return true;
            }
        }
        return false;
    }
}
