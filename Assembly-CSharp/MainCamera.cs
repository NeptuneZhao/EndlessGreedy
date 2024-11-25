using System;
using UnityEngine;

// Token: 0x02000584 RID: 1412
public class MainCamera : MonoBehaviour
{
	// Token: 0x060020EB RID: 8427 RVA: 0x000B86D7 File Offset: 0x000B68D7
	private void Awake()
	{
		if (Camera.main != null)
		{
			UnityEngine.Object.Destroy(Camera.main.gameObject);
		}
		base.gameObject.tag = "MainCamera";
	}
}
