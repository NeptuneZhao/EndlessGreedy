using HarmonyLib;
using System;
using TUNING;

[HarmonyPatch(typeof(MinionResume))]
internal class SuperMinionResume
{
	[HarmonyPrefix, HarmonyPatch(nameof(MinionResume.Sim200ms), new Type[] { typeof(float) })]
	public static bool Prefix(MinionResume __instance, float dt)
	{
		__instance.DEBUG_SecondsAlive += dt;
		if (!__instance.GetComponent<KPrefabID>().HasTag(GameTags.Dead))
		{
			__instance.DEBUG_PassiveExperienceGained += dt / 2f;
			__instance.AddExperience(dt / 2f + 1000f);
		}
		return false;
	}
}