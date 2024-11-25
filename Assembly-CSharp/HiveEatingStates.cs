using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020000D9 RID: 217
public class HiveEatingStates : GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>
{
	// Token: 0x060003F7 RID: 1015 RVA: 0x00020408 File Offset: 0x0001E608
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.eating;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.State state = this.eating;
		string name = CREATURES.STATUSITEMS.HIVE_DIGESTING.NAME;
		string tooltip = CREATURES.STATUSITEMS.HIVE_DIGESTING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).DefaultState(this.eating.pre).Enter(delegate(HiveEatingStates.Instance smi)
		{
			smi.TurnOn();
		}).Exit(delegate(HiveEatingStates.Instance smi)
		{
			smi.TurnOff();
		});
		this.eating.pre.PlayAnim("eating_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.eating.loop);
		this.eating.loop.PlayAnim("eating_loop", KAnim.PlayMode.Loop).Update(delegate(HiveEatingStates.Instance smi, float dt)
		{
			smi.EatOreFromStorage(smi, dt);
		}, UpdateRate.SIM_4000ms, false).EventTransition(GameHashes.OnStorageChange, this.eating.pst, (HiveEatingStates.Instance smi) => !smi.storage.FindFirst(smi.def.consumedOre));
		this.eating.pst.PlayAnim("eating_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToEat, false);
	}

	// Token: 0x040002BA RID: 698
	public HiveEatingStates.EatingStates eating;

	// Token: 0x040002BB RID: 699
	public GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.State behaviourcomplete;

	// Token: 0x02001051 RID: 4177
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06007BB2 RID: 31666 RVA: 0x00303FD0 File Offset: 0x003021D0
		public Def(Tag consumedOre)
		{
			this.consumedOre = consumedOre;
		}

		// Token: 0x04005C8E RID: 23694
		public Tag consumedOre;
	}

	// Token: 0x02001052 RID: 4178
	public class EatingStates : GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.State
	{
		// Token: 0x04005C8F RID: 23695
		public GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.State pre;

		// Token: 0x04005C90 RID: 23696
		public GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.State loop;

		// Token: 0x04005C91 RID: 23697
		public GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.State pst;
	}

	// Token: 0x02001053 RID: 4179
	public new class Instance : GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.GameInstance
	{
		// Token: 0x06007BB4 RID: 31668 RVA: 0x00303FE7 File Offset: 0x003021E7
		public Instance(Chore<HiveEatingStates.Instance> chore, HiveEatingStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToEat);
		}

		// Token: 0x06007BB5 RID: 31669 RVA: 0x0030400B File Offset: 0x0030220B
		public void TurnOn()
		{
			this.emitter.emitRads = 600f * this.emitter.emitRate;
			this.emitter.Refresh();
		}

		// Token: 0x06007BB6 RID: 31670 RVA: 0x00304034 File Offset: 0x00302234
		public void TurnOff()
		{
			this.emitter.emitRads = 0f;
			this.emitter.Refresh();
		}

		// Token: 0x06007BB7 RID: 31671 RVA: 0x00304054 File Offset: 0x00302254
		public void EatOreFromStorage(HiveEatingStates.Instance smi, float dt)
		{
			GameObject gameObject = smi.storage.FindFirst(smi.def.consumedOre);
			if (!gameObject)
			{
				return;
			}
			float num = 0.25f;
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (component == null)
			{
				return;
			}
			PrimaryElement component2 = component.GetComponent<PrimaryElement>();
			if (component2 == null)
			{
				return;
			}
			Diet.Info dietInfo = smi.GetSMI<BeehiveCalorieMonitor.Instance>().stomach.diet.GetDietInfo(component.PrefabTag);
			if (dietInfo == null)
			{
				return;
			}
			AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(smi.gameObject);
			float calories = amountInstance.GetMax() - amountInstance.value;
			float num2 = dietInfo.ConvertCaloriesToConsumptionMass(calories);
			float num3 = num * dt;
			if (num2 < num3)
			{
				num3 = num2;
			}
			num3 = Mathf.Min(num3, component2.Mass);
			component2.Mass -= num3;
			Pickupable component3 = component2.GetComponent<Pickupable>();
			if (component3.storage != null)
			{
				component3.storage.Trigger(-1452790913, smi.gameObject);
				component3.storage.Trigger(-1697596308, smi.gameObject);
			}
			float calories2 = dietInfo.ConvertConsumptionMassToCalories(num3);
			CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = new CreatureCalorieMonitor.CaloriesConsumedEvent
			{
				tag = component.PrefabTag,
				calories = calories2
			};
			smi.gameObject.Trigger(-2038961714, caloriesConsumedEvent);
		}

		// Token: 0x04005C92 RID: 23698
		[MyCmpReq]
		public Storage storage;

		// Token: 0x04005C93 RID: 23699
		[MyCmpReq]
		private RadiationEmitter emitter;
	}
}
