using System;
using System.IO;
using System.Linq;
using SaveSystem;
using UnityEngine;

namespace UI.MainMenu
{
    public abstract class Menu : MonoBehaviour
    {
        protected string _saveFile = " ";
        protected string _lastSave;

        private void Awake()
        {
            _lastSave = Directory.GetFiles(Path.Combine(Application.persistentDataPath))
                .Select(x => new FileInfo(x))
                .OrderByDescending(x => x.LastWriteTime)
                .FirstOrDefault()
                ?.ToString();
        }

        public abstract void Initialize();
        public virtual void Hide() => gameObject.SetActive(false);
        public virtual void Show() => gameObject.SetActive(true);
    }
}