using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPrefire
{
	public abstract class Map
	{
		public string name { get; set; }

		public float sizeX { get; set; }

		public float sizeY { get; set; }

		public float startX { get; set; }

		public float startY { get; set; }

		public float ConvertX (float x)
		{
			x += (startX < 0) ? startX * -1 : startX;
			x = (float)Math.Floor ((x / sizeX) * 860);
			return x;
		}

		public float ConvertY (float y)
		{
			y += (startY < 0) ? startY * -1 : startY;
			y = (float)Math.Floor ((y / sizeY) * 860);
			y = (y - 860) * -1;
			return y;
		}

	}
		
}
