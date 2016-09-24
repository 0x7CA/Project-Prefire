using System;
using System.Collections.Generic;

namespace ProjectPrefire
{
	public class Player
	{
		public int playerId { get; }

		public int team { get; }

		public List<PlayerState> playerStates { get; }

		public Player (int id, int team)
		{
			this.playerId = id;
			this.team = team;
			this.playerStates = new List<PlayerState> ();
		}
	}
}

