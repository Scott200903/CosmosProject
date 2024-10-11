using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Sys = Cosmos.System;
using CosmosProject.UserManagement;
using System.Reflection.Metadata;
using System.Diagnostics;
using Cosmos.System.FileSystem;
using System.IO;

namespace CosmosProject
{
	public class Kernel : Sys.Kernel
	{
		List<User> tmp;
		Sys.FileSystem.CosmosVFS fs = new Cosmos.System.FileSystem.CosmosVFS();

		DateTime dt_start = DateTime.Now;
		protected override void BeforeRun()
		{
			Console.WriteLine("Cosmos booted successfully.");
			tmp = new List<User>();

			Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
		}
		protected override void Run()
		{
			//Help
			//version
			// args[] für verschiedene Argumente

			string versionString = "0.1";

			Console.Write("Enter Command: ");
			string input = Console.ReadLine();

			string possible = "moegliche Commands:\n" +
				"space [OPTIONS] - Verwaltung des Speichers\n" +
				"-type - Ausgabe des Dateisystems\n" +
				"-free - Ausgabe des freien Speicherplatzes\n\n" +
				"poweroff OR shutdown - Herunterfahren des Systems\n\n" +
				"dir OR ls - Listet die Dateien auf\n\n" +
				"version - Ausgabe der aktuellen Version\n\n" +
				"runtime - Gibt die aktuelle Runtime zurück\n\n" +
				"echo [OPTIONS] - gibt die Optionen aus (mehrere Optionen moeglich)\n\n" +
				"user [OPTIONS] - kommt noch";

			string[] args = input.Split(" ");

			switch (args[0].ToLower())
			{
				case "help":
					{
						Console.WriteLine(possible);
						break;
					}
				case "space":
					{
						getSpace(args);
						break;
					}
				case "dir":
					{
						listdir();
						break;
					}
				case "ls":
					{
						listdir();
						break;
					}
				case "echo":
					{
						Echo(args);
						break;
					}
				case "cat":
					{
						catfile(args);
						break;
					}
				case "user":
					{
						break;
					}
				case "poweroff":
					{
						Environment.Exit(0);
						break;
					}
				case "shutdown":
					{
						Environment.Exit(0);
						break;
					}
				case "version":
					{
						Console.WriteLine($"Version: {versionString}");
						break;
					}
				case "runtime":
					{
						Console.WriteLine(runtime());
						break;
					}
				default:
					{
						Console.WriteLine("Invalid Command! Enter \"help\" for more informations!");
						break;
					}
			}

			//if(input == "adduser")
			//{
			//	Console.Write("Enter Username:");
			//	string username = Console.ReadLine();

			//	Console.Write("Enter Vorname:");
			//	string vorname = Console.ReadLine();

			//	Console.Write("Enter Nachname:");
			//	string nachname = Console.ReadLine();

			//	Console.Write("Enter Password:");
			//	string password = Console.ReadLine();

			//	Console.Write("Enter Email:");
			//	string email = Console.ReadLine();

			//	Console.Write("Enter isAdmin:");
			//	string isAdmin = Console.ReadLine();

			//	try
			//	{
			//		bool tmpadmin = true;
			//		User usr = new User(username, vorname, nachname, password, email, tmpadmin);
			//		tmp.Add(usr);
			//	}
			//	catch (Exception ex)
			//	{
			//		Console.WriteLine(ex.ToString());
			//		Thread.Sleep(5000);
			//	}
			//}

			//if (input == "listuser")
			//{
			//	Console.WriteLine("Username:Vorname:Nachname:Password:Email:tisAdmin");

			//	foreach (User usr in tmp)
			//	{
			//		Console.WriteLine(usr.getUsername() + ":" + usr.getVorname() + ":" + usr.getNachname() + ":" + usr.getPassword() + ":" + usr.getEmail() + ":" + usr.getStatus());
			//	}

			//	Console.WriteLine("Anzahl an Benutzern: " + tmp.Count);
			//}
		}
		public void getSpace(string[] args)
		{
			if(args.Length < 2)
			{
				Console.WriteLine("Enter an option!\n -free - get availible free Space ...");
			}
			else
			{
				for(int i = 1; i < args.Length; i++)
				{
					switch (args[i].ToLower())
					{
						case "-free":
							{
								Console.WriteLine("Availible Free Space: " + fs.GetAvailableFreeSpace(@"0:\"));
								break;
							}
						case "-type":
							{
								Console.WriteLine("Type: " + fs.GetFileSystemType(@"0:\"));
								break;
							}
						default:
							{

								break;
							}
					}
				}
			}
		}

		private string runtime()
		{
			TimeSpan span = DateTime.Now - dt_start;
			return String.Format("Laufzeit: {0} Hours {1} Minutes {2} Seconds", span.Hours, span.Minutes, span.Seconds);
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

		private void listdir()
		{
			var files_list = Directory.GetFiles(@"0:\");

			foreach (var file in files_list)
			{
				Console.WriteLine(file);
			}
		}

		private void catfile(string[] payload)
		{
			if(payload.Length > 1)
			{
				try
				{
					string contents = File.ReadAllText(@"0:\" + payload[1]);

					Console.WriteLine(contents);
				}
				catch(Exception ex)
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
