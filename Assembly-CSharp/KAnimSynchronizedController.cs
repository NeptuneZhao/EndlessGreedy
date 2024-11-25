using System;
using UnityEngine;

// Token: 0x020004E3 RID: 1251
public class KAnimSynchronizedController
{
	// Token: 0x170000CD RID: 205
	// (get) Token: 0x06001B76 RID: 7030 RVA: 0x0008FC60 File Offset: 0x0008DE60
	// (set) Token: 0x06001B77 RID: 7031 RVA: 0x0008FC68 File Offset: 0x0008DE68
	public string Postfix
	{
		get
		{
			return this.postfix;
		}
		set
		{
			this.postfix = value;
		}
	}

	// Token: 0x06001B78 RID: 7032 RVA: 0x0008FC74 File Offset: 0x0008DE74
	public KAnimSynchronizedController(KAnimControllerBase controller, Grid.SceneLayer layer, string postfix)
	{
		this.controller = controller;
		this.Postfix = postfix;
		GameObject gameObject = Util.KInstantiate(EntityPrefabs.Instance.ForegroundLayer, controller.gameObject, null);
		gameObject.name = controller.name + postfix;
		this.synchronizedController = gameObject.GetComponent<KAnimControllerBase>();
		this.synchronizedController.AnimFiles = controller.AnimFiles;
		gameObject.SetActive(true);
		this.synchronizedController.initialAnim = controller.initialAnim + postfix;
		this.synchronizedController.defaultAnim = this.synchronizedController.initialAnim;
		Vector3 position = new Vector3(0f, 0f, Grid.GetLayerZ(layer) - 0.1f);
		gameObject.transform.SetLocalPosition(position);
		this.link = new KAnimLink(controller, this.synchronizedController);
		this.Dirty();
		KAnimSynchronizer synchronizer = controller.GetSynchronizer();
		synchronizer.Add(this);
		synchronizer.SyncController(this);
	}

	// Token: 0x06001B79 RID: 7033 RVA: 0x0008FD64 File Offset: 0x0008DF64
	public void Enable(bool enable)
	{
		this.synchronizedController.enabled = enable;
	}

	// Token: 0x06001B7A RID: 7034 RVA: 0x0008FD72 File Offset: 0x0008DF72
	public void Play(HashedString anim_name, KAnim.PlayMode mode = KAnim.PlayMode.Once, float speed = 1f, float time_offset = 0f)
	{
		if (this.synchronizedController.enabled && this.synchronizedController.HasAnimation(anim_name))
		{
			this.synchronizedController.Play(anim_name, mode, speed, time_offset);
		}
	}

	// Token: 0x06001B7B RID: 7035 RVA: 0x0008FDA0 File Offset: 0x0008DFA0
	public void Dirty()
	{
		if (this.synchronizedController == null)
		{
			return;
		}
		this.synchronizedController.Offset = this.controller.Offset;
		this.synchronizedController.Pivot = this.controller.Pivot;
		this.synchronizedController.Rotation = this.controller.Rotation;
		this.synchronizedController.FlipX = this.controller.FlipX;
		this.synchronizedController.FlipY = this.controller.FlipY;
	}

	// Token: 0x04000F89 RID: 3977
	private KAnimControllerBase controller;

	// Token: 0x04000F8A RID: 3978
	public KAnimControllerBase synchronizedController;

	// Token: 0x04000F8B RID: 3979
	private KAnimLink link;

	// Token: 0x04000F8C RID: 3980
	private string postfix;
}
