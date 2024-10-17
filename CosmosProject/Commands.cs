using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CosmosProject
{
	public class Commands
	{
		public string Identifier { get; set; }
		public int PID { get; set; }

		public virtual void help()
		{ }

		public virtual void run(string[] payload)
		{ }
	} 

	public class MakeDir : Commands 
	{
		public MakeDir(string ident, int pid) 
		{
			this.Identifier = ident;
			this.PID = pid;
		}

		public override void help()
		{
			Console.WriteLine("Help from " + Identifier);
		}

		public override void run(string[] payload)
		{
			Console.WriteLine("Run from " + Identifier);
		}
	}

}
