using System;
using System.Collections.Generic;
using Database;
using UnityEngine;

// Token: 0x02000DE2 RID: 3554
public class UIMinion : KMonoBehaviour, UIMinionOrMannequin.ITarget
{
	// Token: 0x170007D8 RID: 2008
	// (get) Token: 0x060070E3 RID: 28899 RVA: 0x002ABF2D File Offset: 0x002AA12D
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

	// Token: 0x170007D9 RID: 2009
	// (get) Token: 0x060070E4 RID: 28900 RVA: 0x002ABF49 File Offset: 0x002AA149
	// (set) Token: 0x060070E5 RID: 28901 RVA: 0x002ABF51 File Offset: 0x002AA151
	public Option<Personality> Personality { get; private set; }

	// Token: 0x060070E6 RID: 28902 RVA: 0x002ABF5A File Offset: 0x002AA15A
	protected override void OnSpawn()
	{
		this.TrySpawn();
	}

	// Token: 0x060070E7 RID: 28903 RVA: 0x002ABF64 File Offset: 0x002AA164
	public void TrySpawn()
	{
		if (this.animController == null)
		{
			this.animController = Util.KInstantiateUI(Assets.GetPrefab(MinionUIPortrait.ID), base.gameObject, false).GetComponent<KBatchedAnimController>();
			this.animController.gameObject.SetActive(true);
			this.animController.animScale = 0.38f;
			this.animController.Play("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
			BaseMinionConfig.ConfigureSymbols(this.animController.gameObject, true);
			this.spawn = this.animController.gameObject;
		}
	}

	// Token: 0x060070E8 RID: 28904 RVA: 0x002AC00B File Offset: 0x002AA20B
	public void SetMinion(Personality personality)
	{
		this.SpawnedAvatar.GetComponent<Accessorizer>().ApplyMinionPersonality(personality);
		this.Personality = personality;
		base.gameObject.AddOrGet<MinionVoiceProviderMB>().voice = MinionVoice.ByPersonality(personality);
	}

	// Token: 0x060070E9 RID: 28905 RVA: 0x002AC048 File Offset: 0x002AA248
	public void SetOutfit(ClothingOutfitUtility.OutfitType outfitType, IEnumerable<ClothingItemResource> outfit)
	{
		outfit = UIMinionOrMannequinITargetExtensions.GetOutfitWithDefaultItems(outfitType, outfit);
		WearableAccessorizer component = this.SpawnedAvatar.GetComponent<WearableAccessorizer>();
		component.ClearClothingItems(null);
		component.ApplyClothingItems(outfitType, outfit);
	}

	// Token: 0x060070EA RID: 28906 RVA: 0x002AC080 File Offset: 0x002AA280
	public MinionVoice GetMinionVoice()
	{
		return MinionVoice.ByObject(this.SpawnedAvatar).UnwrapOr(MinionVoice.Random(), null);
	}

	// Token: 0x060070EB RID: 28907 RVA: 0x002AC0A8 File Offset: 0x002AA2A8
	public void React(UIMinionOrMannequinReactSource source)
	{
		if (source != UIMinionOrMannequinReactSource.OnPersonalityChanged && this.lastReactSource == source)
		{
			KAnim.Anim currentAnim = this.animController.GetCurrentAnim();
			if (currentAnim != null && currentAnim.name != "idle_default")
			{
				return;
			}
		}
		switch (source)
		{
		case UIMinionOrMannequinReactSource.OnPersonalityChanged:
			this.animController.Play("react", KAnim.PlayMode.Once, 1f, 0f);
			goto IL_195;
		case UIMinionOrMannequinReactSource.OnWholeOutfitChanged:
		case UIMinionOrMannequinReactSource.OnBottomChanged:
			this.animController.Play("react_bottoms", KAnim.PlayMode.Once, 1f, 0f);
			goto IL_195;
		case UIMinionOrMannequinReactSource.OnHatChanged:
			this.animController.Play("react_glasses", KAnim.PlayMode.Once, 1f, 0f);
			goto IL_195;
		case UIMinionOrMannequinReactSource.OnTopChanged:
			this.animController.Play("react_tops", KAnim.PlayMode.Once, 1f, 0f);
			goto IL_195;
		case UIMinionOrMannequinReactSource.OnGlovesChanged:
			this.animController.Play("react_gloves", KAnim.PlayMode.Once, 1f, 0f);
			goto IL_195;
		case UIMinionOrMannequinReactSource.OnShoesChanged:
			this.animController.Play("react_shoes", KAnim.PlayMode.Once, 1f, 0f);
			goto IL_195;
		}
		this.animController.Play("cheer_pre", KAnim.PlayMode.Once, 1f, 0f);
		this.animController.Queue("cheer_loop", KAnim.PlayMode.Once, 1f, 0f);
		this.animController.Queue("cheer_pst", KAnim.PlayMode.Once, 1f, 0f);
		IL_195:
		this.animController.Queue("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
		this.lastReactSource = source;
	}

	// Token: 0x04004DA1 RID: 19873
	public const float ANIM_SCALE = 0.38f;

	// Token: 0x04004DA2 RID: 19874
	private KBatchedAnimController animController;

	// Token: 0x04004DA3 RID: 19875
	private GameObject spawn;

	// Token: 0x04004DA5 RID: 19877
	private UIMinionOrMannequinReactSource lastReactSource;
}
