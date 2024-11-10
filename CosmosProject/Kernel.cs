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
		public static string CurrentPath = @"0:\\";

		public static string UserConfigFile = @"0:\configs\users.txt";

		public static char[] forbiddenchars = new char[]{
			'$',
			'%',
			'_',
		};

		public static char[] forbiddenpwchars = new char[]{
			'ü',
			'Ü',
			'ä',
			'Ä',
			'ö',
			'Ö',
			' '
		};

		public static List<User> allusers;
		public static List<User> adminusers;
		public static List<User> normalusers;
		public static User CurrentUser = new User();

		public int cntusers;
		public int cntadmin;
		private int welcome = 0;
		public static Sys.FileSystem.CosmosVFS fs = new Cosmos.System.FileSystem.CosmosVFS();

		public static DateTime dt_start;
		List<string> usedCommands;
		static int statuscode;
		string possible;
		string versionString;
		Commands comm = new Commands();
		protected override void BeforeRun()
		{
			dt_start = DateTime.Now;

			cntadmin = 0;
			cntusers = 0;

			normalusers = new List<User>();
			adminusers = new List<User>();

			Console.WriteLine("Cosmos booted successfully.");
			allusers = new List<User>();

			//allusers = getallUsers();

			usedCommands = new List<string>();

			Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);

			//possible = "moegliche Commands:\n" +
			//	"space [OPTIONS] - Verwaltung des Speichers\n" +
			//	"-type - Ausgabe des Dateisystems\n" +
			//	"-free - Ausgabe des freien Speicherplatzes\n--------\n" +
			//	"poweroff OR shutdown - Herunterfahren des Systems\n\n" +
			//	"dir OR ls - Listet die Dateien auf\n--------\n" +
			//	"cat [OPTIONS] OR type [OPTIONS] - Gibt den Inhalt einer Datei auf dem Terminal aus\n--------\n" +
			//	"version - Ausgabe der aktuellen Version\n--------\n" +
			//	"runtime - Gibt die aktuelle Runtime zurück\n--------\n" +
			//	"mkdir [OPTIONS] - Erstellt ein Verzeichnis mit dem angegebenen Namen\n--------\n" +
			//	"touch [OPTIONS] - Erstellt eine Datei mit dem angegebenen Namen\n--------\n" +
			//	"errcode - Gibt den letzten Exit-Code eines Programms zurück\n--------\n" +
			//	"editfile [OPTIONS] - fuegt eine Zeile in einer Datei hinzu\n--------\n" +
			//	"rm [OPTIONS] - loescht eine Datei\n--------\n" +
			//	"echo [OPTIONS] - gibt die Optionen aus (mehrere Optionen moeglich)\n--------\n" +
			//	"user [OPTIONS] - kommt noch\n" + 
			//	"-add - Anlegen eines neuen Users in Config-File\n" +
			//	"-list - Ausgabe alles bereits angelegten Users\n--------\n";

			versionString = "0.6";
		}
		protected override void Run()
		{
			if (!File.Exists(UserConfigFile))
			{
				fs.CreateFile(UserConfigFile);
			}

			try
			{
				allusers.Clear();
				adminusers.Clear();
				normalusers.Clear();
				allusers = UserControls.getallUsers();

			}
			catch (Exception ex) {Console.WriteLine(ex.Message); return; }
			

			foreach (User tmp in allusers)
			{
				//Console.WriteLine(tmp.getUsername());
				//Console.WriteLine(tmp.getVorname());
				//Console.WriteLine(tmp.getNachname());
				//Console.WriteLine(tmp.getEmail());
				//Console.WriteLine(tmp.getPerm());
				//Console.WriteLine(tmp.getPassword());
				if (tmp.getPerm() == 0)
				{
					cntusers++;
					normalusers.Add(tmp);
				}
				else if (tmp.getPerm() == 1)
				{
					cntadmin++;
					adminusers.Add(tmp);
				}
			}

			if (cntadmin == 0)
			{
				Console.WriteLine("Es existiert kein Administrator!!");
				Console.WriteLine("Bitte legen Sie einen an:");

				Console.Write("Enter Username:");
				string username = Console.ReadLine();

				Console.Write("Enter Vorname:");
				string vorname = Console.ReadLine();

				Console.Write("Enter Nachname:");
				string nachname = Console.ReadLine();

				string password = "";
				while (!UserControls.isvalidpw(password))
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

				User usr = new User(username, vorname, nachname, password, email, 1);
				string userstring = usr.getUsername() + ":" + usr.getVorname() + ":" + usr.getNachname() + ":" + User.GenerateHash(usr.getPassword()) + ":" + usr.getEmail() + ":" + usr.getPerm();

				WriteLN(Kernel.UserConfigFile, userstring);

				Console.WriteLine("Administrator wurde angelegt.");
			}

			while (CurrentUser.getUsername() == "" && CurrentUser.getVorname() == "" && CurrentUser.getNachname() == "")
			{
				Console.Clear();
				welcome = 0;
				Console.WriteLine("You are not logged in.");
				Console.WriteLine("Please enter an username and password!");
				Console.WriteLine("Username: ");
				string username = Console.ReadLine();
				Console.WriteLine("Password: ");
				string password = Console.ReadLine();
				Console.WriteLine(User.GenerateHash(password));

				UserControls.login(username, password);
			}

			if(welcome == 0)
			{
				Console.Clear();
				Console.WriteLine("USER LOGGED IN");
				Console.WriteLine("Hello {0} {1}, Welcome back!", CurrentUser.getVorname(), CurrentUser.getNachname());
				Console.WriteLine("System-Version {0}", versionString);
				Console.WriteLine("System-Start {0}", dt_start);
				Console.WriteLine("================================");
				welcome++;
			}


			Console.Write("Enter Command: ");

			string input = Console.ReadLine();
			usedCommands.Add(input);

			string[] args = input.Split(" ");

			comm.commands(args);

			//switch (args[0].ToLower())
			//{
			//	case "help":
			//		{
			//			Console.WriteLine(possible);
			//			break;
			//		}
			//	case "space":
			//		{
			//			getSpace(args);
			//			break;
			//		}
			//	//case "dir":
			//	//	{
			//	//		if (args.Length == 1)
			//	//		{
			//	//			listdir(@"0:\");
			//	//			break;
			//	//		}
			//	//		else
			//	//		{
			//	//			listdir(@"0:\" + args[1]);
			//	//			break;
			//	//		}
			//	//	}
			//	//case "ls":
			//	//	{
			//	//		if (args.Length == 1)
			//	//		{
			//	//			listdir(@"0:\");
			//	//			break;
			//	//		}
			//	//		else
			//	//		{
			//	//			listdir(@"0:\" + args[1]);
			//	//			break;
			//	//		}
			//	//	}
			//	case "write":
			//		{
			//			WriteLN(@"0:\configs\users.txt", "Testtest");
			//			break;
			//		}
			//	//case "echo":
			//	//	{
			//	//		Echo(args);
			//	//		break;
			//	//	}
			//	//case "cat":
			//	//	{
			//	//		catfile(args);
			//	//		break;
			//	//	}
			//	//case "type":
			//	//	{
			//	//		catfile(args);
			//	//		break;
			//	//	}
			//	//case "user":
			//	//	{
			//	//		controlUser(args);
			//	//		break;
			//	//	}
			//	//case "poweroff":
			//	//	{
			//	//		Environment.Exit(0);
			//	//		break;
			//	//	}
			//	//case "shutdown":
			//	//	{
			//	//		Environment.Exit(0);
			//	//		break;
			//	//	}
			//	//case "version":
			//	//	{
			//	//		Console.WriteLine($"Version: {versionString}");
			//	//		break;
			//	//	}
			//	//case "runtime":
			//	//	{
			//	//		Console.WriteLine(runtime());
			//	//		break;
			//	//	}
			//	//case "mkdir":
			//	//	{
			//	//		statuscode = makedir(args);
			//	//		break;
			//	//	}
			//	//case "touch":
			//	//	{
			//	//		statuscode = createfile(args);
			//	//		break;
			//	//	}
			//	case "errcode":
			//		{
			//			Console.WriteLine(statuscode.ToString());
			//			break;
			//		}
			//	//case "editfile":
			//	//	{
			//	//		statuscode = editfile(args);
			//	//		break;
			//	//	}
			//	//case "rm":
			//	//	{
			//	//		statuscode = deletefile(args);
			//	//		break;
			//	//	}
			//	//case "cls":
			//	//	{
			//	//		Console.Clear();
			//	//		break;
			//	//	}
			//	//case "clear":
			//	//	{
			//	//		Console.Clear();
			//	//		break;
			//	//	}
			//	default:
			//		{
			//			Console.WriteLine("Invalid Command! Enter \"help\" for more informations!");
			//			break;
			//		}
			//}

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
	}
}
