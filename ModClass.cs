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
                EnemySpawningUtils.SpawnEnemy(preloads["GG_Gruz_Mother"]["_Enemies/Giant Fly"]);
            }
        }
    }
}