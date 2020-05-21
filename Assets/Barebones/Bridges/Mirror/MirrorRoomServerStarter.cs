using Barebones.MasterServer;
using Barebones.Networking;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Barebones.Bridges.Mirror
{
    public class MirrorRoomServerStarter : BaseClientBehaviour
    {
        #region INSPECTOR

        [Header("Master Connection Settings")]
        [SerializeField]
        private string masterIp = "127.0.0.1";
        [SerializeField]
        private int masterPort = 5000;
        [Header("Editor Settings")]

        public HelpBox _editorHeader = new HelpBox()
        {
            Text = "Starts the Mirror server if conditions are met",
            Type = HelpBoxType.Info
        };

        /// <summary>
        /// This will start server in editor automatically
        /// </summary>
        [SerializeField]
        private bool autoStartInEditor = true;
        /// <summary>
        /// After the room is successfully registered system will try to join you automatically
        /// </summary>
        [SerializeField]
        protected bool autoJoinRoom = true;

        #endregion

        protected override void Awake()
        {
            // If master IP is provided via cmd arguments
            masterIp = Msf.Args.ExtractValue(Msf.Args.Names.MasterIp, masterIp);

            // If master port is provided via cmd arguments
            masterPort = Msf.Args.ExtractValueInt(Msf.Args.Names.MasterPort, masterPort);

            base.Awake();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Connection.RemoveConnectionListener(OnConnectedToMasterHandler);
        }

        /// <summary>
        /// Invokes when this module is started
        /// </summary>
        protected override void OnInitialize()
        {
            // Register listening to connection
            Connection.AddConnectionListener(OnConnectedToMasterHandler);

            // If we are in test mode
            if (IsAllowedToBeStartedInEditor())
            {
                // Listen to when a room is registered. This is for test purpose only
                Msf.Server.Rooms.OnRoomRegisteredEvent += OnRoomRegisteredEventHandler;
            }

            // If connection to master server is not established
            if (!Connection.IsConnected)
            {
                Connection.Connect(masterIp, masterPort);
            }
        }

        /// <summary>
        /// Check is this module is allowed to be started in editor. This feature is for testing purpose only
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsAllowedToBeStartedInEditor()
        {
            return !Msf.Client.Rooms.ForceClientMode
                && Msf.Runtime.IsEditor
                   && autoStartInEditor
                   && Msf.Client.Rooms.LastReceivedAccess == null;
        }

        /// <summary>
        /// Invokes when the room is registered in test mode
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void OnRoomRegisteredEventHandler(RoomController roomController)
        {
            // Let's join the room automatically
            if (autoJoinRoom)
            {
                // But if we are not signed in yet
                if (!Msf.Client.Auth.IsSignedIn)
                {
                    // Let's do it rign now as a guest
                    Msf.Client.Auth.SignInAsGuest((accountInfo, error) =>
                    {
                        // If signing in is failed
                        if(accountInfo == null)
                        {
                            logger.Error($"An error occurred when signing in as guest in test mode. Error: {error}");
                            return;
                        }

                        logger.Info("You are successfully logged in as guest in test mode");


                    });
                }
            }
        }

        /// <summary>
        /// Invokes when msf client connected to master server
        /// </summary>
        private void OnConnectedToMasterHandler()
        {
            // If our room is started as spawned process
            if (Msf.Server.Spawners.IsSpawnedProccess)
            {
                // Try to register spawned process first
                RegisterSpawnedProcess();
                return;
            }

            // If we are testing our room in editor
            if (IsAllowedToBeStartedInEditor())
            {

            }
        }

        /// <summary>
        /// Before we register our room we need to register spawned process if required
        /// </summary>
        protected void RegisterSpawnedProcess()
        {
            // Let's register this process
            Msf.Server.Spawners.RegisterSpawnedProcess(Msf.Args.SpawnTaskId, Msf.Args.SpawnTaskUniqueCode, (taskController, error) =>
            {
                if (taskController == null)
                {
                    logger.Error($"Room server process cannot be registered. The reason is: {error}");
                    return;
                }

                // If room port is provided via cmd arguments
                if (Msf.Args.IsProvided(Msf.Args.Names.RoomPort))
                {
                    SetPort((ushort)Msf.Args.RoomPort);
                }

                // Start Mirror server
                NetworkManager.singleton.StartServer();
            });
        }

        /// <summary>
        /// Get mirror network manager
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetNetworkManager<T>() where T : NetworkManager
        {
            return NetworkManager.singleton as T;
        }

        /// <summary>
        /// Sets an address 
        /// </summary>
        /// <param name="roomAddress"></param>
        public void SetAddress(string roomAddress)
        {
            NetworkManager.singleton.networkAddress = roomAddress;
        }

        /// <summary>
        /// Gets an address
        /// </summary>
        /// <param name="roomIp"></param>
        public string GetAddress()
        {
            return NetworkManager.singleton.networkAddress;
        }

        /// <summary>
        /// Set network transport port
        /// </summary>
        /// <param name="port"></param>
        public virtual void SetPort(ushort port)
        {
            ((TelepathyTransport)Transport.activeTransport).port = port;
        }

        /// <summary>
        /// Get network transport port
        /// </summary>
        /// <returns></returns>
        public virtual ushort GetPort()
        {
            return ((TelepathyTransport)Transport.activeTransport).port;
        }
    }
}