using Barebones.Logging;
using Barebones.MasterServer;
using Mirror;
using System;
using UnityEngine;

namespace Barebones.Bridges.Mirror
{
    public class MirrorNetworkManager : NetworkManager
    {
        #region INSPECTOR

        [Header("Mirror Network Manager Settings"), SerializeField]
        private HelpBox help = new HelpBox()
        {
            Text = "This is extension of NetworkManager",
            Type = HelpBoxType.Info
        };

        /// <summary>
        /// Log levelof this module
        /// </summary>
        [SerializeField]
        protected LogLevel logLevel = LogLevel.Info;

        #endregion

        /// <summary>
        /// Logger assigned to this module
        /// </summary>
        protected Logging.Logger logger;

        /// <summary>
        /// Invokes when mirror server is started
        /// </summary>
        public event Action OnServerStartedEvent;

        /// <summary>
        /// Invokes when mirror host is started
        /// </summary>
        public event Action OnHostStartedEvent;

        /// <summary>
        /// This is called on the Server when a Client disconnects from the Server
        /// </summary>
        public event Action<NetworkConnection> OnClientDisconnectedEvent;

        public override void Awake()
        {
            logger = Msf.Create.Logger(GetType().Name);
            logger.LogLevel = logLevel;

            base.Awake();

            // Prevent to create player automatically
            autoCreatePlayer = false;
        }

        #region MIRROR CALLBACKS

        /// <summary>
        /// When mirror server is started
        /// </summary>
        public override void OnStartServer()
        {
            base.OnStartServer();
            OnServerStartedEvent?.Invoke();
        }

        public override void OnStartHost()
        {
            base.OnStartHost();
            OnHostStartedEvent?.Invoke();
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            base.OnServerDisconnect(conn);
            OnClientDisconnectedEvent?.Invoke(conn);
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);
            
            // Register handler to listen to player creation message
            NetworkServer.RegisterHandler<CreatePlayerMessage>(CreatePlayerRequestHandler, false);
        }

        #endregion

        /// <summary>
        /// Invokes whenclient requested to create player on mirror server
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        protected virtual void CreatePlayerRequestHandler(NetworkConnection connection, CreatePlayerMessage message)
        {
            var playerObject = Instantiate(playerPrefab);
            NetworkServer.AddPlayerForConnection(connection, playerObject);
        }
    }
}