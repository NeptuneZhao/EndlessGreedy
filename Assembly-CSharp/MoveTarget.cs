using System;
using UnityEngine;

// Token: 0x020009B5 RID: 2485
[AddComponentMenu("KMonoBehaviour/scripts/MoveTarget")]
public class MoveTarget : KMonoBehaviour
{
	// Token: 0x0600482E RID: 18478 RVA: 0x0019D78C File Offset: 0x0019B98C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.gameObject.hideFlags = (HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.DontSaveInEditor | HideFlags.NotEditable | HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset);
	}
}
