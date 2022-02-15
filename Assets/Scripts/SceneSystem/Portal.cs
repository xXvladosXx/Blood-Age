﻿using System.Collections;
using AttackSystem.Weapon;
using Entity;
using MouseSystem;
using SaveSystem;
using SceneManagement;
using UnityEngine;
using UnityEngine.AI;

namespace SceneSystem
{
    public class Portal : MonoBehaviour, IRaycastable
    {
        [SerializeField] private int _sceneToLoad = -1;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private DestinationPortal destinationPortal = DestinationPortal.A;

        private string _defaultSaveFile = "QuickSave";

        enum DestinationPortal
        {
            A,
            B,
            C
        }

        public void StartTransition()
        {
            StartCoroutine(WaitSceneToLoad());
        }

        IEnumerator WaitSceneToLoad()
        {
            DontDestroyOnLoad(gameObject);
            SavingHandler savingHandler = FindObjectOfType<SavingHandler>();
            savingHandler.Save(_defaultSaveFile);
//            LevelLoader.Instance.StartFading();

            yield return new WaitForSeconds(1f);
            yield return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_sceneToLoad);

            savingHandler.Load(_defaultSaveFile);

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);


            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;

            player.transform.position = otherPortal._spawnPoint.position;
            player.transform.rotation = otherPortal._spawnPoint.rotation;

            player.GetComponent<NavMeshAgent>().enabled = true;
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;

                if (portal.destinationPortal == destinationPortal)
                    return portal;
            }

            return null;
        }

        public CursorType GetCursorType()
        {
            return CursorType.PickUp;
        }

        public void ClickAction()
        {
        }

        public bool HandleRaycast(PlayerEntity player)
        {
            return true;
        }
    }
}