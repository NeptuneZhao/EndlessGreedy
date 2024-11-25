using System;
using System.Linq;
using UnityEngine;

// Token: 0x02000CE1 RID: 3297
public readonly struct MinionBrowserScreenConfig
{
	// Token: 0x060065E9 RID: 26089 RVA: 0x0025FC26 File Offset: 0x0025DE26
	public MinionBrowserScreenConfig(MinionBrowserScreen.GridItem[] items, Option<MinionBrowserScreen.GridItem> defaultSelectedItem)
	{
		this.items = items;
		this.defaultSelectedItem = defaultSelectedItem;
		this.isValid = true;
	}

	// Token: 0x060065EA RID: 26090 RVA: 0x0025FC40 File Offset: 0x0025DE40
	public static MinionBrowserScreenConfig Personalities(Option<Personality> defaultSelectedPersonality = default(Option<Personality>))
	{
		MinionBrowserScreen.GridItem.PersonalityTarget[] items = (from personality in Db.Get().Personalities.GetAll(true, false)
		select MinionBrowserScreen.GridItem.Of(personality)).ToArray<MinionBrowserScreen.GridItem.PersonalityTarget>();
		Option<MinionBrowserScreen.GridItem> option = defaultSelectedPersonality.AndThen<MinionBrowserScreen.GridItem>((Personality personality) => items.FirstOrDefault((MinionBrowserScreen.GridItem.PersonalityTarget item) => item.personality == personality));
		if (option.IsNone() && items.Length != 0)
		{
			option = items[0];
		}
		MinionBrowserScreen.GridItem[] array = items;
		return new MinionBrowserScreenConfig(array, option);
	}

	// Token: 0x060065EB RID: 26091 RVA: 0x0025FCD8 File Offset: 0x0025DED8
	public static MinionBrowserScreenConfig MinionInstances(Option<GameObject> defaultSelectedMinionInstance = default(Option<GameObject>))
	{
		MinionBrowserScreen.GridItem.MinionInstanceTarget[] items = (from minionIdentity in Components.MinionIdentities.Items
		select MinionBrowserScreen.GridItem.Of(minionIdentity.gameObject)).ToArray<MinionBrowserScreen.GridItem.MinionInstanceTarget>();
		Option<MinionBrowserScreen.GridItem> option = defaultSelectedMinionInstance.AndThen<MinionBrowserScreen.GridItem>((GameObject minionInstance) => items.FirstOrDefault((MinionBrowserScreen.GridItem.MinionInstanceTarget item) => item.minionInstance == minionInstance));
		if (option.IsNone() && items.Length != 0)
		{
			option = items[0];
		}
		MinionBrowserScreen.GridItem[] array = items;
		return new MinionBrowserScreenConfig(array, option);
	}

	// Token: 0x060065EC RID: 26092 RVA: 0x0025FD66 File Offset: 0x0025DF66
	public void ApplyAndOpenScreen(System.Action onClose = null)
	{
		LockerNavigator.Instance.duplicantCatalogueScreen.GetComponent<MinionBrowserScreen>().Configure(this);
		LockerNavigator.Instance.PushScreen(LockerNavigator.Instance.duplicantCatalogueScreen, onClose);
	}

	// Token: 0x040044C6 RID: 17606
	public readonly MinionBrowserScreen.GridItem[] items;

	// Token: 0x040044C7 RID: 17607
	public readonly Option<MinionBrowserScreen.GridItem> defaultSelectedItem;

	// Token: 0x040044C8 RID: 17608
	public readonly bool isValid;
}
