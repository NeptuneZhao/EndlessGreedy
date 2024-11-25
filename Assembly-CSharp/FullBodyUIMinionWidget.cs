using System;
using System.Linq;
using Database;
using UnityEngine;

// Token: 0x02000C4B RID: 3147
public class FullBodyUIMinionWidget : KMonoBehaviour
{
	// Token: 0x17000733 RID: 1843
	// (get) Token: 0x060060AD RID: 24749 RVA: 0x0023F809 File Offset: 0x0023DA09
	// (set) Token: 0x060060AE RID: 24750 RVA: 0x0023F811 File Offset: 0x0023DA11
	public KBatchedAnimController animController { get; private set; }

	// Token: 0x060060AF RID: 24751 RVA: 0x0023F81A File Offset: 0x0023DA1A
	protected override void OnSpawn()
	{
		this.TrySpawnDisplayMinion();
	}

	// Token: 0x060060B0 RID: 24752 RVA: 0x0023F824 File Offset: 0x0023DA24
	private void TrySpawnDisplayMinion()
	{
		if (this.animController == null)
		{
			this.animController = Util.KInstantiateUI(Assets.GetPrefab(new Tag("FullMinionUIPortrait")), this.duplicantAnimAnchor.gameObject, false).GetComponent<KBatchedAnimController>();
			this.animController.gameObject.SetActive(true);
			this.animController.animScale = 0.38f;
		}
	}

