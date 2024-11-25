using System;

// Token: 0x02000BBF RID: 3007
public enum SimMessageHashes
{
	// Token: 0x04003D07 RID: 15623
	Elements_CreateTable = 1108437482,
	// Token: 0x04003D08 RID: 15624
	Elements_CreateInteractions = -930289787,
	// Token: 0x04003D09 RID: 15625
	SetWorldZones = -457308393,
	// Token: 0x04003D0A RID: 15626
	ModifyCellWorldZone = -449718014,
	// Token: 0x04003D0B RID: 15627
	Disease_CreateTable = 825301935,
	// Token: 0x04003D0C RID: 15628
	Load = -672538170,
	// Token: 0x04003D0D RID: 15629
	Start = -931446686,
	// Token: 0x04003D0E RID: 15630
	AllocateCells = 1092408308,
	// Token: 0x04003D0F RID: 15631
	ClearUnoccupiedCells = -1836204275,
	// Token: 0x04003D10 RID: 15632
	DefineWorldOffsets = -895846551,
	// Token: 0x04003D11 RID: 15633
	PrepareGameData = 1078620451,
	// Token: 0x04003D12 RID: 15634
	SimData_InitializeFromCells = 2062421945,
	// Token: 0x04003D13 RID: 15635
	SimData_ResizeAndInitializeVacuumCells = -752676153,
	// Token: 0x04003D14 RID: 15636
	SimData_FreeCells = -1167792921,
	// Token: 0x04003D15 RID: 15637
	SimFrameManager_NewGameFrame = -775326397,
	// Token: 0x04003D16 RID: 15638
	Dig = 833038498,
	// Token: 0x04003D17 RID: 15639
	ModifyCell = -1252920804,
	// Token: 0x04003D18 RID: 15640
	ModifyCellEnergy = 818320644,
	// Token: 0x04003D19 RID: 15641
	SetInsulationValue = -898773121,
	// Token: 0x04003D1A RID: 15642
	SetStrengthValue = 1593243982,
	// Token: 0x04003D1B RID: 15643
	SetVisibleCells = -563057023,
	// Token: 0x04003D1C RID: 15644
	ChangeCellProperties = -469311643,
	// Token: 0x04003D1D RID: 15645
	AddBuildingHeatExchange = 1739021608,
	// Token: 0x04003D1E RID: 15646
	ModifyBuildingHeatExchange = 1818001569,
	// Token: 0x04003D1F RID: 15647
	ModifyBuildingEnergy = -1348791658,
	// Token: 0x04003D20 RID: 15648
	RemoveBuildingHeatExchange = -456116629,
	// Token: 0x04003D21 RID: 15649
	AddBuildingToBuildingHeatExchange = -1338718217,
	// Token: 0x04003D22 RID: 15650
	AddInContactBuildingToBuildingToBuildingHeatExchange = -1586724321,
	// Token: 0x04003D23 RID: 15651
	RemoveBuildingInContactFromBuildingToBuildingHeatExchange = -1993857213,
	// Token: 0x04003D24 RID: 15652
	RemoveBuildingToBuildingHeatExchange = 697100730,
	// Token: 0x04003D25 RID: 15653
	SetDebugProperties = -1683118492,
	// Token: 0x04003D26 RID: 15654
	MassConsumption = 1727657959,
	// Token: 0x04003D27 RID: 15655
	MassEmission = 797274363,
	// Token: 0x04003D28 RID: 15656
	AddElementConsumer = 2024405073,
	// Token: 0x04003D29 RID: 15657
	RemoveElementConsumer = 894417742,
	// Token: 0x04003D2A RID: 15658
	SetElementConsumerData = 1575539738,
	// Token: 0x04003D2B RID: 15659
	AddElementEmitter = -505471181,
	// Token: 0x04003D2C RID: 15660
	ModifyElementEmitter = 403589164,
	// Token: 0x04003D2D RID: 15661
	RemoveElementEmitter = -1524118282,
	// Token: 0x04003D2E RID: 15662
	AddElementChunk = 1445724082,
	// Token: 0x04003D2F RID: 15663
	RemoveElementChunk = -912908555,
	// Token: 0x04003D30 RID: 15664
	SetElementChunkData = -435115907,
	// Token: 0x04003D31 RID: 15665
	MoveElementChunk = -374911358,
	// Token: 0x04003D32 RID: 15666
	ModifyElementChunkEnergy = 1020555667,
	// Token: 0x04003D33 RID: 15667
	ModifyChunkTemperatureAdjuster = -1387601379,
	// Token: 0x04003D34 RID: 15668
	AddDiseaseEmitter = 1486783027,
	// Token: 0x04003D35 RID: 15669
	ModifyDiseaseEmitter = -1899123924,
	// Token: 0x04003D36 RID: 15670
	RemoveDiseaseEmitter = 468135926,
	// Token: 0x04003D37 RID: 15671
	AddDiseaseConsumer = 348345681,
	// Token: 0x04003D38 RID: 15672
	ModifyDiseaseConsumer = -1822987624,
	// Token: 0x04003D39 RID: 15673
	RemoveDiseaseConsumer = -781641650,
	// Token: 0x04003D3A RID: 15674
	ConsumeDisease = -1019841536,
	// Token: 0x04003D3B RID: 15675
	CellDiseaseModification = -1853671274,
	// Token: 0x04003D3C RID: 15676
	ToggleProfiler = -409964931,
	// Token: 0x04003D3D RID: 15677
	SetSavedOptions = 1154135737,
	// Token: 0x04003D3E RID: 15678
	CellRadiationModification = -1914877797,
	// Token: 0x04003D3F RID: 15679
	RadiationSickness = -727746602,
	// Token: 0x04003D40 RID: 15680
	AddRadiationEmitter = -1505895314,
	// Token: 0x04003D41 RID: 15681
	ModifyRadiationEmitter = -503965465,
	// Token: 0x04003D42 RID: 15682
	RemoveRadiationEmitter = -704259919,
	// Token: 0x04003D43 RID: 15683
	RadiationParamsModification = 377112707
}
