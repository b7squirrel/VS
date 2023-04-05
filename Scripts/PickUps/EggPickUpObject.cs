using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EggPickUpObject : Collectable, IPickUpObject
{
    [SerializeField] List<UpgradeData> upgradeToPick;
    int index;

    protected override void MoveToPlayer()
    {
        // 알은 자력으로 끌려오지 않는다. 끌려와 버리면 알을 얻었다는 것이 읽히지가 않음
    }

    public override void OnHitMagnetField(Vector2 direction)
    {
        // 알은 자력에 영향을 받지 않는다
    }
    
    // 플레이어에게 무기를 설치한 후 Egg 이벤트 화면으로 들어간다
    public void OnPickUp(Character character)
    {
        // 항목을 플레이어가 이미 가지고 있는지 체크
        // upgradeToPick을 remove하면서 탐색하면 에러가 생기므로 똑같은 리스트를 만들어서 반복탐색
        List<UpgradeData> checks = new List<UpgradeData>();
        checks.AddRange(upgradeToPick);

        foreach(UpgradeData item in checks)
        {
            if(character.GetComponent<Level>().HavingWeapon(item))
            upgradeToPick.Remove(item);
        }
            
        index = Random.Range(0, upgradeToPick.Count);
        Debug.Log("Error Test " + upgradeToPick.Count);
        character.GetComponent<Level>().GetWeapon(upgradeToPick[index]);
        
        GameManager.instance.eggPanelManager.EggPanelUP(upgradeToPick[index].newKidAnim);
    }
}
