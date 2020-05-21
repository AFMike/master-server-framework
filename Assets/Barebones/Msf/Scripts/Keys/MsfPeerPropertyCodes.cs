namespace Barebones.MasterServer
{
    public enum MsfPeerPropertyCodes : short
    {
        Start = 32000,

        // Rooms
        RegisteredRooms,

        // Spawners
        RegisteredSpawners,
        ClientSpawnRequest
    }
}