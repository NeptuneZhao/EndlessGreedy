using System;
using UnityEngine;

// Token: 0x02000A60 RID: 2656
public class FetchDrone : KMonoBehaviour
{
	// Token: 0x06004D26 RID: 19750 RVA: 0x001B9E24 File Offset: 0x001B8024
	protected override void OnSpawn()
	{
		ChoreGroup[] array = new ChoreGroup[]
		{
			Db.Get().ChoreGroups.Build,
			Db.Get().ChoreGroups.Basekeeping,
			Db.Get().ChoreGroups.Cook,
			Db.Get().ChoreGroups.Art,
			Db.Get().ChoreGroups.Dig,
			Db.Get().ChoreGroups.Research,
			Db.Get().ChoreGroups.Farming,
			Db.Get().ChoreGroups.Ranching,
			Db.Get().ChoreGroups.MachineOperating,
			Db.Get().ChoreGroups.MedicalAid,
			Db.Get().ChoreGroups.Combat,
			Db.Get().ChoreGroups.LifeSupport,
			Db.Get().ChoreGroups.Recreation,
			Db.Get().ChoreGroups.Toggle,
			Db.Get().ChoreGroups.Rocketry
		};
		for (int i = 0; i < array.Length; i++)
		{
			this.choreConsumer.SetPermittedByUser(array[i], false);
		}
		foreach (Storage storage in base.GetComponents<Storage>())
		{
			if (storage.storageID != GameTags.ChargedPortableBattery)
			{
				this.pickupableStorage = storage;
				break;
			}
		}
		this.SetupPickupable();
		this.pickupableStorage.Subscribe(-1697596308, new Action<object>(this.OnStorageChanged));
	}

	// Token: 0x06004D27 RID: 19751 RVA: 0x001B9FC4 File Offset: 0x001B81C4
	public void SetupPickupable()
	{
		this.animController = base.GetComponent<KBatchedAnimController>();
		GameObject gameObject = Util.NewGameObject(base.gameObject, "pickupableSymbol");
		gameObject.SetActive(false);
		bool flag;
		Vector3 position = this.animController.GetSymbolTransform(FetchDrone.HASH_SNAPTO_THING, out flag).GetColumn(3);
		position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
		gameObject.transform.SetPosition(position);
		this.pickupableKBAC = gameObject.AddComponent<KBatchedAnimController>();
		this.pickupableKBAC.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("algae_kanim")
		};
		this.pickupableKBAC.sceneLayer = Grid.SceneLayer.BuildingUse;
		KBatchedAnimTracker kbatchedAnimTracker = gameObject.AddComponent<KBatchedAnimTracker>();
		kbatchedAnimTracker.symbol = FetchDrone.HASH_SNAPTO_THING;
		kbatchedAnimTracker.offset = Vector3.zero;
	}

	// Token: 0x06004D28 RID: 19752 RVA: 0x001BA092 File Offset: 0x001B8292
	private void OnStorageChanged(object data)
	{
		this.ShowPickupSymbol(!this.pickupableStorage.IsEmpty());
	}

	// Token: 0x06004D29 RID: 19753 RVA: 0x001BA0A8 File Offset: 0x001B82A8
	private void ShowPickupSymbol(bool show)
	{
		this.pickupableKBAC.gameObject.SetActive(show);
		if (show)
		{
			Pickupable component = this.pickupableStorage.items[0].GetComponent<Pickupable>();
			if (component != null)
			{
				KBatchedAnimController component2 = component.GetComponent<KBatchedAnimController>();
				if (component.GetComponent<MinionIdentity>())
				{
					this.AddAnimTracker(component2.gameObject);
				}
				else
				{
					this.pickupableKBAC.SwapAnims(component2.AnimFiles);
					this.pickupableKBAC.Play(component2.currentAnim, KAnim.PlayMode.Loop, 1f, 0f);
				}
			}
		}
		this.animController.SetSymbolVisiblity(FetchDrone.BOTTOM, !show);
		this.animController.SetSymbolVisiblity(FetchDrone.BOTTOM_CARRY, show);
	}

	// Token: 0x06004D2A RID: 19754 RVA: 0x001BA168 File Offset: 0x001B8368
	private void AddAnimTracker(GameObject go)
	{
		KAnimControllerBase component = go.GetComponent<KAnimControllerBase>();
		if (component == null)
		{
			return;
		}
		if (component.AnimFiles != null && component.AnimFiles.Length != 0 && component.AnimFiles[0] != null && component.GetComponent<Pickupable>().trackOnPickup)
		{
			KBatchedAnimTracker kbatchedAnimTracker = go.AddComponent<KBatchedAnimTracker>();
			kbatchedAnimTracker.useTargetPoint = false;
			kbatchedAnimTracker.fadeOut = false;
			kbatchedAnimTracker.symbol = new HashedString("snapTo_chest");
			kbatchedAnimTracker.forceAlwaysVisible = true;
		}
	}

	// Token: 0x0400333B RID: 13115
	private static string HASH_SNAPTO_THING = "snapTo_thing";

	// Token: 0x0400333C RID: 13116
	private static string BOTTOM = "bottom";

	// Token: 0x0400333D RID: 13117
	private static string BOTTOM_CARRY = "bottom_carry";

	// Token: 0x0400333E RID: 13118
	private KBatchedAnimController pickupableKBAC;

	// Token: 0x0400333F RID: 13119
	private KBatchedAnimController animController;

	// Token: 0x04003340 RID: 13120
	private Storage pickupableStorage;

	// Token: 0x04003341 RID: 13121
	[MyCmpAdd]
	private ChoreConsumer choreConsumer;
}
