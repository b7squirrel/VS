using System.Collections.Generic;
using UnityEngine;

public class DropOnDestroy : MonoBehaviour
{
    [SerializeField] List<GameObject> dropItemPrefab;
    [SerializeField][Range(0f, 1f)] float chance = 1f;
    [SerializeField] int exp;
    [SerializeField] int hp;

    bool isQuiting;

    void OnApplicationQuit()
    {
        isQuiting = true;
    }

    public void CheckDrop()
    {
        if (isQuiting) return;

        if (dropItemPrefab.Count <= 0)
        {
            Debug.LogWarning("DropOnDestory, dropItemPrefab 리스트가 비어 있습니다.");
            return;
        }

        bool isGem = false;

        if (Random.value < chance)
        {
            GameObject toDrop = dropItemPrefab[Random.Range(0, dropItemPrefab.Count)];
            if (toDrop == null)
            {
                Debug.LogWarning("DropOnDestroy, dropItemPrefab이 null입니다.");
                return;
            }

            if (GetComponent<Enemy>() != null)
            {
                exp = GetComponent<Enemy>().ExperienceReward;
            }

            if(toDrop.GetComponent<Collectable>() != null)
            {
                isGem = toDrop.GetComponent<Collectable>().IsGem;
            }
            Debug.Log("스폰시작");
            SpawnManager.instance.SpawnObject(transform.position, toDrop, isGem, exp);
        }
    }
}
