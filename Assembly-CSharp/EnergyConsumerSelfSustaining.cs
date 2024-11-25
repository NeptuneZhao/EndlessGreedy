using System;
using System.Diagnostics;
using KSerialization;

// Token: 0x0200087F RID: 2175
[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{name} {WattsUsed}W")]
public class EnergyConsumerSelfSustaining : EnergyConsumer
{
	// Token: 0x1400001B RID: 27
	// (add) Token: 0x06003CEF RID: 15599 RVA: 0x001515E0 File Offset: 0x0014F7E0
	// (remove) Token: 0x06003CF0 RID: 15600 RVA: 0x00151618 File Offset: 0x0014F818
	public event System.Action OnConnectionChanged;

	// Token: 0x1700046D RID: 1133
	// (get) Token: 0x06003CF1 RID: 15601 RVA: 0x0015164D File Offset: 0x0014F84D
	public override bool IsPowered
	{
		get
		{
			return this.isSustained || this.connectionStatus == CircuitManager.ConnectionStatus.Powered;
		}
	}

	// Token: 0x1700046E RID: 1134
	// (get) Token: 0x06003CF2 RID: 15602 RVA: 0x00151662 File Offset: 0x0014F862
	public bool IsExternallyPowered
	{
		get
		{
			return this.connectionStatus == CircuitManager.ConnectionStatus.Powered;
		}
	}

	// Token: 0x06003CF3 RID: 15603 RVA: 0x0015166D File Offset: 0x0014F86D
	public void SetSustained(bool isSustained)
	{
		this.isSustained = isSustained;
	}

	// Token: 0x06003CF4 RID: 15604 RVA: 0x00151678 File Offset: 0x0014F878
	public override void SetConnectionStatus(CircuitManager.ConnectionStatus connection_status)
	{
		CircuitManager.ConnectionStatus connectionStatus = this.connectionStatus;
		switch (connection_status)
		{
		case CircuitManager.ConnectionStatus.NotConnected:
			this.connectionStatus = CircuitManager.ConnectionStatus.NotConnected;
			break;
		case CircuitManager.ConnectionStatus.Unpowered:
			if (this.connectionStatus == CircuitManager.ConnectionStatus.Powered && base.GetComponent<Battery>() == null)
			{
				this.connectionStatus = CircuitManager.ConnectionStatus.Unpowered;
			}
			break;
		case CircuitManager.ConnectionStatus.Powered:
			if (this.connectionStatus != CircuitManager.ConnectionStatus.Powered)
			{
				this.connectionStatus = CircuitManager.ConnectionStatus.Powered;
			}
			break;
		}
		this.UpdatePoweredStatus();
		if (connectionStatus != this.connectionStatus && this.OnConnectionChanged != null)
		{
			this.OnConnectionChanged();
		}
	}

	// Token: 0x06003CF5 RID: 15605 RVA: 0x001516FB File Offset: 0x0014F8FB
	public void UpdatePoweredStatus()
	{
		this.operational.SetFlag(EnergyConsumer.PoweredFlag, this.IsPowered);
	}

	// Token: 0x04002533 RID: 9523
	private bool isSustained;

	// Token: 0x04002534 RID: 9524
	private CircuitManager.ConnectionStatus connectionStatus;
}
