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

		//returnen hier? hoe zit dat met structs..
		public void convertPlayerState(PlayerState state)
		{
			state.posX += (startX < 0) ? startX * -1 : startX;
			state.posY += (startY < 0) ? startY * -1 : startY;
			state.posX = (float)Math.Floor((state.posX / sizeX) * 860);
			state.posY = (float)Math.Floor((state.posY / sizeY) * 860);
			state.posY = (state.posY - 860) * -1;
		}
    }
		
}
