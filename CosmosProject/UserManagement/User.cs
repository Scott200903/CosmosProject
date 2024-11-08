using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosProject.UserManagement
{
	public class User
	{
		private string vorname { get; set; }
		private string nachname { get; set; }
		private string username { get; set; }
		private string password { get; set; }
		private string email { get; set; }

		private int perm {  get; set; }

        //admin = 0 - kein Administrator
        //admin = 1 - Administrator

        public User()
        {
			this.vorname = "";
			this.nachname = "";
			this.username = "";
			this.password = "";
			this.email = "";
			this.perm = 0;
		}
        public User(string username, string vorname, string nachname, string password, string email, int perm)
        {
            this.username = username;
			this.vorname = vorname;
			this.nachname = nachname;
			this.password = password;
			this.email = email;
			this.perm = perm;
        }
		public void setPassword(string newPassword)
		{
			this.password = newPassword;
		}
		public void setEmail(string newEmail)
		{
			this.email = newEmail;
		}
		public void setPerm(int permlevel)
		{
			this.perm = permlevel;
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
		public int getPerm()
		{
			return this.perm;
		}

	}
}
