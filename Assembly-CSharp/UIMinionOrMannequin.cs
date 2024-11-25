using System;
using System.Collections.Generic;
using Database;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DE4 RID: 3556
public class UIMinionOrMannequin : KMonoBehaviour
{
	// Token: 0x170007DA RID: 2010
	// (get) Token: 0x060070ED RID: 28909 RVA: 0x002AC279 File Offset: 0x002AA479
	// (set) Token: 0x060070EE RID: 28910 RVA: 0x002AC281 File Offset: 0x002AA481
	public UIMinionOrMannequin.ITarget current { get; private set; }

	// Token: 0x060070EF RID: 28911 RVA: 0x002AC28A File Offset: 0x002AA48A
	protected override void OnSpawn()
	{
		this.TrySpawn();
	}

	// Token: 0x060070F0 RID: 28912 RVA: 0x002AC294 File Offset: 0x002AA494
	public bool TrySpawn()
	{
		bool flag = false;
		if (this.mannequin.IsNullOrDestroyed())
		{
			GameObject gameObject = new GameObject("UIMannequin");
			gameObject.AddOrGet<RectTransform>().Fill(Padding.All(10f));
			gameObject.transform.SetParent(base.transform, false);
			AspectRatioFitter aspectRatioFitter = gameObject.AddOrGet<AspectRatioFitter>();
			aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
			aspectRatioFitter.aspectRatio = 1f;
			this.mannequin = gameObject.AddOrGet<UIMannequin>();
			this.mannequin.TrySpawn();
			gameObject.SetActive(false);
			flag = true;
		}
		if (this.minion.IsNullOrDestroyed())
		{
			GameObject gameObject2 = new GameObject("UIMinion");
			gameObject2.AddOrGet<RectTransform>().Fill(Padding.All(10f));
			gameObject2.transform.SetParent(base.transform, false);
			AspectRatioFitter aspectRatioFitter2 = gameObject2.AddOrGet<AspectRatioFitter>();
			aspectRatioFitter2.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
			aspectRatioFitter2.aspectRatio = 1f;
			this.minion = gameObject2.AddOrGet<UIMinion>();
			this.minion.TrySpawn();
			gameObject2.SetActive(false);
			flag = true;
		}
		if (flag)
		{
			this.SetAsMannequin();
		}
		return flag;
	}

	// Token: 0x060070F1 RID: 28913 RVA: 0x002AC39C File Offset: 0x002AA59C
	public UIMinionOrMannequin.ITarget SetFrom(Option<Personality> personality)
	{
		if (personality.IsSome())
		{
			return this.SetAsMinion(personality.Unwrap());
		}
		return this.SetAsMannequin();
	}

	// Token: 0x060070F2 RID: 28914 RVA: 0x002AC3BC File Offset: 0x002AA5BC
	public UIMinion SetAsMinion(Personality personality)
	{
		this.mannequin.gameObject.SetActive(false);
		this.minion.gameObject.SetActive(true);
		this.minion.SetMinion(personality);
		this.current = this.minion;
		return this.minion;
	}

	// Token: 0x060070F3 RID: 28915 RVA: 0x002AC409 File Offset: 0x002AA609
	public UIMannequin SetAsMannequin()
	{
		this.minion.gameObject.SetActive(false);
		this.mannequin.gameObject.SetActive(true);
		this.current = this.mannequin;
		return this.mannequin;
	}

	// Token: 0x060070F4 RID: 28916 RVA: 0x002AC440 File Offset: 0x002AA640
	public MinionVoice GetMinionVoice()
	{
		return MinionVoice.ByObject(this.current.SpawnedAvatar).UnwrapOr(MinionVoice.Random(), null);
	}

	// Token: 0x04004DAF RID: 19887
	public UIMinion minion;

	// Token: 0x04004DB0 RID: 19888
	public UIMannequin mannequin;

	// Token: 0x02001EED RID: 7917
	public interface ITarget
	{
		// Token: 0x17000BE5 RID: 3045
		// (get) Token: 0x0600ACFB RID: 44283
		GameObject SpawnedAvatar { get; }

		// Token: 0x17000BE6 RID: 3046
		// (get) Token: 0x0600ACFC RID: 44284
		Option<Personality> Personality { get; }

		// Token: 0x0600ACFD RID: 44285
		void SetOutfit(ClothingOutfitUtility.OutfitType outfitType, IEnumerable<ClothingItemResource> clothingItems);

		// Token: 0x0600ACFE RID: 44286
		void React(UIMinionOrMannequinReactSource source);
	}
}
