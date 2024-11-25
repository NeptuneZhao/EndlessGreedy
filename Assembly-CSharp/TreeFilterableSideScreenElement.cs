using System;
using UnityEngine;

// Token: 0x02000DB8 RID: 3512
[AddComponentMenu("KMonoBehaviour/scripts/TreeFilterableSideScreenElement")]
public class TreeFilterableSideScreenElement : KMonoBehaviour
{
	// Token: 0x06006F2D RID: 28461 RVA: 0x0029BF91 File Offset: 0x0029A191
	public Tag GetElementTag()
	{
		return this.elementTag;
	}

	// Token: 0x170007CA RID: 1994
	// (get) Token: 0x06006F2E RID: 28462 RVA: 0x0029BF99 File Offset: 0x0029A199
	public bool IsSelected
	{
		get
		{
			return this.checkBox.CurrentState == 1;
		}
	}

	// Token: 0x06006F2F RID: 28463 RVA: 0x0029BFA9 File Offset: 0x0029A1A9
	public MultiToggle GetCheckboxToggle()
	{
		return this.checkBox;
	}

	// Token: 0x170007CB RID: 1995
	// (get) Token: 0x06006F30 RID: 28464 RVA: 0x0029BFB1 File Offset: 0x0029A1B1
	// (set) Token: 0x06006F31 RID: 28465 RVA: 0x0029BFB9 File Offset: 0x0029A1B9
	public TreeFilterableSideScreen Parent
	{
		get
		{
			return this.parent;
		}
		set
		{
			this.parent = value;
		}
	}

	// Token: 0x06006F32 RID: 28466 RVA: 0x0029BFC2 File Offset: 0x0029A1C2
	private void Initialize()
	{
		if (this.initialized)
		{
			return;
		}
		this.checkBoxImg = this.checkBox.gameObject.GetComponentInChildrenOnly<KImage>();
		this.checkBox.onClick = new System.Action(this.CheckBoxClicked);
		this.initialized = true;
	}

	// Token: 0x06006F33 RID: 28467 RVA: 0x0029C001 File Offset: 0x0029A201
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Initialize();
	}

	// Token: 0x06006F34 RID: 28468 RVA: 0x0029C010 File Offset: 0x0029A210
	public Sprite GetStorageObjectSprite(Tag t)
	{
		Sprite result = null;
		GameObject prefab = Assets.GetPrefab(t);
		if (prefab != null)
		{
			KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
			if (component != null)
			{
				result = Def.GetUISpriteFromMultiObjectAnim(component.AnimFiles[0], "ui", false, "");
			}
		}
		return result;
	}

	// Token: 0x06006F35 RID: 28469 RVA: 0x0029C05C File Offset: 0x0029A25C
	public void SetSprite(Tag t)
	{
		global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(t, "ui", false);
		this.elementImg.sprite = uisprite.first;
		this.elementImg.color = uisprite.second;
		this.elementImg.gameObject.SetActive(true);
	}

	// Token: 0x06006F36 RID: 28470 RVA: 0x0029C0B0 File Offset: 0x0029A2B0
	public void SetTag(Tag newTag)
	{
		this.Initialize();
		this.elementTag = newTag;
		this.SetSprite(this.elementTag);
		string text = this.elementTag.ProperName();
		if (this.parent.IsStorage)
		{
			float amountInStorage = this.parent.GetAmountInStorage(this.elementTag);
			text = text + ": " + GameUtil.GetFormattedMass(amountInStorage, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
		}
		this.elementName.text = text;
	}

	// Token: 0x06006F37 RID: 28471 RVA: 0x0029C127 File Offset: 0x0029A327
	private void CheckBoxClicked()
	{
		this.SetCheckBox(!this.parent.IsTagAllowed(this.GetElementTag()));
	}

	// Token: 0x06006F38 RID: 28472 RVA: 0x0029C143 File Offset: 0x0029A343
	public void SetCheckBox(bool checkBoxState)
	{
		this.checkBox.ChangeState(checkBoxState ? 1 : 0);
		this.checkBoxImg.enabled = checkBoxState;
		if (this.OnSelectionChanged != null)
		{
			this.OnSelectionChanged(this.GetElementTag(), checkBoxState);
		}
	}

	// Token: 0x04004BD9 RID: 19417
	[SerializeField]
	private LocText elementName;

	// Token: 0x04004BDA RID: 19418
	[SerializeField]
	private MultiToggle checkBox;

	// Token: 0x04004BDB RID: 19419
	[SerializeField]
	private KImage elementImg;

	// Token: 0x04004BDC RID: 19420
	private KImage checkBoxImg;

	// Token: 0x04004BDD RID: 19421
	private Tag elementTag;

	// Token: 0x04004BDE RID: 19422
	public Action<Tag, bool> OnSelectionChanged;

	// Token: 0x04004BDF RID: 19423
	private TreeFilterableSideScreen parent;

	// Token: 0x04004BE0 RID: 19424
	private bool initialized;
}
