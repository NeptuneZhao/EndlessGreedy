using System;
using System.Diagnostics;
using Klei.AI;
using UnityEngine;

// Token: 0x02000554 RID: 1364
public class SolidConsumerMonitor : GameStateMachine<SolidConsumerMonitor, SolidConsumerMonitor.Instance, IStateMachineTarget, SolidConsumerMonitor.Def>
{
	// Token: 0x06001F49 RID: 8009 RVA: 0x000AF5A4 File Offset: 0x000AD7A4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.EventHandler(GameHashes.EatSolidComplete, delegate(SolidConsumerMonitor.Instance smi, object data)
		{
			smi.OnEatSolidComplete(data);
		}).ToggleBehaviour(GameTags.Creatures.WantsToEat, (SolidConsumerMonitor.Instance smi) => smi.targetEdible != null && !smi.targetEdible.HasTag(GameTags.Creatures.ReservedByCreature), null);
		this.satisfied.TagTransition(GameTags.Creatures.Hungry, this.lookingforfood, false);
		this.lookingforfood.TagTransition(GameTags.Creatures.Hungry, this.satisfied, true).PreBrainUpdate(new Action<SolidConsumerMonitor.Instance>(SolidConsumerMonitor.FindFood));
	}

	// Token: 0x06001F4A RID: 8010 RVA: 0x000AF654 File Offset: 0x000AD854
	[Conditional("DETAILED_SOLID_CONSUMER_MONITOR_PROFILE")]
	private static void BeginDetailedSample(string region_name)
	{
	}

	// Token: 0x06001F4B RID: 8011 RVA: 0x000AF656 File Offset: 0x000AD856
	[Conditional("DETAILED_SOLID_CONSUMER_MONITOR_PROFILE")]
	private static void EndDetailedSample(string region_name)
	{
	}

	// Token: 0x06001F4C RID: 8012 RVA: 0x000AF658 File Offset: 0x000AD858
	private static void FindFood(SolidConsumerMonitor.Instance smi)
	{
		if (smi.IsTargetEdibleValid())
		{
			return;
		}
		smi.ClearTargetEdible();
		Diet diet = smi.diet;
		int num = 0;
		int num2 = 0;
		Grid.PosToXY(smi.gameObject.transform.GetPosition(), out num, out num2);
		num -= 8;
		num2 -= 8;
		bool flag = false;
		if (!diet.AllConsumablesAreDirectlyEdiblePlants)
		{
			ListPool<Storage, SolidConsumerMonitor>.PooledList pooledList = ListPool<Storage, SolidConsumerMonitor>.Allocate();
			int num3 = 32;
			foreach (CreatureFeeder creatureFeeder in Components.CreatureFeeders.GetItems(smi.GetMyWorldId()))
			{
				Vector2I targetFeederCell = creatureFeeder.GetTargetFeederCell();
				if (targetFeederCell.x >= num && targetFeederCell.x <= num + num3 && targetFeederCell.y >= num2 && targetFeederCell.y <= num2 + num3 && !creatureFeeder.StoragesAreEmpty())
				{
					int cost = smi.GetCost(Grid.XYToCell(targetFeederCell.x, targetFeederCell.y));
					if (smi.IsCloserThanTargetEdible(cost))
					{
						foreach (Storage storage in creatureFeeder.storages)
						{
							if (!(storage == null) && !storage.IsEmpty() && smi.GetCost(Grid.PosToCell(storage.items[0])) != -1)
							{
								foreach (GameObject gameObject in storage.items)
								{
									if (!(gameObject == null))
									{
										KPrefabID component = gameObject.GetComponent<KPrefabID>();
										if (!component.HasAnyTags(SolidConsumerMonitor.creatureTags) && diet.GetDietInfo(component.PrefabTag) != null)
										{
											smi.SetTargetEdible(gameObject, cost);
											smi.targetEdibleOffset = Vector3.zero;
											flag = true;
											break;
										}
									}
								}
								if (flag)
								{
									break;
								}
							}
						}
					}
				}
			}
			pooledList.Recycle();
		}
		bool flag2 = false;
		if (diet.CanEatAnyPlantDirectly)
		{
			ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList pooledList2 = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
			GameScenePartitioner.Instance.GatherEntries(num, num2, 16, 16, GameScenePartitioner.Instance.plants, pooledList2);
			foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList2)
			{
				KPrefabID kprefabID = (KPrefabID)scenePartitionerEntry.obj;
				Diet.Info dietInfo = diet.GetDietInfo(kprefabID.PrefabTag);
				Vector3 vector = kprefabID.transform.GetPosition();
				bool flag3 = kprefabID.HasTag(GameTags.PlantedOnFloorVessel);
				if (flag3)
				{
					vector += SolidConsumerMonitor.PLANT_ON_FLOOR_VESSEL_OFFSET;
				}
				int num4 = smi.GetCost(Grid.PosToCell(vector));
				Vector3 a = Vector3.zero;
				if (smi.IsCloserThanTargetEdible(num4) && !kprefabID.HasAnyTags(SolidConsumerMonitor.creatureTags) && dietInfo != null)
				{
					if (kprefabID.HasTag(GameTags.Plant))
					{
						IPlantConsumptionInstructions[] plantConsumptionInstructions = GameUtil.GetPlantConsumptionInstructions(kprefabID.gameObject);
						if (plantConsumptionInstructions == null || plantConsumptionInstructions.Length == 0)
						{
							continue;
						}
						bool flag4 = false;
						foreach (IPlantConsumptionInstructions plantConsumptionInstructions2 in plantConsumptionInstructions)
						{
							if (plantConsumptionInstructions2.CanPlantBeEaten() && dietInfo.foodType == plantConsumptionInstructions2.GetDietFoodType())
							{
								CellOffset[] allowedOffsets = plantConsumptionInstructions2.GetAllowedOffsets();
								if (allowedOffsets != null)
								{
									num4 = -1;
									foreach (CellOffset offset in allowedOffsets)
									{
										int cost2 = smi.GetCost(Grid.OffsetCell(Grid.PosToCell(vector), offset));
										if (cost2 != -1 && (num4 == -1 || cost2 < num4))
										{
											num4 = cost2;
											a = offset.ToVector3();
										}
									}
									if (num4 != -1)
									{
										flag4 = true;
										break;
									}
								}
								else
								{
									flag4 = true;
								}
							}
						}
						if (!flag4)
						{
							continue;
						}
					}
					smi.SetTargetEdible(kprefabID.gameObject, num4);
					smi.targetEdibleOffset = a + (flag3 ? SolidConsumerMonitor.PLANT_ON_FLOOR_VESSEL_OFFSET : Vector3.zero);
					flag2 = true;
				}
			}
			pooledList2.Recycle();
		}
		if (!flag2 && diet.CanEatAnyNonDirectlyEdiblePlant && smi.CanSearchForPickupables(flag))
		{
			bool flag5 = false;
			ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList pooledList3 = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
			GameScenePartitioner.Instance.GatherEntries(num, num2, 16, 16, GameScenePartitioner.Instance.pickupablesLayer, pooledList3);
			foreach (ScenePartitionerEntry scenePartitionerEntry2 in pooledList3)
			{
				Pickupable pickupable = (Pickupable)scenePartitionerEntry2.obj;
				KPrefabID kprefabID2 = pickupable.KPrefabID;
				if (!kprefabID2.HasAnyTags(SolidConsumerMonitor.creatureTags) && diet.GetDietInfo(kprefabID2.PrefabTag) != null)
				{
					bool flag6;
					smi.ProcessEdible(pickupable.gameObject, out flag6);
					smi.targetEdibleOffset = Vector3.zero;
					flag5 = (flag5 || flag6);
				}
			}
			pooledList3.Recycle();
		}
	}

	// Token: 0x040011A3 RID: 4515
	public static Vector3 PLANT_ON_FLOOR_VESSEL_OFFSET = Vector3.down;

	// Token: 0x040011A4 RID: 4516
	private GameStateMachine<SolidConsumerMonitor, SolidConsumerMonitor.Instance, IStateMachineTarget, SolidConsumerMonitor.Def>.State satisfied;

	// Token: 0x040011A5 RID: 4517
	private GameStateMachine<SolidConsumerMonitor, SolidConsumerMonitor.Instance, IStateMachineTarget, SolidConsumerMonitor.Def>.State lookingforfood;

	// Token: 0x040011A6 RID: 4518
	private static Tag[] creatureTags = new Tag[]
	{
		GameTags.Creatures.ReservedByCreature,
		GameTags.CreatureBrain
	};

	// Token: 0x02001349 RID: 4937
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006623 RID: 26147
		public Diet diet;
	}

	// Token: 0x0200134A RID: 4938
	public new class Instance : GameStateMachine<SolidConsumerMonitor, SolidConsumerMonitor.Instance, IStateMachineTarget, SolidConsumerMonitor.Def>.GameInstance
	{
		// Token: 0x0600867C RID: 34428 RVA: 0x0032934B File Offset: 0x0032754B
		public Instance(IStateMachineTarget master, SolidConsumerMonitor.Def def) : base(master, def)
		{
			this.diet = DietManager.Instance.GetPrefabDiet(base.gameObject);
		}

		// Token: 0x0600867D RID: 34429 RVA: 0x0032936B File Offset: 0x0032756B
		public bool CanSearchForPickupables(bool foodAtFeeder)
		{
			return !foodAtFeeder;
		}

		// Token: 0x0600867E RID: 34430 RVA: 0x00329371 File Offset: 0x00327571
		public bool IsCloserThanTargetEdible(int cost)
		{
			return cost != -1 && (cost < this.targetEdibleCost || this.targetEdibleCost == -1);
		}

		// Token: 0x0600867F RID: 34431 RVA: 0x00329390 File Offset: 0x00327590
		public bool IsTargetEdibleValid()
		{
			if (this.targetEdible == null || this.targetEdible.HasTag(GameTags.Creatures.ReservedByCreature))
			{
				return false;
			}
			int cost = this.GetCost(Grid.PosToCell(this.targetEdible.transform.GetPosition() + this.targetEdibleOffset));
			return cost != -1 && this.targetEdibleCost <= cost + 4;
		}

		// Token: 0x06008680 RID: 34432 RVA: 0x003293F8 File Offset: 0x003275F8
		public void ClearTargetEdible()
		{
			this.targetEdibleCost = -1;
			this.targetEdible = null;
			this.targetEdibleOffset = Vector3.zero;
		}

		// Token: 0x06008681 RID: 34433 RVA: 0x00329414 File Offset: 0x00327614
		public bool ProcessEdible(GameObject edible, out bool isReachable)
		{
			int cost = this.GetCost(edible);
			isReachable = (cost != -1);
			if (cost != -1 && (cost < this.targetEdibleCost || this.targetEdibleCost == -1))
			{
				this.targetEdibleCost = cost;
				this.targetEdible = edible.gameObject;
				return true;
			}
			return false;
		}

		// Token: 0x06008682 RID: 34434 RVA: 0x0032945E File Offset: 0x0032765E
		public void SetTargetEdible(GameObject gameObject, int cost)
		{
			this.targetEdibleCost = cost;
			this.targetEdible = gameObject;
		}

		// Token: 0x06008683 RID: 34435 RVA: 0x0032946E File Offset: 0x0032766E
		public int GetCost(GameObject edible)
		{
			return this.GetCost(Grid.PosToCell(edible.transform.GetPosition()));
		}

		// Token: 0x06008684 RID: 34436 RVA: 0x00329488 File Offset: 0x00327688
		public int GetCost(int cell)
		{
			if (this.drowningMonitor != null && this.drowningMonitor.canDrownToDeath && !this.drowningMonitor.livesUnderWater && !this.drowningMonitor.IsCellSafe(cell))
			{
				return -1;
			}
			return this.navigator.GetNavigationCost(cell);
		}

		// Token: 0x06008685 RID: 34437 RVA: 0x003294E0 File Offset: 0x003276E0
		public void OnEatSolidComplete(object data)
		{
			KPrefabID kprefabID = data as KPrefabID;
			if (kprefabID == null)
			{
				return;
			}
			PrimaryElement component = kprefabID.GetComponent<PrimaryElement>();
			if (component == null)
			{
				return;
			}
			Diet.Info dietInfo = this.diet.GetDietInfo(kprefabID.PrefabTag);
			if (dietInfo == null)
			{
				return;
			}
			AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(base.smi.gameObject);
			string properName = kprefabID.GetProperName();
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, properName, kprefabID.transform, 1.5f, false);
			float calories = amountInstance.GetMax() - amountInstance.value;
			float num = dietInfo.ConvertCaloriesToConsumptionMass(calories);
			IPlantConsumptionInstructions plantConsumptionInstructions = null;
			foreach (IPlantConsumptionInstructions plantConsumptionInstructions3 in GameUtil.GetPlantConsumptionInstructions(kprefabID.gameObject))
			{
				if (dietInfo.foodType == plantConsumptionInstructions3.GetDietFoodType())
				{
					plantConsumptionInstructions = plantConsumptionInstructions3;
				}
			}
			if (plantConsumptionInstructions != null)
			{
				num = plantConsumptionInstructions.ConsumePlant(num);
			}
			else
			{
				num = Mathf.Min(num, component.Mass);
				component.Mass -= num;
				Pickupable component2 = component.GetComponent<Pickupable>();
				if (component2.storage != null)
				{
					component2.storage.Trigger(-1452790913, base.gameObject);
					component2.storage.Trigger(-1697596308, base.gameObject);
				}
			}
			float calories2 = dietInfo.ConvertConsumptionMassToCalories(num);
			CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = new CreatureCalorieMonitor.CaloriesConsumedEvent
			{
				tag = kprefabID.PrefabTag,
				calories = calories2
			};
			base.Trigger(-2038961714, caloriesConsumedEvent);
			this.targetEdible = null;
		}

		// Token: 0x06008686 RID: 34438 RVA: 0x0032967F File Offset: 0x0032787F
		public string[] GetTargetEdibleEatAnims()
		{
			return this.diet.GetDietInfo(this.targetEdible.PrefabID()).eatAnims;
		}

		// Token: 0x04006624 RID: 26148
		private const int RECALC_THRESHOLD = 4;

		// Token: 0x04006625 RID: 26149
		public GameObject targetEdible;

		// Token: 0x04006626 RID: 26150
		public Vector3 targetEdibleOffset;

		// Token: 0x04006627 RID: 26151
		private int targetEdibleCost;

		// Token: 0x04006628 RID: 26152
		[MyCmpGet]
		private Navigator navigator;

		// Token: 0x04006629 RID: 26153
		[MyCmpGet]
		private DrowningMonitor drowningMonitor;

		// Token: 0x0400662A RID: 26154
		public Diet diet;
	}
}
