using System;

namespace ProcGenGame
{
	// Token: 0x02000E0B RID: 3595
	public interface SymbolicMapElement
	{
		// Token: 0x0600721C RID: 29212
		void ConvertToMap(Chunk world, TerrainCell.SetValuesFunction SetValues, float temperatureMin, float temperatureRange, SeededRandom rnd);
	}
}
