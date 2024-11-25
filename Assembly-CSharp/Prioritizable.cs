using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x020005A3 RID: 1443
[AddComponentMenu("KMonoBehaviour/scripts/Prioritizable")]
public class Prioritizable : KMonoBehaviour
{
	// Token: 0x0600224B RID: 8779 RVA: 0x000BED37 File Offset: 0x000BCF37
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Prioritizable>(-905833192, Prioritizable.OnCopySettingsDelegate);
	}

	// Token: 0x0600224C RID: 8780 RVA: 0x000BED50 File Offset: 0x000BCF50
	private void OnCopySettings(object data)
	{
		Prioritizable component = ((GameObject)data).GetComponent<Prioritizable>();
		if (component != null)
		{
			this.SetMasterPriority(component.GetMasterPriority());
		}
	}

	// Token: 0x0600224D RID: 8781 RVA: 0x000BED80 File Offset: 0x000BCF80
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.masterPriority != -2147483648)
		{
			this.masterPrioritySetting = new PrioritySetting(PriorityScreen.PriorityClass.basic, 5);
			this.masterPriority = int.MinValue;
		}
		PrioritySetting prioritySetting;
		if (SaveLoader.Instance.GameInfo.IsVersionExactly(7, 2) && Prioritizable.conversions.TryGetValue(this.masterPrioritySetting, out prioritySetting))
		{
			this.masterPrioritySetting = prioritySetting;
		}
	}

	// Token: 0x0600224E RID: 8782 RVA: 0x000BEDE4 File Offset: 0x000BCFE4
	protected override void OnSpawn()
	{
		if (this.onPriorityChanged != null)
		{
			this.onPriorityChanged(this.masterPrioritySetting);
		}
		this.RefreshHighPriorityNotification();
		this.RefreshTopPriorityOnWorld();
		Vector3 position = base.transform.GetPosition();
		Extents extents = new Extents((int)position.x, (int)position.y, 1, 1);
		this.scenePartitionerEntry = GameScenePartitioner.Instance.Add(base.name, this, extents, GameScenePartitioner.Instance.prioritizableObjects, null);
		Components.Prioritizables.Add(this);
	}

	// Token: 0x0600224F RID: 8783 RVA: 0x000BEE67 File Offset: 0x000BD067
	public PrioritySetting GetMasterPriority()
	{
		return this.masterPrioritySetting;
	}

	// Token: 0x06002250 RID: 8784 RVA: 0x000BEE70 File Offset: 0x000BD070
	public void SetMasterPriority(PrioritySetting priority)
	{
		if (!priority.Equals(this.masterPrioritySetting))
		{
			this.masterPrioritySetting = priority;
			if (this.onPriorityChanged != null)
			{
				this.onPriorityChanged(this.masterPrioritySetting);
			}
			this.RefreshTopPriorityOnWorld();
			this.RefreshHighPriorityNotification();
		}
	}

	// Token: 0x06002251 RID: 8785 RVA: 0x000BEEC3 File Offset: 0x000BD0C3
	private void RefreshTopPriorityOnWorld()
	{
		this.SetTopPriorityOnWorld(this.IsTopPriority());
	}

	// Token: 0x06002252 RID: 8786 RVA: 0x000BEED4 File Offset: 0x000BD0D4
	private void SetTopPriorityOnWorld(bool state)
	{
		WorldContainer myWorld = base.gameObject.GetMyWorld();
		if (Game.Instance == null || myWorld == null)
		{
			return;
		}
		if (state)
		{
			myWorld.AddTopPriorityPrioritizable(this);
			return;
		}
		myWorld.RemoveTopPriorityPrioritizable(this);
	}

	// Token: 0x06002253 RID: 8787 RVA: 0x000BEF16 File Offset: 0x000BD116
	public void AddRef()
	{
		this.refCount++;
		this.RefreshTopPriorityOnWorld();
		this.RefreshHighPriorityNotification();
	}

	// Token: 0x06002254 RID: 8788 RVA: 0x000BEF32 File Offset: 0x000BD132
	public void RemoveRef()
	{
		this.refCount--;
		if (this.IsTopPriority() || this.refCount == 0)
		{
			this.SetTopPriorityOnWorld(false);
		}
		this.RefreshHighPriorityNotification();
	}

	// Token: 0x06002255 RID: 8789 RVA: 0x000BEF5F File Offset: 0x000BD15F
	public bool IsPrioritizable()
	{
		return this.refCount > 0;
	}

	// Token: 0x06002256 RID: 8790 RVA: 0x000BEF6A File Offset: 0x000BD16A
	public bool IsTopPriority()
	{
		return this.masterPrioritySetting.priority_class == PriorityScreen.PriorityClass.topPriority && this.IsPrioritizable();
	}

	// Token: 0x06002257 RID: 8791 RVA: 0x000BEF84 File Offset: 0x000BD184
	protected override void OnCleanUp()
	{
		WorldContainer myWorld = base.gameObject.GetMyWorld();
		if (myWorld != null)
		{
			myWorld.RemoveTopPriorityPrioritizable(this);
		}
		else
		{
			global::Debug.LogWarning("World has been destroyed before prioritizable " + base.name);
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				worldContainer.RemoveTopPriorityPrioritizable(this);
			}
		}
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref this.scenePartitionerEntry);
		Components.Prioritizables.Remove(this);
	}

	// Token: 0x06002258 RID: 8792 RVA: 0x000BF028 File Offset: 0x000BD228
	public static void AddRef(GameObject go)
	{
		Prioritizable component = go.GetComponent<Prioritizable>();
		if (component != null)
		{
			component.AddRef();
		}
	}

	// Token: 0x06002259 RID: 8793 RVA: 0x000BF04C File Offset: 0x000BD24C
	public static void RemoveRef(GameObject go)
	{
		Prioritizable component = go.GetComponent<Prioritizable>();
		if (component != null)
		{
			component.RemoveRef();
		}
	}

	// Token: 0x0600225A RID: 8794 RVA: 0x000BF070 File Offset: 0x000BD270
	private void RefreshHighPriorityNotification()
	{
		bool flag = this.masterPrioritySetting.priority_class == PriorityScreen.PriorityClass.topPriority && this.IsPrioritizable();
		if (flag && this.highPriorityStatusItem == Guid.Empty)
		{
			this.highPriorityStatusItem = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.EmergencyPriority, null);
			return;
		}
		if (!flag && this.highPriorityStatusItem != Guid.Empty)
		{
			this.highPriorityStatusItem = base.GetComponent<KSelectable>().RemoveStatusItem(this.highPriorityStatusItem, false);
		}
	}

	// Token: 0x0400134C RID: 4940
	[SerializeField]
	[Serialize]
	private int masterPriority = int.MinValue;

	// Token: 0x0400134D RID: 4941
	[SerializeField]
	[Serialize]
	private PrioritySetting masterPrioritySetting = new PrioritySetting(PriorityScreen.PriorityClass.basic, 5);

	// Token: 0x0400134E RID: 4942
	public Action<PrioritySetting> onPriorityChanged;

	// Token: 0x0400134F RID: 4943
	public bool showIcon = true;

	// Token: 0x04001350 RID: 4944
	public Vector2 iconOffset;

	// Token: 0x04001351 RID: 4945
	public float iconScale = 1f;

	// Token: 0x04001352 RID: 4946
	[SerializeField]
	private int refCount;

	// Token: 0x04001353 RID: 4947
	private static readonly EventSystem.IntraObjectHandler<Prioritizable> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Prioritizable>(delegate(Prioritizable component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04001354 RID: 4948
	private static Dictionary<PrioritySetting, PrioritySetting> conversions = new Dictionary<PrioritySetting, PrioritySetting>
	{
		{
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 1),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 4)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 2),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 5)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 3),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 6)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 4),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 7)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 5),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 8)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.high, 1),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 6)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.high, 2),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 7)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.high, 3),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 8)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.high, 4),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 9)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.high, 5),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 9)
		}
	};

	// Token: 0x04001355 RID: 4949
	private HandleVector<int>.Handle scenePartitionerEntry;

	// Token: 0x04001356 RID: 4950
	private Guid highPriorityStatusItem;
}
