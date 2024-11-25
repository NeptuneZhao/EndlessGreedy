using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000B37 RID: 2871
public class Trap : StateMachineComponent<Trap.StatesInstance>
{
	// Token: 0x060055B2 RID: 21938 RVA: 0x001E9E90 File Offset: 0x001E8090
	private static void CreateStatusItems()
	{
		if (Trap.statusSprung == null)
		{
			Trap.statusReady = new StatusItem("Ready", BUILDING.STATUSITEMS.CREATURE_TRAP.READY.NAME, BUILDING.STATUSITEMS.CREATURE_TRAP.READY.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
			Trap.statusSprung = new StatusItem("Sprung", BUILDING.STATUSITEMS.CREATURE_TRAP.SPRUNG.NAME, BUILDING.STATUSITEMS.CREATURE_TRAP.SPRUNG.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
			Trap.statusSprung.resolveTooltipCallback = delegate(string str, object obj)
			{
				Trap.StatesInstance statesInstance = (Trap.StatesInstance)obj;
				return string.Format(str, statesInstance.master.contents.Get().GetProperName());
			};
		}
	}

	// Token: 0x060055B3 RID: 21939 RVA: 0x001E9F3E File Offset: 0x001E813E
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.contents = new Ref<KPrefabID>();
		Trap.CreateStatusItems();
	}

	// Token: 0x060055B4 RID: 21940 RVA: 0x001E9F58 File Offset: 0x001E8158
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Storage component = base.GetComponent<Storage>();
		base.smi.StartSM();
		if (!component.IsEmpty())
		{
			KPrefabID component2 = component.items[0].GetComponent<KPrefabID>();
			if (component2 != null)
			{
				this.contents.Set(component2);
				base.smi.GoTo(base.smi.sm.occupied);
				return;
			}
			component.DropAll(false, false, default(Vector3), true, null);
		}
	}

	// Token: 0x0400382C RID: 14380
	[Serialize]
	private Ref<KPrefabID> contents;

	// Token: 0x0400382D RID: 14381
	public TagSet captureTags = new TagSet();

	// Token: 0x0400382E RID: 14382
	private static StatusItem statusReady;

	// Token: 0x0400382F RID: 14383
	private static StatusItem statusSprung;

	// Token: 0x02001B8C RID: 7052
	public class StatesInstance : GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.GameInstance
	{
		// Token: 0x0600A3C1 RID: 41921 RVA: 0x0038AAAE File Offset: 0x00388CAE
		public StatesInstance(Trap master) : base(master)
		{
		}

		// Token: 0x0600A3C2 RID: 41922 RVA: 0x0038AAB8 File Offset: 0x00388CB8
		public void OnTrapTriggered(object data)
		{
			KPrefabID component = ((GameObject)data).GetComponent<KPrefabID>();
			base.master.contents.Set(component);
			base.smi.sm.trapTriggered.Trigger(base.smi);
		}
	}

	// Token: 0x02001B8D RID: 7053
	public class States : GameStateMachine<Trap.States, Trap.StatesInstance, Trap>
	{
		// Token: 0x0600A3C3 RID: 41923 RVA: 0x0038AB00 File Offset: 0x00388D00
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.ready;
			base.serializable = StateMachine.SerializeType.Never;
			Trap.CreateStatusItems();
			this.ready.EventHandler(GameHashes.TrapTriggered, delegate(Trap.StatesInstance smi, object data)
			{
				smi.OnTrapTriggered(data);
			}).OnSignal(this.trapTriggered, this.trapping).ToggleStatusItem(Trap.statusReady, null);
			this.trapping.PlayAnim("working_pre").OnAnimQueueComplete(this.occupied);
			this.occupied.ToggleTag(GameTags.Trapped).ToggleStatusItem(Trap.statusSprung, (Trap.StatesInstance smi) => smi).DefaultState(this.occupied.idle).EventTransition(GameHashes.OnStorageChange, this.finishedUsing, (Trap.StatesInstance smi) => smi.master.GetComponent<Storage>().IsEmpty());
			this.occupied.idle.PlayAnim("working_loop", KAnim.PlayMode.Loop);
			this.finishedUsing.PlayAnim("working_pst").OnAnimQueueComplete(this.destroySelf);
			this.destroySelf.Enter(delegate(Trap.StatesInstance smi)
			{
				Util.KDestroyGameObject(smi.master.gameObject);
			});
		}

		// Token: 0x04008004 RID: 32772
		public GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State ready;

		// Token: 0x04008005 RID: 32773
		public GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State trapping;

		// Token: 0x04008006 RID: 32774
		public GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State finishedUsing;

		// Token: 0x04008007 RID: 32775
		public GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State destroySelf;

		// Token: 0x04008008 RID: 32776
		public StateMachine<Trap.States, Trap.StatesInstance, Trap, object>.Signal trapTriggered;

		// Token: 0x04008009 RID: 32777
		public Trap.States.OccupiedStates occupied;

		// Token: 0x02002625 RID: 9765
		public class OccupiedStates : GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State
		{
			// Token: 0x0400A9BE RID: 43454
			public GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State idle;
		}
	}
}
