using Cosmos.System.Network.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CosmosProject
{
	public class Commands
	{
		private string currentVersion = "Version 0.3";
		private Dictionary<string, Action<string[]>> commandMap;
		private FileControls fc;
		private UserControls uc;

		public Commands()
		{
			InitializeCommands();
			fc = new FileControls();
			uc = new UserControls();
		}

		private void InitializeCommands()
		{
			commandMap = new Dictionary<string, Action<string[]>>
			{
				{ "help", args => helpCommand() },
				{ "runtime", args => Console.WriteLine(runtime()) },
				{ "version", args => Console.WriteLine(currentVersion) },
				{ "shutdown", args => Environment.Exit(0)},
				{ "poweroff", args => Environment.Exit(0)},
				{ "clear", args => Console.Clear()},
				{ "cls", args => Console.Clear()},
				{ "echo", args => Echo(args) },
				{ "cat", args => catfile(args)},
				{ "type", args => catfile(args)},
				//FileControl
				{ "mkdir", args => fc.commands(args) },
				{ "ls", args => fc.commands(args) },
				{ "dir", args => fc.commands(args) },
				{ "touch", args => fc.commands(args) },
				{ "rm", args => fc.commands(args) },
				{ "file", args => fc.commands(args) },
				//UserControls
				{ "user", args => uc.commands(args) },
				{ "logout", args => uc.commands(args) },
				{ "chmod", args => uc.commands(args) },
				{ "passwd", args => uc.commands(args) },
			};
		}
		public void commands(string[] args)
		{
			if (args.Length == 0)
			{
				Console.WriteLine("Try \"help\" for a quick view of all commands!");
				return;
			}

			string command = args[0].ToLower();

			if (commandMap.ContainsKey(command))
			{
				try
				{
					commandMap[command](args);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error executing command '{command}': {ex.Message}");
				}
			}
			else
			{
				Console.WriteLine("Unknown command, try \"help\" for a quick view of all commands!");
			}
		}

		public string runtime()
		{
			TimeSpan span = DateTime.Now - Kernel.dt_start;
			return String.Format("Laufzeit: {0} Hours {1} Minutes {2} Seconds", span.Hours, span.Minutes, span.Seconds);
		}

		public void helpCommand()
		{

			Console.WriteLine("general commands to manage the system:\n");
			Console.WriteLine("usage: help - print out system commands");
			Console.WriteLine("usage: clear | cls - clear the console");
			Console.WriteLine("usage: echo <arguments> - print out <arguments> from commandline");
			Console.WriteLine("usage: cat <filename> | type <filename> - print out the content from <filename>");
			Console.WriteLine("usage: runtime - print out the runtime of the system");
			Console.WriteLine("usage: version - print out the version of the system");
			Console.WriteLine("usage: poweroff | shutdown - shutdown the system");
			Console.WriteLine("usage: user help - print out user commands");
			Console.WriteLine("usage: file help - print out file commands");
			//string possible =
			//	"Allgemeine Commands zur Verwaltung des Systems:\n\n" +
			//	"poweroff OR shutdown - Herunterfahren des Systems\n--------\n" +
			//	"version - Ausgabe der aktuellen Version\n--------\n" +
			//	"runtime - Gibt die aktuelle Runtime zurück\n--------\n" +
			//	"echo [OPTIONS] - gibt die Argumente von der Kommandozeile aus\n--------\n" +
			//	"clear OR cls - Bereinigt den Inhalt der Kommandozeile\n--------\n"; ;

			//Console.WriteLine(possible);
		}
		private void Echo(string[] payload)
		{
			if (payload.Length > 1)
			{
				for (int i = 1; i < payload.Length; i++)
				{
					Console.Write(payload[i] + " ");
				}
				return;
			}
			else
			{
				Console.WriteLine("Enter arguments that should be printed out!");
				return;
			}
		}

		private void catfile(string[] payload)
		{
			if (payload.Length > 1)
			{
				try
				{
					string contents = File.ReadAllText(@"0:\" + payload[1]);

					Console.WriteLine(contents);
					return;
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());return;
				}
			}
			else
			{
				Console.WriteLine("Enter a filename! Use ls or dir to list all files and directories!");
				return;
			}
		}
	}
}
