using System;
using System.Collections.Generic;

// Token: 0x02000471 RID: 1137
public class VoidChoreProvider : ChoreProvider
{
	// Token: 0x06001878 RID: 6264 RVA: 0x00082CD4 File Offset: 0x00080ED4
	public static void DestroyInstance()
	{
		VoidChoreProvider.Instance = null;
	}

	// Token: 0x06001879 RID: 6265 RVA: 0x00082CDC File Offset: 0x00080EDC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		VoidChoreProvider.Instance = this;
	}

	// Token: 0x0600187A RID: 6266 RVA: 0x00082CEA File Offset: 0x00080EEA
	public override void AddChore(Chore chore)
	{
	}

	// Token: 0x0600187B RID: 6267 RVA: 0x00082CEC File Offset: 0x00080EEC
	public override void RemoveChore(Chore chore)
	{
	}

	// Token: 0x0600187C RID: 6268 RVA: 0x00082CEE File Offset: 0x00080EEE
	public override void CollectChores(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded, List<Chore.Precondition.Context> failed_contexts)
	{
	}

	// Token: 0x04000D90 RID: 3472
	public static VoidChoreProvider Instance;
}
