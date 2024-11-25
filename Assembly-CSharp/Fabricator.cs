using System;
using KSerialization;
using UnityEngine;

// Token: 0x020006CB RID: 1739
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Fabricator")]
public class Fabricator : KMonoBehaviour
{
	// Token: 0x06002C02 RID: 11266 RVA: 0x000F7445 File Offset: 0x000F5645
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}
}
