using Entity;
using StatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Stats
{
    public class StatsDistributor : MonoBehaviour
    {
        [SerializeField] private StatsSystem.Stats _stat;
        [SerializeField] private TextMeshProUGUI _valueText;
        [SerializeField] private Button _minus;
        [SerializeField] private Button _plus;

        private StatsValueStore _statsValue;

        private void Update()
        {
            _minus.interactable = _statsValue.CanAssignPoints(_stat, -1);
            _plus.interactable = _statsValue.CanAssignPoints(_stat, 1);

            _valueText.text = _statsValue.GetProposedPoints(_stat).ToString();
        }

        private void Allocate(int points)
        {
            _statsValue.AssignPoints(_stat, points);
        }

        public void SetInfo(AliveEntity aliveEntity)
        {
            _statsValue = aliveEntity.GetStatsValueStore;
        
            _plus.onClick.AddListener((() => {Allocate(1);}));
            _minus.onClick.AddListener((() => {Allocate(-1);}));
        }
    }
}
