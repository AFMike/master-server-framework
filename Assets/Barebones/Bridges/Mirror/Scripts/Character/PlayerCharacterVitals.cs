using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Barebones.Bridges.Mirror.Character
{
    public delegate void VitalChangeFloatDelegate(ushort key, float value);

    public class PlayerCharacterVitals : PlayerCharacterBehaviour
    {
        [SyncVar]
        private bool isAlive = true;

        /// <summary>
        /// Called on client when one of the vital value is changed
        /// </summary>
        [SyncEvent]
        public event VitalChangeFloatDelegate EventOnVitalChanged;

        /// <summary>
        /// Called when player resurrected
        /// </summary>
        [SyncEvent]
        public event Action EventOnAlive;

        /// <summary>
        /// Called when player dies
        /// </summary>
        [SyncEvent]
        public event Action EventOnDie;

        /// <summary>
        /// Check if character is alive
        /// </summary>
        public bool IsAlive => isAlive;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void NotifyVitalChanged(ushort key, float value)
        {
            EventOnVitalChanged?.Invoke(key, value);
        }

        /// <summary>
        /// 
        /// </summary>
        [Server]
        public void SetAlive()
        {
            isAlive = true;
            EventOnAlive?.Invoke();
        }

        /// <summary>
        /// 
        /// </summary>
        [Server]
        public void SetDead()
        {
            isAlive = false;
            EventOnDie?.Invoke();
        }
    }
}