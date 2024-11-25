using System;
using KSerialization;

// Token: 0x02000CD4 RID: 3284
public abstract class TargetMessage : Message
{
	// Token: 0x06006593 RID: 26003 RVA: 0x0025E6DE File Offset: 0x0025C8DE
	protected TargetMessage()
	{
	}

	// Token: 0x06006594 RID: 26004 RVA: 0x0025E6E6 File Offset: 0x0025C8E6
	public TargetMessage(KPrefabID prefab_id)
	{
		this.target = new MessageTarget(prefab_id);
	}

	// Token: 0x06006595 RID: 26005 RVA: 0x0025E6FA File Offset: 0x0025C8FA
	public MessageTarget GetTarget()
	{
		return this.target;
	}

	// Token: 0x06006596 RID: 26006 RVA: 0x0025E702 File Offset: 0x0025C902
	public override void OnCleanUp()
	{
		this.target.OnCleanUp();
	}

	// Token: 0x0400449A RID: 17562
	[Serialize]
	private MessageTarget target;
}
