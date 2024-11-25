using System;
using KSerialization;
using UnityEngine;

// Token: 0x020009FC RID: 2556
[AddComponentMenu("KMonoBehaviour/scripts/TreeBud")]
public class TreeBud : KMonoBehaviour
{
	// Token: 0x060049FA RID: 18938 RVA: 0x001A70E4 File Offset: 0x001A52E4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		PlantBranch.Instance smi = base.gameObject.GetSMI<PlantBranch.Instance>();
		if (smi != null && !smi.IsRunning())
		{
			smi.StartSM();
		}
	}

	// Token: 0x060049FB RID: 18939 RVA: 0x001A7114 File Offset: 0x001A5314
	public BuddingTrunk GetAndForgetOldTrunk()
	{
		BuddingTrunk result = (this.buddingTrunk == null) ? null : this.buddingTrunk.Get();
		this.buddingTrunk = null;
		return result;
	}

	// Token: 0x0400308B RID: 12427
	[Serialize]
	public Ref<BuddingTrunk> buddingTrunk;
}
