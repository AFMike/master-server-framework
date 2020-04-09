using Barebones.Networking;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Barebones.MasterServer
{
    public class ClientsSpawnRequestPacket : SerializablePacket
    {
        public Dictionary<string, string> Options { get; set; }
        public Dictionary<string, string> CustomOptions { get; set; }

        public override void ToBinaryWriter(EndianBinaryWriter writer)
        {
            writer.Write(Options);
            writer.WriteDictionary(CustomOptions);
        }

        public override void FromBinaryReader(EndianBinaryReader reader)
        {
            Options = reader.ReadDictionary();
            CustomOptions = reader.ReadDictionary();
        }

        public override string ToString()
        {
            return string.Join(" ", Options.Select(opt => $"{opt.Key} {opt.Value}")) + " " + string.Join(" ", CustomOptions.Select(opt => $"{opt.Key} {opt.Value}"));
        }
    }
}