using System;
using System.Collections;
using AttackSystem.Weapon;
using Entity;
using MouseSystem;
using SaveSystem;
using UnityEngine;
using UnityEngine.AI;

namespace SceneSystem
{
    public class Portal : MonoBehaviour, IRaycastable
    {
        [SerializeField] private int _sceneToLoad = -1;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private DestinationPortal destinationPortal = DestinationPortal.A;
        [SerializeField] private bool _destroyOnAwake;

        private string _defaultSaveFile = "QuickSave";
        private bool _wasActive;

        enum DestinationPortal
        {
            A,
            B,
            C
        }

        private void Start()
        {
            gameObject.SetActive(false);
            if (_destroyOnAwake)
                Destroy(gameObject);
        }

        public void StartTransition(AliveEntity aliveEntity)
        {
            StartCoroutine(WaitSceneToLoad(aliveEntity));
        }

        IEnumerator WaitSceneToLoad(AliveEntity aliveEntity)
        {
            DontDestroyOnLoad(gameObject);
            
            SavingHandler.Instance.Save(_defaultSaveFile);
           
            yield return new WaitForSeconds(1f);
            yield return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_sceneToLoad);

            SavingHandler.Instance.Load(_defaultSaveFile);

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal, aliveEntity);
            SavingHandler.Instance.Save(_defaultSaveFile);
            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal, AliveEntity aliveEntity)
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

        public bool HandleRaycast(PlayerEntity player)
        {
            return true;
        }
    }
}