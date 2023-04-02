using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadStageData : MonoBehaviour
{
    public TextAsset text;
    ReadData readData;
    string[,] data;
    List <StageEvent> stageEvents;
    StageEnemyData stageEnemyData;

    void Awake()
    {
        
    }

    StageEventType GetStageEventType(string stageEventType)
    {
        if (stageEventType == "Enemy")
            return StageEventType.SpawnEnemy;
        if (stageEventType == "EnemyGroup")
            return StageEventType.SpawnEnemyGroup;
        if (stageEventType == "SubBoss")
            return StageEventType.SpawnSubBoss;
        if (stageEventType == "Boss")
            return StageEventType.SpawnEnemyBoss;
        if (stageEventType == "Object")
            return StageEventType.SpawnObject;

        return StageEventType.WinStage;
    }

    EnemyData GetEnemyType(string enemyType)
    {
        if (enemyType == "LV1")
            return stageEnemyData.enemyData[0];
        if (enemyType == "LV1_SubBoss")
            return stageEnemyData.enemyData[1];
        if (enemyType == "LV2")
            return stageEnemyData.enemyData[2];
        if (enemyType == "LV2_SubBoss")
            return stageEnemyData.enemyData[3];
        if (enemyType == "LV3")
            return stageEnemyData.enemyData[4];
        if (enemyType == "LV3_SubBoss")
            return stageEnemyData.enemyData[5];
        if (enemyType == "LV4")
            return stageEnemyData.enemyData[6];
        if (enemyType == "LV4_SubBoss")
            return stageEnemyData.enemyData[7];
        if (enemyType == "Boss")
            return stageEnemyData.enemyData[8];
        if (enemyType == "Special_1")
            return stageEnemyData.enemyData[9];
        if (enemyType == "Special_2")
            return stageEnemyData.enemyData[10];
        if (enemyType == "Special_3")
            return stageEnemyData.enemyData[11];
        if (enemyType == "Special_4")
            return stageEnemyData.enemyData[12];
        if (enemyType == "Special_5")
            return stageEnemyData.enemyData[13];
        if (enemyType == "Special_6")
            return stageEnemyData.enemyData[14];
        return stageEnemyData.enemyData[0]; // 일단 채워넣었음
    }

    public List<StageEvent> GetStageEventsList()
    {
        readData = new ReadData();
        data = readData.GetText(text);
        stageEnemyData = GetComponent<StageEnemyData>();
        stageEvents = new List<StageEvent>();
        Debug.Log(data);
        int length = data.GetLength(0);

        for (int i = 0; i < length; i++)
        {
            StageEvent stageEvent = new StageEvent();
            stageEvent.eventType = GetStageEventType(data[i, 0]);
            stageEvent.time = int.Parse(data[i, 1]);
            stageEvent.enemyToSpawn = GetEnemyType(data[i, 2]);
            stageEvent.count = int.Parse(data[i, 3]);

            stageEvents.Add(stageEvent);
        }
        return stageEvents;
    }
}
