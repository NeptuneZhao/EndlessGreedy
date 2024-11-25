using System;

// Token: 0x020006D7 RID: 1751
public class FossilMineSM : ComplexFabricatorSM
{
	// Token: 0x06002C53 RID: 11347 RVA: 0x000F9132 File Offset: 0x000F7332
	protected override void OnSpawn()
	{
	}

	// Token: 0x06002C54 RID: 11348 RVA: 0x000F9134 File Offset: 0x000F7334
	public void Activate()
	{
		base.smi.StartSM();
	}

	// Token: 0x06002C55 RID: 11349 RVA: 0x000F9141 File Offset: 0x000F7341
	public void Deactivate()
	{
		base.smi.StopSM("FossilMine.Deactivated");
	}
}
