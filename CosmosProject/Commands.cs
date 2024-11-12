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
			string possible =
				"Allgemeine Commands zur Verwaltung des Systems:\n\n" +
				"poweroff OR shutdown - Herunterfahren des Systems\n--------\n" +
				"version - Ausgabe der aktuellen Version\n--------\n" +
				"runtime - Gibt die aktuelle Runtime zurück\n--------\n" +
				"echo [OPTIONS] - gibt die Argumente von der Kommandozeile aus\n--------\n" +
				"clear OR cls - Bereinigt den Inhalt der Kommandozeile\n--------\n"; ;

			Console.WriteLine(possible);
		}
		private void Echo(string[] payload)
		{
			if (payload.Length > 1)
			{
				for (int i = 1; i < payload.Length; i++)
				{
					Console.Write(payload[i] + " ");
				}
				Console.WriteLine();
			}
			else
			{
				Console.WriteLine("Enter an option");
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
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
				}
			}
			else
			{
				Console.WriteLine("Enter a filename! Use ls or dir!");
			}
		}
	}
}
