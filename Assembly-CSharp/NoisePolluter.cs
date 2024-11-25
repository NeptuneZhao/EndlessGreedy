using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020009C6 RID: 2502
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/NoisePolluter")]
public class NoisePolluter : KMonoBehaviour, IPolluter
{
	// Token: 0x0600489A RID: 18586 RVA: 0x0019F74F File Offset: 0x0019D94F
	public static bool IsNoiseableCell(int cell)
	{
		return Grid.IsValidCell(cell) && (Grid.IsGas(cell) || !Grid.IsSubstantialLiquid(cell, 0.35f));
	}

	// Token: 0x0600489B RID: 18587 RVA: 0x0019F773 File Offset: 0x0019D973
	public void ResetCells()
	{
		if (this.radius == 0)
		{
			global::Debug.LogFormat("[{0}] has a 0 radius noise, this will disable it", new object[]
			{
				this.GetName()
			});
			return;
		}
	}

	// Token: 0x0600489C RID: 18588 RVA: 0x0019F797 File Offset: 0x0019D997
	public void SetAttributes(Vector2 pos, int dB, GameObject go, string name)
	{
		this.sourceName = name;
		this.noise = dB;
	}

	// Token: 0x0600489D RID: 18589 RVA: 0x0019F7A8 File Offset: 0x0019D9A8
	public int GetRadius()
	{
		return this.radius;
	}

	// Token: 0x0600489E RID: 18590 RVA: 0x0019F7B0 File Offset: 0x0019D9B0
	public int GetNoise()
	{
		return this.noise;
	}

	// Token: 0x0600489F RID: 18591 RVA: 0x0019F7B8 File Offset: 0x0019D9B8
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060048A0 RID: 18592 RVA: 0x0019F7C0 File Offset: 0x0019D9C0
	public void SetSplat(NoiseSplat new_splat)
	{
		this.splat = new_splat;
	}

	// Token: 0x060048A1 RID: 18593 RVA: 0x0019F7C9 File Offset: 0x0019D9C9
	public void Clear()
	{
		if (this.splat != null)
		{
			this.splat.Clear();
			this.splat = null;
		}
	}

	// Token: 0x060048A2 RID: 18594 RVA: 0x0019F7E5 File Offset: 0x0019D9E5
	public Vector2 GetPosition()
	{
		return base.transform.GetPosition();
	}

	// Token: 0x17000518 RID: 1304
	// (get) Token: 0x060048A3 RID: 18595 RVA: 0x0019F7F7 File Offset: 0x0019D9F7
	// (set) Token: 0x060048A4 RID: 18596 RVA: 0x0019F7FF File Offset: 0x0019D9FF
	public string sourceName { get; private set; }

	// Token: 0x17000519 RID: 1305
	// (get) Token: 0x060048A5 RID: 18597 RVA: 0x0019F808 File Offset: 0x0019DA08
	// (set) Token: 0x060048A6 RID: 18598 RVA: 0x0019F810 File Offset: 0x0019DA10
	public bool active { get; private set; }

	// Token: 0x060048A7 RID: 18599 RVA: 0x0019F819 File Offset: 0x0019DA19
	public void SetActive(bool active = true)
	{
		if (!active && this.splat != null)
		{
			AudioEventManager.Get().ClearNoiseSplat(this.splat);
			this.splat.Clear();
		}
		this.active = active;
	}

	// Token: 0x060048A8 RID: 18600 RVA: 0x0019F848 File Offset: 0x0019DA48
	public void Refresh()
	{
		if (this.active)
		{
			if (this.splat != null)
			{
				AudioEventManager.Get().ClearNoiseSplat(this.splat);
				this.splat.Clear();
			}
			KSelectable component = base.GetComponent<KSelectable>();
			string name = (component != null) ? component.GetName() : base.name;
			GameObject gameObject = base.GetComponent<KMonoBehaviour>().gameObject;
			this.splat = AudioEventManager.Get().CreateNoiseSplat(this.GetPosition(), this.noise, this.radius, name, gameObject);
		}
	}

