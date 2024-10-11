using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosProject.UserManagement
{
	public class User
	{
		private string vorname;
		private string nachname;
		private string username;
		private string password;
		private string email;
		private bool isAdmin;

        //admin = 0 - kein Administrator
        //admin = 1 - Administrator

        public User(string username, string vorname, string nachname, string password, string email, bool admin)
        {
            this.username = username;
			this.vorname = vorname;
			this.nachname = nachname;
			this.password = password;
			this.email = email;
			this.isAdmin = admin;
        }
		public void setPassword(string newPassword)
		{
			this.password = newPassword;
		}
		public void setEmail(string newEmail)
		{
			this.email = newEmail;
		}

		public string getUsername()
		{
			return this.username;
		}
		public string getVorname()
		{
			return this.vorname;
		}
		public string getNachname()
		{
			return this.nachname;
		}
		public string getPassword()
		{
			return this.password;
		}
		public string getEmail()
		{
			return this.email;
		}
		public bool getStatus()
		{
			return isAdmin;
		}

	}
}
