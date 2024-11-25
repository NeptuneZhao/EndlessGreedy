using System;
using UnityEngine;

// Token: 0x02000679 RID: 1657
public class BuildingUnderConstruction : Building
{
	// Token: 0x06002905 RID: 10501 RVA: 0x000E8084 File Offset: 0x000E6284
	protected override void OnPrefabInit()
	{
		Vector3 position = base.transform.GetPosition();
		position.z = Grid.GetLayerZ(this.Def.SceneLayer);
		base.transform.SetPosition(position);
		base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Construction"));
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		Rotatable component2 = base.GetComponent<Rotatable>();
		if (component != null && component2 == null)
		{
			component.Offset = this.Def.GetVisualizerOffset();
		}
		KBoxCollider2D component3 = base.GetComponent<KBoxCollider2D>();
		if (component3 != null)
		{
			Vector3 visualizerOffset = this.Def.GetVisualizerOffset();
			component3.offset += new Vector2(visualizerOffset.x, visualizerOffset.y);
		}
		base.OnPrefabInit();
	}

	// Token: 0x06002906 RID: 10502 RVA: 0x000E8150 File Offset: 0x000E6350
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.Def.IsTilePiece)
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			this.Def.RunOnArea(cell, base.Orientation, delegate(int c)
			{
				TileVisualizer.RefreshCell(c, this.Def.TileLayer, this.Def.ReplacementLayer);
			});
		}
		base.RegisterBlockTileRenderer();
	}

	// Token: 0x06002907 RID: 10503 RVA: 0x000E81A5 File Offset: 0x000E63A5
	protected override void OnCleanUp()
	{
		base.UnregisterBlockTileRenderer();
		base.OnCleanUp();
	}

	// Token: 0x0400178B RID: 6027
	[MyCmpAdd]
	private KSelectable selectable;

	// Token: 0x0400178C RID: 6028
	[MyCmpAdd]
	private SaveLoadRoot saveLoadRoot;

	// Token: 0x0400178D RID: 6029
	[MyCmpAdd]
	private KPrefabID kPrefabID;
}