	// Token: 0x060060B1 RID: 24753 RVA: 0x0023F88C File Offset: 0x0023DA8C
	private void InitializeAnimator()
	{
		this.TrySpawnDisplayMinion();
		this.animController.Queue("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
		Accessorizer component = this.animController.GetComponent<Accessorizer>();
		for (int i = component.GetAccessories().Count - 1; i >= 0; i--)
		{
			component.RemoveAccessory(component.GetAccessories()[i].Get());
		}
	}

	// Token: 0x060060B2 RID: 24754 RVA: 0x0023F8FC File Offset: 0x0023DAFC
	public void SetDefaultPortraitAnimator()
	{
		MinionIdentity minionIdentity = (Components.MinionIdentities.Count > 0) ? Components.MinionIdentities[0] : null;
		HashedString id = (minionIdentity != null) ? minionIdentity.personalityResourceId : Db.Get().Personalities.resources.GetRandom<Personality>().Id;
		this.InitializeAnimator();
		this.animController.GetComponent<Accessorizer>().ApplyMinionPersonality(Db.Get().Personalities.Get(id));
		Accessorizer accessorizer = (minionIdentity != null) ? minionIdentity.GetComponent<Accessorizer>() : null;
		KAnim.Build.Symbol hair_symbol = null;
		KAnim.Build.Symbol hat_hair_symbol = null;
		if (accessorizer)
		{
			hair_symbol = accessorizer.GetAccessory(Db.Get().AccessorySlots.Hair).symbol;
			hat_hair_symbol = Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(accessorizer.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol;
		}
		this.UpdateHatOverride(null, hair_symbol, hat_hair_symbol);
		this.UpdateClothingOverride(this.animController.GetComponent<SymbolOverrideController>(), minionIdentity, null);
	}

	// Token: 0x060060B3 RID: 24755 RVA: 0x0023FA24 File Offset: 0x0023DC24
	public void SetPortraitAnimator(IAssignableIdentity assignableIdentity)
	{
		if (assignableIdentity == null || assignableIdentity.IsNull())
		{
			this.SetDefaultPortraitAnimator();
			return;
		}
		this.InitializeAnimator();
		string current_hat = "";
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.GetMinionIdentity(assignableIdentity, out minionIdentity, out storedMinionIdentity);
		Accessorizer accessorizer = null;
		Accessorizer component = this.animController.GetComponent<Accessorizer>();
		KAnim.Build.Symbol hair_symbol = null;
		KAnim.Build.Symbol hat_hair_symbol = null;
		if (minionIdentity != null)
		{
			accessorizer = minionIdentity.GetComponent<Accessorizer>();
			foreach (ResourceRef<Accessory> resourceRef in accessorizer.GetAccessories())
			{
				component.AddAccessory(resourceRef.Get());
			}
			current_hat = minionIdentity.GetComponent<MinionResume>().CurrentHat;
			hair_symbol = accessorizer.GetAccessory(Db.Get().AccessorySlots.Hair).symbol;
			hat_hair_symbol = Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(accessorizer.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol;
		}
		else if (storedMinionIdentity != null)
		{
			foreach (ResourceRef<Accessory> resourceRef2 in storedMinionIdentity.accessories)
			{
				component.AddAccessory(resourceRef2.Get());
			}
			current_hat = storedMinionIdentity.currentHat;
			hair_symbol = storedMinionIdentity.GetAccessory(Db.Get().AccessorySlots.Hair).symbol;
			hat_hair_symbol = Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(storedMinionIdentity.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol;
		}
		this.UpdateHatOverride(current_hat, hair_symbol, hat_hair_symbol);
		this.UpdateClothingOverride(this.animController.GetComponent<SymbolOverrideController>(), minionIdentity, storedMinionIdentity);
	}

	// Token: 0x060060B4 RID: 24756 RVA: 0x0023FC34 File Offset: 0x0023DE34
	private void UpdateHatOverride(string current_hat, KAnim.Build.Symbol hair_symbol, KAnim.Build.Symbol hat_hair_symbol)
	{
		AccessorySlot hat = Db.Get().AccessorySlots.Hat;
		this.animController.SetSymbolVisiblity(hat.targetSymbolId, !string.IsNullOrEmpty(current_hat));
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, string.IsNullOrEmpty(current_hat));
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, !string.IsNullOrEmpty(current_hat));
		SymbolOverrideController component = this.animController.GetComponent<SymbolOverrideController>();
		if (hair_symbol != null)
		{
			component.AddSymbolOverride("snapto_hair_always", hair_symbol, 1);
		}
		if (hat_hair_symbol != null)
		{
			component.AddSymbolOverride(Db.Get().AccessorySlots.HatHair.targetSymbolId, hat_hair_symbol, 1);
		}
	}

	// Token: 0x060060B5 RID: 24757 RVA: 0x0023FD0C File Offset: 0x0023DF0C
	private void UpdateClothingOverride(SymbolOverrideController symbolOverrideController, MinionIdentity identity, StoredMinionIdentity storedMinionIdentity)
	{
		string[] array = null;
		if (identity != null)
		{
			array = identity.GetComponent<WearableAccessorizer>().GetClothingItemsIds(ClothingOutfitUtility.OutfitType.Clothing);
		}
		else if (storedMinionIdentity != null)
		{
			array = storedMinionIdentity.GetClothingItemIds(ClothingOutfitUtility.OutfitType.Clothing);
		}
		if (array != null)
		{
			this.animController.GetComponent<WearableAccessorizer>().ApplyClothingItems(ClothingOutfitUtility.OutfitType.Clothing, from i in array
			select Db.Get().Permits.ClothingItems.Get(i));
		}
	}

	// Token: 0x060060B6 RID: 24758 RVA: 0x0023FD7D File Offset: 0x0023DF7D
	public void UpdateEquipment(Equippable equippable, KAnimFile animFile)
	{
		this.animController.GetComponent<WearableAccessorizer>().ApplyEquipment(equippable, animFile);
	}

	// Token: 0x060060B7 RID: 24759 RVA: 0x0023FD91 File Offset: 0x0023DF91
	public void RemoveEquipment(Equippable equippable)
	{
		this.animController.GetComponent<WearableAccessorizer>().RemoveEquipment(equippable);
	}

	// Token: 0x060060B8 RID: 24760 RVA: 0x0023FDA4 File Offset: 0x0023DFA4
	private void GetMinionIdentity(IAssignableIdentity assignableIdentity, out MinionIdentity minionIdentity, out StoredMinionIdentity storedMinionIdentity)
	{
		if (assignableIdentity is MinionAssignablesProxy)
		{
			minionIdentity = ((MinionAssignablesProxy)assignableIdentity).GetTargetGameObject().GetComponent<MinionIdentity>();
			storedMinionIdentity = ((MinionAssignablesProxy)assignableIdentity).GetTargetGameObject().GetComponent<StoredMinionIdentity>();
			return;
		}
		minionIdentity = (assignableIdentity as MinionIdentity);
		storedMinionIdentity = (assignableIdentity as StoredMinionIdentity);
	}

	// Token: 0x0400415B RID: 16731
	[SerializeField]
	private GameObject duplicantAnimAnchor;

	// Token: 0x0400415D RID: 16733
	public const float UI_MINION_PORTRAIT_ANIM_SCALE = 0.38f;
}
