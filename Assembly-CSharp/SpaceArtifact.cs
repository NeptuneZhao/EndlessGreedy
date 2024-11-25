using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000B11 RID: 2833
[AddComponentMenu("KMonoBehaviour/scripts/SpaceArtifact")]
public class SpaceArtifact : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x0600545D RID: 21597 RVA: 0x001E2ADC File Offset: 0x001E0CDC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.loadCharmed && DlcManager.IsExpansion1Active())
		{
			base.gameObject.AddTag(GameTags.CharmedArtifact);
			this.SetEntombedDecor();
		}
		else
		{
			this.loadCharmed = false;
			this.SetAnalyzedDecor();
		}
		this.UpdateStatusItem();
		Components.SpaceArtifacts.Add(this);
		this.UpdateAnim();
	}

	// Token: 0x0600545E RID: 21598 RVA: 0x001E2B3A File Offset: 0x001E0D3A
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.SpaceArtifacts.Remove(this);
	}

	// Token: 0x0600545F RID: 21599 RVA: 0x001E2B4D File Offset: 0x001E0D4D
	public void RemoveCharm()
	{
		base.gameObject.RemoveTag(GameTags.CharmedArtifact);
		this.UpdateStatusItem();
		this.loadCharmed = false;
		this.UpdateAnim();
		this.SetAnalyzedDecor();
	}

	// Token: 0x06005460 RID: 21600 RVA: 0x001E2B78 File Offset: 0x001E0D78
	private void SetEntombedDecor()
	{
		base.GetComponent<DecorProvider>().SetValues(DECOR.BONUS.TIER0);
	}

	// Token: 0x06005461 RID: 21601 RVA: 0x001E2B8A File Offset: 0x001E0D8A
	private void SetAnalyzedDecor()
	{
		base.GetComponent<DecorProvider>().SetValues(this.artifactTier.decorValues);
	}

	// Token: 0x06005462 RID: 21602 RVA: 0x001E2BA4 File Offset: 0x001E0DA4
	public void UpdateStatusItem()
	{
		if (base.gameObject.HasTag(GameTags.CharmedArtifact))
		{
			base.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.ArtifactEntombed, null);
			return;
		}
		base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.ArtifactEntombed, false);
	}

	// Token: 0x06005463 RID: 21603 RVA: 0x001E2C06 File Offset: 0x001E0E06
	public void SetArtifactTier(ArtifactTier tier)
	{
		this.artifactTier = tier;
	}

	// Token: 0x06005464 RID: 21604 RVA: 0x001E2C0F File Offset: 0x001E0E0F
	public ArtifactTier GetArtifactTier()
	{
		return this.artifactTier;
	}

	// Token: 0x06005465 RID: 21605 RVA: 0x001E2C17 File Offset: 0x001E0E17
	public void SetUIAnim(string anim)
	{
		this.ui_anim = anim;
	}

	// Token: 0x06005466 RID: 21606 RVA: 0x001E2C20 File Offset: 0x001E0E20
	public string GetUIAnim()
	{
		return this.ui_anim;
	}

	// Token: 0x06005467 RID: 21607 RVA: 0x001E2C28 File Offset: 0x001E0E28
	public List<Descriptor> GetEffectDescriptions()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (base.gameObject.HasTag(GameTags.CharmedArtifact))
		{
			Descriptor item = new Descriptor(STRINGS.BUILDINGS.PREFABS.ARTIFACTANALYSISSTATION.PAYLOAD_DROP_RATE.Replace("{chance}", GameUtil.GetFormattedPercent(this.artifactTier.payloadDropChance * 100f, GameUtil.TimeSlice.None)), STRINGS.BUILDINGS.PREFABS.ARTIFACTANALYSISSTATION.PAYLOAD_DROP_RATE_TOOLTIP.Replace("{chance}", GameUtil.GetFormattedPercent(this.artifactTier.payloadDropChance * 100f, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect, false);
			list.Add(item);
		}
		Descriptor item2 = new Descriptor(string.Format("This is an artifact from space", Array.Empty<object>()), string.Format("This is the tooltip string", Array.Empty<object>()), Descriptor.DescriptorType.Information, false);
		list.Add(item2);
		return list;
	}

	// Token: 0x06005468 RID: 21608 RVA: 0x001E2CD8 File Offset: 0x001E0ED8
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return this.GetEffectDescriptions();
	}

	// Token: 0x06005469 RID: 21609 RVA: 0x001E2CE0 File Offset: 0x001E0EE0
	private void UpdateAnim()
	{
		string s;
		if (base.gameObject.HasTag(GameTags.CharmedArtifact))
		{
			s = "entombed_" + this.uniqueAnimNameFragment.Replace("idle_", "");
		}
		else
		{
			s = this.uniqueAnimNameFragment;
		}
		base.GetComponent<KBatchedAnimController>().Play(s, KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x0600546A RID: 21610 RVA: 0x001E2D44 File Offset: 0x001E0F44
	[OnDeserialized]
	public void OnDeserialize()
	{
		Pickupable component = base.GetComponent<Pickupable>();
		if (component != null)
		{
			component.deleteOffGrid = false;
		}
	}

	// Token: 0x04003752 RID: 14162
	public const string ID = "SpaceArtifact";

	// Token: 0x04003753 RID: 14163
	private const string charmedPrefix = "entombed_";

	// Token: 0x04003754 RID: 14164
	private const string idlePrefix = "idle_";

	// Token: 0x04003755 RID: 14165
	[SerializeField]
	private string ui_anim;

	// Token: 0x04003756 RID: 14166
	[Serialize]
	private bool loadCharmed = true;

	// Token: 0x04003757 RID: 14167
	public ArtifactTier artifactTier;

	// Token: 0x04003758 RID: 14168
	public ArtifactType artifactType;

	// Token: 0x04003759 RID: 14169
	public string uniqueAnimNameFragment;
}
