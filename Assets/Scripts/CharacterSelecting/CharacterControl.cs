using System;
using StatsSystem;
using UnityEngine;

namespace CharacterSelecting
{
    public class CharacterControl : MonoBehaviour
    {
        [SerializeField] private Class _class;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _timeRemaining = 1;
        [SerializeField] private GameObject _selectingCircle;

        private Vector3 _startPosition;
        private bool _startCountForward;

        public Class GetClass => _class;
        public GameObject GetSelectingCircle => _selectingCircle; 
        
        private static readonly int Forw = Animator.StringToHash("Forw");
        private static readonly int Clicked = Animator.StringToHash("Clicked");
        private static readonly int Backward = Animator.StringToHash("Backward");

        private void Awake()
        {
            _startPosition = transform.position;
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            if (!CharacterSelectManager.Instance.CharacterControls.Contains(this))
            {
                CharacterSelectManager.Instance.CharacterControls.Add(this);
            }
        }

        private void Update()
        {
            if (_startCountForward)
            {
                _timeRemaining -= Time.deltaTime;

                if (_timeRemaining <= 0)
                {
                    _timeRemaining = 1;
                    _startCountForward = false;
                    _animator.SetBool(Forw, false);
                    PlayCharacterAnimationBackward();
                }
            }
        }

        public void PlayCharacterAnimationForward()
        {
            _animator.SetBool(Backward, false);
            _animator.SetBool(Forw, true);
            _startCountForward = true;
        }
        
        private void PlayCharacterAnimationBackward()
        {
            _animator.SetBool(Backward, true);
        }
    }
}
