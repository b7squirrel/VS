using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotUpCardUI : MonoBehaviour
{
    [SerializeField] UpgradeSuccessUI upgradeSuccessUI;

    [SerializeField] GameObject halo;

    [SerializeField] Animator animUpSlot;
    [SerializeField] Animator animMatSlot;
    [SerializeField] Animator animPlus;
    RectTransform upSlot, matSlot;
    RectTransform upCard, matCard;

    void Start()
    {
        GetComponent<SlotUpCard>().OnCardAcquiredOnUpSlotUI += upCardAcquiredUI;
        GetComponent<SlotUpCard>().OnCardAcquiredOnMatSlotUI += matCardAcquiredUI;
        GetComponent<SlotUpCard>().OnRefreshUI += refreshUpSlotUI;
        GetComponent<SlotUpCard>().OnUpdateUI += UpdateUI;
    }

    void UpdateUI()
    {
        if (upCard != null && upSlot != null)
        {
            upCard.position = upSlot.position;
        }

        if(matCard != null & matSlot != null)
        {
            matCard.position = matSlot.position;
        }
    }

    void OnEnable()
    {
        upgradeSuccessUI.gameObject.SetActive(false);
        animUpSlot.gameObject.SetActive(true);
        animMatSlot.gameObject.SetActive(false);
        animPlus.gameObject.SetActive(false);
        halo.SetActive(false);
    }

    void upCardAcquiredUI(Card card)
    {
        upCard = card.GetComponent<RectTransform>();
        upSlot = animUpSlot.GetComponent<RectTransform>();

        halo.SetActive(true);
        animUpSlot.SetTrigger("HavingCard");
        animMatSlot.gameObject.SetActive(true);
        animMatSlot.SetTrigger("IntoInit");
        animPlus.gameObject.SetActive(true);
        animPlus.SetTrigger("PlusUp");
    }

    void matCardAcquiredUI(Card card)
    {
        matCard = card.GetComponent<RectTransform>();
        matSlot = animMatSlot.GetComponent<RectTransform>();
    }

    void refreshUpSlotUI()
    {
        halo.SetActive(false);
        animUpSlot.SetTrigger("Canceld");
        animMatSlot.SetTrigger("Canceled");
        animPlus.SetTrigger("PlusDown");
        upSlot = null;
        upCard = null;
        matSlot = null;
        matCard = null;
    }
}
