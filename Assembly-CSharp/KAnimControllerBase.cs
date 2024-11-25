using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004DF RID: 1247
public abstract class KAnimControllerBase : MonoBehaviour, ISerializationCallbackReceiver
{
	// Token: 0x06001AE7 RID: 6887 RVA: 0x0008DCB0 File Offset: 0x0008BEB0
	protected KAnimControllerBase()
	{
		this.previousFrame = -1;
		this.currentFrame = -1;
		this.PlaySpeedMultiplier = 1f;
		this.synchronizer = new KAnimSynchronizer(this);
		this.layering = new KAnimLayering(this, this.fgLayer);
		this.isVisible = true;
	}

	// Token: 0x06001AE8 RID: 6888
	public abstract KAnim.Anim GetAnim(int index);

	// Token: 0x170000B6 RID: 182
	// (get) Token: 0x06001AE9 RID: 6889 RVA: 0x0008DDB9 File Offset: 0x0008BFB9
	// (set) Token: 0x06001AEA RID: 6890 RVA: 0x0008DDC1 File Offset: 0x0008BFC1
	public string debugName { get; private set; }

	// Token: 0x170000B7 RID: 183
	// (get) Token: 0x06001AEB RID: 6891 RVA: 0x0008DDCA File Offset: 0x0008BFCA
	// (set) Token: 0x06001AEC RID: 6892 RVA: 0x0008DDD2 File Offset: 0x0008BFD2
	public KAnim.Build curBuild { get; protected set; }

	// Token: 0x14000005 RID: 5
	// (add) Token: 0x06001AED RID: 6893 RVA: 0x0008DDDC File Offset: 0x0008BFDC
	// (remove) Token: 0x06001AEE RID: 6894 RVA: 0x0008DE14 File Offset: 0x0008C014
	public event Action<Color32> OnOverlayColourChanged;

	// Token: 0x170000B8 RID: 184
	// (get) Token: 0x06001AEF RID: 6895 RVA: 0x0008DE49 File Offset: 0x0008C049
	// (set) Token: 0x06001AF0 RID: 6896 RVA: 0x0008DE51 File Offset: 0x0008C051
	public new bool enabled
	{
		get
		{
			return this._enabled;
		}
		set
		{
			this._enabled = value;
			if (!this.hasAwakeRun)
			{
				return;
			}
			if (this._enabled)
			{
				this.Enable();
				return;
			}
			this.Disable();
		}
	}

	// Token: 0x170000B9 RID: 185
	// (get) Token: 0x06001AF1 RID: 6897 RVA: 0x0008DE78 File Offset: 0x0008C078
	public bool HasBatchInstanceData
	{
		get
		{
			return this.batchInstanceData != null;
		}
	}

	// Token: 0x170000BA RID: 186
	// (get) Token: 0x06001AF2 RID: 6898 RVA: 0x0008DE83 File Offset: 0x0008C083
	// (set) Token: 0x06001AF3 RID: 6899 RVA: 0x0008DE8B File Offset: 0x0008C08B
	public SymbolInstanceGpuData symbolInstanceGpuData { get; protected set; }

	// Token: 0x170000BB RID: 187
	// (get) Token: 0x06001AF4 RID: 6900 RVA: 0x0008DE94 File Offset: 0x0008C094
	// (set) Token: 0x06001AF5 RID: 6901 RVA: 0x0008DE9C File Offset: 0x0008C09C
	public SymbolOverrideInfoGpuData symbolOverrideInfoGpuData { get; protected set; }

	// Token: 0x170000BC RID: 188
	// (get) Token: 0x06001AF6 RID: 6902 RVA: 0x0008DEA5 File Offset: 0x0008C0A5
	// (set) Token: 0x06001AF7 RID: 6903 RVA: 0x0008DEB8 File Offset: 0x0008C0B8
	public Color32 TintColour
	{
		get
		{
			return this.batchInstanceData.GetTintColour();
		}
		set
		{
			if (this.batchInstanceData != null && this.batchInstanceData.SetTintColour(value))
			{
				this.SetDirty();
				this.SuspendUpdates(false);
				if (this.OnTintChanged != null)
				{
					this.OnTintChanged(value);
				}
			}
		}
	}

	// Token: 0x170000BD RID: 189
	// (get) Token: 0x06001AF8 RID: 6904 RVA: 0x0008DF06 File Offset: 0x0008C106
	// (set) Token: 0x06001AF9 RID: 6905 RVA: 0x0008DF18 File Offset: 0x0008C118
	public Color32 HighlightColour
	{
		get
		{
			return this.batchInstanceData.GetHighlightcolour();
		}
		set
		{
			if (this.batchInstanceData.SetHighlightColour(value))
			{
				this.SetDirty();
				this.SuspendUpdates(false);
				if (this.OnHighlightChanged != null)
				{
					this.OnHighlightChanged(value);
				}
			}
		}
	}

	// Token: 0x170000BE RID: 190
	// (get) Token: 0x06001AFA RID: 6906 RVA: 0x0008DF53 File Offset: 0x0008C153
	// (set) Token: 0x06001AFB RID: 6907 RVA: 0x0008DF60 File Offset: 0x0008C160
	public Color OverlayColour
	{
		get
		{
			return this.batchInstanceData.GetOverlayColour();
		}
		set
		{
			if (this.batchInstanceData.SetOverlayColour(value))
			{
				this.SetDirty();
				this.SuspendUpdates(false);
				if (this.OnOverlayColourChanged != null)
				{
					this.OnOverlayColourChanged(value);
				}
			}
		}
	}

	// Token: 0x14000006 RID: 6
	// (add) Token: 0x06001AFC RID: 6908 RVA: 0x0008DF98 File Offset: 0x0008C198
	// (remove) Token: 0x06001AFD RID: 6909 RVA: 0x0008DFD0 File Offset: 0x0008C1D0
	public event KAnimControllerBase.KAnimEvent onAnimEnter;

	// Token: 0x14000007 RID: 7
	// (add) Token: 0x06001AFE RID: 6910 RVA: 0x0008E008 File Offset: 0x0008C208
	// (remove) Token: 0x06001AFF RID: 6911 RVA: 0x0008E040 File Offset: 0x0008C240
	public event KAnimControllerBase.KAnimEvent onAnimComplete;

