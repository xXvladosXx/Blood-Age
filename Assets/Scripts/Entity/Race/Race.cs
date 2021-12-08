namespace DefaultNamespace.Entity.Race
{
    using UnityEngine;

    public enum RaceEntity
    {
        Orc,
        Human,
        Elf
    }
    public class Race
    {
        private RaceEntity _race;
        public RaceEntity GetRace => _race;

        public Race(RaceEntity race)
        {
            _race = race;
        }
    }
}