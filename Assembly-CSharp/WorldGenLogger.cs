using System;

// Token: 0x02000E03 RID: 3587
public static class WorldGenLogger
{
	// Token: 0x060071D2 RID: 29138 RVA: 0x002B1CBA File Offset: 0x002AFEBA
	public static void LogException(string message, string stack)
	{
		Debug.LogError(message + "\n" + stack);
	}
}
