using System;
using UnityEngine;

// Token: 0x020004E0 RID: 1248
public class KAnimLayering
{
	// Token: 0x06001B60 RID: 7008 RVA: 0x0008F4B6 File Offset: 0x0008D6B6
	public KAnimLayering(KAnimControllerBase controller, Grid.SceneLayer layer)
	{
		this.controller = controller;
		this.layer = layer;
	}

	// Token: 0x06001B61 RID: 7009 RVA: 0x0008F4D4 File Offset: 0x0008D6D4
	public void SetLayer(Grid.SceneLayer layer)
	{
		this.layer = layer;
		if (this.foregroundController != null)
		{
			Vector3 position = new Vector3(0f, 0f, Grid.GetLayerZ(layer) - this.controller.gameObject.transform.GetPosition().z - 0.1f);
			this.foregroundController.transform.SetLocalPosition(position);
		}
	}

	// Token: 0x06001B62 RID: 7010 RVA: 0x0008F540 File Offset: 0x0008D740
	public void SetIsForeground(bool is_foreground)
	{
		this.isForeground = is_foreground;
	}

	// Token: 0x06001B63 RID: 7011 RVA: 0x0008F549 File Offset: 0x0008D749
	public bool GetIsForeground()
	{
		return this.isForeground;
	}

	// Token: 0x06001B64 RID: 7012 RVA: 0x0008F551 File Offset: 0x0008D751
	public KAnimLink GetLink()
	{
		return this.link;
	}

	// Token: 0x06001B65 RID: 7013 RVA: 0x0008F55C File Offset: 0x0008D75C
	private static bool IsAnimLayered(KAnimFile[] anims)
	{
		foreach (KAnimFile kanimFile in anims)
		{
			if (!(kanimFile == null))
			{
				KAnimFileData data = kanimFile.GetData();
				if (data.build != null)
				{
					KAnim.Build.Symbol[] symbols = data.build.symbols;
					for (int j = 0; j < symbols.Length; j++)
					{
						if ((symbols[j].flags & 8) != 0)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06001B66 RID: 7014 RVA: 0x0008F5C4 File Offset: 0x0008D7C4
	private void HideSymbolsInternal()
	{
		foreach (KAnimFile kanimFile in this.controller.AnimFiles)
		{
			if (!(kanimFile == null))
			{
				KAnimFileData data = kanimFile.GetData();
				if (data.build != null)
				{
					KAnim.Build.Symbol[] symbols = data.build.symbols;
					for (int j = 0; j < symbols.Length; j++)
					{
						if ((symbols[j].flags & 8) != 0 != this.isForeground && !(symbols[j].hash == KAnimLayering.UI))
						{
							this.controller.SetSymbolVisiblity(symbols[j].hash, false);
						}
					}
				}
			}
		}
	}

	// Token: 0x06001B67 RID: 7015 RVA: 0x0008F670 File Offset: 0x0008D870
	public void HideSymbols()
	{
		if (EntityPrefabs.Instance == null)
		{
			return;
		}
		if (this.isForeground)
		{
			return;
		}
		KAnimFile[] animFiles = this.controller.AnimFiles;
		bool flag = KAnimLayering.IsAnimLayered(animFiles);
		if (flag && this.layer != Grid.SceneLayer.NoLayer)
		{
			bool flag2 = this.foregroundController == null;
			if (flag2)
			{
				GameObject gameObject = Util.KInstantiate(EntityPrefabs.Instance.ForegroundLayer, this.controller.gameObject, null);
				gameObject.name = this.controller.name + "_fg";
				this.foregroundController = gameObject.GetComponent<KAnimControllerBase>();
				this.link = new KAnimLink(this.controller, this.foregroundController);
			}
			this.foregroundController.AnimFiles = animFiles;
			this.foregroundController.GetLayering().SetIsForeground(true);
			this.foregroundController.initialAnim = this.controller.initialAnim;
			this.Dirty();
			KAnimSynchronizer synchronizer = this.controller.GetSynchronizer();
			if (flag2)
			{
				synchronizer.Add(this.foregroundController);
			}
			else
			{
				this.foregroundController.GetComponent<KBatchedAnimController>().SwapAnims(this.foregroundController.AnimFiles);
			}
			synchronizer.Sync(this.foregroundController);
			Vector3 position = new Vector3(0f, 0f, Grid.GetLayerZ(this.layer) - this.controller.gameObject.transform.GetPosition().z - 0.1f);
			this.foregroundController.gameObject.transform.SetLocalPosition(position);
			this.foregroundController.gameObject.SetActive(true);
		}
		else if (!flag && this.foregroundController != null)
		{
			this.controller.GetSynchronizer().Remove(this.foregroundController);
			this.foregroundController.gameObject.DeleteObject();
			this.link = null;
		}
		if (this.foregroundController != null)
		{
			this.HideSymbolsInternal();
			KAnimLayering layering = this.foregroundController.GetLayering();
			if (layering != null)
			{
				layering.HideSymbolsInternal();
			}
		}
	}

	// Token: 0x06001B68 RID: 7016 RVA: 0x0008F873 File Offset: 0x0008DA73
	public void RefreshForegroundBatchGroup()
	{
		if (this.foregroundController == null)
		{
			return;
		}
		this.foregroundController.GetComponent<KBatchedAnimController>().SwapAnims(this.foregroundController.AnimFiles);
	}

	// Token: 0x06001B69 RID: 7017 RVA: 0x0008F8A0 File Offset: 0x0008DAA0
	public void Dirty()
	{
		if (this.foregroundController == null)
		{
			return;
		}
		this.foregroundController.Offset = this.controller.Offset;
		this.foregroundController.Pivot = this.controller.Pivot;
		this.foregroundController.Rotation = this.controller.Rotation;
		this.foregroundController.FlipX = this.controller.FlipX;
		this.foregroundController.FlipY = this.controller.FlipY;
	}

	// Token: 0x04000F7B RID: 3963
	private bool isForeground;

	// Token: 0x04000F7C RID: 3964
	private KAnimControllerBase controller;

	// Token: 0x04000F7D RID: 3965
	private KAnimControllerBase foregroundController;

	// Token: 0x04000F7E RID: 3966
	private KAnimLink link;

	// Token: 0x04000F7F RID: 3967
	private Grid.SceneLayer layer = Grid.SceneLayer.BuildingFront;

	// Token: 0x04000F80 RID: 3968
	public static readonly KAnimHashedString UI = new KAnimHashedString("ui");
}
