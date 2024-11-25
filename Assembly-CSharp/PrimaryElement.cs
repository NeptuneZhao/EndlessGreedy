using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Klei;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02000A02 RID: 2562
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/PrimaryElement")]
public class PrimaryElement : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x06004A2B RID: 18987 RVA: 0x001A7D78 File Offset: 0x001A5F78
	public void SetUseSimDiseaseInfo(bool use)
	{
		this.useSimDiseaseInfo = use;
	}

	// Token: 0x1700052E RID: 1326
	// (get) Token: 0x06004A2C RID: 18988 RVA: 0x001A7D81 File Offset: 0x001A5F81
	// (set) Token: 0x06004A2D RID: 18989 RVA: 0x001A7D8C File Offset: 0x001A5F8C
	[Serialize]
	public float Units
	{
		get
		{
			return this._units;
		}
		set
		{
			if (float.IsInfinity(value) || float.IsNaN(value))
			{
				DebugUtil.DevLogError("Invalid units value for element, setting Units to 0");
				this._units = 0f;
			}
			else
			{
				this._units = value;
			}
			if (this.onDataChanged != null)
			{
				this.onDataChanged(this);
			}
		}
	}

	// Token: 0x1700052F RID: 1327
	// (get) Token: 0x06004A2E RID: 18990 RVA: 0x001A7DDB File Offset: 0x001A5FDB
	// (set) Token: 0x06004A2F RID: 18991 RVA: 0x001A7DE9 File Offset: 0x001A5FE9
	public float Temperature
	{
		get
		{
			return this.getTemperatureCallback(this);
		}
		set
		{
			this.SetTemperature(value);
		}
	}

	// Token: 0x17000530 RID: 1328
	// (get) Token: 0x06004A30 RID: 18992 RVA: 0x001A7DF2 File Offset: 0x001A5FF2
	// (set) Token: 0x06004A31 RID: 18993 RVA: 0x001A7DFA File Offset: 0x001A5FFA
	public float InternalTemperature
	{
		get
		{
			return this._Temperature;
		}
		set
		{
			this._Temperature = value;
		}
	}

	// Token: 0x06004A32 RID: 18994 RVA: 0x001A7E04 File Offset: 0x001A6004
	[OnSerializing]
	private void OnSerializing()
	{
		this._Temperature = this.Temperature;
		this.SanitizeMassAndTemperature();
		this.diseaseID.HashValue = 0;
		this.diseaseCount = 0;
		if (this.useSimDiseaseInfo)
		{
			int i = Grid.PosToCell(base.transform.GetPosition());
			if (Grid.DiseaseIdx[i] != 255)
			{
				this.diseaseID = Db.Get().Diseases[(int)Grid.DiseaseIdx[i]].id;
				this.diseaseCount = Grid.DiseaseCount[i];
				return;
			}
		}
		else if (this.diseaseHandle.IsValid())
		{
			DiseaseHeader header = GameComps.DiseaseContainers.GetHeader(this.diseaseHandle);
			if (header.diseaseIdx != 255)
			{
				this.diseaseID = Db.Get().Diseases[(int)header.diseaseIdx].id;
				this.diseaseCount = header.diseaseCount;
			}
		}
	}

	// Token: 0x06004A33 RID: 18995 RVA: 0x001A7EF4 File Offset: 0x001A60F4
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.ElementID == (SimHashes)351109216)
		{
			this.ElementID = SimHashes.Creature;
		}
		this.SanitizeMassAndTemperature();
		float temperature = this._Temperature;
		if (float.IsNaN(temperature) || float.IsInfinity(temperature) || temperature < 0f || 10000f < temperature)
		{
			DeserializeWarnings.Instance.PrimaryElementTemperatureIsNan.Warn(string.Format("{0} has invalid temperature of {1}. Resetting temperature.", base.name, this.Temperature), null);
			temperature = this.Element.defaultValues.temperature;
		}
		this._Temperature = temperature;
		this.Temperature = temperature;
		if (this.Element == null)
		{
			DeserializeWarnings.Instance.PrimaryElementHasNoElement.Warn(base.name + "Primary element has no element.", null);
		}
		if (this.Mass < 0f)
		{
			DebugUtil.DevLogError(base.gameObject, "deserialized ore with less than 0 mass. Error! Destroying");
			Util.KDestroyGameObject(base.gameObject);
			return;
		}
		if (this.Mass == 0f && !this.KeepZeroMassObject)
		{
			DebugUtil.DevLogError(base.gameObject, "deserialized element with 0 mass. Destroying");
			Util.KDestroyGameObject(base.gameObject);
			return;
		}
		if (this.onDataChanged != null)
		{
			this.onDataChanged(this);
		}
		byte index = Db.Get().Diseases.GetIndex(this.diseaseID);
		if (index == 255 || this.diseaseCount <= 0)
		{
			if (this.diseaseHandle.IsValid())
			{
				GameComps.DiseaseContainers.Remove(base.gameObject);
				this.diseaseHandle.Clear();
				return;
			}
		}
		else
		{
			if (this.diseaseHandle.IsValid())
			{
				DiseaseHeader header = GameComps.DiseaseContainers.GetHeader(this.diseaseHandle);
				header.diseaseIdx = index;
				header.diseaseCount = this.diseaseCount;
				GameComps.DiseaseContainers.SetHeader(this.diseaseHandle, header);
				return;
			}
			this.diseaseHandle = GameComps.DiseaseContainers.Add(base.gameObject, index, this.diseaseCount);
		}
	}

	// Token: 0x06004A34 RID: 18996 RVA: 0x001A80D8 File Offset: 0x001A62D8
	protected override void OnLoadLevel()
	{
		base.OnLoadLevel();
	}

	// Token: 0x06004A35 RID: 18997 RVA: 0x001A80E0 File Offset: 0x001A62E0
	private void SanitizeMassAndTemperature()
	{
		if (this._Temperature <= 0f)
		{
			DebugUtil.DevLogError(base.gameObject.name + " is attempting to serialize a temperature of <= 0K. Resetting to default. world=" + base.gameObject.DebugGetMyWorldName());
			this._Temperature = this.Element.defaultValues.temperature;
		}
		if (this.Mass > PrimaryElement.MAX_MASS)
		{
			DebugUtil.DevLogError(string.Format("{0} is attempting to serialize very large mass {1}. Resetting to default. world={2}", base.gameObject.name, this.Mass, base.gameObject.DebugGetMyWorldName()));
			this.Mass = this.Element.defaultValues.mass;
		}
	}

	// Token: 0x17000531 RID: 1329
	// (get) Token: 0x06004A36 RID: 18998 RVA: 0x001A8188 File Offset: 0x001A6388
	// (set) Token: 0x06004A37 RID: 18999 RVA: 0x001A8197 File Offset: 0x001A6397
	public float Mass
	{
		get
		{
			return this.Units * this.MassPerUnit;
		}
		set
		{
			this.SetMass(value);
			if (this.onDataChanged != null)
			{
				this.onDataChanged(this);
			}
		}
	}

	// Token: 0x06004A38 RID: 19000 RVA: 0x001A81B4 File Offset: 0x001A63B4
	private void SetMass(float mass)
	{
		if ((mass > PrimaryElement.MAX_MASS || mass < 0f) && this.ElementID != SimHashes.Regolith)
		{
			DebugUtil.DevLogErrorFormat(base.gameObject, "{0} is getting an abnormal mass set {1}.", new object[]
			{
				base.gameObject.name,
				mass
			});
		}
		mass = Mathf.Clamp(mass, 0f, PrimaryElement.MAX_MASS);
		this.Units = mass / this.MassPerUnit;
		if (this.Units <= 0f && !this.KeepZeroMassObject)
		{
			Util.KDestroyGameObject(base.gameObject);
		}
	}

	// Token: 0x06004A39 RID: 19001 RVA: 0x001A824C File Offset: 0x001A644C
	private void SetTemperature(float temperature)
	{
		if (float.IsNaN(temperature) || float.IsInfinity(temperature))
		{
			DebugUtil.LogErrorArgs(base.gameObject, new object[]
			{
				"Invalid temperature [" + temperature.ToString() + "]"
			});
			return;
		}
		if (temperature <= 0f)
		{
			KCrashReporter.Assert(false, "Tried to set PrimaryElement.Temperature to a value <= 0", null);
		}
		this.setTemperatureCallback(this, temperature);
	}

	// Token: 0x06004A3A RID: 19002 RVA: 0x001A82B5 File Offset: 0x001A64B5
	public void SetMassTemperature(float mass, float temperature)
	{
		this.SetMass(mass);
		this.SetTemperature(temperature);
	}

	// Token: 0x17000532 RID: 1330
	// (get) Token: 0x06004A3B RID: 19003 RVA: 0x001A82C5 File Offset: 0x001A64C5
	public Element Element
	{
		get
		{
			if (this._Element == null)
			{
				this._Element = ElementLoader.FindElementByHash(this.ElementID);
			}
			return this._Element;
		}
	}

	// Token: 0x17000533 RID: 1331
	// (get) Token: 0x06004A3C RID: 19004 RVA: 0x001A82E8 File Offset: 0x001A64E8
	public byte DiseaseIdx
	{
		get
		{
			if (this.diseaseRedirectTarget)
			{
				return this.diseaseRedirectTarget.DiseaseIdx;
			}
			byte result = byte.MaxValue;
			if (this.useSimDiseaseInfo)
			{
				int i = Grid.PosToCell(base.transform.GetPosition());
				result = Grid.DiseaseIdx[i];
			}
			else if (this.diseaseHandle.IsValid())
			{
				result = GameComps.DiseaseContainers.GetHeader(this.diseaseHandle).diseaseIdx;
			}
			return result;
		}
	}

	// Token: 0x17000534 RID: 1332
	// (get) Token: 0x06004A3D RID: 19005 RVA: 0x001A8360 File Offset: 0x001A6560
	public int DiseaseCount
	{
		get
		{
			if (this.diseaseRedirectTarget)
			{
				return this.diseaseRedirectTarget.DiseaseCount;
			}
			int result = 0;
			if (this.useSimDiseaseInfo)
			{
				int i = Grid.PosToCell(base.transform.GetPosition());
				result = Grid.DiseaseCount[i];
			}
			else if (this.diseaseHandle.IsValid())
			{
				result = GameComps.DiseaseContainers.GetHeader(this.diseaseHandle).diseaseCount;
			}
			return result;
		}
	}

	// Token: 0x06004A3E RID: 19006 RVA: 0x001A83D3 File Offset: 0x001A65D3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		GameComps.InfraredVisualizers.Add(base.gameObject);
		base.Subscribe<PrimaryElement>(1335436905, PrimaryElement.OnSplitFromChunkDelegate);
		base.Subscribe<PrimaryElement>(-2064133523, PrimaryElement.OnAbsorbDelegate);
	}

	// Token: 0x06004A3F RID: 19007 RVA: 0x001A8410 File Offset: 0x001A6610
	protected override void OnSpawn()
	{
		Attributes attributes = this.GetAttributes();
		if (attributes != null)
		{
			foreach (AttributeModifier modifier in this.Element.attributeModifiers)
			{
				attributes.Add(modifier);
			}
		}
	}

	// Token: 0x06004A40 RID: 19008 RVA: 0x001A8474 File Offset: 0x001A6674
	public void ForcePermanentDiseaseContainer(bool force_on)
	{
		if (force_on)
		{
			if (!this.diseaseHandle.IsValid())
			{
				this.diseaseHandle = GameComps.DiseaseContainers.Add(base.gameObject, byte.MaxValue, 0);
			}
		}
		else if (this.diseaseHandle.IsValid() && this.DiseaseIdx == 255)
		{
			GameComps.DiseaseContainers.Remove(base.gameObject);
			this.diseaseHandle.Clear();
		}
		this.forcePermanentDiseaseContainer = force_on;
	}

	// Token: 0x06004A41 RID: 19009 RVA: 0x001A84EB File Offset: 0x001A66EB
	protected override void OnCleanUp()
	{
		GameComps.InfraredVisualizers.Remove(base.gameObject);
		if (this.diseaseHandle.IsValid())
		{
			GameComps.DiseaseContainers.Remove(base.gameObject);
			this.diseaseHandle.Clear();
		}
		base.OnCleanUp();
	}

	// Token: 0x06004A42 RID: 19010 RVA: 0x001A852B File Offset: 0x001A672B
	public void SetElement(SimHashes element_id, bool addTags = true)
	{
		this.ElementID = element_id;
		if (addTags)
		{
			this.UpdateTags();
		}
	}

	// Token: 0x06004A43 RID: 19011 RVA: 0x001A8540 File Offset: 0x001A6740
	public void UpdateTags()
	{
		if (this.ElementID == (SimHashes)0)
		{
			global::Debug.Log("UpdateTags() Primary element 0", base.gameObject);
			return;
		}
		KPrefabID component = base.GetComponent<KPrefabID>();
		if (component != null)
		{
			List<Tag> list = new List<Tag>();
			foreach (Tag item in this.Element.oreTags)
			{
				list.Add(item);
			}
			if (component.HasAnyTags(PrimaryElement.metalTags))
			{
				list.Add(GameTags.StoredMetal);
			}
			foreach (Tag tag in list)
			{
				component.AddTag(tag, false);
			}
		}
	}

	// Token: 0x06004A44 RID: 19012 RVA: 0x001A8604 File Offset: 0x001A6804
	public void ModifyDiseaseCount(int delta, string reason)
	{
		if (this.diseaseRedirectTarget)
		{
			this.diseaseRedirectTarget.ModifyDiseaseCount(delta, reason);
			return;
		}
		if (this.useSimDiseaseInfo)
		{
			SimMessages.ModifyDiseaseOnCell(Grid.PosToCell(this), byte.MaxValue, delta);
			return;
		}
		if (delta != 0 && this.diseaseHandle.IsValid() && GameComps.DiseaseContainers.ModifyDiseaseCount(this.diseaseHandle, delta) <= 0 && !this.forcePermanentDiseaseContainer)
		{
			base.Trigger(-1689370368, false);
			GameComps.DiseaseContainers.Remove(base.gameObject);
			this.diseaseHandle.Clear();
		}
	}

	// Token: 0x06004A45 RID: 19013 RVA: 0x001A86A0 File Offset: 0x001A68A0
	public void AddDisease(byte disease_idx, int delta, string reason)
	{
		if (delta == 0)
		{
			return;
		}
		if (this.diseaseRedirectTarget)
		{
			this.diseaseRedirectTarget.AddDisease(disease_idx, delta, reason);
			return;
		}
		if (this.useSimDiseaseInfo)
		{
			SimMessages.ModifyDiseaseOnCell(Grid.PosToCell(this), disease_idx, delta);
			return;
		}
		if (this.diseaseHandle.IsValid())
		{
			if (GameComps.DiseaseContainers.AddDisease(this.diseaseHandle, disease_idx, delta) <= 0)
			{
				GameComps.DiseaseContainers.Remove(base.gameObject);
				this.diseaseHandle.Clear();
				return;
			}
		}
		else if (delta > 0)
		{
			this.diseaseHandle = GameComps.DiseaseContainers.Add(base.gameObject, disease_idx, delta);
			base.Trigger(-1689370368, true);
			base.Trigger(-283306403, null);
		}
	}

	// Token: 0x06004A46 RID: 19014 RVA: 0x001A875A File Offset: 0x001A695A
	private static float OnGetTemperature(PrimaryElement primary_element)
	{
		return primary_element._Temperature;
	}

	// Token: 0x06004A47 RID: 19015 RVA: 0x001A8764 File Offset: 0x001A6964
	private static void OnSetTemperature(PrimaryElement primary_element, float temperature)
	{
		global::Debug.Assert(!float.IsNaN(temperature));
		if (temperature <= 0f)
		{
			DebugUtil.LogErrorArgs(primary_element.gameObject, new object[]
			{
				primary_element.gameObject.name + " has a temperature of zero which has always been an error in my experience."
			});
		}
		primary_element._Temperature = temperature;
	}

	// Token: 0x06004A48 RID: 19016 RVA: 0x001A87B8 File Offset: 0x001A69B8
	private void OnSplitFromChunk(object data)
	{
		Pickupable pickupable = (Pickupable)data;
		if (pickupable == null)
		{
			return;
		}
		float percent = this.Units / (this.Units + pickupable.PrimaryElement.Units);
		SimUtil.DiseaseInfo percentOfDisease = SimUtil.GetPercentOfDisease(pickupable.PrimaryElement, percent);
		this.AddDisease(percentOfDisease.idx, percentOfDisease.count, "PrimaryElement.SplitFromChunk");
		pickupable.PrimaryElement.ModifyDiseaseCount(-percentOfDisease.count, "PrimaryElement.SplitFromChunk");
	}

	// Token: 0x06004A49 RID: 19017 RVA: 0x001A882C File Offset: 0x001A6A2C
	private void OnAbsorb(object data)
	{
		Pickupable pickupable = (Pickupable)data;
		if (pickupable == null)
		{
			return;
		}
		this.AddDisease(pickupable.PrimaryElement.DiseaseIdx, pickupable.PrimaryElement.DiseaseCount, "PrimaryElement.OnAbsorb");
	}

	// Token: 0x06004A4A RID: 19018 RVA: 0x001A886C File Offset: 0x001A6A6C
	private void SetDiseaseVisualProvider(GameObject visualizer)
	{
		HandleVector<int>.Handle handle = GameComps.DiseaseContainers.GetHandle(base.gameObject);
		if (handle != HandleVector<int>.InvalidHandle)
		{
			DiseaseContainer payload = GameComps.DiseaseContainers.GetPayload(handle);
			payload.visualDiseaseProvider = visualizer;
			GameComps.DiseaseContainers.SetPayload(handle, ref payload);
		}
	}

	// Token: 0x06004A4B RID: 19019 RVA: 0x001A88B8 File Offset: 0x001A6AB8
	public void RedirectDisease(GameObject target)
	{
		this.SetDiseaseVisualProvider(target);
		this.diseaseRedirectTarget = (target ? target.GetComponent<PrimaryElement>() : null);
		global::Debug.Assert(this.diseaseRedirectTarget != this, "Disease redirect target set to myself");
	}

	// Token: 0x040030A8 RID: 12456
	public static float MAX_MASS = 100000f;

	// Token: 0x040030A9 RID: 12457
	public SimTemperatureTransfer sttOptimizationHook;

	// Token: 0x040030AA RID: 12458
	public PrimaryElement.GetTemperatureCallback getTemperatureCallback = new PrimaryElement.GetTemperatureCallback(PrimaryElement.OnGetTemperature);

	// Token: 0x040030AB RID: 12459
	public PrimaryElement.SetTemperatureCallback setTemperatureCallback = new PrimaryElement.SetTemperatureCallback(PrimaryElement.OnSetTemperature);

	// Token: 0x040030AC RID: 12460
	private PrimaryElement diseaseRedirectTarget;

	// Token: 0x040030AD RID: 12461
	private bool useSimDiseaseInfo;

	// Token: 0x040030AE RID: 12462
	public const float DefaultChunkMass = 400f;

	// Token: 0x040030AF RID: 12463
	private static readonly Tag[] metalTags = new Tag[]
	{
		GameTags.Metal,
		GameTags.RefinedMetal
	};

	// Token: 0x040030B0 RID: 12464
	[Serialize]
	[HashedEnum]
	public SimHashes ElementID;

	// Token: 0x040030B1 RID: 12465
	private float _units = 1f;

	// Token: 0x040030B2 RID: 12466
	[Serialize]
	[SerializeField]
	private float _Temperature;

	// Token: 0x040030B3 RID: 12467
	[Serialize]
	[NonSerialized]
	public bool KeepZeroMassObject;

	// Token: 0x040030B4 RID: 12468
	[Serialize]
	private HashedString diseaseID;

	// Token: 0x040030B5 RID: 12469
	[Serialize]
	private int diseaseCount;

	// Token: 0x040030B6 RID: 12470
	private HandleVector<int>.Handle diseaseHandle = HandleVector<int>.InvalidHandle;

	// Token: 0x040030B7 RID: 12471
	public float MassPerUnit = 1f;

	// Token: 0x040030B8 RID: 12472
	[NonSerialized]
	private Element _Element;

	// Token: 0x040030B9 RID: 12473
	[NonSerialized]
	public Action<PrimaryElement> onDataChanged;

	// Token: 0x040030BA RID: 12474
	[NonSerialized]
	private bool forcePermanentDiseaseContainer;

	// Token: 0x040030BB RID: 12475
	private static readonly EventSystem.IntraObjectHandler<PrimaryElement> OnSplitFromChunkDelegate = new EventSystem.IntraObjectHandler<PrimaryElement>(delegate(PrimaryElement component, object data)
	{
		component.OnSplitFromChunk(data);
	});

	// Token: 0x040030BC RID: 12476
	private static readonly EventSystem.IntraObjectHandler<PrimaryElement> OnAbsorbDelegate = new EventSystem.IntraObjectHandler<PrimaryElement>(delegate(PrimaryElement component, object data)
	{
		component.OnAbsorb(data);
	});

	// Token: 0x02001A1E RID: 6686
	// (Invoke) Token: 0x06009F31 RID: 40753
	public delegate float GetTemperatureCallback(PrimaryElement primary_element);

	// Token: 0x02001A1F RID: 6687
	// (Invoke) Token: 0x06009F35 RID: 40757
	public delegate void SetTemperatureCallback(PrimaryElement primary_element, float temperature);
}
