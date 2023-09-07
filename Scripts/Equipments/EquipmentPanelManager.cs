using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ClearSlots - 슬롯 부수기
/// GenerateAllCardsList - 슬롯 생성하고 정렬, 카드 Display
/// </summary>
public class EquipmentPanelManager : MonoBehaviour
{
    CardData CardOnDisplay { get; set; } // 디스플레이에 올라가 있는 오리 카드

    DisplayCardOnSlot displayCardOnSlot; // 슬롯 위에 있는 카드 Display
    CardDataManager cardDataManager;
    UpPanelUI upPanelUI; // UI 관련 클래스

    // 카드들이 보여지는 Field
    [SerializeField] AllField field; // 모든 카드

    // 업그레이드 슬롯, 재료 슬롯
    [SerializeField] CardSlot upCardSlot;
    [SerializeField] CardSlot matCardSlot;

    void Awake()
    {
        displayCardOnSlot = GetComponent<DisplayCardOnSlot>();
        cardDataManager = FindObjectOfType<CardDataManager>();
        upPanelUI = GetComponent<UpPanelUI>();

        // upCardSlot.EmptySlot();
        // matCardSlot.EmptySlot();
        // SetAllField();
    }

    void OnEnable()
    {
        SetAllFieldTypeOf("Weapon");
    }

    public void ClearAllFieldSlots()
    {
        field.ClearSlots();
        // matField?.ClearSlots();
    }
    public void SetAllFieldTypeOf(string cardType)
    {
        ClearAllFieldSlots();
        List<CardData> card = new();
        card = cardDataManager.GetMyCardList().FindAll(x => x.Type == cardType);
        field.GenerateAllCardsOfType(card);
    }
}