using System;

// Token: 0x02000845 RID: 2117
public class DiagnosticCriterion
{
	// Token: 0x1700043C RID: 1084
	// (get) Token: 0x06003B22 RID: 15138 RVA: 0x00144EB7 File Offset: 0x001430B7
	// (set) Token: 0x06003B23 RID: 15139 RVA: 0x00144EBF File Offset: 0x001430BF
	public string id { get; private set; }

	// Token: 0x1700043D RID: 1085
	// (get) Token: 0x06003B24 RID: 15140 RVA: 0x00144EC8 File Offset: 0x001430C8
	// (set) Token: 0x06003B25 RID: 15141 RVA: 0x00144ED0 File Offset: 0x001430D0
	public string name { get; private set; }

	// Token: 0x06003B26 RID: 15142 RVA: 0x00144ED9 File Offset: 0x001430D9
	public DiagnosticCriterion(string name, Func<ColonyDiagnostic.DiagnosticResult> action)
	{
		this.name = name;
		this.evaluateAction = action;
	}

	// Token: 0x06003B27 RID: 15143 RVA: 0x00144EEF File Offset: 0x001430EF
	public void SetID(string id)
	{
		this.id = id;
	}

	// Token: 0x06003B28 RID: 15144 RVA: 0x00144EF8 File Offset: 0x001430F8
	public ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		return this.evaluateAction();
	}

	// Token: 0x040023E2 RID: 9186
	private Func<ColonyDiagnostic.DiagnosticResult> evaluateAction;
}
