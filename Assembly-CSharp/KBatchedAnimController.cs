using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020004E5 RID: 1253
[DebuggerDisplay("{name} visible={isVisible} suspendUpdates={suspendUpdates} moving={moving}")]
public class KBatchedAnimController : KAnimControllerBase, KAnimConverter.IAnimConverter
{
	// Token: 0x06001B8B RID: 7051 RVA: 0x00090312 File Offset: 0x0008E512
	public int GetCurrentFrameIndex()
	{
		return this.curAnimFrameIdx;
	}

	// Token: 0x06001B8C RID: 7052 RVA: 0x0009031A File Offset: 0x0008E51A
	public KBatchedAnimInstanceData GetBatchInstanceData()
	{
		return this.batchInstanceData;
	}

	// Token: 0x170000CF RID: 207
	// (get) Token: 0x06001B8D RID: 7053 RVA: 0x00090322 File Offset: 0x0008E522
	// (set) Token: 0x06001B8E RID: 7054 RVA: 0x0009032A File Offset: 0x0008E52A
	protected bool forceRebuild
	{
		get
		{
			return this._forceRebuild;
		}
		set
		{
			this._forceRebuild = value;
		}
	}

	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x06001B8F RID: 7055 RVA: 0x00090333 File Offset: 0x0008E533
	public bool IsMoving
	{
		get
		{
			return this.moving;
		}
	}

	// Token: 0x06001B90 RID: 7056 RVA: 0x0009033C File Offset: 0x0008E53C
	public KBatchedAnimController()
	{
		this.batchInstanceData = new KBatchedAnimInstanceData(this);
	}

	// Token: 0x06001B91 RID: 7057 RVA: 0x000903BA File Offset: 0x0008E5BA
	public bool IsActive()
	{
		return base.isActiveAndEnabled && this._enabled;
	}

	// Token: 0x06001B92 RID: 7058 RVA: 0x000903CC File Offset: 0x0008E5CC
	public bool IsVisible()
	{
		return this.isVisible;
	}

	// Token: 0x06001B93 RID: 7059 RVA: 0x000903D4 File Offset: 0x0008E5D4
	public Vector4 GetPositionData()
	{
		if (this.getPositionDataFunctionInUse != null)
		{
			return this.getPositionDataFunctionInUse();
		}
		Vector3 position = base.transform.GetPosition();
		Vector3 positionIncludingOffset = base.PositionIncludingOffset;
		return new Vector4(position.x, position.y, positionIncludingOffset.x, positionIncludingOffset.y);
	}

	// Token: 0x06001B94 RID: 7060 RVA: 0x00090428 File Offset: 0x0008E628
	public void SetSymbolScale(KAnimHashedString symbol_name, float scale)
	{
		KAnim.Build.Symbol symbol = KAnimBatchManager.Instance().GetBatchGroupData(this.GetBatchGroupID(false)).GetSymbol(symbol_name);
		if (symbol == null)
		{
			return;
		}
		base.symbolInstanceGpuData.SetSymbolScale(symbol.symbolIndexInSourceBuild, scale);
		this.SuspendUpdates(false);
		this.SetDirty();
	}

	// Token: 0x06001B95 RID: 7061 RVA: 0x00090470 File Offset: 0x0008E670
	public void SetSymbolTint(KAnimHashedString symbol_name, Color color)
	{
		KAnim.Build.Symbol symbol = KAnimBatchManager.Instance().GetBatchGroupData(this.GetBatchGroupID(false)).GetSymbol(symbol_name);
		if (symbol == null)
		{
			return;
		}
		base.symbolInstanceGpuData.SetSymbolTint(symbol.symbolIndexInSourceBuild, color);
		this.SuspendUpdates(false);
		this.SetDirty();
	}

	// Token: 0x06001B96 RID: 7062 RVA: 0x000904B8 File Offset: 0x0008E6B8
	public Vector2I GetCellXY()
	{
		Vector3 positionIncludingOffset = base.PositionIncludingOffset;
		if (Grid.CellSizeInMeters == 0f)
		{
			return new Vector2I((int)positionIncludingOffset.x, (int)positionIncludingOffset.y);
		}
		return Grid.PosToXY(positionIncludingOffset);
	}

	// Token: 0x06001B97 RID: 7063 RVA: 0x000904F2 File Offset: 0x0008E6F2
	public float GetZ()
	{
		return base.transform.GetPosition().z;
	}

	// Token: 0x06001B98 RID: 7064 RVA: 0x00090504 File Offset: 0x0008E704
	public string GetName()
	{
		return base.name;
	}

	// Token: 0x06001B99 RID: 7065 RVA: 0x0009050C File Offset: 0x0008E70C
	public override KAnim.Anim GetAnim(int index)
	{
		if (!this.batchGroupID.IsValid || !(this.batchGroupID != KAnimBatchManager.NO_BATCH))
		{
			global::Debug.LogError(base.name + " batch not ready");
		}
		KBatchGroupData batchGroupData = KAnimBatchManager.Instance().GetBatchGroupData(this.batchGroupID);
		global::Debug.Assert(batchGroupData != null);
		return batchGroupData.GetAnim(index);
	}

	// Token: 0x06001B9A RID: 7066 RVA: 0x00090570 File Offset: 0x0008E770
	private void Initialize()
	{
		if (this.batchGroupID.IsValid && this.batchGroupID != KAnimBatchManager.NO_BATCH)
		{
			this.DeRegister();
			this.Register();
		}
	}

