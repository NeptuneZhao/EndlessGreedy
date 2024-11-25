using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200081C RID: 2076
[SkipSaveFileSerialization]
public class ReceptacleMonitor : StateMachineComponent<ReceptacleMonitor.StatesInstance>, IGameObjectEffectDescriptor, IWiltCause, ISim1000ms
{
	// Token: 0x17000409 RID: 1033
	// (get) Token: 0x0600396C RID: 14700 RVA: 0x00139357 File Offset: 0x00137557
	public bool Replanted
	{
		get
		{
			return this.replanted;
		}
	}

	// Token: 0x0600396D RID: 14701 RVA: 0x0013935F File Offset: 0x0013755F
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x0600396E RID: 14702 RVA: 0x00139372 File Offset: 0x00137572
	public PlantablePlot GetReceptacle()
	{
		return (PlantablePlot)base.smi.sm.receptacle.Get(base.smi);
	}

	// Token: 0x0600396F RID: 14703 RVA: 0x00139394 File Offset: 0x00137594
	public void SetReceptacle(PlantablePlot plot = null)
	{
		if (plot == null)
		{
			base.smi.sm.receptacle.Set(null, base.smi, false);
			this.replanted = false;
		}
		else
		{
			base.smi.sm.receptacle.Set(plot, base.smi, false);
			this.replanted = true;
		}
		base.Trigger(-1636776682, null);
	}

	// Token: 0x06003970 RID: 14704 RVA: 0x00139404 File Offset: 0x00137604
	public void Sim1000ms(float dt)
	{
		if (base.smi.sm.receptacle.Get(base.smi) == null)
		{
			base.smi.GoTo(base.smi.sm.wild);
			return;
		}
		Operational component = base.smi.sm.receptacle.Get(base.smi).GetComponent<Operational>();
		if (component == null)
		{
			base.smi.GoTo(base.smi.sm.operational);
			return;
		}
		if (component.IsOperational)
		{
			base.smi.GoTo(base.smi.sm.operational);
			return;
		}
		base.smi.GoTo(base.smi.sm.inoperational);
	}

	// Token: 0x1700040A RID: 1034
	// (get) Token: 0x06003971 RID: 14705 RVA: 0x001394D5 File Offset: 0x001376D5
	WiltCondition.Condition[] IWiltCause.Conditions
	{
		get
		{
			return new WiltCondition.Condition[]
			{
				WiltCondition.Condition.Receptacle
			};
		}
	}

	// Token: 0x1700040B RID: 1035
	// (get) Token: 0x06003972 RID: 14706 RVA: 0x001394E4 File Offset: 0x001376E4
	public string WiltStateString
	{
		get
		{
			string text = "";
			if (base.smi.IsInsideState(base.smi.sm.inoperational))
			{
				text += CREATURES.STATUSITEMS.RECEPTACLEINOPERATIONAL.NAME;
			}
			return text;
		}
	}

	// Token: 0x06003973 RID: 14707 RVA: 0x00139526 File Offset: 0x00137726
	public bool HasReceptacle()
	{
		return !base.smi.IsInsideState(base.smi.sm.wild);
	}

	// Token: 0x06003974 RID: 14708 RVA: 0x00139546 File Offset: 0x00137746
	public bool HasOperationalReceptacle()
	{
		return base.smi.IsInsideState(base.smi.sm.operational);
	}

	// Token: 0x06003975 RID: 14709 RVA: 0x00139563 File Offset: 0x00137763
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(UI.GAMEOBJECTEFFECTS.REQUIRES_RECEPTACLE, UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_RECEPTACLE, Descriptor.DescriptorType.Requirement, false)
		};
	}

	// Token: 0x04002294 RID: 8852
	private bool replanted;

	// Token: 0x0200172B RID: 5931
	public class StatesInstance : GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.GameInstance
	{
		// Token: 0x060094E0 RID: 38112 RVA: 0x0035E412 File Offset: 0x0035C612
		public StatesInstance(ReceptacleMonitor master) : base(master)
		{
		}
	}

	// Token: 0x0200172C RID: 5932
	public class States : GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor>
	{
		// Token: 0x060094E1 RID: 38113 RVA: 0x0035E41C File Offset: 0x0035C61C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.wild;
			base.serializable = StateMachine.SerializeType.Never;
			this.wild.TriggerOnEnter(GameHashes.ReceptacleOperational, null);
			this.inoperational.TriggerOnEnter(GameHashes.ReceptacleInoperational, null);
			this.operational.TriggerOnEnter(GameHashes.ReceptacleOperational, null);
		}

		// Token: 0x040071F9 RID: 29177
		public StateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.ObjectParameter<SingleEntityReceptacle> receptacle;

		// Token: 0x040071FA RID: 29178
		public GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.State wild;

		// Token: 0x040071FB RID: 29179
		public GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.State inoperational;

		// Token: 0x040071FC RID: 29180
		public GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.State operational;
	}
}
