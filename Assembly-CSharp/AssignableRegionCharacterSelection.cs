using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BE1 RID: 3041
[AddComponentMenu("KMonoBehaviour/scripts/AssignableRegionCharacterSelection")]
public class AssignableRegionCharacterSelection : KMonoBehaviour
{
	// Token: 0x14000021 RID: 33
	// (add) Token: 0x06005C83 RID: 23683 RVA: 0x0021D8A4 File Offset: 0x0021BAA4
	// (remove) Token: 0x06005C84 RID: 23684 RVA: 0x0021D8DC File Offset: 0x0021BADC
	public event Action<MinionIdentity> OnDuplicantSelected;

	// Token: 0x06005C85 RID: 23685 RVA: 0x0021D911 File Offset: 0x0021BB11
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.buttonPool = new UIPool<KButton>(this.buttonPrefab);
		base.gameObject.SetActive(false);
	}

	// Token: 0x06005C86 RID: 23686 RVA: 0x0021D938 File Offset: 0x0021BB38
	public void Open()
	{
		base.gameObject.SetActive(true);
		this.buttonPool.ClearAll();
		foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
		{
			KButton btn = this.buttonPool.GetFreeElement(this.buttonParent, true);
			CrewPortrait componentInChildren = btn.GetComponentInChildren<CrewPortrait>();
			componentInChildren.SetIdentityObject(minionIdentity, true);
			this.portraitList.Add(componentInChildren);
			btn.ClearOnClick();
			btn.onClick += delegate()
			{
				this.SelectDuplicant(btn);
			};
			this.buttonIdentityMap.Add(btn, minionIdentity);
		}
	}

	// Token: 0x06005C87 RID: 23687 RVA: 0x0021DA20 File Offset: 0x0021BC20
	public void Close()
	{
		this.buttonPool.DestroyAllActive();
		this.buttonIdentityMap.Clear();
		this.portraitList.Clear();
		base.gameObject.SetActive(false);
	}

	// Token: 0x06005C88 RID: 23688 RVA: 0x0021DA4F File Offset: 0x0021BC4F
	private void SelectDuplicant(KButton btn)
	{
		if (this.OnDuplicantSelected != null)
		{
			this.OnDuplicantSelected(this.buttonIdentityMap[btn]);
		}
		this.Close();
	}

	// Token: 0x04003DBD RID: 15805
	[SerializeField]
	private KButton buttonPrefab;

	// Token: 0x04003DBE RID: 15806
	[SerializeField]
	private GameObject buttonParent;

	// Token: 0x04003DBF RID: 15807
	private UIPool<KButton> buttonPool;

	// Token: 0x04003DC0 RID: 15808
	private Dictionary<KButton, MinionIdentity> buttonIdentityMap = new Dictionary<KButton, MinionIdentity>();

	// Token: 0x04003DC1 RID: 15809
	private List<CrewPortrait> portraitList = new List<CrewPortrait>();
}
