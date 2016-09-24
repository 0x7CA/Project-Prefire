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
		//Logger log = new Logger();
		Drawer drawer { get; set; }

		private Match match;

		public Analyzer (Match match)
		{
			this.match = match;
			Control canvas = Application.OpenForms [0].Controls.Find ("mapBox", true).FirstOrDefault ();
			drawer = new Drawer (canvas);
		}

		public void Filter (int delay = 3)
		{
			Logger.Instance.WriteLog ("Applying filters..");
			Logger.Instance.WriteLog ("Drawing objects..");
			foreach (Player player in match.players) {
				foreach (PlayerState state in player.playerStates) {
					Color color = Color.Blue;
					if (player.team == 1) {
						color = Color.Red;
					}

					float x2 = state.posX + 25 * (float)Math.Cos ((float)(Math.PI / 180) * state.angX * -1);
					float y2 = state.posY + 25 * (float)Math.Sin ((float)(Math.PI / 180) * state.angY * -1);

					drawer.drawCircle (state.posX, state.posY, color);
					drawer.drawLine (state.posX, state.posY, x2, y2, color);
				}
			}
		}
	}
}
