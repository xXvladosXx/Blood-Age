using System;
using System.Collections.Generic;
using System.Linq;
using StatsSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace CharacterSelecting
{
    public class CharacterSelectManager : MonoBehaviour
    {
        public static CharacterSelectManager Instance { get; private set; }

        [SerializeField] private SelectedCharacterData _characterSelect;
        [SerializeField] private CharacterSelectLight _characterSelectLight;
        [SerializeField] private CharacterHoverLight _characterHoverLight;
        [SerializeField] private GameObject _selectingCircle;

        private Class _selectedClass;
        private Ray _ray;
        private RaycastHit _hit;
        private Camera _camera;
        private CharacterControl _characterControl;
        private List<CharacterControl> _characters = new List<CharacterControl>();
        private GameObject circle = null;

        public List<CharacterControl> CharacterControls
        {
            get => _characters;
            set => _characters = value;
        }

        private void Awake()
        {
            Instance = this;

            _selectedClass = Class.None;
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            _ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(_ray, out _hit))
            {
                if (_hit.collider.TryGetComponent(out CharacterControl characterControl))
                {
                    print("Hit");
                    _characterHoverLight.SetLight(characterControl);
                }
                else
                {
                    _characterHoverLight.ResetLight();
                }
            }

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                if (_hit.collider
                    .TryGetComponent(out CharacterControl characterControl))
                {
                    
                    
                    if (characterControl != _characterControl)
                    {
                        _characterControl = characterControl;
                        _characterControl.PlayCharacterAnimationForward();
                        
                        Destroy(circle);
                        circle = Instantiate(characterControl.GetSelectingCircle, characterControl.transform.position,
                            Quaternion.identity);
                        
                        var characterSelectLight = _characterSelectLight.transform;
                        characterSelectLight.position = _characterHoverLight.transform.position;
                        characterSelectLight.parent = _characterControl.transform;

                        _characterHoverLight.ResetLight();
                        _characterSelectLight.GetLight.enabled = true;
                        _characterSelect.Class = characterControl.GetClass;
                    }
                    else
                    {
                        Destroy(circle);
                    }
                }
                else
                {
                    Destroy(circle);
                    _characterControl = null;
                    _characterSelect.Class = Class.None;
                    _characterSelectLight.GetLight.enabled = false;
                }
            }
        }
    }
}

