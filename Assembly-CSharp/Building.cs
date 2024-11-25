using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Database;
using STRINGS;
using UnityEngine;

// Token: 0x0200066A RID: 1642
[AddComponentMenu("KMonoBehaviour/scripts/Building")]
public class Building : KMonoBehaviour, IGameObjectEffectDescriptor, IUniformGridObject, IApproachable
{
	// Token: 0x170001F6 RID: 502
	// (get) Token: 0x06002881 RID: 10369 RVA: 0x000E56E3 File Offset: 0x000E38E3
	public Orientation Orientation
	{
		get
		{
			if (!(this.rotatable != null))
			{
				return Orientation.Neutral;
			}
			return this.rotatable.GetOrientation();
		}
	}

	// Token: 0x170001F7 RID: 503
	// (get) Token: 0x06002882 RID: 10370 RVA: 0x000E5700 File Offset: 0x000E3900
	public int[] PlacementCells
	{
		get
		{
			if (this.placementCells == null)
			{
				this.RefreshCells();
			}
			return this.placementCells;
		}
	}

	// Token: 0x06002883 RID: 10371 RVA: 0x000E5716 File Offset: 0x000E3916
	public Extents GetExtents()
	{
		if (this.extents.width == 0 || this.extents.height == 0)
		{
			this.RefreshCells();
		}
		return this.extents;
	}

	// Token: 0x06002884 RID: 10372 RVA: 0x000E5740 File Offset: 0x000E3940
	public Extents GetValidPlacementExtents()
	{
		Extents result = this.GetExtents();
		result.x--;
		result.y--;
		result.width += 2;
		result.height += 2;
		return result;
	}

	// Token: 0x06002885 RID: 10373 RVA: 0x000E5788 File Offset: 0x000E3988
	public bool PlacementCellsContainCell(int cell)
	{
		for (int i = 0; i < this.PlacementCells.Length; i++)
		{
			if (this.PlacementCells[i] == cell)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002886 RID: 10374 RVA: 0x000E57B8 File Offset: 0x000E39B8
	public void RefreshCells()
	{
		this.placementCells = new int[this.Def.PlacementOffsets.Length];
		int num = Grid.PosToCell(this);
		if (num < 0)
		{
			this.extents.x = -1;
			this.extents.y = -1;
			this.extents.width = this.Def.WidthInCells;
			this.extents.height = this.Def.HeightInCells;
			return;
		}
		Orientation orientation = this.Orientation;
		for (int i = 0; i < this.Def.PlacementOffsets.Length; i++)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.Def.PlacementOffsets[i], orientation);
			int num2 = Grid.OffsetCell(num, rotatedCellOffset);
			this.placementCells[i] = num2;
		}
		int num3 = 0;
		int num4 = 0;
		Grid.CellToXY(this.placementCells[0], out num3, out num4);
		int num5 = num3;
		int num6 = num4;
		foreach (int cell in this.placementCells)
		{
			int val = 0;
			int val2 = 0;
			Grid.CellToXY(cell, out val, out val2);
			num3 = Math.Min(num3, val);
			num4 = Math.Min(num4, val2);
			num5 = Math.Max(num5, val);
			num6 = Math.Max(num6, val2);
		}
		this.extents.x = num3;
		this.extents.y = num4;
		this.extents.width = num5 - num3 + 1;
		this.extents.height = num6 - num4 + 1;
	}

	// Token: 0x06002887 RID: 10375 RVA: 0x000E5928 File Offset: 0x000E3B28
	[OnDeserialized]
	internal void OnDeserialized()
	{
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		if (component != null && component.Temperature == 0f)
		{
			if (component.Element == null)
			{
				DeserializeWarnings.Instance.PrimaryElementHasNoElement.Warn(base.name + " primary element has no element.", base.gameObject);
				return;
			}
			if (!(this is BuildingUnderConstruction))
			{
				DeserializeWarnings.Instance.BuildingTemeperatureIsZeroKelvin.Warn(base.name + " is at zero degrees kelvin. Resetting temperature.", null);
				component.Temperature = component.Element.defaultValues.temperature;
			}
		}
	}

