using System;
using UnityEngine;

// Token: 0x02000939 RID: 2361
[AddComponentMenu("KMonoBehaviour/scripts/KTime")]
public class KTime : KMonoBehaviour
{
	// Token: 0x170004ED RID: 1261
	// (get) Token: 0x060044A4 RID: 17572 RVA: 0x00186B8B File Offset: 0x00184D8B
	// (set) Token: 0x060044A5 RID: 17573 RVA: 0x00186B93 File Offset: 0x00184D93
	public float UnscaledGameTime { get; set; }

	// Token: 0x170004EE RID: 1262
	// (get) Token: 0x060044A6 RID: 17574 RVA: 0x00186B9C File Offset: 0x00184D9C
	// (set) Token: 0x060044A7 RID: 17575 RVA: 0x00186BA3 File Offset: 0x00184DA3
	public static KTime Instance { get; private set; }

	// Token: 0x060044A8 RID: 17576 RVA: 0x00186BAB File Offset: 0x00184DAB
	public static void DestroyInstance()
	{
		KTime.Instance = null;
	}

	// Token: 0x060044A9 RID: 17577 RVA: 0x00186BB3 File Offset: 0x00184DB3
	protected override void OnPrefabInit()
	{
		KTime.Instance = this;
		this.UnscaledGameTime = Time.unscaledTime;
	}

	// Token: 0x060044AA RID: 17578 RVA: 0x00186BC6 File Offset: 0x00184DC6
	protected override void OnCleanUp()
	{
		KTime.Instance = null;
	}

	// Token: 0x060044AB RID: 17579 RVA: 0x00186BCE File Offset: 0x00184DCE
	public void Update()
	{
		if (!SpeedControlScreen.Instance.IsPaused)
		{
			this.UnscaledGameTime += Time.unscaledDeltaTime;
		}
	}
}
