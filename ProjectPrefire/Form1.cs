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
		private List<Match> matches = new List<Match> ();
		private Logger logger = Logger.Instance;

		public Form1 ()
		{
			InitializeComponent ();
			logger.SetOutput (log);
		}

		private void Form1_Load (object sender, EventArgs e)
		{

			//TODO CSV parsing code could potentially be cleaned up a little.
			logger.WriteLog ("*****************");
			logger.WriteLog ("BEGIN CSV PARSING");
			logger.WriteLog ("*****************");
			logger.WriteLog ("Getting current directory..");
			string folder = Path.GetDirectoryName (Assembly.GetEntryAssembly ().Location);
			logger.WriteLog ("Directory: " + folder);
			logger.WriteLog ("Searching for files with the _meta.csv suffix..");
			string[] replays = Directory.GetFiles (folder, "*_meta.csv");

			if (replays.Length == 0) {
				logger.WriteLog ("Could not find any replays... terminating");
				System.Environment.Exit (1);
			}


			logger.WriteLog ("Detetected Replay meta files:");


			foreach (string r in replays) {
				logger.WriteLog ("\t" + r);
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
				logger.WriteLog ("Creating game object for: " + filename + "..");
				Match match = new Match (rows, MapFactory.Instance.GetMap (mapName));
				matches.Add (match);
				//uhhh..?
				mapBox.Image = Image.FromFile (mapName + ".png");

			}
			logger.WriteLog ("*****************");
			logger.WriteLog ("END CSV PARSING");
			logger.WriteLog ("*****************");
		}

		private void mapBox_Click (object sender, EventArgs e)
		{
			logger.WriteLog ("Starting analysis..");
			Analyzer a = new Analyzer (matches.First ());
			a.Filter ();
		}

		private void log_SelectedIndexChanged (object sender, EventArgs e)
		{

		}
	}
}
