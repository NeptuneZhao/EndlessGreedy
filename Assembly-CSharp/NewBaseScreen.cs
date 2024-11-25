using System;
using System.Runtime.CompilerServices;
using FMODUnity;
using Klei.AI;
using ProcGenGame;
using UnityEngine;

// Token: 0x02000BA5 RID: 2981
public class NewBaseScreen : KScreen
{
	// Token: 0x06005A4B RID: 23115 RVA: 0x0020C052 File Offset: 0x0020A252
	public override float GetSortKey()
	{
		return 1f;
	}

	// Token: 0x06005A4C RID: 23116 RVA: 0x0020C059 File Offset: 0x0020A259
	protected override void OnPrefabInit()
	{
		NewBaseScreen.Instance = this;
		base.OnPrefabInit();
		TimeOfDay.Instance.SetScale(0f);
	}

	// Token: 0x06005A4D RID: 23117 RVA: 0x0020C076 File Offset: 0x0020A276
	protected override void OnForcedCleanUp()
	{
		NewBaseScreen.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06005A4E RID: 23118 RVA: 0x0020C084 File Offset: 0x0020A284
	public static Vector2I SetInitialCamera()
	{
		Vector2I vector2I = SaveLoader.Instance.cachedGSD.baseStartPos;
		vector2I += ClusterManager.Instance.GetStartWorld().WorldOffset;
		Vector3 pos = Grid.CellToPosCCC(Grid.OffsetCell(Grid.OffsetCell(0, vector2I.x, vector2I.y), 0, -2), Grid.SceneLayer.Background);
		CameraController.Instance.SetMaxOrthographicSize(40f);
		CameraController.Instance.SnapTo(pos);
		CameraController.Instance.SetTargetPos(pos, 20f, false);
		CameraController.Instance.OrthographicSize = 40f;
		CameraSaveData.valid = false;
		return vector2I;
	}

	// Token: 0x06005A4F RID: 23119 RVA: 0x0020C11C File Offset: 0x0020A31C
	protected override void OnActivate()
	{
		if (this.disabledUIElements != null)
		{
			foreach (CanvasGroup canvasGroup in this.disabledUIElements)
			{
				if (canvasGroup != null)
				{
					canvasGroup.interactable = false;
				}
			}
		}
		NewBaseScreen.SetInitialCamera();
		if (SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Unpause(false);
		}
		this.Final();
	}

	// Token: 0x06005A50 RID: 23120 RVA: 0x0020C17D File Offset: 0x0020A37D
	public void Init(Cluster clusterLayout, ITelepadDeliverable[] startingMinionStats)
	{
		this.m_clusterLayout = clusterLayout;
		this.m_minionStartingStats = startingMinionStats;
	}

	// Token: 0x06005A51 RID: 23121 RVA: 0x0020C190 File Offset: 0x0020A390
	protected override void OnDeactivate()
	{
		Game.Instance.Trigger(-122303817, null);
		if (this.disabledUIElements != null)
		{
			foreach (CanvasGroup canvasGroup in this.disabledUIElements)
			{
				if (canvasGroup != null)
				{
					canvasGroup.interactable = true;
				}
			}
		}
	}

	// Token: 0x06005A52 RID: 23122 RVA: 0x0020C1E0 File Offset: 0x0020A3E0
	public override void OnKeyDown(KButtonEvent e)
	{
		global::Action[] array = new global::Action[4];
		RuntimeHelpers.InitializeArray(array, fieldof(<PrivateImplementationDetails>.46E7A7E6CE942EAE1E13925BEDED6E6321F99918099A108FDB32BB9510B8E88D).FieldHandle);
		global::Action[] array2 = array;
		if (!e.Consumed)
		{
			int num = 0;
			while (num < array2.Length && !e.TryConsume(array2[num]))
			{
				num++;
			}
		}
	}

	// Token: 0x06005A53 RID: 23123 RVA: 0x0020C220 File Offset: 0x0020A420
	private void Final()
	{
		SpeedControlScreen.Instance.Unpause(false);
		GameObject telepad = GameUtil.GetTelepad(ClusterManager.Instance.GetStartWorld().id);
		if (telepad)
		{
			this.SpawnMinions(Grid.PosToCell(telepad));
		}
		Game.Instance.baseAlreadyCreated = true;
		this.Deactivate();
	}

	// Token: 0x06005A54 RID: 23124 RVA: 0x0020C274 File Offset: 0x0020A474
	private void SpawnMinions(int headquartersCell)
	{
		if (headquartersCell == -1)
		{
			global::Debug.LogWarning("No headquarters in saved base template. Cannot place minions. Confirm there is a headquarters saved to the base template, or consider creating a new one.");
			return;
		}
		int num;
		int num2;
		Grid.CellToXY(headquartersCell, out num, out num2);
		if (Grid.WidthInCells < 64)
		{
			return;
		}
		int baseLeft = this.m_clusterLayout.currentWorld.BaseLeft;
		int baseRight = this.m_clusterLayout.currentWorld.BaseRight;
		Effect a_new_hope = Db.Get().effects.Get("AnewHope");
		Action<object> <>9__0;
		for (int i = 0; i < this.m_minionStartingStats.Length; i++)
		{
			MinionStartingStats minionStartingStats = (MinionStartingStats)this.m_minionStartingStats[i];
			int x = num + i % (baseRight - baseLeft) + 1;
			int y = num2;
			int cell = Grid.XYToCell(x, y);
			GameObject prefab = Assets.GetPrefab(BaseMinionConfig.GetMinionIDForModel(minionStartingStats.personality.model));
			GameObject gameObject = Util.KInstantiate(prefab, null, null);
			gameObject.name = prefab.name;
			Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
			gameObject.transform.SetLocalPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
			gameObject.SetActive(true);
			minionStartingStats.Apply(gameObject);
			GameScheduler instance = GameScheduler.Instance;
			string name = "ANewHope";
			float time = 3f + 0.5f * (float)i;
			Action<object> callback;
			if ((callback = <>9__0) == null)
			{
				callback = (<>9__0 = delegate(object m)
				{
					GameObject gameObject2 = m as GameObject;
					if (gameObject2 == null)
					{
						return;
					}
					gameObject2.GetComponent<Effects>().Add(a_new_hope, true);
				});
			}
			instance.Schedule(name, time, callback, gameObject, null);
			if (minionStartingStats.personality.model == GameTags.Minions.Models.Bionic)
			{
				GameScheduler.Instance.Schedule("ExtraPowerBanks", 3f + 4.5f * (float)i, delegate(object m)
				{
					GameUtil.GetTelepad(ClusterManager.Instance.GetStartWorld().id).Trigger(1982288670, null);
				}, gameObject, null);
			}
		}
		ClusterManager.Instance.activeWorld.SetDupeVisited();
	}

	// Token: 0x04003B59 RID: 15193
	public static NewBaseScreen Instance;

	// Token: 0x04003B5A RID: 15194
	[SerializeField]
	private CanvasGroup[] disabledUIElements;

	// Token: 0x04003B5B RID: 15195
	public EventReference ScanSoundMigrated;

	// Token: 0x04003B5C RID: 15196
	public EventReference BuildBaseSoundMigrated;

	// Token: 0x04003B5D RID: 15197
	private ITelepadDeliverable[] m_minionStartingStats;

	// Token: 0x04003B5E RID: 15198
	private Cluster m_clusterLayout;
}
