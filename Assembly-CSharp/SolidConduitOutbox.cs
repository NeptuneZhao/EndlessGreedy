using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200076C RID: 1900
[SerializationConfig(MemberSerialization.OptIn)]
public class SolidConduitOutbox : StateMachineComponent<SolidConduitOutbox.SMInstance>
{
	// Token: 0x0600331E RID: 13086 RVA: 0x00118A93 File Offset: 0x00116C93
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600331F RID: 13087 RVA: 0x00118A9B File Offset: 0x00116C9B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.meter = new MeterController(this, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		base.Subscribe<SolidConduitOutbox>(-1697596308, SolidConduitOutbox.OnStorageChangedDelegate);
		this.UpdateMeter();
		base.smi.StartSM();
	}

	// Token: 0x06003320 RID: 13088 RVA: 0x00118AD9 File Offset: 0x00116CD9
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06003321 RID: 13089 RVA: 0x00118AE1 File Offset: 0x00116CE1
	private void OnStorageChanged(object data)
	{
		this.UpdateMeter();
	}

	// Token: 0x06003322 RID: 13090 RVA: 0x00118AEC File Offset: 0x00116CEC
	private void UpdateMeter()
	{
		float positionPercent = Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg);
		this.meter.SetPositionPercent(positionPercent);
	}

	// Token: 0x06003323 RID: 13091 RVA: 0x00118B22 File Offset: 0x00116D22
	private void UpdateConsuming()
	{
		base.smi.sm.consuming.Set(this.consumer.IsConsuming, base.smi, false);
	}

	// Token: 0x04001E2F RID: 7727
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001E30 RID: 7728
	[MyCmpReq]
	private SolidConduitConsumer consumer;

	// Token: 0x04001E31 RID: 7729
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x04001E32 RID: 7730
	private MeterController meter;

	// Token: 0x04001E33 RID: 7731
	private static readonly EventSystem.IntraObjectHandler<SolidConduitOutbox> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<SolidConduitOutbox>(delegate(SolidConduitOutbox component, object data)
	{
		component.OnStorageChanged(data);
	});

	// Token: 0x020015FB RID: 5627
	public class SMInstance : GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.GameInstance
	{
		// Token: 0x0600908F RID: 37007 RVA: 0x0034BF60 File Offset: 0x0034A160
		public SMInstance(SolidConduitOutbox master) : base(master)
		{
		}
	}

	// Token: 0x020015FC RID: 5628
	public class States : GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox>
	{
		// Token: 0x06009090 RID: 37008 RVA: 0x0034BF6C File Offset: 0x0034A16C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.Update("RefreshConsuming", delegate(SolidConduitOutbox.SMInstance smi, float dt)
			{
				smi.master.UpdateConsuming();
			}, UpdateRate.SIM_1000ms, false);
			this.idle.PlayAnim("on").ParamTransition<bool>(this.consuming, this.working, GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.IsTrue);
			this.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).ParamTransition<bool>(this.consuming, this.post, GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.IsFalse);
			this.post.PlayAnim("working_pst").OnAnimQueueComplete(this.idle);
		}

		// Token: 0x04006E50 RID: 28240
		public StateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.BoolParameter consuming;

		// Token: 0x04006E51 RID: 28241
		public GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.State idle;

		// Token: 0x04006E52 RID: 28242
		public GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.State working;

		// Token: 0x04006E53 RID: 28243
		public GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.State post;
	}
}
