using System;
using System.Diagnostics;
using Steamworks;
using UnityEngine;

// Token: 0x02000B6C RID: 2924
public class SteamAchievementService : MonoBehaviour
{
	// Token: 0x17000698 RID: 1688
	// (get) Token: 0x060057D1 RID: 22481 RVA: 0x001F9E1F File Offset: 0x001F801F
	public static SteamAchievementService Instance
	{
		get
		{
			return SteamAchievementService.instance;
		}
	}

	// Token: 0x060057D2 RID: 22482 RVA: 0x001F9E28 File Offset: 0x001F8028
	public static void Initialize()
	{
		if (SteamAchievementService.instance == null)
		{
			GameObject gameObject = GameObject.Find("/SteamManager");
			SteamAchievementService.instance = gameObject.GetComponent<SteamAchievementService>();
			if (SteamAchievementService.instance == null)
			{
				SteamAchievementService.instance = gameObject.AddComponent<SteamAchievementService>();
			}
		}
	}

	// Token: 0x060057D3 RID: 22483 RVA: 0x001F9E70 File Offset: 0x001F8070
	public void Awake()
	{
		this.setupComplete = false;
		global::Debug.Assert(SteamAchievementService.instance == null);
		SteamAchievementService.instance = this;
	}

	// Token: 0x060057D4 RID: 22484 RVA: 0x001F9E8F File Offset: 0x001F808F
	private void OnDestroy()
	{
		global::Debug.Assert(SteamAchievementService.instance == this);
		SteamAchievementService.instance = null;
	}

	// Token: 0x060057D5 RID: 22485 RVA: 0x001F9EA7 File Offset: 0x001F80A7
	private void Update()
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		if (Game.Instance != null)
		{
			return;
		}
		if (!this.setupComplete && DistributionPlatform.Initialized)
		{
			this.Setup();
		}
	}

	// Token: 0x060057D6 RID: 22486 RVA: 0x001F9ED4 File Offset: 0x001F80D4
	private void Setup()
	{
		this.cbUserStatsReceived = Callback<UserStatsReceived_t>.Create(new Callback<UserStatsReceived_t>.DispatchDelegate(this.OnUserStatsReceived));
		this.cbUserStatsStored = Callback<UserStatsStored_t>.Create(new Callback<UserStatsStored_t>.DispatchDelegate(this.OnUserStatsStored));
		this.cbUserAchievementStored = Callback<UserAchievementStored_t>.Create(new Callback<UserAchievementStored_t>.DispatchDelegate(this.OnUserAchievementStored));
		this.setupComplete = true;
		this.RefreshStats();
	}

	// Token: 0x060057D7 RID: 22487 RVA: 0x001F9F33 File Offset: 0x001F8133
	private void RefreshStats()
	{
		SteamUserStats.RequestCurrentStats();
	}

	// Token: 0x060057D8 RID: 22488 RVA: 0x001F9F3B File Offset: 0x001F813B
	private void OnUserStatsReceived(UserStatsReceived_t data)
	{
		if (data.m_eResult != EResult.k_EResultOK)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"OnUserStatsReceived",
				data.m_eResult,
				data.m_steamIDUser
			});
			return;
		}
	}

	// Token: 0x060057D9 RID: 22489 RVA: 0x001F9F76 File Offset: 0x001F8176
	private void OnUserStatsStored(UserStatsStored_t data)
	{
		if (data.m_eResult != EResult.k_EResultOK)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"OnUserStatsStored",
				data.m_eResult
			});
			return;
		}
	}

	// Token: 0x060057DA RID: 22490 RVA: 0x001F9FA3 File Offset: 0x001F81A3
	private void OnUserAchievementStored(UserAchievementStored_t data)
	{
	}

	// Token: 0x060057DB RID: 22491 RVA: 0x001F9FA8 File Offset: 0x001F81A8
	public void Unlock(string achievement_id)
	{
		bool flag = SteamUserStats.SetAchievement(achievement_id);
		global::Debug.LogFormat("SetAchievement {0} {1}", new object[]
		{
			achievement_id,
			flag
		});
		bool flag2 = SteamUserStats.StoreStats();
		global::Debug.LogFormat("StoreStats {0}", new object[]
		{
			flag2
		});
	}

	// Token: 0x060057DC RID: 22492 RVA: 0x001F9FF8 File Offset: 0x001F81F8
	[Conditional("UNITY_EDITOR")]
	[ContextMenu("Reset All Achievements")]
	private void ResetAllAchievements()
	{
		bool flag = SteamUserStats.ResetAllStats(true);
		global::Debug.LogFormat("ResetAllStats {0}", new object[]
		{
			flag
		});
		if (flag)
		{
			this.RefreshStats();
		}
	}

	// Token: 0x04003964 RID: 14692
	private Callback<UserStatsReceived_t> cbUserStatsReceived;

	// Token: 0x04003965 RID: 14693
	private Callback<UserStatsStored_t> cbUserStatsStored;

	// Token: 0x04003966 RID: 14694
	private Callback<UserAchievementStored_t> cbUserAchievementStored;

	// Token: 0x04003967 RID: 14695
	private bool setupComplete;

	// Token: 0x04003968 RID: 14696
	private static SteamAchievementService instance;
}
