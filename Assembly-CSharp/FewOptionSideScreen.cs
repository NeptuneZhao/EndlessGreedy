using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D69 RID: 3433
public class FewOptionSideScreen : SideScreenContent
{
	// Token: 0x06006C0F RID: 27663 RVA: 0x0028A6C7 File Offset: 0x002888C7
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.RefreshOptions();
		}
	}

	// Token: 0x06006C10 RID: 27664 RVA: 0x0028A6DC File Offset: 0x002888DC
	private void RefreshOptions()
	{
		foreach (KeyValuePair<Tag, GameObject> keyValuePair in this.rows)
		{
			keyValuePair.Value.GetComponent<MultiToggle>().ChangeState((keyValuePair.Key == this.targetFewOptions.GetSelectedOption()) ? 1 : 0);
		}
	}

	// Token: 0x06006C11 RID: 27665 RVA: 0x0028A758 File Offset: 0x00288958
	private void ClearRows()
	{
		for (int i = this.rowContainer.childCount - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.rowContainer.GetChild(i));
		}
		this.rows.Clear();
	}

	// Token: 0x06006C12 RID: 27666 RVA: 0x0028A79C File Offset: 0x0028899C
	private void SpawnRows()
	{
		FewOptionSideScreen.IFewOptionSideScreen.Option[] options = this.targetFewOptions.GetOptions();
		for (int i = 0; i < options.Length; i++)
		{
			FewOptionSideScreen.IFewOptionSideScreen.Option option = options[i];
			GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.rowContainer.gameObject, true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("label").SetText(option.labelText);
			component.GetReference<Image>("icon").sprite = option.iconSpriteColorTuple.first;
			component.GetReference<Image>("icon").color = option.iconSpriteColorTuple.second;
			gameObject.GetComponent<ToolTip>().toolTip = option.tooltipText;
			gameObject.GetComponent<MultiToggle>().onClick = delegate()
			{
				this.targetFewOptions.OnOptionSelected(option);
				this.RefreshOptions();
			};
			this.rows.Add(option.tag, gameObject);
		}
		this.RefreshOptions();
	}

	// Token: 0x06006C13 RID: 27667 RVA: 0x0028A8A5 File Offset: 0x00288AA5
	public override void SetTarget(GameObject target)
	{
		this.ClearRows();
		this.targetFewOptions = target.GetComponent<FewOptionSideScreen.IFewOptionSideScreen>();
		this.SpawnRows();
	}

	// Token: 0x06006C14 RID: 27668 RVA: 0x0028A8BF File Offset: 0x00288ABF
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<FewOptionSideScreen.IFewOptionSideScreen>() != null;
	}

	// Token: 0x040049AC RID: 18860
	public GameObject rowPrefab;

	// Token: 0x040049AD RID: 18861
	public RectTransform rowContainer;

	// Token: 0x040049AE RID: 18862
	public Dictionary<Tag, GameObject> rows = new Dictionary<Tag, GameObject>();

	// Token: 0x040049AF RID: 18863
	private FewOptionSideScreen.IFewOptionSideScreen targetFewOptions;

	// Token: 0x02001E96 RID: 7830
	public interface IFewOptionSideScreen
	{
		// Token: 0x0600ABDD RID: 43997
		FewOptionSideScreen.IFewOptionSideScreen.Option[] GetOptions();

		// Token: 0x0600ABDE RID: 43998
		void OnOptionSelected(FewOptionSideScreen.IFewOptionSideScreen.Option option);

		// Token: 0x0600ABDF RID: 43999
		Tag GetSelectedOption();

		// Token: 0x0200265F RID: 9823
		public struct Option
		{
			// Token: 0x0600C236 RID: 49718 RVA: 0x003E00F4 File Offset: 0x003DE2F4
			public Option(Tag tag, string labelText, global::Tuple<Sprite, Color> iconSpriteColorTuple, string tooltipText = "")
			{
				this.tag = tag;
				this.labelText = labelText;
				this.iconSpriteColorTuple = iconSpriteColorTuple;
				this.tooltipText = tooltipText;
			}

			// Token: 0x0400AA72 RID: 43634
			public Tag tag;

			// Token: 0x0400AA73 RID: 43635
			public string labelText;

			// Token: 0x0400AA74 RID: 43636
			public string tooltipText;

			// Token: 0x0400AA75 RID: 43637
			public global::Tuple<Sprite, Color> iconSpriteColorTuple;
		}
	}
}
