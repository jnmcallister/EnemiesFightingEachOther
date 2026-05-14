using System;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using Modding;
using UnityEngine;
using UObject = UnityEngine.Object;
using Satchel;

namespace EnemiesFightingEachOther
{
    public class EnemiesFightingEachOther : Mod
    {
        internal static EnemiesFightingEachOther Instance;

        int count = 1;

        public override List<ValueTuple<string, string>> GetPreloadNames()
        {
            return new List<ValueTuple<string, string>>
            {
                ("GG_Gruz_Mother", "_Enemies/Giant Fly"),
                ("GG_Hive_Knight", "Battle Scene/Hive Knight"),
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

            UObject.DontDestroyOnLoad(preloadedObjects["GG_Gruz_Mother"]["_Enemies/Giant Fly"]);
            UObject.DontDestroyOnLoad(preloadedObjects["GG_Hive_Knight"]["Battle Scene/Hive Knight"]);
            preloads = preloadedObjects;

            Log("Finished accessing preloads");


            // Subscribe to hooks
            ModHooks.HeroUpdateHook += OnHeroUpdate;


            Log("Initialized");
        }

        public void OnHeroUpdate()
        {
            if (Input.GetKeyDown(KeyCode.O)) // Gruz mother
            {
                EnemySpawningUtils.SpawnEnemy(preloads["GG_Gruz_Mother"]["_Enemies/Giant Fly"]);
            }
            if (Input.GetKeyDown(KeyCode.P)) // Hive knight
            {
                GameObject hiveKnight = EnemySpawningUtils.SpawnEnemy(preloads["GG_Hive_Knight"]["Battle Scene/Hive Knight"]);

                // Keep hive knight awake
                hiveKnight.AddComponent<FSMWaker>();

                // Disable spawning bees (bees crash the game)
                PlayMakerFSM fsm = hiveKnight.LocateMyFSM("Control");
                Utils.RemoveElementFromSendRandomEventV3(fsm, "Phase 3", 1, "BEE ROAR");

                // Make sure he spawns at the player's y-level
                float playerYLevel = HeroController.instance.transform.position.y;
                fsm.FsmVariables.FindFsmFloat("Ground Y").Value = playerYLevel + 1.0f; // Add 1 so he doesn't spawn partially in the floor
            }
        }
    }
}