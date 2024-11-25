using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x0200046B RID: 1131
[AddComponentMenu("KMonoBehaviour/scripts/ConsumableConsumer")]
public class ConsumableConsumer : KMonoBehaviour
{
	// Token: 0x06001855 RID: 6229 RVA: 0x0008219C File Offset: 0x0008039C
	[OnDeserialized]
	[Obsolete]
	private void OnDeserialized()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 29))
		{
			this.forbiddenTagSet = new HashSet<Tag>(this.forbiddenTags);
			this.forbiddenTags = null;
		}
	}

	// Token: 0x06001856 RID: 6230 RVA: 0x000821D8 File Offset: 0x000803D8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (ConsumerManager.instance != null)
		{
			this.forbiddenTagSet = new HashSet<Tag>(ConsumerManager.instance.DefaultForbiddenTagsList);
			if (this.HasTag(GameTags.Minions.Models.Standard))
			{
				this.dietaryRestrictionTagSet = new HashSet<Tag>(ConsumerManager.instance.StandardDuplicantDietaryRestrictions);
				return;
			}
			if (this.HasTag(GameTags.Minions.Models.Bionic))
			{
				this.dietaryRestrictionTagSet = new HashSet<Tag>(ConsumerManager.instance.BionicDuplicantDietaryRestrictions);
				return;
			}
		}
		else
		{
			this.forbiddenTagSet = new HashSet<Tag>();
			this.dietaryRestrictionTagSet = new HashSet<Tag>();
		}
	}

	// Token: 0x06001857 RID: 6231 RVA: 0x0008226C File Offset: 0x0008046C
	public bool IsPermitted(string consumable_id)
	{
		Tag item = new Tag(consumable_id);
		return !this.forbiddenTagSet.Contains(item) && !this.dietaryRestrictionTagSet.Contains(item);
	}

	// Token: 0x06001858 RID: 6232 RVA: 0x000822A0 File Offset: 0x000804A0
	public bool IsDietRestricted(string consumable_id)
	{
		Tag item = new Tag(consumable_id);
		return this.dietaryRestrictionTagSet.Contains(item);
	}

	// Token: 0x06001859 RID: 6233 RVA: 0x000822C4 File Offset: 0x000804C4
	public void SetPermitted(string consumable_id, bool is_allowed)
	{
		Tag item = new Tag(consumable_id);
		is_allowed = (is_allowed && !this.dietaryRestrictionTagSet.Contains(consumable_id));
		if (is_allowed)
		{
			this.forbiddenTagSet.Remove(item);
		}
		else
		{
			this.forbiddenTagSet.Add(item);
		}
		this.consumableRulesChanged.Signal();
	}

	// Token: 0x04000D82 RID: 3458
	[Obsolete("Deprecated, use forbiddenTagSet")]
	[Serialize]
	[HideInInspector]
	public Tag[] forbiddenTags;

	// Token: 0x04000D83 RID: 3459
	[Serialize]
	public HashSet<Tag> forbiddenTagSet;

	// Token: 0x04000D84 RID: 3460
	[Serialize]
	public HashSet<Tag> dietaryRestrictionTagSet;

	// Token: 0x04000D85 RID: 3461
	public System.Action consumableRulesChanged;
}
