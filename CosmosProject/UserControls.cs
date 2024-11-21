using Cosmos.System.Network.IPv4.TCP;
using CosmosProject.UserManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CosmosProject
{
	public class UserControls
	{
		private string currentVersion = "Version 0.3";
		private Dictionary<string, Action<string[]>> commandMap;

		public UserControls()
		{
			InitializeUserControls();
		}
		private void InitializeUserControls()
		{
			commandMap = new Dictionary<string, Action<string[]>>
			{
				{ "user", args => controlUser(args) },
				{ "logout", args => Kernel.CurrentUser = new User() },
				{ "chmod", args => Chmod(args) },
				{ "passwd", args => ChangePassword(Kernel.CurrentUser) }
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

		public static bool login(string username, string password)
		{
			if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
			{
				foreach (User x in Kernel.allusers)
				{
					if (x.getUsername() == username)
					{
						if (x.getPassword().Equals(User.GenerateHash(password)))
						{
							Kernel.CurrentUser = x;
							return true;
						}
						else
						{
							Console.WriteLine("username or password is wrong! try again.");
							return false;
						}
					}
					else
					{
						Console.WriteLine("user not found");
					}
				}
				Console.WriteLine("No User with username {0} found! Please try again!", username);
				return false;
			}
			return false;
		}

		public static List<User> getallUsers()
		{
			List<User> users = new List<User>();
			int linecnt = 0;
			foreach (string line in File.ReadLines(Kernel.UserConfigFile))
			{
				//Console.WriteLine(linecnt);
				//linecnt++;
				if(line == "" || String.IsNullOrEmpty(line))
				{
					//Console.WriteLine("line empty");
					return new List<User>();
				}
				string[] splitted = line.Split(":");
				if (splitted.Length == 0)
				{
					Console.WriteLine("splitted length 0");
					return new List<User>();
				}
				else
				{
					if(splitted.Length < 6)
					{
						Console.WriteLine("Format from {0} is wrong! File will be reseted!", Kernel.UserConfigFile);
						File.Delete(Kernel.UserConfigFile);
						Kernel.CurrentUser = new User();
						return new List<User>();
					}
					//Console.WriteLine("User add");
					int permlevel = int.Parse(splitted[5]);
					users.Add(new User(splitted[0], splitted[1], splitted[2], splitted[3], splitted[4], permlevel));
					//Console.WriteLine("User added");
				}
			}
			return users;
		}

		public static bool isvalidpw(string password)
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

				foreach (char c in Kernel.forbiddenpwchars)
				{
					if (password.Contains(c))
					{
						Console.WriteLine("Invalid Character {0} in password", c);
						return valid = false;
					}
				}
				int cntupper = 0;
				foreach(char c in password.ToCharArray())
				{
					if(char.IsUpper(c))
					{
						cntupper++;
					}
				}
				int cntnum = 0;
				foreach (char c in password.ToCharArray())
				{
					if (char.IsNumber(c))
					{
						cntnum++;
					}
				}
				if( cntupper < 2 && cntnum < 2)
				{
					Console.WriteLine("Count of Digits or Upper-Letters are to few (min. 2 Chars)");
					return valid = false;
				}

				return valid = true;
			}
			return valid = false;
		}

		public static void WriteLN(string path, string text)
		{
			//Console.WriteLine(path);

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

		public void controlUser(string[] payload)
		{
			if (payload.Length == 1)
			{
				Console.WriteLine("To few arguments!");
				Console.WriteLine("Enter \"user help\" to see more informations");
			}
			else
			{
				switch (payload[1].ToLower())
				{
					case "--add":
						{
							if(Kernel.CurrentUser.getPerm() == 1)
							{
								AddUser();
								break;
							}
							else
							{
								NoPermissions();
								break;
							}
							//bool sameusername;
							//string username = "";
							//do
							//{
							//	sameusername = false;
							//	Console.Clear();
							//	Console.WriteLine("Create a new user\n=================");
							//	Console.Write("Enter Username:");
							//	username = Console.ReadLine();

							//	Kernel.allusers = getallUsers();

							//	foreach (User ex in Kernel.allusers)
							//	{
							//		if (ex.getUsername().Equals(username))
							//		{
							//			Console.WriteLine("user with username \"{0}\" already exists!", username);
							//			sameusername = true;
							//			Thread.Sleep(3000);
							//		}
							//	}
							//	if(sameusername == false)
							//	{
							//		sameusername = false;
							//	}
							//} while (sameusername == true);


							//Console.Write("Enter Vorname:");
							//string vorname = Console.ReadLine();

							//Console.Write("Enter Nachname:");
							//string nachname = Console.ReadLine();

							//string password = "";
							//do
							//{
							//	Console.Write("Enter Password:");
							//	password = Console.ReadLine();

							//	if(isvalidpw(password) == true)
							//	{
							//		Console.Write("Enter Password again:");
							//		string passwordretype = Console.ReadLine();

							//		if (password != passwordretype)
							//		{
							//			Console.WriteLine("Password not the same");
							//			return;
							//		}
							//		break;
							//	}

							//} while (isvalidpw(password) == false);
							
							//Console.Write("Enter Email:");
							//string email = Console.ReadLine();

							//User usr = new User(username, vorname, nachname, password, email, 0);
							//string userstring = usr.getUsername() + ":" + usr.getVorname() + ":" + usr.getNachname() + ":" + User.GenerateHash(usr.getPassword()) + ":" + usr.getEmail() + ":" + usr.getPerm().ToString();

							//Console.WriteLine(userstring);

							//string usercnfFile = @"0:\configs\users.txt";

							//WriteLN(usercnfFile, userstring);

							break;
						}
					case "--list":
						{
							Kernel.allusers = getallUsers();

							foreach (User u in Kernel.allusers)
							{
								Console.WriteLine(u.getUsername() + "||" + u.getVorname() + "||" + u.getNachname() + "||" + u.getPassword() + "||" + u.getEmail() + "||" + u.getPerm());
							}
							break;
						}
					case "--delete":
						{
							deleteUser(payload);
							break;
						}
					case "--edit":
						{
							if (Kernel.CurrentUser.getPerm() == 1)
							{
								EditUser();
								break;
							}
							else
							{
								NoPermissions();
								break;
							}

						}
					case "help":
						{
							helpFileControls();
							break;
						}
					default:
						{
							Console.WriteLine("Invalid Command! Enter \"user help\" for more informations!");
							break;
						}
				}
			}
		}

		private void EditUser()
		{
			User choosedUser = new User();

			if(Kernel.CurrentUser.getPerm() != 1)
			{
				NoPermissions();
			}
			do
			{
				Console.Clear();
				Console.WriteLine("Editing users\n=================");

				foreach(User u in Kernel.allusers)
				{
					Console.WriteLine(u.getUsername() + "||" + u.getVorname() + "||" + u.getNachname() + "||" + u.getPassword() + "||" + u.getEmail() + "||" + u.getPerm());
				}

				Console.Write("Choose a username: ");
				string choosedusername = Console.ReadLine();

				foreach(User u in Kernel.allusers)
				{
					if(u.getUsername() == choosedusername)
					{
						choosedUser = u; break;
					}
				}

				if(choosedUser.getUsername() != "")
				{
					EditUserProperties(choosedUser);
				}
				else
				{
					Console.Write("Choosed a user not exists! Try another one");
					Thread.Sleep(3000);
				}

			} while (choosedUser.getUsername() == "");
		}
		public void EditUserProperties(User choosedUser)
		{
			Console.Clear();
			Console.WriteLine("Editing {0}, {1} {2}\n=================", choosedUser.getUsername(), choosedUser.getVorname(), choosedUser.getNachname());

			Console.WriteLine("Username: " + choosedUser.getUsername());
			Console.WriteLine("Vorname: " + choosedUser.getVorname());
			Console.WriteLine("Nachname: " + choosedUser.getNachname());
			Console.WriteLine("E-Mail-Adresse: " + choosedUser.getEmail());
			Console.WriteLine("Berechtigung: " + choosedUser.getPerm());
			Console.WriteLine("Passwort (Hashed): " + choosedUser.getPassword());
			Console.WriteLine();
			Console.WriteLine("Welche Eigenschaften möchtest du aendern? (mehrere moeglich)");

			string props = Console.ReadLine();

			string[] splitprops = props.Split(" ");

			User beforeEdit = new User(choosedUser);

			User tmpUser = choosedUser;

			foreach (string s in splitprops)
			{
				switch (s.ToLower())
				{
					case "username":
						{
							Console.WriteLine("Old " + s + ": " + choosedUser.getUsername());
							Console.Write("New " + s + ": ");
							string newUsername = Console.ReadLine();

							beforeEdit.setUsername(newUsername);
							break;
						}
					case "vorname":
						{
							Console.WriteLine("Old " + s + ": " + choosedUser.getVorname());
							Console.Write("New " + s + ": ");
							string newVorname = Console.ReadLine();

							beforeEdit.setVorname(newVorname);
							break;
						}
					case "nachname":
						{
							Console.WriteLine("Old " + s + ": " + choosedUser.getNachname());
							Console.Write("New " + s + ": ");
							string newNachname = Console.ReadLine();

							beforeEdit.setNachname(newNachname);
							break;
						}
					case "email":
						{
							Console.WriteLine("Old " + s + ": " + choosedUser.getEmail());
							Console.Write("New " + s + ": ");
							string newEmail = Console.ReadLine();

							beforeEdit.setEmail(newEmail);
							break;
						}
					case "berechtigung":
						{
							Console.WriteLine("Old " + s + ": " + choosedUser.getPerm());
							Console.Write("New " + s + ": ");
							int newPerm = Convert.ToInt16(Console.ReadLine());

							beforeEdit.setPerm(newPerm);
							break;
						}
					case "passwort":
						{
							Console.WriteLine("Old " + s + ": " + choosedUser.getPassword());
							Console.Write("New " + s + ": ");
							string newPasswort = Console.ReadLine();

							beforeEdit.setPassword(newPasswort);
							break;
						}
					default:
						{
							Console.WriteLine("Angegebene Eigenschaft gibt es nicht!\nMoeglichkeiten sind:\nvorname, nachname," +
											  "username, passwort, emails, berechtigung");
							return;
						}
				}
			}
			Console.Clear();
			Console.WriteLine("Bist du sicher den User zu aendern?");
			Console.WriteLine("Alte Eigenschaften:\n");
			Console.WriteLine("Username: " + choosedUser.getUsername());
			Console.WriteLine("Vorname: " + choosedUser.getVorname());
			Console.WriteLine("Nachname: " + choosedUser.getNachname());
			Console.WriteLine("E-Mail-Adresse: " + choosedUser.getEmail());
			Console.WriteLine("Berechtigung: " + choosedUser.getPerm());
			Console.WriteLine("Passwort (Hashed): " + choosedUser.getPassword());
			Console.WriteLine("===============================================");
			Console.WriteLine("Neue Eigenschaften:\n");
			Console.WriteLine("Username: " + beforeEdit.getUsername());
			Console.WriteLine("Vorname: " + beforeEdit.getVorname());
			Console.WriteLine("Nachname: " + beforeEdit.getNachname());
			Console.WriteLine("E-Mail-Adresse: " + beforeEdit.getEmail());
			Console.WriteLine("Berechtigung: " + beforeEdit.getPerm());
			Console.WriteLine("Passwort (Hashed): " + beforeEdit.getPassword());
			bool valid = false;
			do
			{
				valid = false;
				Console.Write("Sollen die Eigenschaften uebernommen werden?(y/n) ");
				char confirm = Convert.ToChar(Console.ReadLine().Substring(0, 1));

				if (confirm == 'y' || confirm == 'Y')
				{
					FileControls.FindandReplace(Kernel.UserConfigFile, choosedUser.getUsername(), beforeEdit);
					Console.WriteLine("Properties edited!");
					valid = true;
				}
				else if (confirm == 'n' || confirm == 'N')
				{
					Console.WriteLine("Properties not edited!");
					valid = true;
				}
				else
				{
					Console.WriteLine("Invalid input");
					valid = false;
				}
			} while (valid != true);


			return;
		}
		private void AddUser()
		{
			bool sameusername;
			string username = "";
			do
			{
				sameusername = false;
				Console.Clear();
				Console.WriteLine("Create a new user\n=================");
				Console.Write("Enter Username:");
				username = Console.ReadLine();
				if(username == "q")
				{
					return;
				}

				Kernel.allusers = getallUsers();

				foreach (User ex in Kernel.allusers)
				{
					if (ex.getUsername().Equals(username))
					{
						Console.WriteLine("user with username \"{0}\" already exists!", username);
						sameusername = true;
						Thread.Sleep(3000);
					}
				}
				if (sameusername == false)
				{
					sameusername = false;
				}
			} while (sameusername == true);


			Console.Write("Enter Vorname:");
			string vorname = Console.ReadLine();

			Console.Write("Enter Nachname:");
			string nachname = Console.ReadLine();

			string password = "";
			do
			{
				Console.Write("Enter Password:");
				password = Console.ReadLine();

				if (isvalidpw(password) == true)
				{
					Console.Write("Enter Password again:");
					string passwordretype = Console.ReadLine();

					if (password != passwordretype)
					{
						Console.WriteLine("Password not the same");
						return;
					}
					break;
				}

			} while (isvalidpw(password) == false);

			Console.Write("Enter Email:");
			string email = Console.ReadLine();

			User usr = new User(username, vorname, nachname, password, email, 0);
			string userstring = usr.getUsername() + ":" + usr.getVorname() + ":" + usr.getNachname() + ":" + User.GenerateHash(usr.getPassword()) + ":" + usr.getEmail() + ":" + usr.getPerm().ToString();

			Console.WriteLine(userstring);

			WriteLN(Kernel.UserConfigFile, userstring);

			return;
		}
		public static void NoPermissions()
		{
			Console.WriteLine("You have no permissions to edit users!\nTry to login with Administrator-Account");
			return;
		}
		public void Chmod(string[] payload)
		{
			if(Kernel.CurrentUser.getPerm() != 1)
			{
				NoPermissions();
			}
			else
			{
				if (payload.Length < 3)
				{
					Console.WriteLine("Wrong usage! \"chmod <username> <permission>\"\n<username> try \"user --list\" and <permission> 1 administrator and 0 normal user");
					return;
				}
				else
				{
					if (int.Parse(payload[2]) == 1 || int.Parse(payload[2]) == 0)
					{
						bool changed = false;
						foreach (User tmp in Kernel.allusers)
						{
							if (tmp.getUsername() == payload[1])
							{
								tmp.setPerm(int.Parse(payload[2]));
								//Console.WriteLine("User found!");
								FileControls.FindandReplace(Kernel.UserConfigFile, tmp.getUsername(), tmp);
								changed = true;
							}
						}

						if(changed == false)
						{
							Console.WriteLine("User not found: {0}", payload[1]);
							return;
						}
						else
						{
							Console.WriteLine("Permission changed!");
							return;
						}
						return;
					}
					else
					{
						Console.WriteLine("Permission is not allowed! Only 0 and 1");
						return;
					}
					
				}
			}
		}

		public void ChangePassword(User user)
		{
			Console.WriteLine("Change password from {0}, {1} {2}", user.getUsername(), user.getVorname(), user.getNachname());

			string password = "";
			do
			{
				Console.Write("Enter new password:");
				password = Console.ReadLine();

				if (isvalidpw(password) == true)
				{
					Console.Write("Enter password again:");
					string passwordretype = Console.ReadLine();

					if (password != passwordretype)
					{
						Console.WriteLine("Password not the same");
						return;
					}
				}

			} while (isvalidpw(password) == false);

			user.setPassword(password);
			FileControls.FindandReplace(Kernel.UserConfigFile, user.getUsername(), user);
			Console.WriteLine("password from {0}, {1} {2} successfully changed!", user.getUsername(), user.getVorname(), user.getNachname());
			Console.WriteLine("Changes will be applied after logout");
		}

		public void deleteUser(string[] payload)
		{
			User choosedUser = null;

			if (Kernel.CurrentUser.getPerm() != 1)
			{
				NoPermissions();
				return;
			}

			//EInbringen dass man den Username auch auf der Kommandozeile eingeben kann

			if(payload.Length <= 2)
			{
				do
				{
					Console.Clear();
					Console.WriteLine("Deleting users\n=================");

					foreach (User u in Kernel.allusers)
					{
						Console.WriteLine(u.getUsername() + "||" + u.getVorname() + "||" + u.getNachname() + "||" + u.getPassword() + "||" + u.getEmail() + "||" + u.getPerm());
					}

					Console.Write("Choose a username: ");
					string choosedusername = Console.ReadLine();

					foreach (User u in Kernel.allusers)
					{
						if (u.getUsername() == choosedusername)
						{
							choosedUser = u; break;
						}
					}

					if (choosedUser == null)
					{
						Console.Write("Choosed a user not exists! Try another one");
						Thread.Sleep(3000);
					}
				} while (choosedUser == null);

				Console.Clear();
				Console.WriteLine("Deleting user {0}, {1} {2}\n=================", choosedUser.getUsername(), choosedUser.getVorname(), choosedUser.getNachname());

				bool valid = false;
				do
				{
					valid = false;
					Console.Write("Soll der User wirklich gelöscht werden?(y/n) ");
					char confirm = Convert.ToChar(Console.ReadLine().Substring(0, 1));

					if (confirm == 'y' || confirm == 'Y')
					{
						Kernel.allusers.Remove(choosedUser);
						FileControls.updateConfig(Kernel.allusers);
						Console.WriteLine("user deleted!");
						if(Kernel.CurrentUser == choosedUser)
						{
							Kernel.CurrentUser = new User();
						}
						valid = true;
					}
					else if (confirm == 'n' || confirm == 'N')
					{
						Console.WriteLine("user not deleted!");
						valid = true;
					}
					else
					{
						Console.WriteLine("Invalid input");
						valid = false;
					}
				} while (valid != true);
			}

			return;
		}

		public void helpFileControls()
		{
			Console.WriteLine("general commands to manage the users:\n");
			Console.WriteLine("usage: user <options> - controls user actions");
			Console.WriteLine("\t<options> --add add a new user to the system (only as an administrator!)");
			Console.WriteLine("\t<options> --edit edit an existing user from the system (only as an administrator!)");
			Console.WriteLine("\t<options> --list list all existing users from the system from file \"0:\\configs\\users.txt\"");
			Console.WriteLine("\t<options> --delete delete an user from the system");
			Console.WriteLine("usage: chmod <username> <permission> - change the permission to <permission> from <username> (only as an administrator!)");
			Console.WriteLine("\t<permissions> 0 | 1 - 0 = normal user, 1 = administrator");
			Console.WriteLine("usage: passwd - change the password from current user");
			Console.WriteLine("usage: logout - logout the current user");
		}

		public static string ReadPassword()
		{
			// Ein StringBuilder wird verwendet, um das Passwort zeichenweise sicher zusammenzusetzen.
			StringBuilder passwordBuilder = new StringBuilder();
			ConsoleKeyInfo keyInfo;
			while (true)
			{
				// Liest eine Taste von der Konsole, ohne sie direkt anzuzeigen (true = keine Ausgabe).
				keyInfo = Console.ReadKey(true);
				// Prüft, ob die Enter-Taste gedrückt wurde (Signal für das Ende der Eingabe).
				if (keyInfo.Key == ConsoleKey.Enter)
				{
					Console.WriteLine(); // Springt nach Abschluss der Eingabe in eine neue Zeile.
					break; // Bricht die Schleife ab und beendet die Eingabe.
				}
				// Prüft, ob die Backspace-Taste gedrückt wurde (zum Löschen des letzten Zeichens).
				else if (keyInfo.Key == ConsoleKey.Backspace)
				{
					// Wenn der Passwort-String nicht leer ist:
					if (passwordBuilder.Length > 0)
					{
						// Entfernt das letzte Zeichen aus dem Passwort.
						passwordBuilder.Remove(passwordBuilder.Length - 1, 1);
						// Löscht das letzte Sternchen von der Konsole.
						// Zeile 1 bewegt den Cursor zurück, " " überschreibt das Zeichen,
						// und Zeile 3 bewegt den Cursor erneut zurück.
						// Alternativ statt Zeile 1 und 3 geht ebenfalls Console.Write("\b \b");
						// -> schreibt Sonderzeichen auf die Befehlszeile (nicht erwünscht)
						Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
						Console.Write(" ");
						Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
					}
				}
				else
				{
					// Fügt das gedrückte Zeichen dem Passwort hinzu.
					passwordBuilder.Append(keyInfo.KeyChar);
					// Gibt ein Sternchen (*) auf der Konsole aus, um die Eingabe zu verschleiern.
					Console.Write("*");
				}
			}
			// Gibt das vollständige Passwort als String zurück.
			return passwordBuilder.ToString();
		}
	}
}
