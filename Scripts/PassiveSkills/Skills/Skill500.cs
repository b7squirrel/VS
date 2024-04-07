﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill500 : MonoBehaviour, ISkill
{
    public int Name { get; set; } = 500;
    public float CoolDownTime { get; set; } = 5f;
    public int Grade { get; set; }
    public int EvoStage { get; set; }
    float skillCounter;
    float rate = .3f; // 등급, 스킬 레벨에 따라 얼마만큼 쿨타임에 영향을 미치게 할 지 정하는 비율

    [SerializeField] int defaultDamage;

    public void UseSkill()
    {
        if (skillCounter > new Equation().GetCoolDownTime(rate, Grade, EvoStage, CoolDownTime))
        {
            defaultDamage = new Equation().GetSkillDamage(rate, Grade, EvoStage, defaultDamage);

            Debug.Log($"Skill Damage {defaultDamage}");
        }
    }
}
