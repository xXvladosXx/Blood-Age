namespace DefaultNamespace.Entity
{
    using DefaultNamespace.Entity.Race;
    using UnityEngine;

    public class OrcEntity : AliveEntity, ITargetable
    {
        protected override void Init()
        {
            EnemyRaces.Add(new Race.Race(RaceEntity.Human));
            EnemyRaces.Add(new Race.Race(RaceEntity.Elf));
            
            Race = new Race.Race(RaceEntity.Orc);
        }
    }
}