namespace StatsSystem
{
    using System;

    public class LevelUp
    {
        public event Action OnExperienceGive;
        
        private float _currentExp;
        public float GetCurrentExp => _currentExp;

        public void ExperienceReward(float experience)
        {
            _currentExp += experience;
            OnExperienceGive?.Invoke();
        }
        
        
    }
}