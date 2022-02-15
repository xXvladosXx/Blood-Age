using Entity;
using MouseSystem;
using UI.Stats;
using UnityEngine;

namespace DialogueSystem.AIDialogue
{
    public class AIConversant : AliveEntity, IRaycastable
    {
        [SerializeField] private Dialogue _dialogue;
        [SerializeField] private string _name;
        [SerializeField] private Vector3 _iconSpawnPosition = new Vector3(0,5,0);

        private GameObject _currentIcon;
        public string GetName => _name;
        public Dialogue GetDialogue => _dialogue;
        
        
        public CursorType GetCursorType() => CursorType.Dialogue;
        public void ClickAction()
        {
            HealthBarEntity.Instance.ShowHealth(this);
        }

        public bool HandleRaycast(PlayerEntity player) => _dialogue != null;

        protected override void Init()
        {
            
        }
    }
}