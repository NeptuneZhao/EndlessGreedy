using System;
using UnityEngine;

// Token: 0x02000D13 RID: 3347
public class PopIn : MonoBehaviour
{
	// Token: 0x06006872 RID: 26738 RVA: 0x00271C34 File Offset: 0x0026FE34
	private void OnEnable()
	{
		this.StartPopIn(true);
	}

	// Token: 0x06006873 RID: 26739 RVA: 0x00271C40 File Offset: 0x0026FE40
	private void Update()
	{
		float num = Mathf.Lerp(base.transform.localScale.x, this.targetScale, Time.unscaledDeltaTime * this.speed);
		base.transform.localScale = new Vector3(num, num, 1f);
	}

	// Token: 0x06006874 RID: 26740 RVA: 0x00271C8C File Offset: 0x0026FE8C
	public void StartPopIn(bool force_reset = false)
	{
		if (force_reset)
		{
			base.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
		}
		this.targetScale = 1f;
	}

	// Token: 0x06006875 RID: 26741 RVA: 0x00271CBB File Offset: 0x0026FEBB
	public void StartPopOut()
	{
		this.targetScale = 0f;
	}

	// Token: 0x040046A2 RID: 18082
	private float targetScale;

	// Token: 0x040046A3 RID: 18083
	public float speed;
}
