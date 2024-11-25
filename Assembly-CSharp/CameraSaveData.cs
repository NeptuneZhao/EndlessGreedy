using System;
using UnityEngine;

// Token: 0x020007A4 RID: 1956
public static class CameraSaveData
{
	// Token: 0x0600358B RID: 13707 RVA: 0x00123420 File Offset: 0x00121620
	public static void Load(FastReader reader)
	{
		CameraSaveData.position = reader.ReadVector3();
		CameraSaveData.localScale = reader.ReadVector3();
		CameraSaveData.rotation = reader.ReadQuaternion();
		CameraSaveData.orthographicsSize = reader.ReadSingle();
		CameraSaveData.valid = true;
	}

	// Token: 0x04001FCE RID: 8142
	public static bool valid;

	// Token: 0x04001FCF RID: 8143
	public static Vector3 position;

	// Token: 0x04001FD0 RID: 8144
	public static Vector3 localScale;

	// Token: 0x04001FD1 RID: 8145
	public static Quaternion rotation;

	// Token: 0x04001FD2 RID: 8146
	public static float orthographicsSize;
}