	// Token: 0x06002888 RID: 10376 RVA: 0x000E59C0 File Offset: 0x000E3BC0
	public static void CreateBuildingMeltedNotification(GameObject building)
	{
		Vector3 pos = building.transform.GetPosition();
		Notifier notifier = building.AddOrGet<Notifier>();
		Notification notification = new Notification(MISC.NOTIFICATIONS.BUILDING_MELTED.NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.BUILDING_MELTED.TOOLTIP + notificationList.ReduceMessages(false), "/t• " + notifier.GetProperName(), true, 0f, delegate(object o)
		{
			GameUtil.FocusCamera(pos);
		}, null, null, true, true, false);
		notifier.Add(notification, "");
	}

	// Token: 0x06002889 RID: 10377 RVA: 0x000E5A4E File Offset: 0x000E3C4E
	public void SetDescription(string desc)
	{
		this.description = desc;
	}

	// Token: 0x170001F8 RID: 504
	// (get) Token: 0x0600288A RID: 10378 RVA: 0x000E5A57 File Offset: 0x000E3C57
	public string Desc
	{
		get
		{
			return this.Def.Desc;
		}
	}

	// Token: 0x170001F9 RID: 505
	// (get) Token: 0x0600288B RID: 10379 RVA: 0x000E5A64 File Offset: 0x000E3C64
	public string DescFlavour
	{
		get
		{
			return this.descriptionFlavour;
		}
	}

	// Token: 0x170001FA RID: 506
	// (get) Token: 0x0600288C RID: 10380 RVA: 0x000E5A6C File Offset: 0x000E3C6C
	public string DescEffect
	{
		get
		{
			return this.Def.Effect;
		}
	}

	// Token: 0x0600288D RID: 10381 RVA: 0x000E5A79 File Offset: 0x000E3C79
	public void SetDescriptionFlavour(string descriptionFlavour)
	{
		this.descriptionFlavour = descriptionFlavour;
	}

