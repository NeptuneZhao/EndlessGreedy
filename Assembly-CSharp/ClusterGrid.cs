using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using STRINGS;
using UnityEngine;

// Token: 0x02000AED RID: 2797
public class ClusterGrid
{
	// Token: 0x06005361 RID: 21345 RVA: 0x001DE4B0 File Offset: 0x001DC6B0
	public static void DestroyInstance()
	{
		ClusterGrid.Instance = null;
	}

	// Token: 0x06005362 RID: 21346 RVA: 0x001DE4B8 File Offset: 0x001DC6B8
	private ClusterFogOfWarManager.Instance GetFOWManager()
	{
		if (this.m_fowManager == null)
		{
			this.m_fowManager = SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>();
		}
		return this.m_fowManager;
	}

	// Token: 0x06005363 RID: 21347 RVA: 0x001DE4D8 File Offset: 0x001DC6D8
	public bool IsValidCell(AxialI cell)
	{
		return this.cellContents.ContainsKey(cell);
	}

	// Token: 0x06005364 RID: 21348 RVA: 0x001DE4E6 File Offset: 0x001DC6E6
	public ClusterGrid(int numRings)
	{
		ClusterGrid.Instance = this;
		this.GenerateGrid(numRings);
		this.m_onClusterLocationChangedDelegate = new Action<object>(this.OnClusterLocationChanged);
	}

	// Token: 0x06005365 RID: 21349 RVA: 0x001DE518 File Offset: 0x001DC718
	public ClusterRevealLevel GetCellRevealLevel(AxialI cell)
	{
		return this.GetFOWManager().GetCellRevealLevel(cell);
	}

	// Token: 0x06005366 RID: 21350 RVA: 0x001DE526 File Offset: 0x001DC726
	public bool IsCellVisible(AxialI cell)
	{
		return this.GetFOWManager().IsLocationRevealed(cell);
	}

	// Token: 0x06005367 RID: 21351 RVA: 0x001DE534 File Offset: 0x001DC734
	public float GetRevealCompleteFraction(AxialI cell)
	{
		return this.GetFOWManager().GetRevealCompleteFraction(cell);
	}

	// Token: 0x06005368 RID: 21352 RVA: 0x001DE542 File Offset: 0x001DC742
	public bool IsVisible(ClusterGridEntity entity)
	{
		return entity.IsVisible && this.IsCellVisible(entity.Location);
	}

	// Token: 0x06005369 RID: 21353 RVA: 0x001DE55C File Offset: 0x001DC75C
	public List<ClusterGridEntity> GetVisibleEntitiesAtCell(AxialI cell)
	{
		if (this.IsValidCell(cell) && this.GetFOWManager().IsLocationRevealed(cell))
		{
			return (from entity in this.cellContents[cell]
			where entity.IsVisible
			select entity).ToList<ClusterGridEntity>();
		}
		return new List<ClusterGridEntity>();
	}

	// Token: 0x0600536A RID: 21354 RVA: 0x001DE5BC File Offset: 0x001DC7BC
	public ClusterGridEntity GetVisibleEntityOfLayerAtCell(AxialI cell, EntityLayer entityLayer)
	{
		if (this.IsValidCell(cell) && this.GetFOWManager().IsLocationRevealed(cell))
		{
			foreach (ClusterGridEntity clusterGridEntity in this.cellContents[cell])
			{
				if (clusterGridEntity.IsVisible && clusterGridEntity.Layer == entityLayer)
				{
					return clusterGridEntity;
				}
			}
		}
		return null;
	}

	// Token: 0x0600536B RID: 21355 RVA: 0x001DE640 File Offset: 0x001DC840
	public ClusterGridEntity GetVisibleEntityOfLayerAtAdjacentCell(AxialI cell, EntityLayer entityLayer)
	{
		return AxialUtil.GetRing(cell, 1).SelectMany(new Func<AxialI, IEnumerable<ClusterGridEntity>>(this.GetVisibleEntitiesAtCell)).FirstOrDefault((ClusterGridEntity entity) => entity.Layer == entityLayer);
	}

