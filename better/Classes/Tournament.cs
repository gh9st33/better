using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace better.Classes
{
    public class Tournament
    {
        public int? id;
        public Casino? Casino;
        public string? Name;
        public string? Game;
        public float PrizePool;
        public float BuyIn;
        public int? Players;
        public int? MaxPlayers;
        public DateTime StartDate;
        public DateTime? EndDate;
        public string? Status;
        public string? Mode;
        public bool IsLocked = false;
        public List<PlayerAccount> MyPlayers = new List<PlayerAccount>();
        public Tournament(Casino casino, string name, string game, float prizePool, float buyIn, int players, int maxPlayers, DateTime startDate, DateTime endDate, string status, string mode)
        {
            Casino = casino;
            Name = name;
            Game = game;
            PrizePool = prizePool;
            BuyIn = buyIn;
            Players = players;
            MaxPlayers = maxPlayers;
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
            Mode = mode;

        }

        public void AddPlayer(PlayerAccount playerAccount)
        {
            MyPlayers.Add(playerAccount);
        }

        public void RemovePlayer(PlayerAccount playerAccount)
        {
            MyPlayers.Remove(playerAccount);
        }

    }
}
