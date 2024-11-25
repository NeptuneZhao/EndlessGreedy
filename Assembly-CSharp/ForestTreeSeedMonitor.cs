using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200016A RID: 362
public class ForestTreeSeedMonitor : KMonoBehaviour
{
	// Token: 0x17000011 RID: 17
	// (get) Token: 0x0600071B RID: 1819 RVA: 0x0002F43F File Offset: 0x0002D63F
	public bool ExtraSeedAvailable
	{
		get
		{
			return this.hasExtraSeedAvailable;
		}
	}

	// Token: 0x0600071C RID: 1820 RVA: 0x0002F448 File Offset: 0x0002D648
	public void ExtractExtraSeed()
	{
		if (!this.hasExtraSeedAvailable)
		{
			return;
		}
		this.hasExtraSeedAvailable = false;
		Vector3 position = base.transform.position;
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
		Util.KInstantiate(Assets.GetPrefab("ForestTreeSeed"), position).SetActive(true);
	}

	// Token: 0x0600071D RID: 1821 RVA: 0x0002F49A File Offset: 0x0002D69A
	public void TryRollNewSeed()
	{
		if (!this.hasExtraSeedAvailable && UnityEngine.Random.Range(0, 100) < 5)
		{
			this.hasExtraSeedAvailable = true;
		}
	}

	// Token: 0x04000505 RID: 1285
	[Serialize]
	private bool hasExtraSeedAvailable;
}
