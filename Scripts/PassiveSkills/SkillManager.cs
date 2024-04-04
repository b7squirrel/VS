﻿using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어에 붙여서 사용. 
/// </summary>
public class SkillManager : MonoBehaviour
{
    ISkill[] skills;
    event Action onSkill;
    float cooldownTime;
    float skillTimeCounter;

    void Start()
    {
        skills = GetComponentsInChildren<ISkill>();
        Debug.Log("Number of skills = " + skills.Length);
        string skillName = GameManager.instance.startingDataContainer.GetSkillName();
        Init(skillName);
        Debug.Log("Skill Name = " + skillName);

    }
    void Update()
    {
        if (onSkill == null) return;

        if (skillTimeCounter > cooldownTime)
        {
            onSkill?.Invoke();
            skillTimeCounter = 0;
        }
        else
        {
            skillTimeCounter += Time.deltaTime;
        }
    }
    /// <summary>
    /// 이름으로 스킬을 찾아서 등록시킴
    /// </summary>
    public void Init(string _Name)
    {
        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i].Name == _Name)
            {
                cooldownTime = skills[i].CoolDownTime;
                onSkill += skills[i].UseSkill;
                return;
            }
        }
        Debug.Log("스킬이 없습니다.");
    }
}

