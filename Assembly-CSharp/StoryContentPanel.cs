using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using Klei.CustomSettings;
using ProcGen;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DD0 RID: 3536
public class StoryContentPanel : KMonoBehaviour
{
	// Token: 0x06007060 RID: 28768 RVA: 0x002A83F8 File Offset: 0x002A65F8
	public List<string> GetActiveStories()
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, StoryContentPanel.StoryState> keyValuePair in this.storyStates)
		{
			if (keyValuePair.Value == StoryContentPanel.StoryState.Guaranteed)
			{
				list.Add(keyValuePair.Key);
			}
		}
		return list;
	}

	// Token: 0x06007061 RID: 28769 RVA: 0x002A8464 File Offset: 0x002A6664
	public void Init()
	{
		this.SpawnRows();
		this.RefreshRows();
		this.RefreshDescriptionPanel();
		this.SelectDefault();
		CustomGameSettings.Instance.OnStorySettingChanged += this.OnStorySettingChanged;
	}

	// Token: 0x06007062 RID: 28770 RVA: 0x002A8494 File Offset: 0x002A6694
	public void Cleanup()
	{
		CustomGameSettings.Instance.OnStorySettingChanged -= this.OnStorySettingChanged;
	}

	// Token: 0x06007063 RID: 28771 RVA: 0x002A84AC File Offset: 0x002A66AC
	private void OnStorySettingChanged(SettingConfig config, SettingLevel level)
	{
		this.storyStates[config.id] = ((level.id == "Guaranteed") ? StoryContentPanel.StoryState.Guaranteed : StoryContentPanel.StoryState.Forbidden);
		this.RefreshStoryDisplay(config.id);
	}

	// Token: 0x06007064 RID: 28772 RVA: 0x002A84E4 File Offset: 0x002A66E4
	private void SpawnRows()
	{
		using (List<Story>.Enumerator enumerator = Db.Get().Stories.resources.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Story story = enumerator.Current;
				GameObject gameObject = global::Util.KInstantiateUI(this.storyRowPrefab, this.storyRowContainer, true);
				HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
				component.GetReference<LocText>("Label").SetText(Strings.Get(story.StoryTrait.name));
				MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
				component2.onClick = (System.Action)Delegate.Combine(component2.onClick, new System.Action(delegate()
				{
					this.SelectRow(story.Id);
				}));
				this.storyRows.Add(story.Id, gameObject);
				component.GetReference<Image>("Icon").sprite = Assets.GetSprite(story.StoryTrait.icon);
				MultiToggle reference = component.GetReference<MultiToggle>("checkbox");
				reference.onClick = (System.Action)Delegate.Combine(reference.onClick, new System.Action(delegate()
				{
					this.IncrementStorySetting(story.Id, true);
					this.RefreshStoryDisplay(story.Id);
				}));
				this.storyStates.Add(story.Id, this._defaultStoryState);
			}
		}
		this.RefreshAllStoryStates();
		this.mainScreen.RefreshStoryLabel();
	}

	// Token: 0x06007065 RID: 28773 RVA: 0x002A8660 File Offset: 0x002A6860
	private void SelectRow(string id)
	{
		this.selectedStoryId = id;
		this.RefreshRows();
		this.RefreshDescriptionPanel();
	}

	// Token: 0x06007066 RID: 28774 RVA: 0x002A8678 File Offset: 0x002A6878
	public void SelectDefault()
	{
		foreach (KeyValuePair<string, StoryContentPanel.StoryState> keyValuePair in this.storyStates)
		{
			if (keyValuePair.Value == StoryContentPanel.StoryState.Guaranteed)
			{
				this.SelectRow(keyValuePair.Key);
				return;
			}
		}
		using (Dictionary<string, StoryContentPanel.StoryState>.Enumerator enumerator = this.storyStates.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				KeyValuePair<string, StoryContentPanel.StoryState> keyValuePair2 = enumerator.Current;
				this.SelectRow(keyValuePair2.Key);
			}
		}
	}

	// Token: 0x06007067 RID: 28775 RVA: 0x002A8728 File Offset: 0x002A6928
	private void IncrementStorySetting(string storyId, bool forward = true)
	{
		int num = (int)this.storyStates[storyId];
		num += (forward ? 1 : -1);
		if (num < 0)
		{
			num += 2;
		}
		num %= 2;
		this.SetStoryState(storyId, (StoryContentPanel.StoryState)num);
		this.mainScreen.RefreshRowsAndDescriptions();
	}

	// Token: 0x06007068 RID: 28776 RVA: 0x002A876C File Offset: 0x002A696C
	private void SetStoryState(string storyId, StoryContentPanel.StoryState state)
	{
		this.storyStates[storyId] = state;
		SettingConfig config = CustomGameSettings.Instance.StorySettings[storyId];
		CustomGameSettings.Instance.SetStorySetting(config, this.storyStates[storyId] == StoryContentPanel.StoryState.Guaranteed);
	}

	// Token: 0x06007069 RID: 28777 RVA: 0x002A87B4 File Offset: 0x002A69B4
	public void SelectRandomStories(int min = 5, int max = 5, bool useBias = false)
	{
		int num = UnityEngine.Random.Range(min, max);
		List<Story> list = new List<Story>(Db.Get().Stories.resources);
		List<Story> list2 = new List<Story>();
		list.Shuffle<Story>();
		int num2 = 0;
		while (num2 < num && list.Count - 1 >= num2)
		{
			list2.Add(list[num2]);
			num2++;
		}
		float num3 = 0.7f;
		int num4 = list2.Count((Story x) => x.IsNew());
		if (useBias && num4 == 0 && UnityEngine.Random.value < num3)
		{
			List<Story> list3 = (from x in Db.Get().Stories.resources
			where x.IsNew()
			select x).ToList<Story>();
			list3.Shuffle<Story>();
			if (list3.Count > 0)
			{
				list2.RemoveAt(0);
				list2.Add(list3[0]);
			}
		}
		foreach (Story story in list)
		{
			this.SetStoryState(story.Id, list2.Contains(story) ? StoryContentPanel.StoryState.Guaranteed : StoryContentPanel.StoryState.Forbidden);
		}
		this.RefreshAllStoryStates();
		this.mainScreen.RefreshRowsAndDescriptions();
	}

	// Token: 0x0600706A RID: 28778 RVA: 0x002A8918 File Offset: 0x002A6B18
	private void RefreshAllStoryStates()
	{
		foreach (string id in this.storyRows.Keys)
		{
			this.RefreshStoryDisplay(id);
		}
	}

	// Token: 0x0600706B RID: 28779 RVA: 0x002A8970 File Offset: 0x002A6B70
	private void RefreshStoryDisplay(string id)
	{
		MultiToggle reference = this.storyRows[id].GetComponent<HierarchyReferences>().GetReference<MultiToggle>("checkbox");
		StoryContentPanel.StoryState storyState = this.storyStates[id];
		if (storyState == StoryContentPanel.StoryState.Forbidden)
		{
			reference.ChangeState(0);
			return;
		}
		if (storyState != StoryContentPanel.StoryState.Guaranteed)
		{
			return;
		}
		reference.ChangeState(1);
	}

	// Token: 0x0600706C RID: 28780 RVA: 0x002A89C0 File Offset: 0x002A6BC0
	private void RefreshRows()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.storyRows)
		{
			keyValuePair.Value.GetComponent<MultiToggle>().ChangeState((keyValuePair.Key == this.selectedStoryId) ? 1 : 0);
		}
	}

	// Token: 0x0600706D RID: 28781 RVA: 0x002A8A38 File Offset: 0x002A6C38
	private void RefreshDescriptionPanel()
	{
		if (this.selectedStoryId.IsNullOrWhiteSpace())
		{
			this.selectedStoryTitleLabel.SetText("");
			this.selectedStoryDescriptionLabel.SetText("");
			return;
		}
		WorldTrait storyTrait = Db.Get().Stories.GetStoryTrait(this.selectedStoryId, true);
		this.selectedStoryTitleLabel.SetText(Strings.Get(storyTrait.name));
		this.selectedStoryDescriptionLabel.SetText(Strings.Get(storyTrait.description));
		string s = storyTrait.icon.Replace("_icon", "_image");
		this.selectedStoryImage.sprite = Assets.GetSprite(s);
	}

	// Token: 0x0600706E RID: 28782 RVA: 0x002A8AEC File Offset: 0x002A6CEC
	public string GetTraitsString(bool tooltip = false)
	{
		int num = 0;
		int num2 = 5;
		foreach (KeyValuePair<string, StoryContentPanel.StoryState> keyValuePair in this.storyStates)
		{
			if (keyValuePair.Value == StoryContentPanel.StoryState.Guaranteed)
			{
				num++;
			}
		}
		string text = UI.FRONTEND.COLONYDESTINATIONSCREEN.STORY_TRAITS_HEADER;
		string str;
		if (num != 0)
		{
			if (num != 1)
			{
				str = string.Format(UI.FRONTEND.COLONYDESTINATIONSCREEN.TRAIT_COUNT, num);
			}
			else
			{
				str = UI.FRONTEND.COLONYDESTINATIONSCREEN.SINGLE_TRAIT;
			}
		}
		else
		{
			str = UI.FRONTEND.COLONYDESTINATIONSCREEN.NO_TRAITS;
		}
		text = text + ": " + str;
		if (num > num2)
		{
			text = text + " " + UI.FRONTEND.COLONYDESTINATIONSCREEN.TOO_MANY_TRAITS_WARNING;
		}
		if (tooltip)
		{
			foreach (KeyValuePair<string, StoryContentPanel.StoryState> keyValuePair2 in this.storyStates)
			{
				if (keyValuePair2.Value == StoryContentPanel.StoryState.Guaranteed)
				{
					WorldTrait storyTrait = Db.Get().Stories.Get(keyValuePair2.Key).StoryTrait;
					text = string.Concat(new string[]
					{
						text,
						"\n\n<b>",
						Strings.Get(storyTrait.name).String,
						"</b>\n",
						Strings.Get(storyTrait.description).String
					});
				}
			}
			if (num > num2)
			{
				text = text + "\n\n" + UI.FRONTEND.COLONYDESTINATIONSCREEN.TOO_MANY_TRAITS_WARNING_TOOLTIP;
			}
		}
		return text;
	}

	// Token: 0x04004D38 RID: 19768
	[SerializeField]
	private GameObject storyRowPrefab;

	// Token: 0x04004D39 RID: 19769
	[SerializeField]
	private GameObject storyRowContainer;

	// Token: 0x04004D3A RID: 19770
	private Dictionary<string, GameObject> storyRows = new Dictionary<string, GameObject>();

	// Token: 0x04004D3B RID: 19771
	public const int DEFAULT_RANDOMIZE_STORY_COUNT = 5;

	// Token: 0x04004D3C RID: 19772
	private Dictionary<string, StoryContentPanel.StoryState> storyStates = new Dictionary<string, StoryContentPanel.StoryState>();

	// Token: 0x04004D3D RID: 19773
	private string selectedStoryId = "";

	// Token: 0x04004D3E RID: 19774
	[SerializeField]
	private ColonyDestinationSelectScreen mainScreen;

	// Token: 0x04004D3F RID: 19775
	[Header("Trait Count")]
	[Header("SelectedStory")]
	[SerializeField]
	private Image selectedStoryImage;

	// Token: 0x04004D40 RID: 19776
	[SerializeField]
	private LocText selectedStoryTitleLabel;

	// Token: 0x04004D41 RID: 19777
	[SerializeField]
	private LocText selectedStoryDescriptionLabel;

	// Token: 0x04004D42 RID: 19778
	[SerializeField]
	private Sprite spriteForbidden;

	// Token: 0x04004D43 RID: 19779
	[SerializeField]
	private Sprite spritePossible;

	// Token: 0x04004D44 RID: 19780
	[SerializeField]
	private Sprite spriteGuaranteed;

	// Token: 0x04004D45 RID: 19781
	private StoryContentPanel.StoryState _defaultStoryState;

	// Token: 0x04004D46 RID: 19782
	private List<string> storyTraitSettings = new List<string>
	{
		"None",
		"Few",
		"Lots"
	};

	// Token: 0x02001EE3 RID: 7907
	private enum StoryState
	{
		// Token: 0x04008BCE RID: 35790
		Forbidden,
		// Token: 0x04008BCF RID: 35791
		Guaranteed,
		// Token: 0x04008BD0 RID: 35792
		LENGTH
	}
}
