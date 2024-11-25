using System;
using UnityEngine;

// Token: 0x0200064E RID: 1614
public class ArtifactModule : SingleEntityReceptacle, IRenderEveryTick
{
	// Token: 0x0600277A RID: 10106 RVA: 0x000E0CDC File Offset: 0x000DEEDC
	protected override void OnSpawn()
	{
		this.craft = this.module.CraftInterface.GetComponent<Clustercraft>();
		if (this.craft.Status == Clustercraft.CraftStatus.InFlight && base.occupyingObject != null)
		{
			base.occupyingObject.SetActive(false);
		}
		base.OnSpawn();
		base.Subscribe(705820818, new Action<object>(this.OnEnterSpace));
		base.Subscribe(-1165815793, new Action<object>(this.OnExitSpace));
	}

	// Token: 0x0600277B RID: 10107 RVA: 0x000E0D5D File Offset: 0x000DEF5D
	public void RenderEveryTick(float dt)
	{
		this.ArtifactTrackModulePosition();
	}

	// Token: 0x0600277C RID: 10108 RVA: 0x000E0D68 File Offset: 0x000DEF68
	private void ArtifactTrackModulePosition()
	{
		this.occupyingObjectRelativePosition = this.animController.Offset + Vector3.up * 0.5f + new Vector3(0f, 0f, -1f);
		if (base.occupyingObject != null)
		{
			this.PositionOccupyingObject();
		}
	}

	// Token: 0x0600277D RID: 10109 RVA: 0x000E0DC7 File Offset: 0x000DEFC7
	private void OnEnterSpace(object data)
	{
		if (base.occupyingObject != null)
		{
			base.occupyingObject.SetActive(false);
		}
	}

	// Token: 0x0600277E RID: 10110 RVA: 0x000E0DE3 File Offset: 0x000DEFE3
	private void OnExitSpace(object data)
	{
		if (base.occupyingObject != null)
		{
			base.occupyingObject.SetActive(true);
		}
	}

	// Token: 0x040016C3 RID: 5827
	[MyCmpReq]
	private KBatchedAnimController animController;

	// Token: 0x040016C4 RID: 5828
	[MyCmpReq]
	private RocketModuleCluster module;

	// Token: 0x040016C5 RID: 5829
	private Clustercraft craft;
}
