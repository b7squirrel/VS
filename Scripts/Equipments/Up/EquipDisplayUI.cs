using UnityEngine;
using UnityEngine.UI;

public class EquipDisplayUI : MonoBehaviour
{
    [SerializeField] Transform cardBaseContainer; // 등급 5개 
    [SerializeField] Transform starContainer;
    [SerializeField] protected TMPro.TextMeshProUGUI Title;
    [SerializeField] protected TMPro.TextMeshProUGUI Level;
    [SerializeField] protected GameObject starPrefab;
    GameObject[] stars;
    [SerializeField] Animator charImage;
    [SerializeField] Animator[] EquipmentImages;
    [SerializeField] Image HeadImage;
    [SerializeField] Image ChestImage;
    [SerializeField] Image FaceImage;
    [SerializeField] Image HandImage;
    [SerializeField] TMPro.TextMeshProUGUI atk, hp;
    [SerializeField] CardsDictionary cardDictionary;
    [SerializeField] GameObject atkLabel, hpLabel;
    [SerializeField] GameObject button;

    public void SetWeaponDisplay(CardData charCardData, OriAttribute currentAttr)
    {
        // 별과 카드 색깔
        cardBaseContainer.gameObject.SetActive(true);

        int intGrade = new Convert().GradeToInt(charCardData.Grade);
        SetNumStar(intGrade + 1);

        for (int i = 0; i < 5; i++)
        {
            cardBaseContainer.GetChild(i).gameObject.SetActive(false);
        }
        cardBaseContainer.GetChild(intGrade).gameObject.SetActive(true);

        // 카드 이름 텍스트
        Title.text = charCardData.Name;
        // 카드 레벨 텍스트
        Level.text = "LV1";

        // 오리 이미지
        charImage.gameObject.SetActive(true);
        WeaponData wd = cardDictionary.GetWeaponItemData(charCardData).weaponData;
        // charImage.sprite = wd.charImage;
        charImage.runtimeAnimatorController = wd.Animators.CardImageAnim;

        atk.text = currentAttr.Atk.ToString();
        hp.text = currentAttr.Hp.ToString();

        button.SetActive(true);
    }
    public void SetEquipmentDisplay(CardData itemCardData, bool isAdding)
    {
        Item data = cardDictionary.GetWeaponItemData(itemCardData).itemData;
        int index = new Convert().EquipmentTypeToInt(itemCardData.EquipmentType);

        if (isAdding)
        {
            EquipmentImages[index].gameObject.SetActive(true);

            EquipmentImages[index].runtimeAnimatorController = data.CardItemAnimator.CardImageAnim;
            RestartAnim();
        }
        else
        {
            EquipmentImages[index].gameObject.SetActive(false);
        }

        button.SetActive(true);
    }
    void RestartAnim()
    {
        for (int i = 0; i < EquipmentImages.Length; i++)
        {
            if (EquipmentImages[i].gameObject.activeSelf)
                EquipmentImages[i].Rebind();
        }
        charImage.Rebind();
    }

    protected virtual void SetNumStar(int numStars)
    {
        if (stars == null)
        {
            // 5개 만들어서 비활성화
            stars = new GameObject[5];
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i] = Instantiate(starPrefab, starContainer);
                stars[i].SetActive(false);
            }
        }

        // 등급만큼 별 활성화하고 별리스트에 넣기
        for (int i = 0; i < numStars; i++)
        {
            stars[i].SetActive(true);
        }
    }

    public void OffDisplay()
    {
        // 카드 Base Container 비활성화
        cardBaseContainer.gameObject.SetActive(false);

        // 별 비활성화
        if (stars != null)
        {
            for (int i = 0; i < stars.Length; i++)
            {
                if (stars[i].activeSelf)
                    stars[i].SetActive(false);
            }
        }

        // 카드 레벨 텍스트
        Level.text = "";
        Title.text = "";

        atkLabel.SetActive(false);
        hpLabel.SetActive(false);
        // charImage.color = new Color(1, 1, 1, 0);
        for (int i = 0; i < EquipmentImages.Length; i++)
        {
            EquipmentImages[i].gameObject.SetActive(false);
        }
        charImage.gameObject.SetActive(false);

        button.SetActive(false);
    }
    public void OnDisplay(CardData cardData)
    {
        atkLabel.SetActive(true);
        hpLabel.SetActive(true);
        // charImage.color = new Color(1, 1, 1, 1);
    }
}
