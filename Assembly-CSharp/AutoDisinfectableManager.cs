using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200052C RID: 1324
[AddComponentMenu("KMonoBehaviour/scripts/AutoDisinfectableManager")]
public class AutoDisinfectableManager : KMonoBehaviour, ISim1000ms
{
	// Token: 0x06001DCE RID: 7630 RVA: 0x000A55C3 File Offset: 0x000A37C3
	public static void DestroyInstance()
	{
		AutoDisinfectableManager.Instance = null;
	}

	// Token: 0x06001DCF RID: 7631 RVA: 0x000A55CB File Offset: 0x000A37CB
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		AutoDisinfectableManager.Instance = this;
	}

	// Token: 0x06001DD0 RID: 7632 RVA: 0x000A55D9 File Offset: 0x000A37D9
	public void AddAutoDisinfectable(AutoDisinfectable auto_disinfectable)
	{
		this.autoDisinfectables.Add(auto_disinfectable);
	}

	// Token: 0x06001DD1 RID: 7633 RVA: 0x000A55E7 File Offset: 0x000A37E7
	public void RemoveAutoDisinfectable(AutoDisinfectable auto_disinfectable)
	{
		auto_disinfectable.CancelChore();
		this.autoDisinfectables.Remove(auto_disinfectable);
	}

	// Token: 0x06001DD2 RID: 7634 RVA: 0x000A55FC File Offset: 0x000A37FC
	public void Sim1000ms(float dt)
	{
		for (int i = 0; i < this.autoDisinfectables.Count; i++)
		{
			this.autoDisinfectables[i].RefreshChore();
		}
	}

	// Token: 0x040010BC RID: 4284
	private List<AutoDisinfectable> autoDisinfectables = new List<AutoDisinfectable>();

	// Token: 0x040010BD RID: 4285
	public static AutoDisinfectableManager Instance;
}
