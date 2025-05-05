using System;
using SQLite;
using System.Collections.Generic;

namespace TopSpaceMAUI.Util
{
	public static class DatabaseExtensions
	{
		public static bool TableExists (SQLiteConnection db, string table)
		{
			try {
				List<SQLiteConnection.ColumnInfo> lstColumns = db.GetTableInfo(table);
				if (lstColumns.Count > 0) {
					return true;
				} 
				return false;
			} catch {
				return false;
			}
		}

		public static bool ColumnExists(SQLiteConnection db, string table, string column)
		{
			if (TableExists (db, table)) {
				try {
					List<SQLiteConnection.ColumnInfo> lstColumns = db.GetTableInfo(table);
					foreach (var columnData in lstColumns) {
						if (columnData.Name == column) {
							return true;
						}
					}
					return false;
				} catch {
					return false;
				}
			} else {
				// Se a tabela não existe não posso realizar nenhuma operação
				return true;
			}
		}
	}
}
