using System;
using KSerialization;

// Token: 0x020004CC RID: 1228
[SerializationConfig(MemberSerialization.OptIn)]
public abstract class StateMachineComponent : KMonoBehaviour, ISaveLoadable, IStateMachineTarget
{
	// Token: 0x06001A6A RID: 6762
	public abstract StateMachine.Instance GetSMI();

	// Token: 0x04000F0B RID: 3851
	[MyCmpAdd]
	protected StateMachineController stateMachineController;
}
