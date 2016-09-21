using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DemoInfo;
using System.Windows.Forms;
using System.Drawing;
namespace ProjectPrefire
{
    class Analyzer
    {
        Logger log = new Logger();
        Drawer drawer { get; set; }

        public DemoInfo.Vector pos { get; set; }
        public string map { get; set; }

        PosData posData { get; set; }
        PosData metaData { get; set; }

        static float sizeX { get; set; }
        static float sizeY { get; set; }
        static float startX { get; set; }
        static float startY { get; set; }
        static float ratiox { get; set; }
        static float ratioy { get; set; }
        static float canvasW { get; set; }
        static float canvasH { get; set; }


        public Analyzer(PosData pos, PosData meta, string map)
        {

            posData = pos;
            //metaData = meta.IndexRows<int>("PLAYERID").SortRowsByKey();
            Control canvas = Application.OpenForms[0].Controls.Find("mapBox", true).FirstOrDefault();
            drawer = new Drawer(canvas, map);


            var maptype = Type.GetType("ProjectPrefire.Maps." + map);
            var Map = (Map)Activator.CreateInstance(maptype);

            sizeX = Map.sizeX;
            sizeY = Map.sizeY;
            startX = Map.startX;
            startY = Map.startY;

            canvasW = canvas.Width;
            canvasH = canvas.Height;

            ratiox = (sizeX / canvas.Width);
            ratioy = (sizeY / canvas.Height);


            int i = 0;
            /*foreach(var round in posData.round)
            {

                drawer.drawCircle((float)posData.X[i], (float)posData.Y[i]);
                i++;
            }*/

            Filter();
            /*foreach(int round in posData.round.Distinct())
            {
                int i = 0;
                var posDatafiltered = Array.FindIndex(posData, row => row.Round == round);

                while ((i = posData.round.IndexOf(round, i)) != -1)
                {
                    // Print out the substring.
                    Console.WriteLine(s.Substring(i));

                    // Increment the index.
                    i++;
                }
                log.writeLog(posData.X.ToString() + pos.Y[0]);
                drawer.drawCircle(pos.X[0], pos.Y[0]);
            }*/

        }

        public void Replay()
        {

        }

        public void Filter(int delay = 3)
        {
            foreach (int id in posData.playerID.Distinct())
            {
                float dPosX = 0;
                float dPosY = 0;
                for (int i = 0; i < posData.X.Length/10; i++)
                {
                    if(dPosX == posData.X[id + i * 10])
                    {
                        Vector2 coords = ConvertCoords(posData.X[id + i * 10], posData.Y[id + i * 10]);

                        int team = posData.team[id + i * 10];
                        Color color = Color.Blue;
                        if (team == 1)
                        {
                            color = Color.Red;
                        }
                        
                        
                        //viewangle endpoint calc
                        float x2 = coords.X + 25 * (float)Math.Cos((float)(Math.PI / 180) * posData.viewX[id + i * 10] *-1);
                        float y2 = coords.Y + 25 * (float)Math.Sin((float)(Math.PI / 180) * posData.viewX[id + i * 10]*-1);
                        drawer.drawCircle(coords.X, coords.Y, color);
                        drawer.drawLine(coords.X,coords.Y,x2,y2, color);

                    }
                    dPosX = posData.X[id + i * 10];
                    dPosY = posData.Y[id + i * 10];
                }
                drawer.drawString("HIE asdasd1 123 AD dd", 10, 10 * id, Color.Red);
                drawer.drawRectangle(10, 10, 100, 100, Color.Red);
            }
        }

        Vector2 ConvertCoords(float x, float y)
        {
            x += (startX < 0) ? startX * -1 : startX;
            y += (startY < 0) ? startY * -1 : startY;
            x = (float)Math.Floor((x / sizeX) * 860);
            y = (float)Math.Floor((y / sizeY) * 860);
            y = (y - 860) * -1;
            return new Vector2(x,y);
        }
    }
}
