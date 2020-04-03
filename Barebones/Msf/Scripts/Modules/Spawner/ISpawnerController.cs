using Barebones.Logging;
using Barebones.Networking;

namespace Barebones.MasterServer
{
    public interface ISpawnerController
    {
        SpawnerConfig SpawnSettings { get; }
        Logger Logger { get; }
        IClientSocket Connection { get; }
        int SpawnerId { get; }
        void SpawnRequestHandler(SpawnRequestPacket packet, IIncommingMessage message);
        void KillRequestHandler(int spawnId);
        void KillProcesses();
    }
}