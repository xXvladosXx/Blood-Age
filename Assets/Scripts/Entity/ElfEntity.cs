using Entity.Race;

namespace Entity
{
    public class ElfEntity : AliveEntity 
    {
        protected override void Init()
        {
            EnemyRaces.Add(new Race.Race(RaceEntity.Orc));
            
            Race = new Race.Race(RaceEntity.Elf);
        }
    }
}