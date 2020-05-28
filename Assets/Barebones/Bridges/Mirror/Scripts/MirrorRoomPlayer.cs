using Barebones.MasterServer;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Barebones.Bridges.Mirror
{
    public class MirrorRoomPlayer
    {
        public MirrorRoomPlayer()
        {
        }

        public MirrorRoomPlayer(int msfPeerId, NetworkConnection mirrorPeer, string username, DictionaryOptions customOptions)
        {
            MsfPeerId = msfPeerId;
            MirrorPeer = mirrorPeer ?? throw new ArgumentNullException(nameof(mirrorPeer));
            Username = username ?? throw new ArgumentNullException(nameof(username));
            CustomOptions = customOptions ?? throw new ArgumentNullException(nameof(customOptions));
            Profile = new ObservableServerProfile(username);
        }

        /// <summary>
        /// Id of the Masterserver connection
        /// </summary>
        public int MsfPeerId { get; set; }

        /// <summary>
        /// Connection of mirror
        /// </summary>
        public NetworkConnection MirrorPeer { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Custom options user can use in game
        /// </summary>
        public DictionaryOptions CustomOptions { get; set; }

        /// <summary>
        /// Player profile
        /// </summary>
        public ObservableServerProfile Profile { get; set; }
    }
}
