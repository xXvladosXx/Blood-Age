namespace StatsSystem
{
    using DefaultNamespace;
    using UnityEngine;

    [CreateAssetMenu (menuName = "Stat/Buff")]
    public class Buff : ScriptableObject
    {
        [SerializeField] private CharacteristicBonus _characteristicBonus;
        public CharacteristicBonus GetCharacteristicBonus => _characteristicBonus;
        
        [SerializeField] private float _time;
        public float GetLenghtOfEffect => _time;
    }
}