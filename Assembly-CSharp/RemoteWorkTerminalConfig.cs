using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x0200037C RID: 892
public class RemoteWorkTerminalConfig : IBuildingConfig
{
	// Token: 0x0600127A RID: 4730 RVA: 0x0006558C File Offset: 0x0006378C
	public override BuildingDef CreateBuildingDef()
	{
		string id = RemoteWorkTerminalConfig.ID;
		int width = 3;
		int height = 2;
		string anim = "remote_work_terminal_kanim";
		int hitpoints = 30;
		float construction_time = 60f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER2, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		return buildingDef;
	}

	// Token: 0x0600127B RID: 4731 RVA: 0x000655FC File Offset: 0x000637FC
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddComponent<RemoteWorkTerminal>().workTime = float.PositiveInfinity;
		go.AddComponent<RemoteWorkTerminalSM>();
		go.AddOrGet<Operational>();
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 100f;
		storage.showInUI = true;
		storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Insulate
		});
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = RemoteWorkTerminalConfig.INPUT_MATERIAL;
		manualDeliveryKG.refillMass = 5f;
		manualDeliveryKG.capacity = 10f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.ResearchFetch.IdHash;
		manualDeliveryKG.operationalRequirement = Operational.State.Functional;
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(RemoteWorkTerminalConfig.INPUT_MATERIAL, 0.006666667f, true)
		};
		elementConverter.showDescriptors = false;
		go.AddOrGet<ElementConverterOperationalRequirement>();
		Prioritizable.AddRef(go);
	}

	// Token: 0x0600127C RID: 4732 RVA: 0x000656E5 File Offset: 0x000638E5
	public override string[] GetRequiredDlcIds()
	{
		return new string[]
		{
			"DLC3_ID"
		};
	}

	// Token: 0x04000AB3 RID: 2739
	public static string ID = "RemoteWorkTerminal";

	// Token: 0x04000AB4 RID: 2740
	public static readonly Tag INPUT_MATERIAL = new Tag("OrbitalResearchDatabank");

	// Token: 0x04000AB5 RID: 2741
	public const float INPUT_CAPACITY = 10f;

	// Token: 0x04000AB6 RID: 2742
	public const float INPUT_CONSUMPTION_RATE_PER_S = 0.006666667f;

	// Token: 0x04000AB7 RID: 2743
	public const float INPUT_REFILL_RATIO = 0.5f;
}
