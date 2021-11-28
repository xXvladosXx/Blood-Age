namespace DefaultNamespace.SkillSystem.SkillInfo
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class SkillData
    {
        public SkillData(GameObject user)
        {
            _user = user;
        }
        
        public IEnumerable<GameObject> Target { get; set; }
        public Vector3 MousePosition { get; set; }
        public Transform Renderer { get; set; }

        private GameObject _user;
        public GameObject GetUser => _user;
              
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