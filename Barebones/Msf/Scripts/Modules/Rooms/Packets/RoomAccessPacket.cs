using Barebones.Networking;
using System.Collections.Generic;

namespace Barebones.MasterServer
{
    public class RoomAccessPacket : SerializablePacket
    {
        public string RoomIp { get; set; }
        public int RoomPort { get; set; }
        public string Token { get; set; }
        public int RoomId { get; set; }
        public string SceneName { get; set; } = string.Empty;
        public DictionaryOptions CustomOptions { get; set; }

        public override void ToBinaryWriter(EndianBinaryWriter writer)
        {
            writer.Write(Token);
            writer.Write(RoomIp);
            writer.Write(RoomPort);
            writer.Write(RoomId);
            writer.Write(SceneName);
            writer.Write(CustomOptions.ToDictionary());
        }

        public override void FromBinaryReader(EndianBinaryReader reader)
        {
            Token = reader.ReadString();
            RoomIp = reader.ReadString();
            RoomPort = reader.ReadInt32();
            RoomId = reader.ReadInt32();
            SceneName = reader.ReadString();
            CustomOptions = new DictionaryOptions(reader.ReadDictionary());
        }

        public override string ToString()
        {
            return $"[RoomAccessPacket| PublicAddress: {RoomIp}:{RoomPort}, RoomId: {RoomId}, Token: {Token}, Properties: {CustomOptions.ToReadableString()}]";
        }
    }
}