	// Token: 0x06001B9B RID: 7067 RVA: 0x000905AB File Offset: 0x0008E7AB
	private void OnMovementStateChanged(bool is_moving)
	{
		if (is_moving == this.moving)
		{
			return;
		}
		this.moving = is_moving;
		this.SetDirty();
		this.ConfigureUpdateListener();
	}

	// Token: 0x06001B9C RID: 7068 RVA: 0x000905CA File Offset: 0x0008E7CA
	private static void OnMovementStateChanged(Transform transform, bool is_moving)
	{
		transform.GetComponent<KBatchedAnimController>().OnMovementStateChanged(is_moving);
	}

	// Token: 0x06001B9D RID: 7069 RVA: 0x000905D8 File Offset: 0x0008E7D8
	private void SetBatchGroup(KAnimFileData kafd)
	{
		if (this.batchGroupID.IsValid && kafd != null && this.batchGroupID == kafd.batchTag)
		{
			return;
		}
		DebugUtil.Assert(!this.batchGroupID.IsValid, "Should only be setting the batch group once.");
		DebugUtil.Assert(kafd != null, "Null anim data!! For", base.name);
		base.curBuild = kafd.build;
		DebugUtil.Assert(base.curBuild != null, "Null build for anim!! ", base.name, kafd.name);
		KAnimGroupFile.Group group = KAnimGroupFile.GetGroup(base.curBuild.batchTag);
		HashedString batchGroupID = kafd.build.batchTag;
		if (group.renderType == KAnimBatchGroup.RendererType.DontRender || group.renderType == KAnimBatchGroup.RendererType.AnimOnly)
		{
			bool isValid = group.swapTarget.IsValid;
			string str = "Invalid swap target fro group [";
			HashedString id = group.id;
			global::Debug.Assert(isValid, str + id.ToString() + "]");
			batchGroupID = group.swapTarget;
		}
		this.batchGroupID = batchGroupID;
		base.symbolInstanceGpuData = new SymbolInstanceGpuData(KAnimBatchManager.instance.GetBatchGroupData(this.batchGroupID).maxSymbolsPerBuild);
		base.symbolOverrideInfoGpuData = new SymbolOverrideInfoGpuData(KAnimBatchManager.instance.GetBatchGroupData(this.batchGroupID).symbolFrameInstances.Count);
		if (!this.batchGroupID.IsValid || this.batchGroupID == KAnimBatchManager.NO_BATCH)
		{
			global::Debug.LogError("Batch is not ready: " + base.name);
		}
		if (this.materialType == KAnimBatchGroup.MaterialType.Default && this.batchGroupID == KAnimBatchManager.BATCH_HUMAN)
		{
			this.materialType = KAnimBatchGroup.MaterialType.Human;
		}
	}

	// Token: 0x06001B9E RID: 7070 RVA: 0x00090774 File Offset: 0x0008E974
	public void LoadAnims()
	{
		if (!KAnimBatchManager.Instance().isReady)
		{
			global::Debug.LogError("KAnimBatchManager is not ready when loading anim:" + base.name);
		}
		if (this.animFiles.Length == 0)
		{
			DebugUtil.Assert(false, "KBatchedAnimController has no anim files:" + base.name);
		}
		if (!this.animFiles[0].IsBuildLoaded)
		{
			DebugUtil.LogErrorArgs(base.gameObject, new object[]
			{
				string.Format("First anim file needs to be the build file but {0} doesn't have an associated build", this.animFiles[0].GetData().name)
			});
		}
		this.overrideAnims.Clear();
		this.anims.Clear();
		this.SetBatchGroup(this.animFiles[0].GetData());
		for (int i = 0; i < this.animFiles.Length; i++)
		{
			base.AddAnims(this.animFiles[i]);
		}
		this.forceRebuild = true;
		if (this.layering != null)
		{
			this.layering.HideSymbols();
		}
		if (this.usingNewSymbolOverrideSystem)
		{
			DebugUtil.Assert(base.GetComponent<SymbolOverrideController>() != null);
		}
	}

	// Token: 0x06001B9F RID: 7071 RVA: 0x00090880 File Offset: 0x0008EA80
	public void SwapAnims(KAnimFile[] anims)
	{
		if (this.batchGroupID.IsValid)
		{
			this.DeRegister();
			this.batchGroupID = HashedString.Invalid;
		}
		base.AnimFiles = anims;
		this.LoadAnims();
		if (base.curBuild != null)
		{
			this.UpdateHiddenSymbolSet(this.hiddenSymbolsSet);
		}
		this.Register();
	}

