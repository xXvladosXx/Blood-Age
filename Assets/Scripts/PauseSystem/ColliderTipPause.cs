using System;
using StateMachine.PlayerStates;
using TipSystem;
using UnityEngine;

namespace PauseSystem
{
    public class ColliderTipPause : MonoBehaviour
    {
        [TextArea(minLines:2, maxLines:10)]
        [SerializeField] private string _tipText;
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerStateManager>() != null)
            {
                TipPopup.Instance.ActivateTip(_tipText);
                Destroy(gameObject);
            }
        }

    }
}