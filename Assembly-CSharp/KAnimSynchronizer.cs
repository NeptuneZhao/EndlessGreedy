using System;
using System.Collections.Generic;

// Token: 0x020004E4 RID: 1252
public class KAnimSynchronizer
{
	// Token: 0x170000CE RID: 206
	// (get) Token: 0x06001B7C RID: 7036 RVA: 0x0008FE2A File Offset: 0x0008E02A
	// (set) Token: 0x06001B7D RID: 7037 RVA: 0x0008FE32 File Offset: 0x0008E032
	public string IdleAnim
	{
		get
		{
			return this.idle_anim;
		}
		set
		{
			this.idle_anim = value;
		}
	}

	// Token: 0x06001B7E RID: 7038 RVA: 0x0008FE3B File Offset: 0x0008E03B
	public KAnimSynchronizer(KAnimControllerBase master_controller)
	{
		this.masterController = master_controller;
	}

	// Token: 0x06001B7F RID: 7039 RVA: 0x0008FE6B File Offset: 0x0008E06B
	private void Clear(KAnimControllerBase controller)
	{
		controller.Play(this.IdleAnim, KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06001B80 RID: 7040 RVA: 0x0008FE89 File Offset: 0x0008E089
	public void Add(KAnimControllerBase controller)
	{
		this.Targets.Add(controller);
	}

	// Token: 0x06001B81 RID: 7041 RVA: 0x0008FE97 File Offset: 0x0008E097
	public void Remove(KAnimControllerBase controller)
	{
		this.Clear(controller);
		this.Targets.Remove(controller);
	}

	// Token: 0x06001B82 RID: 7042 RVA: 0x0008FEAD File Offset: 0x0008E0AD
	public void RemoveWithoutIdleAnim(KAnimControllerBase controller)
	{
		this.Targets.Remove(controller);
	}

	// Token: 0x06001B83 RID: 7043 RVA: 0x0008FEBC File Offset: 0x0008E0BC
	private void Clear(KAnimSynchronizedController controller)
	{
		controller.Play(this.IdleAnim, KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06001B84 RID: 7044 RVA: 0x0008FEDA File Offset: 0x0008E0DA
	public void Add(KAnimSynchronizedController controller)
	{
		this.SyncedControllers.Add(controller);
	}

	// Token: 0x06001B85 RID: 7045 RVA: 0x0008FEE8 File Offset: 0x0008E0E8
	public void Remove(KAnimSynchronizedController controller)
	{
		this.Clear(controller);
		this.SyncedControllers.Remove(controller);
	}

	// Token: 0x06001B86 RID: 7046 RVA: 0x0008FF00 File Offset: 0x0008E100
	public void Clear()
	{
		foreach (KAnimControllerBase kanimControllerBase in this.Targets)
		{
			if (!(kanimControllerBase == null) && kanimControllerBase.AnimFiles != null)
			{
				this.Clear(kanimControllerBase);
			}
		}
		this.Targets.Clear();
		foreach (KAnimSynchronizedController kanimSynchronizedController in this.SyncedControllers)
		{
			if (!(kanimSynchronizedController.synchronizedController == null) && kanimSynchronizedController.synchronizedController.AnimFiles != null)
			{
				this.Clear(kanimSynchronizedController);
			}
		}
		this.SyncedControllers.Clear();
	}

	// Token: 0x06001B87 RID: 7047 RVA: 0x0008FFD8 File Offset: 0x0008E1D8
	public void Sync(KAnimControllerBase controller)
	{
		if (this.masterController == null)
		{
			return;
		}
		if (controller == null)
		{
			return;
		}
		KAnim.Anim currentAnim = this.masterController.GetCurrentAnim();
		if (currentAnim != null && !string.IsNullOrEmpty(controller.defaultAnim) && !controller.HasAnimation(currentAnim.name))
		{
			controller.Play(controller.defaultAnim, KAnim.PlayMode.Loop, 1f, 0f);
			return;
		}
		if (currentAnim == null)
		{
			return;
		}
		KAnim.PlayMode mode = this.masterController.GetMode();
		float playSpeed = this.masterController.GetPlaySpeed();
		float elapsedTime = this.masterController.GetElapsedTime();
		controller.Play(currentAnim.name, mode, playSpeed, elapsedTime);
		Facing component = controller.GetComponent<Facing>();
		if (component != null)
		{
			float num = component.transform.GetPosition().x;
			num += (this.masterController.FlipX ? -0.5f : 0.5f);
			component.Face(num);
			return;
		}
		controller.FlipX = this.masterController.FlipX;
		controller.FlipY = this.masterController.FlipY;
	}

	// Token: 0x06001B88 RID: 7048 RVA: 0x000900F8 File Offset: 0x0008E2F8
	public void SyncController(KAnimSynchronizedController controller)
	{
		if (this.masterController == null)
		{
			return;
		}
		if (controller == null)
		{
			return;
		}
		KAnim.Anim currentAnim = this.masterController.GetCurrentAnim();
		string s = (currentAnim != null) ? (currentAnim.name + controller.Postfix) : string.Empty;
		if (!string.IsNullOrEmpty(controller.synchronizedController.defaultAnim) && !controller.synchronizedController.HasAnimation(s))
		{
			controller.Play(controller.synchronizedController.defaultAnim, KAnim.PlayMode.Loop, 1f, 0f);
			return;
		}
		if (currentAnim == null)
		{
			return;
		}
		KAnim.PlayMode mode = this.masterController.GetMode();
		float playSpeed = this.masterController.GetPlaySpeed();
		float elapsedTime = this.masterController.GetElapsedTime();
		controller.Play(s, mode, playSpeed, elapsedTime);
		Facing component = controller.synchronizedController.GetComponent<Facing>();
		if (component != null)
		{
			float num = component.transform.GetPosition().x;
			num += (this.masterController.FlipX ? -0.5f : 0.5f);
			component.Face(num);
			return;
		}
		controller.synchronizedController.FlipX = this.masterController.FlipX;
		controller.synchronizedController.FlipY = this.masterController.FlipY;
	}

	// Token: 0x06001B89 RID: 7049 RVA: 0x00090240 File Offset: 0x0008E440
	public void Sync()
	{
		for (int i = 0; i < this.Targets.Count; i++)
		{
			KAnimControllerBase controller = this.Targets[i];
			this.Sync(controller);
		}
		for (int j = 0; j < this.SyncedControllers.Count; j++)
		{
			KAnimSynchronizedController controller2 = this.SyncedControllers[j];
			this.SyncController(controller2);
		}
	}

	// Token: 0x06001B8A RID: 7050 RVA: 0x000902A4 File Offset: 0x0008E4A4
	public void SyncTime()
	{
		float elapsedTime = this.masterController.GetElapsedTime();
		for (int i = 0; i < this.Targets.Count; i++)
		{
			this.Targets[i].SetElapsedTime(elapsedTime);
		}
		for (int j = 0; j < this.SyncedControllers.Count; j++)
		{
			this.SyncedControllers[j].synchronizedController.SetElapsedTime(elapsedTime);
		}
	}

	// Token: 0x04000F8D RID: 3981
	private string idle_anim = "idle_default";

	// Token: 0x04000F8E RID: 3982
	private KAnimControllerBase masterController;

	// Token: 0x04000F8F RID: 3983
	private List<KAnimControllerBase> Targets = new List<KAnimControllerBase>();

	// Token: 0x04000F90 RID: 3984
	private List<KAnimSynchronizedController> SyncedControllers = new List<KAnimSynchronizedController>();
}
