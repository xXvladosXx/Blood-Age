using System.Collections.Generic;
using System.Linq;
using Entity.Race;
using SaveSystem;
using UnityEngine;

namespace Entity
{
    public class HumanEntity : AliveEntity
    {
        
        protected override void Init()
        {
            EnemyRaces.Add(new Race.Race(RaceEntity.Orc));
            
            Race = new Race.Race(RaceEntity.Human);
        }

    }
}