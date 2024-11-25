using System;
using System.Collections.Generic;
using System.Diagnostics;
using Klei.AI;
using STRINGS;

// Token: 0x02000874 RID: 2164
[DebuggerDisplay("{name}")]
[Serializable]
public class Element : IComparable<Element>
{
	// Token: 0x06003C47 RID: 15431 RVA: 0x0014E5A5 File Offset: 0x0014C7A5
	public float PressureToMass(float pressure)
	{
		return pressure / this.defaultValues.pressure;
	}

	// Token: 0x1700044B RID: 1099
	// (get) Token: 0x06003C48 RID: 15432 RVA: 0x0014E5B4 File Offset: 0x0014C7B4
	public bool IsSlippery
	{
		get
		{
			return this.HasTag(GameTags.Slippery);
		}
	}

	// Token: 0x1700044C RID: 1100
	// (get) Token: 0x06003C49 RID: 15433 RVA: 0x0014E5C1 File Offset: 0x0014C7C1
	public bool IsUnstable
	{
		get
		{
			return this.HasTag(GameTags.Unstable);
		}
	}

	// Token: 0x1700044D RID: 1101
	// (get) Token: 0x06003C4A RID: 15434 RVA: 0x0014E5CE File Offset: 0x0014C7CE
	public bool IsLiquid
	{
		get
		{
			return (this.state & Element.State.Solid) == Element.State.Liquid;
		}
	}

	// Token: 0x1700044E RID: 1102
	// (get) Token: 0x06003C4B RID: 15435 RVA: 0x0014E5DB File Offset: 0x0014C7DB
	public bool IsGas
	{
		get
		{
			return (this.state & Element.State.Solid) == Element.State.Gas;
		}
	}

	// Token: 0x1700044F RID: 1103
	// (get) Token: 0x06003C4C RID: 15436 RVA: 0x0014E5E8 File Offset: 0x0014C7E8
	public bool IsSolid
	{
		get
		{
			return (this.state & Element.State.Solid) == Element.State.Solid;
		}
	}

	// Token: 0x17000450 RID: 1104
	// (get) Token: 0x06003C4D RID: 15437 RVA: 0x0014E5F5 File Offset: 0x0014C7F5
	public bool IsVacuum
	{
		get
		{
			return (this.state & Element.State.Solid) == Element.State.Vacuum;
		}
	}

	// Token: 0x17000451 RID: 1105
	// (get) Token: 0x06003C4E RID: 15438 RVA: 0x0014E602 File Offset: 0x0014C802
	public bool IsTemperatureInsulated
	{
		get
		{
			return (this.state & Element.State.TemperatureInsulated) > Element.State.Vacuum;
		}
	}

	// Token: 0x06003C4F RID: 15439 RVA: 0x0014E610 File Offset: 0x0014C810
	public bool IsState(Element.State expected_state)
	{
		return (this.state & Element.State.Solid) == expected_state;
	}

	// Token: 0x17000452 RID: 1106
	// (get) Token: 0x06003C50 RID: 15440 RVA: 0x0014E61D File Offset: 0x0014C81D
	public bool HasTransitionUp
	{
		get
		{
			return this.highTempTransitionTarget != (SimHashes)0 && this.highTempTransitionTarget != SimHashes.Unobtanium && this.highTempTransition != null && this.highTempTransition != this;
		}
	}

	// Token: 0x17000453 RID: 1107
	// (get) Token: 0x06003C51 RID: 15441 RVA: 0x0014E64A File Offset: 0x0014C84A
	// (set) Token: 0x06003C52 RID: 15442 RVA: 0x0014E652 File Offset: 0x0014C852
	public string name { get; set; }

	// Token: 0x17000454 RID: 1108
	// (get) Token: 0x06003C53 RID: 15443 RVA: 0x0014E65B File Offset: 0x0014C85B
	// (set) Token: 0x06003C54 RID: 15444 RVA: 0x0014E663 File Offset: 0x0014C863
	public string nameUpperCase { get; set; }

	// Token: 0x17000455 RID: 1109
	// (get) Token: 0x06003C55 RID: 15445 RVA: 0x0014E66C File Offset: 0x0014C86C
	// (set) Token: 0x06003C56 RID: 15446 RVA: 0x0014E674 File Offset: 0x0014C874
	public string description { get; set; }

	// Token: 0x06003C57 RID: 15447 RVA: 0x0014E67D File Offset: 0x0014C87D
	public string GetStateString()
	{
		return Element.GetStateString(this.state);
	}

