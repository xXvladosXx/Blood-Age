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

        private Health Health { get; set; }

        private void Awake()
        {
            Instance = this;
            HideHealth();
        }

        public void ShowHealth(AliveEntity enemy)
        {
            Health = enemy.GetHealth;
            _foreground.fillAmount = Health.GetCurrentHealth / Health.GetMaxHealth;
            _name.text = enemy.SerializableClass.ToString();
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
            _foreground.fillAmount = pct;
        }

    }
}