	// Token: 0x0600288E RID: 10382 RVA: 0x000E5A84 File Offset: 0x000E3C84
	protected override void OnSpawn()
	{
		if (this.Def == null)
		{
			global::Debug.LogError("Missing building definition on object " + base.name);
		}
		KSelectable component = base.GetComponent<KSelectable>();
		if (component != null)
		{
			component.SetName(this.Def.Name);
			component.SetStatusIndicatorOffset(new Vector3(0f, -0.35f, 0f));
		}
		Prioritizable component2 = base.GetComponent<Prioritizable>();
		if (component2 != null)
		{
			component2.iconOffset.y = 0.3f;
		}
		if (base.GetComponent<KPrefabID>().HasTag(RoomConstraints.ConstraintTags.IndustrialMachinery))
		{
			this.scenePartitionerEntry = GameScenePartitioner.Instance.Add(base.name, base.gameObject, this.GetExtents(), GameScenePartitioner.Instance.industrialBuildings, null);
		}
		if (this.Def.Deprecated && base.GetComponent<KSelectable>() != null)
		{
			KSelectable component3 = base.GetComponent<KSelectable>();
			Building.deprecatedBuildingStatusItem = new StatusItem("BUILDING_DEPRECATED", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			component3.AddStatusItem(Building.deprecatedBuildingStatusItem, null);
		}
	}

	// Token: 0x0600288F RID: 10383 RVA: 0x000E5BA3 File Offset: 0x000E3DA3
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.scenePartitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06002890 RID: 10384 RVA: 0x000E5BBB File Offset: 0x000E3DBB
	public virtual void UpdatePosition()
	{
		this.RefreshCells();
		GameScenePartitioner.Instance.UpdatePosition(this.scenePartitionerEntry, this.GetExtents());
	}

	// Token: 0x06002891 RID: 10385 RVA: 0x000E5BDC File Offset: 0x000E3DDC
	protected void RegisterBlockTileRenderer()
	{
		if (this.Def.BlockTileAtlas != null)
		{
			PrimaryElement component = base.GetComponent<PrimaryElement>();
			if (component != null)
			{
				SimHashes visualizationElementID = this.GetVisualizationElementID(component);
				int cell = Grid.PosToCell(base.transform.GetPosition());
				Constructable component2 = base.GetComponent<Constructable>();
				bool isReplacement = component2 != null && component2.IsReplacementTile;
				World.Instance.blockTileRenderer.AddBlock(base.gameObject.layer, this.Def, isReplacement, visualizationElementID, cell);
			}
		}
	}

	// Token: 0x06002892 RID: 10386 RVA: 0x000E5C64 File Offset: 0x000E3E64
	public CellOffset GetRotatedOffset(CellOffset offset)
	{
		if (!(this.rotatable != null))
		{
			return offset;
		}
		return this.rotatable.GetRotatedCellOffset(offset);
	}

	// Token: 0x06002893 RID: 10387 RVA: 0x000E5C82 File Offset: 0x000E3E82
	public int GetBottomLeftCell()
	{
		return Grid.PosToCell(base.transform.GetPosition());
	}

	// Token: 0x06002894 RID: 10388 RVA: 0x000E5C94 File Offset: 0x000E3E94
	public int GetPowerInputCell()
	{
		CellOffset rotatedOffset = this.GetRotatedOffset(this.Def.PowerInputOffset);
		return Grid.OffsetCell(this.GetBottomLeftCell(), rotatedOffset);
	}

	// Token: 0x06002895 RID: 10389 RVA: 0x000E5CC0 File Offset: 0x000E3EC0
	public int GetPowerOutputCell()
	{
		CellOffset rotatedOffset = this.GetRotatedOffset(this.Def.PowerOutputOffset);
		return Grid.OffsetCell(this.GetBottomLeftCell(), rotatedOffset);
	}

	// Token: 0x06002896 RID: 10390 RVA: 0x000E5CEC File Offset: 0x000E3EEC
	public int GetUtilityInputCell()
	{
		CellOffset rotatedOffset = this.GetRotatedOffset(this.Def.UtilityInputOffset);
		return Grid.OffsetCell(this.GetBottomLeftCell(), rotatedOffset);
	}

	// Token: 0x06002897 RID: 10391 RVA: 0x000E5D18 File Offset: 0x000E3F18
	public int GetHighEnergyParticleInputCell()
	{
		CellOffset rotatedOffset = this.GetRotatedOffset(this.Def.HighEnergyParticleInputOffset);
		return Grid.OffsetCell(this.GetBottomLeftCell(), rotatedOffset);
	}

	// Token: 0x06002898 RID: 10392 RVA: 0x000E5D44 File Offset: 0x000E3F44
	public int GetHighEnergyParticleOutputCell()
	{
		CellOffset rotatedOffset = this.GetRotatedOffset(this.Def.HighEnergyParticleOutputOffset);
		return Grid.OffsetCell(this.GetBottomLeftCell(), rotatedOffset);
	}

	// Token: 0x06002899 RID: 10393 RVA: 0x000E5D70 File Offset: 0x000E3F70
	public int GetUtilityOutputCell()
	{
		CellOffset rotatedOffset = this.GetRotatedOffset(this.Def.UtilityOutputOffset);
		return Grid.OffsetCell(this.GetBottomLeftCell(), rotatedOffset);
	}

	// Token: 0x0600289A RID: 10394 RVA: 0x000E5D9B File Offset: 0x000E3F9B
	public CellOffset GetUtilityInputOffset()
	{
		return this.GetRotatedOffset(this.Def.UtilityInputOffset);
	}

	// Token: 0x0600289B RID: 10395 RVA: 0x000E5DAE File Offset: 0x000E3FAE
	public CellOffset GetUtilityOutputOffset()
	{
		return this.GetRotatedOffset(this.Def.UtilityOutputOffset);
	}

	// Token: 0x0600289C RID: 10396 RVA: 0x000E5DC1 File Offset: 0x000E3FC1
	public CellOffset GetHighEnergyParticleInputOffset()
	{
		return this.GetRotatedOffset(this.Def.HighEnergyParticleInputOffset);
	}

	// Token: 0x0600289D RID: 10397 RVA: 0x000E5DD4 File Offset: 0x000E3FD4
	public CellOffset GetHighEnergyParticleOutputOffset()
	{
		return this.GetRotatedOffset(this.Def.HighEnergyParticleOutputOffset);
	}

	// Token: 0x0600289E RID: 10398 RVA: 0x000E5DE8 File Offset: 0x000E3FE8
	protected void UnregisterBlockTileRenderer()
	{
		if (this.Def.BlockTileAtlas != null)
		{
			PrimaryElement component = base.GetComponent<PrimaryElement>();
			if (component != null)
			{
				SimHashes visualizationElementID = this.GetVisualizationElementID(component);
				int cell = Grid.PosToCell(base.transform.GetPosition());
				Constructable component2 = base.GetComponent<Constructable>();
				bool isReplacement = component2 != null && component2.IsReplacementTile;
				World.Instance.blockTileRenderer.RemoveBlock(this.Def, isReplacement, visualizationElementID, cell);
			}
		}
	}

	// Token: 0x0600289F RID: 10399 RVA: 0x000E5E65 File Offset: 0x000E4065
	private SimHashes GetVisualizationElementID(PrimaryElement pe)
	{
		if (!(this is BuildingComplete))
		{
			return SimHashes.Void;
		}
		return pe.ElementID;
	}

	// Token: 0x060028A0 RID: 10400 RVA: 0x000E5E7B File Offset: 0x000E407B
	public void RunOnArea(Action<int> callback)
	{
		this.Def.RunOnArea(Grid.PosToCell(this), this.Orientation, callback);
	}

	// Token: 0x060028A1 RID: 10401 RVA: 0x000E5E98 File Offset: 0x000E4098
	public List<Descriptor> RequirementDescriptors(BuildingDef def)
	{
		List<Descriptor> list = new List<Descriptor>();
		BuildingComplete component = def.BuildingComplete.GetComponent<BuildingComplete>();
		if (def.RequiresPowerInput)
		{
			float wattsNeededWhenActive = component.GetComponent<IEnergyConsumer>().WattsNeededWhenActive;
			if (wattsNeededWhenActive > 0f)
			{
				string formattedWattage = GameUtil.GetFormattedWattage(wattsNeededWhenActive, GameUtil.WattageFormatterUnit.Automatic, true);
				Descriptor item = new Descriptor(string.Format(UI.BUILDINGEFFECTS.REQUIRESPOWER, formattedWattage), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESPOWER, formattedWattage), Descriptor.DescriptorType.Requirement, false);
				list.Add(item);
			}
		}
		if (def.InputConduitType == ConduitType.Liquid)
		{
			Descriptor item2 = default(Descriptor);
			item2.SetupDescriptor(UI.BUILDINGEFFECTS.REQUIRESLIQUIDINPUT, UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESLIQUIDINPUT, Descriptor.DescriptorType.Requirement);
			list.Add(item2);
		}
		else if (def.InputConduitType == ConduitType.Gas)
		{
			Descriptor item3 = default(Descriptor);
			item3.SetupDescriptor(UI.BUILDINGEFFECTS.REQUIRESGASINPUT, UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESGASINPUT, Descriptor.DescriptorType.Requirement);
			list.Add(item3);
		}
		if (def.OutputConduitType == ConduitType.Liquid)
		{
			Descriptor item4 = default(Descriptor);
			item4.SetupDescriptor(UI.BUILDINGEFFECTS.REQUIRESLIQUIDOUTPUT, UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESLIQUIDOUTPUT, Descriptor.DescriptorType.Requirement);
			list.Add(item4);
		}
		else if (def.OutputConduitType == ConduitType.Gas)
		{
			Descriptor item5 = default(Descriptor);
			item5.SetupDescriptor(UI.BUILDINGEFFECTS.REQUIRESGASOUTPUT, UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESGASOUTPUT, Descriptor.DescriptorType.Requirement);
			list.Add(item5);
		}
		if (component.isManuallyOperated)
		{
			Descriptor item6 = default(Descriptor);
			item6.SetupDescriptor(UI.BUILDINGEFFECTS.REQUIRESMANUALOPERATION, UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESMANUALOPERATION, Descriptor.DescriptorType.Requirement);
			list.Add(item6);
		}
		if (component.isArtable)
		{
			Descriptor item7 = default(Descriptor);
			item7.SetupDescriptor(UI.BUILDINGEFFECTS.REQUIRESCREATIVITY, UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESCREATIVITY, Descriptor.DescriptorType.Requirement);
			list.Add(item7);
		}
		if (def.BuildingUnderConstruction != null)
		{
			Constructable component2 = def.BuildingUnderConstruction.GetComponent<Constructable>();
			if (component2 != null && component2.requiredSkillPerk != HashedString.Invalid)
			{
				StringBuilder stringBuilder = new StringBuilder();
				List<Skill> skillsWithPerk = Db.Get().Skills.GetSkillsWithPerk(component2.requiredSkillPerk);
				for (int i = 0; i < skillsWithPerk.Count; i++)
				{
					Skill skill = skillsWithPerk[i];
					stringBuilder.Append(skill.Name);
					if (i != skillsWithPerk.Count - 1)
					{
						stringBuilder.Append(", ");
					}
				}
				string replacement = stringBuilder.ToString();
				list.Add(new Descriptor(UI.BUILD_REQUIRES_SKILL.Replace("{Skill}", replacement), UI.BUILD_REQUIRES_SKILL_TOOLTIP.Replace("{Skill}", replacement), Descriptor.DescriptorType.Requirement, false));
			}
		}
		return list;
	}

