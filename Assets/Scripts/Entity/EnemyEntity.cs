using System.Collections.Generic;
using System.Linq;
using LootSystem;
using SaveSystem;
using UnityEngine;

namespace Entity
{
    [RequireComponent(typeof(LootSpawner))]

    public class EnemyEntity : AliveEntity
    {
        private LootSpawner _lootSpawner;

        protected override void DieActions()
        {
            base.DieActions();
            _lootSpawner.SpawnLoot(transform.position);
            LeanTween.delayedCall(3f, () =>
            {
                gameObject.SetActive(false);
            });
        }

        protected override void Init()
        {
            Outline = GetComponentInChildren<Outline>();
            _lootSpawner = GetComponent<LootSpawner>();
        }

    }
}