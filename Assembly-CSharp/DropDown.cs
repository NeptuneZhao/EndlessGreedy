using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C3E RID: 3134
[AddComponentMenu("KMonoBehaviour/scripts/DropDown")]
public class DropDown : KMonoBehaviour
{
	// Token: 0x1700072D RID: 1837
	// (get) Token: 0x06006048 RID: 24648 RVA: 0x0023CD5F File Offset: 0x0023AF5F
	// (set) Token: 0x06006049 RID: 24649 RVA: 0x0023CD67 File Offset: 0x0023AF67
	public bool open { get; private set; }

	// Token: 0x1700072E RID: 1838
	// (get) Token: 0x0600604A RID: 24650 RVA: 0x0023CD70 File Offset: 0x0023AF70
	public List<IListableOption> Entries
	{
		get
		{
			return this.entries;
		}
	}

	// Token: 0x0600604B RID: 24651 RVA: 0x0023CD78 File Offset: 0x0023AF78
	public void Initialize(IEnumerable<IListableOption> contentKeys, Action<IListableOption, object> onEntrySelectedAction, Func<IListableOption, IListableOption, object, int> sortFunction = null, Action<DropDownEntry, object> refreshAction = null, bool displaySelectedValueWhenClosed = true, object targetData = null)
	{
		this.targetData = targetData;
		this.sortFunction = sortFunction;
		this.onEntrySelectedAction = onEntrySelectedAction;
		this.displaySelectedValueWhenClosed = displaySelectedValueWhenClosed;
		this.rowRefreshAction = refreshAction;
		this.ChangeContent(contentKeys);
		this.openButton.ClearOnClick();
		this.openButton.onClick += delegate()
		{
			this.OnClick();
		};
		this.canvasScaler = GameScreenManager.Instance.ssOverlayCanvas.GetComponent<KCanvasScaler>();
	}

	// Token: 0x0600604C RID: 24652 RVA: 0x0023CDE9 File Offset: 0x0023AFE9
	public void CustomizeEmptyRow(string txt, Sprite icon)
	{
		this.emptyRowLabel = txt;
		this.emptyRowSprite = icon;
	}

	// Token: 0x0600604D RID: 24653 RVA: 0x0023CDF9 File Offset: 0x0023AFF9
	public void OnClick()
	{
		if (!this.open)
		{
			this.Open();
			return;
		}
		this.Close();
	}

	// Token: 0x0600604E RID: 24654 RVA: 0x0023CE10 File Offset: 0x0023B010
	public void ChangeContent(IEnumerable<IListableOption> contentKeys)
	{
		this.entries.Clear();
		foreach (IListableOption item in contentKeys)
		{
			this.entries.Add(item);
		}
		this.built = false;
	}

	// Token: 0x0600604F RID: 24655 RVA: 0x0023CE70 File Offset: 0x0023B070
	private void Update()
	{
		if (!this.open)
		{
			return;
		}
		if (!Input.GetMouseButtonDown(0) && Input.GetAxis("Mouse ScrollWheel") == 0f && !KInputManager.steamInputInterpreter.GetSteamInputActionIsDown(global::Action.MouseLeft))
		{
			return;
		}
		float canvasScale = this.canvasScaler.GetCanvasScale();
		if (this.scrollRect.rectTransform().GetPosition().x + this.scrollRect.rectTransform().sizeDelta.x * canvasScale < KInputManager.GetMousePos().x || this.scrollRect.rectTransform().GetPosition().x > KInputManager.GetMousePos().x || this.scrollRect.rectTransform().GetPosition().y - this.scrollRect.rectTransform().sizeDelta.y * canvasScale > KInputManager.GetMousePos().y || this.scrollRect.rectTransform().GetPosition().y < KInputManager.GetMousePos().y)
		{
			this.Close();
		}
	}

