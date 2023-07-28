using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace better.Classes
{
    public class Casino
    {

        public string Name;
        public string URL;
        public string? Software;
        public string? Notes;
        public bool InclaveSSO = false;
        public List<Tournament> Tournaments = new List<Tournament>();
        public List<PlayerAccount> PlayerAccounts = new List<PlayerAccount>();
        public string DataFile = "casino.json";
        public Casino(string name, string url, string? software = null, string? Notes = null)
        {
            Name = name;
            URL = url;
            Software = software;
            Notes = Notes;

        }

        public void AddTournament(Tournament tournament)
        {
            Tournaments.Add(tournament);
        }

        public void AddPlayerAccount(PlayerAccount playerAccount)
        {
            PlayerAccounts.Add(playerAccount);
        }

        public void RemoveTournament(Tournament tournament)
        {
            Tournaments.Remove(tournament);
        }

        public void RemovePlayerAccount(PlayerAccount playerAccount)
        {
            PlayerAccounts.Remove(playerAccount);
        }

        public void SetInclaveSSO(bool value)
        {
            InclaveSSO = value;
        }

        public void SetSoftware(string software)
        {
            Software = software;
        }

        public void SetNotes(string notes)
        {
            Notes = notes;
        }

        public void SetURL(string url)
        {
            URL = url;
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public void SetTournaments(List<Tournament> tournaments)
        {
            Tournaments = tournaments;
        }

        public DateTime GetNextTournamentStart()
        {
            var nextTournament = Tournaments.Where(t => t.StartDate > DateTime.Now).OrderBy(t => t.StartDate).FirstOrDefault();
            return nextTournament.StartDate;
        }

        private void SaveCasinoToJson(string? path)
        {
            if (path == null)
            {
                path = DataFile;
            }

            var json = System.Text.Json.JsonSerializer.Serialize(this);
            try
            {
                System.IO.File.WriteAllText(path, json);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }

        private void LoadCasinoFromJson(string? path)
        {
            if (path == null)
            {
                path = DataFile;
            }

            var json = System.IO.File.ReadAllText(path);
            try
            {
                var casino = System.Text.Json.JsonSerializer.Deserialize<Casino>(json);
                Name = casino.Name;
                URL = casino.URL;
                Software = casino.Software;
                Notes = casino.Notes;
                InclaveSSO = casino.InclaveSSO;
                Tournaments = casino.Tournaments;
                PlayerAccounts = casino.PlayerAccounts;
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e + " We Either Cannot Access or File Doesn't Exist.");
            }
        }
    }
}
