namespace DefaultNamespace
{
    using UnityEngine;

    public class DestroyableObject : MonoBehaviour, IDestroyable
    {
        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}