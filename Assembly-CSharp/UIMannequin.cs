using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using UnityEngine;

// Token: 0x02000DE1 RID: 3553
public class UIMannequin : KMonoBehaviour, UIMinionOrMannequin.ITarget
{
	// Token: 0x170007D6 RID: 2006
	// (get) Token: 0x060070DB RID: 28891 RVA: 0x002ABA94 File Offset: 0x002A9C94
	public GameObject SpawnedAvatar
	{
		get
		{
			if (this.spawn == null)
			{
				this.TrySpawn();
			}
			return this.spawn;
		}
	}

	// Token: 0x170007D7 RID: 2007
	// (get) Token: 0x060070DC RID: 28892 RVA: 0x002ABAB0 File Offset: 0x002A9CB0
	public Option<Personality> Personality
	{
		get
		{
			return default(Option<Personality>);
		}
	}

	// Token: 0x060070DD RID: 28893 RVA: 0x002ABAC6 File Offset: 0x002A9CC6
	protected override void OnSpawn()
	{
		this.TrySpawn();
	}

	// Token: 0x060070DE RID: 28894 RVA: 0x002ABAD0 File Offset: 0x002A9CD0
	public void TrySpawn()
	{
		if (this.animController == null)
		{
			this.animController = Util.KInstantiateUI(Assets.GetPrefab(MannequinUIPortrait.ID), base.gameObject, false).GetComponent<KBatchedAnimController>();
			this.animController.LoadAnims();
			this.animController.gameObject.SetActive(true);
			this.animController.animScale = 0.38f;
			this.animController.Play("idle", KAnim.PlayMode.Paused, 1f, 0f);
			this.spawn = this.animController.gameObject;
			BaseMinionConfig.ConfigureSymbols(this.spawn, false);
			base.gameObject.AddOrGet<MinionVoiceProviderMB>().voice = Option.None;
		}
	}

	// Token: 0x060070DF RID: 28895 RVA: 0x002ABB98 File Offset: 0x002A9D98
	public void SetOutfit(ClothingOutfitUtility.OutfitType outfitType, IEnumerable<ClothingItemResource> outfit)
	{
		bool flag = outfit.Count<ClothingItemResource>() == 0;
		if (this.shouldShowOutfitWithDefaultItems)
		{
			outfit = UIMinionOrMannequinITargetExtensions.GetOutfitWithDefaultItems(outfitType, outfit);
		}
		this.SpawnedAvatar.GetComponent<SymbolOverrideController>().RemoveAllSymbolOverrides(0);
		BaseMinionConfig.ConfigureSymbols(this.SpawnedAvatar, false);
		Accessorizer component = this.SpawnedAvatar.GetComponent<Accessorizer>();
		WearableAccessorizer component2 = this.SpawnedAvatar.GetComponent<WearableAccessorizer>();
		component.ApplyMinionPersonality(this.personalityToUseForDefaultClothing.UnwrapOr(Db.Get().Personalities.Get("ABE"), null));
		component2.ClearClothingItems(null);
		component2.ApplyClothingItems(outfitType, outfit);
		List<KAnimHashedString> list = new List<KAnimHashedString>(32);
		if (this.shouldShowOutfitWithDefaultItems && outfitType == ClothingOutfitUtility.OutfitType.Clothing)
		{
			list.Add("foot");
			list.Add("hand_paint");
			if (flag)
			{
				list.Add("belt");
			}
			if (!(from item in outfit
			select item.Category).Contains(PermitCategory.DupeTops))
			{
				list.Add("torso");
				list.Add("neck");
				list.Add("arm_lower");
				list.Add("arm_lower_sleeve");
				list.Add("arm_sleeve");
				list.Add("cuff");
			}
			if (!(from item in outfit
			select item.Category).Contains(PermitCategory.DupeGloves))
			{
				list.Add("arm_lower_sleeve");
				list.Add("cuff");
			}
			if (!(from item in outfit
			select item.Category).Contains(PermitCategory.DupeBottoms))
			{
				list.Add("leg");
				list.Add("pelvis");
			}
		}
		KAnimHashedString[] source = outfit.SelectMany((ClothingItemResource item) => from s in item.AnimFile.GetData().build.symbols
		select s.hash).Concat(list).ToArray<KAnimHashedString>();
		foreach (KAnim.Build.Symbol symbol in this.animController.AnimFiles[0].GetData().build.symbols)
		{
			if (symbol.hash == "mannequin_arm" || symbol.hash == "mannequin_body" || symbol.hash == "mannequin_headshape" || symbol.hash == "mannequin_leg")
			{
				this.animController.SetSymbolVisiblity(symbol.hash, true);
			}
			else
			{
				this.animController.SetSymbolVisiblity(symbol.hash, source.Contains(symbol.hash));
			}
		}
	}

	// Token: 0x060070E0 RID: 28896 RVA: 0x002ABEA8 File Offset: 0x002AA0A8
	private static ClothingItemResource GetItemForCategory(PermitCategory category, IEnumerable<ClothingItemResource> outfit)
	{
		foreach (ClothingItemResource clothingItemResource in outfit)
		{
			if (clothingItemResource.Category == category)
			{
				return clothingItemResource;
			}
		}
		return null;
	}

	// Token: 0x060070E1 RID: 28897 RVA: 0x002ABEFC File Offset: 0x002AA0FC
	public void React(UIMinionOrMannequinReactSource source)
	{
		this.animController.Play("idle", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x04004D9C RID: 19868
	public const float ANIM_SCALE = 0.38f;

	// Token: 0x04004D9D RID: 19869
	private KBatchedAnimController animController;

	// Token: 0x04004D9E RID: 19870
	private GameObject spawn;

	// Token: 0x04004D9F RID: 19871
	public bool shouldShowOutfitWithDefaultItems = true;

	// Token: 0x04004DA0 RID: 19872
	public Option<Personality> personalityToUseForDefaultClothing;
}
