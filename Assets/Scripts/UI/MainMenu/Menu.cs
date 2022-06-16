using System;
using System.IO;
using System.Linq;
using SaveSystem;
using UnityEngine;

namespace UI.MainMenu
{
    public abstract class Menu : MonoBehaviour
    {
        protected string SaveFile = " ";
        protected SavingHandler SavingHandler;
        private void Awake()
        {
            SavingHandler = FindObjectOfType<SavingHandler>();
        }

        public abstract void Initialize();
        public virtual void Hide() => gameObject.SetActive(false);
        public virtual void Show() => gameObject.SetActive(true);
    }
}