	// Token: 0x0600536C RID: 21356 RVA: 0x001DE684 File Offset: 0x001DC884
	public List<ClusterGridEntity> GetHiddenEntitiesOfLayerAtCell(AxialI cell, EntityLayer entityLayer)
	{
		return (from entity in AxialUtil.GetRing(cell, 0).SelectMany(new Func<AxialI, IEnumerable<ClusterGridEntity>>(this.GetHiddenEntitiesAtCell))
		where entity.Layer == entityLayer
		select entity).ToList<ClusterGridEntity>();
	}

	// Token: 0x0600536D RID: 21357 RVA: 0x001DE6CC File Offset: 0x001DC8CC
	public List<ClusterGridEntity> GetEntitiesOfLayerAtCell(AxialI cell, EntityLayer entityLayer)
	{
		return (from entity in AxialUtil.GetRing(cell, 0).SelectMany(new Func<AxialI, IEnumerable<ClusterGridEntity>>(this.GetEntitiesOnCell))
		where entity.Layer == entityLayer
		select entity).ToList<ClusterGridEntity>();
	}

	// Token: 0x0600536E RID: 21358 RVA: 0x001DE714 File Offset: 0x001DC914
	public ClusterGridEntity GetEntityOfLayerAtCell(AxialI cell, EntityLayer entityLayer)
	{
		return AxialUtil.GetRing(cell, 0).SelectMany(new Func<AxialI, IEnumerable<ClusterGridEntity>>(this.GetEntitiesOnCell)).FirstOrDefault((ClusterGridEntity entity) => entity.Layer == entityLayer);
	}

	// Token: 0x0600536F RID: 21359 RVA: 0x001DE758 File Offset: 0x001DC958
	public List<ClusterGridEntity> GetHiddenEntitiesAtCell(AxialI cell)
	{
		if (this.cellContents.ContainsKey(cell) && !this.GetFOWManager().IsLocationRevealed(cell))
		{
			return (from entity in this.cellContents[cell]
			where entity.IsVisible
			select entity).ToList<ClusterGridEntity>();
		}
		return new List<ClusterGridEntity>();
	}

	// Token: 0x06005370 RID: 21360 RVA: 0x001DE7BC File Offset: 0x001DC9BC
	public List<ClusterGridEntity> GetNotVisibleEntitiesAtAdjacentCell(AxialI cell)
	{
		return AxialUtil.GetRing(cell, 1).SelectMany(new Func<AxialI, IEnumerable<ClusterGridEntity>>(this.GetHiddenEntitiesAtCell)).ToList<ClusterGridEntity>();
	}

	// Token: 0x06005371 RID: 21361 RVA: 0x001DE7DC File Offset: 0x001DC9DC
	public List<ClusterGridEntity> GetNotVisibleEntitiesOfLayerAtAdjacentCell(AxialI cell, EntityLayer entityLayer)
	{
		return (from entity in AxialUtil.GetRing(cell, 1).SelectMany(new Func<AxialI, IEnumerable<ClusterGridEntity>>(this.GetHiddenEntitiesAtCell))
		where entity.Layer == entityLayer
		select entity).ToList<ClusterGridEntity>();
	}

	// Token: 0x06005372 RID: 21362 RVA: 0x001DE824 File Offset: 0x001DCA24
	public bool GetVisibleUnidentifiedMeteorShowerWithinRadius(AxialI center, int radius, out ClusterMapMeteorShower.Instance result)
	{
		for (int i = 0; i <= radius; i++)
		{
			foreach (AxialI axialI in AxialUtil.GetRing(center, i))
			{
				if (this.IsValidCell(axialI) && this.GetFOWManager().IsLocationRevealed(axialI))
				{
					foreach (ClusterGridEntity cmp in this.GetEntitiesOfLayerAtCell(axialI, EntityLayer.Craft))
					{
						ClusterMapMeteorShower.Instance smi = cmp.GetSMI<ClusterMapMeteorShower.Instance>();
						if (smi != null && !smi.HasBeenIdentified)
						{
							result = smi;
							return true;
						}
					}
				}
			}
		}
		result = null;
		return false;
	}

