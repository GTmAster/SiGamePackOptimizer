using System.Collections.Generic;

namespace SiGamePackOptimizer
{
    internal interface IZipFile
    {
        void Unpack(string path);
        void Pack(string path);
        IEnumerable<string> Entries { get; }
        byte[] GetEntryContent(string entryName);
        void InsertEntry(string entryName, byte[] content);
    }
}