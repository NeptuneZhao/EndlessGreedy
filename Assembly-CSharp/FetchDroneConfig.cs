﻿using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000387 RID: 903
public class FetchDroneConfig : IEntityConfig
{
	// Token: 0x060012C3 RID: 4803 RVA: 0x00067105 File Offset: 0x00065305
	public string[] GetDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x060012C4 RID: 4804 RVA: 0x0006710C File Offset: 0x0006530C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateBasicEntity("FetchDrone", this.name, this.desc, 300f, true, Assets.GetAnim("swoopy_bot_kanim"), "idle_loop", Grid.SceneLayer.Creatures, SimHashes.Creature, null, 293f);
		KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
		component.isMovable = true;
		gameObject.AddOrGet<LoopingSounds>();
		KBoxCollider2D kboxCollider2D = gameObject.AddOrGet<KBoxCollider2D>();
		kboxCollider2D.size = new Vector2(1f, 1f);
		kboxCollider2D.offset = new Vector2f(0f, 0.5f);
		Modifiers modifiers = gameObject.AddOrGet<Modifiers>();
		modifiers.initialAmounts.Add(Db.Get().Amounts.HitPoints.Id);
		modifiers.initialAttributes.Add(Db.Get().Attributes.CarryAmount.Id);
		modifiers.initialAmounts.Add(Db.Get().Amounts.InternalElectroBank.Id);
		string text = "FetchDroneBaseTrait";
		gameObject.AddOrGet<Traits>();
		Trait trait = Db.Get().CreateTrait(text, this.name, this.name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Attributes.CarryAmount.Id, TUNING.ROBOTS.FETCHDRONE.CARRY_CAPACITY, this.name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.InternalElectroBank.maxAttribute.Id, 120000f, this.name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.InternalElectroBank.deltaAttribute.Id, -30f, this.name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, TUNING.ROBOTS.FETCHDRONE.HIT_POINTS, this.name, false, false, true));
		modifiers.initialTraits.Add(text);
		gameObject.AddOrGet<AttributeConverters>();
		GridVisibility gridVisibility = gameObject.AddOrGet<GridVisibility>();
		gridVisibility.radius = 30;
		gridVisibility.innerRadius = 20f;
		StandardWorker standardWorker = gameObject.AddOrGet<StandardWorker>();
		standardWorker.isFetchDrone = true;
		standardWorker.fetchOffsets = new CellOffset[]
		{
			CellOffset.up
		};
		gameObject.AddOrGet<Effects>();
		gameObject.AddOrGet<Traits>();
		gameObject.AddOrGet<AnimEventHandler>();
		MoverLayerOccupier moverLayerOccupier = gameObject.AddOrGet<MoverLayerOccupier>();
		moverLayerOccupier.objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Rover,
			ObjectLayer.Mover
		};
		moverLayerOccupier.cellOffsets = new CellOffset[]
		{
			CellOffset.none,
			new CellOffset(0, 1)
		};
		RobotElectroBankMonitor.Def def = gameObject.AddOrGetDef<RobotElectroBankMonitor.Def>();
		def.lowBatteryWarningPercent = 0.2f;
		def.wattage = 200f;
		gameObject.AddOrGet<FetchDrone>();
		Storage storage = gameObject.AddComponent<Storage>();
		storage.fxPrefix = Storage.FXPrefix.PickedUp;
		storage.dropOnLoad = true;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		gameObject.AddOrGetDef<CreatureDebugGoToMonitor.Def>();
		Deconstructable deconstructable = gameObject.AddOrGet<Deconstructable>();
		deconstructable.enabled = false;
		deconstructable.audioSize = "medium";
		deconstructable.looseEntityDeconstructable = true;
		Storage storage2 = gameObject.AddComponent<Storage>();
		storage2.storageID = GameTags.ChargedPortableBattery;
		storage2.showInUI = true;
		ManualDeliveryKG manualDeliveryKG = gameObject.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage2);
		manualDeliveryKG.RequestedItemTag = GameTags.ChargedPortableBattery;
		manualDeliveryKG.capacity = 1.2f;
		manualDeliveryKG.refillMass = 1.2f;
		manualDeliveryKG.MinimumMass = 1f;
		manualDeliveryKG.MassPerUnit = 20f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		gameObject.AddOrGetDef<RobotElectroBankMonitor.Def>().lowBatteryWarningPercent = 0.2f;
		gameObject.AddOrGetDef<RobotAi.Def>().DeleteOnDead = true;
		ChoreTable.Builder chore_table = new ChoreTable.Builder().Add(new RobotDeathStates.Def
		{
			deathAnim = "idle_dead"
		}, true, Db.Get().ChoreTypes.Die.priority).Add(new DebugGoToStates.Def(), true, -1).Add(new DrowningStates.Def(), true, -1).Add(new IdleStates.Def(), true, Db.Get().ChoreTypes.Idle.priority);
		EntityTemplates.AddCreatureBrain(gameObject, chore_table, GameTags.Robots.Models.FetchDrone, null);
		KPrefabID kprefabID = gameObject.AddOrGet<KPrefabID>();
		kprefabID.RemoveTag(GameTags.CreatureBrain);
		kprefabID.AddTag(GameTags.DupeBrain, false);
		kprefabID.AddTag(GameTags.Robot, false);
		Navigator navigator = gameObject.AddOrGet<Navigator>();
		navigator.NavGridName = "FlyerNavGrid1x1";
		navigator.CurrentNavType = NavType.Hover;
		navigator.defaultSpeed = 2f;
		navigator.updateProber = true;
		navigator.sceneLayer = Grid.SceneLayer.Creatures;
		gameObject.AddOrGet<Sensors>();
		Pickupable pickupable = gameObject.AddOrGet<Pickupable>();
		pickupable.handleFallerComponents = false;
		pickupable.SetWorkTime(5f);
		gameObject.AddOrGet<SnapOn>();
		FetchDroneConfig.SetupLaserEffects(gameObject);
		component.SetSymbolVisiblity("snapto_pivot", false);
		component.SetSymbolVisiblity("snapto_thing", false);
		component.SetSymbolVisiblity("snapTo_chest", false);
		gameObject.AddOrGet<EntombVulnerable>();
		gameObject.AddComponent<OccupyArea>().SetCellOffsets(new CellOffset[]
		{
			CellOffset.none
		});
		gameObject.AddOrGet<DrowningMonitor>();
		gameObject.AddOrGetDef<SubmergedMonitor.Def>();
		gameObject.AddOrGet<Health>();
		SymbolOverrideControllerUtil.AddToPrefab(gameObject);
		return gameObject;
	}

	// Token: 0x060012C5 RID: 4805 RVA: 0x00067600 File Offset: 0x00065800
	private static void SetupLaserEffects(GameObject prefab)
	{
		GameObject gameObject = new GameObject("LaserEffect");
		gameObject.transform.parent = prefab.transform;
		KBatchedAnimEventToggler kbatchedAnimEventToggler = gameObject.AddComponent<KBatchedAnimEventToggler>();
		kbatchedAnimEventToggler.eventSource = prefab;
		kbatchedAnimEventToggler.enableEvent = "LaserOn";
		kbatchedAnimEventToggler.disableEvent = "LaserOff";
		kbatchedAnimEventToggler.entries = new List<KBatchedAnimEventToggler.Entry>();
		FetchDroneConfig.LaserEffect[] array = new FetchDroneConfig.LaserEffect[]
		{
			new FetchDroneConfig.LaserEffect
			{
				id = "DigEffect",
				animFile = "laser_kanim",
				anim = "idle",
				context = "dig"
			},
			new FetchDroneConfig.LaserEffect
			{
				id = "BuildEffect",
				animFile = "construct_beam_kanim",
				anim = "loop",
				context = "build"
			},
			new FetchDroneConfig.LaserEffect
			{
				id = "FetchLiquidEffect",
				animFile = "hose_fx_kanim",
				anim = "loop",
				context = "fetchliquid"
			},
			new FetchDroneConfig.LaserEffect
			{
				id = "PaintEffect",
				animFile = "paint_beam_kanim",
				anim = "loop",
				context = "paint"
			},
			new FetchDroneConfig.LaserEffect
			{
				id = "HarvestEffect",
				animFile = "plant_harvest_beam_kanim",
				anim = "loop",
				context = "harvest"
			},
			new FetchDroneConfig.LaserEffect
			{
				id = "CaptureEffect",
				animFile = "net_gun_fx_kanim",
				anim = "loop",
				context = "capture"
			},
			new FetchDroneConfig.LaserEffect
			{
				id = "AttackEffect",
				animFile = "attack_beam_fx_kanim",
				anim = "loop",
				context = "attack"
			},
			new FetchDroneConfig.LaserEffect
			{
				id = "PickupEffect",
				animFile = "vacuum_fx_kanim",
				anim = "loop",
				context = "pickup"
			},
			new FetchDroneConfig.LaserEffect
			{
				id = "StoreEffect",
				animFile = "vacuum_reverse_fx_kanim",
				anim = "loop",
				context = "store"
			},
			new FetchDroneConfig.LaserEffect
			{
				id = "DisinfectEffect",
				animFile = "plant_spray_beam_kanim",
				anim = "loop",
				context = "disinfect"
			},
			new FetchDroneConfig.LaserEffect
			{
				id = "TendEffect",
				animFile = "plant_tending_beam_fx_kanim",
				anim = "loop",
				context = "tend"
			},
			new FetchDroneConfig.LaserEffect
			{
				id = "PowerTinkerEffect",
				animFile = "electrician_beam_fx_kanim",
				anim = "idle",
				context = "powertinker"
			},
			new FetchDroneConfig.LaserEffect
			{
				id = "SpecialistDigEffect",
				animFile = "senior_miner_beam_fx_kanim",
				anim = "idle",
				context = "specialistdig"
			},
			new FetchDroneConfig.LaserEffect
			{
				id = "DemolishEffect",
				animFile = "poi_demolish_fx_kanim",
				anim = "idle",
				context = "demolish"
			}
		};
		KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
		foreach (FetchDroneConfig.LaserEffect laserEffect in array)
		{
			GameObject gameObject2 = new GameObject(laserEffect.id);
			gameObject2.transform.parent = gameObject.transform;
			gameObject2.AddOrGet<KPrefabID>().PrefabTag = new Tag(laserEffect.id);
			KBatchedAnimTracker kbatchedAnimTracker = gameObject2.AddOrGet<KBatchedAnimTracker>();
			kbatchedAnimTracker.controller = component;
			kbatchedAnimTracker.symbol = new HashedString("snapto_thing");
			kbatchedAnimTracker.offset = new Vector3(40f, 0f, 0f);
			kbatchedAnimTracker.useTargetPoint = true;
			KBatchedAnimController kbatchedAnimController = gameObject2.AddOrGet<KBatchedAnimController>();
			kbatchedAnimController.AnimFiles = new KAnimFile[]
			{
				Assets.GetAnim(laserEffect.animFile)
			};
			KBatchedAnimEventToggler.Entry item = new KBatchedAnimEventToggler.Entry
			{
				anim = laserEffect.anim,
				context = laserEffect.context,
				controller = kbatchedAnimController
			};
			kbatchedAnimEventToggler.entries.Add(item);
			gameObject2.AddOrGet<LoopingSounds>();
		}
	}

	// Token: 0x060012C6 RID: 4806 RVA: 0x00067B40 File Offset: 0x00065D40
	public void OnPrefabInit(GameObject inst)
	{
		ChoreConsumer component = inst.GetComponent<ChoreConsumer>();
		if (component != null)
		{
			component.AddProvider(GlobalChoreProvider.Instance);
		}
	}

	// Token: 0x060012C7 RID: 4807 RVA: 0x00067B68 File Offset: 0x00065D68
	public void OnSpawn(GameObject inst)
	{
		inst.GetComponent<StandardWorker>().fetchOffsets = new CellOffset[]
		{
			CellOffset.up
		};
		Sensors component = inst.GetComponent<Sensors>();
		component.Add(new PathProberSensor(component));
		component.Add(new PickupableSensor(component));
		PathProber component2 = inst.GetComponent<PathProber>();
		if (component2 != null)
		{
			component2.SetGroupProber(MinionGroupProber.Get());
		}
		inst.GetComponent<LoopingSounds>().StartSound(GlobalAssets.GetSound("Flydo_flying_LP", false));
	}

	// Token: 0x04000AE5 RID: 2789
	public const string ID = "FetchDrone";

	// Token: 0x04000AE6 RID: 2790
	public const SimHashes MATERIAL = SimHashes.Steel;

	// Token: 0x04000AE7 RID: 2791
	public const float MASS = 300f;

	// Token: 0x04000AE8 RID: 2792
	private const float WIDTH = 1f;

	// Token: 0x04000AE9 RID: 2793
	private const float HEIGHT = 1f;

	// Token: 0x04000AEA RID: 2794
	private const float CARRY_AMOUNT = 200f;

	// Token: 0x04000AEB RID: 2795
	private const float HIT_POINTS = 50f;

	// Token: 0x04000AEC RID: 2796
	private string name = STRINGS.ROBOTS.MODELS.FLYDO.NAME;

	// Token: 0x04000AED RID: 2797
	private string desc = STRINGS.ROBOTS.MODELS.FLYDO.DESC;

	// Token: 0x0200114B RID: 4427
	public struct LaserEffect
	{
		// Token: 0x04005FAB RID: 24491
		public string id;

		// Token: 0x04005FAC RID: 24492
		public string animFile;

		// Token: 0x04005FAD RID: 24493
		public string anim;

		// Token: 0x04005FAE RID: 24494
		public HashedString context;
	}
}
