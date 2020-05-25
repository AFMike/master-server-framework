using Barebones.MasterServer;
using Barebones.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Barebones.Bridges.Mirror
{
    public class MirrorRoomClientStarter : BaseClientBehaviour
    {
        protected override void OnInitialize()
        {
            if (Msf.Options.Has(MsfDictKeys.autoStartRoomClient))
            {
                Debug.Log("Helllllllooooooo");
                Connection.AddConnectionListener(OnConnectedToMasterServerEventHandler);
            }
        }

        private void OnConnectedToMasterServerEventHandler()
        {
            MsfTimer.WaitForEndOfFrame(() =>
            {
                Debug.Log("I'm heeeeeere");
                MirrorRoomClient.Instance.StartClient();
            });
        }
    }
}