	// Token: 0x06003C58 RID: 15448 RVA: 0x0014E68A File Offset: 0x0014C88A
	public static string GetStateString(Element.State state)
	{
		if ((state & Element.State.Solid) == Element.State.Solid)
		{
			return ELEMENTS.STATE.SOLID;
		}
		if ((state & Element.State.Solid) == Element.State.Liquid)
		{
			return ELEMENTS.STATE.LIQUID;
		}
		if ((state & Element.State.Solid) == Element.State.Gas)
		{
			return ELEMENTS.STATE.GAS;
		}
		return ELEMENTS.STATE.VACUUM;
	}

	// Token: 0x06003C59 RID: 15449 RVA: 0x0014E6CC File Offset: 0x0014C8CC
	public string FullDescription(bool addHardnessColor = true)
	{
		string text = this.Description();
		if (this.IsSolid)
		{
			text += "\n\n";
			text += string.Format(ELEMENTS.ELEMENTDESCSOLID, this.GetMaterialCategoryTag().ProperName(), GameUtil.GetFormattedTemperature(this.highTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), GameUtil.GetHardnessString(this, addHardnessColor));
		}
		else if (this.IsLiquid)
		{
			text += "\n\n";
			text += string.Format(ELEMENTS.ELEMENTDESCLIQUID, this.GetMaterialCategoryTag().ProperName(), GameUtil.GetFormattedTemperature(this.lowTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), GameUtil.GetFormattedTemperature(this.highTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
		}
		else if (!this.IsVacuum)
		{
			text += "\n\n";
			text += string.Format(ELEMENTS.ELEMENTDESCGAS, this.GetMaterialCategoryTag().ProperName(), GameUtil.GetFormattedTemperature(this.lowTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
		}
		string text2 = ELEMENTS.THERMALPROPERTIES;
		text2 = text2.Replace("{SPECIFIC_HEAT_CAPACITY}", GameUtil.GetFormattedSHC(this.specificHeatCapacity));
		text2 = text2.Replace("{THERMAL_CONDUCTIVITY}", GameUtil.GetFormattedThermalConductivity(this.thermalConductivity));
		text = text + "\n" + text2;
		if (DlcManager.FeatureRadiationEnabled())
		{
			text = text + "\n" + string.Format(ELEMENTS.RADIATIONPROPERTIES, this.radiationAbsorptionFactor, GameUtil.GetFormattedRads(this.radiationPer1000Mass * 1.1f / 600f, GameUtil.TimeSlice.PerCycle));
		}
		if (this.oreTags.Length != 0 && !this.IsVacuum)
		{
			text += "\n\n";
			string text3 = "";
			for (int i = 0; i < this.oreTags.Length; i++)
			{
				Tag a = new Tag(this.oreTags[i]);
				if (!(a == GameTags.HideFromCodex) && !(a == GameTags.HideFromSpawnTool))
				{
					text3 += a.ProperName();
					if (i < this.oreTags.Length - 1)
					{
						text3 += ", ";
					}
				}
			}
			text += string.Format(ELEMENTS.ELEMENTPROPERTIES, text3);
		}
		if (this.attributeModifiers.Count > 0)
		{
			foreach (AttributeModifier attributeModifier in this.attributeModifiers)
			{
				string name = Db.Get().BuildingAttributes.Get(attributeModifier.AttributeId).Name;
				string formattedString = attributeModifier.GetFormattedString();
				text = text + "\n" + string.Format(DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, name, formattedString);
			}
		}
		return text;
	}

	// Token: 0x06003C5A RID: 15450 RVA: 0x0014E994 File Offset: 0x0014CB94
	public string Description()
	{
		return this.description;
	}

	// Token: 0x06003C5B RID: 15451 RVA: 0x0014E99C File Offset: 0x0014CB9C
	public bool HasTag(Tag search_tag)
	{
		return this.tag == search_tag || Array.IndexOf<Tag>(this.oreTags, search_tag) != -1;
	}

	// Token: 0x06003C5C RID: 15452 RVA: 0x0014E9C0 File Offset: 0x0014CBC0
	public Tag GetMaterialCategoryTag()
	{
		return this.materialCategory;
	}

	// Token: 0x06003C5D RID: 15453 RVA: 0x0014E9C8 File Offset: 0x0014CBC8
	public int CompareTo(Element other)
	{
		return this.id - other.id;
	}

	// Token: 0x040024A6 RID: 9382
	public const int INVALID_ID = 0;

	// Token: 0x040024A7 RID: 9383
	public SimHashes id;

	// Token: 0x040024A8 RID: 9384
	public Tag tag;

	// Token: 0x040024A9 RID: 9385
	public ushort idx;

	// Token: 0x040024AA RID: 9386
	public float specificHeatCapacity;

	// Token: 0x040024AB RID: 9387
	public float thermalConductivity = 1f;

	// Token: 0x040024AC RID: 9388
	public float molarMass = 1f;

	// Token: 0x040024AD RID: 9389
	public float strength;

	// Token: 0x040024AE RID: 9390
	public float flow;

	// Token: 0x040024AF RID: 9391
	public float maxCompression;

	// Token: 0x040024B0 RID: 9392
	public float viscosity;

	// Token: 0x040024B1 RID: 9393
	public float minHorizontalFlow = float.PositiveInfinity;

	// Token: 0x040024B2 RID: 9394
	public float minVerticalFlow = float.PositiveInfinity;

	// Token: 0x040024B3 RID: 9395
	public float maxMass = 10000f;

	// Token: 0x040024B4 RID: 9396
	public float solidSurfaceAreaMultiplier;

	// Token: 0x040024B5 RID: 9397
	public float liquidSurfaceAreaMultiplier;

	// Token: 0x040024B6 RID: 9398
	public float gasSurfaceAreaMultiplier;

	// Token: 0x040024B7 RID: 9399
	public Element.State state;

	// Token: 0x040024B8 RID: 9400
	public byte hardness;

	// Token: 0x040024B9 RID: 9401
	public float lowTemp;

	// Token: 0x040024BA RID: 9402
	public SimHashes lowTempTransitionTarget;

	// Token: 0x040024BB RID: 9403
	public Element lowTempTransition;

	// Token: 0x040024BC RID: 9404
	public float highTemp;

	// Token: 0x040024BD RID: 9405
	public SimHashes highTempTransitionTarget;

	// Token: 0x040024BE RID: 9406
	public Element highTempTransition;

	// Token: 0x040024BF RID: 9407
	public SimHashes highTempTransitionOreID = SimHashes.Vacuum;

	// Token: 0x040024C0 RID: 9408
	public float highTempTransitionOreMassConversion;

	// Token: 0x040024C1 RID: 9409
	public SimHashes lowTempTransitionOreID = SimHashes.Vacuum;

	// Token: 0x040024C2 RID: 9410
	public float lowTempTransitionOreMassConversion;

	// Token: 0x040024C3 RID: 9411
	public SimHashes sublimateId;

	// Token: 0x040024C4 RID: 9412
	public SimHashes convertId;

	// Token: 0x040024C5 RID: 9413
	public SpawnFXHashes sublimateFX;

	// Token: 0x040024C6 RID: 9414
	public float sublimateRate;

	// Token: 0x040024C7 RID: 9415
	public float sublimateEfficiency;

	// Token: 0x040024C8 RID: 9416
	public float sublimateProbability;

	// Token: 0x040024C9 RID: 9417
	public float offGasPercentage;

	// Token: 0x040024CA RID: 9418
	public float lightAbsorptionFactor;

	// Token: 0x040024CB RID: 9419
	public float radiationAbsorptionFactor;

	// Token: 0x040024CC RID: 9420
	public float radiationPer1000Mass;

	// Token: 0x040024CD RID: 9421
	public Sim.PhysicsData defaultValues;

	// Token: 0x040024CE RID: 9422
	public float toxicity;

	// Token: 0x040024CF RID: 9423
	public Substance substance;

	// Token: 0x040024D0 RID: 9424
	public Tag materialCategory;

	// Token: 0x040024D1 RID: 9425
	public int buildMenuSort;

	// Token: 0x040024D2 RID: 9426
	public ElementLoader.ElementComposition[] elementComposition;

	// Token: 0x040024D3 RID: 9427
	public Tag[] oreTags = new Tag[0];

	// Token: 0x040024D4 RID: 9428
	public List<AttributeModifier> attributeModifiers = new List<AttributeModifier>();

	// Token: 0x040024D5 RID: 9429
	public bool disabled;

	// Token: 0x040024D6 RID: 9430
	public string dlcId;

	// Token: 0x040024D7 RID: 9431
	public const byte StateMask = 3;

	// Token: 0x02001779 RID: 6009
	[Serializable]
	public enum State : byte
	{
		// Token: 0x040072D4 RID: 29396
		Vacuum,
		// Token: 0x040072D5 RID: 29397
		Gas,
		// Token: 0x040072D6 RID: 29398
		Liquid,
		// Token: 0x040072D7 RID: 29399
		Solid,
		// Token: 0x040072D8 RID: 29400
		Unbreakable,
		// Token: 0x040072D9 RID: 29401
		Unstable = 8,
		// Token: 0x040072DA RID: 29402
		TemperatureInsulated = 16
	}
}
