using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200065D RID: 1629
[AddComponentMenu("KMonoBehaviour/scripts/Baggable")]
public class Baggable : KMonoBehaviour
{
	// Token: 0x06002826 RID: 10278 RVA: 0x000E3E1C File Offset: 0x000E201C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.minionAnimOverride = Assets.GetAnim("anim_restrain_creature_kanim");
		Pickupable pickupable = base.gameObject.AddOrGet<Pickupable>();
		pickupable.workAnims = new HashedString[]
		{
			new HashedString("capture"),
			new HashedString("pickup")
		};
		pickupable.workAnimPlayMode = KAnim.PlayMode.Once;
		pickupable.workingPstComplete = null;
		pickupable.workingPstFailed = null;
		pickupable.overrideAnims = new KAnimFile[]
		{
			this.minionAnimOverride
		};
		pickupable.trackOnPickup = false;
		pickupable.useGunforPickup = this.useGunForPickup;
		pickupable.synchronizeAnims = false;
		pickupable.SetWorkTime(3f);
		if (this.mustStandOntopOfTrapForPickup)
		{
			pickupable.SetOffsets(new CellOffset[]
			{
				default(CellOffset),
				new CellOffset(0, -1)
			});
		}
		base.Subscribe<Baggable>(856640610, Baggable.OnStoreDelegate);
		if (base.transform.parent != null)
		{
			if (base.transform.parent.GetComponent<Trap>() != null || base.transform.parent.GetSMI<ReusableTrap.Instance>() != null)
			{
				base.GetComponent<KBatchedAnimController>().enabled = true;
			}
			if (base.transform.parent.GetComponent<EggIncubator>() != null)
			{
				this.wrangled = true;
			}
		}
		if (this.wrangled)
		{
			this.SetWrangled();
		}
	}

	// Token: 0x06002827 RID: 10279 RVA: 0x000E3F78 File Offset: 0x000E2178
	private void OnStore(object data)
	{
		Storage storage = data as Storage;
		if (storage != null || (data != null && (bool)data))
		{
			base.gameObject.AddTag(GameTags.Creatures.Bagged);
			if (storage && storage.HasTag(GameTags.BaseMinion))
			{
				this.SetVisible(false);
				return;
			}
		}
		else
		{
			if (!this.keepWrangledNextTimeRemovedFromStorage)
			{
				this.Free();
			}
			this.keepWrangledNextTimeRemovedFromStorage = false;
		}
	}

	// Token: 0x06002828 RID: 10280 RVA: 0x000E3FE8 File Offset: 0x000E21E8
	private void SetVisible(bool visible)
	{
		KAnimControllerBase component = base.gameObject.GetComponent<KAnimControllerBase>();
		if (component != null && component.enabled != visible)
		{
			component.enabled = visible;
		}
		KSelectable component2 = base.gameObject.GetComponent<KSelectable>();
		if (component2 != null && component2.enabled != visible)
		{
			component2.enabled = visible;
		}
	}

	// Token: 0x06002829 RID: 10281 RVA: 0x000E4040 File Offset: 0x000E2240
	public static string GetBaggedAnimName(GameObject baggableObject)
	{
		string result = "trussed";
		Pickupable pickupable = baggableObject.AddOrGet<Pickupable>();
		if (pickupable != null && pickupable.storage != null)
		{
			IBaggedStateAnimationInstructions component = pickupable.storage.GetComponent<IBaggedStateAnimationInstructions>();
			if (component != null)
			{
				string baggedAnimationName = component.GetBaggedAnimationName();
				if (baggedAnimationName != null)
				{
					result = baggedAnimationName;
				}
			}
		}
		return result;
	}

	// Token: 0x0600282A RID: 10282 RVA: 0x000E4090 File Offset: 0x000E2290
	public void SetWrangled()
	{
		this.wrangled = true;
		Navigator component = base.GetComponent<Navigator>();
		if (component && component.IsValidNavType(NavType.Floor))
		{
			component.SetCurrentNavType(NavType.Floor);
		}
		base.gameObject.AddTag(GameTags.Creatures.Bagged);
		base.GetComponent<KAnimControllerBase>().Play(Baggable.GetBaggedAnimName(base.gameObject), KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x0600282B RID: 10283 RVA: 0x000E40F9 File Offset: 0x000E22F9
	public void Free()
	{
		base.gameObject.RemoveTag(GameTags.Creatures.Bagged);
		this.wrangled = false;
		this.SetVisible(true);
	}

	// Token: 0x04001721 RID: 5921
	[SerializeField]
	private KAnimFile minionAnimOverride;

	// Token: 0x04001722 RID: 5922
	public bool mustStandOntopOfTrapForPickup;

	// Token: 0x04001723 RID: 5923
	[Serialize]
	public bool wrangled;

	// Token: 0x04001724 RID: 5924
	[Serialize]
	public bool keepWrangledNextTimeRemovedFromStorage;

	// Token: 0x04001725 RID: 5925
	public bool useGunForPickup;

	// Token: 0x04001726 RID: 5926
	private static readonly EventSystem.IntraObjectHandler<Baggable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Baggable>(delegate(Baggable component, object data)
	{
		component.OnStore(data);
	});

	// Token: 0x04001727 RID: 5927
	public const string DEFAULT_BAGGED_ANIM_NAME = "trussed";
}
