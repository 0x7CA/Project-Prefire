using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using CsvHelper;
using System.Reflection;

namespace ProjectPrefire
{
	public partial class Form1 : Form
	{
		private List<Game> games = new List<Game> ();

		public Form1 ()
		{
			InitializeComponent ();
		}

		private void Form1_Load (object sender, EventArgs e)
		{
			//TODO CSV parsing code could potentially be cleaned up a little.
			Console.WriteLine("*********************************");
			Console.WriteLine("BEGIN CSV PARSING");
			Console.WriteLine("*********************************");
			Console.WriteLine("Getting current directory..");
			string folder = Path.GetDirectoryName (Assembly.GetEntryAssembly().Location);
			Console.WriteLine("Directory: " + folder);
			Console.WriteLine("Searching for files with the _meta.csv suffix..");
			string[] replays = Directory.GetFiles (folder, "*_meta.csv");

			if(replays.Length == 0)
			{
				Console.WriteLine("Could not find any replays... terminating");
				System.Environment.Exit(1);
			}


			Console.WriteLine("Detetected Replay meta files:");


			foreach(string r in replays){
				Console.WriteLine("\t" + r);
			}

			foreach (string replay in replays) {
				string meta = "_meta.csv";
				string filename = Path.GetFileName (replay);
				filename = filename.Substring (0, filename.IndexOf (meta));
				string mapName = filename.Substring (filename.IndexOf ("_") + 1, filename.Length - filename.IndexOf ("_") - 1);
				var rows = new List<string[]> ();
				string[] row;
				var parser = new CsvParser (File.OpenText (filename + "_pos.csv"));
				parser.Configuration.Delimiter = ";";
				parser.Read ();
				while ((row = parser.Read ()) != null) {
					rows.Add (row);
				}
				Console.WriteLine ("Creating game object for: " + filename + "..");
				Game game = new Game (rows, MapFactory.Instance.GetMap (mapName));
				games.Add (game);
				//uhhh..?
				mapBox.Image = Image.FromFile (mapName + ".png");

			}
			Console.WriteLine("*********************************");
			Console.WriteLine("END CSV PARSING");
			Console.WriteLine("*********************************");
		}

		private void mapBox_Click (object sender, EventArgs e)
		{
			Analyzer a = new Analyzer (games.First ());
			a.Filter ();
		}

		private void log_SelectedIndexChanged (object sender, EventArgs e)
		{

		}
	}
}
