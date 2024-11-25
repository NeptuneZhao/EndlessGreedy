using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x02000658 RID: 1624
[AddComponentMenu("KMonoBehaviour/scripts/MinionAssignablesProxy")]
public class MinionAssignablesProxy : KMonoBehaviour, IAssignableIdentity
{
	// Token: 0x170001EC RID: 492
	// (get) Token: 0x060027FB RID: 10235 RVA: 0x000E3297 File Offset: 0x000E1497
	// (set) Token: 0x060027FC RID: 10236 RVA: 0x000E329F File Offset: 0x000E149F
	public IAssignableIdentity target { get; private set; }

	// Token: 0x170001ED RID: 493
	// (get) Token: 0x060027FD RID: 10237 RVA: 0x000E32A8 File Offset: 0x000E14A8
	public bool IsConfigured
	{
		get
		{
			return this.slotsConfigured;
		}
	}

	// Token: 0x170001EE RID: 494
	// (get) Token: 0x060027FE RID: 10238 RVA: 0x000E32B0 File Offset: 0x000E14B0
	public int TargetInstanceID
	{
		get
		{
			return this.target_instance_id;
		}
	}

	// Token: 0x060027FF RID: 10239 RVA: 0x000E32B8 File Offset: 0x000E14B8
	public GameObject GetTargetGameObject()
	{
		if (this.target == null && this.target_instance_id != -1)
		{
			this.RestoreTargetFromInstanceID();
		}
		KMonoBehaviour kmonoBehaviour = (KMonoBehaviour)this.target;
		if (kmonoBehaviour != null)
		{
			return kmonoBehaviour.gameObject;
		}
		return null;
	}

	// Token: 0x06002800 RID: 10240 RVA: 0x000E32FC File Offset: 0x000E14FC
	public float GetArrivalTime()
	{
		if (this.GetTargetGameObject().GetComponent<MinionIdentity>() != null)
		{
			return this.GetTargetGameObject().GetComponent<MinionIdentity>().arrivalTime;
		}
		if (this.GetTargetGameObject().GetComponent<StoredMinionIdentity>() != null)
		{
			return this.GetTargetGameObject().GetComponent<StoredMinionIdentity>().arrivalTime;
		}
		global::Debug.LogError("Could not get minion arrival time");
		return -1f;
	}

	// Token: 0x06002801 RID: 10241 RVA: 0x000E3360 File Offset: 0x000E1560
	public int GetTotalSkillpoints()
	{
		if (this.GetTargetGameObject().GetComponent<MinionIdentity>() != null)
		{
			return this.GetTargetGameObject().GetComponent<MinionResume>().TotalSkillPointsGained;
		}
		if (this.GetTargetGameObject().GetComponent<StoredMinionIdentity>() != null)
		{
			return MinionResume.CalculateTotalSkillPointsGained(this.GetTargetGameObject().GetComponent<StoredMinionIdentity>().TotalExperienceGained);
		}
		global::Debug.LogError("Could not get minion skill points time");
		return -1;
	}

	// Token: 0x06002802 RID: 10242 RVA: 0x000E33C8 File Offset: 0x000E15C8
	public void SetTarget(IAssignableIdentity target, GameObject targetGO)
	{
		global::Debug.Assert(target != null, "target was null");
		if (targetGO == null)
		{
			global::Debug.LogWarningFormat("{0} MinionAssignablesProxy.SetTarget {1}, {2}, {3}. DESTROYING", new object[]
			{
				base.GetInstanceID(),
				this.target_instance_id,
				target,
				targetGO
			});
			Util.KDestroyGameObject(base.gameObject);
		}
		this.target = target;
		this.target_instance_id = targetGO.GetComponent<KPrefabID>().InstanceID;
		base.gameObject.name = "Minion Assignables Proxy : " + targetGO.name;
	}