	// Token: 0x06001BA0 RID: 7072 RVA: 0x000908D8 File Offset: 0x0008EAD8
	public void UpdateAnim(float dt)
	{
		if (this.batch != null && base.transform.hasChanged)
		{
			base.transform.hasChanged = false;
			if (this.batch != null && this.batch.group.maxGroupSize == 1 && this.lastPos.z != base.transform.GetPosition().z)
			{
				this.batch.OverrideZ(base.transform.GetPosition().z);
			}
			Vector3 positionIncludingOffset = base.PositionIncludingOffset;
			this.lastPos = positionIncludingOffset;
			if (this.visibilityType != KAnimControllerBase.VisibilityType.Always && KAnimBatchManager.ControllerToChunkXY(this) != this.lastChunkXY && this.lastChunkXY != KBatchedAnimUpdater.INVALID_CHUNK_ID)
			{
				this.DeRegister();
				this.Register();
			}
			this.SetDirty();
		}
		if (this.batchGroupID == KAnimBatchManager.NO_BATCH || !this.IsActive())
		{
			return;
		}
		if (!this.forceRebuild && (this.mode == KAnim.PlayMode.Paused || this.stopped || this.curAnim == null || (this.mode == KAnim.PlayMode.Once && this.curAnim != null && (this.elapsedTime > this.curAnim.totalTime || this.curAnim.totalTime <= 0f) && this.animQueue.Count == 0)))
		{
			this.SuspendUpdates(true);
		}
		if (!this.isVisible && !this.forceRebuild)
		{
			if (this.visibilityType == KAnimControllerBase.VisibilityType.OffscreenUpdate && !this.stopped && this.mode != KAnim.PlayMode.Paused)
			{
				base.SetElapsedTime(this.elapsedTime + dt * this.playSpeed);
			}
			return;
		}
		this.curAnimFrameIdx = base.GetFrameIdx(this.elapsedTime, true);
		if (this.eventManagerHandle.IsValid() && this.aem != null)
		{
			float elapsedTime = this.aem.GetElapsedTime(this.eventManagerHandle);
			if ((int)((this.elapsedTime - elapsedTime) * 100f) != 0)
			{
				base.UpdateAnimEventSequenceTime();
			}
		}
		this.UpdateFrame(this.elapsedTime);
		if (!this.stopped && this.mode != KAnim.PlayMode.Paused)
		{
			base.SetElapsedTime(this.elapsedTime + dt * this.playSpeed);
		}
		this.forceRebuild = false;
	}

	// Token: 0x06001BA1 RID: 7073 RVA: 0x00090B00 File Offset: 0x0008ED00
	protected override void UpdateFrame(float t)
	{
		base.previousFrame = base.currentFrame;
		if (!this.stopped || this.forceRebuild)
		{
			if (this.curAnim != null && (this.mode == KAnim.PlayMode.Loop || this.elapsedTime <= base.GetDuration() || this.forceRebuild))
			{
				base.currentFrame = this.curAnim.GetFrameIdx(this.mode, this.elapsedTime);
				if (base.currentFrame != base.previousFrame || this.forceRebuild)
				{
					this.SetDirty();
				}
			}
			else
			{
				this.TriggerStop();
			}
			if (!this.stopped && this.mode == KAnim.PlayMode.Loop && base.currentFrame == 0)
			{
				base.AnimEnter(this.curAnim.hash);
			}
		}
		if (this.synchronizer != null)
		{
			this.synchronizer.SyncTime();
		}
	}

	// Token: 0x06001BA2 RID: 7074 RVA: 0x00090BD0 File Offset: 0x0008EDD0
	public override void TriggerStop()
	{
		if (this.animQueue.Count > 0)
		{
			base.StartQueuedAnim();
			return;
		}
		if (this.curAnim != null && this.mode == KAnim.PlayMode.Once)
		{
			base.currentFrame = this.curAnim.numFrames - 1;
			base.Stop();
			base.gameObject.Trigger(-1061186183, null);
			if (this.destroyOnAnimComplete)
			{
				base.DestroySelf();
			}
		}
	}

	// Token: 0x06001BA3 RID: 7075 RVA: 0x00090C3C File Offset: 0x0008EE3C
	public override void UpdateHiddenSymbol(KAnimHashedString symbolToUpdate)
	{
		KBatchGroupData batchGroupData = KAnimBatchManager.instance.GetBatchGroupData(this.batchGroupID);
		for (int i = 0; i < batchGroupData.frameElementSymbols.Count; i++)
		{
			if (!(symbolToUpdate != batchGroupData.frameElementSymbols[i].hash))
			{
				KAnim.Build.Symbol symbol = batchGroupData.frameElementSymbols[i];
				bool is_visible = !this.hiddenSymbolsSet.Contains(symbol.hash);
				base.symbolInstanceGpuData.SetVisible(i, is_visible);
			}
		}
		this.SetDirty();
	}

	// Token: 0x06001BA4 RID: 7076 RVA: 0x00090CC0 File Offset: 0x0008EEC0
	public override void UpdateHiddenSymbolSet(HashSet<KAnimHashedString> symbolsToUpdate)
	{
		KBatchGroupData batchGroupData = KAnimBatchManager.instance.GetBatchGroupData(this.batchGroupID);
		for (int i = 0; i < batchGroupData.frameElementSymbols.Count; i++)
		{
			if (symbolsToUpdate.Contains(batchGroupData.frameElementSymbols[i].hash))
			{
				KAnim.Build.Symbol symbol = batchGroupData.frameElementSymbols[i];
				bool is_visible = !this.hiddenSymbolsSet.Contains(symbol.hash);
				base.symbolInstanceGpuData.SetVisible(i, is_visible);
			}
		}
		this.SetDirty();
	}

	// Token: 0x06001BA5 RID: 7077 RVA: 0x00090D44 File Offset: 0x0008EF44
	public override void UpdateAllHiddenSymbols()
	{
		KBatchGroupData batchGroupData = KAnimBatchManager.instance.GetBatchGroupData(this.batchGroupID);
		for (int i = 0; i < batchGroupData.frameElementSymbols.Count; i++)
		{
			KAnim.Build.Symbol symbol = batchGroupData.frameElementSymbols[i];
			bool is_visible = !this.hiddenSymbolsSet.Contains(symbol.hash);
			base.symbolInstanceGpuData.SetVisible(i, is_visible);
		}
		this.SetDirty();
	}

