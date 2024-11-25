using System;
using UnityEngine;

// Token: 0x020007A8 RID: 1960
public class CellSelectionInstantiator : MonoBehaviour
{
	// Token: 0x060035A4 RID: 13732 RVA: 0x00123A90 File Offset: 0x00121C90
	private void Awake()
	{
		GameObject gameObject = Util.KInstantiate(this.CellSelectionPrefab, null, "WorldSelectionCollider");
		GameObject gameObject2 = Util.KInstantiate(this.CellSelectionPrefab, null, "WorldSelectionCollider");
		CellSelectionObject component = gameObject.GetComponent<CellSelectionObject>();
		CellSelectionObject component2 = gameObject2.GetComponent<CellSelectionObject>();
		component.alternateSelectionObject = component2;
		component2.alternateSelectionObject = component;
	}

	// Token: 0x04001FE4 RID: 8164
	public GameObject CellSelectionPrefab;
}
