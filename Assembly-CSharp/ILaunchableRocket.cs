using System;
using UnityEngine;

// Token: 0x02000ACF RID: 2767
public interface ILaunchableRocket
{
	// Token: 0x1700061C RID: 1564
	// (get) Token: 0x06005233 RID: 21043
	LaunchableRocketRegisterType registerType { get; }

	// Token: 0x1700061D RID: 1565
	// (get) Token: 0x06005234 RID: 21044
	GameObject LaunchableGameObject { get; }

	// Token: 0x1700061E RID: 1566
	// (get) Token: 0x06005235 RID: 21045
	float rocketSpeed { get; }

	// Token: 0x1700061F RID: 1567
	// (get) Token: 0x06005236 RID: 21046
	bool isLanding { get; }
}
