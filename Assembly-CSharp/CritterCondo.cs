using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006B0 RID: 1712
public class CritterCondo : GameStateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>
{
	// Token: 0x06002B21 RID: 11041 RVA: 0x000F2464 File Offset: 0x000F0664
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.inoperational.PlayAnim("off").EventTransition(GameHashes.UpdateRoom, this.operational, new StateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.Transition.ConditionCallback(CritterCondo.IsOperational)).EventTransition(GameHashes.OperationalChanged, this.operational, new StateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.Transition.ConditionCallback(CritterCondo.IsOperational));
		this.operational.PlayAnim("on", KAnim.PlayMode.Loop).EventTransition(GameHashes.UpdateRoom, this.inoperational, GameStateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.Not(new StateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.Transition.ConditionCallback(CritterCondo.IsOperational))).EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.Not(new StateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.Transition.ConditionCallback(CritterCondo.IsOperational)));
	}

	// Token: 0x06002B22 RID: 11042 RVA: 0x000F2516 File Offset: 0x000F0716
	private static bool IsOperational(CritterCondo.Instance smi)
	{
		return smi.def.IsCritterCondoOperationalCb(smi);
	}

	// Token: 0x040018C2 RID: 6338
	public GameStateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.State inoperational;

	// Token: 0x040018C3 RID: 6339
	public GameStateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.State operational;

	// Token: 0x020014A4 RID: 5284
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06008BA1 RID: 35745 RVA: 0x00337633 File Offset: 0x00335833
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			return new List<Descriptor>();
		}

		// Token: 0x04006A85 RID: 27269
		public Func<CritterCondo.Instance, bool> IsCritterCondoOperationalCb;

		// Token: 0x04006A86 RID: 27270
		public StatusItem moveToStatusItem;

		// Token: 0x04006A87 RID: 27271
		public StatusItem interactStatusItem;

		// Token: 0x04006A88 RID: 27272
		public Tag condoTag = "CritterCondo";

		// Token: 0x04006A89 RID: 27273
		public string effectId;
	}

	// Token: 0x020014A5 RID: 5285
	public new class Instance : GameStateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.GameInstance
	{
		// Token: 0x06008BA3 RID: 35747 RVA: 0x00337652 File Offset: 0x00335852
		public Instance(IStateMachineTarget master, CritterCondo.Def def) : base(master, def)
		{
		}

		// Token: 0x06008BA4 RID: 35748 RVA: 0x0033765C File Offset: 0x0033585C
		public override void StartSM()
		{
			base.StartSM();
			Components.CritterCondos.Add(base.smi.GetMyWorldId(), this);
		}

		// Token: 0x06008BA5 RID: 35749 RVA: 0x0033767A File Offset: 0x0033587A
		protected override void OnCleanUp()
		{
			Components.CritterCondos.Remove(base.smi.GetMyWorldId(), this);
		}

		// Token: 0x06008BA6 RID: 35750 RVA: 0x00337692 File Offset: 0x00335892
		public bool IsReserved()
		{
			return base.HasTag(GameTags.Creatures.ReservedByCreature);
		}

		// Token: 0x06008BA7 RID: 35751 RVA: 0x003376A0 File Offset: 0x003358A0
		public void SetReserved(bool isReserved)
		{
			if (isReserved)
			{
				base.GetComponent<KPrefabID>().SetTag(GameTags.Creatures.ReservedByCreature, true);
				return;
			}
			if (base.HasTag(GameTags.Creatures.ReservedByCreature))
			{
				base.GetComponent<KPrefabID>().RemoveTag(GameTags.Creatures.ReservedByCreature);
				return;
			}
			global::Debug.LogWarningFormat(base.smi.gameObject, "Tried to unreserve a condo that wasn't reserved", Array.Empty<object>());
		}

		// Token: 0x06008BA8 RID: 35752 RVA: 0x003376FA File Offset: 0x003358FA
		public int GetInteractStartCell()
		{
			return Grid.PosToCell(this);
		}

		// Token: 0x06008BA9 RID: 35753 RVA: 0x00337702 File Offset: 0x00335902
		public bool CanBeReserved()
		{
			return !this.IsReserved() && CritterCondo.IsOperational(this);
		}
	}
}
