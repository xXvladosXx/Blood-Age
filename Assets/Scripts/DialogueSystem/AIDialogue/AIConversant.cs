using Entity;
using MouseSystem;
using SceneSystem;
using UI.Stats;
using UnityEngine;

namespace DialogueSystem.AIDialogue
{
    public class AIConversant : AliveEntity, IRaycastable
    {
        [SerializeField] private Dialogue _dialogue;
        [SerializeField] private string _name;
        [SerializeField] private Vector3 _iconSpawnPosition = new Vector3(0,5,0);
        [SerializeField] private Portal _portal;

        private GameObject _currentIcon;
        public string GetName => _name;
        public Dialogue GetDialogue => _dialogue;
        public Portal GetPortal => _portal;

        public CursorType GetCursorType() => CursorType.Dialogue;

        public bool HandleRaycast(PlayerEntity player) => _dialogue != null;

        protected override void Init()
        {
            
        }
    }
}