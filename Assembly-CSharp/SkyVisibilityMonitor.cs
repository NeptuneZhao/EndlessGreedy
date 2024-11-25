using System;
using Database;
using UnityEngine;

// Token: 0x02000A9F RID: 2719
public class SkyVisibilityMonitor : GameStateMachine<SkyVisibilityMonitor, SkyVisibilityMonitor.Instance, IStateMachineTarget, SkyVisibilityMonitor.Def>
{
	// Token: 0x06005000 RID: 20480 RVA: 0x001CC699 File Offset: 0x001CA899
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.Update(new Action<SkyVisibilityMonitor.Instance, float>(SkyVisibilityMonitor.CheckSkyVisibility), UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x06005001 RID: 20481 RVA: 0x001CC6C0 File Offset: 0x001CA8C0
	public static void CheckSkyVisibility(SkyVisibilityMonitor.Instance smi, float dt)
	{
		bool hasSkyVisibility = smi.HasSkyVisibility;
		ValueTuple<bool, float> visibilityOf = smi.def.skyVisibilityInfo.GetVisibilityOf(smi.gameObject);
		bool item = visibilityOf.Item1;
		float item2 = visibilityOf.Item2;
		smi.Internal_SetPercentClearSky(item2);
		KSelectable component = smi.GetComponent<KSelectable>();
		component.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisNone, !item, smi);
		component.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisLimited, item && item2 < 1f, smi);
		if (hasSkyVisibility == item)
		{
			return;
		}
		smi.TriggerVisibilityChange();
	}

	// Token: 0x02001AD1 RID: 6865
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007DD8 RID: 32216
		public SkyVisibilityInfo skyVisibilityInfo;
	}

	// Token: 0x02001AD2 RID: 6866
	public new class Instance : GameStateMachine<SkyVisibilityMonitor, SkyVisibilityMonitor.Instance, IStateMachineTarget, SkyVisibilityMonitor.Def>.GameInstance, BuildingStatusItems.ISkyVisInfo
	{
		// Token: 0x17000B22 RID: 2850
		// (get) Token: 0x0600A127 RID: 41255 RVA: 0x00382347 File Offset: 0x00380547
		public bool HasSkyVisibility
		{
			get
			{
				return this.PercentClearSky > 0f && !Mathf.Approximately(0f, this.PercentClearSky);
			}
		}

		// Token: 0x17000B23 RID: 2851
		// (get) Token: 0x0600A128 RID: 41256 RVA: 0x0038236B File Offset: 0x0038056B
		public float PercentClearSky
		{
			get
			{
				return this.percentClearSky01;
			}
		}

		// Token: 0x0600A129 RID: 41257 RVA: 0x00382373 File Offset: 0x00380573
		public void Internal_SetPercentClearSky(float percent01)
		{
			this.percentClearSky01 = percent01;
		}

		// Token: 0x0600A12A RID: 41258 RVA: 0x0038237C File Offset: 0x0038057C
		float BuildingStatusItems.ISkyVisInfo.GetPercentVisible01()
		{
			return this.percentClearSky01;
		}

		// Token: 0x0600A12B RID: 41259 RVA: 0x00382384 File Offset: 0x00380584
		public Instance(IStateMachineTarget master, SkyVisibilityMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x0600A12C RID: 41260 RVA: 0x0038238E File Offset: 0x0038058E
		public override void StartSM()
		{
			base.StartSM();
			SkyVisibilityMonitor.CheckSkyVisibility(this, 0f);
			this.TriggerVisibilityChange();
		}

		// Token: 0x0600A12D RID: 41261 RVA: 0x003823A8 File Offset: 0x003805A8
		public void TriggerVisibilityChange()
		{
			if (this.visibilityStatusItem != null)
			{
				base.smi.GetComponent<KSelectable>().ToggleStatusItem(this.visibilityStatusItem, !this.HasSkyVisibility, this);
			}
			base.smi.GetComponent<Operational>().SetFlag(SkyVisibilityMonitor.Instance.skyVisibilityFlag, this.HasSkyVisibility);
			if (this.SkyVisibilityChanged != null)
			{
				this.SkyVisibilityChanged();
			}
		}

		// Token: 0x04007DD9 RID: 32217
		private float percentClearSky01;

		// Token: 0x04007DDA RID: 32218
		public System.Action SkyVisibilityChanged;

		// Token: 0x04007DDB RID: 32219
		private StatusItem visibilityStatusItem;

		// Token: 0x04007DDC RID: 32220
		private static readonly Operational.Flag skyVisibilityFlag = new Operational.Flag("sky visibility", Operational.Flag.Type.Requirement);
	}
}
