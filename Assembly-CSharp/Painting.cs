using System;

// Token: 0x0200059E RID: 1438
public class Painting : Artable
{
	// Token: 0x060021EB RID: 8683 RVA: 0x000BCC58 File Offset: 0x000BAE58
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.multitoolContext = "paint";
		this.multitoolHitEffectTag = "fx_paint_splash";
	}

	// Token: 0x060021EC RID: 8684 RVA: 0x000BCC8B File Offset: 0x000BAE8B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Paintings.Add(this);
	}

	// Token: 0x060021ED RID: 8685 RVA: 0x000BCC9E File Offset: 0x000BAE9E
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.Paintings.Remove(this);
	}

	// Token: 0x060021EE RID: 8686 RVA: 0x000BCCB1 File Offset: 0x000BAEB1
	public override void SetStage(string stage_id, bool skip_effect)
	{
		base.SetStage(stage_id, skip_effect);
		if (Db.GetArtableStages().Get(stage_id) == null)
		{
			Debug.LogError("Missing stage: " + stage_id);
		}
	}
}
