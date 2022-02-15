using System;
using System.Collections;
using Entity;
using StatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Stats
{
    public class HealthBarEntity : MonoBehaviour
    {
        public static HealthBarEntity Instance { get; private set; }
        
        [SerializeField] private Image _foreground;
        [SerializeField] private float _updateSpeedSeconds = 0.5f;
        [SerializeField] private TextMeshProUGUI _name;

        public Health Health { get; set; }

        private void Awake()
        {
            Instance = this;
            HideHealth();
        }

        public void ShowHealth(AliveEntity enemy)
        {
            Health = enemy.GetHealth;
            _name.text = enemy.name;
            gameObject.SetActive(true);
            Health.OnHealthPctChanged += OnHealthPctChanged;
            Health.OnDie += HideHealth;
        }

        public void HideHealth()
        {
            gameObject.SetActive(false);
            if(Health != null)
                Health.OnHealthPctChanged -= OnHealthPctChanged;
                
            Health = null;
        }

        private void OnHealthPctChanged(float pct)
        {
            StartCoroutine(ChangeToPct(pct));
        }

        private IEnumerator ChangeToPct(float pct)
        {
            float preChangePct = _foreground.fillAmount;
            float elapsed = 0f;

            while (elapsed < _updateSpeedSeconds)
            {
                elapsed += Time.deltaTime;
                _foreground.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / _updateSpeedSeconds);
                yield return null;
            }

            _foreground.fillAmount = pct;
        }
    }
}
