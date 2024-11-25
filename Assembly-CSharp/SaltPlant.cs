using System;

// Token: 0x020009F9 RID: 2553
public class SaltPlant : StateMachineComponent<SaltPlant.StatesInstance>
{
	// Token: 0x060049EC RID: 18924 RVA: 0x001A69A5 File Offset: 0x001A4BA5
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<SaltPlant>(-724860998, SaltPlant.OnWiltDelegate);
		base.Subscribe<SaltPlant>(712767498, SaltPlant.OnWiltRecoverDelegate);
	}

	// Token: 0x060049ED RID: 18925 RVA: 0x001A69CF File Offset: 0x001A4BCF
	private void OnWilt(object data = null)
	{
		base.gameObject.GetComponent<ElementConsumer>().EnableConsumption(false);
	}

	// Token: 0x060049EE RID: 18926 RVA: 0x001A69E2 File Offset: 0x001A4BE2
	private void OnWiltRecover(object data = null)
	{
		base.gameObject.GetComponent<ElementConsumer>().EnableConsumption(true);
	}

	// Token: 0x0400307A RID: 12410
	private static readonly EventSystem.IntraObjectHandler<SaltPlant> OnWiltDelegate = new EventSystem.IntraObjectHandler<SaltPlant>(delegate(SaltPlant component, object data)
	{
		component.OnWilt(data);
	});

	// Token: 0x0400307B RID: 12411
	private static readonly EventSystem.IntraObjectHandler<SaltPlant> OnWiltRecoverDelegate = new EventSystem.IntraObjectHandler<SaltPlant>(delegate(SaltPlant component, object data)
	{
		component.OnWiltRecover(data);
	});

	// Token: 0x02001A11 RID: 6673
	public class StatesInstance : GameStateMachine<SaltPlant.States, SaltPlant.StatesInstance, SaltPlant, object>.GameInstance
	{
		// Token: 0x06009F06 RID: 40710 RVA: 0x0037B020 File Offset: 0x00379220
		public StatesInstance(SaltPlant master) : base(master)
		{
		}
	}

	// Token: 0x02001A12 RID: 6674
	public class States : GameStateMachine<SaltPlant.States, SaltPlant.StatesInstance, SaltPlant>
	{
		// Token: 0x06009F07 RID: 40711 RVA: 0x0037B029 File Offset: 0x00379229
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			default_state = this.alive;
			this.alive.DoNothing();
		}

		// Token: 0x04007B2E RID: 31534
		public GameStateMachine<SaltPlant.States, SaltPlant.StatesInstance, SaltPlant, object>.State alive;
	}
}