	// Token: 0x06001BA6 RID: 7078 RVA: 0x00090DAD File Offset: 0x0008EFAD
	public int GetMaxVisible()
	{
		return this.maxSymbols;
	}

	// Token: 0x170000D1 RID: 209
	// (get) Token: 0x06001BA7 RID: 7079 RVA: 0x00090DB5 File Offset: 0x0008EFB5
	// (set) Token: 0x06001BA8 RID: 7080 RVA: 0x00090DBD File Offset: 0x0008EFBD
	public HashedString batchGroupID { get; private set; }

	// Token: 0x170000D2 RID: 210
	// (get) Token: 0x06001BA9 RID: 7081 RVA: 0x00090DC6 File Offset: 0x0008EFC6
	// (set) Token: 0x06001BAA RID: 7082 RVA: 0x00090DCE File Offset: 0x0008EFCE
	public HashedString batchGroupIDOverride { get; private set; }

	// Token: 0x06001BAB RID: 7083 RVA: 0x00090DD8 File Offset: 0x0008EFD8
	public HashedString GetBatchGroupID(bool isEditorWindow = false)
	{
		global::Debug.Assert(isEditorWindow || this.animFiles == null || this.animFiles.Length == 0 || (this.batchGroupID.IsValid && this.batchGroupID != KAnimBatchManager.NO_BATCH));
		return this.batchGroupID;
	}

	// Token: 0x06001BAC RID: 7084 RVA: 0x00090E2D File Offset: 0x0008F02D
	public HashedString GetBatchGroupIDOverride()
	{
		return this.batchGroupIDOverride;
	}

	// Token: 0x06001BAD RID: 7085 RVA: 0x00090E35 File Offset: 0x0008F035
	public void SetBatchGroupOverride(HashedString id)
	{
		this.batchGroupIDOverride = id;
		this.DeRegister();
		this.Register();
	}

	// Token: 0x06001BAE RID: 7086 RVA: 0x00090E4A File Offset: 0x0008F04A
	public int GetLayer()
	{
		return base.gameObject.layer;
	}

	// Token: 0x06001BAF RID: 7087 RVA: 0x00090E57 File Offset: 0x0008F057
	public KAnimBatch GetBatch()
	{
		return this.batch;
	}

	// Token: 0x06001BB0 RID: 7088 RVA: 0x00090E60 File Offset: 0x0008F060
	public void SetBatch(KAnimBatch new_batch)
	{
		this.batch = new_batch;
		if (this.materialType == KAnimBatchGroup.MaterialType.UI)
		{
			KBatchedAnimCanvasRenderer kbatchedAnimCanvasRenderer = base.GetComponent<KBatchedAnimCanvasRenderer>();
			if (kbatchedAnimCanvasRenderer == null && new_batch != null)
			{
				kbatchedAnimCanvasRenderer = base.gameObject.AddComponent<KBatchedAnimCanvasRenderer>();
			}
			if (kbatchedAnimCanvasRenderer != null)
			{
				kbatchedAnimCanvasRenderer.SetBatch(this);
			}
		}
	}

	// Token: 0x06001BB1 RID: 7089 RVA: 0x00090EAC File Offset: 0x0008F0AC
	public int GetCurrentNumFrames()
	{
		if (this.curAnim == null)
		{
			return 0;
		}
		return this.curAnim.numFrames;
	}

	// Token: 0x06001BB2 RID: 7090 RVA: 0x00090EC3 File Offset: 0x0008F0C3
	public int GetFirstFrameIndex()
	{
		if (this.curAnim == null)
		{
			return -1;
		}
		return this.curAnim.firstFrameIdx;
	}

	// Token: 0x06001BB3 RID: 7091 RVA: 0x00090EDC File Offset: 0x0008F0DC
	private Canvas GetRootCanvas()
	{
		if (this.rt == null)
		{
			return null;
		}
		RectTransform component = this.rt.parent.GetComponent<RectTransform>();
		while (component != null)
		{
			Canvas component2 = component.GetComponent<Canvas>();
			if (component2 != null && component2.isRootCanvas)
			{
				return component2;
			}
			component = component.parent.GetComponent<RectTransform>();
		}
		return null;
	}

