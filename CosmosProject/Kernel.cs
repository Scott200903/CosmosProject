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
using CosmosProject.fsystem;
using Cosmos.System.FileSystem.Listing;
using System.ComponentModel.Design;

namespace CosmosProject
{
	public class Kernel : Sys.Kernel
	{
		List<User> tmp;
		Sys.FileSystem.CosmosVFS fs = new Cosmos.System.FileSystem.CosmosVFS();

		DateTime dt_start = DateTime.Now;
		List<string> usedCommands;
		int statuscode;
		string possible;
		string versionString;
		protected override void BeforeRun()
		{
			Console.WriteLine("Cosmos booted successfully.");
			tmp = new List<User>();

			usedCommands = new List<string>();



			Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);

			possible = "moegliche Commands:\n" +
				"space [OPTIONS] - Verwaltung des Speichers\n" +
				"-type - Ausgabe des Dateisystems\n" +
				"-free - Ausgabe des freien Speicherplatzes\n--------\n" +
				"poweroff OR shutdown - Herunterfahren des Systems\n\n" +
				"dir OR ls - Listet die Dateien auf\n--------\n" +
				"cat [OPTIONS] OR type [OPTIONS] - Gibt den Inhalt einer Datei auf dem Terminal aus\n--------\n" +
				"version - Ausgabe der aktuellen Version\n--------\n" +
				"runtime - Gibt die aktuelle Runtime zurück\n--------\n" +
				"mkdir [OPTIONS] - Erstellt ein Verzeichnis mit dem angegebenen Namen\n--------\n" +
				"touch [OPTIONS] - Erstellt eine Datei mit dem angegebenen Namen\n--------\n" +
				"errcode - Gibt den letzten Exit-Code eines Programms zurück\n--------\n" +
				"editfile [OPTIONS] - fuegt eine Zeile in einer Datei hinzu\n--------\n" +
				"rm [OPTIONS] - loescht eine Datei\n--------\n" +
				"echo [OPTIONS] - gibt die Optionen aus (mehrere Optionen moeglich)\n--------\n" +
				"user [OPTIONS] - kommt noch\n";

			versionString = "0.1";
		}
		protected override void Run()
		{
			//Help
			//version
			// args[] für verschiedene Argumente

			Console.Write("Enter Command: ");

			string input = Console.ReadLine();
			usedCommands.Add(input);

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
						listdir(@"0:\");
						break;
					}
				case "ls":
					{
						listdir(@"0:\");
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
				case "type":
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
				case "mkdir":
					{
						statuscode = makedir(args);
						break;
					}
				case "touch":
					{
						statuscode = createfile(args);
						break;
					}
				case "errcode":
					{
						Console.WriteLine(statuscode.ToString());
						break;
					}
				case "editfile":
					{
						statuscode = editfile(args);
						break;
					}
				case "rm":
					{
						statuscode = deletefile(args);
						break;
					}
				case "cls":
					{
						Console.Clear();
						break;
					}
				case "clear":
					{
						Console.Clear();
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
			if (args.Length < 2)
			{
				Console.WriteLine("Enter an option!\n -free - get availible free Space ...");
			}
			else
			{
				for (int i = 1; i < args.Length; i++)
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
								Console.WriteLine("no options! Enter \"help\" for more informations!"); break;
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

		private void listdir(string root)
		{
			//var directory_list = Directory.GetDirectories(root);

			////var directory_list = Directory.GetDirectories(@"0:\");
			//string[] files_list = new string[300];

			//foreach (var dire in directory_list)
			//{
			//	files_list = Directory.GetFiles();
			//	Console.WriteLine(dire);
			//	foreach (var file in files_list)
			//	{
			//		Console.WriteLine("\t" + file);
			//	}
			//	listdir(dire);
			//}

			//Console.WriteLine("------------------------------------");

			//files_list = Directory.GetFiles(@"0:\");
			//foreach (var file in files_list)
			//{
			//	Console.WriteLine(file);

			var directory_list = fs.GetDirectoryListing(root);

			foreach (var directory in directory_list)
			{
				Console.WriteLine($"{directory.mFullPath}");

				if (File.Exists(directory.mFullPath))
				{
				}
				else
				{
					listdir($"{directory.mFullPath}");
				}
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
		private int makedir(string[] payload)
		{
			if (payload.Length == 2)
			{
				try
				{
					fs.CreateDirectory(@"0:\" + payload[1]);
					return 0;
				}
				catch (Exception e)
				{
					Console.WriteLine(e.ToString());
					return 2;
				}
			}
			else
			{
				Console.WriteLine("Invalid Usage! Enter \"help\" for more informations");
				return 1;
			}
		}

		private int editfile(string[] payload)
		{
			if (payload.Length > 1)
			{
				try
				{
					string contents = File.ReadAllText(@"0:\" + payload[1]);

					Console.WriteLine(contents);

					var input = Console.ReadLine();

					string text = contents + "\n" + input;

					try
					{
						File.WriteAllText(@"0:\" + payload[1], text);
						return 0;
					}
					catch (Exception e)
					{
						Console.WriteLine(e.ToString());
						return 3;
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e.ToString());
					return 2;
				}
			}
			else
			{
				Console.WriteLine("Invalid Usage! Enter \"help\" for more informations");
				return 1;
			}
		}

		private int createfile(string[] payload)
		{
			if (payload.Length == 2)
			{
				try
				{
					if (File.Exists(@"0:\" + payload[1]))
					{
						Console.WriteLine("Datei existiert bereits! Moechtest du die Datei ueberschreiben?");
						string input = Console.ReadLine();

						input = input.ToLower();

						if (input == "y" || input == "j")
						{
							fs.CreateFile(@"0:\" + payload[1]);

							//File.Create(@"0:\" + payload[1]);
							Console.WriteLine("Datei wurde ueberschrieben!");
							return 0;
						}
						else
						{
							Console.WriteLine("Datei wurde nicht ueberschrieben!");
							return 2;
						}
					}
					else
					{
						fs.CreateFile(@"0:\" + payload[1]);
						return 0;
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e.ToString());
					return 1;
				}
			}
			else
			{
				Console.WriteLine("Invalid Usage! Enter \"help\" for more informations");
				return 1;
			}
		}

		private int deletefile(string[] payload)
		{
			if (payload.Length >= 3)
			{
				try
				{
					var directory_list = fs.GetDirectoryListing(@"0:\");

					if (payload[1] == "--file" || payload[1] == "-f")
					{
						if(File.Exists(@"0:\" + payload[2]))
						{
							File.Delete(@"0:\" + payload[2]);
							return 0;
						}
						else
						{
							Console.WriteLine("No such File or Directory!");
							return 2;
						}
					}
					else if (payload[1] == "--dir" || payload[1] == "-d")
					{
						Directory.Delete(@"0:\" + payload[2]);
						return 0;
					}
					else
					{
						Console.WriteLine("Invalid Usage! Enter \"help\" for more informations");
						return -1;
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e.ToString());
					return 1;
				}

			}
			else
			{
				Console.WriteLine("Invalid Usage! Enter \"help\" for more informations");
				return 1;
			}


			//	if (payload.Length == 2)
			//	{
			//		try
			//		{
			//			if (File.Exists(@"0:\" + payload[1]))
			//			{
			//				fs.DeleteFile(@"0:\" + payload[1]);
			//				return 0;
			//			}
			//			else
			//			{
			//				return 3;
			//			}
			//		}
			//		catch (Exception ex)
			//		{
			//			Console.WriteLine(ex.ToString());
			//			return 2;
			//		}
			//	}
		}
	}
}
