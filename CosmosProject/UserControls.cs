using Cosmos.System.Network.IPv4.TCP;
using CosmosProject.UserManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

							User usr = new User(username, vorname, nachname, password, email, 0);
							string userstring = usr.getUsername() + ":" + usr.getVorname() + ":" + usr.getNachname() + ":" + User.GenerateHash(usr.getPassword()) + ":" + usr.getEmail() + ":" + usr.getPerm().ToString();

							Console.WriteLine(userstring);

							string usercnfFile = @"0:\configs\users.txt";

							WriteLN(usercnfFile, userstring);

							break;
						}
					case "--list":
						{
							Kernel.allusers = getallUsers();

							foreach (User u in Kernel.allusers)
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
	}
}