	// Token: 0x06001BB4 RID: 7092 RVA: 0x00090F40 File Offset: 0x0008F140
	public override Matrix2x3 GetTransformMatrix()
	{
		Vector3 v = base.PositionIncludingOffset;
		v.z = 0f;
		Vector2 scale = new Vector2(this.animScale * this.animWidth, -this.animScale * this.animHeight);
		if (this.materialType == KAnimBatchGroup.MaterialType.UI)
		{
			this.rt = base.GetComponent<RectTransform>();
			if (this.rootCanvas == null)
			{
				this.rootCanvas = this.GetRootCanvas();
			}
			if (this.scaler == null && this.rootCanvas != null)
			{
				this.scaler = this.rootCanvas.GetComponent<CanvasScaler>();
			}
			if (this.rootCanvas == null)
			{
				this.screenOffset.x = (float)(Screen.width / 2);
				this.screenOffset.y = (float)(Screen.height / 2);
			}
			else
			{
				this.screenOffset.x = ((this.rootCanvas.renderMode == RenderMode.WorldSpace) ? 0f : (this.rootCanvas.rectTransform().rect.width / 2f));
				this.screenOffset.y = ((this.rootCanvas.renderMode == RenderMode.WorldSpace) ? 0f : (this.rootCanvas.rectTransform().rect.height / 2f));
			}
			float num = 1f;
			if (this.scaler != null)
			{
				num = 1f / this.scaler.scaleFactor;
			}
			v = (this.rt.localToWorldMatrix.MultiplyPoint(this.rt.pivot) + this.offset) * num - this.screenOffset;
			float num2 = this.animWidth * this.animScale;
			float num3 = this.animHeight * this.animScale;
			if (this.setScaleFromAnim && this.curAnim != null)
			{
				num2 *= this.rt.rect.size.x / this.curAnim.unScaledSize.x;
				num3 *= this.rt.rect.size.y / this.curAnim.unScaledSize.y;
			}
			else
			{
				num2 *= this.rt.rect.size.x / this.animOverrideSize.x;
				num3 *= this.rt.rect.size.y / this.animOverrideSize.y;
			}
			scale = new Vector3(this.rt.lossyScale.x * num2 * num, -this.rt.lossyScale.y * num3 * num, this.rt.lossyScale.z * num);
			this.pivot = this.rt.pivot;
		}
		Matrix2x3 n = Matrix2x3.Scale(scale);
		Matrix2x3 n2 = Matrix2x3.Scale(new Vector2(this.flipX ? -1f : 1f, this.flipY ? -1f : 1f));
		Matrix2x3 result;
		if (this.rotation != 0f)
		{
			Matrix2x3 n3 = Matrix2x3.Translate(-this.pivot);
			Matrix2x3 n4 = Matrix2x3.Rotate(this.rotation * 0.017453292f);
			Matrix2x3 n5 = Matrix2x3.Translate(this.pivot) * n4 * n3;
			result = Matrix2x3.TRS(v, base.transform.rotation, base.transform.localScale) * n5 * n * this.navMatrix * n2;
		}
		else
		{
			result = Matrix2x3.TRS(v, base.transform.rotation, base.transform.localScale) * n * this.navMatrix * n2;
		}
		return result;
	}

	// Token: 0x06001BB5 RID: 7093 RVA: 0x00091360 File Offset: 0x0008F560
	public Matrix2x3 GetTransformMatrix(Vector2 customScale)
	{
		Vector3 v = base.PositionIncludingOffset;
		v.z = 0f;
		Vector2 scale = customScale;
		if (this.materialType == KAnimBatchGroup.MaterialType.UI)
		{
			this.rt = base.GetComponent<RectTransform>();
			if (this.rootCanvas == null)
			{
				this.rootCanvas = this.GetRootCanvas();
			}
			if (this.scaler == null && this.rootCanvas != null)
			{
				this.scaler = this.rootCanvas.GetComponent<CanvasScaler>();
			}
			if (this.rootCanvas == null)
			{
				this.screenOffset.x = (float)(Screen.width / 2);
				this.screenOffset.y = (float)(Screen.height / 2);
			}
			else
			{
				this.screenOffset.x = ((this.rootCanvas.renderMode == RenderMode.WorldSpace) ? 0f : (this.rootCanvas.rectTransform().rect.width / 2f));
				this.screenOffset.y = ((this.rootCanvas.renderMode == RenderMode.WorldSpace) ? 0f : (this.rootCanvas.rectTransform().rect.height / 2f));
			}
			float num = 1f;
			if (this.scaler != null)
			{
				num = 1f / this.scaler.scaleFactor;
			}
			v = (this.rt.localToWorldMatrix.MultiplyPoint(this.rt.pivot) + this.offset) * num - this.screenOffset;
			float num2 = this.animWidth * this.animScale;
			float num3 = this.animHeight * this.animScale;
			if (this.setScaleFromAnim && this.curAnim != null)
			{
				num2 *= this.rt.rect.size.x / this.curAnim.unScaledSize.x;
				num3 *= this.rt.rect.size.y / this.curAnim.unScaledSize.y;
			}
			else
			{
				num2 *= this.rt.rect.size.x / this.animOverrideSize.x;
				num3 *= this.rt.rect.size.y / this.animOverrideSize.y;
			}
			scale = new Vector3(this.rt.lossyScale.x * num2 * num, -this.rt.lossyScale.y * num3 * num, this.rt.lossyScale.z * num);
			this.pivot = this.rt.pivot;
		}
		Matrix2x3 n = Matrix2x3.Scale(scale);
		Matrix2x3 n2 = Matrix2x3.Scale(new Vector2(this.flipX ? -1f : 1f, this.flipY ? -1f : 1f));
		Matrix2x3 result;
		if (this.rotation != 0f)
		{
			Matrix2x3 n3 = Matrix2x3.Translate(-this.pivot);
			Matrix2x3 n4 = Matrix2x3.Rotate(this.rotation * 0.017453292f);
			Matrix2x3 n5 = Matrix2x3.Translate(this.pivot) * n4 * n3;
			result = Matrix2x3.TRS(v, base.transform.rotation, base.transform.localScale) * n5 * n * this.navMatrix * n2;
		}
		else
		{
			result = Matrix2x3.TRS(v, base.transform.rotation, base.transform.localScale) * n * this.navMatrix * n2;
		}
		return result;
	}

