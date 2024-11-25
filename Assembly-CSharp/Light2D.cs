using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000A35 RID: 2613
[AddComponentMenu("KMonoBehaviour/scripts/Light2D")]
public class Light2D : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06004BA6 RID: 19366 RVA: 0x001AF864 File Offset: 0x001ADA64
	private T MaybeDirty<T>(T old_value, T new_value, ref bool dirty)
	{
		if (!EqualityComparer<T>.Default.Equals(old_value, new_value))
		{
			dirty = true;
			return new_value;
		}
		return old_value;
	}

	// Token: 0x1700055C RID: 1372
	// (get) Token: 0x06004BA7 RID: 19367 RVA: 0x001AF87A File Offset: 0x001ADA7A
	// (set) Token: 0x06004BA8 RID: 19368 RVA: 0x001AF887 File Offset: 0x001ADA87
	public global::LightShape shape
	{
		get
		{
			return this.pending_emitter_state.shape;
		}
		set
		{
			this.pending_emitter_state.shape = this.MaybeDirty<global::LightShape>(this.pending_emitter_state.shape, value, ref this.dirty_shape);
		}
	}

	// Token: 0x1700055D RID: 1373
	// (get) Token: 0x06004BA9 RID: 19369 RVA: 0x001AF8AC File Offset: 0x001ADAAC
	// (set) Token: 0x06004BAA RID: 19370 RVA: 0x001AF8B4 File Offset: 0x001ADAB4
	public LightGridManager.LightGridEmitter emitter { get; private set; }

	// Token: 0x1700055E RID: 1374
	// (get) Token: 0x06004BAB RID: 19371 RVA: 0x001AF8BD File Offset: 0x001ADABD
	// (set) Token: 0x06004BAC RID: 19372 RVA: 0x001AF8CA File Offset: 0x001ADACA
	public Color Color
	{
		get
		{
			return this.pending_emitter_state.colour;
		}
		set
		{
			this.pending_emitter_state.colour = value;
		}
	}

	// Token: 0x1700055F RID: 1375
	// (get) Token: 0x06004BAD RID: 19373 RVA: 0x001AF8D8 File Offset: 0x001ADAD8
	// (set) Token: 0x06004BAE RID: 19374 RVA: 0x001AF8E5 File Offset: 0x001ADAE5
	public int Lux
	{
		get
		{
			return this.pending_emitter_state.intensity;
		}
		set
		{
			this.pending_emitter_state.intensity = value;
		}
	}

	// Token: 0x17000560 RID: 1376
	// (get) Token: 0x06004BAF RID: 19375 RVA: 0x001AF8F3 File Offset: 0x001ADAF3
	// (set) Token: 0x06004BB0 RID: 19376 RVA: 0x001AF900 File Offset: 0x001ADB00
	public DiscreteShadowCaster.Direction LightDirection
	{
		get
		{
			return this.pending_emitter_state.direction;
		}
		set
		{
			this.pending_emitter_state.direction = this.MaybeDirty<DiscreteShadowCaster.Direction>(this.pending_emitter_state.direction, value, ref this.dirty_shape);
		}
	}

	// Token: 0x17000561 RID: 1377
	// (get) Token: 0x06004BB1 RID: 19377 RVA: 0x001AF925 File Offset: 0x001ADB25
	// (set) Token: 0x06004BB2 RID: 19378 RVA: 0x001AF932 File Offset: 0x001ADB32
	public int Width
	{
		get
		{
			return this.pending_emitter_state.width;
		}
		set
		{
			this.pending_emitter_state.width = this.MaybeDirty<int>(this.pending_emitter_state.width, value, ref this.dirty_shape);
		}
	}

	// Token: 0x17000562 RID: 1378
	// (get) Token: 0x06004BB3 RID: 19379 RVA: 0x001AF957 File Offset: 0x001ADB57
	// (set) Token: 0x06004BB4 RID: 19380 RVA: 0x001AF964 File Offset: 0x001ADB64
	public float Range
	{
		get
		{
			return this.pending_emitter_state.radius;
		}
		set
		{
			this.pending_emitter_state.radius = this.MaybeDirty<float>(this.pending_emitter_state.radius, value, ref this.dirty_shape);
		}
	}

	// Token: 0x17000563 RID: 1379
	// (get) Token: 0x06004BB5 RID: 19381 RVA: 0x001AF989 File Offset: 0x001ADB89
	// (set) Token: 0x06004BB6 RID: 19382 RVA: 0x001AF996 File Offset: 0x001ADB96
	private int origin
	{
		get
		{
			return this.pending_emitter_state.origin;
		}
		set
		{
			this.pending_emitter_state.origin = this.MaybeDirty<int>(this.pending_emitter_state.origin, value, ref this.dirty_position);
		}
	}

	// Token: 0x17000564 RID: 1380
	// (get) Token: 0x06004BB7 RID: 19383 RVA: 0x001AF9BB File Offset: 0x001ADBBB
	// (set) Token: 0x06004BB8 RID: 19384 RVA: 0x001AF9C8 File Offset: 0x001ADBC8
	public float FalloffRate
	{
		get
		{
			return this.pending_emitter_state.falloffRate;
		}
		set
		{
			this.pending_emitter_state.falloffRate = this.MaybeDirty<float>(this.pending_emitter_state.falloffRate, value, ref this.dirty_falloff);
		}
	}

	// Token: 0x17000565 RID: 1381
	// (get) Token: 0x06004BB9 RID: 19385 RVA: 0x001AF9ED File Offset: 0x001ADBED
	// (set) Token: 0x06004BBA RID: 19386 RVA: 0x001AF9F5 File Offset: 0x001ADBF5
	public float IntensityAnimation { get; set; }

	// Token: 0x17000566 RID: 1382
	// (get) Token: 0x06004BBB RID: 19387 RVA: 0x001AF9FE File Offset: 0x001ADBFE
	// (set) Token: 0x06004BBC RID: 19388 RVA: 0x001AFA06 File Offset: 0x001ADC06
	public Vector2 Offset
	{
		get
		{
			return this._offset;
		}
		set
		{
			if (this._offset != value)
			{
				this._offset = value;
				this.origin = Grid.PosToCell(base.transform.GetPosition() + this._offset);
			}
		}
	}

	// Token: 0x17000567 RID: 1383
	// (get) Token: 0x06004BBD RID: 19389 RVA: 0x001AFA43 File Offset: 0x001ADC43
	private bool isRegistered
	{
		get
		{
			return this.solidPartitionerEntry != HandleVector<int>.InvalidHandle;
		}
	}

	// Token: 0x06004BBE RID: 19390 RVA: 0x001AFA58 File Offset: 0x001ADC58
	public Light2D()
	{
		this.emitter = new LightGridManager.LightGridEmitter();
		this.Range = 5f;
		this.Lux = 1000;
	}

	// Token: 0x06004BBF RID: 19391 RVA: 0x001AFAB4 File Offset: 0x001ADCB4
	protected override void OnPrefabInit()
	{
		base.Subscribe<Light2D>(-592767678, Light2D.OnOperationalChangedDelegate);
		if (this.disableOnStore)
		{
			base.Subscribe(856640610, new Action<object>(this.OnStore));
		}
		this.IntensityAnimation = 1f;
	}

	// Token: 0x06004BC0 RID: 19392 RVA: 0x001AFAF4 File Offset: 0x001ADCF4
	private void OnStore(object data)
	{
		global::Debug.Assert(this.disableOnStore, "Only Light2Ds that are disabled on storage should be subscribed to OnStore.");
		Storage storage = data as Storage;
		if (storage != null)
		{
			base.enabled = (storage.GetComponent<ItemPedestal>() != null || storage.GetComponent<MinionIdentity>() != null);
			return;
		}
		base.enabled = true;
	}

	// Token: 0x06004BC1 RID: 19393 RVA: 0x001AFB4C File Offset: 0x001ADD4C
	protected override void OnCmpEnable()
	{
		this.materialPropertyBlock = new MaterialPropertyBlock();
		base.OnCmpEnable();
		Components.Light2Ds.Add(this);
		if (base.isSpawned)
		{
			this.AddToScenePartitioner();
			this.emitter.Refresh(this.pending_emitter_state, true);
		}
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnMoved), "Light2D.OnMoved");
	}

	// Token: 0x06004BC2 RID: 19394 RVA: 0x001AFBB8 File Offset: 0x001ADDB8
	protected override void OnCmpDisable()
	{
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnMoved));
		Components.Light2Ds.Remove(this);
		base.OnCmpDisable();
		this.FullRemove();
	}

	// Token: 0x06004BC3 RID: 19395 RVA: 0x001AFBF0 File Offset: 0x001ADDF0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.origin = Grid.PosToCell(base.transform.GetPosition() + this.Offset);
		if (base.isActiveAndEnabled)
		{
			this.AddToScenePartitioner();
			this.emitter.Refresh(this.pending_emitter_state, true);
		}
	}

	// Token: 0x06004BC4 RID: 19396 RVA: 0x001AFC4A File Offset: 0x001ADE4A
	protected override void OnCleanUp()
	{
		this.FullRemove();
	}

	// Token: 0x06004BC5 RID: 19397 RVA: 0x001AFC52 File Offset: 0x001ADE52
	private void OnMoved()
	{
		if (base.isSpawned)
		{
			this.FullRefresh();
		}
	}

	// Token: 0x06004BC6 RID: 19398 RVA: 0x001AFC62 File Offset: 0x001ADE62
	private HandleVector<int>.Handle AddToLayer(Extents ext, ScenePartitionerLayer layer)
	{
		return GameScenePartitioner.Instance.Add("Light2D", base.gameObject, ext, layer, new Action<object>(this.OnWorldChanged));
	}

	// Token: 0x06004BC7 RID: 19399 RVA: 0x001AFC88 File Offset: 0x001ADE88
	private Extents ComputeExtents()
	{
		Vector2I vector2I = Grid.CellToXY(this.origin);
		int x = 0;
		int y = 0;
		int width = 0;
		int num = 0;
		global::LightShape shape = this.shape;
		if (shape > global::LightShape.Cone)
		{
			if (shape == global::LightShape.Quad)
			{
				width = this.Width;
				num = (int)this.Range;
				int num2 = (this.Width % 2 == 0) ? (this.Width / 2 - 1) : Mathf.FloorToInt((float)(this.Width - 1) * 0.5f);
				Vector2I vector2I2 = vector2I - DiscreteShadowCaster.TravelDirectionToOrtogonalDiractionVector(this.LightDirection) * num2;
				x = vector2I2.x;
				switch (this.LightDirection)
				{
				case DiscreteShadowCaster.Direction.North:
					y = vector2I2.y;
					goto IL_119;
				case DiscreteShadowCaster.Direction.South:
					y = vector2I2.y - num;
					goto IL_119;
				}
				y = vector2I2.y - DiscreteShadowCaster.TravelDirectionToOrtogonalDiractionVector(this.LightDirection).y * num2;
			}
		}
		else
		{
			int num3 = (int)this.Range;
			int num4 = num3 * 2;
			x = vector2I.x - num3;
			y = vector2I.y - num3;
			width = num4;
			num = ((this.shape == global::LightShape.Circle) ? num4 : num3);
		}
		IL_119:
		return new Extents(x, y, width, num);
	}

	// Token: 0x06004BC8 RID: 19400 RVA: 0x001AFDB8 File Offset: 0x001ADFB8
	private void AddToScenePartitioner()
	{
		Extents ext = this.ComputeExtents();
		this.solidPartitionerEntry = this.AddToLayer(ext, GameScenePartitioner.Instance.solidChangedLayer);
		this.liquidPartitionerEntry = this.AddToLayer(ext, GameScenePartitioner.Instance.liquidChangedLayer);
	}

	// Token: 0x06004BC9 RID: 19401 RVA: 0x001AFDFA File Offset: 0x001ADFFA
	private void RemoveFromScenePartitioner()
	{
		if (this.isRegistered)
		{
			GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
			GameScenePartitioner.Instance.Free(ref this.liquidPartitionerEntry);
		}
	}

	// Token: 0x06004BCA RID: 19402 RVA: 0x001AFE24 File Offset: 0x001AE024
	private void MoveInScenePartitioner()
	{
		GameScenePartitioner.Instance.UpdatePosition(this.solidPartitionerEntry, this.ComputeExtents());
		GameScenePartitioner.Instance.UpdatePosition(this.liquidPartitionerEntry, this.ComputeExtents());
	}

	// Token: 0x06004BCB RID: 19403 RVA: 0x001AFE52 File Offset: 0x001AE052
	private void EmitterRefresh()
	{
		this.emitter.Refresh(this.pending_emitter_state, true);
	}

	// Token: 0x06004BCC RID: 19404 RVA: 0x001AFE67 File Offset: 0x001AE067
	[ContextMenu("Refresh")]
	public void FullRefresh()
	{
		if (!base.isSpawned || !base.isActiveAndEnabled)
		{
			return;
		}
		DebugUtil.DevAssert(this.isRegistered, "shouldn't be refreshing if we aren't spawned and enabled", null);
		this.RefreshShapeAndPosition();
		this.EmitterRefresh();
	}

	// Token: 0x06004BCD RID: 19405 RVA: 0x001AFE98 File Offset: 0x001AE098
	public void FullRemove()
	{
		this.RemoveFromScenePartitioner();
		this.emitter.RemoveFromGrid();
	}

	// Token: 0x06004BCE RID: 19406 RVA: 0x001AFEAC File Offset: 0x001AE0AC
	public Light2D.RefreshResult RefreshShapeAndPosition()
	{
		if (!base.isSpawned)
		{
			return Light2D.RefreshResult.None;
		}
		if (!base.isActiveAndEnabled)
		{
			this.FullRemove();
			return Light2D.RefreshResult.Removed;
		}
		int num = Grid.PosToCell(base.transform.GetPosition() + this.Offset);
		if (!Grid.IsValidCell(num))
		{
			this.FullRemove();
			return Light2D.RefreshResult.Removed;
		}
		this.origin = num;
		if (this.dirty_shape)
		{
			this.RemoveFromScenePartitioner();
			this.AddToScenePartitioner();
		}
		else if (this.dirty_position)
		{
			this.MoveInScenePartitioner();
		}
		if (this.dirty_falloff)
		{
			this.EmitterRefresh();
		}
		this.dirty_shape = false;
		this.dirty_position = false;
		this.dirty_falloff = false;
		return Light2D.RefreshResult.Updated;
	}

	// Token: 0x06004BCF RID: 19407 RVA: 0x001AFF53 File Offset: 0x001AE153
	private void OnWorldChanged(object data)
	{
		this.FullRefresh();
	}

	// Token: 0x06004BD0 RID: 19408 RVA: 0x001AFF5C File Offset: 0x001AE15C
	public virtual List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.EMITS_LIGHT, this.Range), UI.GAMEOBJECTEFFECTS.TOOLTIPS.EMITS_LIGHT, Descriptor.DescriptorType.Effect, false),
			new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.EMITS_LIGHT_LUX, this.Lux), UI.GAMEOBJECTEFFECTS.TOOLTIPS.EMITS_LIGHT_LUX, Descriptor.DescriptorType.Effect, false)
		};
	}

	// Token: 0x04003188 RID: 12680
	public bool autoRespondToOperational = true;

	// Token: 0x04003189 RID: 12681
	private bool dirty_shape;

	// Token: 0x0400318A RID: 12682
	private bool dirty_position;

	// Token: 0x0400318B RID: 12683
	private bool dirty_falloff;

	// Token: 0x0400318C RID: 12684
	[SerializeField]
	private LightGridManager.LightGridEmitter.State pending_emitter_state = LightGridManager.LightGridEmitter.State.DEFAULT;

	// Token: 0x0400318F RID: 12687
	public float Angle;

	// Token: 0x04003190 RID: 12688
	public Vector2 Direction;

	// Token: 0x04003191 RID: 12689
	[SerializeField]
	private Vector2 _offset;

	// Token: 0x04003192 RID: 12690
	public bool drawOverlay;

	// Token: 0x04003193 RID: 12691
	public Color overlayColour;

	// Token: 0x04003194 RID: 12692
	public MaterialPropertyBlock materialPropertyBlock;

	// Token: 0x04003195 RID: 12693
	private HandleVector<int>.Handle solidPartitionerEntry = HandleVector<int>.InvalidHandle;

	// Token: 0x04003196 RID: 12694
	private HandleVector<int>.Handle liquidPartitionerEntry = HandleVector<int>.InvalidHandle;

	// Token: 0x04003197 RID: 12695
	public bool disableOnStore;

	// Token: 0x04003198 RID: 12696
	private static readonly EventSystem.IntraObjectHandler<Light2D> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<Light2D>(delegate(Light2D light, object data)
	{
		if (light.autoRespondToOperational)
		{
			light.enabled = (bool)data;
		}
	});

	// Token: 0x02001A47 RID: 6727
	public enum RefreshResult
	{
		// Token: 0x04007BE4 RID: 31716
		None,
		// Token: 0x04007BE5 RID: 31717
		Removed,
		// Token: 0x04007BE6 RID: 31718
		Updated
	}
}
