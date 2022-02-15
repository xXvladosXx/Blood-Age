using Entity;
using UnityEngine;

namespace MouseSystem
{
    public interface IRaycastable
    {
        CursorType GetCursorType();
        void ClickAction();
        bool HandleRaycast(PlayerEntity player);
    }

    public enum CursorType
    {
        Dialogue,
        Movement,
        Combat,
        PickUp,
        UI,
        None
    }
}