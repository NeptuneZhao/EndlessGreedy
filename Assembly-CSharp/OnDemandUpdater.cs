using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009D5 RID: 2517
public class OnDemandUpdater : MonoBehaviour
{
	// Token: 0x0600491A RID: 18714 RVA: 0x001A2C2B File Offset: 0x001A0E2B
	public static void DestroyInstance()
	{
		OnDemandUpdater.Instance = null;
	}

	// Token: 0x0600491B RID: 18715 RVA: 0x001A2C33 File Offset: 0x001A0E33
	private void Awake()
	{
		OnDemandUpdater.Instance = this;
	}

	// Token: 0x0600491C RID: 18716 RVA: 0x001A2C3B File Offset: 0x001A0E3B
	public void Register(IUpdateOnDemand updater)
	{
		if (!this.Updaters.Contains(updater))
		{
			this.Updaters.Add(updater);
		}
	}

	// Token: 0x0600491D RID: 18717 RVA: 0x001A2C57 File Offset: 0x001A0E57
	public void Unregister(IUpdateOnDemand updater)
	{
		if (this.Updaters.Contains(updater))
		{
			this.Updaters.Remove(updater);
		}
	}

	// Token: 0x0600491E RID: 18718 RVA: 0x001A2C74 File Offset: 0x001A0E74
	private void Update()
	{
		for (int i = 0; i < this.Updaters.Count; i++)
		{
			if (this.Updaters[i] != null)
			{
				this.Updaters[i].UpdateOnDemand();
			}
		}
	}

	// Token: 0x04002FD7 RID: 12247
	private List<IUpdateOnDemand> Updaters = new List<IUpdateOnDemand>();

	// Token: 0x04002FD8 RID: 12248
	public static OnDemandUpdater Instance;
}
