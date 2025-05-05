using System;

namespace TopSpaceMAUI.Util
{
	public class Objective
	{
		public static void RefreshObjectivesShelf (string brandName, decimal? objective, decimal actualVisit, bool editing = false)
		{
			// Se tiver objetivo e o mesmo foi alcançado, decrementar o número de objetivos
			// Caso após alcançar o objetivo o usuário altere o valor e o mesmo não tenha mais alcançado a meta, o número de objetivos deve ser acrescido
			if (!Model.Objective.RefreshedMetricTypeShelf.ContainsKey (brandName)) {
				Model.Objective.RefreshedMetricTypeShelf.Add (brandName, false);
			}
			if (objective.HasValue) {
				if (actualVisit >= objective) {
					if (Model.Objective.RefreshedMetricTypeShelf.ContainsKey (brandName)) {
						if (!Model.Objective.RefreshedMetricTypeShelf [brandName]) {
							Model.Objective.RefreshedMetricTypeShelf [brandName] = true;
							ChangeTotalObjetivesMetricType (brandName, Config.METRIC_TYPE_SHELF_NAME, true, editing);
							if (Model.Objective.TotalObjectivesBrand.ContainsKey (brandName)) {
								Model.Objective.TotalObjectivesBrand [brandName]--;
							}
							Model.Objective.TotalObjectives--;
						}
					}
				} else {
					if (editing) {
						if (Model.Objective.RefreshedMetricTypeShelf.ContainsKey (brandName)) {
							if (Model.Objective.RefreshedMetricTypeShelf [brandName]) {
								Model.Objective.RefreshedMetricTypeShelf [brandName] = false;
                                ChangeTotalObjetivesMetricType (brandName, Config.METRIC_TYPE_SHELF_NAME, false, editing);
								if (Model.Objective.TotalObjectivesBrand.ContainsKey (brandName)) {
									Model.Objective.TotalObjectivesBrand [brandName]++;
								}
								Model.Objective.TotalObjectives++;
							}
						}
					}
				}
			}
		}

		public static void RefreshMetricType (string brandName, string metricType, string metricName, decimal? objective, decimal actualVisit, bool editing = false)
		{
			// Se tiver objetivo e o mesmo foi alcançado, decrementar o número de objetivos
			// Caso após alcançar o objetivo o usuário altere o valor e o mesmo não tenha mais alcançado a meta, o número de objetivos deve ser acrescido
			if (!Model.Objective.RefreshedMetricTypeOthers.ContainsKey (new Tuple<string, string> (brandName, metricName))) {
				Model.Objective.RefreshedMetricTypeOthers.Add (new Tuple<string, string> (brandName, metricName), false);
			}
			if (objective != null) {
				if (actualVisit >= objective) {
					if (Model.Objective.RefreshedMetricTypeOthers.ContainsKey (new Tuple<string, string> (brandName, metricName))) {
						if (!Model.Objective.RefreshedMetricTypeOthers [(new Tuple<string, string> (brandName, metricName))]) {
							Model.Objective.RefreshedMetricTypeOthers [(new Tuple<string, string> (brandName, metricName))] = true;
							ChangeTotalObjetivesMetricType (brandName, metricType, true, editing);
							if (Model.Objective.TotalObjectivesBrand.ContainsKey (brandName)) {
								Model.Objective.TotalObjectivesBrand [brandName]--;
							}
							Model.Objective.TotalObjectives--;
						}
					}
				} else {
					if (editing) {
						if (Model.Objective.RefreshedMetricTypeOthers.ContainsKey (new Tuple<string, string> (brandName, metricName))) {
							if (Model.Objective.RefreshedMetricTypeOthers [(new Tuple<string, string> (brandName, metricName))]) {
								Model.Objective.RefreshedMetricTypeOthers [(new Tuple<string, string> (brandName, metricName))] = false;
								ChangeTotalObjetivesMetricType (brandName, metricType, false, editing);
								if (Model.Objective.TotalObjectivesBrand.ContainsKey (brandName)) {
									Model.Objective.TotalObjectivesBrand [brandName]++;
								}
								Model.Objective.TotalObjectives++;
							}
						}
					}
				}
			}
		}

		private static void ChangeTotalObjetivesMetricType (string brandName, string metricType, bool decrease, bool editing = false)
		{
			switch (metricType) {
				case Config.METRIC_TYPE_SHELF_NAME:
					if (Model.Objective.TotalObjectivesMetricTypeShelf.ContainsKey (brandName)) {
						if (decrease) {
							Model.Objective.TotalObjectivesMetricTypeShelf [brandName]--;
						} else {
							if (editing) {
								Model.Objective.TotalObjectivesMetricTypeShelf [brandName]++;
							}
						}
					}
					break;
				case Config.METRIC_TYPE_DISPLAY_NAME:
					if (Model.Objective.TotalObjectivesMetricTypeDisplay.ContainsKey (brandName)) {
						if (decrease) {
							Model.Objective.TotalObjectivesMetricTypeDisplay [brandName]--;
						} else {
							if (editing) {
								Model.Objective.TotalObjectivesMetricTypeDisplay [brandName]++;
							}
						}
					}
					break;
			}
		}
	}
}