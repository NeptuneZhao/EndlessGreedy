using System;

// Token: 0x020006A8 RID: 1704
public abstract class ConduitSensor : Switch
{
	// Token: 0x06002AC8 RID: 10952
	protected abstract void ConduitUpdate(float dt);

	// Token: 0x06002AC9 RID: 10953 RVA: 0x000F11B8 File Offset: 0x000EF3B8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.animController = base.GetComponent<KBatchedAnimController>();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
		if (this.conduitType == ConduitType.Liquid || this.conduitType == ConduitType.Gas)
		{
			Conduit.GetFlowManager(this.conduitType).AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
			return;
		}
		SolidConduit.GetFlowManager().AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
	}

	// Token: 0x06002ACA RID: 10954 RVA: 0x000F124C File Offset: 0x000EF44C
	protected override void OnCleanUp()
	{
		if (this.conduitType == ConduitType.Liquid || this.conduitType == ConduitType.Gas)
		{
			Conduit.GetFlowManager(this.conduitType).RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		}
		else
		{
			SolidConduit.GetFlowManager().RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		}
		base.OnCleanUp();
	}

	// Token: 0x06002ACB RID: 10955 RVA: 0x000F12A7 File Offset: 0x000EF4A7
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x06002ACC RID: 10956 RVA: 0x000F12B6 File Offset: 0x000EF4B6
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06002ACD RID: 10957 RVA: 0x000F12D4 File Offset: 0x000EF4D4
	protected virtual void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			if (this.switchedOn)
			{
				this.animController.Play(ConduitSensor.ON_ANIMS, KAnim.PlayMode.Loop);
				return;
			}
			this.animController.Play(ConduitSensor.OFF_ANIMS, KAnim.PlayMode.Once);
		}
	}

	// Token: 0x040018A0 RID: 6304
	public ConduitType conduitType;

	// Token: 0x040018A1 RID: 6305
	protected bool wasOn;

	// Token: 0x040018A2 RID: 6306
	protected KBatchedAnimController animController;

	// Token: 0x040018A3 RID: 6307
	protected static readonly HashedString[] ON_ANIMS = new HashedString[]
	{
		"on_pre",
		"on"
	};

	// Token: 0x040018A4 RID: 6308
	protected static readonly HashedString[] OFF_ANIMS = new HashedString[]
	{
		"on_pst",
		"off"
	};
}
