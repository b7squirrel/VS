using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AllField : MonoBehaviour
{
    #region 참조 변수
    [SerializeField] CardDataManager cardDataManager;
    [SerializeField] SetCardDataOnSlot displayCardOnSlot;
    #endregion

    #region 슬롯 생성 관련 변수
    int numSlots;
    CardData firstCardData;
    [SerializeField] GameObject slotPrefab;
    #endregion

    #region Refresh
    public void GenerateAllCardsOfType(List<CardData> cardList)
    {
        List<CardData> cardDatas = new();
        List<GameObject> slots = new();

        cardDatas.AddRange(cardList); // 재료가 될 수 있는 카드들의 리스트

        numSlots = cardDatas.Count;

        // 슬롯 생성
        for (int i = 0; i < numSlots; i++)
        {
            var slot = Instantiate(slotPrefab, transform);
            slot.transform.position = Vector3.zero;
            slot.transform.localScale = new Vector2(0, 0);
            slot.transform.DOScale(new Vector2(.5f, .5f), .2f).SetEase(Ease.OutBack);
            slots.Add(slot);
        }

        // 카드 데이터 정렬
        List<CardData> cardDataSorted = new();
        cardDataSorted.AddRange(cardDatas);

        // 내림차순으로 카드 정렬 
        cardDataSorted.Sort((a, b) =>
        {
            return new Sort().ByGrade(a, b);
        });

        cardDataSorted.Reverse();

        // 카드 Display
        for (int i = 0; i < numSlots; i++)
        {
            firstCardData = cardDataSorted[0];
            displayCardOnSlot.PutCardDataIntoSlot(cardDataSorted[i], slots[i].GetComponent<CardSlot>());
        }
    }

    public CardData GetFirstCardData()
    {
        return firstCardData;
    }

    public void ClearSlots()
    {
        int childCount = transform.childCount;
        if(childCount == 0) return;

        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            Destroy(child.gameObject);
        }

        firstCardData = null;
    }
    #endregion
}
