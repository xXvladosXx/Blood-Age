using System;
using System.Collections.Generic;
using System.Linq;
using Entity;
using UI.Inventory;
using UI.Tooltip;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class CanvasController : MonoBehaviour
    {
        public static CanvasController Instance { get; private set; }

        [SerializeField] private AliveEntity _aliveEntity;
        [SerializeField] private Canvas _canvas;

        private StarterAssetsInputs _userInputs;
        private List<UserInterface> _userInterfaces;

        private List<PanelEnabler> _uiPanelEnablers;
        private PanelEnabler _currentPanelEnabler;

        private bool _itemDrag;
        private bool _interfaceEnter;
        private bool _canEnableInterface;


        private void Awake()
        {
            Instance = this;
            _canEnableInterface = true;
            _userInterfaces = _canvas.GetComponentsInChildren<UserInterface>().ToList();
            _uiPanelEnablers = _canvas.GetComponentsInChildren<PanelEnabler>().ToList();
            _userInputs = _aliveEntity.GetComponent<StarterAssetsInputs>();
        }

        private void Start()
        {
            MouseData mouseData = new MouseData();
            _aliveEntity.GetHealth.OnTakeHit += HideAllUIs;
            _aliveEntity.GetHealth.OnDie += DisableInput;

            foreach (var userInterface in _userInterfaces)
            {
                userInterface.SetMouseData(mouseData);
                userInterface.OnItemDrag += () => _itemDrag = true;
                userInterface.OnItemPlace += () => _itemDrag = false;
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
        }

        private void DisableInput()
        {
            _canEnableInterface = false;
        }

        public void HideAllUIs(AliveEntity obj)
        {
            if (_currentPanelEnabler != null)
            {
                _currentPanelEnabler.HideInterface();
                _currentPanelEnabler = null;
            }
        }

        
        private void UIManagerOnChange(PanelEnabler panelEnabler)
        {
            if(!_canEnableInterface) return;
            
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

            if (_currentPanelEnabler != panelEnabler)
            {
                _currentPanelEnabler.HideInterface();
                _currentPanelEnabler = panelEnabler;
                panelEnabler.ShowInterface();
            }
        }

        private void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame && _currentPanelEnabler != null)
            {
                _currentPanelEnabler.HideInterface();
                _currentPanelEnabler = null;
            }
        
            if (_interfaceEnter || _itemDrag)
            {
                _userInputs.enabled = false;
            }
            else
            {
                _userInputs.enabled = true;
            }
        }
    }
}