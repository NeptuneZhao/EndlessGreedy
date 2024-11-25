using System;
using UnityEngine;

// Token: 0x02000834 RID: 2100
[AddComponentMenu("KMonoBehaviour/scripts/Decomposer")]
public class Decomposer : KMonoBehaviour
{
	// Token: 0x06003A60 RID: 14944 RVA: 0x0013F868 File Offset: 0x0013DA68
	protected override void OnSpawn()
	{
		base.OnSpawn();
		StateMachineController component = base.GetComponent<StateMachineController>();
		if (component == null)
		{
			return;
		}
		DecompositionMonitor.Instance instance = new DecompositionMonitor.Instance(this, null, 1f, false);
		component.AddStateMachineInstance(instance);
		instance.StartSM();
		instance.dirtyWaterMaxRange = 3;
	}
}
