using HarmonyLib;

[HarmonyPatch(typeof(CosmicResearchCenterConfig))]
internal class SuperCosmicResearchCenter
{
    [HarmonyPostfix, HarmonyPatch(nameof(CosmicResearchCenterConfig.ConfigureBuildingTemplate))]
	public static void Postfix(UnityEngine.GameObject go)
	{
		go.AddOrGet<ResearchCenter>().mass_per_point = 0.01f;
	}
}