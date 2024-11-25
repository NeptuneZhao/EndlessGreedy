using System;
using UnityEngine;

// Token: 0x02000470 RID: 1136
[AddComponentMenu("KMonoBehaviour/scripts/User")]
public class User : KMonoBehaviour
{
	// Token: 0x06001876 RID: 6262 RVA: 0x00082CAB File Offset: 0x00080EAB
	public void OnStateMachineStop(string reason, StateMachine.Status status)
	{
		if (status == StateMachine.Status.Success)
		{
			base.Trigger(58624316, null);
			return;
		}
		base.Trigger(1572098533, null);
	}
}
