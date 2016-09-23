using System;


namespace ProjectPrefire
{
	//TODO Not thread safe
    public class MapFactory
    {
		public static MapFactory instance; 

		private MapFactory()
		{
		}
			
		public static MapFactory Instance
		{
			get
			{ 
				if(instance == null)
					instance = new MapFactory();
				return instance;
			}
		}

		public Map GetMap(string mapName)
		{
			switch(mapName)
			{
			case "de_dust2":
				return new de_dust2 ();
			case "de_cache":
				return new de_cache ();

			default:
				return null;
			}
		}
    }
}
