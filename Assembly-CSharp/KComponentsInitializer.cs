using System;

// Token: 0x02000936 RID: 2358
public class KComponentsInitializer : KComponentSpawn
{
	// Token: 0x0600447B RID: 17531 RVA: 0x00185BE3 File Offset: 0x00183DE3
	private void Awake()
	{
		KComponentSpawn.instance = this;
		this.comps = new GameComps();
	}
}
