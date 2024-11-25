using System;

namespace ProcGenGame
{
	// Token: 0x02000E0F RID: 3599
	public class WorldgenException : Exception
	{
		// Token: 0x0600725F RID: 29279 RVA: 0x002BA041 File Offset: 0x002B8241
		public WorldgenException(string message, string userMessage) : base(message)
		{
			this.userMessage = userMessage;
		}

		// Token: 0x04004ED3 RID: 20179
		public readonly string userMessage;
	}
}
