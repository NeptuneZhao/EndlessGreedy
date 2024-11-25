using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000A97 RID: 2711
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/SimCellOccupier")]
public class SimCellOccupier : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x170005B5 RID: 1461
	// (get) Token: 0x06004F73 RID: 20339 RVA: 0x001C8EF4 File Offset: 0x001C70F4
	public bool IsVisuallySolid
	{
		get
		{
			return this.doReplaceElement;
		}
	}

	// Token: 0x06004F74 RID: 20340 RVA: 0x001C8EFC File Offset: 0x001C70FC
	protected override void OnPrefabInit()
	{
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Normal, null);
		if (this.building.Def.IsFoundation)
		{
			this.setConstructedTile = true;
		}
	}

	// Token: 0x06004F75 RID: 20341 RVA: 0x001C8F50 File Offset: 0x001C7150
	protected override void OnSpawn()
	{
		HandleVector<Game.CallbackInfo>.Handle callbackHandle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(new System.Action(this.OnModifyComplete), false));
		int num = this.building.Def.PlacementOffsets.Length;
		float mass_per_cell = this.primaryElement.Mass / (float)num;
		this.building.RunOnArea(delegate(int offset_cell)
		{
			if (this.doReplaceElement)
			{
				SimMessages.ReplaceAndDisplaceElement(offset_cell, this.primaryElement.ElementID, CellEventLogger.Instance.SimCellOccupierOnSpawn, mass_per_cell, this.primaryElement.Temperature, this.primaryElement.DiseaseIdx, this.primaryElement.DiseaseCount, callbackHandle.index);
				callbackHandle = HandleVector<Game.CallbackInfo>.InvalidHandle;
				SimMessages.SetStrength(offset_cell, 0, this.strengthMultiplier);
				Game.Instance.RemoveSolidChangedFilter(offset_cell);
			}
			else
			{
				if (SaveGame.Instance.sandboxEnabled && Grid.Element[offset_cell].IsSolid)
				{
					SimMessages.Dig(offset_cell, -1, false);
				}
				this.ForceSetGameCellData(offset_cell);
				Game.Instance.AddSolidChangedFilter(offset_cell);
			}
			Sim.Cell.Properties simCellProperties = this.GetSimCellProperties();
			SimMessages.SetCellProperties(offset_cell, (byte)simCellProperties);
			Grid.RenderedByWorld[offset_cell] = false;
			Game.Instance.GetComponent<EntombedItemVisualizer>().ForceClear(offset_cell);
		});
		base.Subscribe(675471409, new Action<object>(this.OnMelted));
		base.Subscribe<SimCellOccupier>(-1699355994, SimCellOccupier.OnBuildingRepairedDelegate);
	}

	// Token: 0x06004F76 RID: 20342 RVA: 0x001C8FF8 File Offset: 0x001C71F8
	private void OnMelted(object o)
	{
		Building.CreateBuildingMeltedNotification(base.gameObject);
	}

	// Token: 0x06004F77 RID: 20343 RVA: 0x001C9005 File Offset: 0x001C7205
	protected override void OnCleanUp()
	{
		if (this.callDestroy)
		{
			this.DestroySelf(null);
		}
	}

	// Token: 0x06004F78 RID: 20344 RVA: 0x001C9018 File Offset: 0x001C7218
	private Sim.Cell.Properties GetSimCellProperties()
	{
		Sim.Cell.Properties properties = Sim.Cell.Properties.SolidImpermeable;
		if (this.setGasImpermeable)
		{
			properties |= Sim.Cell.Properties.GasImpermeable;
		}
		if (this.setLiquidImpermeable)
		{
			properties |= Sim.Cell.Properties.LiquidImpermeable;
		}
		if (this.setTransparent)
		{
			properties |= Sim.Cell.Properties.Transparent;
		}
		if (this.setOpaque)
		{
			properties |= Sim.Cell.Properties.Opaque;
		}
		if (this.setConstructedTile)
		{
			properties |= Sim.Cell.Properties.ConstructedTile;
		}
		if (this.notifyOnMelt)
		{
			properties |= Sim.Cell.Properties.NotifyOnMelt;
		}
		return properties;
	}

	// Token: 0x06004F79 RID: 20345 RVA: 0x001C9078 File Offset: 0x001C7278
	public void DestroySelf(System.Action onComplete)
	{
		this.callDestroy = false;
		for (int i = 0; i < this.building.PlacementCells.Length; i++)
		{
			int num = this.building.PlacementCells[i];
			Game.Instance.RemoveSolidChangedFilter(num);
			Sim.Cell.Properties simCellProperties = this.GetSimCellProperties();
			SimMessages.ClearCellProperties(num, (byte)simCellProperties);
			if (this.doReplaceElement && Grid.Element[num].id == this.primaryElement.ElementID)
			{
				HandleVector<int>.Handle handle = GameComps.DiseaseContainers.GetHandle(base.gameObject);
				if (handle.IsValid())
				{
					DiseaseHeader header = GameComps.DiseaseContainers.GetHeader(handle);
					header.diseaseIdx = Grid.DiseaseIdx[num];
					header.diseaseCount = Grid.DiseaseCount[num];
					GameComps.DiseaseContainers.SetHeader(handle, header);
				}
				if (onComplete != null)
				{
					HandleVector<Game.CallbackInfo>.Handle handle2 = Game.Instance.callbackManager.Add(new Game.CallbackInfo(onComplete, false));
					SimMessages.ReplaceElement(num, SimHashes.Vacuum, CellEventLogger.Instance.SimCellOccupierDestroySelf, 0f, -1f, byte.MaxValue, 0, handle2.index);
				}
				else
				{
					SimMessages.ReplaceElement(num, SimHashes.Vacuum, CellEventLogger.Instance.SimCellOccupierDestroySelf, 0f, -1f, byte.MaxValue, 0, -1);
				}
				SimMessages.SetStrength(num, 1, 1f);
			}
			else
			{
				Grid.SetSolid(num, false, CellEventLogger.Instance.SimCellOccupierDestroy);
				onComplete.Signal();
				World.Instance.OnSolidChanged(num);
				GameScenePartitioner.Instance.TriggerEvent(num, GameScenePartitioner.Instance.solidChangedLayer, null);
			}
		}
	}

	// Token: 0x06004F7A RID: 20346 RVA: 0x001C920B File Offset: 0x001C740B
	public bool IsReady()
	{
		return this.isReady;
	}

	// Token: 0x06004F7B RID: 20347 RVA: 0x001C9214 File Offset: 0x001C7414
	private void OnModifyComplete()
	{
		if (this == null || base.gameObject == null)
		{
			return;
		}
		this.isReady = true;
		base.GetComponent<PrimaryElement>().SetUseSimDiseaseInfo(true);
		Vector2I vector2I = Grid.PosToXY(base.transform.GetPosition());
		GameScenePartitioner.Instance.TriggerEvent(vector2I.x, vector2I.y, 1, 1, GameScenePartitioner.Instance.solidChangedLayer, null);
	}

	// Token: 0x06004F7C RID: 20348 RVA: 0x001C9280 File Offset: 0x001C7480
	private void ForceSetGameCellData(int cell)
	{
		bool solid = !Grid.DupePassable[cell];
		Grid.SetSolid(cell, solid, CellEventLogger.Instance.SimCellOccupierForceSolid);
		Pathfinding.Instance.AddDirtyNavGridCell(cell);
		GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.solidChangedLayer, null);
		Grid.Damage[cell] = 0f;
	}

	// Token: 0x06004F7D RID: 20349 RVA: 0x001C92DC File Offset: 0x001C74DC
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = null;
		if (this.movementSpeedMultiplier != 1f)
		{
			list = new List<Descriptor>();
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.DUPLICANTMOVEMENTBOOST, GameUtil.AddPositiveSign(GameUtil.GetFormattedPercent(this.movementSpeedMultiplier * 100f - 100f, GameUtil.TimeSlice.None), this.movementSpeedMultiplier - 1f >= 0f)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.DUPLICANTMOVEMENTBOOST, GameUtil.GetFormattedPercent(this.movementSpeedMultiplier * 100f - 100f, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect);
			list.Add(item);
		}
		return list;
	}

	// Token: 0x06004F7E RID: 20350 RVA: 0x001C9384 File Offset: 0x001C7584
	private void OnBuildingRepaired(object data)
	{
		BuildingHP buildingHP = (BuildingHP)data;
		float damage = 1f - (float)buildingHP.HitPoints / (float)buildingHP.MaxHitPoints;
		this.building.RunOnArea(delegate(int offset_cell)
		{
			WorldDamage.Instance.RestoreDamageToValue(offset_cell, damage);
		});
	}

	// Token: 0x040034CC RID: 13516
	[MyCmpReq]
	private Building building;

	// Token: 0x040034CD RID: 13517
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x040034CE RID: 13518
	[SerializeField]
	public bool doReplaceElement = true;

	// Token: 0x040034CF RID: 13519
	[SerializeField]
	public bool setGasImpermeable;

	// Token: 0x040034D0 RID: 13520
	[SerializeField]
	public bool setLiquidImpermeable;

	// Token: 0x040034D1 RID: 13521
	[SerializeField]
	public bool setTransparent;

	// Token: 0x040034D2 RID: 13522
	[SerializeField]
	public bool setOpaque;

	// Token: 0x040034D3 RID: 13523
	[SerializeField]
	public bool notifyOnMelt;

	// Token: 0x040034D4 RID: 13524
	[SerializeField]
	private bool setConstructedTile;

	// Token: 0x040034D5 RID: 13525
	[SerializeField]
	public float strengthMultiplier = 1f;

	// Token: 0x040034D6 RID: 13526
	[SerializeField]
	public float movementSpeedMultiplier = 1f;

	// Token: 0x040034D7 RID: 13527
	private bool isReady;

	// Token: 0x040034D8 RID: 13528
	private bool callDestroy = true;

	// Token: 0x040034D9 RID: 13529
	private static readonly EventSystem.IntraObjectHandler<SimCellOccupier> OnBuildingRepairedDelegate = new EventSystem.IntraObjectHandler<SimCellOccupier>(delegate(SimCellOccupier component, object data)
	{
		component.OnBuildingRepaired(data);
	});
}
