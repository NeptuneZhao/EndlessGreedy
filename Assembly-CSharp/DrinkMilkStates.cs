using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using UnityEngine;

// Token: 0x020000CF RID: 207
public class DrinkMilkStates : GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>
{
	// Token: 0x060003C3 RID: 963 RVA: 0x0001EE48 File Offset: 0x0001D048
	private static void SetSceneLayer(DrinkMilkStates.Instance smi, Grid.SceneLayer layer)
	{
		SegmentedCreature.Instance smi2 = smi.GetSMI<SegmentedCreature.Instance>();
		if (smi2 != null && smi2.segments != null)
		{
			using (IEnumerator<SegmentedCreature.CreatureSegment> enumerator = smi2.segments.Reverse<SegmentedCreature.CreatureSegment>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SegmentedCreature.CreatureSegment creatureSegment = enumerator.Current;
					creatureSegment.animController.SetSceneLayer(layer);
				}
				return;
			}
		}
		smi.GetComponent<KBatchedAnimController>().SetSceneLayer(layer);
	}

	// Token: 0x060003C4 RID: 964 RVA: 0x0001EEBC File Offset: 0x0001D0BC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.goingToDrink;
		this.root.Enter(new StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State.Callback(DrinkMilkStates.SetTarget)).Enter(new StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State.Callback(DrinkMilkStates.CheckIfCramped)).Enter(new StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State.Callback(DrinkMilkStates.ReserveMilkFeeder)).Exit(new StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State.Callback(DrinkMilkStates.UnreserveMilkFeeder)).Transition(this.behaviourComplete, delegate(DrinkMilkStates.Instance smi)
		{
			MilkFeeder.Instance instance = DrinkMilkStates.GetTargetMilkFeeder(smi);
			if (instance.IsNullOrDestroyed() || !instance.IsOperational())
			{
				smi.GetComponent<KAnimControllerBase>().Queue("idle_loop", KAnim.PlayMode.Loop, 1f, 0f);
				return true;
			}
			return false;
		}, UpdateRate.SIM_200ms);
		GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State state = this.goingToDrink.MoveTo(new Func<DrinkMilkStates.Instance, int>(DrinkMilkStates.GetCellToDrinkFrom), this.drink, null, false);
		string name = CREATURES.STATUSITEMS.LOOKINGFORMILK.NAME;
		string tooltip = CREATURES.STATUSITEMS.LOOKINGFORMILK.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State state2 = this.drink.DefaultState(this.drink.pre).Enter("FaceMilkFeeder", new StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State.Callback(DrinkMilkStates.FaceMilkFeeder));
		string name2 = CREATURES.STATUSITEMS.DRINKINGMILK.NAME;
		string tooltip2 = CREATURES.STATUSITEMS.DRINKINGMILK.TOOLTIP;
		string icon2 = "";
		StatusItem.IconType icon_type2 = StatusItem.IconType.Info;
		NotificationType notification_type2 = NotificationType.Neutral;
		bool allow_multiples2 = false;
		main = Db.Get().StatusItemCategories.Main;
		state2.ToggleStatusItem(name2, tooltip2, icon2, icon_type2, notification_type2, allow_multiples2, default(HashedString), 129022, null, null, main).Enter(delegate(DrinkMilkStates.Instance smi)
		{
			DrinkMilkStates.SetSceneLayer(smi, smi.def.shouldBeBehindMilkTank ? Grid.SceneLayer.BuildingUse : Grid.SceneLayer.Creatures);
		}).Exit(delegate(DrinkMilkStates.Instance smi)
		{
			DrinkMilkStates.SetSceneLayer(smi, Grid.SceneLayer.Creatures);
		});
		this.drink.pre.QueueAnim(new Func<DrinkMilkStates.Instance, string>(DrinkMilkStates.GetAnimDrinkPre), false, null).OnAnimQueueComplete(this.drink.loop);
		this.drink.loop.QueueAnim(new Func<DrinkMilkStates.Instance, string>(DrinkMilkStates.GetAnimDrinkLoop), true, null).Enter(delegate(DrinkMilkStates.Instance smi)
		{
			MilkFeeder.Instance instance = DrinkMilkStates.GetTargetMilkFeeder(smi);
			if (instance != null)
			{
				instance.RequestToStartFeeding(smi);
				return;
			}
			smi.GoTo(this.drink.pst);
		}).OnSignal(this.requestedToStopFeeding, this.drink.pst);
		this.drink.pst.QueueAnim(new Func<DrinkMilkStates.Instance, string>(DrinkMilkStates.GetAnimDrinkPst), false, null).Enter(new StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State.Callback(DrinkMilkStates.DrinkMilkComplete)).OnAnimQueueComplete(this.behaviourComplete);
		this.behaviourComplete.QueueAnim("idle_loop", true, null).BehaviourComplete(GameTags.Creatures.Behaviour_TryToDrinkMilkFromFeeder, false);
	}

	// Token: 0x060003C5 RID: 965 RVA: 0x0001F130 File Offset: 0x0001D330
	private static MilkFeeder.Instance GetTargetMilkFeeder(DrinkMilkStates.Instance smi)
	{
		if (smi.sm.targetMilkFeeder.IsNullOrDestroyed())
		{
			return null;
		}
		GameObject gameObject = smi.sm.targetMilkFeeder.Get(smi);
		if (gameObject.IsNullOrDestroyed())
		{
			return null;
		}
		MilkFeeder.Instance smi2 = gameObject.GetSMI<MilkFeeder.Instance>();
		if (gameObject.IsNullOrDestroyed() || smi2.IsNullOrStopped())
		{
			return null;
		}
		return smi2;
	}

	// Token: 0x060003C6 RID: 966 RVA: 0x0001F187 File Offset: 0x0001D387
	private static void SetTarget(DrinkMilkStates.Instance smi)
	{
		smi.sm.targetMilkFeeder.Set(smi.GetSMI<DrinkMilkMonitor.Instance>().targetMilkFeeder.gameObject, smi, false);
	}

	// Token: 0x060003C7 RID: 967 RVA: 0x0001F1AC File Offset: 0x0001D3AC
	private static void CheckIfCramped(DrinkMilkStates.Instance smi)
	{
		smi.critterIsCramped = smi.GetSMI<DrinkMilkMonitor.Instance>().doesTargetMilkFeederHaveSpaceForCritter;
	}

	// Token: 0x060003C8 RID: 968 RVA: 0x0001F1C0 File Offset: 0x0001D3C0
	private static void ReserveMilkFeeder(DrinkMilkStates.Instance smi)
	{
		MilkFeeder.Instance instance = DrinkMilkStates.GetTargetMilkFeeder(smi);
		if (instance == null)
		{
			return;
		}
		instance.SetReserved(true);
	}

	// Token: 0x060003C9 RID: 969 RVA: 0x0001F1E0 File Offset: 0x0001D3E0
	private static void UnreserveMilkFeeder(DrinkMilkStates.Instance smi)
	{
		MilkFeeder.Instance instance = DrinkMilkStates.GetTargetMilkFeeder(smi);
		if (instance == null)
		{
			return;
		}
		instance.SetReserved(false);
	}

	// Token: 0x060003CA RID: 970 RVA: 0x0001F200 File Offset: 0x0001D400
	private static void DrinkMilkComplete(DrinkMilkStates.Instance smi)
	{
		MilkFeeder.Instance instance = DrinkMilkStates.GetTargetMilkFeeder(smi);
		if (instance == null)
		{
			return;
		}
		smi.GetSMI<DrinkMilkMonitor.Instance>().NotifyFinishedDrinkingMilkFrom(instance);
	}

	// Token: 0x060003CB RID: 971 RVA: 0x0001F224 File Offset: 0x0001D424
	private static int GetCellToDrinkFrom(DrinkMilkStates.Instance smi)
	{
		MilkFeeder.Instance instance = DrinkMilkStates.GetTargetMilkFeeder(smi);
		if (instance == null)
		{
			return Grid.InvalidCell;
		}
		return smi.GetSMI<DrinkMilkMonitor.Instance>().GetDrinkCellOf(instance, smi.critterIsCramped);
	}

	// Token: 0x060003CC RID: 972 RVA: 0x0001F253 File Offset: 0x0001D453
	private static string GetAnimDrinkPre(DrinkMilkStates.Instance smi)
	{
		if (smi.critterIsCramped)
		{
			return "drink_cramped_pre";
		}
		return "drink_pre";
	}

	// Token: 0x060003CD RID: 973 RVA: 0x0001F268 File Offset: 0x0001D468
	private static string GetAnimDrinkLoop(DrinkMilkStates.Instance smi)
	{
		if (smi.critterIsCramped)
		{
			return "drink_cramped_loop";
		}
		return "drink_loop";
	}

	// Token: 0x060003CE RID: 974 RVA: 0x0001F27D File Offset: 0x0001D47D
	private static string GetAnimDrinkPst(DrinkMilkStates.Instance smi)
	{
		if (smi.critterIsCramped)
		{
			return "drink_cramped_pst";
		}
		return "drink_pst";
	}

	// Token: 0x060003CF RID: 975 RVA: 0x0001F294 File Offset: 0x0001D494
	private static void FaceMilkFeeder(DrinkMilkStates.Instance smi)
	{
		MilkFeeder.Instance instance = DrinkMilkStates.GetTargetMilkFeeder(smi);
		if (instance == null)
		{
			return;
		}
		bool isRotated = instance.GetComponent<Rotatable>().IsRotated;
		float num;
		if (smi.critterIsCramped)
		{
			if (isRotated)
			{
				num = -20f;
			}
			else
			{
				num = 20f;
			}
		}
		else if (isRotated)
		{
			num = 20f;
		}
		else
		{
			num = -20f;
		}
		IApproachable approachable = smi.sm.targetMilkFeeder.Get<IApproachable>(smi);
		if (approachable == null)
		{
			return;
		}
		float target_x = approachable.transform.GetPosition().x + num;
		smi.GetComponent<Facing>().Face(target_x);
	}

	// Token: 0x04000298 RID: 664
	public GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State goingToDrink;

	// Token: 0x04000299 RID: 665
	public DrinkMilkStates.EatingState drink;

	// Token: 0x0400029A RID: 666
	public GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State behaviourComplete;

	// Token: 0x0400029B RID: 667
	public StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.TargetParameter targetMilkFeeder;

	// Token: 0x0400029C RID: 668
	public StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.Signal requestedToStopFeeding;

	// Token: 0x02001031 RID: 4145
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06007B6D RID: 31597 RVA: 0x00303897 File Offset: 0x00301A97
		public static CellOffset DrinkCellOffsetGet_CritterOneByOne(MilkFeeder.Instance milkFeederInstance, DrinkMilkMonitor.Instance critterInstance, bool isCramped)
		{
			return milkFeederInstance.GetComponent<Rotatable>().GetRotatedCellOffset(MilkFeederConfig.DRINK_FROM_OFFSET);
		}

		// Token: 0x06007B6E RID: 31598 RVA: 0x003038AC File Offset: 0x00301AAC
		public static CellOffset DrinkCellOffsetGet_GassyMoo(MilkFeeder.Instance milkFeederInstance, DrinkMilkMonitor.Instance critterInstance, bool isCramped)
		{
			Rotatable component = milkFeederInstance.GetComponent<Rotatable>();
			CellOffset rotatedCellOffset = component.GetRotatedCellOffset(MilkFeederConfig.DRINK_FROM_OFFSET);
			if (component.IsRotated)
			{
				rotatedCellOffset.x--;
			}
			if (isCramped)
			{
				if (component.IsRotated)
				{
					rotatedCellOffset.x += 2;
				}
				else
				{
					rotatedCellOffset.x -= 2;
				}
			}
			return rotatedCellOffset;
		}

		// Token: 0x06007B6F RID: 31599 RVA: 0x00303908 File Offset: 0x00301B08
		public static CellOffset DrinkCellOffsetGet_BammothAdult(MilkFeeder.Instance milkFeederInstance, DrinkMilkMonitor.Instance critterInstance, bool isCramped)
		{
			Rotatable component = milkFeederInstance.GetComponent<Rotatable>();
			CellOffset rotatedCellOffset = component.GetRotatedCellOffset(MilkFeederConfig.DRINK_FROM_OFFSET);
			if (!isCramped)
			{
				int x = Grid.CellToXY(Grid.OffsetCell(Grid.PosToCell(milkFeederInstance), rotatedCellOffset)).x;
				int x2 = Grid.PosToXY(critterInstance.transform.position).x;
				if (x > x2 && !component.IsRotated)
				{
					rotatedCellOffset.x++;
				}
				else if (x < x2 && component.IsRotated)
				{
					rotatedCellOffset.x--;
				}
				else if (x == x2)
				{
					if (component.IsRotated)
					{
						rotatedCellOffset.x--;
					}
					else
					{
						rotatedCellOffset.x++;
					}
				}
			}
			else if (component.IsRotated)
			{
				rotatedCellOffset.x++;
			}
			else
			{
				rotatedCellOffset.x--;
			}
			return rotatedCellOffset;
		}

		// Token: 0x04005C60 RID: 23648
		public bool shouldBeBehindMilkTank = true;

		// Token: 0x04005C61 RID: 23649
		public DrinkMilkStates.Def.DrinkCellOffsetGetFn drinkCellOffsetGetFn = new DrinkMilkStates.Def.DrinkCellOffsetGetFn(DrinkMilkStates.Def.DrinkCellOffsetGet_CritterOneByOne);

		// Token: 0x02002382 RID: 9090
		// (Invoke) Token: 0x0600B6C6 RID: 46790
		public delegate CellOffset DrinkCellOffsetGetFn(MilkFeeder.Instance milkFeederInstance, DrinkMilkMonitor.Instance critterInstance, bool isCramped);
	}

	// Token: 0x02001032 RID: 4146
	public new class Instance : GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.GameInstance
	{
		// Token: 0x06007B71 RID: 31601 RVA: 0x003039FB File Offset: 0x00301BFB
		public Instance(Chore<DrinkMilkStates.Instance> chore, DrinkMilkStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Behaviour_TryToDrinkMilkFromFeeder);
		}

		// Token: 0x06007B72 RID: 31602 RVA: 0x00303A1F File Offset: 0x00301C1F
		public void RequestToStopFeeding()
		{
			base.sm.requestedToStopFeeding.Trigger(base.smi);
		}

		// Token: 0x04005C62 RID: 23650
		public bool critterIsCramped;
	}

	// Token: 0x02001033 RID: 4147
	public class EatingState : GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State
	{
		// Token: 0x04005C63 RID: 23651
		public GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State pre;

		// Token: 0x04005C64 RID: 23652
		public GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State loop;

		// Token: 0x04005C65 RID: 23653
		public GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State pst;
	}
}