	// Token: 0x06005373 RID: 21363 RVA: 0x001DE8FC File Offset: 0x001DCAFC
	public ClusterGridEntity GetAsteroidAtCell(AxialI cell)
	{
		if (!this.cellContents.ContainsKey(cell))
		{
			return null;
		}
		return (from e in this.cellContents[cell]
		where e.Layer == EntityLayer.Asteroid
		select e).FirstOrDefault<ClusterGridEntity>();
	}

	// Token: 0x06005374 RID: 21364 RVA: 0x001DE94E File Offset: 0x001DCB4E
	public bool HasVisibleAsteroidAtCell(AxialI cell)
	{
		return this.GetVisibleEntityOfLayerAtCell(cell, EntityLayer.Asteroid) != null;
	}

	// Token: 0x06005375 RID: 21365 RVA: 0x001DE95E File Offset: 0x001DCB5E
	public void RegisterEntity(ClusterGridEntity entity)
	{
		this.cellContents[entity.Location].Add(entity);
		entity.Subscribe(-1298331547, this.m_onClusterLocationChangedDelegate);
	}

	// Token: 0x06005376 RID: 21366 RVA: 0x001DE989 File Offset: 0x001DCB89
	public void UnregisterEntity(ClusterGridEntity entity)
	{
		this.cellContents[entity.Location].Remove(entity);
		entity.Unsubscribe(-1298331547, this.m_onClusterLocationChangedDelegate);
	}

	// Token: 0x06005377 RID: 21367 RVA: 0x001DE9B4 File Offset: 0x001DCBB4
	public void OnClusterLocationChanged(object data)
	{
		ClusterLocationChangedEvent clusterLocationChangedEvent = (ClusterLocationChangedEvent)data;
		global::Debug.Assert(this.IsValidCell(clusterLocationChangedEvent.oldLocation), string.Format("ChangeEntityCell move order FROM invalid location: {0} {1}", clusterLocationChangedEvent.oldLocation, clusterLocationChangedEvent.entity));
		global::Debug.Assert(this.IsValidCell(clusterLocationChangedEvent.newLocation), string.Format("ChangeEntityCell move order TO invalid location: {0} {1}", clusterLocationChangedEvent.newLocation, clusterLocationChangedEvent.entity));
		this.cellContents[clusterLocationChangedEvent.oldLocation].Remove(clusterLocationChangedEvent.entity);
		this.cellContents[clusterLocationChangedEvent.newLocation].Add(clusterLocationChangedEvent.entity);
	}

	// Token: 0x06005378 RID: 21368 RVA: 0x001DEA59 File Offset: 0x001DCC59
	private AxialI GetNeighbor(AxialI cell, AxialI direction)
	{
		return cell + direction;
	}

	// Token: 0x06005379 RID: 21369 RVA: 0x001DEA64 File Offset: 0x001DCC64
	public int GetCellRing(AxialI cell)
	{
		Vector3I vector3I = cell.ToCube();
		Vector3I vector3I2 = new Vector3I(vector3I.x, vector3I.y, vector3I.z);
		Vector3I vector3I3 = new Vector3I(0, 0, 0);
		return (int)((float)((Mathf.Abs(vector3I2.x - vector3I3.x) + Mathf.Abs(vector3I2.y - vector3I3.y) + Mathf.Abs(vector3I2.z - vector3I3.z)) / 2));
	}

	// Token: 0x0600537A RID: 21370 RVA: 0x001DEAD8 File Offset: 0x001DCCD8
	private void CleanUpGrid()
	{
		this.cellContents.Clear();
	}

