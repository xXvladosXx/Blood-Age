using UnityEngine;

namespace UI.Tooltip
{
    public abstract class Tooltip : MonoBehaviour
    {
        private void Awake()
        {
            Initialize();
        }

        protected abstract void Initialize();
    }
}