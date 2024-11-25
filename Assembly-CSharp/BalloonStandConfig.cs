using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020002AA RID: 682
public class BalloonStandConfig : IEntityConfig
{
	// Token: 0x06000E20 RID: 3616 RVA: 0x0005173C File Offset: 0x0004F93C
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000E21 RID: 3617 RVA: 0x00051744 File Offset: 0x0004F944
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(BalloonStandConfig.ID, BalloonStandConfig.ID, false);
		KAnimFile[] overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_balloon_receiver_kanim")
		};
		GetBalloonWorkable getBalloonWorkable = gameObject.AddOrGet<GetBalloonWorkable>();
		getBalloonWorkable.workTime = 2f;
		getBalloonWorkable.workLayer = Grid.SceneLayer.BuildingFront;
		getBalloonWorkable.overrideAnims = overrideAnims;
		getBalloonWorkable.synchronizeAnims = false;
		return gameObject;
	}

	// Token: 0x06000E22 RID: 3618 RVA: 0x000517A2 File Offset: 0x0004F9A2
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000E23 RID: 3619 RVA: 0x000517A4 File Offset: 0x0004F9A4
	public void OnSpawn(GameObject inst)
	{
		GetBalloonWorkable component = inst.GetComponent<GetBalloonWorkable>();
		WorkChore<GetBalloonWorkable> workChore = new WorkChore<GetBalloonWorkable>(Db.Get().ChoreTypes.JoyReaction, component, null, true, new Action<Chore>(this.MakeNewBalloonChore), null, null, true, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, true, PriorityScreen.PriorityClass.high, 5, true, true);
		workChore.AddPrecondition(this.HasNoBalloon, workChore);
		workChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
		component.GetBalloonArtist().NextBalloonOverride();
	}

	// Token: 0x06000E24 RID: 3620 RVA: 0x00051824 File Offset: 0x0004FA24
	private void MakeNewBalloonChore(Chore chore)
	{
		GetBalloonWorkable component = chore.target.GetComponent<GetBalloonWorkable>();
		WorkChore<GetBalloonWorkable> workChore = new WorkChore<GetBalloonWorkable>(Db.Get().ChoreTypes.JoyReaction, component, null, true, new Action<Chore>(this.MakeNewBalloonChore), null, null, true, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, true, PriorityScreen.PriorityClass.high, 5, true, true);
		workChore.AddPrecondition(this.HasNoBalloon, workChore);
		workChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
		component.GetBalloonArtist().NextBalloonOverride();
	}

	// Token: 0x06000E25 RID: 3621 RVA: 0x000518A8 File Offset: 0x0004FAA8
	public BalloonStandConfig()
	{
		Chore.Precondition hasNoBalloon = default(Chore.Precondition);
		hasNoBalloon.id = "HasNoBalloon";
		hasNoBalloon.description = "__ Duplicant doesn't have a balloon already";
		hasNoBalloon.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return !(context.consumerState.consumer == null) && !context.consumerState.gameObject.GetComponent<Effects>().HasEffect("HasBalloon");
		};
		this.HasNoBalloon = hasNoBalloon;
		base..ctor();
	}

	// Token: 0x040008DE RID: 2270
	public static readonly string ID = "BalloonStand";

	// Token: 0x040008DF RID: 2271
	private Chore.Precondition HasNoBalloon;
}
