using System;
using ProcGen;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C32 RID: 3122
[AddComponentMenu("KMonoBehaviour/scripts/DestinationAsteroid2")]
public class DestinationAsteroid2 : KMonoBehaviour
{
	// Token: 0x14000025 RID: 37
	// (add) Token: 0x06005FC4 RID: 24516 RVA: 0x0023927C File Offset: 0x0023747C
	// (remove) Token: 0x06005FC5 RID: 24517 RVA: 0x002392B4 File Offset: 0x002374B4
	public event Action<ColonyDestinationAsteroidBeltData> OnClicked;

	// Token: 0x06005FC6 RID: 24518 RVA: 0x002392E9 File Offset: 0x002374E9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.button.onClick += this.OnClickInternal;
	}

	// Token: 0x06005FC7 RID: 24519 RVA: 0x00239308 File Offset: 0x00237508
	public void SetAsteroid(ColonyDestinationAsteroidBeltData newAsteroidData)
	{
		if (this.asteroidData == null || newAsteroidData.beltPath != this.asteroidData.beltPath)
		{
			this.asteroidData = newAsteroidData;
			ProcGen.World getStartWorld = newAsteroidData.GetStartWorld;
			KAnimFile kanimFile;
			Assets.TryGetAnim(getStartWorld.asteroidIcon.IsNullOrWhiteSpace() ? AsteroidGridEntity.DEFAULT_ASTEROID_ICON_ANIM : getStartWorld.asteroidIcon, out kanimFile);
			if (kanimFile != null)
			{
				this.asteroidImage.gameObject.SetActive(false);
				this.animController.AnimFiles = new KAnimFile[]
				{
					kanimFile
				};
				this.animController.initialMode = KAnim.PlayMode.Loop;
				this.animController.initialAnim = "idle_loop";
				this.animController.gameObject.SetActive(true);
				if (this.animController.HasAnimation(this.animController.initialAnim))
				{
					this.animController.Play(this.animController.initialAnim, KAnim.PlayMode.Loop, 1f, 0f);
				}
			}
			else
			{
				this.animController.gameObject.SetActive(false);
				this.asteroidImage.gameObject.SetActive(true);
				this.asteroidImage.sprite = this.asteroidData.sprite;
				this.imageDlcFrom.gameObject.SetActive(false);
			}
			Sprite sprite = null;
			if (DlcManager.IsDlcId(this.asteroidData.Layout.dlcIdFrom))
			{
				sprite = Assets.GetSprite(DlcManager.GetDlcSmallLogo(this.asteroidData.Layout.dlcIdFrom));
			}
			if (sprite != null)
			{
				this.imageDlcFrom.gameObject.SetActive(true);
				this.imageDlcFrom.sprite = sprite;
				return;
			}
			this.imageDlcFrom.gameObject.SetActive(false);
			this.imageDlcFrom.sprite = sprite;
		}
	}

	// Token: 0x06005FC8 RID: 24520 RVA: 0x002394D7 File Offset: 0x002376D7
	private void OnClickInternal()
	{
		DebugUtil.LogArgs(new object[]
		{
			"Clicked asteroid belt",
			this.asteroidData.beltPath
		});
		this.OnClicked(this.asteroidData);
	}

	// Token: 0x0400408D RID: 16525
	[SerializeField]
	private Image asteroidImage;

	// Token: 0x0400408E RID: 16526
	[SerializeField]
	private KButton button;

	// Token: 0x0400408F RID: 16527
	[SerializeField]
	private KBatchedAnimController animController;

	// Token: 0x04004090 RID: 16528
	[SerializeField]
	private Image imageDlcFrom;

	// Token: 0x04004092 RID: 16530
	private ColonyDestinationAsteroidBeltData asteroidData;
}
