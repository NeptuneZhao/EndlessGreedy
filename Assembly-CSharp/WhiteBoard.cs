using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B5E RID: 2910
public class WhiteBoard : KGameObjectComponentManager<WhiteBoard.Data>, IKComponentManager
{
	// Token: 0x060056F5 RID: 22261 RVA: 0x001F14C8 File Offset: 0x001EF6C8
	public HandleVector<int>.Handle Add(GameObject go)
	{
		return base.Add(go, new WhiteBoard.Data
		{
			keyValueStore = new Dictionary<HashedString, object>()
		});
	}

	// Token: 0x060056F6 RID: 22262 RVA: 0x001F14F4 File Offset: 0x001EF6F4
	protected override void OnCleanUp(HandleVector<int>.Handle h)
	{
		WhiteBoard.Data data = base.GetData(h);
		data.keyValueStore.Clear();
		data.keyValueStore = null;
		base.SetData(h, data);
	}

	// Token: 0x060056F7 RID: 22263 RVA: 0x001F1524 File Offset: 0x001EF724
	public bool HasValue(HandleVector<int>.Handle h, HashedString key)
	{
		return h.IsValid() && base.GetData(h).keyValueStore.ContainsKey(key);
	}

	// Token: 0x060056F8 RID: 22264 RVA: 0x001F1543 File Offset: 0x001EF743
	public object GetValue(HandleVector<int>.Handle h, HashedString key)
	{
		return base.GetData(h).keyValueStore[key];
	}

	// Token: 0x060056F9 RID: 22265 RVA: 0x001F1558 File Offset: 0x001EF758
	public void SetValue(HandleVector<int>.Handle h, HashedString key, object value)
	{
		if (!h.IsValid())
		{
			return;
		}
		WhiteBoard.Data data = base.GetData(h);
		data.keyValueStore[key] = value;
		base.SetData(h, data);
	}

	// Token: 0x060056FA RID: 22266 RVA: 0x001F158C File Offset: 0x001EF78C
	public void RemoveValue(HandleVector<int>.Handle h, HashedString key)
	{
		if (!h.IsValid())
		{
			return;
		}
		WhiteBoard.Data data = base.GetData(h);
		data.keyValueStore.Remove(key);
		base.SetData(h, data);
	}

	// Token: 0x02001BB4 RID: 7092
	public struct Data
	{
		// Token: 0x04008079 RID: 32889
		public Dictionary<HashedString, object> keyValueStore;
	}
}
