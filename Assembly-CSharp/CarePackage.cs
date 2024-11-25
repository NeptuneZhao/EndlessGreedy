﻿using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000690 RID: 1680
public class CarePackage : StateMachineComponent<CarePackage.SMInstance>
{
	// Token: 0x060029E9 RID: 10729 RVA: 0x000EC39C File Offset: 0x000EA59C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		if (this.info != null)
		{
			this.SetAnimToInfo();
		}
		this.reactable = this.CreateReactable();
	}

	// Token: 0x060029EA RID: 10730 RVA: 0x000EC3CC File Offset: 0x000EA5CC
	public Reactable CreateReactable()
	{
		return new EmoteReactable(base.gameObject, "UpgradeFX", Db.Get().ChoreTypes.Emote, 15, 8, 0f, 20f, float.PositiveInfinity, 0f).SetEmote(Db.Get().Emotes.Minion.Cheer);
	}

	// Token: 0x060029EB RID: 10731 RVA: 0x000EC42D File Offset: 0x000EA62D
	protected override void OnCleanUp()
	{
		this.reactable.Cleanup();
		base.OnCleanUp();
	}

	// Token: 0x060029EC RID: 10732 RVA: 0x000EC440 File Offset: 0x000EA640
	public void SetInfo(CarePackageInfo info)
	{
		this.info = info;
		this.SetAnimToInfo();
	}

	// Token: 0x060029ED RID: 10733 RVA: 0x000EC44F File Offset: 0x000EA64F
	public void SetFacade(string facadeID)
	{
		this.facadeID = facadeID;
		this.SetAnimToInfo();
	}

	// Token: 0x060029EE RID: 10734 RVA: 0x000EC460 File Offset: 0x000EA660
	private void SetAnimToInfo()
	{
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("Meter".ToTag()), base.gameObject, null);
		GameObject prefab = Assets.GetPrefab(this.info.id);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		KBatchedAnimController component2 = prefab.GetComponent<KBatchedAnimController>();
		SymbolOverrideController component3 = prefab.GetComponent<SymbolOverrideController>();
		KBatchedAnimController component4 = gameObject.GetComponent<KBatchedAnimController>();
		component4.transform.SetLocalPosition(Vector3.forward);
		component4.AnimFiles = component2.AnimFiles;
		component4.isMovable = true;
		component4.animWidth = component2.animWidth;
		component4.animHeight = component2.animHeight;
		if (component3 != null)
		{
			SymbolOverrideController symbolOverrideController = SymbolOverrideControllerUtil.AddToPrefab(gameObject);
			foreach (SymbolOverrideController.SymbolEntry symbolEntry in component3.GetSymbolOverrides)
			{
				symbolOverrideController.AddSymbolOverride(symbolEntry.targetSymbol, symbolEntry.sourceSymbol, 0);
			}
		}
		component4.initialAnim = component2.initialAnim;
		component4.initialMode = KAnim.PlayMode.Loop;
		if (!string.IsNullOrEmpty(this.facadeID))
		{
			component4.SwapAnims(new KAnimFile[]
			{
				Db.GetEquippableFacades().Get(this.facadeID).AnimFile
			});
			base.GetComponentsInChildren<KBatchedAnimController>()[1].SetSymbolVisiblity("object", false);
		}
		KBatchedAnimTracker component5 = gameObject.GetComponent<KBatchedAnimTracker>();
		component5.controller = component;
		component5.symbol = new HashedString("snapTO_object");
		component5.offset = new Vector3(0f, 0.5f, 0f);
		gameObject.SetActive(true);
		component.SetSymbolVisiblity("snapTO_object", false);
		new KAnimLink(component, component4);
	}

	// Token: 0x060029EF RID: 10735 RVA: 0x000EC600 File Offset: 0x000EA800
	private void SpawnContents()
	{
		if (this.info == null)
		{
			global::Debug.LogWarning("CarePackage has no data to spawn from. Probably a save from before the CarePackage info data was serialized.");
			return;
		}
		GameObject gameObject = null;
		GameObject prefab = Assets.GetPrefab(this.info.id);
		Element element = ElementLoader.GetElement(this.info.id.ToTag());
		Vector3 position = base.transform.position + Vector3.up / 2f;
		if (element == null && prefab != null)
		{
			int num = 0;
			while ((float)num < this.info.quantity)
			{
				gameObject = Util.KInstantiate(prefab, position);
				if (gameObject != null)
				{
					if (!this.facadeID.IsNullOrWhiteSpace())
					{
						EquippableFacade.AddFacadeToEquippable(gameObject.GetComponent<Equippable>(), this.facadeID);
					}
					gameObject.SetActive(true);
				}
				num++;
			}
		}
		else if (element != null)
		{
			float quantity = this.info.quantity;
			gameObject = element.substance.SpawnResource(position, quantity, element.defaultValues.temperature, byte.MaxValue, 0, false, true, false);
		}
		else
		{
			global::Debug.LogWarning("Can't find spawnable thing from tag " + this.info.id);
		}
		if (gameObject != null)
		{
			gameObject.SetActive(true);
		}
	}

	// Token: 0x04001824 RID: 6180
	[Serialize]
	public CarePackageInfo info;

	// Token: 0x04001825 RID: 6181
	private string facadeID;

	// Token: 0x04001826 RID: 6182
	private Reactable reactable;

	// Token: 0x02001478 RID: 5240
	public class SMInstance : GameStateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage, object>.GameInstance
	{
		// Token: 0x06008ABF RID: 35519 RVA: 0x00334979 File Offset: 0x00332B79
		public SMInstance(CarePackage master) : base(master)
		{
		}

		// Token: 0x040069D8 RID: 27096
		public List<Chore> activeUseChores;
	}

	// Token: 0x02001479 RID: 5241
	public class States : GameStateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage>
	{
		// Token: 0x06008AC0 RID: 35520 RVA: 0x00334984 File Offset: 0x00332B84
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.spawn;
			base.serializable = StateMachine.SerializeType.ParamsOnly;
			this.spawn.PlayAnim("portalbirth").OnAnimQueueComplete(this.open).ParamTransition<bool>(this.spawnedContents, this.pst, GameStateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage, object>.IsTrue);
			this.open.PlayAnim("portalbirth_pst").QueueAnim("object_idle_loop", false, null).Exit(delegate(CarePackage.SMInstance smi)
			{
				smi.master.SpawnContents();
				this.spawnedContents.Set(true, smi, false);
			}).ScheduleGoTo(1f, this.pst);
			this.pst.PlayAnim("object_idle_pst").ScheduleGoTo(5f, this.destroy);
			this.destroy.Enter(delegate(CarePackage.SMInstance smi)
			{
				Util.KDestroyGameObject(smi.master.gameObject);
			});
		}

		// Token: 0x040069D9 RID: 27097
		public StateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage, object>.BoolParameter spawnedContents;

		// Token: 0x040069DA RID: 27098
		public GameStateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage, object>.State spawn;

		// Token: 0x040069DB RID: 27099
		public GameStateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage, object>.State open;

		// Token: 0x040069DC RID: 27100
		public GameStateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage, object>.State pst;

		// Token: 0x040069DD RID: 27101
		public GameStateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage, object>.State destroy;
	}
}
