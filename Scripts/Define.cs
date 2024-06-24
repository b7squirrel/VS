﻿// Slot Action
using UnityEngine;

#region 메인 메뉴 관련
public enum SlotType { Field, Up, Mat, None };
public enum EquipSlotType { FieldOri, FieldEquipment, UpEquipment, None }
public enum LaunchSlotType { Field, Up, None }

// Card Slot
public enum TargetSlot { UpField, MatField, UpSlot, MatSlot } // Target Slot to move to when a slot is clicked on.

// Card Data
public enum CardType { Weapon, Item, none }
public enum Grade { Common, Rare, Epic, Unique, Legendary }
public enum EquipmentType { Head, Chest, Face, Hand, Ori }
public enum EssentialEquip { Head, Chest, Face, Hand, Essential }
public enum StartingMember { Zero, First, Second, Third, Forth, Fifth }
public enum DefaultItem { Default }
#endregion

#region 필드 관련
// 필드에서 플레이어의 위치
public enum Region
{
    None, TopLeft, TopRight, BottomLeft, BottomRight
}

public enum SpawnItem { enemy, subBoss, enemyGroup, bossSlime } // Stage Event Manager 에서 사용

public enum EnemyType { Melee, Ranged, Explode } // 적을 스폰할 때 어떤 타입의 적을 스폰할지 정하기 위해
#endregion

#region 음악 관련
public enum StageMusicType
{
    GhostAlley,
    GoGoGO,
    GoGoFasterFaster,
    WhereverAnliens,
    Temp
}
#endregion

#region 슬롯 관련
public enum Slots { CardSlot, EquipSlot, LaunchSlot }
public class MyGrade
{
    public static int Common = 0;
    public static int Rare = 1;
    public static int Epic = 2;
    public static int Unique = 3;
    public static int Legendary = 4;

    public static string[] mGrades = { "Common", "Rare", "Epic", "Unique", "Legendary" };

    public static Color[] GradeColors = new Color[]
    {
        new Color(.6f,.6f,.6f), // white
        new Color(1,0.8f,0), // yellow
        new Color(0,0.8f,1), // blue
        new Color(0.5f,1,0), // green
        new Color(.7f,0.3f,1) // purple
    };

    public static Color[] GradeGlowColors = new Color[]
    {
        new Color(1f,1f,1f,0.25f), // white
        new Color(1,0.9f,0.6f,0.25f), // yellow
        new Color(0.5f,0.9f,0.7f,0.25f), // blue
        new Color(1,0.9f,0.7f, 0.25f), // green
        new Color(1,0.5f,0.9f, 0.25f) // purple
    };
}
#endregion

#region 스킬 관련
public class Skills
{
    public static string[] SkillNames = new string[]
    {
        "Steel Body",
        "Sluggish Slumber",
        "Flash Damage",
        "Invincible Body",
        "Spicy Booster"
    };
    public static string[] SkillDescriptions = new string[]
    {
        "Your friends' body shield you from enemies.",
        "Temporarily slow down all enemies on the screen for a duration.",
        "Inflict periodic damage to all enemies on the screen.",
        "Grant periodic invincibility to the player.",
        "Periodically boost the attack power of both the player and allies."
    };
}

public class StaticValues
{
    public static int MaxLevel = 30;
    public static int MaxEvoStage = 3; // evo stage 0, 1, 2
    public static int MaxGrade = 5; // grade 0, 1, 2, 3, 4
    public static int MaxItemGrade = 3; // grade 0, 1, 2
    public static int MaxSkillNumbers = 5; // skill 1, 2, 3, 4, 5 : 3자리수의 백자리가 0이 될 수 없음
    public static int MaxEnemyNumbers;
    public static int MaxGemNumbers;

    public static float GemDropRate = .5f; // 보석 드롭 확률 60%
}
#endregion

#region 색깔 관련
public class Colors
{
    public static Color[] randomColors = 
        { Color.red, Color.yellow, Color.magenta, Color.white };
}
#endregion

#region 공식 관련
public class Equation
{
    public int GetDamage(int _originalDamage, int _damageBonus)
    {
        int damage = (int)(_originalDamage + (_originalDamage * _damageBonus / 100));
        return damage;
    }

    public int GetCriticalDamage(int _damage)
    {
        int criticalCoefficient = UnityEngine.Random.Range(5, 9);
        int criticalConstant = UnityEngine.Random.Range(1, 100);
        int _cDamage = (_damage * criticalCoefficient) + criticalConstant;
        return _cDamage;
    }

    public float GetCoolDownTime(float _rate, int _grade, int _evoStage, float _defaultCoolDownTime)
    {
        return _defaultCoolDownTime - (_rate * ((_grade + 4) + _evoStage));
    }

    public int GetSkillDamage(float _rate, int _grade, int _evoStage, float _defaultDamage)
    {
        return (int)(_defaultDamage * (int)(_rate * ((_grade + 4) + _evoStage)));
    }

    public float GetSkillDuration(float _rate, int _grade, int _evoStage, float _defaultDuration)
    {
        return _defaultDuration + (_rate * ((_grade + 4) + _evoStage));
    }

    public int GetSkillDamageBonus(float _rate, int _grade, int _evoStage, float _defaultDamageBonus)
    {
        return (int)(_defaultDamageBonus * (int)(8 * (((_grade + 1) * 1.5f) + _evoStage)));
    }

    public float GetSlowSpeedFactor(int _grade, int _evoStage)
    {
        int slownessFactor = 5;
        return .01f * (slownessFactor * ((_grade * 5) + (_evoStage * 2)) + 90f);
    }

    public Vector2 GetSpawnablePos(float _spawnConst, float _offset)
    {
        Vector2 position = new Vector2();
        float f = Random.value > .5f ? 1f : -1f;

        if (Random.value > .5f)
        {
            position.x = Random.Range(-_spawnConst + _offset, _spawnConst - _offset);
            position.y = f > 0 ? (_spawnConst * f) - _offset : (_spawnConst * f) + _offset;
        }
        else
        {
            position.y = Random.Range(-_spawnConst + _offset, _spawnConst - _offset);
            position.x = f > 0 ? (_spawnConst * f) - _offset : (_spawnConst * f) + _offset;
        }

        return position;
    }
    public bool IsOutOfRange(Vector2 posToCheck, float _spawnConst)
    {
        if (posToCheck.x > _spawnConst || posToCheck.x < -_spawnConst
            || posToCheck.y > _spawnConst || posToCheck.y < -_spawnConst)
        {
            return true;
        }
        return false;
    }
}
#endregion