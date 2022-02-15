using Entity;
using UnityEngine;
using UnityEngine.InputSystem;

namespace States
{
    [CreateAssetMenu (menuName = "State/MouseFollow")]
    public class MouseRotationState : StateData
    {
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _speed;
        private AliveEntity _aliveEntity;
        private Camera _camera;

        public override void OnEnter(AnimatorState characterStateAnimator, Animator animator, AnimatorStateInfo stateInfo)
        {
            _aliveEntity = animator.GetComponent<AliveEntity>();
            _camera = Camera.main;
        }

        public override void UpdateAbility(AnimatorState characterStateAnimator, Animator animator, AnimatorStateInfo stateInfo)
        {
            RaycastHit raycastHit;

            if (Physics.Raycast(_camera.ScreenPointToRay(Mouse.current.position.ReadValue()), out raycastHit,
                    Mathf.Infinity, (1 << LayerMask.NameToLayer("Default") | (1 << LayerMask.NameToLayer("Entity")))))
            {
                if (raycastHit.collider.gameObject == _aliveEntity.gameObject)
                {
                    return;
                }
                Vector3 lTargetDir = raycastHit.point - _aliveEntity.transform.position;
                lTargetDir.y = 0.0f;
                    
                _aliveEntity.transform.rotation = Quaternion.RotateTowards(_aliveEntity.transform.rotation, 
                    Quaternion.LookRotation(lTargetDir), Time.time * _speed);
            }
        }

        public override void OnExit(AnimatorState characterStateAnimator, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }
    }
}