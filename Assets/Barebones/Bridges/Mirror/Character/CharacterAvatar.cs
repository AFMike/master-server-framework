using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Barebones.Bridges.Mirror.Character
{
    public class CharacterAvatar : CharacterBehaviour
    {
        [Header("Components"), SerializeField]
        protected GameObject[] remoteParts;

        public override void OnStartClient()
        {
            if (!isLocalPlayer)
            {
                SetPartsActive(true);
            }
        }

        public override void OnStartLocalPlayer()
        {
            SetPartsActive(false);
        }

        public virtual void SetPartsActive(bool value)
        {
            if (remoteParts != null)
            {
                foreach (var part in remoteParts)
                {
                    part.SetActive(value);
                }
            }
        }
    }
}
