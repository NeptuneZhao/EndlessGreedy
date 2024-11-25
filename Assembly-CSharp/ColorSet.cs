using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Token: 0x02000C19 RID: 3097
public class ColorSet : ScriptableObject
{
	// Token: 0x06005F0A RID: 24330 RVA: 0x002352BC File Offset: 0x002334BC
	private void Init()
	{
		if (this.namedLookup == null)
		{
			this.namedLookup = new Dictionary<string, Color32>();
			foreach (FieldInfo fieldInfo in typeof(ColorSet).GetFields())
			{
				if (fieldInfo.FieldType == typeof(Color32))
				{
					this.namedLookup[fieldInfo.Name] = (Color32)fieldInfo.GetValue(this);
				}
			}
		}
	}

	// Token: 0x06005F0B RID: 24331 RVA: 0x00235332 File Offset: 0x00233532
	public Color32 GetColorByName(string name)
	{
		this.Init();
		return this.namedLookup[name];
	}

	// Token: 0x06005F0C RID: 24332 RVA: 0x00235346 File Offset: 0x00233546
	public void RefreshLookup()
	{
		this.namedLookup = null;
		this.Init();
	}

	// Token: 0x06005F0D RID: 24333 RVA: 0x00235355 File Offset: 0x00233555
	public bool IsDefaultColorSet()
	{
		return Array.IndexOf<ColorSet>(GlobalAssets.Instance.colorSetOptions, this) == 0;
	}

	// Token: 0x04003F92 RID: 16274
	public string settingName;

	// Token: 0x04003F93 RID: 16275
	[Header("Logic")]
	public Color32 logicOn;

	// Token: 0x04003F94 RID: 16276
	public Color32 logicOff;

	// Token: 0x04003F95 RID: 16277
	public Color32 logicDisconnected;

	// Token: 0x04003F96 RID: 16278
	public Color32 logicOnText;

	// Token: 0x04003F97 RID: 16279
	public Color32 logicOffText;

	// Token: 0x04003F98 RID: 16280
	public Color32 logicOnSidescreen;

	// Token: 0x04003F99 RID: 16281
	public Color32 logicOffSidescreen;

	// Token: 0x04003F9A RID: 16282
	[Header("Decor")]
	public Color32 decorPositive;

	// Token: 0x04003F9B RID: 16283
	public Color32 decorNegative;

	// Token: 0x04003F9C RID: 16284
	public Color32 decorBaseline;

	// Token: 0x04003F9D RID: 16285
	public Color32 decorHighlightPositive;

	// Token: 0x04003F9E RID: 16286
	public Color32 decorHighlightNegative;

	// Token: 0x04003F9F RID: 16287
	[Header("Crop Overlay")]
	public Color32 cropHalted;

	// Token: 0x04003FA0 RID: 16288
	public Color32 cropGrowing;

	// Token: 0x04003FA1 RID: 16289
	public Color32 cropGrown;

	// Token: 0x04003FA2 RID: 16290
	[Header("Harvest Overlay")]
	public Color32 harvestEnabled;

	// Token: 0x04003FA3 RID: 16291
	public Color32 harvestDisabled;

	// Token: 0x04003FA4 RID: 16292
	[Header("Gameplay Events")]
	public Color32 eventPositive;

	// Token: 0x04003FA5 RID: 16293
	public Color32 eventNegative;

	// Token: 0x04003FA6 RID: 16294
	public Color32 eventNeutral;

	// Token: 0x04003FA7 RID: 16295
	[Header("Notifications")]
	public Color32 NotificationNormal;

	// Token: 0x04003FA8 RID: 16296
	public Color32 NotificationNormalBG;

	// Token: 0x04003FA9 RID: 16297
	public Color32 NotificationBad;

	// Token: 0x04003FAA RID: 16298
	public Color32 NotificationBadBG;

	// Token: 0x04003FAB RID: 16299
	public Color32 NotificationEvent;

	// Token: 0x04003FAC RID: 16300
	public Color32 NotificationEventBG;

	// Token: 0x04003FAD RID: 16301
	public Color32 NotificationMessage;

	// Token: 0x04003FAE RID: 16302
	public Color32 NotificationMessageBG;

	// Token: 0x04003FAF RID: 16303
	public Color32 NotificationMessageImportant;

	// Token: 0x04003FB0 RID: 16304
	public Color32 NotificationMessageImportantBG;

	// Token: 0x04003FB1 RID: 16305
	public Color32 NotificationTutorial;

