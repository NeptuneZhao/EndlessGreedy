using System;
using UnityEngine;

// Token: 0x0200085A RID: 2138
public struct DiseaseContainer
{
	// Token: 0x06003B87 RID: 15239 RVA: 0x00147D28 File Offset: 0x00145F28
	public DiseaseContainer(GameObject go, ushort elemIdx)
	{
		this.elemIdx = elemIdx;
		this.isContainer = (go.GetComponent<IUserControlledCapacity>() != null && go.GetComponent<Storage>() != null);
		Conduit component = go.GetComponent<Conduit>();
		if (component != null)
		{
			this.conduitType = component.type;
		}
		else
		{
			this.conduitType = ConduitType.None;
		}
		this.controller = go.GetComponent<KBatchedAnimController>();
		this.overpopulationCount = 1;
		this.instanceGrowthRate = 1f;
		this.accumulatedError = 0f;
		this.visualDiseaseProvider = null;
		this.autoDisinfectable = go.GetComponent<AutoDisinfectable>();
		if (this.autoDisinfectable != null)
		{
			AutoDisinfectableManager.Instance.AddAutoDisinfectable(this.autoDisinfectable);
		}
	}

	// Token: 0x06003B88 RID: 15240 RVA: 0x00147DD8 File Offset: 0x00145FD8
	public void Clear()
	{
		this.controller = null;
	}

	// Token: 0x040023F9 RID: 9209
	public AutoDisinfectable autoDisinfectable;

	// Token: 0x040023FA RID: 9210
	public ushort elemIdx;

	// Token: 0x040023FB RID: 9211
	public bool isContainer;

	// Token: 0x040023FC RID: 9212
	public ConduitType conduitType;

	// Token: 0x040023FD RID: 9213
	public KBatchedAnimController controller;

	// Token: 0x040023FE RID: 9214
	public GameObject visualDiseaseProvider;

	// Token: 0x040023FF RID: 9215
	public int overpopulationCount;

	// Token: 0x04002400 RID: 9216
	public float instanceGrowthRate;

	// Token: 0x04002401 RID: 9217
	public float accumulatedError;
}
