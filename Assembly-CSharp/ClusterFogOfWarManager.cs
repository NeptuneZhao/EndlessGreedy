using System;
using System.Collections.Generic;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x020007B8 RID: 1976
public class ClusterFogOfWarManager : GameStateMachine<ClusterFogOfWarManager, ClusterFogOfWarManager.Instance, IStateMachineTarget, ClusterFogOfWarManager.Def>
{
	// Token: 0x06003656 RID: 13910 RVA: 0x00127A88 File Offset: 0x00125C88
	public override void InitializeStates(out StateMachine.BaseState defaultState)
	{
		defaultState = this.root;
		this.root.Enter(delegate(ClusterFogOfWarManager.Instance smi)
		{
			smi.Initialize();
		}).EventHandler(GameHashes.DiscoveredWorldsChanged, (ClusterFogOfWarManager.Instance smi) => Game.Instance, delegate(ClusterFogOfWarManager.Instance smi)
		{
			smi.UpdateRevealedCellsFromDiscoveredWorlds();
		});
	}

	// Token: 0x0400203B RID: 8251
	public const int AUTOMATIC_PEEK_RADIUS = 2;

	// Token: 0x02001678 RID: 5752
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001679 RID: 5753
	public new class Instance : GameStateMachine<ClusterFogOfWarManager, ClusterFogOfWarManager.Instance, IStateMachineTarget, ClusterFogOfWarManager.Def>.GameInstance
	{
		// Token: 0x0600926F RID: 37487 RVA: 0x00353EA8 File Offset: 0x003520A8
		public Instance(IStateMachineTarget master, ClusterFogOfWarManager.Def def) : base(master, def)
		{
		}

		// Token: 0x06009270 RID: 37488 RVA: 0x00353EBD File Offset: 0x003520BD
		public void Initialize()
		{
			this.UpdateRevealedCellsFromDiscoveredWorlds();
			this.EnsureRevealedTilesHavePeek();
		}

		// Token: 0x06009271 RID: 37489 RVA: 0x00353ECB File Offset: 0x003520CB
		public ClusterRevealLevel GetCellRevealLevel(AxialI location)
		{
			if (this.GetRevealCompleteFraction(location) >= 1f)
			{
				return ClusterRevealLevel.Visible;
			}
			if (this.GetRevealCompleteFraction(location) > 0f)
			{
				return ClusterRevealLevel.Peeked;
			}
			return ClusterRevealLevel.Hidden;
		}

		// Token: 0x06009272 RID: 37490 RVA: 0x00353EEE File Offset: 0x003520EE
		public void DEBUG_REVEAL_ENTIRE_MAP()
		{
			this.RevealLocation(AxialI.ZERO, 100);
		}

		// Token: 0x06009273 RID: 37491 RVA: 0x00353EFD File Offset: 0x003520FD
		public bool IsLocationRevealed(AxialI location)
		{
			return this.GetRevealCompleteFraction(location) >= 1f;
		}

		// Token: 0x06009274 RID: 37492 RVA: 0x00353F10 File Offset: 0x00352110
		private void EnsureRevealedTilesHavePeek()
		{
			foreach (KeyValuePair<AxialI, List<ClusterGridEntity>> keyValuePair in ClusterGrid.Instance.cellContents)
			{
				if (this.IsLocationRevealed(keyValuePair.Key))
				{
					this.PeekLocation(keyValuePair.Key, 2);
				}
			}
		}

		// Token: 0x06009275 RID: 37493 RVA: 0x00353F80 File Offset: 0x00352180
		public void PeekLocation(AxialI location, int radius)
		{
			foreach (AxialI key in AxialUtil.GetAllPointsWithinRadius(location, radius))
			{
				if (this.m_revealPointsByCell.ContainsKey(key))
				{
					this.m_revealPointsByCell[key] = Mathf.Max(this.m_revealPointsByCell[key], 0.01f);
				}
				else
				{
					this.m_revealPointsByCell[key] = 0.01f;
				}
			}
		}

		// Token: 0x06009276 RID: 37494 RVA: 0x00354010 File Offset: 0x00352210
		public void RevealLocation(AxialI location, int radius = 0)
		{
			if (ClusterGrid.Instance.GetHiddenEntitiesOfLayerAtCell(location, EntityLayer.Asteroid).Count > 0 || ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(location, EntityLayer.Asteroid) != null)
			{
				radius = Mathf.Max(radius, 1);
			}
			bool flag = false;
			foreach (AxialI cell in AxialUtil.GetAllPointsWithinRadius(location, radius))
			{
				flag |= this.RevealCellIfValid(cell);
			}
			if (flag)
			{
				Game.Instance.Trigger(-1991583975, location);
			}
		}

		// Token: 0x06009277 RID: 37495 RVA: 0x003540B4 File Offset: 0x003522B4
		public void EarnRevealPointsForLocation(AxialI location, float points)
		{
			global::Debug.Assert(ClusterGrid.Instance.IsValidCell(location), string.Format("EarnRevealPointsForLocation called with invalid location: {0}", location));
			if (this.IsLocationRevealed(location))
			{
				return;
			}
			if (this.m_revealPointsByCell.ContainsKey(location))
			{
				Dictionary<AxialI, float> revealPointsByCell = this.m_revealPointsByCell;
				revealPointsByCell[location] += points;
			}
			else
			{
				this.m_revealPointsByCell[location] = points;
				Game.Instance.Trigger(-1554423969, location);
			}
			if (this.IsLocationRevealed(location))
			{
				this.RevealLocation(location, 0);
				this.PeekLocation(location, 2);
				Game.Instance.Trigger(-1991583975, location);
			}
		}

		// Token: 0x06009278 RID: 37496 RVA: 0x00354164 File Offset: 0x00352364
		public float GetRevealCompleteFraction(AxialI location)
		{
			if (!ClusterGrid.Instance.IsValidCell(location))
			{
				global::Debug.LogError(string.Format("GetRevealCompleteFraction called with invalid location: {0}, {1}", location.r, location.q));
			}
			if (DebugHandler.RevealFogOfWar)
			{
				return 1f;
			}
			float num;
			if (this.m_revealPointsByCell.TryGetValue(location, out num))
			{
				return Mathf.Min(num / ROCKETRY.CLUSTER_FOW.POINTS_TO_REVEAL, 1f);
			}
			return 0f;
		}

		// Token: 0x06009279 RID: 37497 RVA: 0x003541D7 File Offset: 0x003523D7
		private bool RevealCellIfValid(AxialI cell)
		{
			if (!ClusterGrid.Instance.IsValidCell(cell))
			{
				return false;
			}
			if (this.IsLocationRevealed(cell))
			{
				return false;
			}
			this.m_revealPointsByCell[cell] = ROCKETRY.CLUSTER_FOW.POINTS_TO_REVEAL;
			this.PeekLocation(cell, 2);
			return true;
		}

		// Token: 0x0600927A RID: 37498 RVA: 0x00354210 File Offset: 0x00352410
		public bool GetUnrevealedLocationWithinRadius(AxialI center, int radius, out AxialI result)
		{
			for (int i = 0; i <= radius; i++)
			{
				foreach (AxialI axialI in AxialUtil.GetRing(center, i))
				{
					if (ClusterGrid.Instance.IsValidCell(axialI) && !this.IsLocationRevealed(axialI))
					{
						result = axialI;
						return true;
					}
				}
			}
			result = AxialI.ZERO;
			return false;
		}

		// Token: 0x0600927B RID: 37499 RVA: 0x00354298 File Offset: 0x00352498
		public void UpdateRevealedCellsFromDiscoveredWorlds()
		{
			int radius = DlcManager.IsExpansion1Active() ? 0 : 2;
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				if (worldContainer.IsDiscovered && !DebugHandler.RevealFogOfWar)
				{
					this.RevealLocation(worldContainer.GetComponent<ClusterGridEntity>().Location, radius);
				}
			}
		}

		// Token: 0x04006FC9 RID: 28617
		[Serialize]
		private Dictionary<AxialI, float> m_revealPointsByCell = new Dictionary<AxialI, float>();
	}
}
