using System;

namespace ProjectPrefire
{
	public struct PlayerState
    {
        public float posX { get; set; }
        public float posY { get; set; }
		public float angX { get; set; }
		public float angY { get; set; }
		public int round { get; set;}
		public PlayerState(float posX, float posY, float angX, float angY, int round)
        {
			this.posX = posX;
			this.posY = posY;
			this.angX = angX;
			this.angY = angY;
			this.round = round;
        }

    }
}
