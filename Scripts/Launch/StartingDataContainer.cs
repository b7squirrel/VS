using System.Collections.Generic;
using UnityEngine;

public class StartingDataContainer : MonoBehaviour
{
    OriAttribute leadAttr = new OriAttribute(0, 0);
    WeaponData leadWd;
    List<Item> itemDatas = new();
    int essectialEquipmentIndex;

    [Header("Debugging")]
    [SerializeField] int hp = 0;
    [SerializeField] int atk = 0;
    [SerializeField] List<Item> itemDatasDebug = new();
    [SerializeField] int essectialIndexDebug;

    void Awake() => DontDestroyOnLoad(this);
    public void SetLead(CardData lead, OriAttribute leadAttr)
    {
        itemDatas.Clear();
        itemDatasDebug.Clear();
        essectialEquipmentIndex = -1;
        essectialIndexDebug = -1;

        this.leadAttr = leadAttr;
        // debugging
        hp = this.leadAttr.Hp;
        atk = this.leadAttr.Atk;

        CardsDictionary cardDic = FindAnyObjectByType<CardsDictionary>();
        CardDataManager cardDataManager = FindAnyObjectByType<CardDataManager>();
        CardList cardList = FindObjectOfType<CardList>();

        leadWd = cardDic.GetWeaponItemData(lead).weaponData;

        // 장비 데이터 넘기기
        EquipmentCard[] equipCard = cardList.GetEquipmentsCardData(lead);
        for (int i = 0; i < 4; i++)
        {
            if(equipCard[i] == null) 
            {
                itemDatas.Add(null);
                itemDatasDebug.Add(null);
                continue;
            }
            itemDatas.Add(cardDic.GetWeaponItemData(equipCard[i].CardData).itemData);
            itemDatasDebug.Add(cardDic.GetWeaponItemData(equipCard[i].CardData).itemData);

            if (new CheckIsEssentialItem().IsEssential(equipCard[i].CardData, cardDataManager.GetAllCardList()))
            {
                essectialEquipmentIndex = i;
                essectialIndexDebug = i;
            }
        }
    }

    // Player loads the following information after starting the game
    public OriAttribute GetLeadAttr() => this.leadAttr;
    public WeaponData GetLeadWeaponData() => this.leadWd;
    public List<Item> GetItemDatas() => this.itemDatas;
    public int GetEssectialIndex() => this.essectialEquipmentIndex;
}
