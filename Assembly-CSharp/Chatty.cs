using System;
using System.Collections.Generic;

// Token: 0x020007AC RID: 1964
public class Chatty : KMonoBehaviour, ISimEveryTick
{
	// Token: 0x060035BC RID: 13756 RVA: 0x001246BE File Offset: 0x001228BE
	protected override void OnPrefabInit()
	{
		base.GetComponent<KPrefabID>().AddTag(GameTags.AlwaysConverse, false);
		base.Subscribe(-594200555, new Action<object>(this.OnStartedTalking));
		this.identity = base.GetComponent<MinionIdentity>();
	}

	// Token: 0x060035BD RID: 13757 RVA: 0x001246F8 File Offset: 0x001228F8
	private void OnStartedTalking(object data)
	{
		MinionIdentity minionIdentity = data as MinionIdentity;
		if (minionIdentity == null)
		{
			return;
		}
		this.conversationPartners.Add(minionIdentity);
	}

	// Token: 0x060035BE RID: 13758 RVA: 0x00124724 File Offset: 0x00122924
	public void SimEveryTick(float dt)
	{
		if (this.conversationPartners.Count == 0)
		{
			return;
		}
		for (int i = this.conversationPartners.Count - 1; i >= 0; i--)
		{
			MinionIdentity minionIdentity = this.conversationPartners[i];
			this.conversationPartners.RemoveAt(i);
			if (!(minionIdentity == this.identity))
			{
				minionIdentity.AddTag(GameTags.PleasantConversation);
			}
		}
		base.gameObject.AddTag(GameTags.PleasantConversation);
	}

	// Token: 0x04002006 RID: 8198
	private MinionIdentity identity;

	// Token: 0x04002007 RID: 8199
	private List<MinionIdentity> conversationPartners = new List<MinionIdentity>();
}
