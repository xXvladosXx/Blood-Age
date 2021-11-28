namespace DefaultNamespace.Entity
{
    using UnityEngine;

    public abstract class AliveEntity : MonoBehaviour
    {
        [SerializeField] protected Health health;
        private void Awake()
        {
            Init();
        }

        protected abstract void Init();
    }
}