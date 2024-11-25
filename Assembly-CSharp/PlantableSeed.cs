using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000818 RID: 2072
[AddComponentMenu("KMonoBehaviour/scripts/PlantableSeed")]
public class PlantableSeed : KMonoBehaviour, IReceptacleDirection, IGameObjectEffectDescriptor
{
	// Token: 0x17000402 RID: 1026
	// (get) Token: 0x0600394E RID: 14670 RVA: 0x001388A0 File Offset: 0x00136AA0
	public SingleEntityReceptacle.ReceptacleDirection Direction
	{
		get
		{
			return this.direction;
		}
	}

	// Token: 0x0600394F RID: 14671 RVA: 0x001388A8 File Offset: 0x00136AA8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.timeUntilSelfPlant = Util.RandomVariance(2400f, 600f);
	}

	// Token: 0x06003950 RID: 14672 RVA: 0x001388C5 File Offset: 0x00136AC5
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.PlantableSeeds.Add(this);
	}

	// Token: 0x06003951 RID: 14673 RVA: 0x001388D8 File Offset: 0x00136AD8
	protected override void OnCleanUp()
	{
		Components.PlantableSeeds.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06003952 RID: 14674 RVA: 0x001388EC File Offset: 0x00136AEC
	public void TryPlant(bool allow_plant_from_storage = false)
	{
		this.timeUntilSelfPlant = Util.RandomVariance(2400f, 600f);
		if (!allow_plant_from_storage && base.gameObject.HasTag(GameTags.Stored))
		{
			return;
		}
		int cell = Grid.PosToCell(base.gameObject);
		if (this.TestSuitableGround(cell))
		{
			Vector3 position = Grid.CellToPosCBC(cell, Grid.SceneLayer.BuildingFront);
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(this.PlantID), position, Grid.SceneLayer.BuildingFront, null, 0);
			MutantPlant component = gameObject.GetComponent<MutantPlant>();
			if (component != null)
			{
				base.GetComponent<MutantPlant>().CopyMutationsTo(component);
			}
			gameObject.SetActive(true);
			Pickupable pickupable = this.pickupable.Take(1f);
			if (pickupable != null)
			{
				gameObject.GetComponent<Crop>() != null;
				Util.KDestroyGameObject(pickupable.gameObject);
				return;
			}
			KCrashReporter.Assert(false, "Seed has fractional total amount < 1f", null);
		}
	}

	// Token: 0x06003953 RID: 14675 RVA: 0x001389C0 File Offset: 0x00136BC0
	public bool TestSuitableGround(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		int num;
		if (this.Direction == SingleEntityReceptacle.ReceptacleDirection.Bottom)
		{
			num = Grid.CellAbove(cell);
		}
		else
		{
			num = Grid.CellBelow(cell);
		}
		if (!Grid.IsValidCell(num))
		{
			return false;
		}
		if (Grid.Foundation[num])
		{
			return false;
		}
		if (Grid.Element[num].hardness >= 150)
		{
			return false;
		}
		if (this.replantGroundTag.IsValid && !Grid.Element[num].HasTag(this.replantGroundTag))
		{
			return false;
		}
		GameObject prefab = Assets.GetPrefab(this.PlantID);
		EntombVulnerable component = prefab.GetComponent<EntombVulnerable>();
		if (component != null && !component.IsCellSafe(cell))
		{
			return false;
		}
		DrowningMonitor component2 = prefab.GetComponent<DrowningMonitor>();
		if (component2 != null && !component2.IsCellSafe(cell))
		{
			return false;
		}
		TemperatureVulnerable component3 = prefab.GetComponent<TemperatureVulnerable>();
		if (component3 != null && !component3.IsCellSafe(cell) && Grid.Element[cell].id != SimHashes.Vacuum)
		{
			return false;
		}
		UprootedMonitor component4 = prefab.GetComponent<UprootedMonitor>();
		if (component4 != null && !component4.IsSuitableFoundation(cell))
		{
			return false;
		}
		OccupyArea component5 = prefab.GetComponent<OccupyArea>();
		return !(component5 != null) || component5.CanOccupyArea(cell, ObjectLayer.Building);
	}

	// Token: 0x06003954 RID: 14676 RVA: 0x00138AF4 File Offset: 0x00136CF4
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.direction == SingleEntityReceptacle.ReceptacleDirection.Bottom)
		{
			Descriptor item = new Descriptor(UI.GAMEOBJECTEFFECTS.SEED_REQUIREMENT_CEILING, UI.GAMEOBJECTEFFECTS.TOOLTIPS.SEED_REQUIREMENT_CEILING, Descriptor.DescriptorType.Requirement, false);
			list.Add(item);
		}
		else if (this.direction == SingleEntityReceptacle.ReceptacleDirection.Side)
		{
			Descriptor item2 = new Descriptor(UI.GAMEOBJECTEFFECTS.SEED_REQUIREMENT_WALL, UI.GAMEOBJECTEFFECTS.TOOLTIPS.SEED_REQUIREMENT_WALL, Descriptor.DescriptorType.Requirement, false);
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x04002277 RID: 8823
	public Tag PlantID;

	// Token: 0x04002278 RID: 8824
	public Tag PreviewID;

	// Token: 0x04002279 RID: 8825
	[Serialize]
	public float timeUntilSelfPlant;

	// Token: 0x0400227A RID: 8826
	public Tag replantGroundTag;

	// Token: 0x0400227B RID: 8827
	public string domesticatedDescription;

	// Token: 0x0400227C RID: 8828
	public SingleEntityReceptacle.ReceptacleDirection direction;

	// Token: 0x0400227D RID: 8829
	[MyCmpGet]
	private Pickupable pickupable;
}
