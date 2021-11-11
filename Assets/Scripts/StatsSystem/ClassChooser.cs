namespace DefaultNamespace
{
    using System.Linq;
    using UnityEngine;

    public class ClassChooser : MonoBehaviour
    {
        [SerializeField] private StarterCharacterData _starterCharacterData;
        [SerializeField] private Class _character = Class.Warrior;
        public Class GetClass => _character;

        public float GetStat(Class classChooser, Characteristics characteristics)
        {
            float starterValue = _starterCharacterData.ReturnLevelValueCharacteristics(classChooser, characteristics, 1);
            float valueWithBonus = GetBonus(characteristics) + starterValue;

            return valueWithBonus;
        }

        private float GetBonus(Characteristics characteristic)
        {
            return GetComponents<IModifier>().Sum(bonus => bonus.AddBonus(characteristic));
        }
    }
}