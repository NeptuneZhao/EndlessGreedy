using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000CCD RID: 3277
[AddComponentMenu("KMonoBehaviour/scripts/Messenger")]
public class Messenger : KMonoBehaviour
{
	// Token: 0x17000754 RID: 1876
	// (get) Token: 0x0600655C RID: 25948 RVA: 0x0025DEA9 File Offset: 0x0025C0A9
	public int Count
	{
		get
		{
			return this.messages.Count;
		}
	}

	// Token: 0x0600655D RID: 25949 RVA: 0x0025DEB6 File Offset: 0x0025C0B6
	public IEnumerator<Message> GetEnumerator()
	{
		return this.messages.GetEnumerator();
	}

	// Token: 0x0600655E RID: 25950 RVA: 0x0025DEC3 File Offset: 0x0025C0C3
	public static void DestroyInstance()
	{
		Messenger.Instance = null;
	}

	// Token: 0x17000755 RID: 1877
	// (get) Token: 0x0600655F RID: 25951 RVA: 0x0025DECB File Offset: 0x0025C0CB
	public SerializedList<Message> Messages
	{
		get
		{
			return this.messages;
		}
	}

	// Token: 0x06006560 RID: 25952 RVA: 0x0025DED3 File Offset: 0x0025C0D3
	protected override void OnPrefabInit()
	{
		Messenger.Instance = this;
	}

	// Token: 0x06006561 RID: 25953 RVA: 0x0025DEDC File Offset: 0x0025C0DC
	protected override void OnSpawn()
	{
		int i = 0;
		while (i < this.messages.Count)
		{
			if (this.messages[i].IsValid())
			{
				i++;
			}
			else
			{
				this.messages.RemoveAt(i);
			}
		}
		base.Trigger(-599791736, null);
	}

	// Token: 0x06006562 RID: 25954 RVA: 0x0025DF2C File Offset: 0x0025C12C
	public void QueueMessage(Message message)
	{
		this.messages.Add(message);
		base.Trigger(1558809273, message);
	}

	// Token: 0x06006563 RID: 25955 RVA: 0x0025DF48 File Offset: 0x0025C148
	public Message DequeueMessage()
	{
		Message result = null;
		if (this.messages.Count > 0)
		{
			result = this.messages[0];
			this.messages.RemoveAt(0);
		}
		return result;
	}

	// Token: 0x06006564 RID: 25956 RVA: 0x0025DF80 File Offset: 0x0025C180
	public void ClearAllMessages()
	{
		for (int i = this.messages.Count - 1; i >= 0; i--)
		{
			this.messages.RemoveAt(i);
		}
	}

	// Token: 0x06006565 RID: 25957 RVA: 0x0025DFB1 File Offset: 0x0025C1B1
	public void RemoveMessage(Message m)
	{
		this.messages.Remove(m);
		base.Trigger(-599791736, null);
	}

	// Token: 0x04004486 RID: 17542
	[Serialize]
	private SerializedList<Message> messages = new SerializedList<Message>();

	// Token: 0x04004487 RID: 17543
	public static Messenger Instance;
}
