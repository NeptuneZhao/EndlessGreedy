using System;
using KSerialization;

// Token: 0x020008A5 RID: 2213
[SerializationConfig(MemberSerialization.OptIn)]
public class EventInstanceBase : ISaveLoadable
{
	// Token: 0x06003DE1 RID: 15841 RVA: 0x0015604C File Offset: 0x0015424C
	public EventInstanceBase(EventBase ev)
	{
		this.frame = GameClock.Instance.GetFrame();
		this.eventHash = ev.hash;
		this.ev = ev;
	}

	// Token: 0x06003DE2 RID: 15842 RVA: 0x00156078 File Offset: 0x00154278
	public override string ToString()
	{
		string str = "[" + this.frame.ToString() + "] ";
		if (this.ev != null)
		{
			return str + this.ev.GetDescription(this);
		}
		return str + "Unknown event";
	}

	// Token: 0x040025F7 RID: 9719
	[Serialize]
	public int frame;

	// Token: 0x040025F8 RID: 9720
	[Serialize]
	public int eventHash;

	// Token: 0x040025F9 RID: 9721
	public EventBase ev;
}
