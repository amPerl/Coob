namespace Coob
{
	public class CoobOptions
	{
		public int Port;
		public uint MaxClients;
		public int WorldSeed;

		public CoobOptions()
		{
			Port = Globals.DefaultPort;
			MaxClients = Globals.DefaultMaxClients;
		}
	}
}
