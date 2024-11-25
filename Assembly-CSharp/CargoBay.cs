using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000AB6 RID: 2742
[AddComponentMenu("KMonoBehaviour/scripts/CargoBay")]
public class CargoBay : KMonoBehaviour
{
	// Token: 0x060050D3 RID: 20691 RVA: 0x001D0698 File Offset: 0x001CE898
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<KBatchedAnimController>().Play("grounded", KAnim.PlayMode.Loop, 1f, 0f);
		base.Subscribe<CargoBay>(-1277991738, CargoBay.OnLaunchDelegate);
		base.Subscribe<CargoBay>(-887025858, CargoBay.OnLandDelegate);
		base.Subscribe<CargoBay>(493375141, CargoBay.OnRefreshUserMenuDelegate);
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		});
		this.meter.gameObject.GetComponent<KBatchedAnimTracker>().matchParentOffset = true;
		this.OnStorageChange(null);
		base.Subscribe<CargoBay>(-1697596308, CargoBay.OnStorageChangeDelegate);
	}

	// Token: 0x060050D4 RID: 20692 RVA: 0x001D0770 File Offset: 0x001CE970
	private void OnRefreshUserMenu(object data)
	{
		KIconButtonMenu.ButtonInfo button = new KIconButtonMenu.ButtonInfo("action_empty_contents", UI.USERMENUACTIONS.EMPTYSTORAGE.NAME, delegate()
		{
			this.storage.DropAll(false, false, default(Vector3), true, null);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.EMPTYSTORAGE.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x060050D5 RID: 20693 RVA: 0x001D07CC File Offset: 0x001CE9CC
	private void OnStorageChange(object data)
	{
		this.meter.SetPositionPercent(this.storage.MassStored() / this.storage.Capacity());
	}

	// Token: 0x060050D6 RID: 20694 RVA: 0x001D07F0 File Offset: 0x001CE9F0
	public void SpawnResources(object data)
	{
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			return;
		}
		ILaunchableRocket component = base.GetComponent<RocketModule>().conditionManager.GetComponent<ILaunchableRocket>();
		if (component.registerType == LaunchableRocketRegisterType.Clustercraft)
		{
			return;
		}
		SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(SpacecraftManager.instance.GetSpacecraftID(component));
		int rootCell = Grid.PosToCell(base.gameObject);
		foreach (KeyValuePair<SimHashes, float> keyValuePair in spacecraftDestination.GetMissionResourceResult(this.storage.RemainingCapacity(), this.reservedResources, this.storageType == CargoBay.CargoType.Solids, this.storageType == CargoBay.CargoType.Liquids, this.storageType == CargoBay.CargoType.Gasses))
		{
			Element element = ElementLoader.FindElementByHash(keyValuePair.Key);
			if (this.storageType == CargoBay.CargoType.Solids && element.IsSolid)
			{
				GameObject gameObject = Scenario.SpawnPrefab(rootCell, 0, 0, element.tag.Name, Grid.SceneLayer.Ore);
				gameObject.GetComponent<PrimaryElement>().Mass = keyValuePair.Value;
				gameObject.GetComponent<PrimaryElement>().Temperature = ElementLoader.FindElementByHash(keyValuePair.Key).defaultValues.temperature;
				gameObject.SetActive(true);
				this.storage.Store(gameObject, false, false, true, false);
			}
			else if (this.storageType == CargoBay.CargoType.Liquids && element.IsLiquid)
			{
				this.storage.AddLiquid(keyValuePair.Key, keyValuePair.Value, ElementLoader.FindElementByHash(keyValuePair.Key).defaultValues.temperature, byte.MaxValue, 0, false, true);
			}
			else if (this.storageType == CargoBay.CargoType.Gasses && element.IsGas)
			{
				this.storage.AddGasChunk(keyValuePair.Key, keyValuePair.Value, ElementLoader.FindElementByHash(keyValuePair.Key).defaultValues.temperature, byte.MaxValue, 0, false, true);
			}
		}
		if (this.storageType == CargoBay.CargoType.Entities)
		{
			foreach (KeyValuePair<Tag, int> keyValuePair2 in spacecraftDestination.GetMissionEntityResult())
			{
				GameObject prefab = Assets.GetPrefab(keyValuePair2.Key);
				if (prefab == null)
				{
					KCrashReporter.Assert(false, "Missing prefab: " + keyValuePair2.Key.Name, null);
				}
				else
				{
					for (int i = 0; i < keyValuePair2.Value; i++)
					{
						GameObject gameObject2 = Util.KInstantiate(prefab, base.transform.position);
						gameObject2.SetActive(true);
						this.storage.Store(gameObject2, false, false, true, false);
						Baggable component2 = gameObject2.GetComponent<Baggable>();
						if (component2 != null)
						{
							component2.keepWrangledNextTimeRemovedFromStorage = true;
							component2.SetWrangled();
						}
					}
				}
			}
		}
	}

	// Token: 0x060050D7 RID: 20695 RVA: 0x001D0AE0 File Offset: 0x001CECE0
	public void OnLaunch(object data)
	{
		this.ReserveResources();
		ConduitDispenser component = base.GetComponent<ConduitDispenser>();
		if (component != null)
		{
			component.conduitType = ConduitType.None;
		}
	}

	// Token: 0x060050D8 RID: 20696 RVA: 0x001D0B0C File Offset: 0x001CED0C
	private void ReserveResources()
	{
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			return;
		}
		ILaunchableRocket component = base.GetComponent<RocketModule>().conditionManager.GetComponent<ILaunchableRocket>();
		if (component.registerType == LaunchableRocketRegisterType.Clustercraft)
		{
			return;
		}
		int spacecraftID = SpacecraftManager.instance.GetSpacecraftID(component);
		SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(spacecraftID);
		this.reservedResources = spacecraftDestination.ReserveResources(this);
	}

	// Token: 0x060050D9 RID: 20697 RVA: 0x001D0B64 File Offset: 0x001CED64
	public void OnLand(object data)
	{
		this.SpawnResources(data);
		ConduitDispenser component = base.GetComponent<ConduitDispenser>();
		if (component != null)
		{
			CargoBay.CargoType cargoType = this.storageType;
			if (cargoType == CargoBay.CargoType.Liquids)
			{
				component.conduitType = ConduitType.Liquid;
				return;
			}
			if (cargoType == CargoBay.CargoType.Gasses)
			{
				component.conduitType = ConduitType.Gas;
				return;
			}
			component.conduitType = ConduitType.None;
		}
	}

	// Token: 0x040035B1 RID: 13745
	public Storage storage;

	// Token: 0x040035B2 RID: 13746
	private MeterController meter;

	// Token: 0x040035B3 RID: 13747
	[Serialize]
	public float reservedResources;

	// Token: 0x040035B4 RID: 13748
	public CargoBay.CargoType storageType;

	// Token: 0x040035B5 RID: 13749
	public static Dictionary<Element.State, CargoBay.CargoType> ElementStateToCargoTypes = new Dictionary<Element.State, CargoBay.CargoType>
	{
		{
			Element.State.Gas,
			CargoBay.CargoType.Gasses
		},
		{
			Element.State.Liquid,
			CargoBay.CargoType.Liquids
		},
		{
			Element.State.Solid,
			CargoBay.CargoType.Solids
		}
	};

	// Token: 0x040035B6 RID: 13750
	private static readonly EventSystem.IntraObjectHandler<CargoBay> OnLaunchDelegate = new EventSystem.IntraObjectHandler<CargoBay>(delegate(CargoBay component, object data)
	{
		component.OnLaunch(data);
	});

	// Token: 0x040035B7 RID: 13751
	private static readonly EventSystem.IntraObjectHandler<CargoBay> OnLandDelegate = new EventSystem.IntraObjectHandler<CargoBay>(delegate(CargoBay component, object data)
	{
		component.OnLand(data);
	});

	// Token: 0x040035B8 RID: 13752
	private static readonly EventSystem.IntraObjectHandler<CargoBay> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<CargoBay>(delegate(CargoBay component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x040035B9 RID: 13753
	private static readonly EventSystem.IntraObjectHandler<CargoBay> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<CargoBay>(delegate(CargoBay component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x02001AF4 RID: 6900
	public enum CargoType
	{
		// Token: 0x04007E40 RID: 32320
		Solids,
		// Token: 0x04007E41 RID: 32321
		Liquids,
		// Token: 0x04007E42 RID: 32322
		Gasses,
		// Token: 0x04007E43 RID: 32323
		Entities
	}
}
