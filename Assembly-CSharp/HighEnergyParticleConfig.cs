using System;
using STRINGS;
using UnityEngine;

// Token: 0x020008EC RID: 2284
public class HighEnergyParticleConfig : IEntityConfig
{
	// Token: 0x060041A0 RID: 16800 RVA: 0x00174A7A File Offset: 0x00172C7A
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060041A1 RID: 16801 RVA: 0x00174A84 File Offset: 0x00172C84
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateBasicEntity("HighEnergyParticle", ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME, ITEMS.RADIATION.HIGHENERGYPARITCLE.DESC, 1f, false, Assets.GetAnim("spark_radial_high_energy_particles_kanim"), "travel_pre", Grid.SceneLayer.FXFront2, SimHashes.Creature, null, 293f);
		EntityTemplates.AddCollision(gameObject, EntityTemplates.CollisionShape.CIRCLE, 0.2f, 0.2f);
		gameObject.AddOrGet<LoopingSounds>();
		RadiationEmitter radiationEmitter = gameObject.AddOrGet<RadiationEmitter>();
		radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
		radiationEmitter.radiusProportionalToRads = false;
		radiationEmitter.emitRadiusX = 3;
		radiationEmitter.emitRadiusY = 3;
		radiationEmitter.emitRads = 0.4f * ((float)radiationEmitter.emitRadiusX / 6f);
		gameObject.AddComponent<HighEnergyParticle>().speed = 8f;
		return gameObject;
	}

	// Token: 0x060041A2 RID: 16802 RVA: 0x00174B3B File Offset: 0x00172D3B
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060041A3 RID: 16803 RVA: 0x00174B3D File Offset: 0x00172D3D
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04002B78 RID: 11128
	public const int PARTICLE_SPEED = 8;

	// Token: 0x04002B79 RID: 11129
	public const float PARTICLE_COLLISION_SIZE = 0.2f;

	// Token: 0x04002B7A RID: 11130
	public const float PER_CELL_FALLOFF = 0.1f;

	// Token: 0x04002B7B RID: 11131
	public const float FALLOUT_RATIO = 0.5f;

	// Token: 0x04002B7C RID: 11132
	public const int MAX_PAYLOAD = 500;

	// Token: 0x04002B7D RID: 11133
	public const int EXPLOSION_FALLOUT_TEMPERATURE = 5000;

	// Token: 0x04002B7E RID: 11134
	public const float EXPLOSION_FALLOUT_MASS_PER_PARTICLE = 0.001f;

	// Token: 0x04002B7F RID: 11135
	public const float EXPLOSION_EMIT_DURRATION = 1f;

	// Token: 0x04002B80 RID: 11136
	public const short EXPLOSION_EMIT_RADIUS = 6;

	// Token: 0x04002B81 RID: 11137
	public const string ID = "HighEnergyParticle";
}
