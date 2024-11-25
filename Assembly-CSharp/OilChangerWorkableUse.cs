using System;
using Klei;
using Klei.AI;
using UnityEngine;

// Token: 0x0200073E RID: 1854
public class OilChangerWorkableUse : Workable, IGameObjectEffectDescriptor
{
	// Token: 0x06003152 RID: 12626 RVA: 0x0010FFF0 File Offset: 0x0010E1F0
	private OilChangerWorkableUse()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06003153 RID: 12627 RVA: 0x00110000 File Offset: 0x0010E200
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.attributeConverter = Db.Get().AttributeConverters.ToiletSpeed;
		base.SetWorkTime(8.5f);
	}

	// Token: 0x06003154 RID: 12628 RVA: 0x00110038 File Offset: 0x0010E238
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		if (worker != null)
		{
			Vector3 position = worker.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
			worker.transform.SetPosition(position);
		}
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
		if (roomOfGameObject != null)
		{
			roomOfGameObject.roomType.TriggerRoomEffects(base.GetComponent<KPrefabID>(), worker.GetComponent<Effects>());
		}
	}

	// Token: 0x06003155 RID: 12629 RVA: 0x001100AC File Offset: 0x0010E2AC
	protected override void OnStopWork(WorkerBase worker)
	{
		if (worker != null)
		{
			Vector3 position = worker.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
			worker.transform.SetPosition(position);
		}
		base.OnStopWork(worker);
	}

	// Token: 0x06003156 RID: 12630 RVA: 0x001100F0 File Offset: 0x0010E2F0
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Storage component = base.GetComponent<Storage>();
		BionicOilMonitor.Instance smi = worker.GetSMI<BionicOilMonitor.Instance>();
		if (smi != null)
		{
			float b = 200f - smi.CurrentOilMass;
			float num = Mathf.Min(component.GetMassAvailable(GameTags.LubricatingOil), b);
			float num2 = num;
			float num3 = 0f;
			Storage component2 = base.GetComponent<Storage>();
			SimHashes simHashes = SimHashes.CrudeOil;
			foreach (SimHashes simHashes2 in BionicOilMonitor.LUBRICANT_TYPE_EFFECT.Keys)
			{
				float num4;
				SimUtil.DiseaseInfo diseaseInfo;
				float num5;
				component2.ConsumeAndGetDisease(simHashes2.CreateTag(), num2, out num4, out diseaseInfo, out num5);
				if (num4 > num3)
				{
					simHashes = simHashes2;
					num3 = num4;
				}
				num2 -= num4;
			}
			base.GetComponent<Storage>().ConsumeIgnoringDisease(GameTags.LubricatingOil, num2);
			smi.RefillOil(num);
			Effects component3 = worker.GetComponent<Effects>();
			foreach (SimHashes simHashes3 in BionicOilMonitor.LUBRICANT_TYPE_EFFECT.Keys)
			{
				Effect effect = BionicOilMonitor.LUBRICANT_TYPE_EFFECT[simHashes3];
				if (simHashes == simHashes3)
				{
					component3.Add(effect, true);
				}
				else
				{
					component3.Remove(effect);
				}
			}
		}
		base.OnCompleteWork(worker);
	}
}