	// Token: 0x14000008 RID: 8
	// (add) Token: 0x06001B00 RID: 6912 RVA: 0x0008E078 File Offset: 0x0008C278
	// (remove) Token: 0x06001B01 RID: 6913 RVA: 0x0008E0B0 File Offset: 0x0008C2B0
	public event Action<int> onLayerChanged;

	// Token: 0x170000BF RID: 191
	// (get) Token: 0x06001B02 RID: 6914 RVA: 0x0008E0E5 File Offset: 0x0008C2E5
	// (set) Token: 0x06001B03 RID: 6915 RVA: 0x0008E0ED File Offset: 0x0008C2ED
	public int previousFrame { get; protected set; }

	// Token: 0x170000C0 RID: 192
	// (get) Token: 0x06001B04 RID: 6916 RVA: 0x0008E0F6 File Offset: 0x0008C2F6
	// (set) Token: 0x06001B05 RID: 6917 RVA: 0x0008E0FE File Offset: 0x0008C2FE
	public int currentFrame { get; protected set; }

	// Token: 0x170000C1 RID: 193
	// (get) Token: 0x06001B06 RID: 6918 RVA: 0x0008E108 File Offset: 0x0008C308
	public HashedString currentAnim
	{
		get
		{
			if (this.curAnim == null)
			{
				return default(HashedString);
			}
			return this.curAnim.hash;
		}
	}

	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x06001B08 RID: 6920 RVA: 0x0008E13B File Offset: 0x0008C33B
	// (set) Token: 0x06001B07 RID: 6919 RVA: 0x0008E132 File Offset: 0x0008C332
	public float PlaySpeedMultiplier { get; set; }

	// Token: 0x06001B09 RID: 6921 RVA: 0x0008E143 File Offset: 0x0008C343
	public void SetFGLayer(Grid.SceneLayer layer)
	{
		this.fgLayer = layer;
		this.GetLayering();
		if (this.layering != null)
		{
			this.layering.SetLayer(this.fgLayer);
		}
	}

	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x06001B0A RID: 6922 RVA: 0x0008E16C File Offset: 0x0008C36C
	// (set) Token: 0x06001B0B RID: 6923 RVA: 0x0008E174 File Offset: 0x0008C374
	public KAnim.PlayMode PlayMode
	{
		get
		{
			return this.mode;
		}
		set
		{
			this.mode = value;
		}
	}

	// Token: 0x170000C4 RID: 196
	// (get) Token: 0x06001B0C RID: 6924 RVA: 0x0008E17D File Offset: 0x0008C37D
	// (set) Token: 0x06001B0D RID: 6925 RVA: 0x0008E185 File Offset: 0x0008C385
	public bool FlipX
	{
		get
		{
			return this.flipX;
		}
		set
		{
			this.flipX = value;
			if (this.layering != null)
			{
				this.layering.Dirty();
			}
			this.SetDirty();
		}
	}

	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x06001B0E RID: 6926 RVA: 0x0008E1A7 File Offset: 0x0008C3A7
	// (set) Token: 0x06001B0F RID: 6927 RVA: 0x0008E1AF File Offset: 0x0008C3AF
	public bool FlipY
	{
		get
		{
			return this.flipY;
		}
		set
		{
			this.flipY = value;
			if (this.layering != null)
			{
				this.layering.Dirty();
			}
			this.SetDirty();
		}
	}

	// Token: 0x170000C6 RID: 198
	// (get) Token: 0x06001B10 RID: 6928 RVA: 0x0008E1D1 File Offset: 0x0008C3D1
	// (set) Token: 0x06001B11 RID: 6929 RVA: 0x0008E1D9 File Offset: 0x0008C3D9
	public Vector3 Offset
	{
		get
		{
			return this.offset;
		}
		set
		{
			this.offset = value;
			if (this.layering != null)
			{
				this.layering.Dirty();
			}
			this.DeRegister();
			this.Register();
			this.RefreshVisibilityListener();
			this.SetDirty();
		}
	}

	// Token: 0x170000C7 RID: 199
	// (get) Token: 0x06001B12 RID: 6930 RVA: 0x0008E20D File Offset: 0x0008C40D
	// (set) Token: 0x06001B13 RID: 6931 RVA: 0x0008E215 File Offset: 0x0008C415
	public float Rotation
	{
		get
		{
			return this.rotation;
		}
		set
		{
			this.rotation = value;
			if (this.layering != null)
			{
				this.layering.Dirty();
			}
			this.SetDirty();
		}
	}

	// Token: 0x170000C8 RID: 200
	// (get) Token: 0x06001B14 RID: 6932 RVA: 0x0008E237 File Offset: 0x0008C437
	// (set) Token: 0x06001B15 RID: 6933 RVA: 0x0008E23F File Offset: 0x0008C43F
	public Vector3 Pivot
	{
		get
		{
			return this.pivot;
		}
		set
		{
			this.pivot = value;
			if (this.layering != null)
			{
				this.layering.Dirty();
			}
			this.SetDirty();
		}
	}

	// Token: 0x170000C9 RID: 201
	// (get) Token: 0x06001B16 RID: 6934 RVA: 0x0008E261 File Offset: 0x0008C461
	public Vector3 PositionIncludingOffset
	{
		get
		{
			return base.transform.GetPosition() + this.Offset;
		}
	}

	// Token: 0x06001B17 RID: 6935 RVA: 0x0008E279 File Offset: 0x0008C479
	public KAnimBatchGroup.MaterialType GetMaterialType()
	{
		return this.materialType;
	}

	// Token: 0x06001B18 RID: 6936 RVA: 0x0008E284 File Offset: 0x0008C484
	public Vector3 GetWorldPivot()
	{
		Vector3 position = base.transform.GetPosition();
		KBoxCollider2D component = base.GetComponent<KBoxCollider2D>();
		if (component != null)
		{
			position.x += component.offset.x;
			position.y += component.offset.y - component.size.y / 2f;
		}
		return position;
	}

