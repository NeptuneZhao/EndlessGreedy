using System;
using UnityEngine;

// Token: 0x02000534 RID: 1332
[AddComponentMenu("KMonoBehaviour/scripts/CameraFollowHelper")]
public class CameraFollowHelper : KMonoBehaviour
{
	// Token: 0x06001E5D RID: 7773 RVA: 0x000A943F File Offset: 0x000A763F
	private void LateUpdate()
	{
		if (CameraController.Instance != null)
		{
			CameraController.Instance.UpdateFollowTarget();
		}
	}
}
