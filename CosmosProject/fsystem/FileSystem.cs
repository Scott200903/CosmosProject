using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.Listing;
using Cosmos.System.FileSystem.VFS;
using Sys = Cosmos.System;

namespace CosmosProject.fsystem
{
	public class fs : VFSBase
	{

		Sys.FileSystem.CosmosVFS filesys = new Cosmos.System.FileSystem.CosmosVFS();


		public fs() 
		{}

		public override DirectoryEntry CreateDirectory(string aPath)
		{
			throw new NotImplementedException();
		}

		public override DirectoryEntry CreateFile(string aPath)
		{
			throw new NotImplementedException();
		}

		public override bool DeleteDirectory(DirectoryEntry aPath)
		{
			throw new NotImplementedException();
		}

		public override bool DeleteFile(DirectoryEntry aPath)
		{
			throw new NotImplementedException();
		}

		public override long GetAvailableFreeSpace(string aDriveId)
		{
			throw new NotImplementedException();
		}

		public override DirectoryEntry GetDirectory(string aPath)
		{
			throw new NotImplementedException();
		}

		public override List<DirectoryEntry> GetDirectoryListing(string aPath)
		{
			throw new NotImplementedException();
		}

		public override List<DirectoryEntry> GetDirectoryListing(DirectoryEntry aEntry)
		{
			throw new NotImplementedException();
		}

		public override List<Disk> GetDisks()
		{
			throw new NotImplementedException();
		}

		public override DirectoryEntry GetFile(string aPath)
		{
			throw new NotImplementedException();
		}

		public override FileAttributes GetFileAttributes(string aPath)
		{
			throw new NotImplementedException();
		}

		public override string GetFileSystemLabel(string aDriveId)
		{
			throw new NotImplementedException();
		}

		public override string GetFileSystemType(string aDriveId)
		{
			throw new NotImplementedException();
		}

		public override string GetNextFilesystemLetter()
		{
			throw new NotImplementedException();
		}

		public override long GetTotalFreeSpace(string aDriveId)
		{
			throw new NotImplementedException();
		}

		public override long GetTotalSize(string aDriveId)
		{
			throw new NotImplementedException();
		}

		public override DirectoryEntry GetVolume(string aVolume)
		{
			throw new NotImplementedException();
		}

		public override List<DirectoryEntry> GetVolumes()
		{
			throw new NotImplementedException();
		}

		public override void Initialize(bool aShowInfo)
		{
			throw new NotImplementedException();
		}

		public override bool IsValidDriveId(string driveId)
		{
			throw new NotImplementedException();
		}

		public override void SetFileAttributes(string aPath, FileAttributes fileAttributes)
		{
			throw new NotImplementedException();
		}

		public override void SetFileSystemLabel(string aDriveId, string aLabel)
		{
			throw new NotImplementedException();
		}
	}
}
