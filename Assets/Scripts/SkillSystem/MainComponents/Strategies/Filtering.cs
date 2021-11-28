namespace DefaultNamespace.UI.ButtonClickable
{
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class Filtering : ScriptableObject
    {
        public abstract IEnumerable<GameObject> StartFiltering(IEnumerable<GameObject> objectToFilter);
    }
}