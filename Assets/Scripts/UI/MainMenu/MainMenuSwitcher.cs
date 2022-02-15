using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.MainMenu
{
    public class MainMenuSwitcher : MonoBehaviour
    {
        private static MainMenuSwitcher _instance;
        [SerializeField] private Menu _startingMenu;
        [SerializeField] private Menu[] _menus;
        
        
        private Menu _currentMenu;
        private readonly Stack<Menu> _history = new Stack<Menu>();
        private void Awake()
        {
            _instance = this;
        }

        private void Start()
        {
            foreach (var menu in _menus)
            {
                menu.Initialize();
                menu.Hide();
            }

            if (_startingMenu != null)
            {
                Show(_startingMenu, true);
            }
        }

        public static void Show<T>(bool remember = true) where  T : Menu
        {
            foreach (var menu in _instance._menus)
            {
                if (menu is T)
                {
                    if (_instance._currentMenu != null)
                    {
                        if (remember)
                        {
                            _instance._history.Push(_instance._currentMenu);
                        }
                        
                        _instance._currentMenu.Hide();
                    }
                    
                    menu.Show();
                    _instance._currentMenu = menu;
                }
            }
        }

        private static void Show(Menu menu, bool remember = true)
        {
            if (_instance._currentMenu != null)
            {
                if (remember)
                {
                    _instance._history.Push(_instance._currentMenu);
                }
                
                _instance._currentMenu.Hide();
            }
            
            menu.Show();
            _instance._currentMenu = menu;
        }

        public static void ShowLast()
        {
            if (_instance._history.Count != 0)
            {
                Show(_instance._history.Pop(), false);
            }
        }
    }
}