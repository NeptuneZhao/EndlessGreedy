using System;
using System.Collections.Generic;
using Database;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D45 RID: 3397
public class ArtableSelectionSideScreen : SideScreenContent
{
	// Token: 0x06006AD5 RID: 27349 RVA: 0x00283918 File Offset: 0x00281B18
	public override bool IsValidForTarget(GameObject target)
	{
		Artable component = target.GetComponent<Artable>();
		return !(component == null) && !(component.CurrentStage == "Default");
	}

	// Token: 0x06006AD6 RID: 27350 RVA: 0x0028394C File Offset: 0x00281B4C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.applyButton.onClick += delegate()
		{
			this.target.SetUserChosenTargetState(this.selectedStage);
			SelectTool.Instance.Select(null, true);
		};
		this.clearButton.onClick += delegate()
		{
			this.selectedStage = "";
			this.target.SetDefault();
			SelectTool.Instance.Select(null, true);
		};
	}

	// Token: 0x06006AD7 RID: 27351 RVA: 0x00283984 File Offset: 0x00281B84
	public override void SetTarget(GameObject target)
	{
		if (this.workCompleteSub != -1)
		{
			target.Unsubscribe(this.workCompleteSub);
			this.workCompleteSub = -1;
		}
		base.SetTarget(target);
		this.target = target.GetComponent<Artable>();
		this.workCompleteSub = target.Subscribe(-2011693419, new Action<object>(this.OnRefreshTarget));
		this.OnRefreshTarget(null);
	}

	// Token: 0x06006AD8 RID: 27352 RVA: 0x002839E4 File Offset: 0x00281BE4
	public override void ClearTarget()
	{
		this.target.Unsubscribe(-2011693419);
		this.workCompleteSub = -1;
		base.ClearTarget();
	}

	// Token: 0x06006AD9 RID: 27353 RVA: 0x00283A03 File Offset: 0x00281C03
	private void OnRefreshTarget(object data = null)
	{
		if (this.target == null)
		{
			return;
		}
		this.GenerateStateButtons();
		this.selectedStage = this.target.CurrentStage;
		this.RefreshButtons();
	}

	// Token: 0x06006ADA RID: 27354 RVA: 0x00283A34 File Offset: 0x00281C34
	public void GenerateStateButtons()
	{
		foreach (KeyValuePair<string, MultiToggle> keyValuePair in this.buttons)
		{
			Util.KDestroyGameObject(keyValuePair.Value.gameObject);
		}
		this.buttons.Clear();
		foreach (ArtableStage artableStage in Db.GetArtableStages().GetPrefabStages(this.target.GetComponent<KPrefabID>().PrefabID()))
		{
			if (!(artableStage.id == "Default"))
			{
				GameObject gameObject = Util.KInstantiateUI(this.stateButtonPrefab, this.buttonContainer.gameObject, true);
				Sprite sprite = artableStage.GetPermitPresentationInfo().sprite;
				MultiToggle component = gameObject.GetComponent<MultiToggle>();
				component.GetComponent<ToolTip>().SetSimpleTooltip(artableStage.Name);
				component.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = sprite;
				this.buttons.Add(artableStage.id, component);
			}
		}
	}

	// Token: 0x06006ADB RID: 27355 RVA: 0x00283B6C File Offset: 0x00281D6C
	private void RefreshButtons()
	{
		List<ArtableStage> prefabStages = Db.GetArtableStages().GetPrefabStages(this.target.GetComponent<KPrefabID>().PrefabID());
		ArtableStage artableStage = prefabStages.Find((ArtableStage match) => match.id == this.target.CurrentStage);
		int num = 0;
		using (Dictionary<string, MultiToggle>.Enumerator enumerator = this.buttons.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ArtableSelectionSideScreen.<>c__DisplayClass16_0 CS$<>8__locals1 = new ArtableSelectionSideScreen.<>c__DisplayClass16_0();
				CS$<>8__locals1.<>4__this = this;
				CS$<>8__locals1.kvp = enumerator.Current;
				ArtableStage stage = prefabStages.Find((ArtableStage match) => match.id == CS$<>8__locals1.kvp.Key);
				if (stage != null && artableStage != null && stage.statusItem.StatusType != artableStage.statusItem.StatusType)
				{
					CS$<>8__locals1.kvp.Value.gameObject.SetActive(false);
				}
				else if (!stage.IsUnlocked())
				{
					CS$<>8__locals1.kvp.Value.gameObject.SetActive(false);
				}
				else
				{
					num++;
					CS$<>8__locals1.kvp.Value.gameObject.SetActive(true);
					CS$<>8__locals1.kvp.Value.ChangeState((this.selectedStage == CS$<>8__locals1.kvp.Key) ? 1 : 0);
					MultiToggle value = CS$<>8__locals1.kvp.Value;
					value.onClick = (System.Action)Delegate.Combine(value.onClick, new System.Action(delegate()
					{
						CS$<>8__locals1.<>4__this.selectedStage = stage.id;
						CS$<>8__locals1.<>4__this.RefreshButtons();
					}));
				}
			}
		}
		this.scrollTransoform.GetComponent<LayoutElement>().preferredHeight = (float)((num > 3) ? 200 : 100);
	}

	// Token: 0x040048D1 RID: 18641
	private Artable target;

	// Token: 0x040048D2 RID: 18642
	public KButton applyButton;

	// Token: 0x040048D3 RID: 18643
	public KButton clearButton;

	// Token: 0x040048D4 RID: 18644
	public GameObject stateButtonPrefab;

	// Token: 0x040048D5 RID: 18645
	private Dictionary<string, MultiToggle> buttons = new Dictionary<string, MultiToggle>();

	// Token: 0x040048D6 RID: 18646
	[SerializeField]
	private RectTransform scrollTransoform;

	// Token: 0x040048D7 RID: 18647
	private string selectedStage = "";

	// Token: 0x040048D8 RID: 18648
	private const int INVALID_SUBSCRIPTION = -1;

	// Token: 0x040048D9 RID: 18649
	private int workCompleteSub = -1;

	// Token: 0x040048DA RID: 18650
	[SerializeField]
	private RectTransform buttonContainer;
}
