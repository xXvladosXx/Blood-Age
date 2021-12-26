namespace DefaultNamespace.SkillSystem.SkillInfo
{
    using System.Collections;
    using System.Collections.Generic;
    using DefaultNamespace.Entity;
    using UnityEngine;

    public class SkillData
    {
        private AliveEntity _user;

        public SkillData(AliveEntity user)
        {
            _user = user;
        }
        
        public IEnumerable<GameObject> Target { get; set; }
        public Vector3 MousePosition { get; set; }
        public Transform Renderer { get; set; }

        public AliveEntity GetUser => _user;
              
        private bool _cancelled = false;
        public bool IsCancelled => _cancelled;

        public void StartCoroutine(IEnumerator coroutine)
        {
            GetUser.GetComponent<MonoBehaviour>().StartCoroutine(coroutine);
        }

        public void Cancel()
        { 
            _cancelled = true;
        }
    }
}