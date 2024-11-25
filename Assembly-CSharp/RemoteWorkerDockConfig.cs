using System;
using TUNING;
using UnityEngine;

// Token: 0x0200037D RID: 893
public class RemoteWorkerDockConfig : IBuildingConfig
{
	// Token: 0x0600127F RID: 4735 RVA: 0x00065718 File Offset: 0x00063918
	public override BuildingDef CreateBuildingDef()
	{
		string id = RemoteWorkerDockConfig.ID;
		int width = 1;
		int height = 2;
		string anim = "remote_work_dock_kanim";
		int hitpoints = 100;
		float construction_time = 60f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] plastics = MATERIALS.PLASTICS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, plastics, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Plastic";
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.UtilityInputOffset = new CellOffset(0, 1);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		return buildingDef;
	}

	// Token: 0x06001280 RID: 4736 RVA: 0x000657D8 File Offset: 0x000639D8
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		this.AddVisualizer(go);
	}

	// Token: 0x06001281 RID: 4737 RVA: 0x000657E9 File Offset: 0x000639E9
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		this.AddVisualizer(go);
	}

	// Token: 0x06001282 RID: 4738 RVA: 0x000657F4 File Offset: 0x000639F4
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<RemoteWorkerDock>();
		go.AddOrGet<RemoteWorkerDockAnimSM>();
		go.AddOrGet<Storage>();
		go.AddOrGet<Operational>();
		go.AddOrGet<UserNameable>();
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.capacityTag = GameTags.LubricatingOil;
		conduitConsumer.capacityKG = 50f;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.elementFilter = new SimHashes[]
		{
			SimHashes.LiquidGunk
		};
		this.AddVisualizer(go);
		go.AddOrGet<RangeVisualizer>();
	}

	// Token: 0x06001283 RID: 4739 RVA: 0x0006587B File Offset: 0x00063A7B
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x06001284 RID: 4740 RVA: 0x00065884 File Offset: 0x00063A84
	private void AddVisualizer(GameObject prefab)
	{
		RangeVisualizer rangeVisualizer = prefab.AddOrGet<RangeVisualizer>();
		rangeVisualizer.RangeMin.x = -12;
		rangeVisualizer.RangeMin.y = 0;
		rangeVisualizer.RangeMax.x = 12;
		rangeVisualizer.RangeMax.y = 0;
		rangeVisualizer.OriginOffset = default(Vector2I);
		rangeVisualizer.BlockingTileVisible = false;
		prefab.GetComponent<KPrefabID>().instantiateFn += delegate(GameObject go)
		{
			go.GetComponent<RangeVisualizer>().BlockingCb = new Func<int, bool>(RemoteWorkerDockConfig.DockPathBlockingCB);
		};
	}

	// Token: 0x06001285 RID: 4741 RVA: 0x00065908 File Offset: 0x00063B08
	public static bool DockPathBlockingCB(int cell)
	{
		int num = Grid.CellAbove(cell);
		int num2 = Grid.CellBelow(cell);
		return num == Grid.InvalidCell || num2 == Grid.InvalidCell || (!Grid.Foundation[num2] && !Grid.Solid[num2]) || (Grid.Solid[cell] || Grid.Solid[num]);
	}

	// Token: 0x04000AB8 RID: 2744
	public static string ID = "RemoteWorkerDock";

	// Token: 0x04000AB9 RID: 2745
	public const float NEW_WORKER_DELAY_SECONDS = 2f;

	// Token: 0x04000ABA RID: 2746
	public const int WORK_RANGE = 12;

	// Token: 0x04000ABB RID: 2747
	public const float LUBRICANT_CAPACITY_KG = 50f;

	// Token: 0x04000ABC RID: 2748
	public const string ON_EMPTY_ANIM = "on_empty";

	// Token: 0x04000ABD RID: 2749
	public const string ON_FULL_ANIM = "on_full";

	// Token: 0x04000ABE RID: 2750
	public const string OFF_EMPTY_ANIM = "off_empty";

	// Token: 0x04000ABF RID: 2751
	public const string OFF_FULL_ANIM = "off_full";

	// Token: 0x04000AC0 RID: 2752
	public const string NEW_WORKER_ANIM = "new_worker";
}
