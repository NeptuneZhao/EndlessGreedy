using System;
using UnityEngine;

// Token: 0x02000CFF RID: 3327
public readonly struct OutfitBrowserScreenConfig
{
	// Token: 0x06006739 RID: 26425 RVA: 0x0026850C File Offset: 0x0026670C
	public OutfitBrowserScreenConfig(Option<ClothingOutfitUtility.OutfitType> onlyShowOutfitType, Option<ClothingOutfitTarget> selectedTarget, Option<Personality> minionPersonality, Option<GameObject> minionInstance)
	{
		this.onlyShowOutfitType = onlyShowOutfitType;
		this.selectedTarget = selectedTarget;
		this.minionPersonality = minionPersonality;
		this.isPickingOutfitForDupe = (minionPersonality.HasValue || minionInstance.HasValue);
		this.targetMinionInstance = minionInstance;
		this.isValid = true;
		if (minionPersonality.IsSome() || this.targetMinionInstance.IsSome())
		{
			global::Debug.Assert(onlyShowOutfitType.IsSome(), "If viewing outfits for a specific duplicant personality or instance, an onlyShowOutfitType must also be given.");
		}
	}

	// Token: 0x0600673A RID: 26426 RVA: 0x0026857D File Offset: 0x0026677D
	public OutfitBrowserScreenConfig WithOutfitType(Option<ClothingOutfitUtility.OutfitType> onlyShowOutfitType)
	{
		return new OutfitBrowserScreenConfig(onlyShowOutfitType, this.selectedTarget, this.minionPersonality, this.targetMinionInstance);
	}

	// Token: 0x0600673B RID: 26427 RVA: 0x00268597 File Offset: 0x00266797
	public OutfitBrowserScreenConfig WithOutfit(Option<ClothingOutfitTarget> sourceTarget)
	{
		return new OutfitBrowserScreenConfig(this.onlyShowOutfitType, sourceTarget, this.minionPersonality, this.targetMinionInstance);
	}

	// Token: 0x0600673C RID: 26428 RVA: 0x002685B4 File Offset: 0x002667B4
	public string GetMinionName()
	{
		if (this.targetMinionInstance.HasValue)
		{
			return this.targetMinionInstance.Value.GetProperName();
		}
		if (this.minionPersonality.HasValue)
		{
			return this.minionPersonality.Value.Name;
		}
		return "-";
	}

	// Token: 0x0600673D RID: 26429 RVA: 0x00268602 File Offset: 0x00266802
	public static OutfitBrowserScreenConfig Mannequin()
	{
		return new OutfitBrowserScreenConfig(Option.None, Option.None, Option.None, Option.None);
	}

	// Token: 0x0600673E RID: 26430 RVA: 0x00268631 File Offset: 0x00266831
	public static OutfitBrowserScreenConfig Minion(ClothingOutfitUtility.OutfitType onlyShowOutfitType, Personality personality)
	{
		return new OutfitBrowserScreenConfig(onlyShowOutfitType, Option.None, personality, Option.None);
	}

	// Token: 0x0600673F RID: 26431 RVA: 0x00268658 File Offset: 0x00266858
	public static OutfitBrowserScreenConfig Minion(ClothingOutfitUtility.OutfitType onlyShowOutfitType, GameObject minionInstance)
	{
		Personality value = Db.Get().Personalities.Get(minionInstance.GetComponent<MinionIdentity>().personalityResourceId);
		return new OutfitBrowserScreenConfig(onlyShowOutfitType, ClothingOutfitTarget.FromMinion(onlyShowOutfitType, minionInstance), value, minionInstance);
	}

	// Token: 0x06006740 RID: 26432 RVA: 0x002686A4 File Offset: 0x002668A4
	public static OutfitBrowserScreenConfig Minion(ClothingOutfitUtility.OutfitType onlyShowOutfitType, MinionBrowserScreen.GridItem item)
	{
		MinionBrowserScreen.GridItem.PersonalityTarget personalityTarget = item as MinionBrowserScreen.GridItem.PersonalityTarget;
		if (personalityTarget != null)
		{
			return OutfitBrowserScreenConfig.Minion(onlyShowOutfitType, personalityTarget.personality);
		}
		MinionBrowserScreen.GridItem.MinionInstanceTarget minionInstanceTarget = item as MinionBrowserScreen.GridItem.MinionInstanceTarget;
		if (minionInstanceTarget != null)
		{
			return OutfitBrowserScreenConfig.Minion(onlyShowOutfitType, minionInstanceTarget.minionInstance);
		}
		throw new NotImplementedException();
	}

	// Token: 0x06006741 RID: 26433 RVA: 0x002686E4 File Offset: 0x002668E4
	public void ApplyAndOpenScreen()
	{
		LockerNavigator.Instance.outfitBrowserScreen.GetComponent<OutfitBrowserScreen>().Configure(this);
		LockerNavigator.Instance.PushScreen(LockerNavigator.Instance.outfitBrowserScreen, null);
	}

	// Token: 0x040045A4 RID: 17828
	public readonly Option<ClothingOutfitUtility.OutfitType> onlyShowOutfitType;

	// Token: 0x040045A5 RID: 17829
	public readonly Option<ClothingOutfitTarget> selectedTarget;

	// Token: 0x040045A6 RID: 17830
	public readonly Option<Personality> minionPersonality;

	// Token: 0x040045A7 RID: 17831
	public readonly Option<GameObject> targetMinionInstance;

	// Token: 0x040045A8 RID: 17832
	public readonly bool isValid;

	// Token: 0x040045A9 RID: 17833
	public readonly bool isPickingOutfitForDupe;
}
