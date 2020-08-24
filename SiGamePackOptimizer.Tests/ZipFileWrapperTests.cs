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
        private const string TestFilesDir = "TestFiles";
        private readonly string _archive = Path.Combine(TestFilesDir,"archive.siq");

        public ZipFileWrapperTests()
        {
            var localTemp = Path.Combine(Directory.GetCurrentDirectory(), Guid.NewGuid().ToString("N"));
            _target = new ZipFileWrapper(localTemp);
        }

        [Fact]
        public void AllFilesArePacked()
        {
            // arrange
            var tempResult = Path.GetTempPath() + Guid.NewGuid().ToString("N");

            // act
            _target.Unpack(_archive);
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
            _target.Unpack(_archive);
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
            var expectedData = File.ReadAllBytes(Path.Combine(TestFilesDir, @"pic.jpg"));

            // act
            _target.Unpack(_archive);
            var result = _target.GetEntryContent(@"Images/unnamed%20(1).jpg");

            // assert
            result.Should().BeEquivalentTo(expectedData);
        }

        [Fact]
        public void EntryIsInserted()
        {
            // arrange
            const string newEntryName = @"Images\newEntry.jpg";
            var newEntryContent = File.ReadAllBytes(Path.Combine(TestFilesDir, "1000x1503.jpg"));

            // act
            _target.Unpack(_archive);
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