	// Token: 0x06001B19 RID: 6937 RVA: 0x0008E2EC File Offset: 0x0008C4EC
	public KAnim.Anim GetCurrentAnim()
	{
		return this.curAnim;
	}

	// Token: 0x06001B1A RID: 6938 RVA: 0x0008E2F4 File Offset: 0x0008C4F4
	public KAnimHashedString GetBuildHash()
	{
		if (this.curBuild == null)
		{
			return KAnimBatchManager.NO_BATCH;
		}
		return this.curBuild.fileHash;
	}

	// Token: 0x06001B1B RID: 6939 RVA: 0x0008E314 File Offset: 0x0008C514
	protected float GetDuration()
	{
		if (this.curAnim != null)
		{
			return (float)this.curAnim.numFrames / this.curAnim.frameRate;
		}
		return 0f;
	}

	// Token: 0x06001B1C RID: 6940 RVA: 0x0008E33C File Offset: 0x0008C53C
	protected int GetFrameIdxFromOffset(int offset)
	{
		int result = -1;
		if (this.curAnim != null)
		{
			result = offset + this.curAnim.firstFrameIdx;
		}
		return result;
	}

	// Token: 0x06001B1D RID: 6941 RVA: 0x0008E364 File Offset: 0x0008C564
	public int GetFrameIdx(float time, bool absolute)
	{
		int result = -1;
		if (this.curAnim != null)
		{
			result = this.curAnim.GetFrameIdx(this.mode, time) + (absolute ? this.curAnim.firstFrameIdx : 0);
		}
		return result;
	}

	// Token: 0x06001B1E RID: 6942 RVA: 0x0008E3A1 File Offset: 0x0008C5A1
	public bool IsStopped()
	{
		return this.stopped;
	}

	// Token: 0x170000CA RID: 202
	// (get) Token: 0x06001B1F RID: 6943 RVA: 0x0008E3A9 File Offset: 0x0008C5A9
	public KAnim.Anim CurrentAnim
	{
		get
		{
			return this.curAnim;
		}
	}

	// Token: 0x06001B20 RID: 6944 RVA: 0x0008E3B1 File Offset: 0x0008C5B1
	public KAnimSynchronizer GetSynchronizer()
	{
		return this.synchronizer;
	}

	// Token: 0x06001B21 RID: 6945 RVA: 0x0008E3B9 File Offset: 0x0008C5B9
	public KAnimLayering GetLayering()
	{
		if (this.layering == null && this.fgLayer != Grid.SceneLayer.NoLayer)
		{
			this.layering = new KAnimLayering(this, this.fgLayer);
		}
		return this.layering;
	}

	// Token: 0x06001B22 RID: 6946 RVA: 0x0008E3E5 File Offset: 0x0008C5E5
	public KAnim.PlayMode GetMode()
	{
		return this.mode;
	}

	// Token: 0x06001B23 RID: 6947 RVA: 0x0008E3ED File Offset: 0x0008C5ED
	public static string GetModeString(KAnim.PlayMode mode)
	{
		switch (mode)
		{
		case KAnim.PlayMode.Loop:
			return "Loop";
		case KAnim.PlayMode.Once:
			return "Once";
		case KAnim.PlayMode.Paused:
			return "Paused";
		default:
			return "Unknown";
		}
	}

	// Token: 0x06001B24 RID: 6948 RVA: 0x0008E41A File Offset: 0x0008C61A
	public float GetPlaySpeed()
	{
		return this.playSpeed;
	}

	// Token: 0x06001B25 RID: 6949 RVA: 0x0008E422 File Offset: 0x0008C622
	public void SetElapsedTime(float value)
	{
		this.elapsedTime = value;
	}

	// Token: 0x06001B26 RID: 6950 RVA: 0x0008E42B File Offset: 0x0008C62B
	public float GetElapsedTime()
	{
		return this.elapsedTime;
	}

	// Token: 0x06001B27 RID: 6951
	protected abstract void SuspendUpdates(bool suspend);

	// Token: 0x06001B28 RID: 6952
	protected abstract void OnStartQueuedAnim();

	// Token: 0x06001B29 RID: 6953
	public abstract void SetDirty();

	// Token: 0x06001B2A RID: 6954
	protected abstract void RefreshVisibilityListener();

	// Token: 0x06001B2B RID: 6955
	protected abstract void DeRegister();

	// Token: 0x06001B2C RID: 6956
	protected abstract void Register();

	// Token: 0x06001B2D RID: 6957
	protected abstract void OnAwake();

	// Token: 0x06001B2E RID: 6958
	protected abstract void OnStart();

	// Token: 0x06001B2F RID: 6959
	protected abstract void OnStop();

	// Token: 0x06001B30 RID: 6960
	protected abstract void Enable();

	// Token: 0x06001B31 RID: 6961
	protected abstract void Disable();

	// Token: 0x06001B32 RID: 6962
	protected abstract void UpdateFrame(float t);

	// Token: 0x06001B33 RID: 6963
	public abstract Matrix2x3 GetTransformMatrix();

	// Token: 0x06001B34 RID: 6964
	public abstract Matrix2x3 GetSymbolLocalTransform(HashedString symbol, out bool symbolVisible);

	// Token: 0x06001B35 RID: 6965
	public abstract void UpdateAllHiddenSymbols();

	// Token: 0x06001B36 RID: 6966
	public abstract void UpdateHiddenSymbol(KAnimHashedString specificSymbol);

	// Token: 0x06001B37 RID: 6967
	public abstract void UpdateHiddenSymbolSet(HashSet<KAnimHashedString> specificSymbols);

	// Token: 0x06001B38 RID: 6968
	public abstract void TriggerStop();

	// Token: 0x06001B39 RID: 6969 RVA: 0x0008E433 File Offset: 0x0008C633
	public virtual void SetLayer(int layer)
	{
		if (this.onLayerChanged != null)
		{
			this.onLayerChanged(layer);
		}
	}

