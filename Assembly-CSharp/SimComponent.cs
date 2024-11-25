using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000A98 RID: 2712
public abstract class SimComponent : KMonoBehaviour, ISim200ms
{
	// Token: 0x170005B6 RID: 1462
	// (get) Token: 0x06004F81 RID: 20353 RVA: 0x001C9418 File Offset: 0x001C7618
	public bool IsSimActive
	{
		get
		{
			return this.simActive;
		}
	}

	// Token: 0x06004F82 RID: 20354 RVA: 0x001C9420 File Offset: 0x001C7620
	protected virtual void OnSimRegister(HandleVector<Game.ComplexCallbackInfo<int>>.Handle cb_handle)
	{
	}

	// Token: 0x06004F83 RID: 20355 RVA: 0x001C9422 File Offset: 0x001C7622
	protected virtual void OnSimRegistered()
	{
	}

	// Token: 0x06004F84 RID: 20356 RVA: 0x001C9424 File Offset: 0x001C7624
	protected virtual void OnSimActivate()
	{
	}

	// Token: 0x06004F85 RID: 20357 RVA: 0x001C9426 File Offset: 0x001C7626
	protected virtual void OnSimDeactivate()
	{
	}

	// Token: 0x06004F86 RID: 20358 RVA: 0x001C9428 File Offset: 0x001C7628
	protected virtual void OnSimUnregister()
	{
	}

	// Token: 0x06004F87 RID: 20359
	protected abstract Action<int> GetStaticUnregister();

	// Token: 0x06004F88 RID: 20360 RVA: 0x001C942A File Offset: 0x001C762A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06004F89 RID: 20361 RVA: 0x001C9432 File Offset: 0x001C7632
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.SimRegister();
	}

	// Token: 0x06004F8A RID: 20362 RVA: 0x001C9440 File Offset: 0x001C7640
	protected override void OnCleanUp()
	{
		this.SimUnregister();
		base.OnCleanUp();
	}

	// Token: 0x06004F8B RID: 20363 RVA: 0x001C944E File Offset: 0x001C764E
	public void SetSimActive(bool active)
	{
		this.simActive = active;
		this.dirty = true;
	}

	// Token: 0x06004F8C RID: 20364 RVA: 0x001C945E File Offset: 0x001C765E
	public void Sim200ms(float dt)
	{
		if (!Sim.IsValidHandle(this.simHandle))
		{
			return;
		}
		this.UpdateSimState();
	}

	// Token: 0x06004F8D RID: 20365 RVA: 0x001C9474 File Offset: 0x001C7674
	private void UpdateSimState()
	{
		if (!this.dirty)
		{
			return;
		}
		this.dirty = false;
		if (this.simActive)
		{
			this.OnSimActivate();
			return;
		}
		this.OnSimDeactivate();
	}

	// Token: 0x06004F8E RID: 20366 RVA: 0x001C949C File Offset: 0x001C769C
	private void SimRegister()
	{
		if (base.isSpawned && this.simHandle == -1)
		{
			this.simHandle = -2;
			Action<int> static_unregister = this.GetStaticUnregister();
			HandleVector<Game.ComplexCallbackInfo<int>>.Handle cb_handle = Game.Instance.simComponentCallbackManager.Add(delegate(int handle, object data)
			{
				SimComponent.OnSimRegistered(this, handle, static_unregister);
			}, this, "SimComponent.SimRegister");
			this.OnSimRegister(cb_handle);
		}
	}

	// Token: 0x06004F8F RID: 20367 RVA: 0x001C9504 File Offset: 0x001C7704
	private void SimUnregister()
	{
		if (Sim.IsValidHandle(this.simHandle))
		{
			this.OnSimUnregister();
		}
		this.simHandle = -1;
	}

	// Token: 0x06004F90 RID: 20368 RVA: 0x001C9520 File Offset: 0x001C7720
	private static void OnSimRegistered(SimComponent instance, int handle, Action<int> static_unregister)
	{
		if (instance != null)
		{
			instance.simHandle = handle;
			instance.OnSimRegistered();
			return;
		}
		static_unregister(handle);
	}

	// Token: 0x06004F91 RID: 20369 RVA: 0x001C9540 File Offset: 0x001C7740
	[Conditional("ENABLE_LOGGER")]
	protected void Log(string msg)
	{
	}

	// Token: 0x040034DA RID: 13530
	[SerializeField]
	protected int simHandle = -1;

	// Token: 0x040034DB RID: 13531
	private bool simActive = true;

	// Token: 0x040034DC RID: 13532
	private bool dirty = true;
}
