using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace SiGamePackOptimizer.Tests
{
    public class ZipFileWrapperTests: IDisposable
    {
        private readonly ZipFileWrapper _target;
        private readonly string _localTemp;
        private const string Archive = @"TestFiles\archive.siq";

        public ZipFileWrapperTests()
        {
            _localTemp = Path.Combine(Directory.GetCurrentDirectory(), Guid.NewGuid().ToString("N"));
            _target = new ZipFileWrapper(_localTemp);
        }

        [Fact]
        public void AllFilesArePacked()
        {
            // arrange
            var tempResult = Path.GetTempPath() + Guid.NewGuid().ToString("N");

            // act
            _target.Unpack(Archive);
            _target.Pack(tempResult);
            var result = GetArchiveEntries(tempResult).ToArray();
            File.Delete(tempResult);

            // assert
            result.Should()
                .NotBeNull()
                .And.HaveCount(21)
                .And.Contain("content.xml")
                .And.Contain(@"Audio/Wiz_Khalifa_-_Black_And_Yellow-0-16.mp3")
                .And.Contain(@"Images/zabastovka.jpg");
        }

        [Fact]
        public void AllEntriesReturned()
        {
            // act
            _target.Unpack(Archive);
            var result = _target.Entries.ToArray();

            // assert
            result.Should()
                .NotBeNull()
                .And.HaveCount(21)
                .And.Contain(@"Audio/Wiz_Khalifa_-_Black_And_Yellow-0-16.mp3")
                .And.Contain(@"Images/zabastovka.jpg");
        }

        [Fact]
        public void ContentOfEntryIsReturned()
        {
            // assert
            var expectedData = File.ReadAllBytes(@"TestFiles\content.xml");

            // act
            _target.Unpack(Archive);
            var result = _target.GetEntryContent("content.xml");

            // assert
            result.Should().BeEquivalentTo(expectedData);
        }

        [Fact]
        public void EntryIsInserted()
        {
            // arrange
            const string newEntryName = @"Images\newEntry.jpg";
            var newEntryContent = File.ReadAllBytes(@"TestFiles\1000x1503.jpg");

            // act
            _target.Unpack(Archive);
            _target.InsertEntry(newEntryName, newEntryContent);
            var result = _target.GetEntryContent(newEntryName);

            // assert
            result.Should().BeEquivalentTo(newEntryContent);
        }

        private static IEnumerable<string> GetArchiveEntries(string archivePath)
        {
            using var archive = ZipFile.OpenRead(archivePath);
            return archive.Entries.Select(x => x.FullName);
        }

        public void Dispose()
        {
            _target.Dispose();
        }
    }
}