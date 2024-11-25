using System;

// Token: 0x020009AB RID: 2475
public class ToiletMonitor : GameStateMachine<ToiletMonitor, ToiletMonitor.Instance>
{
	// Token: 0x060047FE RID: 18430 RVA: 0x0019C6D4 File Offset: 0x0019A8D4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.EventHandler(GameHashes.ToiletSensorChanged, delegate(ToiletMonitor.Instance smi)
		{
			smi.RefreshStatusItem();
		}).Exit("ClearStatusItem", delegate(ToiletMonitor.Instance smi)
		{
			smi.ClearStatusItem();
		});
	}

	// Token: 0x04002F21 RID: 12065
	public GameStateMachine<ToiletMonitor, ToiletMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04002F22 RID: 12066
	public GameStateMachine<ToiletMonitor, ToiletMonitor.Instance, IStateMachineTarget, object>.State unsatisfied;

	// Token: 0x020019A2 RID: 6562
	public new class Instance : GameStateMachine<ToiletMonitor, ToiletMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009D79 RID: 40313 RVA: 0x00375206 File Offset: 0x00373406
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.toiletSensor = base.GetComponent<Sensors>().GetSensor<ToiletSensor>();
		}

		// Token: 0x06009D7A RID: 40314 RVA: 0x00375220 File Offset: 0x00373420
		public void RefreshStatusItem()
		{
			StatusItem status_item = null;
			if (!this.toiletSensor.AreThereAnyToilets())
			{
				status_item = Db.Get().DuplicantStatusItems.NoToilets;
			}
			else if (!this.toiletSensor.AreThereAnyUsableToilets())
			{
				status_item = Db.Get().DuplicantStatusItems.NoUsableToilets;
			}
			else if (this.toiletSensor.GetNearestUsableToilet() == null)
			{
				status_item = Db.Get().DuplicantStatusItems.ToiletUnreachable;
			}
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Toilet, status_item, null);
		}

		// Token: 0x06009D7B RID: 40315 RVA: 0x003752A7 File Offset: 0x003734A7
		public void ClearStatusItem()
		{
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Toilet, null, null);
		}

		// Token: 0x04007A27 RID: 31271
		private ToiletSensor toiletSensor;
	}
}
