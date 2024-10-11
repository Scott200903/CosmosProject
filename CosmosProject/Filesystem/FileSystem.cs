using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cosmos.System.FileSystem;
using Sys = Cosmos.System;

namespace CosmosProject.Filesystem
{
	public class fs
	{
		public fs() 
		{
			Sys.FileSystem.CosmosVFS fs = new Cosmos.System.FileSystem.CosmosVFS();
		}
	}
}
