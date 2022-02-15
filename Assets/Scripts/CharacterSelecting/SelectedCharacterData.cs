using StatsSystem;
using UnityEngine;

namespace CharacterSelecting
{
    [CreateAssetMenu (menuName = "CharacterSelect/Selection")]
    public class SelectedCharacterData : ScriptableObject
    {
        [SerializeField] private Class _class;

        public Class Class
        {
            get => _class;
            set => _class = value;
        }
    }
}