	// Token: 0x0600537B RID: 21371 RVA: 0x001DEAE8 File Offset: 0x001DCCE8
	private int GetHexDistance(AxialI a, AxialI b)
	{
		Vector3I vector3I = a.ToCube();
		Vector3I vector3I2 = b.ToCube();
		return Mathf.Max(new int[]
		{
			Mathf.Abs(vector3I.x - vector3I2.x),
			Mathf.Abs(vector3I.y - vector3I2.y),
			Mathf.Abs(vector3I.z - vector3I2.z)
		});
	}

	// Token: 0x0600537C RID: 21372 RVA: 0x001DEB50 File Offset: 0x001DCD50
	public List<ClusterGridEntity> GetEntitiesInRange(AxialI center, int range = 1)
	{
		List<ClusterGridEntity> list = new List<ClusterGridEntity>();
		foreach (KeyValuePair<AxialI, List<ClusterGridEntity>> keyValuePair in this.cellContents)
		{
			if (this.GetHexDistance(keyValuePair.Key, center) <= range)
			{
				list.AddRange(keyValuePair.Value);
			}
		}
		return list;
	}

	// Token: 0x0600537D RID: 21373 RVA: 0x001DEBC4 File Offset: 0x001DCDC4
	public List<ClusterGridEntity> GetEntitiesOnCell(AxialI cell)
	{
		return this.cellContents[cell];
	}

	// Token: 0x0600537E RID: 21374 RVA: 0x001DEBD2 File Offset: 0x001DCDD2
	public bool IsInRange(AxialI a, AxialI b, int range = 1)
	{
		return this.GetHexDistance(a, b) <= range;
	}

	// Token: 0x0600537F RID: 21375 RVA: 0x001DEBE4 File Offset: 0x001DCDE4
	private void GenerateGrid(int rings)
	{
		this.CleanUpGrid();
		this.numRings = rings;
		for (int i = -rings + 1; i < rings; i++)
		{
			for (int j = -rings + 1; j < rings; j++)
			{
				for (int k = -rings + 1; k < rings; k++)
				{
					if (i + j + k == 0)
					{
						AxialI key = new AxialI(i, j);
						this.cellContents.Add(key, new List<ClusterGridEntity>());
					}
				}
			}
		}
	}

	// Token: 0x06005380 RID: 21376 RVA: 0x001DEC4C File Offset: 0x001DCE4C
	public AxialI GetRandomCellAtEdgeOfUniverse()
	{
		int num = this.numRings - 1;
		List<AxialI> rings = AxialUtil.GetRings(AxialI.ZERO, num, num);
		return rings.ElementAt(UnityEngine.Random.Range(0, rings.Count));
	}

	// Token: 0x06005381 RID: 21377 RVA: 0x001DEC84 File Offset: 0x001DCE84
	public Vector3 GetPosition(ClusterGridEntity entity)
	{
		float r = (float)entity.Location.R;
		float q = (float)entity.Location.Q;
		List<ClusterGridEntity> list = this.cellContents[entity.Location];
		if (list.Count <= 1 || !entity.SpaceOutInSameHex())
		{
			return AxialUtil.AxialToWorld(r, q);
		}
		int num = 0;
		int num2 = 0;
		foreach (ClusterGridEntity clusterGridEntity in list)
		{
			if (entity == clusterGridEntity)
			{
				num = num2;
			}
			if (clusterGridEntity.SpaceOutInSameHex())
			{
				num2++;
			}
		}
		if (list.Count > num2)
		{
			num2 += 5;
			num += 5;
		}
		else if (num2 > 0)
		{
			num2++;
			num++;
		}
		if (num2 == 0 || num2 == 1)
		{
			return AxialUtil.AxialToWorld(r, q);
		}
		float num3 = Mathf.Min(Mathf.Pow((float)num2, 0.5f), 1f) * 0.5f;
		float num4 = Mathf.Pow((float)num / (float)num2, 0.5f);
		float num5 = 0.81f;
		float num6 = Mathf.Pow((float)num2, 0.5f) * num5;
		float f = 6.2831855f * num6 * num4;
		float x = Mathf.Cos(f) * num3 * num4;
		float y = Mathf.Sin(f) * num3 * num4;
		return AxialUtil.AxialToWorld(r, q) + new Vector3(x, y, 0f);
	}

