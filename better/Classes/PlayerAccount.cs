using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace better.Classes
{
    public class PlayerAccount
    {
        public string Email;
        public string Password;
        public float Balance;
        public String Casino;
        public List<Tournament> Tournaments = new List<Tournament>();
        public bool isLoggedIn = false;
        public bool isDepositor = false;
        public string DataFile { get; set; }
        public PlayerAccount(string email, string password, Casino casino)
        {
            Email = email;
            Password = password;
            Casino = casino.Name;
            DataFile = Casino + '_' + "playerAccount.json";
        }

        public void AddTournament(Tournament tournament)
        {
            Tournaments.Add(tournament);
        }

        public void AddBalance(float amount)
        {
            Balance += amount;
        }

        public void RemoveBalance(float amount)
        {
            Balance -= amount;
        }

        public void SetCasino(string casino)
        {
            Casino = casino;
        }

        public void SetBalance(float balance)
        {
            Balance = balance;
        }
        public void SetIsLoggedIn(bool value)
        {
            isLoggedIn = value;
        }

        public void SavePlayerAccountToJson(string? path)
        {
            if (path == null)
            {
                path = DataFile;
            }
            try
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(this);
                System.IO.File.WriteAllText(path, json);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public void LoadPlayerAccountFromJson(string? path)
        {             
            if (path == null)
            {
                path = DataFile;
            }
            try
            {
                string json = System.IO.File.ReadAllText(path);
                PlayerAccount playerAccount = Newtonsoft.Json.JsonConvert.DeserializeObject<PlayerAccount>(json);
                Email = playerAccount.Email;
                Password = playerAccount.Password;
                Balance = playerAccount.Balance;
                Casino = playerAccount.Casino;
                Tournaments = playerAccount.Tournaments;
                isLoggedIn = playerAccount.isLoggedIn;
                isDepositor = playerAccount.isDepositor;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