	// Token: 0x060048A9 RID: 18601 RVA: 0x0019F8D0 File Offset: 0x0019DAD0
	private void OnActiveChanged(object data)
	{
		bool isActive = ((Operational)data).IsActive;
		this.SetActive(isActive);
		this.Refresh();
	}

	// Token: 0x060048AA RID: 18602 RVA: 0x0019F8F6 File Offset: 0x0019DAF6
	public void SetValues(EffectorValues values)
	{
		this.noise = values.amount;
		this.radius = values.radius;
	}

	// Token: 0x060048AB RID: 18603 RVA: 0x0019F910 File Offset: 0x0019DB10
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.radius == 0 || this.noise == 0)
		{
			global::Debug.LogWarning(string.Concat(new string[]
			{
				"Noisepollutor::OnSpawn [",
				this.GetName(),
				"] noise: [",
				this.noise.ToString(),
				"] radius: [",
				this.radius.ToString(),
				"]"
			}));
			UnityEngine.Object.Destroy(this);
			return;
		}
		this.ResetCells();
		Operational component = base.GetComponent<Operational>();
		if (component != null)
		{
			base.Subscribe<NoisePolluter>(824508782, NoisePolluter.OnActiveChangedDelegate);
		}
		this.refreshCallback = new System.Action(this.Refresh);
		this.refreshPartionerCallback = delegate(object data)
		{
			this.Refresh();
		};
		this.onCollectNoisePollutersCallback = new Action<object>(this.OnCollectNoisePolluters);
		Attributes attributes = this.GetAttributes();
		Db db = Db.Get();
		this.dB = attributes.Add(db.BuildingAttributes.NoisePollution);
		this.dBRadius = attributes.Add(db.BuildingAttributes.NoisePollutionRadius);
		if (this.noise != 0 && this.radius != 0)
		{
			AttributeModifier modifier = new AttributeModifier(db.BuildingAttributes.NoisePollution.Id, (float)this.noise, UI.TOOLTIPS.BASE_VALUE, false, false, true);
			AttributeModifier modifier2 = new AttributeModifier(db.BuildingAttributes.NoisePollutionRadius.Id, (float)this.radius, UI.TOOLTIPS.BASE_VALUE, false, false, true);
			attributes.Add(modifier);
			attributes.Add(modifier2);
		}
		else
		{
			global::Debug.LogWarning(string.Concat(new string[]
			{
				"Noisepollutor::OnSpawn [",
				this.GetName(),
				"] radius: [",
				this.radius.ToString(),
				"] noise: [",
				this.noise.ToString(),
				"]"
			}));
		}
		KBatchedAnimController component2 = base.GetComponent<KBatchedAnimController>();
		this.isMovable = (component2 != null && component2.isMovable);
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "NoisePolluter.OnSpawn");
		AttributeInstance attributeInstance = this.dB;
		attributeInstance.OnDirty = (System.Action)Delegate.Combine(attributeInstance.OnDirty, this.refreshCallback);
		AttributeInstance attributeInstance2 = this.dBRadius;
		attributeInstance2.OnDirty = (System.Action)Delegate.Combine(attributeInstance2.OnDirty, this.refreshCallback);
		if (component != null)
		{
			this.OnActiveChanged(component.IsActive);
		}
	}

	// Token: 0x060048AC RID: 18604 RVA: 0x0019FB91 File Offset: 0x0019DD91
	private void OnCellChange()
	{
		this.Refresh();
	}

	// Token: 0x060048AD RID: 18605 RVA: 0x0019FB99 File Offset: 0x0019DD99
	private void OnCollectNoisePolluters(object data)
	{
		((List<NoisePolluter>)data).Add(this);
	}

	// Token: 0x060048AE RID: 18606 RVA: 0x0019FBA7 File Offset: 0x0019DDA7
	public string GetName()
	{
		if (string.IsNullOrEmpty(this.sourceName))
		{
			this.sourceName = base.GetComponent<KSelectable>().GetName();
		}
		return this.sourceName;
	}

	// Token: 0x060048AF RID: 18607 RVA: 0x0019FBD0 File Offset: 0x0019DDD0
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (base.isSpawned)
		{
			if (this.dB != null)
			{
				AttributeInstance attributeInstance = this.dB;
				attributeInstance.OnDirty = (System.Action)Delegate.Remove(attributeInstance.OnDirty, this.refreshCallback);
				AttributeInstance attributeInstance2 = this.dBRadius;
				attributeInstance2.OnDirty = (System.Action)Delegate.Remove(attributeInstance2.OnDirty, this.refreshCallback);
			}
			if (this.isMovable)
			{
				Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
			}
		}
		if (this.splat != null)
		{
			AudioEventManager.Get().ClearNoiseSplat(this.splat);
			this.splat.Clear();
		}
	}

	// Token: 0x060048B0 RID: 18608 RVA: 0x0019FC7C File Offset: 0x0019DE7C
	public float GetNoiseForCell(int cell)
	{
		return this.splat.GetDBForCell(cell);
	}

	// Token: 0x060048B1 RID: 18609 RVA: 0x0019FC8C File Offset: 0x0019DE8C
	public List<Descriptor> GetEffectDescriptions()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.dB != null && this.dBRadius != null)
		{
			float totalValue = this.dB.GetTotalValue();
			float totalValue2 = this.dBRadius.GetTotalValue();
			string text = (this.noise > 0) ? UI.BUILDINGEFFECTS.TOOLTIPS.NOISE_POLLUTION_INCREASE : UI.BUILDINGEFFECTS.TOOLTIPS.NOISE_POLLUTION_DECREASE;
			text = text + "\n\n" + this.dB.GetAttributeValueTooltip();
			string arg = GameUtil.AddPositiveSign(totalValue.ToString(), totalValue > 0f);
			Descriptor item = new Descriptor(string.Format(UI.BUILDINGEFFECTS.NOISE_CREATED, arg, totalValue2), string.Format(text, arg, totalValue2), Descriptor.DescriptorType.Effect, false);
			list.Add(item);
		}
		else if (this.noise != 0)
		{
			string format = (this.noise >= 0) ? UI.BUILDINGEFFECTS.TOOLTIPS.NOISE_POLLUTION_INCREASE : UI.BUILDINGEFFECTS.TOOLTIPS.NOISE_POLLUTION_DECREASE;
			string arg2 = GameUtil.AddPositiveSign(this.noise.ToString(), this.noise > 0);
			Descriptor item2 = new Descriptor(string.Format(UI.BUILDINGEFFECTS.NOISE_CREATED, arg2, this.radius), string.Format(format, arg2, this.radius), Descriptor.DescriptorType.Effect, false);
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x060048B2 RID: 18610 RVA: 0x0019FDD1 File Offset: 0x0019DFD1
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return this.GetEffectDescriptions();
	}

	// Token: 0x04002F88 RID: 12168
	public const string ID = "NoisePolluter";

	// Token: 0x04002F89 RID: 12169
	public int radius;

	// Token: 0x04002F8A RID: 12170
	public int noise;

	// Token: 0x04002F8B RID: 12171
	public AttributeInstance dB;

	// Token: 0x04002F8C RID: 12172
	public AttributeInstance dBRadius;

	// Token: 0x04002F8D RID: 12173
	private NoiseSplat splat;

	// Token: 0x04002F8F RID: 12175
	public System.Action refreshCallback;

	// Token: 0x04002F90 RID: 12176
	public Action<object> refreshPartionerCallback;

	// Token: 0x04002F91 RID: 12177
	public Action<object> onCollectNoisePollutersCallback;

	// Token: 0x04002F92 RID: 12178
	public bool isMovable;

	// Token: 0x04002F93 RID: 12179
	[MyCmpReq]
	public OccupyArea occupyArea;

	// Token: 0x04002F95 RID: 12181
	private static readonly EventSystem.IntraObjectHandler<NoisePolluter> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<NoisePolluter>(delegate(NoisePolluter component, object data)
	{
		component.OnActiveChanged(data);
	});
}
