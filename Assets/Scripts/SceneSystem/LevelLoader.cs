using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SceneSystem
{
    public class LevelLoader : MonoBehaviour
    {
        public static LevelLoader Instance { get; private set; }
        [SerializeField] private Animator _transition;
        [SerializeField] private float _transitionTime;

        [SerializeField] private Canvas _loaderCanvas;
        [SerializeField] private Image _loadImageProgress;

        private float _target;
        
        private static readonly int Start = Animator.StringToHash("Start");
        
        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void StartFading()
        {
            StartCoroutine(LoadLevel());
        }

        private IEnumerator LoadLevel()
        {
            _transition.SetTrigger(Start);
            yield return new WaitForSeconds(_transitionTime);
        }

        public async void LoadScene(int buildIndex)
        {
            _target = 0;
            //_loadImageProgress.fillAmount = 0;
            
            var scene = SceneManager.LoadSceneAsync(buildIndex);
            scene.allowSceneActivation = false;
            do
            {
                await Task.Delay(100);
                _target = scene.progress;
            } while (scene.progress < 0.9f);

            scene.allowSceneActivation = true;
            //_loaderCanvas.gameObject.SetActive(false);
        }

        private void Update()
        {
            _loadImageProgress.fillAmount =
                Mathf.MoveTowards(_loadImageProgress.fillAmount, _target, 3 * Time.deltaTime);
        }
    }
}