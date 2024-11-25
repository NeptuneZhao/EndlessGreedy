using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000BF1 RID: 3057
public class ClusterMapPathDrawer : MonoBehaviour
{
	// Token: 0x06005D23 RID: 23843 RVA: 0x00223B40 File Offset: 0x00221D40
	public ClusterMapPath AddPath()
	{
		ClusterMapPath clusterMapPath = UnityEngine.Object.Instantiate<ClusterMapPath>(this.pathPrefab, this.pathContainer);
		clusterMapPath.Init();
		return clusterMapPath;
	}

	// Token: 0x06005D24 RID: 23844 RVA: 0x00223B59 File Offset: 0x00221D59
	public static List<Vector2> GetDrawPathList(Vector2 startLocation, List<AxialI> pathPoints)
	{
		List<Vector2> list = new List<Vector2>();
		list.Add(startLocation);
		list.AddRange(from point in pathPoints
		select point.ToWorld2D());
		return list;
	}

	// Token: 0x04003E5D RID: 15965
	public ClusterMapPath pathPrefab;

	// Token: 0x04003E5E RID: 15966
	public Transform pathContainer;
}
