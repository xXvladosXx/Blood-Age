using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SceneManagement;
using StateMachine;
using StateMachine.PlayerStates;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace SaveSystem
{
    public class SavingHandler : MonoBehaviour
    {
        public static SavingHandler Instance { get; private set; }
        
        private const string _defaultSaveFile = "QuickSave";

        private void Awake()
        {
            Instance = this;
        }

        public void StartNewGame(string saveFile)
        {
            StartCoroutine(LoadStartScene(saveFile));
        }

        public void ContinueGame(string saveFile = _defaultSaveFile)
        {
            saveFile = Path.GetFileNameWithoutExtension(saveFile);

            StartCoroutine(LoadScene(saveFile));
        }

        public void LoadGame(string saveFile = _defaultSaveFile)
        {
            StartCoroutine(LoadScene(saveFile));
        }

        private IEnumerator LoadScene(string saveFile)
        {
            DontDestroyOnLoad(gameObject);
            yield return GetComponent<Saving>().LoadScene(saveFile);
        }

        private IEnumerator LoadStartScene(string saveFile)
        {
            DontDestroyOnLoad(gameObject);
            yield return SceneManager.LoadSceneAsync(sceneBuildIndex: 0);
            Save(saveFile);
        }

        private void Update()
        {
            if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            {
                Save(_defaultSaveFile);
            }

            if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            {
                Load(_defaultSaveFile);
            }
        }

        public void Load(string saveFile)
        {
            GetComponent<Saving>().Load(saveFile);
        }

        public void Save(string saveFile)
        {
            if(!FindObjectOfType<PlayerStateManager>().CanSave())
                return;
            
            GetComponent<Saving>().Save(saveFile);
        }

        public IEnumerable<string> SaveList()
        {
            return GetComponent<Saving>().SavesList();
        }
    }
}