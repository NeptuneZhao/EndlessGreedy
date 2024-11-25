using System;
using UnityEngine;

// Token: 0x020005A5 RID: 1445
[AddComponentMenu("KMonoBehaviour/scripts/PumpingStationGuide")]
public class PumpingStationGuide : KMonoBehaviour, IRenderEveryTick
{
	// Token: 0x06002262 RID: 8802 RVA: 0x000BF830 File Offset: 0x000BDA30
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.parentController = this.parent.GetComponent<KBatchedAnimController>();
		this.guideController = base.GetComponent<KBatchedAnimController>();
		this.RefreshTint();
		this.RefreshDepthAvailable();
	}

	// Token: 0x06002263 RID: 8803 RVA: 0x000BF861 File Offset: 0x000BDA61
	public void RefreshPosition()
	{
		if (this.guideController != null && this.guideController.IsMoving)
		{
			this.guideController.SetDirty();
		}
	}

	// Token: 0x06002264 RID: 8804 RVA: 0x000BF889 File Offset: 0x000BDA89
	private void RefreshTint()
	{
		this.guideController.TintColour = this.parentController.TintColour;
	}

	// Token: 0x06002265 RID: 8805 RVA: 0x000BF8A4 File Offset: 0x000BDAA4
	private void RefreshDepthAvailable()
	{
		int depthAvailable = PumpingStationGuide.GetDepthAvailable(Grid.PosToCell(this), this.parent);
		if (depthAvailable != this.previousDepthAvailable)
		{
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			if (depthAvailable == 0)
			{
				component.enabled = false;
			}
			else
			{
				component.enabled = true;
				component.Play(new HashedString("place_pipe" + depthAvailable.ToString()), KAnim.PlayMode.Once, 1f, 0f);
			}
			if (this.occupyTiles)
			{
				PumpingStationGuide.OccupyArea(this.parent, depthAvailable);
			}
			this.previousDepthAvailable = depthAvailable;
		}
	}

	// Token: 0x06002266 RID: 8806 RVA: 0x000BF928 File Offset: 0x000BDB28
	public void RenderEveryTick(float dt)
	{
		this.RefreshPosition();
		this.RefreshTint();
		this.RefreshDepthAvailable();
	}

	// Token: 0x06002267 RID: 8807 RVA: 0x000BF93C File Offset: 0x000BDB3C
	public static void OccupyArea(GameObject go, int depth_available)
	{
		int cell = Grid.PosToCell(go.transform.GetPosition());
		for (int i = 1; i <= 4; i++)
		{
			int key = Grid.OffsetCell(cell, 0, -i);
			int key2 = Grid.OffsetCell(cell, 1, -i);
			if (i <= depth_available)
			{
				Grid.ObjectLayers[1][key] = go;
				Grid.ObjectLayers[1][key2] = go;
			}
			else
			{
				if (Grid.ObjectLayers[1].ContainsKey(key) && Grid.ObjectLayers[1][key] == go)
				{
					Grid.ObjectLayers[1][key] = null;
				}
				if (Grid.ObjectLayers[1].ContainsKey(key2) && Grid.ObjectLayers[1][key2] == go)
				{
					Grid.ObjectLayers[1][key2] = null;
				}
			}
		}
	}

	// Token: 0x06002268 RID: 8808 RVA: 0x000BFA0C File Offset: 0x000BDC0C
	public static int GetDepthAvailable(int root_cell, GameObject pump)
	{
		int num = 4;
		int result = 0;
		for (int i = 1; i <= num; i++)
		{
			int num2 = Grid.OffsetCell(root_cell, 0, -i);
			int num3 = Grid.OffsetCell(root_cell, 1, -i);
			if (!Grid.IsValidCell(num2) || Grid.Solid[num2] || !Grid.IsValidCell(num3) || Grid.Solid[num3] || (Grid.ObjectLayers[1].ContainsKey(num2) && !(Grid.ObjectLayers[1][num2] == null) && !(Grid.ObjectLayers[1][num2] == pump)) || (Grid.ObjectLayers[1].ContainsKey(num3) && !(Grid.ObjectLayers[1][num3] == null) && !(Grid.ObjectLayers[1][num3] == pump)))
			{
				break;
			}
			result = i;
		}
		return result;
	}

	// Token: 0x04001360 RID: 4960
	private int previousDepthAvailable = -1;

	// Token: 0x04001361 RID: 4961
	public GameObject parent;

	// Token: 0x04001362 RID: 4962
	public bool occupyTiles;

	// Token: 0x04001363 RID: 4963
	private KBatchedAnimController parentController;

	// Token: 0x04001364 RID: 4964
	private KBatchedAnimController guideController;
}
