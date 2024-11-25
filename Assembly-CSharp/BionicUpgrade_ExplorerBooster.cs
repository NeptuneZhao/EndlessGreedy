using System;
using UnityEngine;

// Token: 0x02000662 RID: 1634
public class BionicUpgrade_ExplorerBooster : GameStateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>
{
	// Token: 0x06002850 RID: 10320 RVA: 0x000E47BC File Offset: 0x000E29BC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.not_ready;
		this.not_ready.ParamTransition<float>(this.Progress, this.ready, GameStateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>.IsGTEOne).ToggleStatusItem(Db.Get().MiscStatusItems.BionicExplorerBooster, null);
		this.ready.ParamTransition<float>(this.Progress, this.not_ready, GameStateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>.IsLTOne).ToggleStatusItem(Db.Get().MiscStatusItems.BionicExplorerBoosterReady, null);
	}

	// Token: 0x04001735 RID: 5941
	public const float DataGatheringDuration = 600f;

	// Token: 0x04001736 RID: 5942
	private StateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>.FloatParameter Progress;

	// Token: 0x04001737 RID: 5943
	public GameStateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>.State not_ready;

	// Token: 0x04001738 RID: 5944
	public GameStateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>.State ready;

	// Token: 0x0200143D RID: 5181
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200143E RID: 5182
	public new class Instance : GameStateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>.GameInstance
	{
		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x060089D7 RID: 35287 RVA: 0x00331AB0 File Offset: 0x0032FCB0
		public bool IsBeingMonitored
		{
			get
			{
				return this.monitor != null;
			}
		}

		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x060089D8 RID: 35288 RVA: 0x00331ABB File Offset: 0x0032FCBB
		public bool IsReady
		{
			get
			{
				return this.Progress == 1f;
			}
		}

		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x060089D9 RID: 35289 RVA: 0x00331ACA File Offset: 0x0032FCCA
		public float Progress
		{
			get
			{
				return base.sm.Progress.Get(this);
			}
		}

		// Token: 0x060089DA RID: 35290 RVA: 0x00331ADD File Offset: 0x0032FCDD
		public Instance(IStateMachineTarget master, BionicUpgrade_ExplorerBooster.Def def) : base(master, def)
		{
		}

		// Token: 0x060089DB RID: 35291 RVA: 0x00331AE7 File Offset: 0x0032FCE7
		public void SetMonitor(BionicUpgrade_ExplorerBoosterMonitor.Instance monitor)
		{
			this.monitor = monitor;
		}

		// Token: 0x060089DC RID: 35292 RVA: 0x00331AF0 File Offset: 0x0032FCF0
		public void AddData(float dataProgressDelta)
		{
			float dataProgress = Mathf.Clamp(this.Progress + dataProgressDelta, 0f, 1f);
			this.SetDataProgress(dataProgress);
		}

		// Token: 0x060089DD RID: 35293 RVA: 0x00331B1C File Offset: 0x0032FD1C
		public void SetDataProgress(float dataProgress)
		{
			Mathf.Clamp(dataProgress, 0f, 1f);
			base.sm.Progress.Set(dataProgress, this, false);
		}

		// Token: 0x0400693C RID: 26940
		private BionicUpgrade_ExplorerBoosterMonitor.Instance monitor;
	}
}