	// Token: 0x06001B3A RID: 6970 RVA: 0x0008E44C File Offset: 0x0008C64C
	public Vector3 GetPivotSymbolPosition()
	{
		bool flag = false;
		Matrix4x4 symbolTransform = this.GetSymbolTransform(KAnimControllerBase.snaptoPivot, out flag);
		Vector3 position = base.transform.GetPosition();
		if (flag)
		{
			position = new Vector3(symbolTransform[0, 3], symbolTransform[1, 3], symbolTransform[2, 3]);
		}
		return position;
	}

	// Token: 0x06001B3B RID: 6971 RVA: 0x0008E49B File Offset: 0x0008C69B
	public virtual Matrix4x4 GetSymbolTransform(HashedString symbol, out bool symbolVisible)
	{
		symbolVisible = false;
		return Matrix4x4.identity;
	}

	// Token: 0x06001B3C RID: 6972 RVA: 0x0008E4A8 File Offset: 0x0008C6A8
	private void Awake()
	{
		this.aem = Singleton<AnimEventManager>.Instance;
		this.debugName = base.name;
		this.SetFGLayer(this.fgLayer);
		this.OnAwake();
		if (!string.IsNullOrEmpty(this.initialAnim))
		{
			this.SetDirty();
			this.Play(this.initialAnim, this.initialMode, 1f, 0f);
		}
		this.hasAwakeRun = true;
	}

	// Token: 0x06001B3D RID: 6973 RVA: 0x0008E519 File Offset: 0x0008C719
	private void Start()
	{
		this.OnStart();
	}

	// Token: 0x06001B3E RID: 6974 RVA: 0x0008E524 File Offset: 0x0008C724
	protected virtual void OnDestroy()
	{
		this.animFiles = null;
		this.curAnim = null;
		this.curBuild = null;
		this.synchronizer = null;
		this.layering = null;
		this.animQueue = null;
		this.overrideAnims = null;
		this.anims = null;
		this.synchronizer = null;
		this.layering = null;
		this.overrideAnimFiles = null;
	}

	// Token: 0x06001B3F RID: 6975 RVA: 0x0008E57E File Offset: 0x0008C77E
	protected void AnimEnter(HashedString hashed_name)
	{
		if (this.onAnimEnter != null)
		{
			this.onAnimEnter(hashed_name);
		}
	}

	// Token: 0x06001B40 RID: 6976 RVA: 0x0008E594 File Offset: 0x0008C794
	public void Play(HashedString anim_name, KAnim.PlayMode mode = KAnim.PlayMode.Once, float speed = 1f, float time_offset = 0f)
	{
		if (!this.stopped)
		{
			this.Stop();
		}
		this.Queue(anim_name, mode, speed, time_offset);
	}

	// Token: 0x06001B41 RID: 6977 RVA: 0x0008E5B0 File Offset: 0x0008C7B0
	public void Play(HashedString[] anim_names, KAnim.PlayMode mode = KAnim.PlayMode.Once)
	{
		if (!this.stopped)
		{
			this.Stop();
		}
		for (int i = 0; i < anim_names.Length - 1; i++)
		{
			this.Queue(anim_names[i], KAnim.PlayMode.Once, 1f, 0f);
		}
		global::Debug.Assert(anim_names.Length != 0, "Play was called with an empty anim array");
		this.Queue(anim_names[anim_names.Length - 1], mode, 1f, 0f);
	}

	// Token: 0x06001B42 RID: 6978 RVA: 0x0008E620 File Offset: 0x0008C820
	public void Queue(HashedString anim_name, KAnim.PlayMode mode = KAnim.PlayMode.Once, float speed = 1f, float time_offset = 0f)
	{
		this.animQueue.Enqueue(new KAnimControllerBase.AnimData
		{
			anim = anim_name,
			mode = mode,
			speed = speed,
			timeOffset = time_offset
		});
		this.mode = ((mode == KAnim.PlayMode.Paused) ? KAnim.PlayMode.Paused : KAnim.PlayMode.Once);
		if (this.aem != null)
		{
			this.aem.SetMode(this.eventManagerHandle, this.mode);
		}
		if (this.animQueue.Count == 1 && this.stopped)
		{
			this.StartQueuedAnim();
		}
	}

	// Token: 0x06001B43 RID: 6979 RVA: 0x0008E6AB File Offset: 0x0008C8AB
	public void QueueAndSyncTransition(HashedString anim_name, KAnim.PlayMode mode = KAnim.PlayMode.Once, float speed = 1f, float time_offset = 0f)
	{
		this.SyncTransition();
		this.Queue(anim_name, mode, speed, time_offset);
	}

	// Token: 0x06001B44 RID: 6980 RVA: 0x0008E6BE File Offset: 0x0008C8BE
	public void SyncTransition()
	{
		this.elapsedTime %= Mathf.Max(float.Epsilon, this.GetDuration());
	}

	// Token: 0x06001B45 RID: 6981 RVA: 0x0008E6DD File Offset: 0x0008C8DD
	public void ClearQueue()
	{
		this.animQueue.Clear();
	}

	// Token: 0x06001B46 RID: 6982 RVA: 0x0008E6EC File Offset: 0x0008C8EC
	private void Restart(HashedString anim_name, KAnim.PlayMode mode = KAnim.PlayMode.Once, float speed = 1f, float time_offset = 0f)
	{
		if (this.curBuild == null)
		{
			string[] array = new string[5];
			array[0] = "[";
			array[1] = base.gameObject.name;
			array[2] = "] Missing build while trying to play anim [";
			int num = 3;
			HashedString hashedString = anim_name;
			array[num] = hashedString.ToString();
			array[4] = "]";
			global::Debug.LogWarning(string.Concat(array), base.gameObject);
			return;
		}
		Queue<KAnimControllerBase.AnimData> queue = new Queue<KAnimControllerBase.AnimData>();
		queue.Enqueue(new KAnimControllerBase.AnimData
		{
			anim = anim_name,
			mode = mode,
			speed = speed,
			timeOffset = time_offset
		});
		while (this.animQueue.Count > 0)
		{
			queue.Enqueue(this.animQueue.Dequeue());
		}
		this.animQueue = queue;
		if (this.animQueue.Count == 1 && this.stopped)
		{
			this.StartQueuedAnim();
		}
	}

