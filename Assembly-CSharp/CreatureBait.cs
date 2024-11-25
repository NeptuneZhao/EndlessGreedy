using System;
using KSerialization;

// Token: 0x02000C1F RID: 3103
[SerializationConfig(MemberSerialization.OptIn)]
public class CreatureBait : StateMachineComponent<CreatureBait.StatesInstance>
{
	// Token: 0x06005F28 RID: 24360 RVA: 0x00235DD7 File Offset: 0x00233FD7
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06005F29 RID: 24361 RVA: 0x00235DE0 File Offset: 0x00233FE0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Tag[] constructionElements = base.GetComponent<Deconstructable>().constructionElements;
		this.baitElement = ((constructionElements.Length > 1) ? constructionElements[1] : constructionElements[0]);
		base.gameObject.GetSMI<Lure.Instance>().SetActiveLures(new Tag[]
		{
			this.baitElement
		});
		base.smi.StartSM();
	}

	// Token: 0x04004001 RID: 16385
	[Serialize]
	public Tag baitElement;

	// Token: 0x02001D08 RID: 7432
	public class StatesInstance : GameStateMachine<CreatureBait.States, CreatureBait.StatesInstance, CreatureBait, object>.GameInstance
	{
		// Token: 0x0600A781 RID: 42881 RVA: 0x0039AAAE File Offset: 0x00398CAE
		public StatesInstance(CreatureBait master) : base(master)
		{
		}
	}

	// Token: 0x02001D09 RID: 7433
	public class States : GameStateMachine<CreatureBait.States, CreatureBait.StatesInstance, CreatureBait>
	{
		// Token: 0x0600A782 RID: 42882 RVA: 0x0039AAB8 File Offset: 0x00398CB8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.ToggleMainStatusItem(Db.Get().BuildingStatusItems.Baited, null).Enter(delegate(CreatureBait.StatesInstance smi)
			{
				KAnim.Build build = ElementLoader.FindElementByName(smi.master.baitElement.ToString()).substance.anim.GetData().build;
				KAnim.Build.Symbol symbol = build.GetSymbol(new KAnimHashedString(build.name));
				HashedString target_symbol = "snapTo_bait";
				smi.GetComponent<SymbolOverrideController>().AddSymbolOverride(target_symbol, symbol, 0);
			}).TagTransition(GameTags.LureUsed, this.destroy, false);
			this.destroy.PlayAnim("use").EventHandler(GameHashes.AnimQueueComplete, delegate(CreatureBait.StatesInstance smi)
			{
				Util.KDestroyGameObject(smi.master.gameObject);
			});
		}

		// Token: 0x040085D7 RID: 34263
		public GameStateMachine<CreatureBait.States, CreatureBait.StatesInstance, CreatureBait, object>.State idle;

		// Token: 0x040085D8 RID: 34264
		public GameStateMachine<CreatureBait.States, CreatureBait.StatesInstance, CreatureBait, object>.State destroy;
	}
}
