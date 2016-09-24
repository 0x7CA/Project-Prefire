using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using System.Linq;

namespace ProjectPrefire
{
	public class Match
	{
		public List<Player> players { get; set; }

		public Map map { get; set; }

		public Match (List<string[]> csvRows, Map map)
		{
			this.map = map;
			//TODO This might not be the most suitable place.
			#region csv parsing
			//TODO Instead of using the dictionary class to sort rows, Distinct() may offer a better solution.
			Dictionary<string, Player> playerDictionary = new Dictionary<string, Player> ();
			foreach (var row in csvRows) {

				/*
				 *  row[0] = int round
				 *  row[1] = float x position
				 *  row[2] = float y position
				 *  row[3] = float x viewangle
				 *  row[4] = float y viewangle
				 *  row[5] = int playerid
				 */

				if (playerDictionary.ContainsKey (row [5])) {
					Player p = playerDictionary [row [5]];
					p.playerStates.Add (new PlayerState (
						map.ConvertX (float.Parse (row [1])), 
						map.ConvertY (float.Parse (row [2])),
						float.Parse (row [3]),
						float.Parse (row [4]),
						int.Parse (row [0])));
				} else {
					Player p = new Player (int.Parse (row [5]), int.Parse (row [6]));
					Logger.Instance.WriteLog ("Adding information for player with playerid: " + p.playerId);
					p.playerStates.Add (new PlayerState (
						map.ConvertX (float.Parse (row [1])), 
						map.ConvertY (float.Parse (row [2])),
						float.Parse (row [3]),
						float.Parse (row [4]),
						int.Parse (row [0])));
					playerDictionary.Add (row [5], p);
				}
			}
			players = playerDictionary.Values.ToList ();
			#endregion
		}
	}
}

