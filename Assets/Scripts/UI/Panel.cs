using System;
using Entity;
using UnityEngine;

namespace UI
{
    public abstract class Panel : MonoBehaviour
    {
        public abstract void Initialize(AliveEntity aliveEntity);

        public event Action<Panel> OnPanelChange;

        protected void ChangeUI(Panel panel)
        {
            OnPanelChange?.Invoke(panel);
        }
    }
}