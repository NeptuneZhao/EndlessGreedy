using System;
using UnityEngine;

// Token: 0x020009DA RID: 2522
public class OreSizeVisualizerComponents : KGameObjectComponentManager<OreSizeVisualizerData>
{
	// Token: 0x06004935 RID: 18741 RVA: 0x001A37A0 File Offset: 0x001A19A0
	public HandleVector<int>.Handle Add(GameObject go)
	{
		HandleVector<int>.Handle handle = base.Add(go, new OreSizeVisualizerData(go));
		this.OnPrefabInit(handle);
		return handle;
	}

	// Token: 0x06004936 RID: 18742 RVA: 0x001A37C4 File Offset: 0x001A19C4
	public static HashedString GetAnimForMass(float mass)
	{
		for (int i = 0; i < OreSizeVisualizerComponents.MassTiers.Length; i++)
		{
			if (mass <= OreSizeVisualizerComponents.MassTiers[i].massRequired)
			{
				return OreSizeVisualizerComponents.MassTiers[i].animName;
			}
		}
		return HashedString.Invalid;
	}

	// Token: 0x06004937 RID: 18743 RVA: 0x001A380C File Offset: 0x001A1A0C
	protected override void OnPrefabInit(HandleVector<int>.Handle handle)
	{
		Action<object> action = delegate(object ev_data)
		{
			OreSizeVisualizerComponents.OnMassChanged(handle, ev_data);
		};
		OreSizeVisualizerData data = base.GetData(handle);
		data.onMassChangedCB = action;
		data.primaryElement.Subscribe(-2064133523, action);
		data.primaryElement.Subscribe(1335436905, action);
		base.SetData(handle, data);
	}

	// Token: 0x06004938 RID: 18744 RVA: 0x001A387C File Offset: 0x001A1A7C
	protected override void OnSpawn(HandleVector<int>.Handle handle)
	{
		OreSizeVisualizerData data = base.GetData(handle);
		OreSizeVisualizerComponents.OnMassChanged(handle, data.primaryElement.GetComponent<Pickupable>());
	}

	// Token: 0x06004939 RID: 18745 RVA: 0x001A38A4 File Offset: 0x001A1AA4
	protected override void OnCleanUp(HandleVector<int>.Handle handle)
	{
		OreSizeVisualizerData data = base.GetData(handle);
		if (data.primaryElement != null)
		{
			Action<object> onMassChangedCB = data.onMassChangedCB;
			data.primaryElement.Unsubscribe(-2064133523, onMassChangedCB);
			data.primaryElement.Unsubscribe(1335436905, onMassChangedCB);
		}
	}

	// Token: 0x0600493A RID: 18746 RVA: 0x001A38F0 File Offset: 0x001A1AF0
	private static void OnMassChanged(HandleVector<int>.Handle handle, object other_data)
	{
		PrimaryElement primaryElement = GameComps.OreSizeVisualizers.GetData(handle).primaryElement;
		float num = primaryElement.Mass;
		if (other_data != null)
		{
			PrimaryElement component = ((Pickupable)other_data).GetComponent<PrimaryElement>();
			num += component.Mass;
		}
		OreSizeVisualizerComponents.MassTier massTier = default(OreSizeVisualizerComponents.MassTier);
		for (int i = 0; i < OreSizeVisualizerComponents.MassTiers.Length; i++)
		{
			if (num <= OreSizeVisualizerComponents.MassTiers[i].massRequired)
			{
				massTier = OreSizeVisualizerComponents.MassTiers[i];
				break;
			}
		}
		primaryElement.GetComponent<KBatchedAnimController>().Play(massTier.animName, KAnim.PlayMode.Once, 1f, 0f);
		KCircleCollider2D component2 = primaryElement.GetComponent<KCircleCollider2D>();
		if (component2 != null)
		{
			component2.radius = massTier.colliderRadius;
		}
		primaryElement.Trigger(1807976145, null);
	}

	// Token: 0x04002FE9 RID: 12265
	private static readonly OreSizeVisualizerComponents.MassTier[] MassTiers = new OreSizeVisualizerComponents.MassTier[]
	{
		new OreSizeVisualizerComponents.MassTier
		{
			animName = "idle1",
			massRequired = 50f,
			colliderRadius = 0.15f
		},
		new OreSizeVisualizerComponents.MassTier
		{
			animName = "idle2",
			massRequired = 600f,
			colliderRadius = 0.2f
		},
		new OreSizeVisualizerComponents.MassTier
		{
			animName = "idle3",
			massRequired = float.MaxValue,
			colliderRadius = 0.25f
		}
	};

	// Token: 0x020019DD RID: 6621
	private struct MassTier
	{
		// Token: 0x04007ABE RID: 31422
		public HashedString animName;

		// Token: 0x04007ABF RID: 31423
		public float massRequired;

		// Token: 0x04007AC0 RID: 31424
		public float colliderRadius;
	}
}