	// Token: 0x06001B47 RID: 6983 RVA: 0x0008E7CC File Offset: 0x0008C9CC
	protected void StartQueuedAnim()
	{
		this.StopAnimEventSequence();
		this.previousFrame = -1;
		this.currentFrame = -1;
		this.SuspendUpdates(false);
		this.stopped = false;
		this.OnStartQueuedAnim();
		KAnimControllerBase.AnimData animData = this.animQueue.Dequeue();
		while (animData.mode == KAnim.PlayMode.Loop && this.animQueue.Count > 0)
		{
			animData = this.animQueue.Dequeue();
		}
		KAnimControllerBase.AnimLookupData animLookupData;
		if (this.overrideAnims == null || !this.overrideAnims.TryGetValue(animData.anim, out animLookupData))
		{
			if (!this.anims.TryGetValue(animData.anim, out animLookupData))
			{
				bool flag = true;
				if (this.showWhenMissing != null)
				{
					this.showWhenMissing.SetActive(true);
				}
				if (flag)
				{
					this.TriggerStop();
					return;
				}
			}
			else if (this.showWhenMissing != null)
			{
				this.showWhenMissing.SetActive(false);
			}
		}
		this.curAnim = this.GetAnim(animLookupData.animIndex);
		int num = 0;
		if (animData.mode == KAnim.PlayMode.Loop && this.randomiseLoopedOffset)
		{
			num = UnityEngine.Random.Range(0, this.curAnim.numFrames - 1);
		}
		this.prevAnimFrame = -1;
		this.curAnimFrameIdx = this.GetFrameIdxFromOffset(num);
		this.currentFrame = this.curAnimFrameIdx;
		this.mode = animData.mode;
		this.playSpeed = animData.speed * this.PlaySpeedMultiplier;
		this.SetElapsedTime((float)num / this.curAnim.frameRate + animData.timeOffset);
		this.synchronizer.Sync();
		this.StartAnimEventSequence();
		this.AnimEnter(animData.anim);
	}

	// Token: 0x06001B48 RID: 6984 RVA: 0x0008E950 File Offset: 0x0008CB50
	public bool GetSymbolVisiblity(KAnimHashedString symbol)
	{
		return !this.hiddenSymbolsSet.Contains(symbol);
	}

	// Token: 0x06001B49 RID: 6985 RVA: 0x0008E961 File Offset: 0x0008CB61
	public void SetSymbolVisiblity(KAnimHashedString symbol, bool is_visible)
	{
		if (is_visible)
		{
			this.hiddenSymbolsSet.Remove(symbol);
		}
		else if (!this.hiddenSymbolsSet.Contains(symbol))
		{
			this.hiddenSymbolsSet.Add(symbol);
		}
		if (this.curBuild != null)
		{
			this.UpdateHiddenSymbol(symbol);
		}
	}

	// Token: 0x06001B4A RID: 6986 RVA: 0x0008E9A0 File Offset: 0x0008CBA0
	public void BatchSetSymbolsVisiblity(HashSet<KAnimHashedString> symbols, bool is_visible)
	{
		foreach (KAnimHashedString item in symbols)
		{
			if (is_visible)
			{
				this.hiddenSymbolsSet.Remove(item);
			}
			else if (!this.hiddenSymbolsSet.Contains(item))
			{
				this.hiddenSymbolsSet.Add(item);
			}
		}
		if (this.curBuild != null)
		{
			this.UpdateHiddenSymbolSet(symbols);
		}
	}

	// Token: 0x06001B4B RID: 6987 RVA: 0x0008EA24 File Offset: 0x0008CC24
	public void AddAnimOverrides(KAnimFile kanim_file, float priority = 0f)
	{
		if (kanim_file == null)
		{
			global::Debug.LogError(string.Format("AddAnimOverrides tried to add a null override to {0} at position {1}", base.gameObject.name, base.transform.position));
		}
		if (kanim_file.GetData().build != null && kanim_file.GetData().build.symbols.Length != 0)
		{
			SymbolOverrideController component = base.GetComponent<SymbolOverrideController>();
			DebugUtil.Assert(component != null, "Anim overrides containing additional symbols require a symbol override controller.");
			component.AddBuildOverride(kanim_file.GetData(), 0);
		}
		this.overrideAnimFiles.Add(new KAnimControllerBase.OverrideAnimFileData
		{
			priority = priority,
			file = kanim_file
		});
		this.overrideAnimFiles.Sort((KAnimControllerBase.OverrideAnimFileData a, KAnimControllerBase.OverrideAnimFileData b) => b.priority.CompareTo(a.priority));
		this.RebuildOverrides(kanim_file);
	}

	// Token: 0x06001B4C RID: 6988 RVA: 0x0008EAFC File Offset: 0x0008CCFC
	public void RemoveAnimOverrides(KAnimFile kanim_file)
	{
		if (kanim_file == null)
		{
			global::Debug.LogError(string.Format("RemoveAnimOverrides tried to add a null override to {0} at position {1}", base.gameObject.name, base.transform.position));
		}
		if (kanim_file.GetData().build != null && kanim_file.GetData().build.symbols.Length != 0)
		{
			SymbolOverrideController component = base.GetComponent<SymbolOverrideController>();
			DebugUtil.Assert(component != null, "Anim overrides containing additional symbols require a symbol override controller.");
			component.TryRemoveBuildOverride(kanim_file.GetData(), 0);
		}
		for (int i = 0; i < this.overrideAnimFiles.Count; i++)
		{
			if (this.overrideAnimFiles[i].file == kanim_file)
			{
				this.overrideAnimFiles.RemoveAt(i);
				break;
			}
		}
		this.RebuildOverrides(kanim_file);
	}

