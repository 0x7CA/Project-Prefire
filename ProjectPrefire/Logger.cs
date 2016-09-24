using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectPrefire
{
	public class Logger
	{
		public ListBox box;

		public static Logger instance;

		public static Logger Instance {
			get { 
				if (instance == null)
					instance = new Logger ();
				return instance;
			}
		}

		public void SetOutput (ListBox output)
		{
			box = output;	
		}

		private Logger ()
		{
		}



		public void WriteLog (string l)
		{
			box.Items.Add (l);
		}


	}
}