	// Token: 0x06001BB6 RID: 7094 RVA: 0x00091760 File Offset: 0x0008F960
	public override Matrix4x4 GetSymbolTransform(HashedString symbol, out bool symbolVisible)
	{
		if (this.curAnimFrameIdx != -1 && this.batch != null)
		{
			Matrix2x3 symbolLocalTransform = this.GetSymbolLocalTransform(symbol, out symbolVisible);
			if (symbolVisible)
			{
				return this.GetTransformMatrix() * symbolLocalTransform;
			}
		}
		symbolVisible = false;
		return default(Matrix4x4);
	}

	// Token: 0x06001BB7 RID: 7095 RVA: 0x000917B0 File Offset: 0x0008F9B0
	public override Matrix2x3 GetSymbolLocalTransform(HashedString symbol, out bool symbolVisible)
	{
		KAnim.Anim.Frame frame;
		if (this.curAnimFrameIdx != -1 && this.batch != null && this.batch.group.data.TryGetFrame(this.curAnimFrameIdx, out frame))
		{
			for (int i = 0; i < frame.numElements; i++)
			{
				int num = frame.firstElementIdx + i;
				if (num < this.batch.group.data.frameElements.Count)
				{
					KAnim.Anim.FrameElement frameElement = this.batch.group.data.frameElements[num];
					if (frameElement.symbol == symbol)
					{
						symbolVisible = true;
						return frameElement.transform;
					}
				}
			}
		}
		symbolVisible = false;
		return Matrix2x3.identity;
	}

	// Token: 0x06001BB8 RID: 7096 RVA: 0x00091866 File Offset: 0x0008FA66
	public override void SetLayer(int layer)
	{
		if (layer == base.gameObject.layer)
		{
			return;
		}
		base.SetLayer(layer);
		this.DeRegister();
		base.gameObject.layer = layer;
		this.Register();
	}

	// Token: 0x06001BB9 RID: 7097 RVA: 0x00091896 File Offset: 0x0008FA96
	public override void SetDirty()
	{
		if (this.batch != null)
		{
			this.batch.SetDirty(this);
		}
	}

	// Token: 0x06001BBA RID: 7098 RVA: 0x000918AC File Offset: 0x0008FAAC
	protected override void OnStartQueuedAnim()
	{
		this.SuspendUpdates(false);
	}

	// Token: 0x06001BBB RID: 7099 RVA: 0x000918B8 File Offset: 0x0008FAB8
	protected override void OnAwake()
	{
		this.LoadAnims();
		if (this.visibilityType == KAnimControllerBase.VisibilityType.Default)
		{
			this.visibilityType = ((this.materialType == KAnimBatchGroup.MaterialType.UI) ? KAnimControllerBase.VisibilityType.Always : this.visibilityType);
		}
		if (this.materialType == KAnimBatchGroup.MaterialType.Default && this.batchGroupID == KAnimBatchManager.BATCH_HUMAN)
		{
			this.materialType = KAnimBatchGroup.MaterialType.Human;
		}
		this.symbolOverrideController = base.GetComponent<SymbolOverrideController>();
		this.UpdateAllHiddenSymbols();
		this.hasEnableRun = false;
	}

	// Token: 0x06001BBC RID: 7100 RVA: 0x00091928 File Offset: 0x0008FB28
	protected override void OnStart()
	{
		if (this.batch == null)
		{
			this.Initialize();
		}
		if (this.visibilityType == KAnimControllerBase.VisibilityType.Always || this.visibilityType == KAnimControllerBase.VisibilityType.OffscreenUpdate)
		{
			this.ConfigureUpdateListener();
		}
		CellChangeMonitor instance = Singleton<CellChangeMonitor>.Instance;
		if (instance != null)
		{
			instance.RegisterMovementStateChanged(base.transform, new Action<Transform, bool>(KBatchedAnimController.OnMovementStateChanged));
			this.moving = instance.IsMoving(base.transform);
		}
		this.symbolOverrideController = base.GetComponent<SymbolOverrideController>();
		this.SetDirty();
	}

	// Token: 0x06001BBD RID: 7101 RVA: 0x000919A0 File Offset: 0x0008FBA0
	protected override void OnStop()
	{
		this.SetDirty();
	}

	// Token: 0x06001BBE RID: 7102 RVA: 0x000919A8 File Offset: 0x0008FBA8
	private void OnEnable()
	{
		if (this._enabled)
		{
			this.Enable();
		}
	}

	// Token: 0x06001BBF RID: 7103 RVA: 0x000919B8 File Offset: 0x0008FBB8
	protected override void Enable()
	{
		if (this.hasEnableRun)
		{
			return;
		}
		this.hasEnableRun = true;
		if (this.batch == null)
		{
			this.Initialize();
		}
		this.SetDirty();
		this.SuspendUpdates(false);
		this.ConfigureVisibilityListener(true);
		if (!this.stopped && this.curAnim != null && this.mode != KAnim.PlayMode.Paused && !this.eventManagerHandle.IsValid())
		{
			base.StartAnimEventSequence();
		}
	}

	// Token: 0x06001BC0 RID: 7104 RVA: 0x00091A23 File Offset: 0x0008FC23
	private void OnDisable()
	{
		this.Disable();
	}

