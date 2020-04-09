using Barebones.Networking;
using System.Collections.Generic;

namespace Barebones.MasterServer
{
    public class SpawnRequestPacket : SerializablePacket
    {
        public int SpawnerId { get; set; }
        public int SpawnTaskId { get; set; }
        public string SpawnTaskUniqueCode { get; set; } = string.Empty;
        public Dictionary<string, string> CustomOptions { get; set; }
        public string OverrideExePath { get; set; } = string.Empty;
        public Dictionary<string, string> Options { get; set; }

        public override void ToBinaryWriter(EndianBinaryWriter writer)
        {
            writer.Write(SpawnerId);
            writer.Write(SpawnTaskId);
            writer.Write(SpawnTaskUniqueCode);
            writer.WriteDictionary(CustomOptions);
            writer.Write(OverrideExePath);
            writer.Write(Options);
        }

        public override void FromBinaryReader(EndianBinaryReader reader)
        {
            SpawnerId = reader.ReadInt32();
            SpawnTaskId = reader.ReadInt32();
            SpawnTaskUniqueCode = reader.ReadString();
            CustomOptions = reader.ReadDictionary();
            OverrideExePath = reader.ReadString();
            Options = reader.ReadDictionary();
        }
    }
}