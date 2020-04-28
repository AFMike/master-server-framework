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
        private Dictionary<short, IPacketHandler> handlers;

        /// <summary>
        /// Logger assigned to this module
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
            handlers = new Dictionary<short, IPacketHandler>();

            logger = Msf.Create.Logger(GetType().Name);
            logger.LogLevel = logLevel;
        }

        protected virtual void Start()
        {
            ChangeConnection(CreateConnection());
            Initialize();
        }

        protected virtual void OnDestroy()
        {
            if(Connection != null)
            {
                if (handlers != null)
                {
                    foreach (var handler in handlers.Values)
                    {
                        Connection.RemoveHandler(handler);
                    }

                    handlers.Clear();
                }

                Connection.OnStatusChangedEvent -= OnConnectionStatusChanged;
                Connection.RemoveConnectionListener(ConnectedToServer);
                Connection.RemoveDisconnectionListener(DisconnectedFromServer);
            }
        }

        protected virtual IClientSocket CreateConnection()
        {
            return Msf.Client.Connection;
        }

        /// <summary>
        /// Sets a message handler to connection, which is used by this this object
        /// to communicate with server
        /// </summary>
        /// <param name="handler"></param>
        public void SetHandler(IPacketHandler handler)
        {
            handlers[handler.OpCode] = handler;
            Connection?.SetHandler(handler);
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
        /// Removes the packet handler, but only if this exact handler
        /// was used
        /// </summary>
        /// <param name="handler"></param>
        public void RemoveHandler(IPacketHandler handler)
        {
            Connection?.RemoveHandler(handler);
        }

        /// <summary>
        /// Changes the connection object, and sets all of the message handlers of this object
        /// to new connection.
        /// </summary>
        /// <param name="socket"></param>
        public void ChangeConnection(IClientSocket socket)
        {
            if (Connection != null)
            {
                Connection.RemoveConnectionListener(ConnectedToServer);
                Connection.RemoveDisconnectionListener(DisconnectedFromServer);
            }

            Connection = socket;

            // Override packet handlers
            foreach (var packetHandler in handlers.Values)
            {
                socket.SetHandler(packetHandler);
            }

            OnConnectionSocketChanged(Connection);
        }

        private void ConnectedToServer()
        {
            Connection.RemoveConnectionListener(ConnectedToServer);
            Connection.AddDisconnectionListener(DisconnectedFromServer);

            OnClientConnectedToServer();
        }

        private void DisconnectedFromServer()
        {
            Connection.AddConnectionListener(ConnectedToServer);
            Connection.RemoveDisconnectionListener(DisconnectedFromServer);

            OnClientDisconnectedFromServer();
        }

        /// <summary>
        /// Fired when this module is started
        /// </summary>
        protected virtual void Initialize() { }

        /// <summary>
        /// Fired when connection of this module is changed
        /// </summary>
        /// <param name="socket"></param>
        protected virtual void OnConnectionSocketChanged(IClientSocket socket)
        {
            socket.AddConnectionListener(ConnectedToServer);
            socket.OnStatusChangedEvent += OnConnectionStatusChanged;
        }

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
