using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DEE RID: 3566
public class Vignette : KMonoBehaviour
{
	// Token: 0x0600713A RID: 28986 RVA: 0x002AD58F File Offset: 0x002AB78F
	public static void DestroyInstance()
	{
		Vignette.Instance = null;
	}

	// Token: 0x0600713B RID: 28987 RVA: 0x002AD598 File Offset: 0x002AB798
	protected override void OnSpawn()
	{
		this.looping_sounds = base.GetComponent<LoopingSounds>();
		base.OnSpawn();
		Vignette.Instance = this;
		this.defaultColor = this.image.color;
		Game.Instance.Subscribe(1983128072, new Action<object>(this.Refresh));
		Game.Instance.Subscribe(1585324898, new Action<object>(this.Refresh));
		Game.Instance.Subscribe(-1393151672, new Action<object>(this.Refresh));
		Game.Instance.Subscribe(-741654735, new Action<object>(this.Refresh));
		Game.Instance.Subscribe(-2062778933, new Action<object>(this.Refresh));
	}

	// Token: 0x0600713C RID: 28988 RVA: 0x002AD65A File Offset: 0x002AB85A
	public void SetColor(Color color)
	{
		this.image.color = color;
	}

	// Token: 0x0600713D RID: 28989 RVA: 0x002AD668 File Offset: 0x002AB868
	public void Refresh(object data)
	{
		AlertStateManager.Instance alertManager = ClusterManager.Instance.activeWorld.AlertManager;
		if (alertManager == null)
		{
			return;
		}
		if (alertManager.IsYellowAlert())
		{
			this.SetColor(this.yellowAlertColor);
			if (!this.showingYellowAlert)
			{
				this.looping_sounds.StartSound(GlobalAssets.GetSound("YellowAlert_LP", false), true, false, true);
				this.showingYellowAlert = true;
			}
		}
		else
		{
			this.showingYellowAlert = false;
			this.looping_sounds.StopSound(GlobalAssets.GetSound("YellowAlert_LP", false));
		}
		if (alertManager.IsRedAlert())
		{
			this.SetColor(this.redAlertColor);
			if (!this.showingRedAlert)
			{
				this.looping_sounds.StartSound(GlobalAssets.GetSound("RedAlert_LP", false), true, false, true);
				this.showingRedAlert = true;
			}
		}
		else
		{
			this.showingRedAlert = false;
			this.looping_sounds.StopSound(GlobalAssets.GetSound("RedAlert_LP", false));
		}
		if (!this.showingRedAlert && !this.showingYellowAlert)
		{
			this.Reset();
		}
	}

	// Token: 0x0600713E RID: 28990 RVA: 0x002AD758 File Offset: 0x002AB958
	public void Reset()
	{
		this.SetColor(this.defaultColor);
		this.showingRedAlert = false;
		this.showingYellowAlert = false;
		this.looping_sounds.StopSound(GlobalAssets.GetSound("RedAlert_LP", false));
		this.looping_sounds.StopSound(GlobalAssets.GetSound("YellowAlert_LP", false));
	}

	// Token: 0x04004DE1 RID: 19937
	[SerializeField]
	private Image image;

	// Token: 0x04004DE2 RID: 19938
	public Color defaultColor;

	// Token: 0x04004DE3 RID: 19939
	public Color redAlertColor = new Color(1f, 0f, 0f, 0.3f);

	// Token: 0x04004DE4 RID: 19940
	public Color yellowAlertColor = new Color(1f, 1f, 0f, 0.3f);

	// Token: 0x04004DE5 RID: 19941
	public static Vignette Instance;

	// Token: 0x04004DE6 RID: 19942
	private LoopingSounds looping_sounds;

	// Token: 0x04004DE7 RID: 19943
	private bool showingRedAlert;

	// Token: 0x04004DE8 RID: 19944
	private bool showingYellowAlert;
}
