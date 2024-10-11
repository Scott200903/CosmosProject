using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Sys = Cosmos.System;
using CosmosProject.UserManagement;
using System.Reflection.Metadata;
using System.Diagnostics;

namespace CosmosProject
{
	public class Kernel : Sys.Kernel
	{
		List<User> tmp;
		protected override void BeforeRun()
		{
			Console.WriteLine("Cosmos booted successfully.");

			tmp = new List<User>();
		}
		protected override void Run()
		{
			Console.Write("Enter Command: ");
			string input = Console.ReadLine();

			//Shutdown 
			if(input == "poweroff" || input == "shutdown") Environment.Exit(0);

			if(input == "adduser")
			{
				Console.Write("Enter Username:");
				string username = Console.ReadLine();

				Console.Write("Enter Vorname:");
				string vorname = Console.ReadLine();

				Console.Write("Enter Nachname:");
				string nachname = Console.ReadLine();

				Console.Write("Enter Password:");
				string password = Console.ReadLine();

				Console.Write("Enter Email:");
				string email = Console.ReadLine();

				Console.Write("Enter isAdmin:");
				string isAdmin = Console.ReadLine();

				try
				{
					bool tmpadmin = true;
					User usr = new User(username, vorname, nachname, password, email, tmpadmin);
					tmp.Add(usr);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
					Thread.Sleep(5000);
				}s
			}

			if (input == "listuser")
			{
				Console.WriteLine("Username:Vorname:Nachname:Password:Email:tisAdmin");
				foreach (User usr in tmp)
				{
					Console.WriteLine(usr.getUsername() + ":" + usr.getVorname() + ":" + usr.getNachname() + ":" + usr.getPassword() + ":" + usr.getEmail() + ":" + usr.getStatus());
				}

				Console.WriteLine("Anzahl an Benutzern: " + tmp.Count);
			}
			//if (input == "exit")
			//{
			//	Console.Write("Do you want to exit the OS?\nType y for yes and n for no!\n");

			//	string exit = Console.ReadLine();

			//	if(exit == "y" || exit == "Y")
			//	{
			//		Console.WriteLine("Application stopped!");
			//		Thread.Sleep(2000);

			//		Environment.Exit(1);
			//	}
			//	else if(exit == "n" || exit == "N")
			//	{
			//		Console.WriteLine("Application not stopped!");
			//	}
			//}
			//else
			//{
			//	Console.WriteLine(input);
			//}
		}
	}
}
