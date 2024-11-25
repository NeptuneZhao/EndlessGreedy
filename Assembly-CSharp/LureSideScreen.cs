using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D7C RID: 3452
public class LureSideScreen : SideScreenContent
{
	// Token: 0x06006C9B RID: 27803 RVA: 0x0028DA3F File Offset: 0x0028BC3F
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<CreatureLure>() != null;
	}

	// Token: 0x06006C9C RID: 27804 RVA: 0x0028DA50 File Offset: 0x0028BC50
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.target_lure = target.GetComponent<CreatureLure>();
		using (List<Tag>.Enumerator enumerator = this.target_lure.baitTypes.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Tag bait = enumerator.Current;
				Tag bait3 = bait;
				if (!this.toggles_by_tag.ContainsKey(bait))
				{
					GameObject gameObject = Util.KInstantiateUI(this.prefab_toggle, this.toggle_container, true);
					Image reference = gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("FGImage");
					gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").text = ElementLoader.GetElement(bait).name;
					reference.sprite = Def.GetUISpriteFromMultiObjectAnim(ElementLoader.GetElement(bait).substance.anim, "ui", false, "");
					MultiToggle component = gameObject.GetComponent<MultiToggle>();
					this.toggles_by_tag.Add(bait3, component);
				}
				this.toggles_by_tag[bait].onClick = delegate()
				{
					Tag bait2 = bait;
					this.SelectToggle(bait2);
				};
			}
		}
		this.RefreshToggles();
	}

	// Token: 0x06006C9D RID: 27805 RVA: 0x0028DB9C File Offset: 0x0028BD9C
	public void SelectToggle(Tag tag)
	{
		if (this.target_lure.activeBaitSetting != tag)
		{
			this.target_lure.ChangeBaitSetting(tag);
		}
		else
		{
			this.target_lure.ChangeBaitSetting(Tag.Invalid);
		}
		this.RefreshToggles();
	}

	// Token: 0x06006C9E RID: 27806 RVA: 0x0028DBD8 File Offset: 0x0028BDD8
	private void RefreshToggles()
	{
		foreach (KeyValuePair<Tag, MultiToggle> keyValuePair in this.toggles_by_tag)
		{
			if (this.target_lure.activeBaitSetting == keyValuePair.Key)
			{
				keyValuePair.Value.ChangeState(2);
			}
			else
			{
				keyValuePair.Value.ChangeState(1);
			}
			keyValuePair.Value.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.UISIDESCREENS.LURE.ATTRACTS, ElementLoader.GetElement(keyValuePair.Key).name, this.baitAttractionStrings[keyValuePair.Key]));
		}
	}

	// Token: 0x04004A14 RID: 18964
	protected CreatureLure target_lure;

	// Token: 0x04004A15 RID: 18965
	public GameObject prefab_toggle;

	// Token: 0x04004A16 RID: 18966
	public GameObject toggle_container;

	// Token: 0x04004A17 RID: 18967
	public Dictionary<Tag, MultiToggle> toggles_by_tag = new Dictionary<Tag, MultiToggle>();

	// Token: 0x04004A18 RID: 18968
	private Dictionary<Tag, string> baitAttractionStrings = new Dictionary<Tag, string>
	{
		{
			GameTags.SlimeMold,
			CREATURES.SPECIES.PUFT.NAME
		},
		{
			GameTags.Phosphorite,
			CREATURES.SPECIES.LIGHTBUG.NAME
		}
	};
}
