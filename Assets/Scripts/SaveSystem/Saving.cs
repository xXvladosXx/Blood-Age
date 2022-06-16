using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SceneSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaveSystem
{
    public class Saving : MonoBehaviour
    {
        public void Save(string saveFile)
        {
            Dictionary<string, object> capturedStates = LoadFile(saveFile);
            CaptureState(capturedStates);
            SaveFile(saveFile, capturedStates);
        }

        private void CaptureState(Dictionary<string, object> capturedStates)
        {
            foreach (SavableEntity saveableEntity in FindObjectsOfType<SavableEntity>())
            {
                capturedStates[saveableEntity.GetUniqueIdentifier()] = saveableEntity.CaptureState();
            }

            capturedStates["SceneIndexToLoad"] = SceneManager.GetActiveScene().buildIndex;
        }

        private void SaveFile(string saveFile, object captureState)
        {
            string path = GetPathFromSaveFile(saveFile);

            Type type = captureState.GetType();
            print("Saving tp " + path + " " + saveFile);

            using (FileStream fileStream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                if (!type.IsSerializable) return;

                formatter.Serialize(fileStream, captureState);
            }
        }

        public void Load(string saveFile)
        {
            RestoreState(LoadFile(saveFile));
        }

        private Dictionary<string, object> LoadFile(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            if (!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }

            using (FileStream fileStream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Dictionary<string, object>) formatter.Deserialize(fileStream);
            }
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            foreach (SavableEntity saveableEntity in FindObjectsOfType<SavableEntity>())
            {
                string uniqueID = saveableEntity.GetUniqueIdentifier();

                if (state.ContainsKey(uniqueID))
                {
                    saveableEntity.RestoreState(state[uniqueID]);
                }
            }
        }

        public IEnumerator LoadScene(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            int buildIndex = GetSceneIndex(saveFile);

            if (buildIndex != SceneManager.GetActiveScene().buildIndex)
            {
                LevelLoader.Instance.LoadScene(buildIndex);
                yield return null;
            }

            RestoreState(state);
        }

        public bool IsInCurrentScene(string saveFile) => GetSceneIndex(saveFile) == SceneManager.GetActiveScene().buildIndex;

        public int GetSceneIndex(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            if (state.ContainsKey("SceneIndexToLoad"))
            {
                int buildIndex = (int) state["SceneIndexToLoad"];

                return buildIndex;
            }

            return -1;
        }

        public IEnumerable<string> SavesList()
        {
            foreach (var save in Directory.EnumerateFiles(Application.persistentDataPath))
            {
                if (Path.GetExtension(save) == ".sav")
                    yield return Path.GetFileNameWithoutExtension(save);
            }
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}