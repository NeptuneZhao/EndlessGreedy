using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020006D4 RID: 1748
public class FoodDehydrator : GameStateMachine<FoodDehydrator, FoodDehydrator.StatesInstance, IStateMachineTarget, FoodDehydrator.Def>
{
	// Token: 0x06002C4A RID: 11338 RVA: 0x000F8B00 File Offset: 0x000F6D00
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		this.waitingForFuelStatus.resolveStringCallback = ((string str, object obj) => string.Format(str, FOODDEHYDRATORTUNING.FUEL_TAG.ProperName(), GameUtil.GetFormattedMass(5.0000005f, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")));
		default_state = this.waitingForFuel;
		this.waitingForFuel.Enter(delegate(FoodDehydrator.StatesInstance smi)
		{
			smi.operational.SetFlag(FoodDehydrator.foodDehydratorFlag, false);
		}).EventTransition(GameHashes.OnStorageChange, this.working, (FoodDehydrator.StatesInstance smi) => smi.GetAvailableFuel() >= 5.0000005f).ToggleStatusItem(this.waitingForFuelStatus, null);
		this.working.Enter(delegate(FoodDehydrator.StatesInstance smi)
		{
			smi.complexFabricator.SetQueueDirty();
			smi.operational.SetFlag(FoodDehydrator.foodDehydratorFlag, true);
		}).EventHandler(GameHashes.FabricatorOrdersUpdated, delegate(FoodDehydrator.StatesInstance smi)
		{
			smi.UpdateFoodSymbol();
		}).EnterTransition(this.requestEmpty, (FoodDehydrator.StatesInstance smi) => smi.RequiresEmptying()).EventTransition(GameHashes.OnStorageChange, this.waitingForFuel, (FoodDehydrator.StatesInstance smi) => smi.GetAvailableFuel() <= 0f).EventHandlerTransition(GameHashes.FabricatorOrderCompleted, this.requestEmpty, (FoodDehydrator.StatesInstance smi, object data) => smi.RequiresEmptying()).EventHandler(GameHashes.FabricatorOrderStarted, delegate(FoodDehydrator.StatesInstance smi)
		{
			smi.UpdateFoodSymbol();
		});
		this.requestEmpty.ToggleRecurringChore(new Func<FoodDehydrator.StatesInstance, Chore>(this.CreateChore), (FoodDehydrator.StatesInstance smi) => smi.RequiresEmptying()).EventHandlerTransition(GameHashes.OnStorageChange, this.working, (FoodDehydrator.StatesInstance smi, object data) => !smi.RequiresEmptying()).Enter(delegate(FoodDehydrator.StatesInstance smi)
		{
			smi.operational.SetFlag(FoodDehydrator.foodDehydratorFlag, false);
			smi.UpdateFoodSymbol();
		}).ToggleStatusItem(Db.Get().BuildingStatusItems.AwaitingEmptyBuilding, null);
	}

	// Token: 0x06002C4B RID: 11339 RVA: 0x000F8D4C File Offset: 0x000F6F4C
	private Chore CreateChore(FoodDehydrator.StatesInstance smi)
	{
		WorkChore<FoodDehydratorWorkableEmpty> workChore = new WorkChore<FoodDehydratorWorkableEmpty>(Db.Get().ChoreTypes.FoodFetch, smi.master.GetComponent<FoodDehydratorWorkableEmpty>(), null, true, new Action<Chore>(smi.OnEmptyComplete), null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		workChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
		return workChore;
	}

	// Token: 0x0400198F RID: 6543
	private StatusItem waitingForFuelStatus = new StatusItem("waitingForFuelStatus", BUILDING.STATUSITEMS.ENOUGH_FUEL.NAME, BUILDING.STATUSITEMS.ENOUGH_FUEL.TOOLTIP, "status_item_no_gas_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);

	// Token: 0x04001990 RID: 6544
	private static readonly Operational.Flag foodDehydratorFlag = new Operational.Flag("food_dehydrator", Operational.Flag.Type.Requirement);

	// Token: 0x04001991 RID: 6545
	private GameStateMachine<FoodDehydrator, FoodDehydrator.StatesInstance, IStateMachineTarget, FoodDehydrator.Def>.State waitingForFuel;

	// Token: 0x04001992 RID: 6546
	private GameStateMachine<FoodDehydrator, FoodDehydrator.StatesInstance, IStateMachineTarget, FoodDehydrator.Def>.State working;

	// Token: 0x04001993 RID: 6547
	private GameStateMachine<FoodDehydrator, FoodDehydrator.StatesInstance, IStateMachineTarget, FoodDehydrator.Def>.State requestEmpty;

	// Token: 0x020014DD RID: 5341
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06008C53 RID: 35923 RVA: 0x0033A448 File Offset: 0x00338648
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> list = new List<Descriptor>();
			Descriptor item = new Descriptor(UI.BUILDINGEFFECTS.FOOD_DEHYDRATOR_WATER_OUTPUT, UI.BUILDINGEFFECTS.TOOLTIPS.FOOD_DEHYDRATOR_WATER_OUTPUT, Descriptor.DescriptorType.Effect, false);
			list.Add(item);
			return list;
		}
	}

	// Token: 0x020014DE RID: 5342
	public class StatesInstance : GameStateMachine<FoodDehydrator, FoodDehydrator.StatesInstance, IStateMachineTarget, FoodDehydrator.Def>.GameInstance
	{
		// Token: 0x06008C55 RID: 35925 RVA: 0x0033A486 File Offset: 0x00338686
		public StatesInstance(IStateMachineTarget master, FoodDehydrator.Def def) : base(master, def)
		{
			this.SetupFoodSymbol();
		}

		// Token: 0x06008C56 RID: 35926 RVA: 0x0033A496 File Offset: 0x00338696
		public float GetAvailableFuel()
		{
			return this.complexFabricator.inStorage.GetMassAvailable(FOODDEHYDRATORTUNING.FUEL_TAG);
		}

		// Token: 0x06008C57 RID: 35927 RVA: 0x0033A4AD File Offset: 0x003386AD
		public bool RequiresEmptying()
		{
			return !this.complexFabricator.outStorage.IsEmpty();
		}

		// Token: 0x06008C58 RID: 35928 RVA: 0x0033A4C4 File Offset: 0x003386C4
		public void OnEmptyComplete(Chore obj)
		{
			Vector3 position = Grid.CellToPosLCC(Grid.PosToCell(this), Grid.SceneLayer.Ore);
			this.complexFabricator.outStorage.DropAll(position, false, true, default(Vector3), true, null);
		}

		// Token: 0x06008C59 RID: 35929 RVA: 0x0033A500 File Offset: 0x00338700
		public void SetupFoodSymbol()
		{
			GameObject gameObject = Util.NewGameObject(base.gameObject, "food_symbol");
			gameObject.SetActive(false);
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			bool flag;
			Vector3 position = component.GetSymbolTransform(FoodDehydrator.StatesInstance.HASH_FOOD, out flag).GetColumn(3);
			position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
			gameObject.transform.SetPosition(position);
			this.foodKBAC = gameObject.AddComponent<KBatchedAnimController>();
			this.foodKBAC.AnimFiles = new KAnimFile[]
			{
				Assets.GetAnim("mushbar_kanim")
			};
			this.foodKBAC.initialAnim = "object";
			component.SetSymbolVisiblity(FoodDehydrator.StatesInstance.HASH_FOOD, false);
			this.foodKBAC.sceneLayer = Grid.SceneLayer.BuildingUse;
			KBatchedAnimTracker kbatchedAnimTracker = gameObject.AddComponent<KBatchedAnimTracker>();
			kbatchedAnimTracker.symbol = new HashedString("food");
			kbatchedAnimTracker.offset = Vector3.zero;
		}

		// Token: 0x06008C5A RID: 35930 RVA: 0x0033A5E8 File Offset: 0x003387E8
		public void UpdateFoodSymbol()
		{
			ComplexRecipe currentWorkingOrder = this.complexFabricator.CurrentWorkingOrder;
			if (this.complexFabricator.CurrentWorkingOrder != null)
			{
				this.foodKBAC.gameObject.SetActive(true);
				GameObject prefab = Assets.GetPrefab(currentWorkingOrder.ingredients[this.foodIngredientIdx].material);
				this.foodKBAC.SwapAnims(prefab.GetComponent<KBatchedAnimController>().AnimFiles);
				this.foodKBAC.Play("object", KAnim.PlayMode.Loop, 1f, 0f);
				return;
			}
			if (this.complexFabricator.outStorage.items.Count > 0)
			{
				this.foodKBAC.gameObject.SetActive(true);
				this.foodKBAC.SwapAnims(this.complexFabricator.outStorage.items[0].GetComponent<KBatchedAnimController>().AnimFiles);
				this.foodKBAC.Play("object", KAnim.PlayMode.Loop, 1f, 0f);
				return;
			}
			this.foodKBAC.gameObject.SetActive(false);
		}

		// Token: 0x04006B26 RID: 27430
		[MyCmpReq]
		public Operational operational;

		// Token: 0x04006B27 RID: 27431
		[MyCmpReq]
		public ComplexFabricator complexFabricator;

		// Token: 0x04006B28 RID: 27432
		private static string HASH_FOOD = "food";

		// Token: 0x04006B29 RID: 27433
		private KBatchedAnimController foodKBAC;

		// Token: 0x04006B2A RID: 27434
		private int foodIngredientIdx;
	}
}
