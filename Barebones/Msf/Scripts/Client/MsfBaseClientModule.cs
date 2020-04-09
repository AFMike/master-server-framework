using Barebones.Logging;
using Barebones.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Barebones.MasterServer
{
    public abstract class MsfBaseClientModule : MonoBehaviour
    {
        /// <summary>
        /// Client handlers list. Requires for connection changing process. <seealso cref="ChangeConnection(IClientSocket)"/>
        /// </summary>
        private Dictionary<short, IPacketHandler> _handlers;

        /// <summary>
        /// Logger connected to this module
        /// </summary>
        protected Logging.Logger logger;

        [Header("Base Module Settings"), SerializeField]
        protected LogLevel logLevel = LogLevel.Info;

        /// <summary>
        /// Current module connection
        /// </summary>
        public IClientSocket Connection { get; protected set; }

        /// <summary>
        /// Check if current module connection isconnected to server
        /// </summary>
        public bool IsConnected => Connection != null && Connection.IsConnected;

        protected virtual void Awake()
        {
            _handlers = new Dictionary<short, IPacketHandler>();

            logger = Msf.Create.Logger(GetType().Name);
            logger.LogLevel = logLevel;

            ChangeConnection(Msf.Connection);

            Connection.AddConnectionListener(ConnectedToMaster);
            Connection.OnStatusChangedEvent += OnConnectionStatusChanged;
        }

        protected virtual void Start()
        {
            Initialize();
        }

        protected virtual void OnDestroy()
        {
            Connection.OnStatusChangedEvent -= OnConnectionStatusChanged;
            Connection.RemoveConnectionListener(ConnectedToMaster);
        }

        /// <summary>
        /// Sets a message handler to connection, which is used by this this object
        /// to communicate with server
        /// </summary>
        /// <param name="handler"></param>
        public void SetHandler(IPacketHandler handler)
        {
            _handlers[handler.OpCode] = handler;

            if (Connection != null)
            {
                Connection.SetHandler(handler);
            }
        }

        /// <summary>
        /// Sets a message handler to connection, which is used by this this object
        /// to communicate with server 
        /// </summary>
        /// <param name="opCode"></param>
        /// <param name="handler"></param>
        public void SetHandler(short opCode, IncommingMessageHandler handler)
        {
            SetHandler(new PacketHandler(opCode, handler));
        }

        /// <summary>
        /// Changes the connection object, and sets all of the message handlers of this object
        /// to new connection.
        /// </summary>
        /// <param name="socket"></param>
        public void ChangeConnection(IClientSocket socket)
        {
            Connection = socket;

            // Override packet handlers
            foreach (var packetHandler in _handlers.Values)
            {
                socket.SetHandler(packetHandler);
            }
        }

        private void ConnectedToMaster()
        {
            Connection.RemoveConnectionListener(ConnectedToMaster);
            Connection.AddDisconnectionListener(DisconnectedToMaster);

            OnClientConnectedToServer();
        }

        private void DisconnectedToMaster()
        {
            Connection.AddConnectionListener(ConnectedToMaster);
            Connection.RemoveDisconnectionListener(DisconnectedToMaster);

            OnClientDisconnectedFromServer();
        }

        /// <summary>
        /// Fired when this module is started
        /// </summary>
        protected virtual void Initialize() { }

        /// <summary>
        /// Fired each time the connection status was changed
        /// </summary>
        /// <param name="status"></param>
        protected virtual void OnConnectionStatusChanged(ConnectionStatus status) { }

        /// <summary>
        /// Fired when this module successfully connected to server
        /// </summary>
        protected virtual void OnClientConnectedToServer() { }

        /// <summary>
        /// Fired when this module disconnected from server
        /// </summary>
        protected virtual void OnClientDisconnectedFromServer() { }
    }
}
