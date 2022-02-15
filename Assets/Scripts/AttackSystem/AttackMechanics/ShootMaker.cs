using UnityEngine;

namespace AttackSystem.AttackMechanics
{
    public class ShootMaker : MonoBehaviour
    {
        public void StartDraw()
        {
            BowAnimatorController bowAnimatorController = GetComponentInChildren<BowAnimatorController>();
            
            bowAnimatorController.DrawRope();
        }
    }
}