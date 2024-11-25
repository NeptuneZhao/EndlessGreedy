using System;
using System.Collections;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;

// Token: 0x02000AEC RID: 2796
public class AsteroidGridEntity : ClusterGridEntity
{
	// Token: 0x06005351 RID: 21329 RVA: 0x001DDFF2 File Offset: 0x001DC1F2
	public override bool ShowName()
	{
		return true;
	}

	// Token: 0x1700064B RID: 1611
	// (get) Token: 0x06005352 RID: 21330 RVA: 0x001DDFF5 File Offset: 0x001DC1F5
	public override string Name
	{
		get
		{
			return this.m_name;
		}
	}

	// Token: 0x1700064C RID: 1612
	// (get) Token: 0x06005353 RID: 21331 RVA: 0x001DDFFD File Offset: 0x001DC1FD
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.Asteroid;
		}
	}

	// Token: 0x1700064D RID: 1613
	// (get) Token: 0x06005354 RID: 21332 RVA: 0x001DE000 File Offset: 0x001DC200
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			List<ClusterGridEntity.AnimConfig> list = new List<ClusterGridEntity.AnimConfig>();
			ClusterGridEntity.AnimConfig item = new ClusterGridEntity.AnimConfig
			{
				animFile = Assets.GetAnim(this.m_asteroidAnim.IsNullOrWhiteSpace() ? AsteroidGridEntity.DEFAULT_ASTEROID_ICON_ANIM : this.m_asteroidAnim),
				initialAnim = "idle_loop"
			};
			list.Add(item);
			item = new ClusterGridEntity.AnimConfig
			{
				animFile = Assets.GetAnim("orbit_kanim"),
				initialAnim = "orbit"
			};
			list.Add(item);
			item = new ClusterGridEntity.AnimConfig
			{
				animFile = Assets.GetAnim("shower_asteroid_current_kanim"),
				initialAnim = "off",
				playMode = KAnim.PlayMode.Once
			};
			list.Add(item);
			return list;
		}
	}

	// Token: 0x1700064E RID: 1614
	// (get) Token: 0x06005355 RID: 21333 RVA: 0x001DE0C2 File Offset: 0x001DC2C2
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700064F RID: 1615
	// (get) Token: 0x06005356 RID: 21334 RVA: 0x001DE0C5 File Offset: 0x001DC2C5
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Peeked;
		}
	}

	// Token: 0x06005357 RID: 21335 RVA: 0x001DE0C8 File Offset: 0x001DC2C8
	public void Init(string name, AxialI location, string asteroidTypeId)
	{
		this.m_name = name;
		this.m_location = location;
		this.m_asteroidAnim = asteroidTypeId;
	}

	// Token: 0x06005358 RID: 21336 RVA: 0x001DE0E0 File Offset: 0x001DC2E0
	protected override void OnSpawn()
	{
		KAnimFile kanimFile;
		if (!Assets.TryGetAnim(this.m_asteroidAnim, out kanimFile))
		{
			this.m_asteroidAnim = AsteroidGridEntity.DEFAULT_ASTEROID_ICON_ANIM;
		}
		Game.Instance.Subscribe(-1298331547, new Action<object>(this.OnClusterLocationChanged));
		Game.Instance.Subscribe(-1991583975, new Action<object>(this.OnFogOfWarRevealed));
		Game.Instance.Subscribe(78366336, new Action<object>(this.OnMeteorShowerEventChanged));
		Game.Instance.Subscribe(1749562766, new Action<object>(this.OnMeteorShowerEventChanged));
		if (ClusterGrid.Instance.IsCellVisible(this.m_location))
		{
			SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>().RevealLocation(this.m_location, 1);
		}
		base.OnSpawn();
	}

	// Token: 0x06005359 RID: 21337 RVA: 0x001DE1AC File Offset: 0x001DC3AC
	protected override void OnCleanUp()
	{
		Game.Instance.Unsubscribe(-1298331547, new Action<object>(this.OnClusterLocationChanged));
		Game.Instance.Unsubscribe(-1991583975, new Action<object>(this.OnFogOfWarRevealed));
		Game.Instance.Unsubscribe(78366336, new Action<object>(this.OnMeteorShowerEventChanged));
		Game.Instance.Unsubscribe(1749562766, new Action<object>(this.OnMeteorShowerEventChanged));
		base.OnCleanUp();
	}

	// Token: 0x0600535A RID: 21338 RVA: 0x001DE22C File Offset: 0x001DC42C
	public void OnClusterLocationChanged(object data)
	{
		if (this.m_worldContainer.IsDiscovered)
		{
			return;
		}
		if (!ClusterGrid.Instance.IsCellVisible(base.Location))
		{
			return;
		}
		Clustercraft component = ((ClusterLocationChangedEvent)data).entity.GetComponent<Clustercraft>();
		if (component == null)
		{
			return;
		}
		if (component.GetOrbitAsteroid() == this)
		{
			this.m_worldContainer.SetDiscovered(true);
		}
	}

	// Token: 0x0600535B RID: 21339 RVA: 0x001DE28F File Offset: 0x001DC48F
	public override void OnClusterMapIconShown(ClusterRevealLevel levelUsed)
	{
		base.OnClusterMapIconShown(levelUsed);
		if (levelUsed == ClusterRevealLevel.Visible)
		{
			this.RefreshMeteorShowerEffect();
		}
	}

	// Token: 0x0600535C RID: 21340 RVA: 0x001DE2A2 File Offset: 0x001DC4A2
	private void OnMeteorShowerEventChanged(object _worldID)
	{
		if ((int)_worldID == this.m_worldContainer.id)
		{
			this.RefreshMeteorShowerEffect();
		}
	}

	// Token: 0x0600535D RID: 21341 RVA: 0x001DE2C0 File Offset: 0x001DC4C0
	public void RefreshMeteorShowerEffect()
	{
		if (ClusterMapScreen.Instance == null)
		{
			return;
		}
		ClusterMapVisualizer entityVisAnim = ClusterMapScreen.Instance.GetEntityVisAnim(this);
		if (entityVisAnim == null)
		{
			return;
		}
		KBatchedAnimController animController = entityVisAnim.GetAnimController(2);
		if (animController != null)
		{
			List<GameplayEventInstance> list = new List<GameplayEventInstance>();
			GameplayEventManager.Instance.GetActiveEventsOfType<MeteorShowerEvent>(this.m_worldContainer.id, ref list);
			bool flag = false;
			string s = "off";
			foreach (GameplayEventInstance gameplayEventInstance in list)
			{
				if (gameplayEventInstance != null && gameplayEventInstance.smi is MeteorShowerEvent.StatesInstance)
				{
					MeteorShowerEvent.StatesInstance statesInstance = gameplayEventInstance.smi as MeteorShowerEvent.StatesInstance;
					if (statesInstance.IsInsideState(statesInstance.sm.running.bombarding))
					{
						flag = true;
						s = "idle_loop";
						break;
					}
				}
			}
			animController.Play(s, flag ? KAnim.PlayMode.Loop : KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x0600535E RID: 21342 RVA: 0x001DE3C8 File Offset: 0x001DC5C8
	public void OnFogOfWarRevealed(object data = null)
	{
		if (data == null)
		{
			return;
		}
		if ((AxialI)data != this.m_location)
		{
			return;
		}
		if (!ClusterGrid.Instance.IsCellVisible(base.Location))
		{
			return;
		}
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			WorldDetectedMessage message = new WorldDetectedMessage(this.m_worldContainer);
			MusicManager.instance.PlaySong("Stinger_WorldDetected", false);
			Messenger.Instance.QueueMessage(message);
			if (!this.m_worldContainer.IsDiscovered)
			{
				using (IEnumerator enumerator = Components.Clustercrafts.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (((Clustercraft)enumerator.Current).GetOrbitAsteroid() == this)
						{
							this.m_worldContainer.SetDiscovered(true);
							break;
						}
					}
				}
			}
		}
	}

	// Token: 0x040036EA RID: 14058
	public static string DEFAULT_ASTEROID_ICON_ANIM = "asteroid_sandstone_start_kanim";

	// Token: 0x040036EB RID: 14059
	[MyCmpReq]
	private WorldContainer m_worldContainer;

	// Token: 0x040036EC RID: 14060
	[Serialize]
	private string m_name;

	// Token: 0x040036ED RID: 14061
	[Serialize]
	private string m_asteroidAnim;
}
