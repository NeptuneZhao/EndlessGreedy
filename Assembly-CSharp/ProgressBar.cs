using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D17 RID: 3351
[AddComponentMenu("KMonoBehaviour/scripts/ProgressBar")]
public class ProgressBar : KMonoBehaviour
{
	// Token: 0x17000770 RID: 1904
	// (get) Token: 0x0600689F RID: 26783 RVA: 0x0027317A File Offset: 0x0027137A
	// (set) Token: 0x060068A0 RID: 26784 RVA: 0x00273187 File Offset: 0x00271387
	public Color barColor
	{
		get
		{
			return this.bar.color;
		}
		set
		{
			this.bar.color = value;
		}
	}

	// Token: 0x17000771 RID: 1905
	// (get) Token: 0x060068A1 RID: 26785 RVA: 0x00273195 File Offset: 0x00271395
	// (set) Token: 0x060068A2 RID: 26786 RVA: 0x002731A2 File Offset: 0x002713A2
	public float PercentFull
	{
		get
		{
			return this.bar.fillAmount;
		}
		set
		{
			this.bar.fillAmount = value;
		}
	}

	// Token: 0x060068A3 RID: 26787 RVA: 0x002731B0 File Offset: 0x002713B0
	public void SetVisibility(bool visible)
	{
		this.lastVisibilityValue = visible;
		this.RefreshVisibility();
	}

	// Token: 0x060068A4 RID: 26788 RVA: 0x002731C0 File Offset: 0x002713C0
	private void RefreshVisibility()
	{
		int myWorldId = base.gameObject.GetMyWorldId();
		bool flag = this.lastVisibilityValue;
		flag &= (!this.hasBeenInitialize || myWorldId == ClusterManager.Instance.activeWorldId);
		flag &= (!this.autoHide || SimDebugView.Instance == null || SimDebugView.Instance.GetMode() == OverlayModes.None.ID);
		base.gameObject.SetActive(flag);
		if (this.updatePercentFull == null || this.updatePercentFull.Target.IsNullOrDestroyed())
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x060068A5 RID: 26789 RVA: 0x0027325C File Offset: 0x0027145C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.hasBeenInitialize = true;
		if (this.autoHide)
		{
			this.overlayUpdateHandle = Game.Instance.Subscribe(1798162660, new Action<object>(this.OnOverlayChanged));
			if (SimDebugView.Instance != null && SimDebugView.Instance.GetMode() != OverlayModes.None.ID)
			{
				base.gameObject.SetActive(false);
			}
		}
		Game.Instance.Subscribe(1983128072, new Action<object>(this.OnActiveWorldChanged));
		this.SetWorldActive(ClusterManager.Instance.activeWorldId);
		base.enabled = (this.updatePercentFull != null);
		this.RefreshVisibility();
	}

	// Token: 0x060068A6 RID: 26790 RVA: 0x00273310 File Offset: 0x00271510
	private void OnActiveWorldChanged(object data)
	{
		global::Tuple<int, int> tuple = (global::Tuple<int, int>)data;
		this.SetWorldActive(tuple.first);
	}

	// Token: 0x060068A7 RID: 26791 RVA: 0x00273330 File Offset: 0x00271530
	private void SetWorldActive(int worldId)
	{
		this.RefreshVisibility();
	}

	// Token: 0x060068A8 RID: 26792 RVA: 0x00273338 File Offset: 0x00271538
	public void SetUpdateFunc(Func<float> func)
	{
		this.updatePercentFull = func;
		base.enabled = (this.updatePercentFull != null);
	}

	// Token: 0x060068A9 RID: 26793 RVA: 0x00273350 File Offset: 0x00271550
	public virtual void Update()
	{
		if (this.updatePercentFull != null && !this.updatePercentFull.Target.IsNullOrDestroyed())
		{
			this.PercentFull = this.updatePercentFull();
		}
	}

	// Token: 0x060068AA RID: 26794 RVA: 0x0027337D File Offset: 0x0027157D
	public virtual void OnOverlayChanged(object data = null)
	{
		this.RefreshVisibility();
	}

	// Token: 0x060068AB RID: 26795 RVA: 0x00273388 File Offset: 0x00271588
	public void Retarget(GameObject entity)
	{
		Vector3 vector = entity.transform.GetPosition() + Vector3.down * 0.5f;
		Building component = entity.GetComponent<Building>();
		if (component != null)
		{
			vector -= Vector3.right * 0.5f * (float)(component.Def.WidthInCells % 2);
		}
		else
		{
			vector -= Vector3.right * 0.5f;
		}
		base.transform.SetPosition(vector);
	}

	// Token: 0x060068AC RID: 26796 RVA: 0x00273413 File Offset: 0x00271613
	protected override void OnCleanUp()
	{
		if (this.overlayUpdateHandle != -1)
		{
			Game.Instance.Unsubscribe(this.overlayUpdateHandle);
		}
		Game.Instance.Unsubscribe(1983128072, new Action<object>(this.OnActiveWorldChanged));
		base.OnCleanUp();
	}

	// Token: 0x060068AD RID: 26797 RVA: 0x0027344F File Offset: 0x0027164F
	private void OnBecameInvisible()
	{
		base.enabled = false;
	}

	// Token: 0x060068AE RID: 26798 RVA: 0x00273458 File Offset: 0x00271658
	private void OnBecameVisible()
	{
		base.enabled = true;
	}

	// Token: 0x060068AF RID: 26799 RVA: 0x00273464 File Offset: 0x00271664
	public static ProgressBar CreateProgressBar(GameObject entity, Func<float> updateFunc)
	{
		ProgressBar progressBar = Util.KInstantiateUI<ProgressBar>(ProgressBarsConfig.Instance.progressBarPrefab, null, false);
		progressBar.SetUpdateFunc(updateFunc);
		progressBar.transform.SetParent(GameScreenManager.Instance.worldSpaceCanvas.transform);
		progressBar.name = ((entity != null) ? (entity.name + "_") : "") + " ProgressBar";
		progressBar.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("ProgressBar");
		progressBar.Update();
		progressBar.Retarget(entity);
		return progressBar;
	}

	// Token: 0x040046CD RID: 18125
	public Image bar;

	// Token: 0x040046CE RID: 18126
	private Func<float> updatePercentFull;

	// Token: 0x040046CF RID: 18127
	private int overlayUpdateHandle = -1;

	// Token: 0x040046D0 RID: 18128
	public bool autoHide = true;

	// Token: 0x040046D1 RID: 18129
	private bool lastVisibilityValue = true;

	// Token: 0x040046D2 RID: 18130
	private bool hasBeenInitialize;
}
