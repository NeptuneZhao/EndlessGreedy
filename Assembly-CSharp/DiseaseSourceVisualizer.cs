using System;
using Klei.AI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200085D RID: 2141
[AddComponentMenu("KMonoBehaviour/scripts/DiseaseSourceVisualizer")]
public class DiseaseSourceVisualizer : KMonoBehaviour
{
	// Token: 0x06003BA0 RID: 15264 RVA: 0x001488DD File Offset: 0x00146ADD
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UpdateVisibility();
		Components.DiseaseSourceVisualizers.Add(this);
	}

	// Token: 0x06003BA1 RID: 15265 RVA: 0x001488F8 File Offset: 0x00146AF8
	protected override void OnCleanUp()
	{
		OverlayScreen instance = OverlayScreen.Instance;
		instance.OnOverlayChanged = (Action<HashedString>)Delegate.Remove(instance.OnOverlayChanged, new Action<HashedString>(this.OnViewModeChanged));
		base.OnCleanUp();
		Components.DiseaseSourceVisualizers.Remove(this);
		if (this.visualizer != null)
		{
			UnityEngine.Object.Destroy(this.visualizer);
			this.visualizer = null;
		}
	}

	// Token: 0x06003BA2 RID: 15266 RVA: 0x0014895C File Offset: 0x00146B5C
	private void CreateVisualizer()
	{
		if (this.visualizer != null)
		{
			return;
		}
		if (GameScreenManager.Instance.worldSpaceCanvas == null)
		{
			return;
		}
		this.visualizer = Util.KInstantiate(Assets.UIPrefabs.ResourceVisualizer, GameScreenManager.Instance.worldSpaceCanvas, null);
	}

	// Token: 0x06003BA3 RID: 15267 RVA: 0x001489AC File Offset: 0x00146BAC
	public void UpdateVisibility()
	{
		this.CreateVisualizer();
		if (string.IsNullOrEmpty(this.alwaysShowDisease))
		{
			this.visible = false;
		}
		else
		{
			Disease disease = Db.Get().Diseases.Get(this.alwaysShowDisease);
			if (disease != null)
			{
				this.SetVisibleDisease(disease);
			}
		}
		if (OverlayScreen.Instance != null)
		{
			this.Show(OverlayScreen.Instance.GetMode());
		}
	}

	// Token: 0x06003BA4 RID: 15268 RVA: 0x00148A14 File Offset: 0x00146C14
	private void SetVisibleDisease(Disease disease)
	{
		Sprite overlaySprite = Assets.instance.DiseaseVisualization.overlaySprite;
		Color32 colorByName = GlobalAssets.Instance.colorSet.GetColorByName(disease.overlayColourName);
		Image component = this.visualizer.transform.GetChild(0).GetComponent<Image>();
		component.sprite = overlaySprite;
		component.color = colorByName;
		this.visible = true;
	}

	// Token: 0x06003BA5 RID: 15269 RVA: 0x00148A76 File Offset: 0x00146C76
	private void Update()
	{
		if (this.visualizer == null)
		{
			return;
		}
		this.visualizer.transform.SetPosition(base.transform.GetPosition() + this.offset);
	}

	// Token: 0x06003BA6 RID: 15270 RVA: 0x00148AAE File Offset: 0x00146CAE
	private void OnViewModeChanged(HashedString mode)
	{
		this.Show(mode);
	}

	// Token: 0x06003BA7 RID: 15271 RVA: 0x00148AB7 File Offset: 0x00146CB7
	public void Show(HashedString mode)
	{
		base.enabled = (this.visible && mode == OverlayModes.Disease.ID);
		if (this.visualizer != null)
		{
			this.visualizer.SetActive(base.enabled);
		}
	}

	// Token: 0x04002408 RID: 9224
	[SerializeField]
	private Vector3 offset;

	// Token: 0x04002409 RID: 9225
	private GameObject visualizer;

	// Token: 0x0400240A RID: 9226
	private bool visible;

	// Token: 0x0400240B RID: 9227
	public string alwaysShowDisease;
}
