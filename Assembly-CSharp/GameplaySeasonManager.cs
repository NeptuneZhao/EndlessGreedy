using System;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;
using KSerialization;

// Token: 0x020008C9 RID: 2249
public class GameplaySeasonManager : GameStateMachine<GameplaySeasonManager, GameplaySeasonManager.Instance, IStateMachineTarget, GameplaySeasonManager.Def>
{
	// Token: 0x06003FF1 RID: 16369 RVA: 0x0016A624 File Offset: 0x00168824
	public override void InitializeStates(out StateMachine.BaseState defaultState)
	{
		defaultState = this.root;
		this.root.Enter(delegate(GameplaySeasonManager.Instance smi)
		{
			smi.Initialize();
		}).Update(delegate(GameplaySeasonManager.Instance smi, float dt)
		{
			smi.Update(dt);
		}, UpdateRate.SIM_4000ms, false);
	}

	// Token: 0x020017FB RID: 6139
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020017FC RID: 6140
	public new class Instance : GameStateMachine<GameplaySeasonManager, GameplaySeasonManager.Instance, IStateMachineTarget, GameplaySeasonManager.Def>.GameInstance
	{
		// Token: 0x0600970E RID: 38670 RVA: 0x003640B1 File Offset: 0x003622B1
		public Instance(IStateMachineTarget master, GameplaySeasonManager.Def def) : base(master, def)
		{
			this.activeSeasons = new List<GameplaySeasonInstance>();
		}

		// Token: 0x0600970F RID: 38671 RVA: 0x003640C8 File Offset: 0x003622C8
		public void Initialize()
		{
			this.activeSeasons.RemoveAll((GameplaySeasonInstance item) => item.Season == null);
			List<GameplaySeason> list = new List<GameplaySeason>();
			if (this.m_worldContainer != null)
			{
				ClusterGridEntity component = base.GetComponent<ClusterGridEntity>();
				using (List<string>.Enumerator enumerator = this.m_worldContainer.GetSeasonIds().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string text = enumerator.Current;
						GameplaySeason gameplaySeason = Db.Get().GameplaySeasons.TryGet(text);
						if (gameplaySeason == null)
						{
							Debug.LogWarning("world " + component.name + " has invalid season " + text);
						}
						else
						{
							if (gameplaySeason.type != GameplaySeason.Type.World)
							{
								Debug.LogWarning(string.Concat(new string[]
								{
									"world ",
									component.name,
									" has specified season ",
									text,
									", which is not a world type season"
								}));
							}
							list.Add(gameplaySeason);
						}
					}
					goto IL_146;
				}
			}
			Debug.Assert(base.GetComponent<SaveGame>() != null);
			list = (from season in Db.Get().GameplaySeasons.resources
			where season.type == GameplaySeason.Type.Cluster
			select season).ToList<GameplaySeason>();
			IL_146:
			foreach (GameplaySeason gameplaySeason2 in list)
			{
				if (SaveLoader.Instance.IsDLCActiveForCurrentSave(gameplaySeason2.dlcId) && gameplaySeason2.startActive && !this.SeasonExists(gameplaySeason2) && gameplaySeason2.events.Count > 0)
				{
					this.activeSeasons.Add(gameplaySeason2.Instantiate(this.GetWorldId()));
				}
			}
			foreach (GameplaySeasonInstance gameplaySeasonInstance in new List<GameplaySeasonInstance>(this.activeSeasons))
			{
				if (!list.Contains(gameplaySeasonInstance.Season) || !SaveLoader.Instance.IsDLCActiveForCurrentSave(gameplaySeasonInstance.Season.dlcId))
				{
					this.activeSeasons.Remove(gameplaySeasonInstance);
				}
			}
		}

		// Token: 0x06009710 RID: 38672 RVA: 0x00364328 File Offset: 0x00362528
		private int GetWorldId()
		{
			if (this.m_worldContainer != null)
			{
				return this.m_worldContainer.id;
			}
			return -1;
		}

		// Token: 0x06009711 RID: 38673 RVA: 0x00364348 File Offset: 0x00362548
		public void Update(float dt)
		{
			foreach (GameplaySeasonInstance gameplaySeasonInstance in this.activeSeasons)
			{
				if (gameplaySeasonInstance.ShouldGenerateEvents() && GameUtil.GetCurrentTimeInCycles() > gameplaySeasonInstance.NextEventTime)
				{
					int num = 0;
					while (num < gameplaySeasonInstance.Season.numEventsToStartEachPeriod && gameplaySeasonInstance.StartEvent(false))
					{
						num++;
					}
				}
			}
		}

		// Token: 0x06009712 RID: 38674 RVA: 0x003643C8 File Offset: 0x003625C8
		public void StartNewSeason(GameplaySeason seasonType)
		{
			if (SaveLoader.Instance.IsDLCActiveForCurrentSave(seasonType.dlcId))
			{
				this.activeSeasons.Add(seasonType.Instantiate(this.GetWorldId()));
			}
		}

		// Token: 0x06009713 RID: 38675 RVA: 0x003643F4 File Offset: 0x003625F4
		public bool SeasonExists(GameplaySeason seasonType)
		{
			return this.activeSeasons.Find((GameplaySeasonInstance e) => e.Season.IdHash == seasonType.IdHash) != null;
		}

		// Token: 0x0400749B RID: 29851
		[Serialize]
		public List<GameplaySeasonInstance> activeSeasons;

		// Token: 0x0400749C RID: 29852
		[MyCmpGet]
		private WorldContainer m_worldContainer;
	}
}
