using System;
using KSerialization;
using UnityEngine;

// Token: 0x020009ED RID: 2541
[AddComponentMenu("KMonoBehaviour/scripts/BuddingTrunk")]
public class BuddingTrunk : KMonoBehaviour
{
	// Token: 0x060049A0 RID: 18848 RVA: 0x001A57A4 File Offset: 0x001A39A4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		PlantBranchGrower.Instance smi = base.gameObject.GetSMI<PlantBranchGrower.Instance>();
		if (smi != null && !smi.IsRunning())
		{
			smi.StartSM();
		}
	}

	// Token: 0x060049A1 RID: 18849 RVA: 0x001A57D4 File Offset: 0x001A39D4
	public KPrefabID[] GetAndForgetOldSerializedBranches()
	{
		KPrefabID[] array = null;
		if (this.buds != null)
		{
			array = new KPrefabID[this.buds.Length];
			for (int i = 0; i < this.buds.Length; i++)
			{
				HarvestDesignatable harvestDesignatable = (this.buds[i] == null) ? null : this.buds[i].Get();
				array[i] = ((harvestDesignatable == null) ? null : harvestDesignatable.GetComponent<KPrefabID>());
			}
		}
		this.buds = null;
		return array;
	}

	// Token: 0x0400302F RID: 12335
	[Serialize]
	private Ref<HarvestDesignatable>[] buds;
}