	// Token: 0x06001B4D RID: 6989 RVA: 0x0008EBC4 File Offset: 0x0008CDC4
	private void RebuildOverrides(KAnimFile kanim_file)
	{
		bool flag = false;
		this.overrideAnims.Clear();
		for (int i = 0; i < this.overrideAnimFiles.Count; i++)
		{
			KAnimControllerBase.OverrideAnimFileData overrideAnimFileData = this.overrideAnimFiles[i];
			KAnimFileData data = overrideAnimFileData.file.GetData();
			for (int j = 0; j < data.animCount; j++)
			{
				KAnim.Anim anim = data.GetAnim(j);
				if (anim.animFile.hashName != data.hashName)
				{
					global::Debug.LogError(string.Format("How did we get an anim from another file? [{0}] != [{1}] for anim [{2}]", data.name, anim.animFile.name, j));
				}
				KAnimControllerBase.AnimLookupData value = default(KAnimControllerBase.AnimLookupData);
				value.animIndex = anim.index;
				HashedString hashedString = new HashedString(anim.name);
				if (!this.overrideAnims.ContainsKey(hashedString))
				{
					this.overrideAnims[hashedString] = value;
				}
				if (this.curAnim != null && this.curAnim.hash == hashedString && overrideAnimFileData.file == kanim_file)
				{
					flag = true;
				}
			}
		}
		if (flag)
		{
			this.Restart(this.curAnim.name, this.mode, this.playSpeed, 0f);
		}
	}

	// Token: 0x06001B4E RID: 6990 RVA: 0x0008ED14 File Offset: 0x0008CF14
	public bool HasAnimation(HashedString anim_name)
	{
		bool flag = anim_name.IsValid;
		if (flag)
		{
			bool flag2 = this.anims.ContainsKey(anim_name);
			bool flag3 = !flag2 && this.overrideAnims.ContainsKey(anim_name);
			flag = (flag2 || flag3);
		}
		return flag;
	}

	// Token: 0x06001B4F RID: 6991 RVA: 0x0008ED50 File Offset: 0x0008CF50
	public bool HasAnimationFile(KAnimHashedString anim_file_name)
	{
		KAnimFile kanimFile = null;
		return this.TryGetAnimationFile(anim_file_name, out kanimFile);
	}

	// Token: 0x06001B50 RID: 6992 RVA: 0x0008ED68 File Offset: 0x0008CF68
	public bool TryGetAnimationFile(KAnimHashedString anim_file_name, out KAnimFile match)
	{
		match = null;
		if (!anim_file_name.IsValid())
		{
			return false;
		}
		KAnimFileData kanimFileData = null;
		int num = 0;
		int num2 = this.overrideAnimFiles.Count - 1;
		int num3 = (int)((float)this.overrideAnimFiles.Count * 0.5f);
		while (num3 > 0 && match == null && num < num3)
		{
			if (this.overrideAnimFiles[num].file != null)
			{
				kanimFileData = this.overrideAnimFiles[num].file.GetData();
			}
			if (kanimFileData != null && kanimFileData.hashName.HashValue == anim_file_name.HashValue)
			{
				match = this.overrideAnimFiles[num].file;
				break;
			}
			if (this.overrideAnimFiles[num2].file != null)
			{
				kanimFileData = this.overrideAnimFiles[num2].file.GetData();
			}
			if (kanimFileData != null && kanimFileData.hashName.HashValue == anim_file_name.HashValue)
			{
				match = this.overrideAnimFiles[num2].file;
			}
			num++;
			num2--;
		}
		if (match == null && this.overrideAnimFiles.Count % 2 != 0)
		{
			if (this.overrideAnimFiles[num].file != null)
			{
				kanimFileData = this.overrideAnimFiles[num].file.GetData();
			}
			if (kanimFileData != null && kanimFileData.hashName.HashValue == anim_file_name.HashValue)
			{
				match = this.overrideAnimFiles[num].file;
			}
		}
		kanimFileData = null;
		if (match == null && this.animFiles != null)
		{
			num = 0;
			num2 = this.animFiles.Length - 1;
			num3 = (int)((float)this.animFiles.Length * 0.5f);
			while (num3 > 0 && match == null && num < num3)
			{
				if (this.animFiles[num] != null)
				{
					kanimFileData = this.animFiles[num].GetData();
				}
				if (kanimFileData != null && kanimFileData.hashName.HashValue == anim_file_name.HashValue)
				{
					match = this.animFiles[num];
					break;
				}
				if (this.animFiles[num2] != null)
				{
					kanimFileData = this.animFiles[num2].GetData();
				}
				if (kanimFileData != null && kanimFileData.hashName.HashValue == anim_file_name.HashValue)
				{
					match = this.animFiles[num2];
				}
				num++;
				num2--;
			}
			if (match == null && this.animFiles.Length % 2 != 0)
			{
				if (this.animFiles[num] != null)
				{
					kanimFileData = this.animFiles[num].GetData();
				}
				if (kanimFileData != null && kanimFileData.hashName.HashValue == anim_file_name.HashValue)
				{
					match = this.animFiles[num];
				}
			}
		}
		return match != null;
	}

	// Token: 0x06001B51 RID: 6993 RVA: 0x0008F044 File Offset: 0x0008D244
	public void AddAnims(KAnimFile anim_file)
	{
		KAnimFileData data = anim_file.GetData();
		if (data == null)
		{
			global::Debug.LogError("AddAnims() Null animfile data");
			return;
		}
		this.maxSymbols = Mathf.Max(this.maxSymbols, data.maxVisSymbolFrames);
		for (int i = 0; i < data.animCount; i++)
		{
			KAnim.Anim anim = data.GetAnim(i);
			if (anim.animFile.hashName != data.hashName)
			{
				global::Debug.LogErrorFormat("How did we get an anim from another file? [{0}] != [{1}] for anim [{2}]", new object[]
				{
					data.name,
					anim.animFile.name,
					i
				});
			}
			this.anims[anim.hash] = new KAnimControllerBase.AnimLookupData
			{
				animIndex = anim.index
			};
		}
		if (this.usingNewSymbolOverrideSystem && data.buildIndex != -1 && data.build.symbols != null && data.build.symbols.Length != 0)
		{
			base.GetComponent<SymbolOverrideController>().AddBuildOverride(anim_file.GetData(), -1);
		}
	}

