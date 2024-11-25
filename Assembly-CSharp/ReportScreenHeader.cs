using System;
using UnityEngine;

// Token: 0x02000D1F RID: 3359
[AddComponentMenu("KMonoBehaviour/scripts/ReportScreenHeader")]
public class ReportScreenHeader : KMonoBehaviour
{
	// Token: 0x060068FA RID: 26874 RVA: 0x00274FDE File Offset: 0x002731DE
	public void SetMainEntry(ReportManager.ReportGroup reportGroup)
	{
		if (this.mainRow == null)
		{
			this.mainRow = Util.KInstantiateUI(this.rowTemplate.gameObject, base.gameObject, true).GetComponent<ReportScreenHeaderRow>();
		}
		this.mainRow.SetLine(reportGroup);
	}

	// Token: 0x0400471C RID: 18204
	[SerializeField]
	private ReportScreenHeaderRow rowTemplate;

	// Token: 0x0400471D RID: 18205
	private ReportScreenHeaderRow mainRow;
}
