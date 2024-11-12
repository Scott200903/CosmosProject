using CosmosProject.fsystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosProject
{
	public class FileControls
	{
		private string currentVersion = "Version 0.3";
		private Dictionary<string, Action<string[]>> commandMap;

		public FileControls()
		{
			InitializeFileControls();
		}

		private void InitializeFileControls()
		{
			commandMap = new Dictionary<string, Action<string[]>>
			{
				{ "file", args => helpFileControls(args) },
				{ "mkdir", args => makedir(args) },
				{ "ls", args => {
					if(args.Length == 1)
					{
						listdir(@"0:\");
					}
					else
					{
						listdir(@"0:\" + args[1]);
					}
				} },
				{ "dir", args => {
					if(args.Length == 1)
					{
						listdir(@"0:\");
					}
					else
					{
						listdir(@"0:\" + args[1]);
					}
				} },
				{ "touch", args => createfile(args) },
				{ "rm", args => deletefile(args) },
				{ "editfile", args => editfile(args) }
				//	{ "file", args => InitializeFilesystem(args) }
				//};
			};
		}

		public void commands(string[] args)
		{
			if (args.Length == 0)
			{
				Console.WriteLine("Try \"file help\" for a quick view of all commands!");
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
				Console.WriteLine("Unknown command, try \"file help\" for a quick view of all commands!");
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
						File.WriteAllText(@"0:\" + payload[1], contents + text);
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
				Console.WriteLine("Invalid Usage or argument is not a file! Enter \"file help\" for more informations");
				return 1;
			}
		}
		private void listdir(string payload)
		{
			if (!Directory.Exists(payload))
			{
				Console.WriteLine($"{payload} is not  directory");
				return;
			}

			var directory_list = Kernel.fs.GetDirectoryListing(payload);

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
		private void makedir(string[] payload)
		{
			if (payload.Length == 2)
			{
				try
				{
					Kernel.fs.CreateDirectory(@"0:\" + payload[1]);
					return;
				}
				catch (Exception e)
				{
					Console.WriteLine(e.ToString());
					return;
				}
			}
			else
			{
				Console.WriteLine("Invalid Usage! Enter \"file help\" for more informations");
				return;
			}
		}
		public void helpFileControls(string[] payload)
		{
			if (payload.Length < 2)
			{
				Console.WriteLine("Try \"file help\" for a quick view of all commands!");
				return;
			}
			else
			{
				Console.WriteLine("Allgemeine Commands zur File-Verwaltung:\n");
				Console.WriteLine("ls or dir - Listet Verzeichnisstruktur auf ");
				Console.WriteLine("mkdir <directoryname> - Erstellt ein Ordner mit angegebenen Namen");
				Console.WriteLine("touch <filename> - Erstellt eine Datei mit angegebenen Namen");
			}
		}
		private int createfile(string[] payload)
		{
			if (payload.Length >= 2)
			{
				foreach (char c in Kernel.forbiddenchars)
				{
					if (payload[1].Contains(c))
					{
						Console.WriteLine("Invalid char " + c + " in filename");
						return -1;
					}
				}
				try
				{
					if (File.Exists(Kernel.CurrentPath + payload[1]))
					{

						//Aufruf mit -y Argument dann wird die Datei direkt überschrieben
						if (payload.Length == 3)
						{
							if (payload[2] == "-y")
							{
								Kernel.fs.CreateFile(Kernel.CurrentPath + payload[1]);
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
							Kernel.fs.CreateFile(Kernel.CurrentPath + payload[1]);

							//File.Create(@"0:\" + payload[1]);
							Console.WriteLine("Datei wurde ueberschrieben!");
							return 0;
						}
						else if (input == "n")
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
						Kernel.fs.CreateFile(Kernel.CurrentPath + payload[1]);
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
					var directory_list = Kernel.fs.GetDirectoryListing(@"0:\");

					if (payload[1] == "--file" || payload[1] == "-f")
					{
						if (payload[2] == "configs\\users.txt")
						{
							if (Kernel.CurrentUser.getPerm() == 1)
							{
								bool valid = false;
								do
								{
									valid = false;
									Console.Write("Möchtest du {0} wirklich löschen?(y/n) ", @"0:\" + payload[2]);
									char confirm = Convert.ToChar(Console.ReadLine().Substring(0, 1));

									if (confirm == 'y' || confirm == 'Y')
									{
										File.Delete(@"0:\" + payload[2]);
										Console.WriteLine("file deleted!");
										valid = true;
									}
									else if (confirm == 'n' || confirm == 'N')
									{
										Console.WriteLine("file not deleted!");
										valid = true;
									}
									else
									{
										Console.WriteLine("Invalid input");
										valid = false;
									}
								} while (valid != true);
							}
							return 0;
						}
						if (File.Exists(@"0:\" + payload[2]))
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

		public static void FindandReplace(string filename, string searchitem, string replaceitem, int index)
		{
			List<string> alllinesinfile = new List<string>();
			string[] linesplit = new string[15];

			if (File.Exists(filename))
			{
				foreach(string line in File.ReadLines(filename))
				{
					alllinesinfile.Add(line);
				}
				File.WriteAllText(filename, string.Empty);

				foreach(string line in alllinesinfile)
				{
					if (line.Contains(searchitem + ":"))
					{
						linesplit = line.Split(':');
						string replacedline = "";

						for (int i = 0; i < linesplit.Length; i++)
						{
							if (index == 5)
							{
								replacedline = replacedline + replaceitem;
								//Console.WriteLine(replacedline);
							}
							else if (i == index)
							{
								replacedline = replacedline + replaceitem + ":";
								//Console.WriteLine(replacedline);
							}
							else
							{
								replacedline = replacedline + linesplit[i] + ":";
								//Console.WriteLine(replacedline);
							}
						}
						Console.WriteLine("replacedLine = {0}", replacedline);
						UserControls.WriteLN(Kernel.UserConfigFile, replacedline);
					}
					else
					{
						UserControls.WriteLN(Kernel.UserConfigFile, line);
					}
				}
			}
			else
			{
				Console.WriteLine("Filename {0} not exists!", filename); return;
			}

			//if (File.Exists(filename))
			//{
			//	foreach(string line in File.ReadLines(filename))
			//	{
			//		if (line.Contains(searchitem + ":"))
			//		{
			//			Console.WriteLine("line.Contains {0}", searchitem + ":");

				//			linesplit = line.Split(":");

				//			if (index < linesplit.Length)
				//			{
				//				File
				//				Console.WriteLine("linesplit at Index {0} = {1}", index, linesplit[index]);
				//				Console.WriteLine("replaceitem = {0}", replaceitem);
				//				linesplit[(int)index] = replaceitem;
				//				line.re
				//				string replacedline = line.Replace(linesplit[index], replaceitem);

				//				Console.WriteLine("replacedline = {0}", replacedline);
				//			}
				//			else
				//			{
				//				Console.WriteLine("Index {0} is out of Range of Array-Length {1}", index, linesplit.Length);
				//				return;
				//			}
				//		}
				//		else { }
				//	}
				//}
				//else
				//{
				//	Console.WriteLine("Filename {0} not exists!", filename); return;
				//}
		}
	}
}