	// Token: 0x04003FB2 RID: 16306
	public Color32 NotificationTutorialBG;

	// Token: 0x04003FB3 RID: 16307
	[Header("PrioritiesScreen")]
	public Color32 PrioritiesNeutralColor;

	// Token: 0x04003FB4 RID: 16308
	public Color32 PrioritiesLowColor;

	// Token: 0x04003FB5 RID: 16309
	public Color32 PrioritiesHighColor;

	// Token: 0x04003FB6 RID: 16310
	[Header("Info Screen Status Items")]
	public Color32 statusItemBad;

	// Token: 0x04003FB7 RID: 16311
	public Color32 statusItemEvent;

	// Token: 0x04003FB8 RID: 16312
	public Color32 statusItemMessageImportant;

	// Token: 0x04003FB9 RID: 16313
	[Header("Germ Overlay")]
	public Color32 germFoodPoisoning;

	// Token: 0x04003FBA RID: 16314
	public Color32 germPollenGerms;

	// Token: 0x04003FBB RID: 16315
	public Color32 germSlimeLung;

	// Token: 0x04003FBC RID: 16316
	public Color32 germZombieSpores;

	// Token: 0x04003FBD RID: 16317
	public Color32 germRadiationSickness;

	// Token: 0x04003FBE RID: 16318
	[Header("Room Overlay")]
	public Color32 roomNone;

	// Token: 0x04003FBF RID: 16319
	public Color32 roomFood;

	// Token: 0x04003FC0 RID: 16320
	public Color32 roomSleep;

	// Token: 0x04003FC1 RID: 16321
	public Color32 roomRecreation;

	// Token: 0x04003FC2 RID: 16322
	public Color32 roomBathroom;

	// Token: 0x04003FC3 RID: 16323
	public Color32 roomHospital;

	// Token: 0x04003FC4 RID: 16324
	public Color32 roomIndustrial;

	// Token: 0x04003FC5 RID: 16325
	public Color32 roomAgricultural;

	// Token: 0x04003FC6 RID: 16326
	public Color32 roomScience;

	// Token: 0x04003FC7 RID: 16327
	public Color32 roomBionic;

	// Token: 0x04003FC8 RID: 16328
	public Color32 roomPark;

	// Token: 0x04003FC9 RID: 16329
	[Header("Power Overlay")]
	public Color32 powerConsumer;

	// Token: 0x04003FCA RID: 16330
	public Color32 powerGenerator;

	// Token: 0x04003FCB RID: 16331
	public Color32 powerBuildingDisabled;

	// Token: 0x04003FCC RID: 16332
	public Color32 powerCircuitUnpowered;

	// Token: 0x04003FCD RID: 16333
	public Color32 powerCircuitSafe;

	// Token: 0x04003FCE RID: 16334
	public Color32 powerCircuitStraining;

	// Token: 0x04003FCF RID: 16335
	public Color32 powerCircuitOverloading;

	// Token: 0x04003FD0 RID: 16336
	[Header("Light Overlay")]
	public Color32 lightOverlay;

	// Token: 0x04003FD1 RID: 16337
	[Header("Conduit Overlay")]
	public Color32 conduitNormal;

	// Token: 0x04003FD2 RID: 16338
	public Color32 conduitInsulated;

	// Token: 0x04003FD3 RID: 16339
	public Color32 conduitRadiant;

	// Token: 0x04003FD4 RID: 16340
	[Header("Temperature Overlay")]
	public Color32 temperatureThreshold0;

	// Token: 0x04003FD5 RID: 16341
	public Color32 temperatureThreshold1;

	// Token: 0x04003FD6 RID: 16342
	public Color32 temperatureThreshold2;

	// Token: 0x04003FD7 RID: 16343
	public Color32 temperatureThreshold3;

	// Token: 0x04003FD8 RID: 16344
	public Color32 temperatureThreshold4;

	// Token: 0x04003FD9 RID: 16345
	public Color32 temperatureThreshold5;

	// Token: 0x04003FDA RID: 16346
	public Color32 temperatureThreshold6;

	// Token: 0x04003FDB RID: 16347
	public Color32 temperatureThreshold7;

	// Token: 0x04003FDC RID: 16348
	public Color32 heatflowThreshold0;

	// Token: 0x04003FDD RID: 16349
	public Color32 heatflowThreshold1;

	// Token: 0x04003FDE RID: 16350
	public Color32 heatflowThreshold2;

	// Token: 0x04003FDF RID: 16351
	private Dictionary<string, Color32> namedLookup;
}
