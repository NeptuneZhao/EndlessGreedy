using System;
using UnityEngine;

// Token: 0x02000B5C RID: 2908
public class WaterTrapGuide : KMonoBehaviour, IRenderEveryTick
{
	// Token: 0x060056E6 RID: 22246 RVA: 0x001F0F99 File Offset: 0x001EF199
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.parentController = this.parent.GetComponent<KBatchedAnimController>();
		this.guideController = base.GetComponent<KBatchedAnimController>();
		this.RefreshTint();
		this.RefreshDepthAvailable();
	}

	// Token: 0x060056E7 RID: 22247 RVA: 0x001F0FCA File Offset: 0x001EF1CA
	private void RefreshTint()
	{
		this.guideController.TintColour = this.parentController.TintColour;
	}

	// Token: 0x060056E8 RID: 22248 RVA: 0x001F0FE2 File Offset: 0x001EF1E2
	public void RefreshPosition()
	{
		if (this.guideController != null && this.guideController.IsMoving)
		{
			this.guideController.SetDirty();
		}
	}

	// Token: 0x060056E9 RID: 22249 RVA: 0x001F100C File Offset: 0x001EF20C
	private void RefreshDepthAvailable()
	{
		int depthAvailable = WaterTrapGuide.GetDepthAvailable(Grid.PosToCell(this), this.parent);
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
				WaterTrapGuide.OccupyArea(this.parent, depthAvailable);
			}
			this.previousDepthAvailable = depthAvailable;
		}
	}

	// Token: 0x060056EA RID: 22250 RVA: 0x001F1090 File Offset: 0x001EF290
	public void RenderEveryTick(float dt)
	{
		this.RefreshPosition();
		this.RefreshTint();
		this.RefreshDepthAvailable();
	}

	// Token: 0x060056EB RID: 22251 RVA: 0x001F10A4 File Offset: 0x001EF2A4
	public static void OccupyArea(GameObject go, int depth_available)
	{
		int cell = Grid.PosToCell(go.transform.GetPosition());
		for (int i = 1; i <= 4; i++)
		{
			int key = Grid.OffsetCell(cell, 0, -i);
			if (i <= depth_available)
			{
				Grid.ObjectLayers[1][key] = go;
			}
			else if (Grid.ObjectLayers[1].ContainsKey(key) && Grid.ObjectLayers[1][key] == go)
			{
				Grid.ObjectLayers[1][key] = null;
			}
		}
	}

	// Token: 0x060056EC RID: 22252 RVA: 0x001F1124 File Offset: 0x001EF324
	public static int GetDepthAvailable(int root_cell, GameObject pump)
	{
		int num = 4;
		int result = 0;
		for (int i = 1; i <= num; i++)
		{
			int num2 = Grid.OffsetCell(root_cell, 0, -i);
			if (!Grid.IsValidCell(num2) || Grid.Solid[num2] || (Grid.ObjectLayers[1].ContainsKey(num2) && !(Grid.ObjectLayers[1][num2] == null) && !(Grid.ObjectLayers[1][num2] == pump)))
			{
				break;
			}
			result = i;
		}
		return result;
	}

	// Token: 0x040038F3 RID: 14579
	private int previousDepthAvailable = -1;

	// Token: 0x040038F4 RID: 14580
	public GameObject parent;

	// Token: 0x040038F5 RID: 14581
	public bool occupyTiles;

	// Token: 0x040038F6 RID: 14582
	private KBatchedAnimController parentController;

	// Token: 0x040038F7 RID: 14583
	private KBatchedAnimController guideController;
}
