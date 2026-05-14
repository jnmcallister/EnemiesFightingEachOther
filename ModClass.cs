using System;
using System.Collections;
using System.Collections.Generic;
using Modding;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace EnemiesFightingEachOther
{
    public class EnemiesFightingEachOther : Mod
    {
        internal static EnemiesFightingEachOther Instance;

        public override List<ValueTuple<string, string>> GetPreloadNames()
        {
            return new List<ValueTuple<string, string>>
            {
                ("GG_Gruz_Mother", "_Enemies/Giant Fly")
            };
        }
        public Dictionary<string, Dictionary<string, GameObject>> preloads;

        public EnemiesFightingEachOther() : base("EnemiesFightingEachOther")
        {
            Instance = this;
        }
        public override string GetVersion() => "0.1.1";

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Log("Initializing");

            Instance = this;

            // Get preloaded objects
            Log("Accessing preloads");
            var bossPrefab = preloadedObjects["GG_Gruz_Mother"]["_Enemies/Giant Fly"];
            UObject.DontDestroyOnLoad(bossPrefab);
            preloads = preloadedObjects;
            Log("Finished accessing preloads");

            // Subscribe to hooks
            ModHooks.HeroUpdateHook += OnHeroUpdate;

            Log("Initialized");
        }

        public void OnHeroUpdate()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                Log("Spawning Gruz Mother");

                // Instantiate enemy
                GameObject newEnemy = UObject.Instantiate(preloads["GG_Gruz_Mother"]["_Enemies/Giant Fly"],
                    HeroController.instance.transform.position, Quaternion.identity);
                newEnemy.SetActive(true);

                // Create a child object which will handle dealing damage to other enemies
                // This is done because objects on the enemy layer don't collide with each other
                GameObject damageEnemiesGO = new GameObject("Damage Enemies");
                damageEnemiesGO.transform.SetParent(newEnemy.transform, false);
                damageEnemiesGO.layer = 17; // Hero attack layer

                // Let this enemy damage other enemies
                DamageEnemies damageEnemies = damageEnemiesGO.AddComponent<DamageEnemies>();
                damageEnemies.damageDealt = 10;
                damageEnemies.ignoreInvuln = false;

                // Give this child object a collider to interact with other enemies
                Collider2D parentCol = newEnemy.GetComponent<Collider2D>();
                Collider2D childCol = parentCol.CopyCollider2DTo(damageEnemiesGO);
                childCol.isTrigger = true; // Make sure this doesn't collide with the ground
            }
        }
    }
}