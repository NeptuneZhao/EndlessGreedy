using System;
using KSerialization;

// Token: 0x020004CD RID: 1229
[SerializationConfig(MemberSerialization.OptIn)]
public class StateMachineComponent<StateMachineInstanceType> : StateMachineComponent, ISaveLoadable where StateMachineInstanceType : StateMachine.Instance
{
	// Token: 0x170000B1 RID: 177
	// (get) Token: 0x06001A6C RID: 6764 RVA: 0x0008B83C File Offset: 0x00089A3C
	public StateMachineInstanceType smi
	{
		get
		{
			if (this._smi == null)
			{
				this._smi = (StateMachineInstanceType)((object)Activator.CreateInstance(typeof(StateMachineInstanceType), new object[]
				{
					this
				}));
			}
			return this._smi;
		}
	}

	// Token: 0x06001A6D RID: 6765 RVA: 0x0008B875 File Offset: 0x00089A75
	public override StateMachine.Instance GetSMI()
	{
		return this._smi;
	}

	// Token: 0x06001A6E RID: 6766 RVA: 0x0008B882 File Offset: 0x00089A82
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this._smi != null)
		{
			this._smi.StopSM("StateMachineComponent.OnCleanUp");
			this._smi = default(StateMachineInstanceType);
		}
	}

	// Token: 0x06001A6F RID: 6767 RVA: 0x0008B8B8 File Offset: 0x00089AB8
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		if (base.isSpawned)
		{
			this.smi.StartSM();
		}
	}

	// Token: 0x06001A70 RID: 6768 RVA: 0x0008B8D8 File Offset: 0x00089AD8
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this._smi != null)
		{
			this._smi.StopSM("StateMachineComponent.OnDisable");
		}
	}

	// Token: 0x04000F0C RID: 3852
	private StateMachineInstanceType _smi;
}
