namespace SkillSystem.MainComponents.EffectApplyingSkills
{
    using System;
    using System.Collections;
    using DefaultNamespace.SkillSystem.SkillInfo;
    using DefaultNamespace.UI.ButtonClickable;
    using UnityEngine;

    [CreateAssetMenu (menuName = "Skill/Effect/SpawnEffect")]
    public class SpawnEffect : EffectApplying
    {
        [SerializeField] private GameObject _gameObjectToInstanciate;
        [SerializeField] private float _delay;
        [SerializeField] private float _timeToDestroy;

        private GameObject _instanciatedGameObject;
        public override void Effect(SkillData skillData, Action finished)
        {
            skillData.StartCoroutine(StartInstantiating(skillData));
        }

        private IEnumerator StartInstantiating(SkillData skillData)
        {
            yield return new WaitForSeconds(_delay);

            if(_instanciatedGameObject != null) yield break;
            _instanciatedGameObject =
                Instantiate(_gameObjectToInstanciate, skillData.MousePosition, Quaternion.identity);
            
            Destroy(_instanciatedGameObject, _timeToDestroy);
        }
    }
}