	// Token: 0x170000CB RID: 203
	// (get) Token: 0x06001B52 RID: 6994 RVA: 0x0008F146 File Offset: 0x0008D346
	// (set) Token: 0x06001B53 RID: 6995 RVA: 0x0008F150 File Offset: 0x0008D350
	public KAnimFile[] AnimFiles
	{
		get
		{
			return this.animFiles;
		}
		set
		{
			DebugUtil.AssertArgs(value.Length != 0, new object[]
			{
				"Controller has no anim files.",
				base.gameObject
			});
			DebugUtil.AssertArgs(value[0] != null, new object[]
			{
				"First anim file needs to be non-null.",
				base.gameObject
			});
			DebugUtil.AssertArgs(value[0].IsBuildLoaded, new object[]
			{
				"First anim file needs to be the build file.",
				base.gameObject
			});
			for (int i = 0; i < value.Length; i++)
			{
				DebugUtil.AssertArgs(value[i] != null, new object[]
				{
					"Anim file is null",
					base.gameObject
				});
			}
			this.animFiles = new KAnimFile[value.Length];
			for (int j = 0; j < value.Length; j++)
			{
				this.animFiles[j] = value[j];
			}
		}
	}

	// Token: 0x170000CC RID: 204
	// (get) Token: 0x06001B54 RID: 6996 RVA: 0x0008F221 File Offset: 0x0008D421
	public IReadOnlyList<KAnimControllerBase.OverrideAnimFileData> OverrideAnimFiles
	{
		get
		{
			return this.overrideAnimFiles;
		}
	}

	// Token: 0x06001B55 RID: 6997 RVA: 0x0008F22C File Offset: 0x0008D42C
	public void Stop()
	{
		if (this.curAnim != null)
		{
			this.StopAnimEventSequence();
		}
		this.animQueue.Clear();
		this.stopped = true;
		if (this.onAnimComplete != null)
		{
			this.onAnimComplete((this.curAnim == null) ? HashedString.Invalid : this.curAnim.hash);
		}
		this.OnStop();
	}

	// Token: 0x06001B56 RID: 6998 RVA: 0x0008F28C File Offset: 0x0008D48C
	public void StopAndClear()
	{
		if (!this.stopped)
		{
			this.Stop();
		}
		this.bounds.center = Vector3.zero;
		this.bounds.extents = Vector3.zero;
		if (this.OnUpdateBounds != null)
		{
			this.OnUpdateBounds(this.bounds);
		}
	}

	// Token: 0x06001B57 RID: 6999 RVA: 0x0008F2E0 File Offset: 0x0008D4E0
	public float GetPositionPercent()
	{
		return this.GetElapsedTime() / this.GetDuration();
	}

	// Token: 0x06001B58 RID: 7000 RVA: 0x0008F2F0 File Offset: 0x0008D4F0
	public void SetPositionPercent(float percent)
	{
		if (this.curAnim == null)
		{
			return;
		}
		this.SetElapsedTime(percent * (float)this.curAnim.numFrames / this.curAnim.frameRate);
		int frameIdx = this.curAnim.GetFrameIdx(this.mode, this.elapsedTime);
		if (this.currentFrame != frameIdx)
		{
			this.SetDirty();
			this.UpdateAnimEventSequenceTime();
			this.SuspendUpdates(false);
		}
	}

	// Token: 0x06001B59 RID: 7001 RVA: 0x0008F35C File Offset: 0x0008D55C
	protected void StartAnimEventSequence()
	{
		if (!this.layering.GetIsForeground() && this.aem != null)
		{
			this.eventManagerHandle = this.aem.PlayAnim(this, this.curAnim, this.mode, this.elapsedTime, this.visibilityType == KAnimControllerBase.VisibilityType.Always);
		}
	}

	// Token: 0x06001B5A RID: 7002 RVA: 0x0008F3AB File Offset: 0x0008D5AB
	protected void UpdateAnimEventSequenceTime()
	{
		if (this.eventManagerHandle.IsValid() && this.aem != null)
		{
			this.aem.SetElapsedTime(this.eventManagerHandle, this.elapsedTime);
		}
	}

	// Token: 0x06001B5B RID: 7003 RVA: 0x0008F3DC File Offset: 0x0008D5DC
	protected void StopAnimEventSequence()
	{
		if (this.eventManagerHandle.IsValid() && this.aem != null)
		{
			if (!this.stopped && this.mode != KAnim.PlayMode.Paused)
			{
				this.SetElapsedTime(this.aem.GetElapsedTime(this.eventManagerHandle));
			}
			this.aem.StopAnim(this.eventManagerHandle);
			this.eventManagerHandle = HandleVector<int>.InvalidHandle;
		}
	}

