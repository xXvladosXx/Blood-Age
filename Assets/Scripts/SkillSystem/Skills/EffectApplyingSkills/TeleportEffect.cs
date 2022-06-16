using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DefaultNamespace.SkillSystem.SkillInfo;
using Entity;
using SkillSystem.MainComponents.Strategies;
using SkillSystem.SkillInfo;
using StateMachine;
using States;
using UnityEngine;

namespace SkillSystem.Skills.EffectApplyingSkills
{
    [CreateAssetMenu(menuName = "Skill/Effect/TeleportEffect")]
    public class TeleportEffect : EffectApplying, ICollectable
    {
        [SerializeField] private GameObject _teleportEffect;
        [SerializeField] private float _delay;
        [SerializeField] private float _dalayToTeleport;
        
        public override void Effect(SkillData skillData, Action cancel, Action finished)
        {
            skillData.StartCoroutine(Teleport(skillData, skillData.MousePosition));
        }
        private IEnumerator Teleport(SkillData skillData, Vector3 position)
        {
            yield return new WaitForSeconds(_delay);
            var mesh = skillData.GetUser.GetComponentInChildren<CharacterMesh>();
            mesh.gameObject.SetActive(false);

            var capsuleCollider = skillData.GetUser.GetComponent<CapsuleCollider>();
            capsuleCollider.enabled = false;

            var userPosition = skillData.GetUser.transform.position;
            var particlesStart = Instantiate(_teleportEffect, 
            new Vector3(userPosition.x,
                capsuleCollider.height / 2, 
                userPosition.z), Quaternion.identity);
            
            Destroy(particlesStart, 1);

            yield return new WaitForSeconds(_dalayToTeleport);
            userPosition = position;
            skillData.GetUser.transform.position = userPosition;
            skillData.GetUser.GetComponent<Movement>().StartMoveTo(position, 1f);
            
            mesh.gameObject.SetActive(true);
            
            var particlesEnd = Instantiate(_teleportEffect, 
                new Vector3(userPosition.x,
                    capsuleCollider.height / 2, 
                    userPosition.z), Quaternion.identity);
            
            capsuleCollider.enabled = true;

            Destroy(particlesEnd, 1);
        }

        public void AddData(Dictionary<string, StringBuilder> data)
        {
            return;
        }

        
    }
}