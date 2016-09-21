using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProjectPrefire
{
    class PosData : DataFrame
    {
        public float[] X { get; set; }
        public float[] Y { get; set; }
        public float[] viewX { get; set; }
        public float[] viewY { get; set; }
        public int[] playerID { get; set; }
        public int[] team { get; set; }

        public PosData(List<string[]> csv)
        {
            List<int> _round = new List<int>();
            List<float> _X = new List<float>();
            List<float> _Y = new List<float>();
            List<float> _viewX = new List<float>();
            List<float> _viewY = new List<float>();
            List<int> _playerID = new List<int>();
            List<int> _team = new List<int>();

            foreach (var row in csv)
            {

                _round.Add(int.Parse(row[0]));
                _X.Add(float.Parse(row[1]));
                _Y.Add(float.Parse(row[2]));
                _viewX.Add(float.Parse(row[3]));
                _viewY.Add(float.Parse(row[4]));
                _playerID.Add(int.Parse(row[5]));
                _team.Add(int.Parse(row[6]));

            }

            round = _round.ToArray();
            X = _X.ToArray();
            Y = _Y.ToArray();
            viewX = _viewX.ToArray();
            viewY = _viewY.ToArray();
            playerID = _playerID.ToArray();
            team = _team.ToArray();
        }
    }
}
