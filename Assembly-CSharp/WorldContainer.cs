using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Delaunay.Geo;
using Klei;
using KSerialization;
using ProcGen;
using ProcGenGame;
using TemplateClasses;
using TUNING;
using UnityEngine;

// Token: 0x02000B63 RID: 2915
[SerializationConfig(MemberSerialization.OptIn)]
public class WorldContainer : KMonoBehaviour
{
	// Token: 0x1700067A RID: 1658
	// (get) Token: 0x06005725 RID: 22309 RVA: 0x001F1CC1 File Offset: 0x001EFEC1
	// (set) Token: 0x06005726 RID: 22310 RVA: 0x001F1CC9 File Offset: 0x001EFEC9
	[Serialize]
	public WorldInventory worldInventory { get; private set; }

	// Token: 0x1700067B RID: 1659
	// (get) Token: 0x06005727 RID: 22311 RVA: 0x001F1CD2 File Offset: 0x001EFED2
	// (set) Token: 0x06005728 RID: 22312 RVA: 0x001F1CDA File Offset: 0x001EFEDA
	public Dictionary<Tag, float> materialNeeds { get; private set; }

	// Token: 0x1700067C RID: 1660
	// (get) Token: 0x06005729 RID: 22313 RVA: 0x001F1CE3 File Offset: 0x001EFEE3
	public bool IsModuleInterior
	{
		get
		{
			return this.isModuleInterior;
		}
	}

	// Token: 0x1700067D RID: 1661
	// (get) Token: 0x0600572A RID: 22314 RVA: 0x001F1CEB File Offset: 0x001EFEEB
	public bool IsDiscovered
	{
		get
		{
			return this.isDiscovered || DebugHandler.RevealFogOfWar;
		}
	}

	// Token: 0x1700067E RID: 1662
	// (get) Token: 0x0600572B RID: 22315 RVA: 0x001F1CFC File Offset: 0x001EFEFC
	public bool IsStartWorld
	{
		get
		{
			return this.isStartWorld;
		}
	}

	// Token: 0x1700067F RID: 1663
	// (get) Token: 0x0600572C RID: 22316 RVA: 0x001F1D04 File Offset: 0x001EFF04
	public bool IsDupeVisited
	{
		get
		{
			return this.isDupeVisited;
		}
	}

	// Token: 0x17000680 RID: 1664
	// (get) Token: 0x0600572D RID: 22317 RVA: 0x001F1D0C File Offset: 0x001EFF0C
	public float DupeVisitedTimestamp
	{
		get
		{
			return this.dupeVisitedTimestamp;
		}
	}

	// Token: 0x17000681 RID: 1665
	// (get) Token: 0x0600572E RID: 22318 RVA: 0x001F1D14 File Offset: 0x001EFF14
	public float DiscoveryTimestamp
	{
		get
		{
			return this.discoveryTimestamp;
		}
	}

	// Token: 0x17000682 RID: 1666
	// (get) Token: 0x0600572F RID: 22319 RVA: 0x001F1D1C File Offset: 0x001EFF1C
	public bool IsRoverVisted
	{
		get
		{
			return this.isRoverVisited;
		}
	}

	// Token: 0x17000683 RID: 1667
	// (get) Token: 0x06005730 RID: 22320 RVA: 0x001F1D24 File Offset: 0x001EFF24
	public bool IsSurfaceRevealed
	{
		get
		{
			return this.isSurfaceRevealed;
		}
	}

	// Token: 0x17000684 RID: 1668
	// (get) Token: 0x06005731 RID: 22321 RVA: 0x001F1D2C File Offset: 0x001EFF2C
	public Dictionary<string, int> SunlightFixedTraits
	{
		get
		{
			return this.sunlightFixedTraits;
		}
	}

	// Token: 0x17000685 RID: 1669
	// (get) Token: 0x06005732 RID: 22322 RVA: 0x001F1D34 File Offset: 0x001EFF34
	public Dictionary<string, int> NorthernLightsFixedTraits
	{
		get
		{
			return this.northernLightsFixedTraits;
		}
	}

	// Token: 0x17000686 RID: 1670
	// (get) Token: 0x06005733 RID: 22323 RVA: 0x001F1D3C File Offset: 0x001EFF3C
	public Dictionary<string, int> CosmicRadiationFixedTraits
	{
		get
		{
			return this.cosmicRadiationFixedTraits;
		}
	}

	// Token: 0x17000687 RID: 1671
	// (get) Token: 0x06005734 RID: 22324 RVA: 0x001F1D44 File Offset: 0x001EFF44
	public List<string> Biomes
	{
		get
		{
			return this.m_subworldNames;
		}
	}

	// Token: 0x17000688 RID: 1672
	// (get) Token: 0x06005735 RID: 22325 RVA: 0x001F1D4C File Offset: 0x001EFF4C
	public List<string> GeneratedBiomes
	{
		get
		{
			return this.m_generatedSubworlds;
		}
	}

	// Token: 0x17000689 RID: 1673
	// (get) Token: 0x06005736 RID: 22326 RVA: 0x001F1D54 File Offset: 0x001EFF54
	public List<string> WorldTraitIds
	{
		get
		{
			return this.m_worldTraitIds;
		}
	}

	// Token: 0x1700068A RID: 1674
	// (get) Token: 0x06005737 RID: 22327 RVA: 0x001F1D5C File Offset: 0x001EFF5C
	public List<string> StoryTraitIds
	{
		get
		{
			return this.m_storyTraitIds;
		}
	}

	// Token: 0x1700068B RID: 1675
	// (get) Token: 0x06005738 RID: 22328 RVA: 0x001F1D64 File Offset: 0x001EFF64
	public AlertStateManager.Instance AlertManager
	{
		get
		{
			if (this.m_alertManager == null)
			{
				StateMachineController component = base.GetComponent<StateMachineController>();
				this.m_alertManager = component.GetSMI<AlertStateManager.Instance>();
			}
			global::Debug.Assert(this.m_alertManager != null, "AlertStateManager should never be null.");
			return this.m_alertManager;
		}
	}

	// Token: 0x06005739 RID: 22329 RVA: 0x001F1DA5 File Offset: 0x001EFFA5
	public void AddTopPriorityPrioritizable(Prioritizable prioritizable)
	{
		if (!this.yellowAlertTasks.Contains(prioritizable))
		{
			this.yellowAlertTasks.Add(prioritizable);
		}
		this.RefreshHasTopPriorityChore();
	}

	// Token: 0x0600573A RID: 22330 RVA: 0x001F1DC8 File Offset: 0x001EFFC8
	public void RemoveTopPriorityPrioritizable(Prioritizable prioritizable)
	{
		for (int i = this.yellowAlertTasks.Count - 1; i >= 0; i--)
		{
			if (this.yellowAlertTasks[i] == prioritizable || this.yellowAlertTasks[i].Equals(null))
			{
				this.yellowAlertTasks.RemoveAt(i);
			}
		}
		this.RefreshHasTopPriorityChore();
	}