	// Token: 0x06002803 RID: 10243 RVA: 0x000E3460 File Offset: 0x000E1660
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.ownables = new List<Ownables>
		{
			base.gameObject.AddOrGet<Ownables>()
		};
		Components.MinionAssignablesProxy.Add(this);
		base.Subscribe<MinionAssignablesProxy>(1502190696, MinionAssignablesProxy.OnQueueDestroyObjectDelegate);
		this.ConfigureAssignableSlots();
	}

	// Token: 0x06002804 RID: 10244 RVA: 0x000E34B1 File Offset: 0x000E16B1
	[OnDeserialized]
	private void OnDeserialized()
	{
	}

	// Token: 0x06002805 RID: 10245 RVA: 0x000E34B4 File Offset: 0x000E16B4
	public void ConfigureAssignableSlots()
	{
		if (this.slotsConfigured)
		{
			return;
		}
		Ownables component = base.GetComponent<Ownables>();
		Equipment component2 = base.GetComponent<Equipment>();
		if (component2 != null)
		{
			foreach (AssignableSlot assignableSlot in Db.Get().AssignableSlots.resources)
			{
				if (assignableSlot is OwnableSlot)
				{
					OwnableSlotInstance slot_instance = new OwnableSlotInstance(component, (OwnableSlot)assignableSlot);
					component.Add(slot_instance);
				}
				else if (assignableSlot is EquipmentSlot)
				{
					EquipmentSlotInstance slot_instance2 = new EquipmentSlotInstance(component2, (EquipmentSlot)assignableSlot);
					component2.Add(slot_instance2);
				}
			}
		}
		this.slotsConfigured = true;
	}

	// Token: 0x06002806 RID: 10246 RVA: 0x000E3570 File Offset: 0x000E1770
	public void RestoreTargetFromInstanceID()
	{
		if (this.target_instance_id != -1 && this.target == null)
		{
			KPrefabID instance = KPrefabIDTracker.Get().GetInstance(this.target_instance_id);
			if (instance)
			{
				IAssignableIdentity component = instance.GetComponent<IAssignableIdentity>();
				if (component != null)
				{
					this.SetTarget(component, instance.gameObject);
					return;
				}
				global::Debug.LogWarningFormat("RestoreTargetFromInstanceID target ID {0} was found but it wasn't an IAssignableIdentity, destroying proxy object.", new object[]
				{
					this.target_instance_id
				});
				Util.KDestroyGameObject(base.gameObject);
				return;
			}
			else
			{
				global::Debug.LogWarningFormat("RestoreTargetFromInstanceID target ID {0} was not found, destroying proxy object.", new object[]
				{
					this.target_instance_id
				});
				Util.KDestroyGameObject(base.gameObject);
			}
		}
	}

	// Token: 0x06002807 RID: 10247 RVA: 0x000E3618 File Offset: 0x000E1818
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.RestoreTargetFromInstanceID();
		if (this.target != null)
		{
			base.Subscribe<MinionAssignablesProxy>(-1585839766, MinionAssignablesProxy.OnAssignablesChangedDelegate);
			Game.Instance.assignmentManager.AddToAssignmentGroup("public", this);
		}
	}

	// Token: 0x06002808 RID: 10248 RVA: 0x000E3654 File Offset: 0x000E1854
	private void OnQueueDestroyObject(object data)
	{
		Components.MinionAssignablesProxy.Remove(this);
	}

	// Token: 0x06002809 RID: 10249 RVA: 0x000E3661 File Offset: 0x000E1861
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Game.Instance.assignmentManager.RemoveFromAllGroups(this);
		base.GetComponent<Ownables>().UnassignAll();
		base.GetComponent<Equipment>().UnequipAll();
	}

	// Token: 0x0600280A RID: 10250 RVA: 0x000E368F File Offset: 0x000E188F
	private void OnAssignablesChanged(object data)
	{
		if (!this.target.IsNull())
		{
			((KMonoBehaviour)this.target).Trigger(-1585839766, data);
		}
	}

	// Token: 0x0600280B RID: 10251 RVA: 0x000E36B4 File Offset: 0x000E18B4
	private void CheckTarget()
	{
		if (this.target == null)
		{
			KPrefabID instance = KPrefabIDTracker.Get().GetInstance(this.target_instance_id);
			if (instance != null)
			{
				this.target = instance.GetComponent<IAssignableIdentity>();
				if (this.target != null)
				{
					MinionIdentity minionIdentity = this.target as MinionIdentity;
					if (minionIdentity)
					{
						minionIdentity.ValidateProxy();
						return;
					}
					StoredMinionIdentity storedMinionIdentity = this.target as StoredMinionIdentity;
					if (storedMinionIdentity)
					{
						storedMinionIdentity.ValidateProxy();
					}
				}
			}
		}
	}

	// Token: 0x0600280C RID: 10252 RVA: 0x000E372C File Offset: 0x000E192C
	public List<Ownables> GetOwners()
	{
		this.CheckTarget();
		return this.target.GetOwners();
	}

	// Token: 0x0600280D RID: 10253 RVA: 0x000E373F File Offset: 0x000E193F
	public string GetProperName()
	{
		this.CheckTarget();
		return this.target.GetProperName();
	}

	// Token: 0x0600280E RID: 10254 RVA: 0x000E3752 File Offset: 0x000E1952
	public Ownables GetSoleOwner()
	{
		this.CheckTarget();
		return this.target.GetSoleOwner();
	}

	// Token: 0x0600280F RID: 10255 RVA: 0x000E3765 File Offset: 0x000E1965
	public bool HasOwner(Assignables owner)
	{
		this.CheckTarget();
		return this.target.HasOwner(owner);
	}

	// Token: 0x06002810 RID: 10256 RVA: 0x000E3779 File Offset: 0x000E1979
	public int NumOwners()
	{
		this.CheckTarget();
		return this.target.NumOwners();
	}

	// Token: 0x06002811 RID: 10257 RVA: 0x000E378C File Offset: 0x000E198C
	public bool IsNull()
	{
		this.CheckTarget();
		return this.target.IsNull();
	}

	// Token: 0x06002812 RID: 10258 RVA: 0x000E37A0 File Offset: 0x000E19A0
	public static Ref<MinionAssignablesProxy> InitAssignableProxy(Ref<MinionAssignablesProxy> assignableProxyRef, IAssignableIdentity source)
	{
		if (assignableProxyRef == null)
		{
			assignableProxyRef = new Ref<MinionAssignablesProxy>();
		}
		GameObject gameObject = ((KMonoBehaviour)source).gameObject;
		MinionAssignablesProxy minionAssignablesProxy = assignableProxyRef.Get();
		if (minionAssignablesProxy == null)
		{
			GameObject gameObject2 = GameUtil.KInstantiate(Assets.GetPrefab(MinionAssignablesProxyConfig.ID), Grid.SceneLayer.NoLayer, null, 0);
			minionAssignablesProxy = gameObject2.GetComponent<MinionAssignablesProxy>();
			minionAssignablesProxy.SetTarget(source, gameObject);
			gameObject2.SetActive(true);
			assignableProxyRef.Set(minionAssignablesProxy);
		}
		else
		{
			minionAssignablesProxy.SetTarget(source, gameObject);
		}
		return assignableProxyRef;
	}

	// Token: 0x04001718 RID: 5912
	public List<Ownables> ownables;

	// Token: 0x0400171A RID: 5914
	[Serialize]
	private int target_instance_id = -1;

	// Token: 0x0400171B RID: 5915
	private bool slotsConfigured;

	// Token: 0x0400171C RID: 5916
	private static readonly EventSystem.IntraObjectHandler<MinionAssignablesProxy> OnAssignablesChangedDelegate = new EventSystem.IntraObjectHandler<MinionAssignablesProxy>(delegate(MinionAssignablesProxy component, object data)
	{
		component.OnAssignablesChanged(data);
	});

	// Token: 0x0400171D RID: 5917
	private static readonly EventSystem.IntraObjectHandler<MinionAssignablesProxy> OnQueueDestroyObjectDelegate = new EventSystem.IntraObjectHandler<MinionAssignablesProxy>(delegate(MinionAssignablesProxy component, object data)
	{
		component.OnQueueDestroyObject(data);
	});
}
