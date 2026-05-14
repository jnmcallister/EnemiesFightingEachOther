using UnityEngine;

namespace EnemiesFightingEachOther
{
    /// <summary>
    /// Keeps an FSM awake. Borrowed from Enemy Randomizer mod
    /// https://github.com/Kerr1291/EnemyRandomizer/blob/master/EnemyRandomizerDll/EnemyRandomizerDB/Library/Spawners/FSMWaker.cs
    /// </summary>
    public class FSMWaker : MonoBehaviour
    {
        public PlayMakerFSM fsm;
        public string wakeState = "Sleep";
        public string wakeString = "WAKE";

        protected virtual void OnEnable()
        {
            fsm = gameObject.LocateMyFSM("Control"); // Default
        }

        protected virtual void Update()
        {
            if (fsm != null && fsm.ActiveStateName == wakeState)
            {
                fsm.SendEvent(wakeString);
            }
        }
    }
}