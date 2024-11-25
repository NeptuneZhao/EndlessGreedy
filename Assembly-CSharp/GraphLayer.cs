using System;
using UnityEngine;

// Token: 0x02000C56 RID: 3158
[RequireComponent(typeof(GraphBase))]
[AddComponentMenu("KMonoBehaviour/scripts/GraphLayer")]
public class GraphLayer : KMonoBehaviour
{
	// Token: 0x17000736 RID: 1846
	// (get) Token: 0x0600610A RID: 24842 RVA: 0x00241F7B File Offset: 0x0024017B
	public GraphBase graph
	{
		get
		{
			if (this.graph_base == null)
			{
				this.graph_base = base.GetComponent<GraphBase>();
			}
			return this.graph_base;
		}
	}

	// Token: 0x040041B9 RID: 16825
	[MyCmpReq]
	private GraphBase graph_base;
}
