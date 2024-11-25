using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000C7E RID: 3198
public class KleiItemDropScreen_PermitVis : KMonoBehaviour
{
	// Token: 0x06006270 RID: 25200 RVA: 0x0024C104 File Offset: 0x0024A304
	public void ConfigureWith(DropScreenPresentationInfo info)
	{
		this.ResetState();
		this.equipmentVis.gameObject.SetActive(false);
		this.fallbackVis.gameObject.SetActive(false);
		if (info.UseEquipmentVis)
		{
			this.equipmentVis.gameObject.SetActive(true);
			this.equipmentVis.ConfigureWith(info);
			return;
		}
		this.fallbackVis.gameObject.SetActive(true);
		this.fallbackVis.ConfigureWith(info);
	}

	// Token: 0x06006271 RID: 25201 RVA: 0x0024C17C File Offset: 0x0024A37C
	public Promise AnimateIn()
	{
		return Updater.RunRoutine(this, this.AnimateInRoutine());
	}

	// Token: 0x06006272 RID: 25202 RVA: 0x0024C18A File Offset: 0x0024A38A
	public Promise AnimateOut()
	{
		return Updater.RunRoutine(this, this.AnimateOutRoutine());
	}

	// Token: 0x06006273 RID: 25203 RVA: 0x0024C198 File Offset: 0x0024A398
	private IEnumerator AnimateInRoutine()
	{
		this.root.gameObject.SetActive(true);
		yield return Updater.Ease(delegate(Vector3 v3)
		{
			this.root.transform.localScale = v3;
		}, this.root.transform.localScale, Vector3.one, 0.5f, Easing.EaseOutBack, -1f);
		yield break;
	}

	// Token: 0x06006274 RID: 25204 RVA: 0x0024C1A7 File Offset: 0x0024A3A7
	private IEnumerator AnimateOutRoutine()
	{
		yield return Updater.Ease(delegate(Vector3 v3)
		{
			this.root.transform.localScale = v3;
		}, this.root.transform.localScale, Vector3.zero, 0.25f, null, -1f);
		this.root.gameObject.SetActive(true);
		yield break;
	}

	// Token: 0x06006275 RID: 25205 RVA: 0x0024C1B6 File Offset: 0x0024A3B6
	public void ResetState()
	{
		this.root.transform.localScale = Vector3.zero;
	}

	// Token: 0x040042D2 RID: 17106
	[SerializeField]
	private RectTransform root;

	// Token: 0x040042D3 RID: 17107
	[Header("Different Permit Visualizers")]
	[SerializeField]
	private KleiItemDropScreen_PermitVis_Fallback fallbackVis;

	// Token: 0x040042D4 RID: 17108
	[SerializeField]
	private KleiItemDropScreen_PermitVis_DupeEquipment equipmentVis;
}
