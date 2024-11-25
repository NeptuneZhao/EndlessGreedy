using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000EB RID: 235
public class MoveToLureStates : GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>
{
	// Token: 0x0600043C RID: 1084 RVA: 0x00021E78 File Offset: 0x00020078
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.move;
		GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.CONSIDERINGLURE.NAME;
		string tooltip = CREATURES.STATUSITEMS.CONSIDERINGLURE.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.move.MoveTo(new Func<MoveToLureStates.Instance, int>(MoveToLureStates.GetLureCell), new Func<MoveToLureStates.Instance, CellOffset[]>(MoveToLureStates.GetLureOffsets), this.arrive_at_lure, this.behaviourcomplete, false);
		this.arrive_at_lure.Enter(delegate(MoveToLureStates.Instance smi)
		{
			Lure.Instance targetLure = MoveToLureStates.GetTargetLure(smi);
			if (targetLure != null && targetLure.HasTag(GameTags.OneTimeUseLure))
			{
				targetLure.GetComponent<KPrefabID>().AddTag(GameTags.LureUsed, false);
			}
		}).GoTo(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.MoveToLure, false);
	}

	// Token: 0x0600043D RID: 1085 RVA: 0x00021F50 File Offset: 0x00020150
	private static Lure.Instance GetTargetLure(MoveToLureStates.Instance smi)
	{
		GameObject targetLure = smi.GetSMI<LureableMonitor.Instance>().GetTargetLure();
		if (targetLure == null)
		{
			return null;
		}
		return targetLure.GetSMI<Lure.Instance>();
	}

	// Token: 0x0600043E RID: 1086 RVA: 0x00021F7C File Offset: 0x0002017C
	private static int GetLureCell(MoveToLureStates.Instance smi)
	{
		Lure.Instance targetLure = MoveToLureStates.GetTargetLure(smi);
		if (targetLure == null)
		{
			return Grid.InvalidCell;
		}
		return Grid.PosToCell(targetLure);
	}

	// Token: 0x0600043F RID: 1087 RVA: 0x00021FA0 File Offset: 0x000201A0
	private static CellOffset[] GetLureOffsets(MoveToLureStates.Instance smi)
	{
		Lure.Instance targetLure = MoveToLureStates.GetTargetLure(smi);
		if (targetLure == null)
		{
			return null;
		}
		return targetLure.LurePoints;
	}

	// Token: 0x040002DF RID: 735
	public GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.State move;

	// Token: 0x040002E0 RID: 736
	public GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.State arrive_at_lure;

	// Token: 0x040002E1 RID: 737
	public GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.State behaviourcomplete;

	// Token: 0x02001084 RID: 4228
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001085 RID: 4229
	public new class Instance : GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.GameInstance
	{
		// Token: 0x06007C29 RID: 31785 RVA: 0x00304C77 File Offset: 0x00302E77
		public Instance(Chore<MoveToLureStates.Instance> chore, MoveToLureStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.MoveToLure);
		}
	}
}
