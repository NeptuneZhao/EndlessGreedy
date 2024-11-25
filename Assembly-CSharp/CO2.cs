using System;
using KSerialization;
using UnityEngine;

// Token: 0x020007A2 RID: 1954
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/CO2")]
public class CO2 : KMonoBehaviour
{
	// Token: 0x0600357E RID: 13694 RVA: 0x00122F1E File Offset: 0x0012111E
	public void StartLoop()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		component.Play("exhale_pre", KAnim.PlayMode.Once, 1f, 0f);
		component.Play("exhale_loop", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x0600357F RID: 13695 RVA: 0x00122F5B File Offset: 0x0012115B
	public void TriggerDestroy()
	{
		base.GetComponent<KBatchedAnimController>().Play("exhale_pst", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x04001FC1 RID: 8129
	[Serialize]
	[NonSerialized]
	public Vector3 velocity = Vector3.zero;

	// Token: 0x04001FC2 RID: 8130
	[Serialize]
	[NonSerialized]
	public float mass;

	// Token: 0x04001FC3 RID: 8131
	[Serialize]
	[NonSerialized]
	public float temperature;

	// Token: 0x04001FC4 RID: 8132
	[Serialize]
	[NonSerialized]
	public float lifetimeRemaining;
}
