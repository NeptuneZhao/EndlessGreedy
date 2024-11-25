using System;
using UnityEngine;

// Token: 0x02000578 RID: 1400
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/KSelectable")]
public class KSelectable : KMonoBehaviour
{
	// Token: 0x1700015B RID: 347
	// (get) Token: 0x06002078 RID: 8312 RVA: 0x000B5DAF File Offset: 0x000B3FAF
	public bool IsSelected
	{
		get
		{
			return this.selected;
		}
	}

	// Token: 0x1700015C RID: 348
	// (get) Token: 0x06002079 RID: 8313 RVA: 0x000B5DB7 File Offset: 0x000B3FB7
	// (set) Token: 0x0600207A RID: 8314 RVA: 0x000B5DC9 File Offset: 0x000B3FC9
	public bool IsSelectable
	{
		get
		{
			return this.selectable && base.isActiveAndEnabled;
		}
		set
		{
			this.selectable = value;
		}
	}

	// Token: 0x1700015D RID: 349
	// (get) Token: 0x0600207B RID: 8315 RVA: 0x000B5DD2 File Offset: 0x000B3FD2
	public bool DisableSelectMarker
	{
		get
		{
			return this.disableSelectMarker;
		}
	}

	// Token: 0x0600207C RID: 8316 RVA: 0x000B5DDC File Offset: 0x000B3FDC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.statusItemGroup = new StatusItemGroup(base.gameObject);
		base.GetComponent<KPrefabID>() != null;
		if (this.entityName == null || this.entityName.Length <= 0)
		{
			this.SetName(base.name);
		}
		if (this.entityGender == null)
		{
			this.entityGender = "NB";
		}
	}

	// Token: 0x0600207D RID: 8317 RVA: 0x000B5E44 File Offset: 0x000B4044
	public virtual string GetName()
	{
		if (this.entityName == null || this.entityName == "" || this.entityName.Length <= 0)
		{
			global::Debug.Log("Warning Item has blank name!", base.gameObject);
			return base.name;
		}
		return this.entityName;
	}

	// Token: 0x0600207E RID: 8318 RVA: 0x000B5E96 File Offset: 0x000B4096
	public void SetStatusIndicatorOffset(Vector3 offset)
	{
		if (this.statusItemGroup == null)
		{
			return;
		}
		this.statusItemGroup.SetOffset(offset);
	}

	// Token: 0x0600207F RID: 8319 RVA: 0x000B5EAD File Offset: 0x000B40AD
	public void SetName(string name)
	{
		this.entityName = name;
	}

	// Token: 0x06002080 RID: 8320 RVA: 0x000B5EB6 File Offset: 0x000B40B6
	public void SetGender(string Gender)
	{
		this.entityGender = Gender;
	}

	// Token: 0x06002081 RID: 8321 RVA: 0x000B5EC0 File Offset: 0x000B40C0
	public float GetZoom()
	{
		Bounds bounds = Util.GetBounds(base.gameObject);
		return 1.05f * Mathf.Max(bounds.extents.x, bounds.extents.y);
	}

	// Token: 0x06002082 RID: 8322 RVA: 0x000B5EFC File Offset: 0x000B40FC
	public Vector3 GetPortraitLocation()
	{
		return Util.GetBounds(base.gameObject).center;
	}

	// Token: 0x06002083 RID: 8323 RVA: 0x000B5F1C File Offset: 0x000B411C
	private void ClearHighlight()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.HighlightColour = new Color(0f, 0f, 0f, 0f);
		}
		base.Trigger(-1201923725, false);
	}

	// Token: 0x06002084 RID: 8324 RVA: 0x000B5F70 File Offset: 0x000B4170
	private void ApplyHighlight(float highlight)
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.HighlightColour = new Color(highlight, highlight, highlight, highlight);
		}
		base.Trigger(-1201923725, true);
	}

	// Token: 0x06002085 RID: 8325 RVA: 0x000B5FB4 File Offset: 0x000B41B4
	public void Select()
	{
		this.selected = true;
		this.ClearHighlight();
		this.ApplyHighlight(0.2f);
		base.Trigger(-1503271301, true);
		if (base.GetComponent<LoopingSounds>() != null)
		{
			base.GetComponent<LoopingSounds>().UpdateObjectSelection(this.selected);
		}
		if (base.transform.GetComponentInParent<LoopingSounds>() != null)
		{
			base.transform.GetComponentInParent<LoopingSounds>().UpdateObjectSelection(this.selected);
		}
		int childCount = base.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			int childCount2 = base.transform.GetChild(i).childCount;
			for (int j = 0; j < childCount2; j++)
			{
				if (base.transform.GetChild(i).transform.GetChild(j).GetComponent<LoopingSounds>() != null)
				{
					base.transform.GetChild(i).transform.GetChild(j).GetComponent<LoopingSounds>().UpdateObjectSelection(this.selected);
				}
			}
		}
		this.UpdateWorkerSelection(this.selected);
		this.UpdateWorkableSelection(this.selected);
	}

	// Token: 0x06002086 RID: 8326 RVA: 0x000B60CC File Offset: 0x000B42CC
	public void Unselect()
	{
		if (this.selected)
		{
			this.selected = false;
			this.ClearHighlight();
			base.Trigger(-1503271301, false);
		}
		if (base.GetComponent<LoopingSounds>() != null)
		{
			base.GetComponent<LoopingSounds>().UpdateObjectSelection(this.selected);
		}
		if (base.transform.GetComponentInParent<LoopingSounds>() != null)
		{
			base.transform.GetComponentInParent<LoopingSounds>().UpdateObjectSelection(this.selected);
		}
		foreach (object obj in base.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.GetComponent<LoopingSounds>() != null)
			{
				transform.GetComponent<LoopingSounds>().UpdateObjectSelection(this.selected);
			}
		}
		this.UpdateWorkerSelection(this.selected);
		this.UpdateWorkableSelection(this.selected);
	}

	// Token: 0x06002087 RID: 8327 RVA: 0x000B61C4 File Offset: 0x000B43C4
	public void Hover(bool playAudio)
	{
		this.ClearHighlight();
		if (!DebugHandler.HideUI)
		{
			this.ApplyHighlight(0.25f);
		}
		if (playAudio)
		{
			this.PlayHoverSound();
		}
	}

	// Token: 0x06002088 RID: 8328 RVA: 0x000B61E7 File Offset: 0x000B43E7
	private void PlayHoverSound()
	{
		if (CellSelectionObject.IsSelectionObject(base.gameObject))
		{
			return;
		}
		UISounds.PlaySound(UISounds.Sound.Object_Mouseover);
	}

	// Token: 0x06002089 RID: 8329 RVA: 0x000B61FD File Offset: 0x000B43FD
	public void Unhover()
	{
		if (!this.selected)
		{
			this.ClearHighlight();
		}
	}

	// Token: 0x0600208A RID: 8330 RVA: 0x000B620D File Offset: 0x000B440D
	public Guid ToggleStatusItem(StatusItem status_item, bool on, object data = null)
	{
		if (on)
		{
			return this.AddStatusItem(status_item, data);
		}
		return this.RemoveStatusItem(status_item, false);
	}

	// Token: 0x0600208B RID: 8331 RVA: 0x000B6223 File Offset: 0x000B4423
	public Guid ToggleStatusItem(StatusItem status_item, Guid guid, bool show, object data = null)
	{
		if (show)
		{
			if (guid != Guid.Empty)
			{
				return guid;
			}
			return this.AddStatusItem(status_item, data);
		}
		else
		{
			if (guid != Guid.Empty)
			{
				return this.RemoveStatusItem(guid, false);
			}
			return guid;
		}
	}

	// Token: 0x0600208C RID: 8332 RVA: 0x000B6258 File Offset: 0x000B4458
	public Guid SetStatusItem(StatusItemCategory category, StatusItem status_item, object data = null)
	{
		if (this.statusItemGroup == null)
		{
			return Guid.Empty;
		}
		return this.statusItemGroup.SetStatusItem(category, status_item, data);
	}

	// Token: 0x0600208D RID: 8333 RVA: 0x000B6276 File Offset: 0x000B4476
	public Guid ReplaceStatusItem(Guid guid, StatusItem status_item, object data = null)
	{
		if (this.statusItemGroup == null)
		{
			return Guid.Empty;
		}
		if (guid != Guid.Empty)
		{
			this.statusItemGroup.RemoveStatusItem(guid, false);
		}
		return this.AddStatusItem(status_item, data);
	}

	// Token: 0x0600208E RID: 8334 RVA: 0x000B62A9 File Offset: 0x000B44A9
	public Guid AddStatusItem(StatusItem status_item, object data = null)
	{
		if (this.statusItemGroup == null)
		{
			return Guid.Empty;
		}
		return this.statusItemGroup.AddStatusItem(status_item, data, null);
	}

	// Token: 0x0600208F RID: 8335 RVA: 0x000B62C7 File Offset: 0x000B44C7
	public Guid RemoveStatusItem(StatusItem status_item, bool immediate = false)
	{
		if (this.statusItemGroup == null)
		{
			return Guid.Empty;
		}
		this.statusItemGroup.RemoveStatusItem(status_item, immediate);
		return Guid.Empty;
	}

	// Token: 0x06002090 RID: 8336 RVA: 0x000B62EA File Offset: 0x000B44EA
	public Guid RemoveStatusItem(Guid guid, bool immediate = false)
	{
		if (this.statusItemGroup == null)
		{
			return Guid.Empty;
		}
		this.statusItemGroup.RemoveStatusItem(guid, immediate);
		return Guid.Empty;
	}

	// Token: 0x06002091 RID: 8337 RVA: 0x000B630D File Offset: 0x000B450D
	public bool HasStatusItem(StatusItem status_item)
	{
		return this.statusItemGroup != null && this.statusItemGroup.HasStatusItem(status_item);
	}

	// Token: 0x06002092 RID: 8338 RVA: 0x000B6325 File Offset: 0x000B4525
	public StatusItemGroup.Entry GetStatusItem(StatusItemCategory category)
	{
		return this.statusItemGroup.GetStatusItem(category);
	}

	// Token: 0x06002093 RID: 8339 RVA: 0x000B6333 File Offset: 0x000B4533
	public StatusItemGroup GetStatusItemGroup()
	{
		return this.statusItemGroup;
	}

	// Token: 0x06002094 RID: 8340 RVA: 0x000B633C File Offset: 0x000B453C
	public void UpdateWorkerSelection(bool selected)
	{
		Workable[] components = base.GetComponents<Workable>();
		if (components.Length != 0)
		{
			for (int i = 0; i < components.Length; i++)
			{
				if (components[i].worker != null && components[i].GetComponent<LoopingSounds>() != null)
				{
					components[i].GetComponent<LoopingSounds>().UpdateObjectSelection(selected);
				}
			}
		}
	}

	// Token: 0x06002095 RID: 8341 RVA: 0x000B6390 File Offset: 0x000B4590
	public void UpdateWorkableSelection(bool selected)
	{
		WorkerBase component = base.GetComponent<WorkerBase>();
		if (component != null && component.GetWorkable() != null)
		{
			Workable workable = base.GetComponent<WorkerBase>().GetWorkable();
			if (workable.GetComponent<LoopingSounds>() != null)
			{
				workable.GetComponent<LoopingSounds>().UpdateObjectSelection(selected);
			}
		}
	}

	// Token: 0x06002096 RID: 8342 RVA: 0x000B63E1 File Offset: 0x000B45E1
	protected override void OnLoadLevel()
	{
		this.OnCleanUp();
		base.OnLoadLevel();
	}

	// Token: 0x06002097 RID: 8343 RVA: 0x000B63F0 File Offset: 0x000B45F0
	protected override void OnCleanUp()
	{
		if (this.statusItemGroup != null)
		{
			this.statusItemGroup.Destroy();
			this.statusItemGroup = null;
		}
		if (this.selected && SelectTool.Instance != null)
		{
			if (SelectTool.Instance.selected == this)
			{
				SelectTool.Instance.Select(null, true);
			}
			else
			{
				this.Unselect();
			}
		}
		base.OnCleanUp();
	}

	// Token: 0x04001249 RID: 4681
	private const float hoverHighlight = 0.25f;

	// Token: 0x0400124A RID: 4682
	private const float selectHighlight = 0.2f;

	// Token: 0x0400124B RID: 4683
	public string entityName;

	// Token: 0x0400124C RID: 4684
	public string entityGender;

	// Token: 0x0400124D RID: 4685
	private bool selected;

	// Token: 0x0400124E RID: 4686
	[SerializeField]
	private bool selectable = true;

	// Token: 0x0400124F RID: 4687
	[SerializeField]
	private bool disableSelectMarker;

	// Token: 0x04001250 RID: 4688
	private StatusItemGroup statusItemGroup;
}
