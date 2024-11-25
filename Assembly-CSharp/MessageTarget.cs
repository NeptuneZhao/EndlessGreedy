using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000CCC RID: 3276
[SerializationConfig(MemberSerialization.OptIn)]
public class MessageTarget : ISaveLoadable
{
	// Token: 0x06006556 RID: 25942 RVA: 0x0025DD08 File Offset: 0x0025BF08
	public MessageTarget(KPrefabID prefab_id)
	{
		this.prefabId.Set(prefab_id);
		this.position = prefab_id.transform.GetPosition();
		this.name = "Unknown";
		KSelectable component = prefab_id.GetComponent<KSelectable>();
		if (component != null)
		{
			this.name = component.GetName();
		}
		prefab_id.Subscribe(-1940207677, new Action<object>(this.OnAbsorbedBy));
	}

	// Token: 0x06006557 RID: 25943 RVA: 0x0025DD82 File Offset: 0x0025BF82
	public Vector3 GetPosition()
	{
		if (this.prefabId.Get() != null)
		{
			return this.prefabId.Get().transform.GetPosition();
		}
		return this.position;
	}

	// Token: 0x06006558 RID: 25944 RVA: 0x0025DDB3 File Offset: 0x0025BFB3
	public KSelectable GetSelectable()
	{
		if (this.prefabId.Get() != null)
		{
			return this.prefabId.Get().transform.GetComponent<KSelectable>();
		}
		return null;
	}

	// Token: 0x06006559 RID: 25945 RVA: 0x0025DDDF File Offset: 0x0025BFDF
	public string GetName()
	{
		return this.name;
	}

	// Token: 0x0600655A RID: 25946 RVA: 0x0025DDE8 File Offset: 0x0025BFE8
	private void OnAbsorbedBy(object data)
	{
		if (this.prefabId.Get() != null)
		{
			this.prefabId.Get().Unsubscribe(-1940207677, new Action<object>(this.OnAbsorbedBy));
		}
		KPrefabID component = ((GameObject)data).GetComponent<KPrefabID>();
		component.Subscribe(-1940207677, new Action<object>(this.OnAbsorbedBy));
		this.prefabId.Set(component);
	}

	// Token: 0x0600655B RID: 25947 RVA: 0x0025DE5C File Offset: 0x0025C05C
	public void OnCleanUp()
	{
		if (this.prefabId.Get() != null)
		{
			this.prefabId.Get().Unsubscribe(-1940207677, new Action<object>(this.OnAbsorbedBy));
			this.prefabId.Set(null);
		}
	}

	// Token: 0x04004483 RID: 17539
	[Serialize]
	private Ref<KPrefabID> prefabId = new Ref<KPrefabID>();

	// Token: 0x04004484 RID: 17540
	[Serialize]
	private Vector3 position;

	// Token: 0x04004485 RID: 17541
	[Serialize]
	private string name;
}
