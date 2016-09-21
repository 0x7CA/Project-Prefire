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

        private PosData loadDemo(string file, string args = "")
        {
            var rows = new List<string[]>();
            string[] row;

            var parser = new CsvParser(File.OpenText(file));
            parser.Configuration.Delimiter = ";";

            parser.Read();
            while ((row = parser.Read()) != null)
            {
                rows.Add(row);
            }
            

            PosData posData = new PosData(rows);
            return posData;
        }
        Logger logger = new Logger();
        PosData posData;
        //List<string[]> metaData;
        string map;
        private void Form1_Load(object sender, EventArgs e)
        {

            string folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            string[] replays = Directory.GetFiles(folder, "*_meta.csv");
            foreach(string replay in replays)
            {
                string meta = "_meta.csv";
                string filename = Path.GetFileName(replay);
                filename = filename.Substring(0, filename.IndexOf(meta));
                
                //metaData = loadDemo(replay);
                posData = loadDemo(filename + "_pos.csv");

                map = filename.Substring(filename.IndexOf("_")+1,filename.Length-filename.IndexOf("_")-1);

                mapBox.Image = Image.FromFile(map + ".png");

                logger.writeLog("XD");
            }


            //startWorker();


        }


        private void mapBox_Click(object sender, EventArgs e)
        {
            Analyzer analyzer = new Analyzer(posData, null, map);
            analyzer.Replay();

        }

        private void log_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /*private void startWorker()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += (sender, e) =>
            {
                // call the XYZ function
                e.Result = data.start();
            };
            worker.RunWorkerCompleted += (sender, e) =>
            {
                // use the result of the XYZ function:
                var result = e.Result;
                // Here you can safely manipulate the GUI controls
            };
            worker.RunWorkerAsync();
        }*/
    }
}
