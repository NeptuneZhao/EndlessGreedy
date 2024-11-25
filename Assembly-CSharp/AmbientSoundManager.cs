using System;
using UnityEngine;

// Token: 0x02000646 RID: 1606
[AddComponentMenu("KMonoBehaviour/scripts/AmbientSoundManager")]
public class AmbientSoundManager : KMonoBehaviour
{
	// Token: 0x170001E4 RID: 484
	// (get) Token: 0x0600274E RID: 10062 RVA: 0x000DFC35 File Offset: 0x000DDE35
	// (set) Token: 0x0600274F RID: 10063 RVA: 0x000DFC3C File Offset: 0x000DDE3C
	public static AmbientSoundManager Instance { get; private set; }

	// Token: 0x06002750 RID: 10064 RVA: 0x000DFC44 File Offset: 0x000DDE44
	public static void Destroy()
	{
		AmbientSoundManager.Instance = null;
	}

	// Token: 0x06002751 RID: 10065 RVA: 0x000DFC4C File Offset: 0x000DDE4C
	protected override void OnPrefabInit()
	{
		AmbientSoundManager.Instance = this;
	}

	// Token: 0x06002752 RID: 10066 RVA: 0x000DFC54 File Offset: 0x000DDE54
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06002753 RID: 10067 RVA: 0x000DFC5C File Offset: 0x000DDE5C
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		AmbientSoundManager.Instance = null;
	}

	// Token: 0x04001699 RID: 5785
	[MyCmpAdd]
	private LoopingSounds loopingSounds;
}