	// Token: 0x060028A2 RID: 10402 RVA: 0x000E6138 File Offset: 0x000E4338
	public List<Descriptor> EffectDescriptors(BuildingDef def)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (def.EffectDescription != null)
		{
			list.AddRange(def.EffectDescription);
		}
		if (def.GeneratorWattageRating > 0f && base.GetComponent<Battery>() == null)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ENERGYGENERATED, GameUtil.GetFormattedWattage(def.GeneratorWattageRating, GameUtil.WattageFormatterUnit.Automatic, true)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ENERGYGENERATED, GameUtil.GetFormattedWattage(def.GeneratorWattageRating, GameUtil.WattageFormatterUnit.Automatic, true)), Descriptor.DescriptorType.Effect);
			list.Add(item);
		}
		if (def.ExhaustKilowattsWhenActive > 0f || def.SelfHeatKilowattsWhenActive > 0f)
		{
			Descriptor item2 = default(Descriptor);
			string formattedHeatEnergy = GameUtil.GetFormattedHeatEnergy((def.ExhaustKilowattsWhenActive + def.SelfHeatKilowattsWhenActive) * 1000f, GameUtil.HeatEnergyFormatterUnit.Automatic);
			item2.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.HEATGENERATED, formattedHeatEnergy), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.HEATGENERATED, formattedHeatEnergy), Descriptor.DescriptorType.Effect);
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x060028A3 RID: 10403 RVA: 0x000E6238 File Offset: 0x000E4438
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		foreach (Descriptor item in this.RequirementDescriptors(this.Def))
		{
			list.Add(item);
		}
		foreach (Descriptor item2 in this.EffectDescriptors(this.Def))
		{
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x060028A4 RID: 10404 RVA: 0x000E62E0 File Offset: 0x000E44E0
	public override Vector2 PosMin()
	{
		Extents extents = this.GetExtents();
		return new Vector2((float)extents.x, (float)extents.y);
	}

	// Token: 0x060028A5 RID: 10405 RVA: 0x000E6308 File Offset: 0x000E4508
	public override Vector2 PosMax()
	{
		Extents extents = this.GetExtents();
		return new Vector2((float)(extents.x + extents.width), (float)(extents.y + extents.height));
	}

	// Token: 0x060028A6 RID: 10406 RVA: 0x000E633D File Offset: 0x000E453D
	public CellOffset[] GetOffsets()
	{
		return OffsetGroups.Use;
	}

	// Token: 0x060028A7 RID: 10407 RVA: 0x000E6344 File Offset: 0x000E4544
	public int GetCell()
	{
		return Grid.PosToCell(this);
	}

	// Token: 0x04001747 RID: 5959
	public BuildingDef Def;

	// Token: 0x04001748 RID: 5960
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04001749 RID: 5961
	[MyCmpAdd]
	private StateMachineController stateMachineController;

	// Token: 0x0400174A RID: 5962
	private int[] placementCells;

	// Token: 0x0400174B RID: 5963
	private Extents extents;

	// Token: 0x0400174C RID: 5964
	private static StatusItem deprecatedBuildingStatusItem;

	// Token: 0x0400174D RID: 5965
	private string description;

	// Token: 0x0400174E RID: 5966
	private string descriptionFlavour;

	// Token: 0x0400174F RID: 5967
	private HandleVector<int>.Handle scenePartitionerEntry;
}
