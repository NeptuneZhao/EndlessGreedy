using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000060 RID: 96
public class CritterCondoConfig : IBuildingConfig
{
	// Token: 0x060001C4 RID: 452 RVA: 0x0000CD1C File Offset: 0x0000AF1C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "CritterCondo";
		int width = 3;
		int height = 3;
		string anim = "critter_condo_kanim";
		int hitpoints = 100;
		float construction_time = 120f;
		float[] construction_mass = new float[]
		{
			200f,
			10f
		};
		string[] construction_materials = new string[]
		{
			"BuildableRaw",
			"BuildingFiber"
		};
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER3, none, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		return buildingDef;
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x0000CD96 File Offset: 0x0000AF96
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x060001C6 RID: 454 RVA: 0x0000CD98 File Offset: 0x0000AF98
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
	}

	// Token: 0x060001C7 RID: 455 RVA: 0x0000CD9C File Offset: 0x0000AF9C
	public override void DoPostConfigureComplete(GameObject go)
	{
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.CreaturePen.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
		Effect effect = new Effect("InteractedWithCritterCondo", STRINGS.CREATURES.MODIFIERS.CRITTERCONDOINTERACTEFFECT.NAME, STRINGS.CREATURES.MODIFIERS.CRITTERCONDOINTERACTEFFECT.TOOLTIP, 600f, true, true, false, null, -1f, 0f, null, "");
		effect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 1f, STRINGS.CREATURES.MODIFIERS.CRITTERCONDOINTERACTEFFECT.NAME, false, false, true));
		Db.Get().effects.Add(effect);
		CritterCondo.Def def = go.AddOrGetDef<CritterCondo.Def>();
		def.IsCritterCondoOperationalCb = ((CritterCondo.Instance condo_smi) => condo_smi.GetComponent<RoomTracker>().IsInCorrectRoom() && !condo_smi.GetComponent<Floodable>().IsFlooded);
		def.moveToStatusItem = new StatusItem("CRITTERCONDO.MOVINGTO", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
		def.interactStatusItem = new StatusItem("CRITTERCONDO.INTERACTING", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
		def.effectId = effect.Id;
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RanchStationType, false);
	}

	// Token: 0x060001C8 RID: 456 RVA: 0x0000CEE2 File Offset: 0x0000B0E2
	public override void ConfigurePost(BuildingDef def)
	{
	}

	// Token: 0x0400011A RID: 282
	public const string ID = "CritterCondo";

	// Token: 0x0400011B RID: 283
	public const float EFFECT_DURATION_IN_SECONDS = 600f;
}
