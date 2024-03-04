using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public class CharCard
{
    public CharCard(CardData _cardData)
    {
        CardData = _cardData;
        equipmentCards = new EquipmentCard[4];
        IsEquipped = false;
        for (int i = 0; i < equipmentCards.Length; i++)
        {
            equipmentCards[i] = null;
        }
        cardName = _cardData.Name + "_" + _cardData.Grade;
    }
    public string cardName; // 그냥 인스펙터에서 보기 편하게 하기 위한 변수 
    public int numberOfEquipments;
    public CardData CardData;
    public EquipmentCard[] equipmentCards;
    public int totalHp, totalAtk;
    public bool IsEquipped; // 문법상 이상하지만 그냥 equipmentCard와 통일하기 위해
}

[System.Serializable]
public class EquipmentCard
{
    public EquipmentCard(CardData _cardData)
    {
        CardData = _cardData;
        IsEquipped = false;

        cardName = _cardData.Name + "_" + _cardData.Grade;
        EquippedWho = null;
    }
    public CardData EquippedWho;
    public string cardName; // 그냥 인스펙터에서 보기 편하게 하기 위한 변수 
    public string equippedWho; // 그냥 인스펙터에서 보기 편하게 하기 위한 변수 
    public CardData CardData;
    public bool IsEquipped;
}

public class CardList : MonoBehaviour
{
    [SerializeField] List<CharCard> charCards;
    [SerializeField] List<EquipmentCard> equipmentCards;

    CardDataManager cardDataManager;
    EquipmentDataManager equipmentDataManager;

    void Awake()
    {
        cardDataManager = GetComponent<CardDataManager>();
        equipmentDataManager = GetComponent<EquipmentDataManager>();
    }

    public void Equip(CardData charData, CardData equipData)
    {
        CharCard charCard = FindCharCard(charData);
        EquipmentCard equipmentCard = FindEquipmentCard(equipData);

        int index = new Convert().EquipmentTypeToInt(equipData.EquipmentType);
        charCard.equipmentCards[index] = equipmentCard;
        equipmentCard.IsEquipped = true;
        equipmentCard.EquippedWho = charCard.CardData;

        equipmentCard.equippedWho = equipmentCard.EquippedWho.Name;
        charCard.numberOfEquipments++;
        charCard.IsEquipped = true;

        EquipStats(charCard, equipData); // 장비의 스탯을 오리카드에 반영
        equipmentDataManager.UpdateEquipment(charCard, index); // 데이터 업데이트 및 저장
    }
    public void UnEquip(CardData charData, EquipmentCard _equipmentCard)
    {
        CharCard charCard = FindCharCard(charData);

        int index =
                new Convert().EquipmentTypeToInt(_equipmentCard.CardData.EquipmentType);
        charCard.equipmentCards[index] = null;
        _equipmentCard.IsEquipped = false;
        charCard.numberOfEquipments--;
        charCard.IsEquipped = charCard.numberOfEquipments > 0; // 장비 수가 0이 되면 charCard의 IsEquipped false

        UnEquipStats(charCard, _equipmentCard.CardData);  // 장비의 스탯을 오리카드에서 제거

        equipmentDataManager.UpdateEquipment(charCard, index);// 데이터 업데이트 및 저장
    }

    public CharCard FindCharCard(CardData charCardData)
    {
        CharCard oriCard = charCards.Find(x => x.CardData.ID == charCardData.ID);
        if (oriCard == null) Debug.Log("Can't find ID " + charCardData.ID);
        return oriCard;
    }
    // 카드 데이터로 EquipmentCard 얻기
    public EquipmentCard FindEquipmentCard(CardData equipCardData)
    {
        EquipmentCard card = equipmentCards.Find(x => x.CardData.ID == equipCardData.ID);
        if (card == null) Debug.Log("Can't find ID " + equipCardData.ID);
        return card;
    }
    // 특정 오리 카드의 장비 카드 얻기
    public EquipmentCard[] GetEquipmentsCardData(CardData charCardData)
    {
        return FindCharCard(charCardData).equipmentCards;
    }

    // weapon과 item을 분리해서 저장
    public void InitCardList()
    {
        charCards = new();
        equipmentCards = new();

        List<CardData> myCardList = cardDataManager.GetMyCardList();

        for (int i = 0; i < myCardList.Count; i++)
        {
            if (myCardList[i].Type == CardType.Weapon.ToString())
            {
                CharCard _charCard = new(myCardList[i]);
                charCards.Add(_charCard);
            }
            else if (myCardList[i].Type == CardType.Item.ToString())
            {
                EquipmentCard equipCard = new(myCardList[i]);
                equipmentCards.Add(equipCard);
            }
        }

        // 모든 카드가 분류되고 나면 장비 데이터대로 장비 장착하기
        for (int i = 0; i < charCards.Count; i++)
        {
            LoadEquipmentData(charCards[i]);
        }
    }
    // charCards에 장비 데이터 로드해서 장착하기
    // 장착시킨 장비는 isEqupped로 설정하기
    void LoadEquipmentData(CharCard _charCard)
    {
        // 매개 변수로 받은 오리카드의 장비 데이터를 찾아서 equipData에 저장
        List<CardEquipmentData> myEquipmentData = equipmentDataManager.GetMyEquipmentsList();

        if (myEquipmentData.Count == 0) return;

        CardEquipmentData equipData = myEquipmentData.Find(x => x.charID == _charCard.CardData.ID);

        if (equipData == null)
            return;
        // ID로 각각의 EquipmentCard를 찾아서 장비 카드만 모아둔 equipmentCards 리스트에서 찾아 준다
        for (int i = 0; i < 4; i++)
        {
            if (equipData.IDs[i] > 0) // 해당 부위에 장비카드가 있다면 (아이디가 없으면 -1, 아이디는 1부터 부여되므로)
            {
                EquipmentCard equipCard = FindCardDataByID(equipData.IDs[i]);
                Equip(_charCard.CardData, equipCard.CardData);
            }
        }
        // 저장되어 있는 데이터를 가져와서 반영하는 것이므로 또 저장할 필요가 없다.
    }
    EquipmentCard FindCardDataByID(int cardID)
    {
        EquipmentCard equipmentCard = equipmentCards.Find(x => x.CardData.ID == cardID);
        return equipmentCard;
    }

    void EquipStats(CharCard _charCard, CardData _equipCard)
    {
        _charCard.totalHp += _equipCard.Hp;
        _charCard.totalAtk += _equipCard.Atk;
    }
    void UnEquipStats(CharCard _charCard, CardData _equipCard)
    {
        _charCard.totalHp -= _equipCard.Hp;
        _charCard.totalAtk -= _equipCard.Atk;
    }

    public List<EquipmentCard> GetEquipmentCardsList()
    {
        return equipmentCards;
    }
}
