using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Entity;
using SkillSystem;
using SkillSystem.MainComponents;
using UnityEngine;

public class SkillPanel : MonoBehaviour
{
    [SerializeField] private AliveEntity _aliveEntity;

    private List<ActiveSkill> _activeSkills = new List<ActiveSkill>();
    private void Awake()
    {
        foreach (var skillNode in _aliveEntity.GetComponent<SkillBuilder>().GetSkillNodes)
        {
            if (skillNode is ActiveSkill activeSkill)
            {
                _activeSkills.Add(activeSkill);
            }
        }

        foreach (var activeSkill in _activeSkills)
        {
            print(activeSkill.name);
        }
    }
}
