using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000B3D RID: 2877
[SerializationConfig(MemberSerialization.OptOut)]
[AddComponentMenu("KMonoBehaviour/scripts/UnstableGround")]
public class UnstableGround : KMonoBehaviour
{
	// Token: 0x04003842 RID: 14402
	public SimHashes element;

	// Token: 0x04003843 RID: 14403
	public float mass;

	// Token: 0x04003844 RID: 14404
	public float temperature;

	// Token: 0x04003845 RID: 14405
	public byte diseaseIdx;

	// Token: 0x04003846 RID: 14406
	public int diseaseCount;
}
