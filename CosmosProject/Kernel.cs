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
using System.Security.Cryptography;
using Microsoft.VisualBasic;
using static System.Net.Mime.MediaTypeNames;
using System.Data;

namespace CosmosProject
{
	public class Kernel : Sys.Kernel
	{
		public string CurrentPath = @"0:\\";

		public string UserConfigFile = @"0:\configs\users.txt";

		public char[] forbiddenchars = new char[]{
			'$',
			'%',
			'_',
		};

		public char[] forbiddenpwchars = new char[]{
			'ü',
			'Ü',
			'ä',
			'Ä',
			'ö',
			'Ö',
			' '
		};

		List<User> allusers;

		Sys.FileSystem.CosmosVFS fs = new Cosmos.System.FileSystem.CosmosVFS();

		DateTime dt_start = DateTime.Now;
		List<string> usedCommands;
		int statuscode;
		string possible;
		string versionString;
		protected override void BeforeRun()
		{
			Console.WriteLine("Cosmos booted successfully.");
			allusers = new List<User>();

			//allusers = getallUsers();

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
						if (args.Length == 1)
						{
							listdir(@"0:\");
							break;
						}
						else
						{
							listdir(@"0:\" + args[1]);
							break;
						}
					}
				case "ls":
					{
						if (args.Length == 1)
						{
							listdir(@"0:\");
							break;
						}
						else
						{
							listdir(@"0:\" + args[1]);
							break;
						}
					}
				case "write":
					{
						WriteLN(@"0:\configs\users.txt", "Testtest");
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
						controlUser(args);
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
		public List<User> getallUsers()
		{
			List<User> users = new List<User>();
			foreach (string line in File.ReadLines(UserConfigFile))
			{
				string[] splitted = line.Split(":");
				users.Add(new User(splitted[0], splitted[1], splitted[2], splitted[3], splitted[4]));
			}

			return users;
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

		public bool isvalidpw(string password)
		{
			bool valid = false;
			if (password == null)
			{
				return valid = false;
			}
			else
			{
				if (password.Length < 6)
				{
					Console.WriteLine("Password is to short - min. 6 characters");
					return valid = false;
				}

				foreach (char c in forbiddenpwchars)
				{
					if (password.Contains(c))
					{
						Console.WriteLine("Invalid Character {0} in password", c);
						return valid = false;
					}
				}

				return valid = true;
			}
			return valid = false;
		}

		public void WriteLN(string path, string text)
		{
			Console.WriteLine(path);

			if (File.Exists(path))
			{
				using (StreamWriter sw = File.AppendText(path))
				{
					sw.WriteLine(text);
					sw.Close();
				}
			}
			else
			{
				Console.WriteLine("File not exists");
			}
		}

		public bool CheckFile(string path)
		{
			try
			{
				bool check = File.Exists(@"0:\" + path);
				return check;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}
		public void controlUser(string[] payload)
		{
			if (payload.Length == 1)
			{
				Console.WriteLine("no argumnets");
			}
			else
			{
				switch (payload[1].ToLower())
				{
					case "-add":
						{
							Console.Write("Enter Username:");
							string username = Console.ReadLine();

							Console.Write("Enter Vorname:");
							string vorname = Console.ReadLine();

							Console.Write("Enter Nachname:");
							string nachname = Console.ReadLine();

							string password = "";
							while (!isvalidpw(password))
							{
								Console.Write("Enter Password:");
								password = Console.ReadLine();

								Console.Write("Enter Password again:");
								string passwordretype = Console.ReadLine();

								if (password != passwordretype)
								{
									Console.WriteLine("Password not the same");
									return;
								}
							}
							Console.Write("Enter Email:");
							string email = Console.ReadLine();

							User usr = new User(username, vorname, nachname, password, email);
							string userstring = usr.getUsername() + ":" + usr.getVorname() + ":" + usr.getNachname() + ":" + usr.getPassword().GetHashCode().ToString() + ":" + usr.getEmail();

							Console.WriteLine(userstring);

							string usercnfFile = @"0:\configs\users.txt";

							WriteLN(usercnfFile, userstring);

							break;
						}
					case "-list":
						{
							allusers = getallUsers();

							foreach(User u in allusers)
							{
								Console.WriteLine(u.getUsername() + "||" + u.getVorname() + "||" + u.getNachname() + "||" + u.getPassword() + "||" + u.getEmail());
							}
						}

						//if (File.Exists(usercnfFile))
						//{
						//	Wr

						//	File.WriteAllText(usercnfFile, content +"\n" + userstring);
						//	return;
						//}
						//else
						//{
						//	Console.WriteLine("Config-File for Users doesn´t exists!");
						//	return;
						//}							

						//

						//string configUser = @"0:\users\configs.txt";
						//if (File.Exists(@"0:\users\configs.txt"))
						//{
						//	string configUser = @"0:\users\configs.txt";
						//	User usr = new();
						//	string contents = File.ReadAllText(configUser);
						//	string userstring = usr.getUsername() + ":" + usr.getVorname() + ":" + usr.getNachname() + ":" + ComputeSha256Hash(usr.getPassword()) + ":" + usr.getEmail();

						//	File.WriteAllText(configUser, contents + userstring);
						//}
						//else
						//{
						//	Console.WriteLine("Config not exists");
						//}
						break;
				
					default:
						{
							Console.WriteLine("Invalid Command! Enter \"help\" for more informations!");
							break;
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
		private void listdir(string payload)
		{
			if (!Directory.Exists(payload))
			{
				Console.WriteLine($"{payload} is not a file or directory");
				return;
			}

			var directory_list = fs.GetDirectoryListing(payload);
	
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
			if (payload.Length > 1 && File.Exists(@"0:\" + payload[1]))
			{
				try
				{
					string contents = File.ReadAllText(@"0:\" + payload[1]);

					if (String.IsNullOrEmpty(contents) || contents.Equals("\n"))
					{
						contents = "";
					}

					string input = "";
					string text = "";

					while (true)
					{
						input = Console.ReadLine();

						if (input.Equals("quit", StringComparison.OrdinalIgnoreCase))
						{
							break;
						}

						text += input + "\n";
					}

					try
					{
						File.WriteAllText(@"0:\" + payload[1],contents +  text);
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
				Console.WriteLine("Invalid Usage or argument is not a file! Enter \"help\" for more informations");
				return 1;
			}
		}

		private int createfile(string[] payload)
		{
			if (payload.Length >= 2)
			{
				foreach(char c in forbiddenchars)
				{
					if (payload[1].Contains(c))
					{
						Console.WriteLine("Invalid char " + c + " in filename");
						return -1;
					}
				}
				try
				{
					if (File.Exists(CurrentPath + payload[1]))
					{
						
						//Aufruf mit -y Argument dann wird die Datei direkt überschrieben
						if(payload.Length == 3)
						{
							if (payload[2] == "-y")
							{
								fs.CreateFile(CurrentPath + payload[1]);
								//File.Create(@"0:\" + payload[1]);
								Console.WriteLine("Datei wurde ueberschrieben!");
								return 0;
							}
						}

						Console.WriteLine("Datei existiert bereits! Moechtest du die Datei ueberschreiben?");
						string input = Console.ReadLine();

						input = input.ToLower();

						if (input == "y" || input == "j")
						{
							fs.CreateFile(CurrentPath + payload[1]);

							//File.Create(@"0:\" + payload[1]);
							Console.WriteLine("Datei wurde ueberschrieben!");
							return 0;
						}
						else if(input == "n")
						{
							Console.WriteLine("Datei wurde nicht ueberschrieben!");
							return 2;
						}
						else
						{
							Console.WriteLine("Datei wurde nicht ueberschrieben!");
							return 2;
						}
					}
					else
					{
						fs.CreateFile(CurrentPath + payload[1]);
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



		//public bool Create(string command)
		//{
		//	string[] args = command.Split(" ");

		//	if(args.Length < 3)
		//	{
		//		Console.WriteLine("To few Arguments");
		//		return false;
		//	}
		//	//Create File local
		//	else if(args.Length == 2)
		//	{
		//		string filename = args[1];

		//		foreach(char c in forbiddenchars)
		//		{
		//			if (filename.Contains(c))
		//			{
		//				Console.WriteLine("Invalid chars");
		//				return false;
		//			}
		//		}

		//		if (Directory.Exists(CurrentPath + filename))
		//		{
		//			Console.WriteLine("File exists");
		//			return false;
		//		}
				
		//		//Für Berechtigungen muss man eine Klasse erstellen, die angibt, welche leute wo schreiben können

		//		File.Create(CurrentPath + filename);	
		//		return true;

		//	}
		//	//CReate file in Directory
		//	else if(args.Length == 3)
		//	{

		//	}
		//	else
		//	{
		//		string location = args[1];
		//		string filename = args[2];

		//		if (CheckFile(CurrentPath+filename))
		//		{
		//			Console.WriteLine("File exists");
		//		}
		//		else
		//		{
		//			if (Directory.Exists(CurrentPath + location + filename))
		//			{

		//			}
		//		}
		//	}

			
		//}
	}
}
