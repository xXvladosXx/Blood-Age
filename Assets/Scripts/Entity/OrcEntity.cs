using Entity.Race;

namespace Entity
{
    public class OrcEntity : AliveEntity
    {
        protected override void Init()
        {
            EnemyRaces.Add(new Race.Race(RaceEntity.Human));
            EnemyRaces.Add(new Race.Race(RaceEntity.Elf));
            
            Race = new Race.Race(RaceEntity.Orc);
        }
    }
}