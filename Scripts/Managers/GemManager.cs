using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 현재 화면 안에 있는 Gem의 갯수를 세고 
// 최대치라면 더 이상 젬을 pool에서 빼오지 않고 경험치만 기존 보석에 합쳐주기

// 보석의 collectable(GemPickupObject가 상속)의 isVisible이 참이 될때마다 
// GemManager의 gemsVisible에 추가
// 보석이 disable되면 GemManager의 gemsVisible에서 제거

// poolManager에서 보석을 생성할 때마다 gemsVisible 갯수가 최대치인지 체크해서 
// 최대치이면 보석을 생성하지 않고 경험치를 기존 보석에 더해주는 MergeExp를 실행 
public class GemManager : MonoBehaviour
{
    [SerializeField] int MaxGemNumbers;
    public int GemNumbers {get; private set; }
    [SerializeField] List<Transform> gems;
    [SerializeField] List<Transform> gemsVisible;
    string gemPoolingKey;
    Character character;

    [Header("Feedback")]
    [SerializeField] AudioClip gemPickup_A;

    void Awake()
    {
        gemsVisible = new List<Transform>();
        character = FindObjectOfType<Character>();
    }

    public void OnPoolingGem(Transform gemCreated)
    {
        gems.Add(gemCreated);
    }

    public void AddVisibleGemToList(Transform gemVisible)
    {
        bool result = gemsVisible.Exists(x => x == gemVisible);
        if (result)
            return;
        gemsVisible.Add(gemVisible);
    }

    public void RemoveVisibleGemFromList(Transform gemVisible)
    {
        gemsVisible.Remove(gemVisible);
    }
    
    public bool IsVisibleGemMax()
    {
        if (gemsVisible.Count >= MaxGemNumbers)
            return true;
        return false;
    }

    public void PutExpToPlayer(int exp)
    {
        PlayGemSound();
        character.level.AddExperience(exp);
    }

    public void MergeExp(int exp)
    {
        int index = UnityEngine.Random.Range(0, gemsVisible.Count);
        gemsVisible[index].GetComponent<GemPickUpObject>().ExpAmount += exp;

        //temp
        gemsVisible[index].GetComponent<Collectable>().TempWhite();
    }

    public List<Transform> GetGemVisible()
    {
        if (gemsVisible == null || gemsVisible.Count == 0)
            return null;
        return gemsVisible;
    }

    public void PlayGemSound()
    {
        SoundManager.instance.Play(gemPickup_A);
    }
}
