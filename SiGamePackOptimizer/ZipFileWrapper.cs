using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace SiGamePackOptimizer
{
    internal class ZipFileWrapper: IZipFile, IDisposable
    {
        private readonly string _tempFile;
        private ZipArchive _zipArchive;

        public ZipFileWrapper(string tempFile)
        {
            _tempFile = tempFile;
        }

        public void Unpack(string path)
        {
            File.Copy(path, _tempFile, overwrite: true);
            _zipArchive = ZipFile.Open(_tempFile, ZipArchiveMode.Update);
        }

        public void Pack(string path)
        {
            _zipArchive.Dispose();
            File.Copy(_tempFile, path, overwrite: true);
        }

        public IEnumerable<string> Entries => _zipArchive.Entries.Select(x => x.FullName);

        public byte[] GetEntryContent(string entryName)
        {
            var entry = _zipArchive.GetEntry(entryName);
            if (entry == null)
                throw new ArgumentException($"Entry with name {entryName} doesn't exist in archive");
            using var entryStream = entry.Open();
            using var ms = new MemoryStream();
            entryStream.CopyTo(ms);
            return ms.ToArray();
        }

        public void InsertEntry(string entryName, byte[] content)
        {
            var oldEntry = _zipArchive.GetEntry(entryName);
            oldEntry?.Delete();
            var newEntry = _zipArchive.CreateEntry(entryName);
            using var entryStream = newEntry.Open();
            entryStream.Write(content);
        }

        public void Dispose()
        {
            _zipArchive?.Dispose();
            if (File.Exists(_tempFile))
                File.Delete(_tempFile);
        }
    }
}
