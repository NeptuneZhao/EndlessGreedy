using System;

// Token: 0x02000B3F RID: 2879
public interface IDigActionEntity
{
	// Token: 0x060055FA RID: 22010
	void Dig();

	// Token: 0x060055FB RID: 22011
	void MarkForDig(bool instantOnDebug = true);
}
