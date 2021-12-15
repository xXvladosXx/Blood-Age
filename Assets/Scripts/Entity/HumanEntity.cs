﻿namespace DefaultNamespace.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DefaultNamespace.Entity.Race;
    using UnityEngine;

    public class HumanEntity : AliveEntity, ITargetable
    {
        
        protected override void Init()
        {
            EnemyRaces.Add(new Race.Race(RaceEntity.Orc));
            
            Stats.Add(DefaultNamespace.Stats.Agility, 20);
            Stats.Add(DefaultNamespace.Stats.Strength, 15);
            Stats.Add(DefaultNamespace.Stats.Intelligence, 25);
            
            Race = new Race.Race(RaceEntity.Human);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out AliveEntity aliveEntity)) return;
            
            foreach (var enemyRace in EnemyRaces.Where(enemyRace => enemyRace == aliveEntity.GetRace))
            {
                Debug.Log("Destroy " + other.gameObject);
            }
        }
    }
}