	// Token: 0x06005382 RID: 21378 RVA: 0x001DEE08 File Offset: 0x001DD008
	public List<AxialI> GetPath(AxialI start, AxialI end, ClusterDestinationSelector destination_selector)
	{
		string text;
		return this.GetPath(start, end, destination_selector, out text, false);
	}

	// Token: 0x06005383 RID: 21379 RVA: 0x001DEE24 File Offset: 0x001DD024
	public List<AxialI> GetPath(AxialI start, AxialI end, ClusterDestinationSelector destination_selector, out string fail_reason, bool dodgeHiddenAsteroids = false)
	{
		ClusterGrid.<>c__DisplayClass41_0 CS$<>8__locals1;
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.destination_selector = destination_selector;
		CS$<>8__locals1.start = start;
		CS$<>8__locals1.end = end;
		CS$<>8__locals1.dodgeHiddenAsteroids = dodgeHiddenAsteroids;
		fail_reason = null;
		if (!CS$<>8__locals1.destination_selector.canNavigateFogOfWar && !this.IsCellVisible(CS$<>8__locals1.end))
		{
			fail_reason = UI.CLUSTERMAP.TOOLTIP_INVALID_DESTINATION_FOG_OF_WAR;
			return null;
		}
		ClusterGridEntity visibleEntityOfLayerAtCell = this.GetVisibleEntityOfLayerAtCell(CS$<>8__locals1.end, EntityLayer.Asteroid);
		if (visibleEntityOfLayerAtCell != null && CS$<>8__locals1.destination_selector.requireLaunchPadOnAsteroidDestination)
		{
			bool flag = false;
			using (IEnumerator enumerator = Components.LaunchPads.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (((LaunchPad)enumerator.Current).GetMyWorldLocation() == visibleEntityOfLayerAtCell.Location)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				fail_reason = UI.CLUSTERMAP.TOOLTIP_INVALID_DESTINATION_NO_LAUNCH_PAD;
				return null;
			}
		}
		if (visibleEntityOfLayerAtCell == null && CS$<>8__locals1.destination_selector.requireAsteroidDestination)
		{
			fail_reason = UI.CLUSTERMAP.TOOLTIP_INVALID_DESTINATION_REQUIRE_ASTEROID;
			return null;
		}
		CS$<>8__locals1.frontier = new HashSet<AxialI>();
		CS$<>8__locals1.visited = new HashSet<AxialI>();
		CS$<>8__locals1.buffer = new HashSet<AxialI>();
		CS$<>8__locals1.cameFrom = new Dictionary<AxialI, AxialI>();
		CS$<>8__locals1.frontier.Add(CS$<>8__locals1.start);
		while (!CS$<>8__locals1.frontier.Contains(CS$<>8__locals1.end) && CS$<>8__locals1.frontier.Count > 0)
		{
			this.<GetPath>g__ExpandFrontier|41_0(ref CS$<>8__locals1);
		}
		if (CS$<>8__locals1.frontier.Contains(CS$<>8__locals1.end))
		{
			List<AxialI> list = new List<AxialI>();
			AxialI axialI = CS$<>8__locals1.end;
			while (axialI != CS$<>8__locals1.start)
			{
				list.Add(axialI);
				axialI = CS$<>8__locals1.cameFrom[axialI];
			}
			list.Reverse();
			return list;
		}
		fail_reason = UI.CLUSTERMAP.TOOLTIP_INVALID_DESTINATION_NO_PATH;
		return null;
	}

