using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

// Token: 0x02000DED RID: 3565
[AddComponentMenu("KMonoBehaviour/scripts/VideoWidget")]
public class VideoWidget : KMonoBehaviour
{
	// Token: 0x06007134 RID: 28980 RVA: 0x002AD441 File Offset: 0x002AB641
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.button.onClick += this.Clicked;
		this.rawImage = this.thumbnailPlayer.GetComponent<RawImage>();
	}

	// Token: 0x06007135 RID: 28981 RVA: 0x002AD474 File Offset: 0x002AB674
	private void Clicked()
	{
		VideoScreen.Instance.PlayVideo(this.clip, false, default(EventReference), false);
		if (!string.IsNullOrEmpty(this.overlayName))
		{
			VideoScreen.Instance.SetOverlayText(this.overlayName, this.texts);
		}
	}

	// Token: 0x06007136 RID: 28982 RVA: 0x002AD4C0 File Offset: 0x002AB6C0
	public void SetClip(VideoClip clip, string overlayName = null, List<string> texts = null)
	{
		if (clip == null)
		{
			global::Debug.LogWarning("Tried to assign null video clip to VideoWidget");
			return;
		}
		this.clip = clip;
		this.overlayName = overlayName;
		this.texts = texts;
		this.renderTexture = new RenderTexture(Convert.ToInt32(clip.width), Convert.ToInt32(clip.height), 16);
		this.thumbnailPlayer.targetTexture = this.renderTexture;
		this.rawImage.texture = this.renderTexture;
		base.StartCoroutine(this.ConfigureThumbnail());
	}

	// Token: 0x06007137 RID: 28983 RVA: 0x002AD548 File Offset: 0x002AB748
	private IEnumerator ConfigureThumbnail()
	{
		this.thumbnailPlayer.audioOutputMode = VideoAudioOutputMode.None;
		this.thumbnailPlayer.clip = this.clip;
		this.thumbnailPlayer.time = 0.0;
		this.thumbnailPlayer.Play();
		yield return null;
		yield break;
	}

	// Token: 0x06007138 RID: 28984 RVA: 0x002AD557 File Offset: 0x002AB757
	private void Update()
	{
		if (this.thumbnailPlayer.isPlaying && this.thumbnailPlayer.time > 2.0)
		{
			this.thumbnailPlayer.Pause();
		}
	}

	// Token: 0x04004DDA RID: 19930
	[SerializeField]
	private VideoClip clip;

	// Token: 0x04004DDB RID: 19931
	[SerializeField]
	private VideoPlayer thumbnailPlayer;

	// Token: 0x04004DDC RID: 19932
	[SerializeField]
	private KButton button;

	// Token: 0x04004DDD RID: 19933
	[SerializeField]
	private string overlayName;

	// Token: 0x04004DDE RID: 19934
	[SerializeField]
	private List<string> texts;

	// Token: 0x04004DDF RID: 19935
	private RenderTexture renderTexture;

	// Token: 0x04004DE0 RID: 19936
	private RawImage rawImage;
}
