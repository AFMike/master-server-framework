using Barebones.MasterServer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Barebones.Bridges.Mirror.Character
{
    public class PlayerCharacter : PlayerCharacterBehaviour
    {
        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();

            // Notify listeners about that player character is in game
            Msf.Events.Invoke(MsfEventKeys.playerStartedGame, this);
        }

        private void OnDestroy()
        {
            if (isLocalPlayer)
            {
                Msf.Events.Invoke(MsfEventKeys.playerFinishedGame);
            }
        }
    }
}
