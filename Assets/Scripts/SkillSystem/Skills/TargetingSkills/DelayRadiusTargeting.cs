using System;
using System.Collections;
using System.Collections.Generic;
using SkillSystem.MainComponents.Strategies;
using SkillSystem.SkillInfo;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SkillSystem.Skills.TargetingSkills
{
    [CreateAssetMenu(fileName = "Targeting", menuName = "Skill/Targeting/DelayClick", order = 0)]
    public class DelayRadiusTargeting : Targeting, ICollectable
    {
        [SerializeField] private Texture2D _cursorTexture;
        [SerializeField] private Vector2 _cursorHotspot;
        [SerializeField] private float _skillRadius;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _distanceToCastSkill;
        [SerializeField] private float _transitionDuration = 0.05f;

        [SerializeField] private GameObject _skillRadiusRenderer;
        [SerializeField] private GameObject _skillDistanceRenderer;

        private StarterAssetsInputs _user;
        private Animator _animator;
        private GameObject _skillRenderer;
        private GameObject _skillRadiusCast;

        public override void StartTargeting(SkillData skillData, Action finishedAttack, Action canceledAttack)
        {
            _user = skillData.GetUser.GetComponent<StarterAssetsInputs>();
            _animator = _user.GetComponent<Animator>();

            _user.StartCoroutine(WaitToCastSkill(skillData, finishedAttack, canceledAttack));
        }

        private IEnumerator WaitToCastSkill(SkillData skillData, Action finishedAttack, Action canceledAttack)
        {
            if (_skillRenderer == null)
                _skillRenderer = Instantiate(_skillRadiusRenderer, _user.transform);

            _skillRenderer.transform.localScale = new Vector3(_skillRadius * 1.5f, 0.1f, _skillRadius * 1.5f);

            if (_skillRadiusCast == null)
                _skillRadiusCast = Instantiate(_skillDistanceRenderer, _user.transform);

            _skillRadiusCast.transform.position = _user.transform.position;
            _skillRadiusCast.transform.localScale =
                new Vector3(_distanceToCastSkill * 1.5f, 0.1f, _distanceToCastSkill * 1.5f);

            Cursor.SetCursor(_cursorTexture, _cursorHotspot, CursorMode.Auto);

            while (true)
            {
                RaycastHit raycastHit;

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out raycastHit,
                    1000, _layerMask))
                {
                    _skillRenderer.transform.position = raycastHit.point;

                    var distanceToCast = Vector3.Distance(skillData.GetUser.transform.position, raycastHit.point);

                    if (Mouse.current.leftButton.isPressed)
                    {
                        if (distanceToCast > _distanceToCastSkill / 2)
                        {
                            yield return null;
                        }
                        else
                        {
                            DestroyRenderers(_skillRenderer, _skillRadiusCast);
                            while (Mouse.current.leftButton.isPressed)
                            {
                                yield return null;
                                skillData.GetUser.transform.LookAt((raycastHit.point));
                            }

                            skillData.MousePosition = raycastHit.point;
                            skillData.Targets = GetGameobjectsInRadius(raycastHit.point);
                            skillData.GetUser.transform.LookAt((raycastHit.point));

                            break;
                        }
                    }
                    else if (_user.CancelInput)
                    {
                        DestroyRenderers(_skillRenderer, _skillRadiusCast);
                        canceledAttack();

                        yield break;
                    }
                }

                yield return null;
            }

            finishedAttack();
        }

        private void DestroyRenderers(GameObject skillRenderer, GameObject skillRadiusCast)
        {
            Destroy(skillRenderer);
            Destroy(skillRadiusCast);
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        private IEnumerable<GameObject> GetGameobjectsInRadius(Vector3 point)
        {
            var hits = Physics.SphereCastAll(point, _skillRadius, Vector3.up, 100);

            foreach (var raycastHit in hits)
            {
                yield return raycastHit.collider.gameObject;
            }
        }

        public void AddData(Dictionary<string, float> data)
        {
            data.Add("AOE", 1);
            if(_distanceToCastSkill != 0)
                data.Add("Distance", _distanceToCastSkill);
            if(_skillRadius != 0)
                data.Add("Radius", _skillRadius);
        }
    }
}