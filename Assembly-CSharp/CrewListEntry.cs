using System;
using Klei.AI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000CA3 RID: 3235
[AddComponentMenu("KMonoBehaviour/scripts/CrewListEntry")]
public class CrewListEntry : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	// Token: 0x17000746 RID: 1862
	// (get) Token: 0x060063AD RID: 25517 RVA: 0x00252BDA File Offset: 0x00250DDA
	public MinionIdentity Identity
	{
		get
		{
			return this.identity;
		}
	}

	// Token: 0x060063AE RID: 25518 RVA: 0x00252BE2 File Offset: 0x00250DE2
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.mouseOver = true;
		this.BGImage.enabled = true;
		this.BorderHighlight.color = new Color(0.65882355f, 0.2901961f, 0.4745098f);
	}

	// Token: 0x060063AF RID: 25519 RVA: 0x00252C16 File Offset: 0x00250E16
	public void OnPointerExit(PointerEventData eventData)
	{
		this.mouseOver = false;
		this.BGImage.enabled = false;
		this.BorderHighlight.color = new Color(0.8f, 0.8f, 0.8f);
	}

	// Token: 0x060063B0 RID: 25520 RVA: 0x00252C4C File Offset: 0x00250E4C
	public void OnPointerClick(PointerEventData eventData)
	{
		bool focus = Time.unscaledTime - this.lastClickTime < 0.3f;
		this.SelectCrewMember(focus);
		this.lastClickTime = Time.unscaledTime;
	}

	// Token: 0x060063B1 RID: 25521 RVA: 0x00252C80 File Offset: 0x00250E80
	public virtual void Populate(MinionIdentity _identity)
	{
		this.identity = _identity;
		if (this.portrait == null)
		{
			GameObject parent = (this.crewPortraitParent != null) ? this.crewPortraitParent : base.gameObject;
			this.portrait = Util.KInstantiateUI<CrewPortrait>(this.PortraitPrefab.gameObject, parent, false);
			if (this.crewPortraitParent == null)
			{
				this.portrait.transform.SetSiblingIndex(2);
			}
		}
		this.portrait.SetIdentityObject(_identity, true);
	}

	// Token: 0x060063B2 RID: 25522 RVA: 0x00252D03 File Offset: 0x00250F03
	public virtual void Refresh()
	{
	}

	// Token: 0x060063B3 RID: 25523 RVA: 0x00252D05 File Offset: 0x00250F05
	public void RefreshCrewPortraitContent()
	{
		if (this.portrait != null)
		{
			this.portrait.ForceRefresh();
		}
	}

	// Token: 0x060063B4 RID: 25524 RVA: 0x00252D20 File Offset: 0x00250F20
	private string seniorityString()
	{
		return this.identity.GetAttributes().GetProfessionString(true);
	}

	// Token: 0x060063B5 RID: 25525 RVA: 0x00252D34 File Offset: 0x00250F34
	public void SelectCrewMember(bool focus)
	{
		if (focus)
		{
			SelectTool.Instance.SelectAndFocus(this.identity.transform.GetPosition(), this.identity.GetComponent<KSelectable>(), new Vector3(8f, 0f, 0f));
			return;
		}
		SelectTool.Instance.Select(this.identity.GetComponent<KSelectable>(), false);
	}

	// Token: 0x040043AD RID: 17325
	protected MinionIdentity identity;

	// Token: 0x040043AE RID: 17326
	protected CrewPortrait portrait;

	// Token: 0x040043AF RID: 17327
	public CrewPortrait PortraitPrefab;

	// Token: 0x040043B0 RID: 17328
	public GameObject crewPortraitParent;

	// Token: 0x040043B1 RID: 17329
	protected bool mouseOver;

	// Token: 0x040043B2 RID: 17330
	public Image BorderHighlight;

	// Token: 0x040043B3 RID: 17331
	public Image BGImage;

	// Token: 0x040043B4 RID: 17332
	public float lastClickTime;
}
