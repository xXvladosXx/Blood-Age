namespace SkillSystem.MainComponents.EffectApplyingSkills
{
    using System;
    using System.Collections;
    using DefaultNamespace.Entity;
    using DefaultNamespace.SkillSystem.SkillInfo;
    using DefaultNamespace.UI.ButtonClickable;
    using SkillSystem.MainComponents.TargetingSkills;
    using UnityEngine;
    using UnityEngine.InputSystem;

    [CreateAssetMenu(menuName = "Skill/Effect/TeleportEffect")]
    public class TeleportEffect : EffectApplying
    {
        [SerializeField] private GameObject _teleportEffect;
        [SerializeField] private float _delay;
        public override void Effect(SkillData skillData, Action finished)
        {
            skillData.StartCoroutine(Teleport(skillData, skillData.MousePosition));

            finished();
        }

        private IEnumerator Teleport(SkillData skillData, Vector3 position)
        {
            yield return new WaitForSeconds(_delay);
            var mesh = skillData.GetUser.GetComponentInChildren<CharacterMesh>();
            mesh.gameObject.SetActive(false);
            var particlesStart = Instantiate(_teleportEffect, skillData.GetUser.transform.position, Quaternion.identity);
            Destroy(particlesStart, 1);

            skillData.GetUser.transform.position = position;
            skillData.GetUser.GetComponent<Movement>().StartMoveTo(position, 1f);
            
            mesh.gameObject.SetActive(true);
            var particlesEnd = Instantiate(_teleportEffect, position, Quaternion.identity);
            Destroy(particlesEnd, 1);
        }
    }
}