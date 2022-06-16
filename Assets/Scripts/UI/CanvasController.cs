using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Entity;
using PauseSystem;
using StateMachine;
using StateMachine.BaseStates;
using StateMachine.PlayerStates;
using UI.Inventory;
using UI.MainMenu;
using UI.Tooltip;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UI
{
    public class CanvasController : MonoBehaviour
    {
        [SerializeField] private AliveEntity _aliveEntity;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private GameObject _deathSceen;
        [SerializeField] private float _time;
        [SerializeField] private GameObject _characterData;

        private PlayerInputs _userInputs;
        private List<UserInterface> _userInterfaces;

        private List<PanelEnabler> _uiPanelEnablers;
        private PanelEnabler _currentPanelEnabler;
        private MainMenuSwitcher _mainMenuSwitcher;
        private bool _canEnableInterface;

        private bool IsPaused => ProjectContext.Instance.PauseManager.IsPaused;
        private void Awake()
        {
            _userInterfaces = _canvas.GetComponentsInChildren<UserInterface>().ToList();
            _uiPanelEnablers = _canvas.GetComponentsInChildren<PanelEnabler>().ToList();
            _mainMenuSwitcher = _canvas.GetComponentInChildren<MainMenuSwitcher>();
            _userInputs = _aliveEntity.GetComponent<PlayerInputs>();
            MakeUIInputPossible(true);

            
            if (_aliveEntity.GetComponent<IStateSwitcher>().GetCurrentState is IdleBaseState playerState)
            {
                playerState.OnAlive += OnDeathScreenDeactivate;
            }
        }

        
        private void Start()
        {
            MouseData mouseData = new MouseData();
            _aliveEntity.GetHealth.OnTakeHit += HideAllUIs;
            _aliveEntity.GetHealth.OnDie += (() =>
            {
                MakeUIInputPossible(false);
                DeactivateCharacterData();
                HideAllUIs(_aliveEntity);
                _deathSceen.SetActive(true);
                DOVirtual.DelayedCall(_time,() =>
                {
                    _mainMenuSwitcher.ShowInterface();
                    _currentPanelEnabler = _mainMenuSwitcher;
                });
            });

            foreach (var userInterface in _userInterfaces)
            {
                userInterface.SetMouseData(mouseData);
                userInterface.OnItemDrag += () =>
                {
                    _userInputs.DisableInput();
                    MakeUIInputPossible(false);
                };
                userInterface.OnItemPlace += () =>
                {
                    _userInputs.EnableInput();
                    MakeUIInputPossible(true);
                };
            }

            foreach (var view in _canvas.GetComponentsInChildren<Panel>())
            {
                view.Initialize(_aliveEntity);
            }
            
            var keyPanelEnablers = new List<PanelEnablerKey>();
            var staticPanelEnabler = new List<PanelEnablerStaticAction>();
            
            foreach (var panelEnabler in _uiPanelEnablers)
            {
                switch (panelEnabler)
                {
                    case PanelEnablerKey panelEnablerKey:
                        keyPanelEnablers.Add(panelEnablerKey);
                        break;
                    case PanelEnablerStaticAction panelEnablerStaticAction:
                        staticPanelEnabler.Add(panelEnablerStaticAction);
                        break;
                }
            }
            
            foreach (var panelEnabler in keyPanelEnablers)
            {
                panelEnabler.HideInterface();
                panelEnabler.OnUIChange += UIManagerOnChange;
            }
            
            foreach (var panelEnabler in staticPanelEnabler)
            {
                panelEnabler.HideInterface();
                panelEnabler.OnUIChange += UIManagerOnChange;
            }

            MakeUIInputPossible(GameZoneManager.Instance.GetGameZone == GameZoneManager.GameZone.Savezone);
        }

        private void MakeUIInputPossible(bool canEnable)
        {
            _canEnableInterface = canEnable;

            foreach (var uiPanelEnabler in _uiPanelEnablers)
            {
                uiPanelEnabler.MakeInputPossible(canEnable);
            }
        }

        public void HideAllUIs(AliveEntity obj)
        {
            if (_currentPanelEnabler != null)
            {
                _currentPanelEnabler.HideInterface();
                _currentPanelEnabler = null;
            }
        }

        
        public void UIManagerOnChange(PanelEnabler panelEnabler)
        {
            if(!_canEnableInterface) return;

            if (_currentPanelEnabler is PanelEnablerStaticAction && panelEnabler is PanelEnablerStaticAction && _currentPanelEnabler != panelEnabler)
            {
                _currentPanelEnabler.HideInterface();
                _currentPanelEnabler = panelEnabler;
                panelEnabler.ShowInterface();
                return;
            }
            
            if (_currentPanelEnabler == null)
            {
                _currentPanelEnabler = panelEnabler;
                panelEnabler.ShowInterface();
                return;
            }

            if (_currentPanelEnabler == panelEnabler)
            {
                panelEnabler.HideInterface();
                _currentPanelEnabler = null;
                return;
            }

            if (_currentPanelEnabler != panelEnabler && !(_currentPanelEnabler is PanelEnablerStaticAction))
            {
                _currentPanelEnabler.HideInterface();
                _currentPanelEnabler = panelEnabler;
                panelEnabler.ShowInterface();
            }
        }

        private void Update()
        {
            if(_aliveEntity.GetHealth.IsDead()) return;
            if(IsPaused) return;

            if (Keyboard.current.escapeKey.wasPressedThisFrame && _currentPanelEnabler == null)
            {
                _currentPanelEnabler = _mainMenuSwitcher;
                _mainMenuSwitcher.ShowInterface();
                _mainMenuSwitcher.SetStartMenu();
                return;
            }
            
            if (Keyboard.current.escapeKey.wasPressedThisFrame && _currentPanelEnabler != null && !(_currentPanelEnabler is PanelEnablerStaticAction) )
            {
                _currentPanelEnabler.HideInterface();
                _currentPanelEnabler = null;
            }
        }
        private void DeactivateCharacterData()
        {
            _characterData.SetActive(false);
        }

        private void OnDeathScreenDeactivate()
        {
            _characterData.SetActive(true);
            _deathSceen.gameObject.SetActive(false);
        }

    }
}