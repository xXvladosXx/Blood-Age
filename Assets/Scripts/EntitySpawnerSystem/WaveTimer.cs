using System.Collections;
using TMPro;
using UnityEngine;

namespace EntitySpawnerSystem
{
    public class WaveTimer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        private void Start()
        {
            EntitySpawner.Instance.OnWaveStartSpawn += ShowText;
        }

        private void ShowText(int time)
        {
            _text.text = $"GET READY FOR THE NEXT WAVE {time}";
            _text.enabled = true;
            StartCoroutine(DisableText());
        }

        private IEnumerator DisableText()
        {
            yield return new WaitForSeconds(EntitySpawner.Instance.GetTimeBetweenWaves);

            _text.enabled = false;
        }

        private void OnDisable()
        {
            EntitySpawner.Instance.OnWaveStartSpawn -= ShowText;
        }
    }
}