	// Token: 0x06005384 RID: 21380 RVA: 0x001DF014 File Offset: 0x001DD214
	public void GetLocationDescription(AxialI location, out Sprite sprite, out string label, out string sublabel)
	{
		ClusterGridEntity clusterGridEntity = this.GetVisibleEntitiesAtCell(location).Find((ClusterGridEntity x) => x.Layer == EntityLayer.Asteroid);
		ClusterGridEntity visibleEntityOfLayerAtAdjacentCell = this.GetVisibleEntityOfLayerAtAdjacentCell(location, EntityLayer.Asteroid);
		if (clusterGridEntity != null)
		{
			sprite = clusterGridEntity.GetUISprite();
			label = clusterGridEntity.Name;
			WorldContainer component = clusterGridEntity.GetComponent<WorldContainer>();
			sublabel = Strings.Get(component.worldType);
			return;
		}
		if (visibleEntityOfLayerAtAdjacentCell != null)
		{
			sprite = visibleEntityOfLayerAtAdjacentCell.GetUISprite();
			label = UI.SPACEDESTINATIONS.ORBIT.NAME_FMT.Replace("{Name}", visibleEntityOfLayerAtAdjacentCell.Name);
			WorldContainer component2 = visibleEntityOfLayerAtAdjacentCell.GetComponent<WorldContainer>();
			sublabel = Strings.Get(component2.worldType);
			return;
		}
		if (this.IsCellVisible(location))
		{
			sprite = Assets.GetSprite("hex_unknown");
			label = UI.SPACEDESTINATIONS.EMPTY_SPACE.NAME;
			sublabel = "";
			return;
		}
		sprite = Assets.GetSprite("unknown_far");
		label = UI.SPACEDESTINATIONS.FOG_OF_WAR_SPACE.NAME;
		sublabel = "";
	}

	// Token: 0x06005385 RID: 21381 RVA: 0x001DF124 File Offset: 0x001DD324
	[CompilerGenerated]
	private void <GetPath>g__ExpandFrontier|41_0(ref ClusterGrid.<>c__DisplayClass41_0 A_1)
	{
		A_1.buffer.Clear();
		foreach (AxialI axialI in A_1.frontier)
		{
			foreach (AxialI direction in AxialI.DIRECTIONS)
			{
				AxialI neighbor = this.GetNeighbor(axialI, direction);
				if (!A_1.visited.Contains(neighbor) && this.IsValidCell(neighbor) && (this.IsCellVisible(neighbor) || A_1.destination_selector.canNavigateFogOfWar) && (!this.HasVisibleAsteroidAtCell(neighbor) || !(neighbor != A_1.start) || !(neighbor != A_1.end)) && (!A_1.dodgeHiddenAsteroids || !(ClusterGrid.Instance.GetAsteroidAtCell(neighbor) != null) || ClusterGrid.Instance.GetAsteroidAtCell(neighbor).IsVisibleInFOW == ClusterRevealLevel.Visible || !(neighbor != A_1.start) || !(neighbor != A_1.end)))
				{
					A_1.buffer.Add(neighbor);
					if (!A_1.cameFrom.ContainsKey(neighbor))
					{
						A_1.cameFrom.Add(neighbor, axialI);
					}
				}
			}
			A_1.visited.Add(axialI);
		}
		HashSet<AxialI> frontier = A_1.frontier;
		A_1.frontier = A_1.buffer;
		A_1.buffer = frontier;
	}

	// Token: 0x040036EE RID: 14062
	public static ClusterGrid Instance;

	// Token: 0x040036EF RID: 14063
	public const float NodeDistanceScale = 600f;

	// Token: 0x040036F0 RID: 14064
	private const float MAX_OFFSET_RADIUS = 0.5f;

	// Token: 0x040036F1 RID: 14065
	public int numRings;

	// Token: 0x040036F2 RID: 14066
	private ClusterFogOfWarManager.Instance m_fowManager;

	// Token: 0x040036F3 RID: 14067
	private Action<object> m_onClusterLocationChangedDelegate;

	// Token: 0x040036F4 RID: 14068
	public Dictionary<AxialI, List<ClusterGridEntity>> cellContents = new Dictionary<AxialI, List<ClusterGridEntity>>();
}