	// Token: 0x06001B5C RID: 7004 RVA: 0x0008F442 File Offset: 0x0008D642
	protected void DestroySelf()
	{
		if (this.onDestroySelf != null)
		{
			this.onDestroySelf(base.gameObject);
			return;
		}
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x06001B5D RID: 7005 RVA: 0x0008F469 File Offset: 0x0008D669
	void ISerializationCallbackReceiver.OnBeforeSerialize()
	{
		this.hiddenSymbols.Clear();
		this.hiddenSymbols = new List<KAnimHashedString>(this.hiddenSymbolsSet);
	}

	// Token: 0x06001B5E RID: 7006 RVA: 0x0008F487 File Offset: 0x0008D687
	void ISerializationCallbackReceiver.OnAfterDeserialize()
	{
		this.hiddenSymbolsSet = new HashSet<KAnimHashedString>(this.hiddenSymbols);
		this.hiddenSymbols.Clear();
	}

	// Token: 0x04000F3E RID: 3902
	[NonSerialized]
	public GameObject showWhenMissing;

	// Token: 0x04000F3F RID: 3903
	[SerializeField]
	public KAnimBatchGroup.MaterialType materialType;

	// Token: 0x04000F40 RID: 3904
	[SerializeField]
	public string initialAnim;

	// Token: 0x04000F41 RID: 3905
	[SerializeField]
	public KAnim.PlayMode initialMode = KAnim.PlayMode.Once;

	// Token: 0x04000F42 RID: 3906
	[SerializeField]
	protected KAnimFile[] animFiles = new KAnimFile[0];

	// Token: 0x04000F43 RID: 3907
	[SerializeField]
	protected Vector3 offset;

	// Token: 0x04000F44 RID: 3908
	[SerializeField]
	protected Vector3 pivot;

	// Token: 0x04000F45 RID: 3909
	[SerializeField]
	protected float rotation;

	// Token: 0x04000F46 RID: 3910
	[SerializeField]
	public bool destroyOnAnimComplete;

	// Token: 0x04000F47 RID: 3911
	[SerializeField]
	public bool inactiveDisable;

	// Token: 0x04000F48 RID: 3912
	[SerializeField]
	protected bool flipX;

	// Token: 0x04000F49 RID: 3913
	[SerializeField]
	protected bool flipY;

	// Token: 0x04000F4A RID: 3914
	[SerializeField]
	public bool forceUseGameTime;

	// Token: 0x04000F4B RID: 3915
	public string defaultAnim;

	// Token: 0x04000F4D RID: 3917
	protected KAnim.Anim curAnim;

	// Token: 0x04000F4E RID: 3918
	protected int curAnimFrameIdx = -1;

	// Token: 0x04000F4F RID: 3919
	protected int prevAnimFrame = -1;

	// Token: 0x04000F50 RID: 3920
	public bool usingNewSymbolOverrideSystem;

	// Token: 0x04000F52 RID: 3922
	protected HandleVector<int>.Handle eventManagerHandle = HandleVector<int>.InvalidHandle;

	// Token: 0x04000F53 RID: 3923
	protected List<KAnimControllerBase.OverrideAnimFileData> overrideAnimFiles = new List<KAnimControllerBase.OverrideAnimFileData>();

	// Token: 0x04000F54 RID: 3924
	protected DeepProfiler DeepProfiler = new DeepProfiler(false);

	// Token: 0x04000F55 RID: 3925
	public bool randomiseLoopedOffset;

	// Token: 0x04000F56 RID: 3926
	protected float elapsedTime;

	// Token: 0x04000F57 RID: 3927
	protected float playSpeed = 1f;

	// Token: 0x04000F58 RID: 3928
	protected KAnim.PlayMode mode = KAnim.PlayMode.Once;

	// Token: 0x04000F59 RID: 3929
	protected bool stopped = true;

	// Token: 0x04000F5A RID: 3930
	public float animHeight = 1f;

	// Token: 0x04000F5B RID: 3931
	public float animWidth = 1f;

	// Token: 0x04000F5C RID: 3932
	protected bool isVisible;

	// Token: 0x04000F5D RID: 3933
	protected Bounds bounds;

	// Token: 0x04000F5E RID: 3934
	public Action<Bounds> OnUpdateBounds;

	// Token: 0x04000F5F RID: 3935
	public Action<Color> OnTintChanged;

	// Token: 0x04000F60 RID: 3936
	public Action<Color> OnHighlightChanged;

	// Token: 0x04000F62 RID: 3938
	protected KAnimSynchronizer synchronizer;

	// Token: 0x04000F63 RID: 3939
	protected KAnimLayering layering;

	// Token: 0x04000F64 RID: 3940
	[SerializeField]
	protected bool _enabled = true;

	// Token: 0x04000F65 RID: 3941
	protected bool hasEnableRun;

	// Token: 0x04000F66 RID: 3942
	protected bool hasAwakeRun;

	// Token: 0x04000F67 RID: 3943
	protected KBatchedAnimInstanceData batchInstanceData;

	// Token: 0x04000F6A RID: 3946
	public KAnimControllerBase.VisibilityType visibilityType;

	// Token: 0x04000F6E RID: 3950
	public Action<GameObject> onDestroySelf;

	// Token: 0x04000F71 RID: 3953
	[SerializeField]
	protected List<KAnimHashedString> hiddenSymbols = new List<KAnimHashedString>();

	// Token: 0x04000F72 RID: 3954
	[SerializeField]
	protected HashSet<KAnimHashedString> hiddenSymbolsSet = new HashSet<KAnimHashedString>();

	// Token: 0x04000F73 RID: 3955
	protected Dictionary<HashedString, KAnimControllerBase.AnimLookupData> anims = new Dictionary<HashedString, KAnimControllerBase.AnimLookupData>();

	// Token: 0x04000F74 RID: 3956
	protected Dictionary<HashedString, KAnimControllerBase.AnimLookupData> overrideAnims = new Dictionary<HashedString, KAnimControllerBase.AnimLookupData>();

	// Token: 0x04000F75 RID: 3957
	protected Queue<KAnimControllerBase.AnimData> animQueue = new Queue<KAnimControllerBase.AnimData>();

	// Token: 0x04000F76 RID: 3958
	protected int maxSymbols;

	// Token: 0x04000F78 RID: 3960
	public Grid.SceneLayer fgLayer = Grid.SceneLayer.NoLayer;

	// Token: 0x04000F79 RID: 3961
	protected AnimEventManager aem;

	// Token: 0x04000F7A RID: 3962
	private static HashedString snaptoPivot = new HashedString("snapTo_pivot");

	// Token: 0x020012B4 RID: 4788
	public struct OverrideAnimFileData
	{
		// Token: 0x04006418 RID: 25624
		public float priority;

		// Token: 0x04006419 RID: 25625
		public KAnimFile file;
	}

	// Token: 0x020012B5 RID: 4789
	public struct AnimLookupData
	{
		// Token: 0x0400641A RID: 25626
		public int animIndex;
	}

	// Token: 0x020012B6 RID: 4790
	public struct AnimData
	{
		// Token: 0x0400641B RID: 25627
		public HashedString anim;

		// Token: 0x0400641C RID: 25628
		public KAnim.PlayMode mode;

		// Token: 0x0400641D RID: 25629
		public float speed;

		// Token: 0x0400641E RID: 25630
		public float timeOffset;
	}

	// Token: 0x020012B7 RID: 4791
	public enum VisibilityType
	{
		// Token: 0x04006420 RID: 25632
		Default,
		// Token: 0x04006421 RID: 25633
		OffscreenUpdate,
		// Token: 0x04006422 RID: 25634
		Always
	}

	// Token: 0x020012B8 RID: 4792
	// (Invoke) Token: 0x060084C5 RID: 33989
	public delegate void KAnimEvent(HashedString name);
}
