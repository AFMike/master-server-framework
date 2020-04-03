using Barebones.Logging;
using Barebones.Networking;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace Barebones.MasterServer
{
    public class SpawnerController : ISpawnerController
    {
        /// <summary>
        /// Just <see cref="Process"/> lock
        /// </summary>
        protected static object processLock = new object();

        /// <summary>
        /// List of spawned processes
        /// </summary>
        protected Dictionary<int, Process> processes = new Dictionary<int, Process>();

        /// <summary>
        /// Current connection
        /// </summary>
        public IClientSocket Connection { get; protected set; }

        /// <summary>
        /// Id of this spawner controller that master server gives
        /// </summary>
        public int SpawnerId { get; protected set; }

        /// <summary>
        /// Settings, which are used by the default spawn handler
        /// </summary>
        public SpawnerConfig SpawnSettings { get; protected set; }

        public Logger Logger { get; protected set; }

        /// <summary>
        /// Fired when process is started
        /// </summary>
        public event Action OnProcessStartedEvent;

        /// <summary>
        /// Fired when process is killed
        /// </summary>
        public event Action OnProcessKilledEvent;

        /// <summary>
        /// Create new instance of spawner controller
        /// </summary>
        /// <param name="spawnerId"></param>
        /// <param name="connection"></param>
        /// <param name="options"></param>
        public SpawnerController(int spawnerId, IClientSocket connection)
        {
            Logger = Msf.Create.Logger(typeof(SpawnerController).Name, LogLevel.Info);

            Connection = connection;
            SpawnerId = spawnerId;

            SpawnSettings = new SpawnerConfig()
            {
                MasterIp = connection.ConnectionIp,
                MasterPort = connection.ConnectionPort,
                SpawnInBatchmode = false
            };

            // Add static handlers to listen one message for all controllers
            connection.SetHandler((short)MsfMessageCodes.SpawnProcessRequest, SpawnProcessRequestHandler);
            connection.SetHandler((short)MsfMessageCodes.KillProcessRequest, KillProcessRequestHandler);
        }

        /// <summary>
        /// Handles spawn request for all controllers filtered by ID
        /// </summary>
        /// <param name="message"></param>
        private static void SpawnProcessRequestHandler(IIncommingMessage message)
        {
            var data = message.Deserialize(new SpawnRequestPacket());
            var controller = Msf.Server.Spawners.GetController(data.SpawnerId) as SpawnerController;

            if (controller == null)
            {
                if (message.IsExpectingResponse)
                {
                    message.Respond("Couldn't find a spawn controller", ResponseStatus.NotHandled);
                }

                return;
            }

            controller.Logger.Debug($"Spawn process requested for spawn controller [{controller.SpawnerId}]");
            controller.SpawnRequestHandler(data, message);
        }

        /// <summary>
        /// Handles kill request for all controllers filtered by ID
        /// </summary>
        /// <param name="message"></param>
        private static void KillProcessRequestHandler(IIncommingMessage message)
        {
            var data = message.Deserialize(new KillSpawnedProcessRequestPacket());
            var controller = Msf.Server.Spawners.GetController(data.SpawnerId) as SpawnerController;

            if (controller == null)
            {
                if (message.IsExpectingResponse)
                {
                    message.Respond("Couldn't find a spawn controller", ResponseStatus.NotHandled);
                }

                return;
            }

            controller.Logger.Debug($"Kill process requested for spawn controller [{controller.SpawnerId}]");
            controller.KillRequestHandler(data.SpawnId);
            message.Respond(ResponseStatus.Success);
        }

        /// <summary>
        /// Notifies all listeners that process is started
        /// </summary>
        /// <param name="spawnId"></param>
        /// <param name="processId"></param>
        /// <param name="cmdArgs"></param>
        public void NotifyProcessStarted(int spawnId, int processId, string cmdArgs)
        {
            Msf.Server.Spawners.NotifyProcessStarted(spawnId, processId, cmdArgs, Connection);
            OnProcessStartedEvent?.Invoke();
        }

        /// <summary>
        /// Notifies all listeners that process is killed
        /// </summary>
        /// <param name="spawnId"></param>
        public void NotifyProcessKilled(int spawnId)
        {
            Msf.Server.Spawners.NotifyProcessKilled(spawnId);
            OnProcessKilledEvent?.Invoke();
        }

        /// <summary>
        /// Notifies master server, how many processes are running on a specified spawner
        /// </summary>
        /// <param name="count"></param>
        public void UpdateProcessesCount(int count)
        {
            Msf.Server.Spawners.UpdateProcessesCount(SpawnerId, count, Connection);
        }

        /// <summary>
        /// Default kill spawned process request handler that will be used by controller if <see cref="spawnRequestHandler"/> is not overriden
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="message"></param>
        public virtual void SpawnRequestHandler(SpawnRequestPacket packet, IIncommingMessage message)
        {
            Logger.Debug($"Default spawn handler started handling a request to spawn process for spawn controller [{SpawnerId}]");

            ////////////////////////////////////////////
            /// Create process args string
            var processArguments = new StringBuilder();
            processArguments.Append(" ");

            ////////////////////////////////////////////
            /// Check if we're overriding an IP to master server
            var masterIpArgument = string.IsNullOrEmpty(SpawnSettings.MasterIp) ?
                Connection.ConnectionIp : SpawnSettings.MasterIp;

            ////////////////////////////////////////////
            /// Create msater IP arg
            processArguments.Append($"{Msf.Args.Names.MasterIp} {masterIpArgument}");
            processArguments.Append(" ");

            ////////////////////////////////////////////
            /// Check if we're overriding a port to master server
            var masterPortArgument = SpawnSettings.MasterPort < 0 ?
                Connection.ConnectionPort : SpawnSettings.MasterPort;

            ////////////////////////////////////////////
            /// Create master port arg
            processArguments.Append($"{Msf.Args.Names.MasterPort} {masterPortArgument}");
            processArguments.Append(" ");

            ////////////////////////////////////////////
            /// Room Name
            if (packet.Properties.ContainsKey(MsfDictKeys.roomName))
            {
                /// Create room name arg
                processArguments.Append($"{Msf.Args.Names.RoomName} \"{packet.Properties[MsfDictKeys.roomName]}\"");
                processArguments.Append(" ");
            }

            ////////////////////////////////////////////
            /// Machine Ip
            var machineIpArgument = SpawnSettings.MachineIp;

            /// Create room IP arg
            processArguments.Append($"{Msf.Args.Names.RoomIp} {machineIpArgument}");
            processArguments.Append(" ");

            ////////////////////////////////////////////
            /// Create port for room arg
            int machinePortArgument = Msf.Server.Spawners.GetAvailablePort();
            processArguments.Append($"{Msf.Args.Names.RoomPort} {machinePortArgument}");
            processArguments.Append(" ");

            ////////////////////////////////////////////
            /// Get the scene name
            var sceneNameArgument = packet.Properties.ContainsKey(MsfDictKeys.sceneName)
                ? $"{Msf.Args.Names.LoadScene} {packet.Properties[MsfDictKeys.sceneName]}" : string.Empty;

            /// Create scene name arg
            processArguments.Append(sceneNameArgument);
            processArguments.Append(" ");

            ////////////////////////////////////////////
            /// If spawn in batchmode was set and `DontSpawnInBatchmode` arg is not provided
            var spawnInBatchmodeArgument = SpawnSettings.SpawnInBatchmode && !Msf.Args.DontSpawnInBatchmode;
            processArguments.Append((spawnInBatchmodeArgument ? "-batchmode -nographics" : string.Empty));
            processArguments.Append(" ");

            ////////////////////////////////////////////
            /// Create use websockets arg
            processArguments.Append((SpawnSettings.UseWebSockets ? Msf.Args.Names.UseWebSockets + " " : string.Empty));
            processArguments.Append(" ");

            ////////////////////////////////////////////
            /// Create spawn id arg
            processArguments.Append($"{Msf.Args.Names.SpawnId} {packet.SpawnId}");
            processArguments.Append(" ");

            ////////////////////////////////////////////
            /// Create spawn code arg
            processArguments.Append($"{Msf.Args.Names.SpawnCode} \"{packet.SpawnCode}\"");
            processArguments.Append(" ");

            ////////////////////////////////////////////
            /// Create destroy ui arg
            processArguments.Append((Msf.Args.DestroyUi ? Msf.Args.Names.DestroyUi + " " : string.Empty));
            processArguments.Append(" ");

            ////////////////////////////////////////////
            /// Create custom args
            processArguments.Append(packet.CustomArgs);
            processArguments.Append(" ");

            ///////////////////////////////////////////
            /// Path to executable
            var executablePath = SpawnSettings.ExecutablePath;

            if (string.IsNullOrEmpty(executablePath))
            {
                executablePath = File.Exists(Environment.GetCommandLineArgs()[0])
                    ? Environment.GetCommandLineArgs()[0]
                    : Process.GetCurrentProcess().MainModule.FileName;
            }

            // In case a path is provided with the request
            if (packet.Properties.ContainsKey(MsfDictKeys.executablePath))
            {
                executablePath = packet.Properties[MsfDictKeys.executablePath];
            }

            if (!string.IsNullOrEmpty(packet.OverrideExePath))
            {
                executablePath = packet.OverrideExePath;
            }

            /// Create info about starting process
            var startProcessInfo = new ProcessStartInfo(executablePath)
            {
                CreateNoWindow = false,
                UseShellExecute = false,
                Arguments = processArguments.ToString()
            };

            Logger.Debug("Starting process with args: " + startProcessInfo.Arguments);

            var processStarted = false;

            try
            {
                new Thread(() =>
                {
                    try
                    {
                        using (var process = Process.Start(startProcessInfo))
                        {
                            Logger.Debug("Process started. Spawn Id: " + packet.SpawnId + ", pid: " + process.Id);
                            processStarted = true;

                            lock (processLock)
                            {
                                // Save the process
                                processes[packet.SpawnId] = process;
                            }

                            var processId = process.Id;

                            // Notify server that we've successfully handled the request
                            MsfTimer.RunInMainThread(() =>
                            {
                                message.Respond(ResponseStatus.Success);
                                NotifyProcessStarted(packet.SpawnId, processId, startProcessInfo.Arguments);
                            });

                            process.WaitForExit();
                        }
                    }
                    catch (Exception e)
                    {
                        if (!processStarted)
                        {
                            MsfTimer.RunInMainThread(() => { message.Respond(ResponseStatus.Failed); });
                        }

                        Logger.Error("An exception was thrown while starting a process. Make sure that you have set a correct build path. " +
                                     $"We've tried to start a process at [{executablePath}]. You can change it at 'SpawnerBehaviour' component");
                        Logger.Error(e);
                    }
                    finally
                    {
                        lock (processLock)
                        {
                            // Remove the process
                            processes.Remove(packet.SpawnId);
                        }

                        MsfTimer.RunInMainThread(() =>
                        {
                            // Release the port number
                            Msf.Server.Spawners.ReleasePort(machinePortArgument);
                            Logger.Debug($"Notifying about killed process with spawn id [{packet.SpawnId}]");
                            NotifyProcessKilled(packet.SpawnId);
                        });
                    }

                }).Start();
            }
            catch (Exception e)
            {
                message.Respond(e.Message, ResponseStatus.Error);
                Logs.Error(e);
            }
        }

        /// <summary>
        /// Default kill spawned process request handler that will be used by controller if <see cref="killRequestHandler"/> is not overriden
        /// </summary>
        /// <param name="spawnId"></param>
        public virtual void KillRequestHandler(int spawnId)
        {
            Logger.Debug($"Default kill request handler started handling a request to kill a process with id [{spawnId}] for spawn controller with id [{SpawnerId}]");

            try
            {
                Process process;

                lock (processLock)
                {
                    processes.TryGetValue(spawnId, out process);
                    processes.Remove(spawnId);
                }

                if (process != null)
                {
                    process.Kill();
                }
            }
            catch (Exception e)
            {
                Logger.Error($"Got error while killing a spawned process with id [{spawnId}]");
                Logger.Error(e);
            }
        }

        /// <summary>
        /// Kill all processes running in this controller
        /// </summary>
        public void KillProcesses()
        {
            var list = new List<Process>();

            lock (processLock)
            {
                foreach (var process in processes.Values)
                {
                    list.Add(process);
                }
            }

            foreach (var process in list)
            {
                process.Kill();
            }
        }
    }
}