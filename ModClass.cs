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

        //List<Collider2D> enemyColliders;

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
        public override string GetVersion() => "0.1.0";

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Log("Initializing");

            Instance = this;

            //enemyColliders = new List<Collider2D>();

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
                newEnemy.layer = 17;

                // Let this enemy damage other enemies
                DamageEnemies damageEnemies = newEnemy.AddComponent<DamageEnemies>();
                damageEnemies.damageDealt = 10;
                //Collider2D col = newEnemy.GetComponent<Collider2D>();
                //enemyColliders.Add(col);
                
                // Allow collisions with every other enemy
                //for (int i = 0; i < enemyColliders.Count; i++)
                //{
                //    Log($"{col.name}, {enemyColliders[i].name}");
                //    Physics2D.IgnoreCollision(col, enemyColliders[i], false);
                //}
            }
        }
    }
}