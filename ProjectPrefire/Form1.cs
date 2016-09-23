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

namespace ProjectPrefire
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
			
        Logger logger = new Logger();
		List<Game> games = new List<Game>();

        private void Form1_Load(object sender, EventArgs e)
        {
			//TODO CSV parsing code could potentially be cleaned up a little.
            string folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            string[] replays = Directory.GetFiles(folder, "*_meta.csv");
            foreach(string replay in replays)
            {
                string meta = "_meta.csv";
                string filename = Path.GetFileName(replay);
				filename = filename.Substring(0, filename.IndexOf(meta));
				string mapName = filename.Substring(filename.IndexOf("_")+1,filename.Length-filename.IndexOf("_")-1);
				var rows = new List<string[]>();
				string[] row;
				var parser = new CsvParser(File.OpenText(filename + "_pos.csv"));
				parser.Configuration.Delimiter = ";";
				parser.Read();
				while ((row = parser.Read()) != null)
				{
					rows.Add(row);
				}
				Game game = new Game (rows, MapFactory.Instance.GetMap(mapName));
				games.Add (game);
				//uhhh..?
				mapBox.Image = Image.FromFile(mapName + ".png");
                logger.writeLog("XD");
            }
        }

        private void mapBox_Click(object sender, EventArgs e)
        {
			new Analyzer(games.First());
            //analyzer.Replay();
        }

        private void log_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