	// Token: 0x06006050 RID: 24656 RVA: 0x0023CF74 File Offset: 0x0023B174
	private void Build(List<IListableOption> contentKeys)
	{
		this.built = true;
		for (int i = this.contentContainer.childCount - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.contentContainer.GetChild(i));
		}
		this.rowLookup.Clear();
		if (this.addEmptyRow)
		{
			this.emptyRow = Util.KInstantiateUI(this.rowEntryPrefab, this.contentContainer.gameObject, true);
			this.emptyRow.GetComponent<KButton>().onClick += delegate()
			{
				this.onEntrySelectedAction(null, this.targetData);
				if (this.displaySelectedValueWhenClosed)
				{
					this.selectedLabel.text = (this.emptyRowLabel ?? UI.DROPDOWN.NONE);
				}
				this.Close();
			};
			string text = this.emptyRowLabel ?? UI.DROPDOWN.NONE;
			this.emptyRow.GetComponent<DropDownEntry>().label.text = text;
			if (this.emptyRowSprite != null)
			{
				this.emptyRow.GetComponent<DropDownEntry>().image.sprite = this.emptyRowSprite;
			}
		}
		for (int j = 0; j < contentKeys.Count; j++)
		{
			GameObject gameObject = Util.KInstantiateUI(this.rowEntryPrefab, this.contentContainer.gameObject, true);
			IListableOption id = contentKeys[j];
			gameObject.GetComponent<DropDownEntry>().entryData = id;
			gameObject.GetComponent<KButton>().onClick += delegate()
			{
				this.onEntrySelectedAction(id, this.targetData);
				if (this.displaySelectedValueWhenClosed)
				{
					this.selectedLabel.text = id.GetProperName();
				}
				this.Close();
			};
			this.rowLookup.Add(id, gameObject);
		}
		this.RefreshEntries();
		this.Close();
		this.scrollRect.gameObject.transform.SetParent(this.targetDropDownContainer.transform);
		this.scrollRect.gameObject.SetActive(false);
	}

	// Token: 0x06006051 RID: 24657 RVA: 0x0023D114 File Offset: 0x0023B314
	private void RefreshEntries()
	{
		foreach (KeyValuePair<IListableOption, GameObject> keyValuePair in this.rowLookup)
		{
			DropDownEntry component = keyValuePair.Value.GetComponent<DropDownEntry>();
			component.label.text = keyValuePair.Key.GetProperName();
			if (component.portrait != null && keyValuePair.Key is IAssignableIdentity)
			{
				component.portrait.SetIdentityObject(keyValuePair.Key as IAssignableIdentity, true);
			}
		}
		if (this.sortFunction != null)
		{
			this.entries.Sort((IListableOption a, IListableOption b) => this.sortFunction(a, b, this.targetData));
			for (int i = 0; i < this.entries.Count; i++)
			{
				this.rowLookup[this.entries[i]].transform.SetAsFirstSibling();
			}
			if (this.emptyRow != null)
			{
				this.emptyRow.transform.SetAsFirstSibling();
			}
		}
		foreach (KeyValuePair<IListableOption, GameObject> keyValuePair2 in this.rowLookup)
		{
			DropDownEntry component2 = keyValuePair2.Value.GetComponent<DropDownEntry>();
			this.rowRefreshAction(component2, this.targetData);
		}
		if (this.emptyRow != null)
		{
			this.rowRefreshAction(this.emptyRow.GetComponent<DropDownEntry>(), this.targetData);
		}
	}

	// Token: 0x06006052 RID: 24658 RVA: 0x0023D2B8 File Offset: 0x0023B4B8
	protected override void OnCleanUp()
	{
		Util.KDestroyGameObject(this.scrollRect);
		base.OnCleanUp();
	}

	// Token: 0x06006053 RID: 24659 RVA: 0x0023D2CC File Offset: 0x0023B4CC
	public void Open()
	{
		if (this.open)
		{
			return;
		}
		if (!this.built)
		{
			this.Build(this.entries);
		}
		else
		{
			this.RefreshEntries();
		}
		this.open = true;
		this.scrollRect.gameObject.SetActive(true);
		this.scrollRect.rectTransform().localScale = Vector3.one;
		foreach (KeyValuePair<IListableOption, GameObject> keyValuePair in this.rowLookup)
		{
			keyValuePair.Value.SetActive(true);
		}
		float num = Mathf.Max(32f, this.rowEntryPrefab.GetComponent<LayoutElement>().preferredHeight);
		this.scrollRect.rectTransform().sizeDelta = new Vector2(this.scrollRect.rectTransform().sizeDelta.x, num * (float)Mathf.Min(this.contentContainer.childCount, 8));
		Vector3 vector = this.dropdownAlignmentTarget.TransformPoint(this.dropdownAlignmentTarget.rect.x, this.dropdownAlignmentTarget.rect.y, 0f);
		Vector2 v = new Vector2(Mathf.Min(0f, (float)Screen.width - (vector.x + (this.rowEntryPrefab.GetComponent<LayoutElement>().minWidth * this.canvasScaler.GetCanvasScale() + DropDown.edgePadding.x))), -Mathf.Min(0f, vector.y - (this.scrollRect.rectTransform().sizeDelta.y * this.canvasScaler.GetCanvasScale() + DropDown.edgePadding.y)));
		vector += v;
		this.scrollRect.rectTransform().SetPosition(vector);
	}

	// Token: 0x06006054 RID: 24660 RVA: 0x0023D4B0 File Offset: 0x0023B6B0
	public void Close()
	{
		if (!this.open)
		{
			return;
		}
		this.open = false;
		foreach (KeyValuePair<IListableOption, GameObject> keyValuePair in this.rowLookup)
		{
			keyValuePair.Value.SetActive(false);
		}
		this.scrollRect.SetActive(false);
	}

	// Token: 0x040040FF RID: 16639
	public GameObject targetDropDownContainer;

	// Token: 0x04004100 RID: 16640
	public LocText selectedLabel;

	// Token: 0x04004102 RID: 16642
	public KButton openButton;

	// Token: 0x04004103 RID: 16643
	public Transform contentContainer;

	// Token: 0x04004104 RID: 16644
	public GameObject scrollRect;

	// Token: 0x04004105 RID: 16645
	public RectTransform dropdownAlignmentTarget;

	// Token: 0x04004106 RID: 16646
	public GameObject rowEntryPrefab;

	// Token: 0x04004107 RID: 16647
	public bool addEmptyRow = true;

	// Token: 0x04004108 RID: 16648
	private static Vector2 edgePadding = new Vector2(8f, 8f);

	// Token: 0x04004109 RID: 16649
	public object targetData;

	// Token: 0x0400410A RID: 16650
	private List<IListableOption> entries = new List<IListableOption>();

	// Token: 0x0400410B RID: 16651
	private Action<IListableOption, object> onEntrySelectedAction;

	// Token: 0x0400410C RID: 16652
	private Action<DropDownEntry, object> rowRefreshAction;

	// Token: 0x0400410D RID: 16653
	public Dictionary<IListableOption, GameObject> rowLookup = new Dictionary<IListableOption, GameObject>();

	// Token: 0x0400410E RID: 16654
	private Func<IListableOption, IListableOption, object, int> sortFunction;

	// Token: 0x0400410F RID: 16655
	private GameObject emptyRow;

	// Token: 0x04004110 RID: 16656
	private string emptyRowLabel;

	// Token: 0x04004111 RID: 16657
	private Sprite emptyRowSprite;

	// Token: 0x04004112 RID: 16658
	private bool built;

	// Token: 0x04004113 RID: 16659
	private bool displaySelectedValueWhenClosed = true;

	// Token: 0x04004114 RID: 16660
	private const int ROWS_BEFORE_SCROLL = 8;

	// Token: 0x04004115 RID: 16661
	private KCanvasScaler canvasScaler;
}
