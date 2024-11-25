using System;
using UnityEngine;

// Token: 0x02000D03 RID: 3331
public readonly struct OutfitDesignerScreenConfig
{
	// Token: 0x06006767 RID: 26471 RVA: 0x00269CD8 File Offset: 0x00267ED8
	public OutfitDesignerScreenConfig(ClothingOutfitTarget sourceTarget, Option<Personality> minionPersonality, Option<GameObject> targetMinionInstance, Action<ClothingOutfitTarget> onWriteToOutfitTargetFn = null)
	{
		this.sourceTarget = sourceTarget;
		this.outfitTemplate = (sourceTarget.IsTemplateOutfit() ? Option.Some<ClothingOutfitTarget>(sourceTarget) : Option.None);
		this.minionPersonality = minionPersonality;
		this.targetMinionInstance = targetMinionInstance;
		this.onWriteToOutfitTargetFn = onWriteToOutfitTargetFn;
		this.isValid = true;
		ClothingOutfitTarget.MinionInstance minionInstance;
		if (sourceTarget.Is<ClothingOutfitTarget.MinionInstance>(out minionInstance))
		{
			global::Debug.Assert(targetMinionInstance.HasValue && targetMinionInstance == minionInstance.minionInstance);
		}
	}

	// Token: 0x06006768 RID: 26472 RVA: 0x00269D52 File Offset: 0x00267F52
	public OutfitDesignerScreenConfig WithOutfit(ClothingOutfitTarget sourceTarget)
	{
		return new OutfitDesignerScreenConfig(sourceTarget, this.minionPersonality, this.targetMinionInstance, this.onWriteToOutfitTargetFn);
	}

	// Token: 0x06006769 RID: 26473 RVA: 0x00269D6C File Offset: 0x00267F6C
	public OutfitDesignerScreenConfig OnWriteToOutfitTarget(Action<ClothingOutfitTarget> onWriteToOutfitTargetFn)
	{
		return new OutfitDesignerScreenConfig(this.sourceTarget, this.minionPersonality, this.targetMinionInstance, onWriteToOutfitTargetFn);
	}

	// Token: 0x0600676A RID: 26474 RVA: 0x00269D86 File Offset: 0x00267F86
	public static OutfitDesignerScreenConfig Mannequin(ClothingOutfitTarget outfit)
	{
		return new OutfitDesignerScreenConfig(outfit, Option.None, Option.None, null);
	}

	// Token: 0x0600676B RID: 26475 RVA: 0x00269DA3 File Offset: 0x00267FA3
	public static OutfitDesignerScreenConfig Minion(ClothingOutfitTarget outfit, Personality personality)
	{
		return new OutfitDesignerScreenConfig(outfit, personality, Option.None, null);
	}

	// Token: 0x0600676C RID: 26476 RVA: 0x00269DBC File Offset: 0x00267FBC
	public static OutfitDesignerScreenConfig Minion(ClothingOutfitTarget outfit, GameObject targetMinionInstance)
	{
		Personality value = Db.Get().Personalities.Get(targetMinionInstance.GetComponent<MinionIdentity>().personalityResourceId);
		ClothingOutfitTarget.MinionInstance minionInstance;
		global::Debug.Assert(outfit.Is<ClothingOutfitTarget.MinionInstance>(out minionInstance));
		global::Debug.Assert(minionInstance.minionInstance == targetMinionInstance);
		return new OutfitDesignerScreenConfig(outfit, value, targetMinionInstance, null);
	}

	// Token: 0x0600676D RID: 26477 RVA: 0x00269E18 File Offset: 0x00268018
	public static OutfitDesignerScreenConfig Minion(ClothingOutfitTarget outfit, MinionBrowserScreen.GridItem item)
	{
		MinionBrowserScreen.GridItem.PersonalityTarget personalityTarget = item as MinionBrowserScreen.GridItem.PersonalityTarget;
		if (personalityTarget != null)
		{
			return OutfitDesignerScreenConfig.Minion(outfit, personalityTarget.personality);
		}
		MinionBrowserScreen.GridItem.MinionInstanceTarget minionInstanceTarget = item as MinionBrowserScreen.GridItem.MinionInstanceTarget;
		if (minionInstanceTarget != null)
		{
			return OutfitDesignerScreenConfig.Minion(outfit, minionInstanceTarget.minionInstance);
		}
		throw new NotImplementedException();
	}

	// Token: 0x0600676E RID: 26478 RVA: 0x00269E58 File Offset: 0x00268058
	public void ApplyAndOpenScreen()
	{
		LockerNavigator.Instance.outfitDesignerScreen.GetComponent<OutfitDesignerScreen>().Configure(this);
		LockerNavigator.Instance.PushScreen(LockerNavigator.Instance.outfitDesignerScreen, null);
	}

	// Token: 0x040045D0 RID: 17872
	public readonly ClothingOutfitTarget sourceTarget;

	// Token: 0x040045D1 RID: 17873
	public readonly Option<ClothingOutfitTarget> outfitTemplate;

	// Token: 0x040045D2 RID: 17874
	public readonly Option<Personality> minionPersonality;

	// Token: 0x040045D3 RID: 17875
	public readonly Option<GameObject> targetMinionInstance;

	// Token: 0x040045D4 RID: 17876
	public readonly Action<ClothingOutfitTarget> onWriteToOutfitTargetFn;

	// Token: 0x040045D5 RID: 17877
	public readonly bool isValid;
}
