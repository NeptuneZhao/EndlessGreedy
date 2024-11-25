using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A32 RID: 2610
public class FossilDigsiteLampLight : Light2D
{
	// Token: 0x1700055B RID: 1371
	// (get) Token: 0x06004B96 RID: 19350 RVA: 0x001AF534 File Offset: 0x001AD734
	// (set) Token: 0x06004B95 RID: 19349 RVA: 0x001AF52B File Offset: 0x001AD72B
	public bool independent { get; private set; }

	// Token: 0x06004B97 RID: 19351 RVA: 0x001AF53C File Offset: 0x001AD73C
	protected override void OnPrefabInit()
	{
		base.Subscribe<FossilDigsiteLampLight>(-592767678, FossilDigsiteLampLight.OnOperationalChangedDelegate);
		base.IntensityAnimation = 1f;
	}

	// Token: 0x06004B98 RID: 19352 RVA: 0x001AF55C File Offset: 0x001AD75C
	public void SetIndependentState(bool isIndependent, bool checkOperational = true)
	{
		this.independent = isIndependent;
		Operational component = base.GetComponent<Operational>();
		if (component != null && this.independent && checkOperational && base.enabled != component.IsOperational)
		{
			base.enabled = component.IsOperational;
		}
	}

	// Token: 0x06004B99 RID: 19353 RVA: 0x001AF5A7 File Offset: 0x001AD7A7
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		if (this.independent || base.enabled)
		{
			return base.GetDescriptors(go);
		}
		return new List<Descriptor>();
	}

	// Token: 0x0400317F RID: 12671
	private static readonly EventSystem.IntraObjectHandler<FossilDigsiteLampLight> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<FossilDigsiteLampLight>(delegate(FossilDigsiteLampLight light, object data)
	{
		if (light.independent)
		{
			light.enabled = (bool)data;
		}
	});
}