	// Token: 0x06001BC1 RID: 7105 RVA: 0x00091A2C File Offset: 0x0008FC2C
	protected override void Disable()
	{
		if (App.IsExiting || KMonoBehaviour.isLoadingScene)
		{
			return;
		}
		if (!this.hasEnableRun)
		{
			return;
		}
		this.hasEnableRun = false;
		this.SuspendUpdates(true);
		if (this.batch != null)
		{
			this.DeRegister();
		}
		this.ConfigureVisibilityListener(false);
		base.StopAnimEventSequence();
	}

	// Token: 0x06001BC2 RID: 7106 RVA: 0x00091A7C File Offset: 0x0008FC7C
	protected override void OnDestroy()
	{
		if (App.IsExiting)
		{
			return;
		}
		CellChangeMonitor instance = Singleton<CellChangeMonitor>.Instance;
		if (instance != null)
		{
			instance.UnregisterMovementStateChanged(base.transform, new Action<Transform, bool>(KBatchedAnimController.OnMovementStateChanged));
		}
		KBatchedAnimUpdater instance2 = Singleton<KBatchedAnimUpdater>.Instance;
		if (instance2 != null)
		{
			instance2.UpdateUnregister(this);
		}
		this.isVisible = false;
		this.DeRegister();
		this.stopped = true;
		base.StopAnimEventSequence();
		this.batchInstanceData = null;
		this.batch = null;
		base.OnDestroy();
	}

	// Token: 0x06001BC3 RID: 7107 RVA: 0x00091AF0 File Offset: 0x0008FCF0
	public void SetBlendValue(float value)
	{
		this.batchInstanceData.SetBlend(value);
		this.SetDirty();
	}

	// Token: 0x06001BC4 RID: 7108 RVA: 0x00091B04 File Offset: 0x0008FD04
	public SymbolOverrideController SetupSymbolOverriding()
	{
		if (!this.symbolOverrideController.IsNullOrDestroyed())
		{
			return this.symbolOverrideController;
		}
		this.usingNewSymbolOverrideSystem = true;
		this.symbolOverrideController = SymbolOverrideControllerUtil.AddToPrefab(base.gameObject);
		return this.symbolOverrideController;
	}

	// Token: 0x06001BC5 RID: 7109 RVA: 0x00091B38 File Offset: 0x0008FD38
	public bool ApplySymbolOverrides()
	{
		this.batch.atlases.Apply(this.batch.matProperties);
		if (this.symbolOverrideController != null)
		{
			if (this.symbolOverrideControllerVersion != this.symbolOverrideController.version || this.symbolOverrideController.applySymbolOverridesEveryFrame)
			{
				this.symbolOverrideControllerVersion = this.symbolOverrideController.version;
				this.symbolOverrideController.ApplyOverrides();
			}
			this.symbolOverrideController.ApplyAtlases();
			return true;
		}
		return false;
	}

	// Token: 0x06001BC6 RID: 7110 RVA: 0x00091BB8 File Offset: 0x0008FDB8
	public void SetSymbolOverrides(int symbol_start_idx, int symbol_num_frames, int atlas_idx, KBatchGroupData source_data, int source_start_idx, int source_num_frames)
	{
		base.symbolOverrideInfoGpuData.SetSymbolOverrideInfo(symbol_start_idx, symbol_num_frames, atlas_idx, source_data, source_start_idx, source_num_frames);
	}

	// Token: 0x06001BC7 RID: 7111 RVA: 0x00091BCE File Offset: 0x0008FDCE
	public void SetSymbolOverride(int symbol_idx, ref KAnim.Build.SymbolFrameInstance symbol_frame_instance)
	{
		base.symbolOverrideInfoGpuData.SetSymbolOverrideInfo(symbol_idx, ref symbol_frame_instance);
	}

	// Token: 0x06001BC8 RID: 7112 RVA: 0x00091BE0 File Offset: 0x0008FDE0
	protected override void Register()
	{
		if (!this.IsActive())
		{
			return;
		}
		if (this.batch != null)
		{
			return;
		}
		if (this.batchGroupID.IsValid && this.batchGroupID != KAnimBatchManager.NO_BATCH)
		{
			this.lastChunkXY = KAnimBatchManager.ControllerToChunkXY(this);
			KAnimBatchManager.Instance().Register(this);
			this.forceRebuild = true;
			this.SetDirty();
		}
	}

	// Token: 0x06001BC9 RID: 7113 RVA: 0x00091C45 File Offset: 0x0008FE45
	protected override void DeRegister()
	{
		if (this.batch != null)
		{
			this.batch.Deregister(this);
		}
	}

	// Token: 0x06001BCA RID: 7114 RVA: 0x00091C5C File Offset: 0x0008FE5C
	private void ConfigureUpdateListener()
	{
		if ((this.IsActive() && !this.suspendUpdates && this.isVisible) || this.moving || this.visibilityType == KAnimControllerBase.VisibilityType.OffscreenUpdate || this.visibilityType == KAnimControllerBase.VisibilityType.Always)
		{
			Singleton<KBatchedAnimUpdater>.Instance.UpdateRegister(this);
			return;
		}
		Singleton<KBatchedAnimUpdater>.Instance.UpdateUnregister(this);
	}

	// Token: 0x06001BCB RID: 7115 RVA: 0x00091CB7 File Offset: 0x0008FEB7
	protected override void SuspendUpdates(bool suspend)
	{
		this.suspendUpdates = suspend;
		this.ConfigureUpdateListener();
	}