	// Token: 0x1700068C RID: 1676
	// (get) Token: 0x0600573B RID: 22331 RVA: 0x001F1E27 File Offset: 0x001F0027
	// (set) Token: 0x0600573C RID: 22332 RVA: 0x001F1E2F File Offset: 0x001F002F
	public int ParentWorldId { get; private set; }

	// Token: 0x0600573D RID: 22333 RVA: 0x001F1E38 File Offset: 0x001F0038
	public ICollection<int> GetChildWorldIds()
	{
		return this.m_childWorlds;
	}

	// Token: 0x0600573E RID: 22334 RVA: 0x001F1E40 File Offset: 0x001F0040
	private void OnWorldRemoved(object data)
	{
		int num = (data is int) ? ((int)data) : 255;
		if (num != 255)
		{
			this.m_childWorlds.Remove(num);
		}
	}

	// Token: 0x0600573F RID: 22335 RVA: 0x001F1E78 File Offset: 0x001F0078
	private void OnWorldParentChanged(object data)
	{
		WorldParentChangedEventArgs worldParentChangedEventArgs = data as WorldParentChangedEventArgs;
		if (worldParentChangedEventArgs == null)
		{
			return;
		}
		if (worldParentChangedEventArgs.world.ParentWorldId == this.id)
		{
			this.m_childWorlds.Add(worldParentChangedEventArgs.world.id);
		}
		if (worldParentChangedEventArgs.lastParentId == this.ParentWorldId)
		{
			this.m_childWorlds.Remove(worldParentChangedEventArgs.world.id);
		}
	}

	// Token: 0x06005740 RID: 22336 RVA: 0x001F1EE0 File Offset: 0x001F00E0
	public Quadrant[] GetQuadrantOfCell(int cell, int depth = 1)
	{
		Vector2 vector = new Vector2((float)this.WorldSize.x * Grid.CellSizeInMeters, (float)this.worldSize.y * Grid.CellSizeInMeters);
		Vector2 vector2 = Grid.CellToPos2D(Grid.XYToCell(this.WorldOffset.x, this.WorldOffset.y));
		Vector2 vector3 = Grid.CellToPos2D(cell);
		Quadrant[] array = new Quadrant[depth];
		Vector2 vector4 = new Vector2(vector2.x, (float)this.worldOffset.y + vector.y);
		Vector2 vector5 = new Vector2(vector2.x + vector.x, (float)this.worldOffset.y);
		for (int i = 0; i < depth; i++)
		{
			float num = vector5.x - vector4.x;
			float num2 = vector4.y - vector5.y;
			float num3 = num * 0.5f;
			float num4 = num2 * 0.5f;
			if (vector3.x >= vector4.x + num3 && vector3.y >= vector5.y + num4)
			{
				array[i] = Quadrant.NE;
			}
			if (vector3.x >= vector4.x + num3 && vector3.y < vector5.y + num4)
			{
				array[i] = Quadrant.SE;
			}
			if (vector3.x < vector4.x + num3 && vector3.y < vector5.y + num4)
			{
				array[i] = Quadrant.SW;
			}
			if (vector3.x < vector4.x + num3 && vector3.y >= vector5.y + num4)
			{
				array[i] = Quadrant.NW;
			}
			switch (array[i])
			{
			case Quadrant.NE:
				vector4.x += num3;
				vector5.y += num4;
				break;
			case Quadrant.NW:
				vector5.x -= num3;
				vector5.y += num4;
				break;
			case Quadrant.SW:
				vector4.y -= num4;
				vector5.x -= num3;
				break;
			case Quadrant.SE:
				vector4.x += num3;
				vector4.y -= num4;
				break;
			}
		}
		return array;
	}

	// Token: 0x06005741 RID: 22337 RVA: 0x001F210D File Offset: 0x001F030D
	[OnDeserialized]
	private void OnDeserialized()
	{
		this.ParentWorldId = this.id;
	}

