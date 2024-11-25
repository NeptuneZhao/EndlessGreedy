using System;
using UnityEngine;

// Token: 0x02000D4A RID: 3402
public class AutoPlumberSideScreen : SideScreenContent
{
	// Token: 0x06006B0C RID: 27404 RVA: 0x00285160 File Offset: 0x00283360
	protected override void OnSpawn()
	{
		this.activateButton.onClick += delegate()
		{
			DevAutoPlumber.AutoPlumbBuilding(this.building);
		};
		this.powerButton.onClick += delegate()
		{
			DevAutoPlumber.DoElectricalPlumbing(this.building);
		};
		this.pipesButton.onClick += delegate()
		{
			DevAutoPlumber.DoLiquidAndGasPlumbing(this.building);
		};
		this.solidsButton.onClick += delegate()
		{
			DevAutoPlumber.SetupSolidOreDelivery(this.building);
		};
		this.minionButton.onClick += delegate()
		{
			this.SpawnMinion();
		};
	}

	// Token: 0x06006B0D RID: 27405 RVA: 0x002851E0 File Offset: 0x002833E0
	private void SpawnMinion()
	{
		MinionStartingStats minionStartingStats = new MinionStartingStats(false, null, null, true);
		GameObject prefab = Assets.GetPrefab(BaseMinionConfig.GetMinionIDForModel(minionStartingStats.personality.model));
		GameObject gameObject = Util.KInstantiate(prefab, null, null);
		gameObject.name = prefab.name;
		Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
		Vector3 position = Grid.CellToPos(Grid.PosToCell(this.building), CellAlignment.Bottom, Grid.SceneLayer.Move);
		gameObject.transform.SetLocalPosition(position);
		gameObject.SetActive(true);
		minionStartingStats.Apply(gameObject);
	}

	// Token: 0x06006B0E RID: 27406 RVA: 0x0028525F File Offset: 0x0028345F
	public override int GetSideScreenSortOrder()
	{
		return -150;
	}

	// Token: 0x06006B0F RID: 27407 RVA: 0x00285266 File Offset: 0x00283466
	public override bool IsValidForTarget(GameObject target)
	{
		return DebugHandler.InstantBuildMode && target.GetComponent<Building>() != null;
	}

	// Token: 0x06006B10 RID: 27408 RVA: 0x0028527D File Offset: 0x0028347D
	public override void SetTarget(GameObject target)
	{
		this.building = target.GetComponent<Building>();
	}

	// Token: 0x06006B11 RID: 27409 RVA: 0x0028528B File Offset: 0x0028348B
	public override void ClearTarget()
	{
	}

	// Token: 0x040048FB RID: 18683
	public KButton activateButton;

	// Token: 0x040048FC RID: 18684
	public KButton powerButton;

	// Token: 0x040048FD RID: 18685
	public KButton pipesButton;

	// Token: 0x040048FE RID: 18686
	public KButton solidsButton;

	// Token: 0x040048FF RID: 18687
	public KButton minionButton;

	// Token: 0x04004900 RID: 18688
	private Building building;
}
