using System;
using System.Collections.Generic;

namespace TopSpaceMAUI.Util
{
	public class POSMaterial
	{
		/// <summary>
		/// Verificar qual é a entrada de dados no presente momento, alterar a lógica juntamente com o server-side
		/// </summary>
		/// <returns>The report type.</returns>
		/// <param name="now">Now.</param>
		public static string CheckInputData (DateTime now)
		{
			if (now.Day <= Config.POS_MATERIAL_NEEDED_FIRST_N_DAYS) {
				return Config.POS_MATERIAL_NEEDED;
			}
			return Config.POS_MATERIAL_NO_ACTION;
		}

		/// <summary>
		/// Verificar se os dados devem ser sincronizados no presente momento, alterar a lógica juntamente com o server-side
		/// </summary>
		/// <returns>The report type.</returns>
		/// <param name="now">Now.</param>
		public static bool CheckSyncData (DateTime now)
		{
			bool sync = false;

			if (now.Day <= (Config.POS_MATERIAL_NEEDED_FIRST_N_DAYS + Config.POS_MATERIAL_EXTRA_DAYS)) {
				sync = true;
			}

			return sync;
		}
	}
}