	// Token: 0x06005742 RID: 22338 RVA: 0x001F211C File Offset: 0x001F031C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.worldInventory = base.GetComponent<WorldInventory>();
		this.materialNeeds = new Dictionary<Tag, float>();
		ClusterManager.Instance.RegisterWorldContainer(this);
		Game.Instance.Subscribe(880851192, new Action<object>(this.OnWorldParentChanged));
		ClusterManager.Instance.Subscribe(-1078710002, new Action<object>(this.OnWorldRemoved));
	}

	// Token: 0x06005743 RID: 22339 RVA: 0x001F218C File Offset: 0x001F038C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.gameObject.AddOrGet<InfoDescription>().DescriptionLocString = this.worldDescription;
		this.RefreshHasTopPriorityChore();
		this.UpgradeFixedTraits();
		this.RefreshFixedTraits();
		if (DlcManager.IsPureVanilla())
		{
			this.isStartWorld = true;
			this.isDupeVisited = true;
		}
	}

	// Token: 0x06005744 RID: 22340 RVA: 0x001F21DC File Offset: 0x001F03DC
	protected override void OnCleanUp()
	{
		SaveGame.Instance.materialSelectorSerializer.WipeWorldSelectionData(this.id);
		Game.Instance.Unsubscribe(880851192, new Action<object>(this.OnWorldParentChanged));
		ClusterManager.Instance.Unsubscribe(-1078710002, new Action<object>(this.OnWorldRemoved));
		base.OnCleanUp();
	}

	// Token: 0x06005745 RID: 22341 RVA: 0x001F223C File Offset: 0x001F043C
	private void UpgradeFixedTraits()
	{
		if (this.sunlightFixedTrait == null || this.sunlightFixedTrait == "")
		{
			new Dictionary<int, string>
			{
				{
					160000,
					FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_HIGH
				},
				{
					0,
					FIXEDTRAITS.SUNLIGHT.NAME.NONE
				},
				{
					10000,
					FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_LOW
				},
				{
					20000,
					FIXEDTRAITS.SUNLIGHT.NAME.VERY_LOW
				},
				{
					30000,
					FIXEDTRAITS.SUNLIGHT.NAME.LOW
				},
				{
					35000,
					FIXEDTRAITS.SUNLIGHT.NAME.MED_LOW
				},
				{
					40000,
					FIXEDTRAITS.SUNLIGHT.NAME.MED
				},
				{
					50000,
					FIXEDTRAITS.SUNLIGHT.NAME.MED_HIGH
				},
				{
					60000,
					FIXEDTRAITS.SUNLIGHT.NAME.HIGH
				},
				{
					80000,
					FIXEDTRAITS.SUNLIGHT.NAME.VERY_HIGH
				},
				{
					120000,
					FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_HIGH
				}
			}.TryGetValue(this.sunlight, out this.sunlightFixedTrait);
		}
		if (this.cosmicRadiationFixedTrait == null || this.cosmicRadiationFixedTrait == "")
		{
			new Dictionary<int, string>
			{
				{
					0,
					FIXEDTRAITS.COSMICRADIATION.NAME.NONE
				},
				{
					6,
					FIXEDTRAITS.COSMICRADIATION.NAME.VERY_VERY_LOW
				},
				{
					12,
					FIXEDTRAITS.COSMICRADIATION.NAME.VERY_LOW
				},
				{
					18,
					FIXEDTRAITS.COSMICRADIATION.NAME.LOW
				},
				{
					21,
					FIXEDTRAITS.COSMICRADIATION.NAME.MED_LOW
				},
				{
					25,
					FIXEDTRAITS.COSMICRADIATION.NAME.MED
				},
				{
					31,
					FIXEDTRAITS.COSMICRADIATION.NAME.MED_HIGH
				},
				{
					37,
					FIXEDTRAITS.COSMICRADIATION.NAME.HIGH
				},
				{
					50,
					FIXEDTRAITS.COSMICRADIATION.NAME.VERY_HIGH
				},
				{
					75,
					FIXEDTRAITS.COSMICRADIATION.NAME.VERY_VERY_HIGH
				}
			}.TryGetValue(this.cosmicRadiation, out this.cosmicRadiationFixedTrait);
		}
	}

	// Token: 0x06005746 RID: 22342 RVA: 0x001F23DD File Offset: 0x001F05DD
	private void RefreshFixedTraits()
	{
		this.sunlight = this.GetSunlightValueFromFixedTrait();
		this.cosmicRadiation = this.GetCosmicRadiationValueFromFixedTrait();
		this.northernlights = this.GetNorthernlightValueFromFixedTrait();
	}

	// Token: 0x06005747 RID: 22343 RVA: 0x001F2403 File Offset: 0x001F0603
	private void RefreshHasTopPriorityChore()
	{
		if (this.AlertManager != null)
		{
			this.AlertManager.SetHasTopPriorityChore(this.yellowAlertTasks.Count > 0);
		}
	}

	// Token: 0x06005748 RID: 22344 RVA: 0x001F2426 File Offset: 0x001F0626
	public List<string> GetSeasonIds()
	{
		return this.m_seasonIds;
	}

	// Token: 0x06005749 RID: 22345 RVA: 0x001F242E File Offset: 0x001F062E
	public bool IsRedAlert()
	{
		return this.m_alertManager.IsRedAlert();
	}

	// Token: 0x0600574A RID: 22346 RVA: 0x001F243B File Offset: 0x001F063B
	public bool IsYellowAlert()
	{
		return this.m_alertManager.IsYellowAlert();
	}

	// Token: 0x0600574B RID: 22347 RVA: 0x001F2448 File Offset: 0x001F0648
	public string GetRandomName()
	{
		if (!this.overrideName.IsNullOrWhiteSpace())
		{
			return Strings.Get(this.overrideName);
		}
		return GameUtil.GenerateRandomWorldName(this.nameTables);
	}

	// Token: 0x0600574C RID: 22348 RVA: 0x001F2473 File Offset: 0x001F0673
	public void SetID(int id)
	{
		this.id = id;
		this.ParentWorldId = id;
	}

	// Token: 0x0600574D RID: 22349 RVA: 0x001F2484 File Offset: 0x001F0684
	public void SetParentIdx(int parentIdx)
	{
		this.parentChangeArgs.lastParentId = this.ParentWorldId;
		this.parentChangeArgs.world = this;
		this.ParentWorldId = parentIdx;
		Game.Instance.Trigger(880851192, this.parentChangeArgs);
		this.parentChangeArgs.lastParentId = 255;
	}

	// Token: 0x1700068D RID: 1677
	// (get) Token: 0x0600574E RID: 22350 RVA: 0x001F24DA File Offset: 0x001F06DA
	public Vector2 minimumBounds
	{
		get
		{
			return new Vector2((float)this.worldOffset.x, (float)this.worldOffset.y);
		}
	}

	// Token: 0x1700068E RID: 1678
	// (get) Token: 0x0600574F RID: 22351 RVA: 0x001F24F9 File Offset: 0x001F06F9
	public Vector2 maximumBounds
	{
		get
		{
			return new Vector2((float)(this.worldOffset.x + (this.worldSize.x - 1)), (float)(this.worldOffset.y + (this.worldSize.y - 1)));
		}
	}

	// Token: 0x1700068F RID: 1679
	// (get) Token: 0x06005750 RID: 22352 RVA: 0x001F2534 File Offset: 0x001F0734
	public Vector2I WorldSize
	{
		get
		{
			return this.worldSize;
		}
	}

	// Token: 0x17000690 RID: 1680
	// (get) Token: 0x06005751 RID: 22353 RVA: 0x001F253C File Offset: 0x001F073C
	public Vector2I WorldOffset
	{
		get
		{
			return this.worldOffset;
		}
	}

	// Token: 0x17000691 RID: 1681
	// (get) Token: 0x06005752 RID: 22354 RVA: 0x001F2544 File Offset: 0x001F0744
	public bool FullyEnclosedBorder
	{
		get
		{
			return this.fullyEnclosedBorder;
		}
	}

	// Token: 0x17000692 RID: 1682
	// (get) Token: 0x06005753 RID: 22355 RVA: 0x001F254C File Offset: 0x001F074C
	public int Height
	{
		get
		{
			return this.worldSize.y;
		}
	}

	// Token: 0x17000693 RID: 1683
	// (get) Token: 0x06005754 RID: 22356 RVA: 0x001F2559 File Offset: 0x001F0759
	public int Width
	{
		get
		{
			return this.worldSize.x;
		}
	}

	// Token: 0x06005755 RID: 22357 RVA: 0x001F2566 File Offset: 0x001F0766
	public void SetDiscovered(bool reveal_surface = false)
	{
		if (!this.isDiscovered)
		{
			this.discoveryTimestamp = GameUtil.GetCurrentTimeInCycles();
		}
		this.isDiscovered = true;
		if (reveal_surface)
		{
			this.LookAtSurface();
		}
		Game.Instance.Trigger(-521212405, this);
	}

	// Token: 0x06005756 RID: 22358 RVA: 0x001F259B File Offset: 0x001F079B
	public void SetDupeVisited()
	{
		if (!this.isDupeVisited)
		{
			this.dupeVisitedTimestamp = GameUtil.GetCurrentTimeInCycles();
			this.isDupeVisited = true;
			Game.Instance.Trigger(-434755240, this);
		}
	}

	// Token: 0x06005757 RID: 22359 RVA: 0x001F25C7 File Offset: 0x001F07C7
	public void SetRoverLanded()
	{
		this.isRoverVisited = true;
	}

	// Token: 0x06005758 RID: 22360 RVA: 0x001F25D0 File Offset: 0x001F07D0
	public void SetRocketInteriorWorldDetails(int world_id, Vector2I size, Vector2I offset)
	{
		this.SetID(world_id);
		this.fullyEnclosedBorder = true;
		this.worldOffset = offset;
		this.worldSize = size;
		this.isDiscovered = true;
		this.isModuleInterior = true;
		this.m_seasonIds = new List<string>();
	}

	// Token: 0x06005759 RID: 22361 RVA: 0x001F2608 File Offset: 0x001F0808
	private static int IsClockwise(Vector2 first, Vector2 second, Vector2 origin)
	{
		if (first == second)
		{
			return 0;
		}
		Vector2 vector = first - origin;
		Vector2 vector2 = second - origin;
		float num = Mathf.Atan2(vector.x, vector.y);
		float num2 = Mathf.Atan2(vector2.x, vector2.y);
		if (num < num2)
		{
			return 1;
		}
		if (num > num2)
		{
			return -1;
		}
		if (vector.sqrMagnitude >= vector2.sqrMagnitude)
		{
			return -1;
		}
		return 1;
	}

	// Token: 0x0600575A RID: 22362 RVA: 0x001F2674 File Offset: 0x001F0874
	public void PlaceInteriorTemplate(string template_name, System.Action callback)
	{
		TemplateContainer template = TemplateCache.GetTemplate(template_name);
		Vector2 pos = new Vector2((float)(this.worldSize.x / 2 + this.worldOffset.x), (float)(this.worldSize.y / 2 + this.worldOffset.y));
		TemplateLoader.Stamp(template, pos, callback);
		float val = template.info.size.X / 2f;
		float val2 = template.info.size.Y / 2f;
		float num = Math.Max(val, val2);
		GridVisibility.Reveal((int)pos.x, (int)pos.y, (int)num + 3 + 5, num + 3f);
		WorldDetailSave clusterDetailSave = SaveLoader.Instance.clusterDetailSave;
		this.overworldCell = new WorldDetailSave.OverworldCell();
		List<Vector2> list = new List<Vector2>(template.cells.Count);
		foreach (Prefab prefab in template.buildings)
		{
			if (prefab.id == "RocketWallTile")
			{
				Vector2 vector = new Vector2((float)prefab.location_x + pos.x, (float)prefab.location_y + pos.y);
				if (vector.x > pos.x)
				{
					vector.x += 0.5f;
				}
				if (vector.y > pos.y)
				{
					vector.y += 0.5f;
				}
				list.Add(vector);
			}
		}
		list.Sort((Vector2 v1, Vector2 v2) => WorldContainer.IsClockwise(v1, v2, pos));
		Polygon polygon = new Polygon(list);
		this.overworldCell.poly = polygon;
		this.overworldCell.zoneType = SubWorld.ZoneType.RocketInterior;
		this.overworldCell.tags = new TagSet
		{
			WorldGenTags.RocketInterior
		};
		clusterDetailSave.overworldCells.Add(this.overworldCell);
		for (int i = 0; i < this.worldSize.y; i++)
		{
			for (int j = 0; j < this.worldSize.x; j++)
			{
				Vector2I vector2I = new Vector2I(this.worldOffset.x + j, this.worldOffset.y + i);
				int num2 = Grid.XYToCell(vector2I.x, vector2I.y);
				if (polygon.Contains(new Vector2((float)vector2I.x, (float)vector2I.y)))
				{
					SimMessages.ModifyCellWorldZone(num2, 14);
					global::World.Instance.zoneRenderData.worldZoneTypes[num2] = SubWorld.ZoneType.RocketInterior;
				}
				else
				{
					SimMessages.ModifyCellWorldZone(num2, 7);
					global::World.Instance.zoneRenderData.worldZoneTypes[num2] = SubWorld.ZoneType.Space;
				}
			}
		}
	}

	// Token: 0x0600575B RID: 22363 RVA: 0x001F297C File Offset: 0x001F0B7C
	private int GetDefaultValueForFixedTraitCategory(Dictionary<string, int> traitCategory)
	{
		if (traitCategory == this.northernLightsFixedTraits)
		{
			return FIXEDTRAITS.NORTHERNLIGHTS.DEFAULT_VALUE;
		}
		if (traitCategory == this.sunlightFixedTraits)
		{
			return FIXEDTRAITS.SUNLIGHT.DEFAULT_VALUE;
		}
		if (traitCategory == this.cosmicRadiationFixedTraits)
		{
			return FIXEDTRAITS.COSMICRADIATION.DEFAULT_VALUE;
		}
		return 0;
	}

	// Token: 0x0600575C RID: 22364 RVA: 0x001F29AC File Offset: 0x001F0BAC
	private string GetDefaultFixedTraitFor(Dictionary<string, int> traitCategory)
	{
		if (traitCategory == this.northernLightsFixedTraits)
		{
			return FIXEDTRAITS.NORTHERNLIGHTS.NAME.DEFAULT;
		}
		if (traitCategory == this.sunlightFixedTraits)
		{
			return FIXEDTRAITS.SUNLIGHT.NAME.DEFAULT;
		}
		if (traitCategory == this.cosmicRadiationFixedTraits)
		{
			return FIXEDTRAITS.COSMICRADIATION.NAME.DEFAULT;
		}
		return null;
	}

	// Token: 0x0600575D RID: 22365 RVA: 0x001F29DC File Offset: 0x001F0BDC
	private string GetFixedTraitsFor(Dictionary<string, int> traitCategory, WorldGen world)
	{
		foreach (string text in world.Settings.world.fixedTraits)
		{
			if (traitCategory.ContainsKey(text))
			{
				return text;
			}
		}
		return this.GetDefaultFixedTraitFor(traitCategory);
	}

	// Token: 0x0600575E RID: 22366 RVA: 0x001F2A48 File Offset: 0x001F0C48
	private int GetFixedTraitValueForTrait(Dictionary<string, int> traitCategory, ref string trait)
	{
		if (trait == null)
		{
			trait = this.GetDefaultFixedTraitFor(traitCategory);
		}
		if (traitCategory.ContainsKey(trait))
		{
			return traitCategory[trait];
		}
		return this.GetDefaultValueForFixedTraitCategory(traitCategory);
	}

	// Token: 0x0600575F RID: 22367 RVA: 0x001F2A71 File Offset: 0x001F0C71
	private string GetNorthernlightFixedTraits(WorldGen world)
	{
		return this.GetFixedTraitsFor(this.northernLightsFixedTraits, world);
	}

	// Token: 0x06005760 RID: 22368 RVA: 0x001F2A80 File Offset: 0x001F0C80
	private string GetSunlightFromFixedTraits(WorldGen world)
	{
		return this.GetFixedTraitsFor(this.sunlightFixedTraits, world);
	}

	// Token: 0x06005761 RID: 22369 RVA: 0x001F2A8F File Offset: 0x001F0C8F
	private string GetCosmicRadiationFromFixedTraits(WorldGen world)
	{
		return this.GetFixedTraitsFor(this.cosmicRadiationFixedTraits, world);
	}

	// Token: 0x06005762 RID: 22370 RVA: 0x001F2A9E File Offset: 0x001F0C9E
	private int GetNorthernlightValueFromFixedTrait()
	{
		return this.GetFixedTraitValueForTrait(this.northernLightsFixedTraits, ref this.northernLightFixedTrait);
	}

	// Token: 0x06005763 RID: 22371 RVA: 0x001F2AB2 File Offset: 0x001F0CB2
	private int GetSunlightValueFromFixedTrait()
	{
		return this.GetFixedTraitValueForTrait(this.sunlightFixedTraits, ref this.sunlightFixedTrait);
	}

	// Token: 0x06005764 RID: 22372 RVA: 0x001F2AC6 File Offset: 0x001F0CC6
	private int GetCosmicRadiationValueFromFixedTrait()
	{
		return this.GetFixedTraitValueForTrait(this.cosmicRadiationFixedTraits, ref this.cosmicRadiationFixedTrait);
	}

	// Token: 0x06005765 RID: 22373 RVA: 0x001F2ADC File Offset: 0x001F0CDC
	public void SetWorldDetails(WorldGen world)
	{
		if (world != null)
		{
			this.fullyEnclosedBorder = (world.Settings.GetBoolSetting("DrawWorldBorder") && world.Settings.GetBoolSetting("DrawWorldBorderOverVacuum"));
			this.worldOffset = world.GetPosition();
			this.worldSize = world.GetSize();
			this.isDiscovered = world.isStartingWorld;
			this.isStartWorld = world.isStartingWorld;
			this.worldName = world.Settings.world.filePath;
			this.nameTables = world.Settings.world.nameTables;
			this.worldTags = ((world.Settings.world.worldTags != null) ? world.Settings.world.worldTags.ToArray().ToTagArray() : new Tag[0]);
			this.worldDescription = world.Settings.world.description;
			this.worldType = world.Settings.world.name;
			this.isModuleInterior = world.Settings.world.moduleInterior;
			this.m_seasonIds = new List<string>(world.Settings.world.seasons);
			this.m_generatedSubworlds = world.Settings.world.generatedSubworlds;
			this.northernLightFixedTrait = this.GetNorthernlightFixedTraits(world);
			this.sunlightFixedTrait = this.GetSunlightFromFixedTraits(world);
			this.cosmicRadiationFixedTrait = this.GetCosmicRadiationFromFixedTraits(world);
			this.sunlight = this.GetSunlightValueFromFixedTrait();
			this.northernlights = this.GetNorthernlightValueFromFixedTrait();
			this.cosmicRadiation = this.GetCosmicRadiationValueFromFixedTrait();
			this.currentCosmicIntensity = (float)this.cosmicRadiation;
			HashSet<string> hashSet = new HashSet<string>();
			foreach (string text in world.Settings.world.generatedSubworlds)
			{
				text = text.Substring(0, text.LastIndexOf('/'));
				text = text.Substring(text.LastIndexOf('/') + 1, text.Length - (text.LastIndexOf('/') + 1));
				hashSet.Add(text);
			}
			this.m_subworldNames = hashSet.ToList<string>();
			this.m_worldTraitIds = new List<string>();
			this.m_worldTraitIds.AddRange(world.Settings.GetWorldTraitIDs());
			this.m_storyTraitIds = new List<string>();
			this.m_storyTraitIds.AddRange(world.Settings.GetStoryTraitIDs());
			return;
		}
		this.fullyEnclosedBorder = false;
		this.worldOffset = Vector2I.zero;
		this.worldSize = new Vector2I(Grid.WidthInCells, Grid.HeightInCells);
		this.isDiscovered = true;
		this.isStartWorld = true;
		this.isDupeVisited = true;
		this.m_seasonIds = new List<string>
		{
			Db.Get().GameplaySeasons.MeteorShowers.Id
		};
	}

	// Token: 0x06005766 RID: 22374 RVA: 0x001F2DB4 File Offset: 0x001F0FB4
	public bool ContainsPoint(Vector2 point)
	{
		return point.x >= (float)this.worldOffset.x && point.y >= (float)this.worldOffset.y && point.x < (float)(this.worldOffset.x + this.worldSize.x) && point.y < (float)(this.worldOffset.y + this.worldSize.y);
	}

	// Token: 0x06005767 RID: 22375 RVA: 0x001F2E2C File Offset: 0x001F102C
	public void LookAtSurface()
	{
		if (!this.IsDupeVisited)
		{
			this.RevealSurface();
		}
		Vector3? vector = this.SetSurfaceCameraPos();
		if (ClusterManager.Instance.activeWorldId == this.id && vector != null)
		{
			CameraController.Instance.SnapTo(vector.Value);
		}
	}

	// Token: 0x06005768 RID: 22376 RVA: 0x001F2E7C File Offset: 0x001F107C
	public void RevealSurface()
	{
		if (this.isSurfaceRevealed)
		{
			return;
		}
		this.isSurfaceRevealed = true;
		for (int i = 0; i < this.worldSize.x; i++)
		{
			for (int j = this.worldSize.y - 1; j >= 0; j--)
			{
				int cell = Grid.XYToCell(i + this.worldOffset.x, j + this.worldOffset.y);
				if (!Grid.IsValidCell(cell) || Grid.IsSolidCell(cell) || Grid.IsLiquid(cell))
				{
					break;
				}
				GridVisibility.Reveal(i + this.worldOffset.X, j + this.worldOffset.y, 7, 1f);
			}
		}
	}

	// Token: 0x06005769 RID: 22377 RVA: 0x001F2F28 File Offset: 0x001F1128
	private Vector3? SetSurfaceCameraPos()
	{
		if (SaveGame.Instance != null)
		{
			int num = (int)this.maximumBounds.y;
			for (int i = 0; i < this.worldSize.X; i++)
			{
				for (int j = this.worldSize.y - 1; j >= 0; j--)
				{
					int num2 = j + this.worldOffset.y;
					int num3 = Grid.XYToCell(i + this.worldOffset.x, num2);
					if (Grid.IsValidCell(num3) && (Grid.Solid[num3] || Grid.IsLiquid(num3)))
					{
						num = Math.Min(num, num2);
						break;
					}
				}
			}
			int num4 = (num + this.worldOffset.y + this.worldSize.y) / 2;
			Vector3 vector = new Vector3((float)(this.WorldOffset.x + this.Width / 2), (float)num4, 0f);
			SaveGame.Instance.GetComponent<UserNavigation>().SetWorldCameraStartPosition(this.id, vector);
			return new Vector3?(vector);
		}
		return null;
	}

	// Token: 0x0600576A RID: 22378 RVA: 0x001F303C File Offset: 0x001F123C
	public void EjectAllDupes(Vector3 spawn_pos)
	{
		foreach (MinionIdentity minionIdentity in Components.MinionIdentities.GetWorldItems(this.id, false))
		{
			minionIdentity.transform.SetLocalPosition(spawn_pos);
		}
	}

	// Token: 0x0600576B RID: 22379 RVA: 0x001F30A0 File Offset: 0x001F12A0
	public void SpacePodAllDupes(AxialI sourceLocation, SimHashes podElement)
	{
		foreach (MinionIdentity minionIdentity in Components.MinionIdentities.GetWorldItems(this.id, false))
		{
			if (!minionIdentity.HasTag(GameTags.Dead))
			{
				Vector3 position = new Vector3(-1f, -1f, 0f);
				GameObject gameObject = global::Util.KInstantiate(Assets.GetPrefab("EscapePod"), position);
				gameObject.GetComponent<PrimaryElement>().SetElement(podElement, true);
				gameObject.SetActive(true);
				gameObject.GetComponent<MinionStorage>().SerializeMinion(minionIdentity.gameObject);
				TravellingCargoLander.StatesInstance smi = gameObject.GetSMI<TravellingCargoLander.StatesInstance>();
				smi.StartSM();
				smi.Travel(sourceLocation, ClusterUtil.ClosestVisibleAsteroidToLocation(sourceLocation).Location);
			}
		}
	}

	// Token: 0x0600576C RID: 22380 RVA: 0x001F3178 File Offset: 0x001F1378
	public void DestroyWorldBuildings(out HashSet<int> noRefundTiles)
	{
		this.TransferBuildingMaterials(out noRefundTiles);
		foreach (ClustercraftInteriorDoor cmp in Components.ClusterCraftInteriorDoors.GetWorldItems(this.id, false))
		{
			cmp.DeleteObject();
		}
		this.ClearWorldZones();
	}

	// Token: 0x0600576D RID: 22381 RVA: 0x001F31E0 File Offset: 0x001F13E0
	public void TransferResourcesToParentWorld(Vector3 spawn_pos, HashSet<int> noRefundTiles)
	{
		this.TransferPickupables(spawn_pos);
		this.TransferLiquidsSolidsAndGases(spawn_pos, noRefundTiles);
	}

	// Token: 0x0600576E RID: 22382 RVA: 0x001F31F4 File Offset: 0x001F13F4
	public void TransferResourcesToDebris(AxialI sourceLocation, HashSet<int> noRefundTiles, SimHashes debrisContainerElement)
	{
		List<Storage> list = new List<Storage>();
		this.TransferPickupablesToDebris(ref list, debrisContainerElement);
		this.TransferLiquidsSolidsAndGasesToDebris(ref list, noRefundTiles, debrisContainerElement);
		foreach (Storage cmp in list)
		{
			RailGunPayload.StatesInstance smi = cmp.GetSMI<RailGunPayload.StatesInstance>();
			smi.StartSM();
			smi.Travel(sourceLocation, ClusterUtil.ClosestVisibleAsteroidToLocation(sourceLocation).Location);
		}
	}

	// Token: 0x0600576F RID: 22383 RVA: 0x001F3270 File Offset: 0x001F1470
	private void TransferBuildingMaterials(out HashSet<int> noRefundTiles)
	{
		HashSet<int> retTemplateFoundationCells = new HashSet<int>();
		ListPool<ScenePartitionerEntry, ClusterManager>.PooledList pooledList = ListPool<ScenePartitionerEntry, ClusterManager>.Allocate();
		GameScenePartitioner.Instance.GatherEntries((int)this.minimumBounds.x, (int)this.minimumBounds.y, this.Width, this.Height, GameScenePartitioner.Instance.completeBuildings, pooledList);
		Action<int> <>9__0;
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			BuildingComplete buildingComplete = scenePartitionerEntry.obj as BuildingComplete;
			if (buildingComplete != null)
			{
				Deconstructable component = buildingComplete.GetComponent<Deconstructable>();
				if (component != null && !buildingComplete.HasTag(GameTags.NoRocketRefund))
				{
					PrimaryElement component2 = buildingComplete.GetComponent<PrimaryElement>();
					float temperature = component2.Temperature;
					byte diseaseIdx = component2.DiseaseIdx;
					int diseaseCount = component2.DiseaseCount;
					int num = 0;
					while (num < component.constructionElements.Length && buildingComplete.Def.Mass.Length > num)
					{
						Element element = ElementLoader.GetElement(component.constructionElements[num]);
						if (element != null)
						{
							element.substance.SpawnResource(buildingComplete.transform.GetPosition(), buildingComplete.Def.Mass[num], temperature, diseaseIdx, diseaseCount, false, false, false);
						}
						else
						{
							GameObject prefab = Assets.GetPrefab(component.constructionElements[num]);
							int num2 = 0;
							while ((float)num2 < buildingComplete.Def.Mass[num])
							{
								GameUtil.KInstantiate(prefab, buildingComplete.transform.GetPosition(), Grid.SceneLayer.Ore, null, 0).SetActive(true);
								num2++;
							}
						}
						num++;
					}
				}
				SimCellOccupier component3 = buildingComplete.GetComponent<SimCellOccupier>();
				if (component3 != null && component3.doReplaceElement)
				{
					Building building = buildingComplete;
					Action<int> callback;
					if ((callback = <>9__0) == null)
					{
						callback = (<>9__0 = delegate(int cell)
						{
							retTemplateFoundationCells.Add(cell);
						});
					}
					building.RunOnArea(callback);
				}
				Storage component4 = buildingComplete.GetComponent<Storage>();
				if (component4 != null)
				{
					component4.DropAll(false, false, default(Vector3), true, null);
				}
				PlantablePlot component5 = buildingComplete.GetComponent<PlantablePlot>();
				if (component5 != null)
				{
					SeedProducer seedProducer = (component5.Occupant != null) ? component5.Occupant.GetComponent<SeedProducer>() : null;
					if (seedProducer != null)
					{
						seedProducer.DropSeed(null);
					}
				}
				buildingComplete.DeleteObject();
			}
		}
		pooledList.Clear();
		noRefundTiles = retTemplateFoundationCells;
	}

	// Token: 0x06005770 RID: 22384 RVA: 0x001F3500 File Offset: 0x001F1700
	private void TransferPickupables(Vector3 pos)
	{
		int cell = Grid.PosToCell(pos);
		ListPool<ScenePartitionerEntry, ClusterManager>.PooledList pooledList = ListPool<ScenePartitionerEntry, ClusterManager>.Allocate();
		GameScenePartitioner.Instance.GatherEntries((int)this.minimumBounds.x, (int)this.minimumBounds.y, this.Width, this.Height, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			if (scenePartitionerEntry.obj != null)
			{
				Pickupable pickupable = scenePartitionerEntry.obj as Pickupable;
				if (pickupable != null)
				{
					pickupable.gameObject.transform.SetLocalPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
				}
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x06005771 RID: 22385 RVA: 0x001F35CC File Offset: 0x001F17CC
	private void TransferLiquidsSolidsAndGases(Vector3 pos, HashSet<int> noRefundTiles)
	{
		int num = (int)this.minimumBounds.x;
		while ((float)num <= this.maximumBounds.x)
		{
			int num2 = (int)this.minimumBounds.y;
			while ((float)num2 <= this.maximumBounds.y)
			{
				int num3 = Grid.XYToCell(num, num2);
				if (!noRefundTiles.Contains(num3))
				{
					Element element = Grid.Element[num3];
					if (element != null && !element.IsVacuum)
					{
						element.substance.SpawnResource(pos, Grid.Mass[num3], Grid.Temperature[num3], Grid.DiseaseIdx[num3], Grid.DiseaseCount[num3], false, false, false);
					}
				}
				num2++;
			}
			num++;
		}
	}

	// Token: 0x06005772 RID: 22386 RVA: 0x001F3684 File Offset: 0x001F1884
	private void TransferPickupablesToDebris(ref List<Storage> debrisObjects, SimHashes debrisContainerElement)
	{
		ListPool<ScenePartitionerEntry, ClusterManager>.PooledList pooledList = ListPool<ScenePartitionerEntry, ClusterManager>.Allocate();
		GameScenePartitioner.Instance.GatherEntries((int)this.minimumBounds.x, (int)this.minimumBounds.y, this.Width, this.Height, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			if (scenePartitionerEntry.obj != null)
			{
				Pickupable pickupable = scenePartitionerEntry.obj as Pickupable;
				if (pickupable != null)
				{
					if (pickupable.KPrefabID.HasTag(GameTags.BaseMinion))
					{
						global::Util.KDestroyGameObject(pickupable.gameObject);
					}
					else
					{
						pickupable.PrimaryElement.Units = (float)Mathf.Max(1, Mathf.RoundToInt(pickupable.PrimaryElement.Units * 0.5f));
						if ((debrisObjects.Count == 0 || debrisObjects[debrisObjects.Count - 1].RemainingCapacity() == 0f) && pickupable.PrimaryElement.Mass > 0f)
						{
							debrisObjects.Add(CraftModuleInterface.SpawnRocketDebris(" from World Objects", debrisContainerElement));
						}
						Storage storage = debrisObjects[debrisObjects.Count - 1];
						while (pickupable.PrimaryElement.Mass > storage.RemainingCapacity())
						{
							Pickupable pickupable2 = pickupable.Take(storage.RemainingCapacity());
							storage.Store(pickupable2.gameObject, false, false, true, false);
							storage = CraftModuleInterface.SpawnRocketDebris(" from World Objects", debrisContainerElement);
							debrisObjects.Add(storage);
						}
						if (pickupable.PrimaryElement.Mass > 0f)
						{
							storage.Store(pickupable.gameObject, false, false, true, false);
						}
					}
				}
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x06005773 RID: 22387 RVA: 0x001F385C File Offset: 0x001F1A5C
	private void TransferLiquidsSolidsAndGasesToDebris(ref List<Storage> debrisObjects, HashSet<int> noRefundTiles, SimHashes debrisContainerElement)
	{
		int num = (int)this.minimumBounds.x;
		while ((float)num <= this.maximumBounds.x)
		{
			int num2 = (int)this.minimumBounds.y;
			while ((float)num2 <= this.maximumBounds.y)
			{
				int num3 = Grid.XYToCell(num, num2);
				if (!noRefundTiles.Contains(num3))
				{
					Element element = Grid.Element[num3];
					if (element != null && !element.IsVacuum)
					{
						float num4 = Grid.Mass[num3];
						num4 *= 0.5f;
						if ((debrisObjects.Count == 0 || debrisObjects[debrisObjects.Count - 1].RemainingCapacity() == 0f) && num4 > 0f)
						{
							debrisObjects.Add(CraftModuleInterface.SpawnRocketDebris(" from World Tiles", debrisContainerElement));
						}
						Storage storage = debrisObjects[debrisObjects.Count - 1];
						while (num4 > 0f)
						{
							float num5 = Mathf.Min(num4, storage.RemainingCapacity());
							num4 -= num5;
							storage.AddOre(element.id, num5, Grid.Temperature[num3], Grid.DiseaseIdx[num3], Grid.DiseaseCount[num3], false, true);
							if (num4 > 0f)
							{
								storage = CraftModuleInterface.SpawnRocketDebris(" from World Tiles", debrisContainerElement);
								debrisObjects.Add(storage);
							}
						}
					}
				}
				num2++;
			}
			num++;
		}
	}

	// Token: 0x06005774 RID: 22388 RVA: 0x001F39C4 File Offset: 0x001F1BC4
	public void CancelChores()
	{
		for (int i = 0; i < 45; i++)
		{
			int num = (int)this.minimumBounds.x;
			while ((float)num <= this.maximumBounds.x)
			{
				int num2 = (int)this.minimumBounds.y;
				while ((float)num2 <= this.maximumBounds.y)
				{
					int cell = Grid.XYToCell(num, num2);
					GameObject gameObject = Grid.Objects[cell, i];
					if (gameObject != null)
					{
						gameObject.Trigger(2127324410, true);
					}
					num2++;
				}
				num++;
			}
		}
		List<Chore> list;
		GlobalChoreProvider.Instance.choreWorldMap.TryGetValue(this.id, out list);
		int num3 = 0;
		while (list != null && num3 < list.Count)
		{
			Chore chore = list[num3];
			if (chore != null && chore.target != null && !chore.isNull)
			{
				chore.Cancel("World destroyed");
			}
			num3++;
		}
		List<FetchChore> list2;
		GlobalChoreProvider.Instance.fetchMap.TryGetValue(this.id, out list2);
		int num4 = 0;
		while (list2 != null && num4 < list2.Count)
		{
			FetchChore fetchChore = list2[num4];
			if (fetchChore != null && fetchChore.target != null && !fetchChore.isNull)
			{
				fetchChore.Cancel("World destroyed");
			}
			num4++;
		}
	}

	// Token: 0x06005775 RID: 22389 RVA: 0x001F3B1C File Offset: 0x001F1D1C
	public void ClearWorldZones()
	{
		if (this.overworldCell != null)
		{
			WorldDetailSave clusterDetailSave = SaveLoader.Instance.clusterDetailSave;
			int num = -1;
			for (int i = 0; i < SaveLoader.Instance.clusterDetailSave.overworldCells.Count; i++)
			{
				WorldDetailSave.OverworldCell overworldCell = SaveLoader.Instance.clusterDetailSave.overworldCells[i];
				if (overworldCell.zoneType == this.overworldCell.zoneType && overworldCell.tags != null && this.overworldCell.tags != null && overworldCell.tags.ContainsAll(this.overworldCell.tags) && overworldCell.poly.bounds == this.overworldCell.poly.bounds)
				{
					num = i;
					break;
				}
			}
			if (num >= 0)
			{
				clusterDetailSave.overworldCells.RemoveAt(num);
			}
		}
		int num2 = (int)this.minimumBounds.y;
		while ((float)num2 <= this.maximumBounds.y)
		{
			int num3 = (int)this.minimumBounds.x;
			while ((float)num3 <= this.maximumBounds.x)
			{
				SimMessages.ModifyCellWorldZone(Grid.XYToCell(num3, num2), byte.MaxValue);
				num3++;
			}
			num2++;
		}
	}

	// Token: 0x06005776 RID: 22390 RVA: 0x001F3C54 File Offset: 0x001F1E54
	public int GetSafeCell()
	{
		if (this.IsModuleInterior)
		{
			using (List<RocketControlStation>.Enumerator enumerator = Components.RocketControlStations.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					RocketControlStation rocketControlStation = enumerator.Current;
					if (rocketControlStation.GetMyWorldId() == this.id)
					{
						return Grid.PosToCell(rocketControlStation);
					}
				}
				goto IL_A2;
			}
		}
		foreach (Telepad telepad in Components.Telepads.Items)
		{
			if (telepad.GetMyWorldId() == this.id)
			{
				return Grid.PosToCell(telepad);
			}
		}
		IL_A2:
		return Grid.XYToCell(this.worldOffset.x + this.worldSize.x / 2, this.worldOffset.y + this.worldSize.y / 2);
	}

	// Token: 0x06005777 RID: 22391 RVA: 0x001F3D58 File Offset: 0x001F1F58
	public string GetStatus()
	{
		return ColonyDiagnosticUtility.Instance.GetWorldDiagnosticResultStatus(this.id);
	}

	// Token: 0x0400390D RID: 14605
	[Serialize]
	public int id = -1;

	// Token: 0x0400390E RID: 14606
	[Serialize]
	public Tag prefabTag;

	// Token: 0x04003911 RID: 14609
	[Serialize]
	private Vector2I worldOffset;

	// Token: 0x04003912 RID: 14610
	[Serialize]
	private Vector2I worldSize;

	// Token: 0x04003913 RID: 14611
	[Serialize]
	private bool fullyEnclosedBorder;

	// Token: 0x04003914 RID: 14612
	[Serialize]
	private bool isModuleInterior;

	// Token: 0x04003915 RID: 14613
	[Serialize]
	private WorldDetailSave.OverworldCell overworldCell;

	// Token: 0x04003916 RID: 14614
	[Serialize]
	private bool isDiscovered;

	// Token: 0x04003917 RID: 14615
	[Serialize]
	private bool isStartWorld;

	// Token: 0x04003918 RID: 14616
	[Serialize]
	private bool isDupeVisited;

	// Token: 0x04003919 RID: 14617
	[Serialize]
	private float dupeVisitedTimestamp = -1f;

	// Token: 0x0400391A RID: 14618
	[Serialize]
	private float discoveryTimestamp = -1f;

	// Token: 0x0400391B RID: 14619
	[Serialize]
	private bool isRoverVisited;

	// Token: 0x0400391C RID: 14620
	[Serialize]
	private bool isSurfaceRevealed;

	// Token: 0x0400391D RID: 14621
	[Serialize]
	public string worldName;

	// Token: 0x0400391E RID: 14622
	[Serialize]
	public string[] nameTables;

	// Token: 0x0400391F RID: 14623
	[Serialize]
	public Tag[] worldTags;

	// Token: 0x04003920 RID: 14624
	[Serialize]
	public string overrideName;

	// Token: 0x04003921 RID: 14625
	[Serialize]
	public string worldType;

	// Token: 0x04003922 RID: 14626
	[Serialize]
	public string worldDescription;

	// Token: 0x04003923 RID: 14627
	[Serialize]
	public int northernlights = FIXEDTRAITS.NORTHERNLIGHTS.DEFAULT_VALUE;

	// Token: 0x04003924 RID: 14628
	[Serialize]
	public int sunlight = FIXEDTRAITS.SUNLIGHT.DEFAULT_VALUE;

	// Token: 0x04003925 RID: 14629
	[Serialize]
	public int cosmicRadiation = FIXEDTRAITS.COSMICRADIATION.DEFAULT_VALUE;

	// Token: 0x04003926 RID: 14630
	[Serialize]
	public float currentSunlightIntensity;

	// Token: 0x04003927 RID: 14631
	[Serialize]
	public float currentCosmicIntensity = (float)FIXEDTRAITS.COSMICRADIATION.DEFAULT_VALUE;

	// Token: 0x04003928 RID: 14632
	[Serialize]
	public string sunlightFixedTrait;

	// Token: 0x04003929 RID: 14633
	[Serialize]
	public string cosmicRadiationFixedTrait;

	// Token: 0x0400392A RID: 14634
	[Serialize]
	public string northernLightFixedTrait;

	// Token: 0x0400392B RID: 14635
	[Serialize]
	public int fixedTraitsUpdateVersion = 1;

	// Token: 0x0400392C RID: 14636
	private Dictionary<string, int> sunlightFixedTraits = new Dictionary<string, int>
	{
		{
			FIXEDTRAITS.SUNLIGHT.NAME.NONE,
			FIXEDTRAITS.SUNLIGHT.NONE
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_LOW,
			FIXEDTRAITS.SUNLIGHT.VERY_VERY_LOW
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.VERY_LOW,
			FIXEDTRAITS.SUNLIGHT.VERY_LOW
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.LOW,
			FIXEDTRAITS.SUNLIGHT.LOW
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.MED_LOW,
			FIXEDTRAITS.SUNLIGHT.MED_LOW
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.MED,
			FIXEDTRAITS.SUNLIGHT.MED
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.MED_HIGH,
			FIXEDTRAITS.SUNLIGHT.MED_HIGH
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.HIGH,
			FIXEDTRAITS.SUNLIGHT.HIGH
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.VERY_HIGH,
			FIXEDTRAITS.SUNLIGHT.VERY_HIGH
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_HIGH,
			FIXEDTRAITS.SUNLIGHT.VERY_VERY_HIGH
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_VERY_HIGH,
			FIXEDTRAITS.SUNLIGHT.VERY_VERY_VERY_HIGH
		}
	};

	// Token: 0x0400392D RID: 14637
	private Dictionary<string, int> northernLightsFixedTraits = new Dictionary<string, int>
	{
		{
			FIXEDTRAITS.NORTHERNLIGHTS.NAME.NONE,
			FIXEDTRAITS.NORTHERNLIGHTS.NONE
		},
		{
			FIXEDTRAITS.NORTHERNLIGHTS.NAME.ENABLED,
			FIXEDTRAITS.NORTHERNLIGHTS.ENABLED
		}
	};

	// Token: 0x0400392E RID: 14638
	private Dictionary<string, int> cosmicRadiationFixedTraits = new Dictionary<string, int>
	{
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.NONE,
			FIXEDTRAITS.COSMICRADIATION.NONE
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.VERY_VERY_LOW,
			FIXEDTRAITS.COSMICRADIATION.VERY_VERY_LOW
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.VERY_LOW,
			FIXEDTRAITS.COSMICRADIATION.VERY_LOW
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.LOW,
			FIXEDTRAITS.COSMICRADIATION.LOW
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.MED_LOW,
			FIXEDTRAITS.COSMICRADIATION.MED_LOW
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.MED,
			FIXEDTRAITS.COSMICRADIATION.MED
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.MED_HIGH,
			FIXEDTRAITS.COSMICRADIATION.MED_HIGH
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.HIGH,
			FIXEDTRAITS.COSMICRADIATION.HIGH
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.VERY_HIGH,
			FIXEDTRAITS.COSMICRADIATION.VERY_HIGH
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.VERY_VERY_HIGH,
			FIXEDTRAITS.COSMICRADIATION.VERY_VERY_HIGH
		}
	};

	// Token: 0x0400392F RID: 14639
	[Serialize]
	private List<string> m_seasonIds;

	// Token: 0x04003930 RID: 14640
	[Serialize]
	private List<string> m_subworldNames;

	// Token: 0x04003931 RID: 14641
	[Serialize]
	private List<string> m_worldTraitIds;

	// Token: 0x04003932 RID: 14642
	[Serialize]
	private List<string> m_storyTraitIds;

	// Token: 0x04003933 RID: 14643
	[Serialize]
	private List<string> m_generatedSubworlds;

	// Token: 0x04003934 RID: 14644
	private WorldParentChangedEventArgs parentChangeArgs = new WorldParentChangedEventArgs();

	// Token: 0x04003935 RID: 14645
	[MySmiReq]
	private AlertStateManager.Instance m_alertManager;

	// Token: 0x04003936 RID: 14646
	private List<Prioritizable> yellowAlertTasks = new List<Prioritizable>();

	// Token: 0x04003938 RID: 14648
	private List<int> m_childWorlds = new List<int>();
}
