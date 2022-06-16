using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SceneSystem;
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
        
        public string GetLastSave => Directory.GetFiles(Path.Combine(Application.persistentDataPath))
            .Select(x => new FileInfo(x))
            .OrderByDescending(x => x.LastWriteTime)
            .FirstOrDefault()
            ?.ToString();

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void StartNewGame(string saveFile)
        {
            LevelLoader.Instance.StartFading();
            StartCoroutine(LoadStartScene(saveFile));
        }

        public void ContinueGame(string saveFile = _defaultSaveFile)
        {
            LevelLoader.Instance.StartFading();
            saveFile = Path.GetFileNameWithoutExtension(saveFile);

            StartCoroutine(LoadScene(saveFile));
        }

        public void LoadGame(string saveFile = _defaultSaveFile)
        {
            if(!GetComponent<Saving>().IsInCurrentScene(saveFile))
                LevelLoader.Instance.StartFading();

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
            yield return SceneManager.LoadSceneAsync(sceneBuildIndex: 3);
            Save(saveFile);
        }

        public void Load(string saveFile)
        {
            LevelLoader.Instance.StartFading();

            GetComponent<Saving>().Load(saveFile);
        }

        public void Save(string saveFile)
        {
            GetComponent<Saving>().Save(saveFile);
        }

        public IEnumerable<string> SaveList()
        {
            return GetComponent<Saving>().SavesList();
        }

        public void OverwriteGame(string save)
        {
            GetComponent<Saving>().Save(save);
        }

        public int GetIntFromPlayerPrefs(string id)
        {
            return PlayerPrefs.GetInt(id);
        }

        public float GetFloatFromPlayerPrefs(string id)
        {
            return PlayerPrefs.GetFloat(id,1);
        }
        public void SetIntToPlayerPrefs(string id, int value)
        {
            PlayerPrefs.SetInt(id, value);
            PlayerPrefs.Save();
        }
        
        public void SetFloatToPlayerPrefs(string id, float value)
        {
            PlayerPrefs.SetFloat(id, value);
            PlayerPrefs.Save();
        }
    }
}