	// Token: 0x06001BCC RID: 7116 RVA: 0x00091CC6 File Offset: 0x0008FEC6
	public void SetVisiblity(bool is_visible)
	{
		if (is_visible != this.isVisible)
		{
			this.isVisible = is_visible;
			if (is_visible)
			{
				this.SuspendUpdates(false);
				this.SetDirty();
				base.UpdateAnimEventSequenceTime();
				return;
			}
			this.SuspendUpdates(true);
			this.SetDirty();
		}
	}

	// Token: 0x06001BCD RID: 7117 RVA: 0x00091CFC File Offset: 0x0008FEFC
	private void ConfigureVisibilityListener(bool enabled)
	{
		if (this.visibilityType == KAnimControllerBase.VisibilityType.Always || this.visibilityType == KAnimControllerBase.VisibilityType.OffscreenUpdate)
		{
			return;
		}
		if (enabled)
		{
			this.RegisterVisibilityListener();
			return;
		}
		this.UnregisterVisibilityListener();
	}

	// Token: 0x06001BCE RID: 7118 RVA: 0x00091D21 File Offset: 0x0008FF21
	public virtual KAnimConverter.PostProcessingEffects GetPostProcessingEffectsCompatibility()
	{
		return this.postProcessingEffectsAllowed;
	}

	// Token: 0x06001BCF RID: 7119 RVA: 0x00091D29 File Offset: 0x0008FF29
	public float GetPostProcessingParams()
	{
		return this.postProcessingParameters;
	}

	// Token: 0x06001BD0 RID: 7120 RVA: 0x00091D31 File Offset: 0x0008FF31
	protected override void RefreshVisibilityListener()
	{
		if (!this.visibilityListenerRegistered)
		{
			return;
		}
		this.ConfigureVisibilityListener(false);
		this.ConfigureVisibilityListener(true);
	}

	// Token: 0x06001BD1 RID: 7121 RVA: 0x00091D4A File Offset: 0x0008FF4A
	private void RegisterVisibilityListener()
	{
		DebugUtil.Assert(!this.visibilityListenerRegistered);
		Singleton<KBatchedAnimUpdater>.Instance.VisibilityRegister(this);
		this.visibilityListenerRegistered = true;
	}

	// Token: 0x06001BD2 RID: 7122 RVA: 0x00091D6C File Offset: 0x0008FF6C
	private void UnregisterVisibilityListener()
	{
		DebugUtil.Assert(this.visibilityListenerRegistered);
		Singleton<KBatchedAnimUpdater>.Instance.VisibilityUnregister(this);
		this.visibilityListenerRegistered = false;
	}

	// Token: 0x06001BD3 RID: 7123 RVA: 0x00091D8C File Offset: 0x0008FF8C
	public void SetSceneLayer(Grid.SceneLayer layer)
	{
		float layerZ = Grid.GetLayerZ(layer);
		this.sceneLayer = layer;
		Vector3 position = base.transform.GetPosition();
		position.z = layerZ;
		base.transform.SetPosition(position);
		this.DeRegister();
		this.Register();
	}

	// Token: 0x04000F91 RID: 3985
	[NonSerialized]
	protected bool _forceRebuild;

	// Token: 0x04000F92 RID: 3986
	private Vector3 lastPos = Vector3.zero;

	// Token: 0x04000F93 RID: 3987
	private Vector2I lastChunkXY = KBatchedAnimUpdater.INVALID_CHUNK_ID;

	// Token: 0x04000F94 RID: 3988
	private KAnimBatch batch;

	// Token: 0x04000F95 RID: 3989
	public float animScale = 0.005f;

	// Token: 0x04000F96 RID: 3990
	private bool suspendUpdates;

	// Token: 0x04000F97 RID: 3991
	private bool visibilityListenerRegistered;

	// Token: 0x04000F98 RID: 3992
	private bool moving;

	// Token: 0x04000F99 RID: 3993
	private SymbolOverrideController symbolOverrideController;

	// Token: 0x04000F9A RID: 3994
	private int symbolOverrideControllerVersion;

	// Token: 0x04000F9B RID: 3995
	[NonSerialized]
	public KBatchedAnimUpdater.RegistrationState updateRegistrationState = KBatchedAnimUpdater.RegistrationState.Unregistered;

	// Token: 0x04000F9C RID: 3996
	public Grid.SceneLayer sceneLayer;

	// Token: 0x04000F9D RID: 3997
	private RectTransform rt;

	// Token: 0x04000F9E RID: 3998
	private Vector3 screenOffset = new Vector3(0f, 0f, 0f);

	// Token: 0x04000F9F RID: 3999
	public Matrix2x3 navMatrix = Matrix2x3.identity;

	// Token: 0x04000FA0 RID: 4000
	private CanvasScaler scaler;

	// Token: 0x04000FA1 RID: 4001
	public bool setScaleFromAnim = true;

	// Token: 0x04000FA2 RID: 4002
	public Vector2 animOverrideSize = Vector2.one;

	// Token: 0x04000FA3 RID: 4003
	private Canvas rootCanvas;

	// Token: 0x04000FA4 RID: 4004
	public bool isMovable;

	// Token: 0x04000FA5 RID: 4005
	public Func<Vector4> getPositionDataFunctionInUse;

	// Token: 0x04000FA6 RID: 4006
	public KAnimConverter.PostProcessingEffects postProcessingEffectsAllowed;

	// Token: 0x04000FA7 RID: 4007
	public float postProcessingParameters;
}
