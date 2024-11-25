using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DC5 RID: 3525
[AddComponentMenu("KMonoBehaviour/scripts/Slideshow")]
public class Slideshow : KMonoBehaviour
{
	// Token: 0x06006FD1 RID: 28625 RVA: 0x002A299C File Offset: 0x002A0B9C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.timeUntilNextSlide = this.timePerSlide;
		if (this.transparentIfEmpty && this.sprites != null && this.sprites.Length == 0)
		{
			this.imageTarget.color = Color.clear;
		}
		if (this.isExpandable)
		{
			this.button = base.GetComponent<KButton>();
			this.button.onClick += delegate()
			{
				if (this.onBeforePlay != null)
				{
					this.onBeforePlay();
				}
				SlideshowUpdateType slideshowUpdateType = this.updateType;
				if (slideshowUpdateType == SlideshowUpdateType.preloadedSprites)
				{
					VideoScreen.Instance.PlaySlideShow(this.sprites);
					return;
				}
				if (slideshowUpdateType != SlideshowUpdateType.loadOnDemand)
				{
					return;
				}
				VideoScreen.Instance.PlaySlideShow(this.files);
			};
		}
		if (this.nextButton != null)
		{
			this.nextButton.onClick += delegate()
			{
				this.nextSlide();
			};
		}
		if (this.prevButton != null)
		{
			this.prevButton.onClick += delegate()
			{
				this.prevSlide();
			};
		}
		if (this.pauseButton != null)
		{
			this.pauseButton.onClick += delegate()
			{
				this.SetPaused(!this.paused);
			};
		}
		if (this.closeButton != null)
		{
			this.closeButton.onClick += delegate()
			{
				VideoScreen.Instance.Stop();
				if (this.onEndingPlay != null)
				{
					this.onEndingPlay();
				}
			};
		}
	}

	// Token: 0x06006FD2 RID: 28626 RVA: 0x002A2AA4 File Offset: 0x002A0CA4
	public void SetPaused(bool state)
	{
		this.paused = state;
		if (this.pauseIcon != null)
		{
			this.pauseIcon.gameObject.SetActive(!this.paused);
		}
		if (this.unpauseIcon != null)
		{
			this.unpauseIcon.gameObject.SetActive(this.paused);
		}
		if (this.prevButton != null)
		{
			this.prevButton.gameObject.SetActive(this.paused);
		}
		if (this.nextButton != null)
		{
			this.nextButton.gameObject.SetActive(this.paused);
		}
	}

	// Token: 0x06006FD3 RID: 28627 RVA: 0x002A2B4C File Offset: 0x002A0D4C
	private void resetSlide(bool enable)
	{
		this.timeUntilNextSlide = this.timePerSlide;
		this.currentSlide = 0;
		if (enable)
		{
			this.imageTarget.color = Color.white;
			return;
		}
		if (this.transparentIfEmpty)
		{
			this.imageTarget.color = Color.clear;
		}
	}

	// Token: 0x06006FD4 RID: 28628 RVA: 0x002A2B98 File Offset: 0x002A0D98
	private Sprite loadSlide(string file)
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		Texture2D texture2D = new Texture2D(512, 768);
		texture2D.filterMode = FilterMode.Point;
		texture2D.LoadImage(File.ReadAllBytes(file));
		return Sprite.Create(texture2D, new Rect(Vector2.zero, new Vector2((float)texture2D.width, (float)texture2D.height)), new Vector2(0.5f, 0.5f), 100f, 0U, SpriteMeshType.FullRect);
	}

	// Token: 0x06006FD5 RID: 28629 RVA: 0x002A2C08 File Offset: 0x002A0E08
	public void SetFiles(string[] files, int loadFrame = -1)
	{
		if (files == null)
		{
			return;
		}
		this.files = files;
		bool flag = files.Length != 0 && files[0] != null;
		this.resetSlide(flag);
		if (flag)
		{
			int num = (loadFrame != -1) ? loadFrame : (files.Length - 1);
			string file = files[num];
			Sprite slide = this.loadSlide(file);
			this.setSlide(slide);
			this.currentSlideImage = slide;
		}
	}

	// Token: 0x06006FD6 RID: 28630 RVA: 0x002A2C60 File Offset: 0x002A0E60
	public void updateSize(Sprite sprite)
	{
		Vector2 fittedSize = this.GetFittedSize(sprite, 960f, 960f);
		base.GetComponent<RectTransform>().sizeDelta = fittedSize;
	}

	// Token: 0x06006FD7 RID: 28631 RVA: 0x002A2C8B File Offset: 0x002A0E8B
	public void SetSprites(Sprite[] sprites)
	{
		if (sprites == null)
		{
			return;
		}
		this.sprites = sprites;
		this.resetSlide(sprites.Length != 0 && sprites[0] != null);
		if (sprites.Length != 0 && sprites[0] != null)
		{
			this.setSlide(sprites[0]);
		}
	}

	// Token: 0x06006FD8 RID: 28632 RVA: 0x002A2CC8 File Offset: 0x002A0EC8
	public Vector2 GetFittedSize(Sprite sprite, float maxWidth, float maxHeight)
	{
		if (sprite == null || sprite.texture == null)
		{
			return Vector2.zero;
		}
		int width = sprite.texture.width;
		int height = sprite.texture.height;
		float num = maxWidth / (float)width;
		float num2 = maxHeight / (float)height;
		if (num < num2)
		{
			return new Vector2((float)width * num, (float)height * num);
		}
		return new Vector2((float)width * num2, (float)height * num2);
	}

	// Token: 0x06006FD9 RID: 28633 RVA: 0x002A2D33 File Offset: 0x002A0F33
	public void setSlide(Sprite slide)
	{
		if (slide == null)
		{
			return;
		}
		this.imageTarget.texture = slide.texture;
		this.updateSize(slide);
	}

	// Token: 0x06006FDA RID: 28634 RVA: 0x002A2D57 File Offset: 0x002A0F57
	public void nextSlide()
	{
		this.setSlideIndex(this.currentSlide + 1);
	}

	// Token: 0x06006FDB RID: 28635 RVA: 0x002A2D67 File Offset: 0x002A0F67
	public void prevSlide()
	{
		this.setSlideIndex(this.currentSlide - 1);
	}

	// Token: 0x06006FDC RID: 28636 RVA: 0x002A2D78 File Offset: 0x002A0F78
	private void setSlideIndex(int slideIndex)
	{
		this.timeUntilNextSlide = this.timePerSlide;
		SlideshowUpdateType slideshowUpdateType = this.updateType;
		if (slideshowUpdateType != SlideshowUpdateType.preloadedSprites)
		{
			if (slideshowUpdateType != SlideshowUpdateType.loadOnDemand)
			{
				return;
			}
			if (slideIndex < 0)
			{
				slideIndex = this.files.Length + slideIndex;
			}
			this.currentSlide = slideIndex % this.files.Length;
			if (this.currentSlide == this.files.Length - 1)
			{
				this.timeUntilNextSlide *= this.timeFactorForLastSlide;
			}
			if (this.playInThumbnail)
			{
				if (this.currentSlideImage != null)
				{
					UnityEngine.Object.Destroy(this.currentSlideImage.texture);
					UnityEngine.Object.Destroy(this.currentSlideImage);
					GC.Collect();
				}
				this.currentSlideImage = this.loadSlide(this.files[this.currentSlide]);
				this.setSlide(this.currentSlideImage);
			}
		}
		else
		{
			if (slideIndex < 0)
			{
				slideIndex = this.sprites.Length + slideIndex;
			}
			this.currentSlide = slideIndex % this.sprites.Length;
			if (this.currentSlide == this.sprites.Length - 1)
			{
				this.timeUntilNextSlide *= this.timeFactorForLastSlide;
			}
			if (this.playInThumbnail)
			{
				this.setSlide(this.sprites[this.currentSlide]);
				return;
			}
		}
	}

	// Token: 0x06006FDD RID: 28637 RVA: 0x002A2EA4 File Offset: 0x002A10A4
	private void Update()
	{
		if (this.updateType == SlideshowUpdateType.preloadedSprites && (this.sprites == null || this.sprites.Length == 0))
		{
			return;
		}
		if (this.updateType == SlideshowUpdateType.loadOnDemand && (this.files == null || this.files.Length == 0))
		{
			return;
		}
		if (this.paused)
		{
			return;
		}
		this.timeUntilNextSlide -= Time.unscaledDeltaTime;
		if (this.timeUntilNextSlide <= 0f)
		{
			this.nextSlide();
		}
	}

	// Token: 0x04004C98 RID: 19608
	public RawImage imageTarget;

	// Token: 0x04004C99 RID: 19609
	private string[] files;

	// Token: 0x04004C9A RID: 19610
	private Sprite currentSlideImage;

	// Token: 0x04004C9B RID: 19611
	private Sprite[] sprites;

	// Token: 0x04004C9C RID: 19612
	public float timePerSlide = 1f;

	// Token: 0x04004C9D RID: 19613
	public float timeFactorForLastSlide = 3f;

	// Token: 0x04004C9E RID: 19614
	private int currentSlide;

	// Token: 0x04004C9F RID: 19615
	private float timeUntilNextSlide;

	// Token: 0x04004CA0 RID: 19616
	private bool paused;

	// Token: 0x04004CA1 RID: 19617
	public bool playInThumbnail;

	// Token: 0x04004CA2 RID: 19618
	public SlideshowUpdateType updateType;

	// Token: 0x04004CA3 RID: 19619
	[SerializeField]
	private bool isExpandable;

	// Token: 0x04004CA4 RID: 19620
	[SerializeField]
	private KButton button;

	// Token: 0x04004CA5 RID: 19621
	[SerializeField]
	private bool transparentIfEmpty = true;

	// Token: 0x04004CA6 RID: 19622
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04004CA7 RID: 19623
	[SerializeField]
	private KButton prevButton;

	// Token: 0x04004CA8 RID: 19624
	[SerializeField]
	private KButton nextButton;

	// Token: 0x04004CA9 RID: 19625
	[SerializeField]
	private KButton pauseButton;

	// Token: 0x04004CAA RID: 19626
	[SerializeField]
	private Image pauseIcon;

	// Token: 0x04004CAB RID: 19627
	[SerializeField]
	private Image unpauseIcon;

	// Token: 0x04004CAC RID: 19628
	public Slideshow.onBeforeAndEndPlayDelegate onBeforePlay;

	// Token: 0x04004CAD RID: 19629
	public Slideshow.onBeforeAndEndPlayDelegate onEndingPlay;

	// Token: 0x02001ED7 RID: 7895
	// (Invoke) Token: 0x0600ACC1 RID: 44225
	public delegate void onBeforeAndEndPlayDelegate();
}
