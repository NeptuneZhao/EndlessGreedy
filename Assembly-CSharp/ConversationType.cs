using System;
using UnityEngine;

// Token: 0x020007E5 RID: 2021
public class ConversationType
{
	// Token: 0x060037E3 RID: 14307 RVA: 0x00131479 File Offset: 0x0012F679
	public virtual void NewTarget(MinionIdentity speaker)
	{
	}

	// Token: 0x060037E4 RID: 14308 RVA: 0x0013147B File Offset: 0x0012F67B
	public virtual Conversation.Topic GetNextTopic(MinionIdentity speaker, Conversation.Topic lastTopic)
	{
		return null;
	}

	// Token: 0x060037E5 RID: 14309 RVA: 0x0013147E File Offset: 0x0012F67E
	public virtual Sprite GetSprite(string topic)
	{
		return null;
	}

	// Token: 0x040021AA RID: 8618
	public string id;

	// Token: 0x040021AB RID: 8619
	public string target;
}
