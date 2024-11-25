using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003EE RID: 1006
public class UnderwaterCritterCondoConfig : IBuildingConfig
{
	// Token: 0x06001508 RID: 5384 RVA: 0x00073DAC File Offset: 0x00071FAC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "UnderwaterCritterCondo";
		int width = 3;
		int height = 3;
		string anim = "underwater_critter_condo_kanim";
		int hitpoints = 100;
		float construction_time = 120f;
		float[] construction_mass = new float[]
		{
			200f
		};
		string[] plastics = MATERIALS.PLASTICS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, plastics, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER3, none, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.Floodable = false;
		return buildingDef;
	}

	// Token: 0x06001509 RID: 5385 RVA: 0x00073E14 File Offset: 0x00072014
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x0600150A RID: 5386 RVA: 0x00073E16 File Offset: 0x00072016
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
	}

	// Token: 0x0600150B RID: 5387 RVA: 0x00073E18 File Offset: 0x00072018
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<Submergable>();
		Effect effect = new Effect("InteractedWithUnderwaterCondo", STRINGS.CREATURES.MODIFIERS.CRITTERCONDOINTERACTEFFECT.NAME, STRINGS.CREATURES.MODIFIERS.UNDERWATERCRITTERCONDOINTERACTEFFECT.TOOLTIP, 600f, true, true, false, null, -1f, 0f, null, "");
		effect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 1f, STRINGS.CREATURES.MODIFIERS.CRITTERCONDOINTERACTEFFECT.NAME, false, false, true));
		Db.Get().effects.Add(effect);
		CritterCondo.Def def = go.AddOrGetDef<CritterCondo.Def>();
		def.IsCritterCondoOperationalCb = delegate(CritterCondo.Instance condo_smi)
		{
			Building component = condo_smi.GetComponent<Building>();
			for (int i = 0; i < component.PlacementCells.Length; i++)
			{
				if (!Grid.IsLiquid(component.PlacementCells[i]))
				{
					return false;
				}
			}
			return true;
		};
		def.moveToStatusItem = new StatusItem("UNDERWATERCRITTERCONDO.MOVINGTO", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
		def.interactStatusItem = new StatusItem("UNDERWATERCRITTERCONDO.INTERACTING", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
		def.condoTag = "UnderwaterCritterCondo";
		def.effectId = effect.Id;
	}

	// Token: 0x0600150C RID: 5388 RVA: 0x00073F3E File Offset: 0x0007213E
	public override void ConfigurePost(BuildingDef def)
	{
	}

	// Token: 0x04000BF7 RID: 3063
	public const string ID = "UnderwaterCritterCondo";

	// Token: 0x04000BF8 RID: 3064
	public static readonly Operational.Flag Submerged = new Operational.Flag("Submerged", Operational.Flag.Type.Requirement);
}
