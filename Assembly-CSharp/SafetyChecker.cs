using System;

// Token: 0x0200049F RID: 1183
public class SafetyChecker
{
	// Token: 0x1700009F RID: 159
	// (get) Token: 0x06001982 RID: 6530 RVA: 0x000887CF File Offset: 0x000869CF
	// (set) Token: 0x06001983 RID: 6531 RVA: 0x000887D7 File Offset: 0x000869D7
	public SafetyChecker.Condition[] conditions { get; private set; }

	// Token: 0x06001984 RID: 6532 RVA: 0x000887E0 File Offset: 0x000869E0
	public SafetyChecker(SafetyChecker.Condition[] conditions)
	{
		this.conditions = conditions;
	}

	// Token: 0x06001985 RID: 6533 RVA: 0x000887F0 File Offset: 0x000869F0
	public int GetSafetyConditions(int cell, int cost, SafetyChecker.Context context, out bool all_conditions_met)
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < this.conditions.Length; i++)
		{
			SafetyChecker.Condition condition = this.conditions[i];
			if (condition.callback(cell, cost, context))
			{
				num |= condition.mask;
				num2++;
			}
		}
		all_conditions_met = (num2 == this.conditions.Length);
		return num;
	}

	// Token: 0x0200126D RID: 4717
	public struct Condition
	{
		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x06008325 RID: 33573 RVA: 0x0031E5A3 File Offset: 0x0031C7A3
		// (set) Token: 0x06008326 RID: 33574 RVA: 0x0031E5AB File Offset: 0x0031C7AB
		public SafetyChecker.Condition.Callback callback { readonly get; private set; }

		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x06008327 RID: 33575 RVA: 0x0031E5B4 File Offset: 0x0031C7B4
		// (set) Token: 0x06008328 RID: 33576 RVA: 0x0031E5BC File Offset: 0x0031C7BC
		public int mask { readonly get; private set; }

		// Token: 0x06008329 RID: 33577 RVA: 0x0031E5C5 File Offset: 0x0031C7C5
		public Condition(string id, int condition_mask, SafetyChecker.Condition.Callback condition_callback)
		{
			this = default(SafetyChecker.Condition);
			this.callback = condition_callback;
			this.mask = condition_mask;
		}

		// Token: 0x02002405 RID: 9221
		// (Invoke) Token: 0x0600B8A9 RID: 47273
		public delegate bool Callback(int cell, int cost, SafetyChecker.Context context);
	}

	// Token: 0x0200126E RID: 4718
	public struct Context
	{
		// Token: 0x0600832A RID: 33578 RVA: 0x0031E5DC File Offset: 0x0031C7DC
		public Context(KMonoBehaviour cmp)
		{
			this.cell = Grid.PosToCell(cmp);
			this.navigator = cmp.GetComponent<Navigator>();
			this.oxygenBreather = cmp.GetComponent<OxygenBreather>();
			this.minionBrain = cmp.GetComponent<MinionBrain>();
			this.temperatureTransferer = cmp.GetComponent<SimTemperatureTransfer>();
			this.primaryElement = cmp.GetComponent<PrimaryElement>();
			this.worldID = this.navigator.GetMyWorldId();
		}

		// Token: 0x04006364 RID: 25444
		public Navigator navigator;

		// Token: 0x04006365 RID: 25445
		public OxygenBreather oxygenBreather;

		// Token: 0x04006366 RID: 25446
		public SimTemperatureTransfer temperatureTransferer;

		// Token: 0x04006367 RID: 25447
		public PrimaryElement primaryElement;

		// Token: 0x04006368 RID: 25448
		public MinionBrain minionBrain;

		// Token: 0x04006369 RID: 25449
		public int worldID;

		// Token: 0x0400636A RID: 25450
		public int cell;
	}
}
