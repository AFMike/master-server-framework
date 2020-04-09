using Barebones.Logging;
using Barebones.Networking;
using System;
using System.Collections.Generic;

namespace Barebones.MasterServer
{
    /// <summary>
    /// Represents a spawn request, and manages the state of request
    /// from start to finalization
    /// </summary>
    public class SpawnTask
    {
        private SpawnStatus status;
        protected List<Action<SpawnTask>> whenDoneCallbacks;

        public int Id { get; private set; }
        public string UniqueCode { get; private set; }
        public RegisteredSpawner Spawner { get; private set; }
        public Dictionary<string, string> Options { get; private set; }
        public Dictionary<string, string> CustomOptions { get; private set; }
        public SpawnFinalizationPacket FinalizationPacket { get; private set; }

        public event Action<SpawnStatus> OnStatusChangedEvent;

        public SpawnTask(int spawnTaskId, RegisteredSpawner spawner, Dictionary<string, string> properties, Dictionary<string, string> customOptions)
        {
            Id = spawnTaskId;

            Spawner = spawner;
            Options = properties;
            CustomOptions = customOptions;

            UniqueCode = Msf.Helper.CreateRandomString(6);
            whenDoneCallbacks = new List<Action<SpawnTask>>();
        }

        public bool IsAborted { get { return status < SpawnStatus.None; } }

        public bool IsDoneStartingProcess { get { return IsAborted || IsProcessStarted; } }

        public bool IsProcessStarted { get { return Status >= SpawnStatus.WaitingForProcess; } }

        public SpawnStatus Status
        {
            get { return status; }
            set
            {
                status = value;

                OnStatusChangedEvent?.Invoke(status);

                if (status >= SpawnStatus.Finalized || status < SpawnStatus.None)
                {
                    NotifyDoneListeners();
                }
            }
        }

        /// <summary>
        /// Peer, who registered a started process for this task
        /// (for example, a game server)
        /// </summary>
        public IPeer RegisteredPeer { get; private set; }

        /// <summary>
        /// Who requested to spawn
        /// (most likely clients peer)
        /// Can be null
        /// </summary>
        public IPeer Requester { get; set; }

        public void OnProcessStarted()
        {
            if (!IsAborted && Status < SpawnStatus.WaitingForProcess)
            {
                Status = SpawnStatus.WaitingForProcess;
            }
        }

        public void OnProcessKilled()
        {
            Status = SpawnStatus.Killed;
        }

        public void OnRegistered(IPeer peerWhoRegistered)
        {
            RegisteredPeer = peerWhoRegistered;

            if (!IsAborted && Status < SpawnStatus.ProcessRegistered)
            {
                Status = SpawnStatus.ProcessRegistered;
            }
        }

        public void OnFinalized(SpawnFinalizationPacket finalizationPacket)
        {
            FinalizationPacket = finalizationPacket;
            if (!IsAborted && Status < SpawnStatus.Finalized)
            {
                Status = SpawnStatus.Finalized;
            }
        }

        public override string ToString()
        {
            return string.Format("[SpawnTask: id - {0}]", Id);
        }

        protected void NotifyDoneListeners()
        {
            foreach (var callback in whenDoneCallbacks)
            {
                callback.Invoke(this);
            }

            whenDoneCallbacks.Clear();
        }

        /// <summary>
        /// Callback will be called when spawn task is aborted or completed 
        /// (game server is opened)
        /// </summary>
        /// <param name="callback"></param>
        public SpawnTask WhenDone(Action<SpawnTask> callback)
        {
            whenDoneCallbacks.Add(callback);
            return this;
        }

        public void Abort()
        {
            if (Status >= SpawnStatus.Finalized)
            {
                return;
            }

            Status = SpawnStatus.Aborting;

            KillSpawnedProcess();
        }

        public void KillSpawnedProcess()
        {
            Spawner.SendKillRequest(Id, killed =>
            {
                Status = SpawnStatus.Aborted;

                if (!killed)
                {
                    Logs.Warn("Spawned Process might not have been killed");
                }
            });
        }

    }
}