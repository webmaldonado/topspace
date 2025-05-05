using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TopSpaceMAUI.Util;

namespace TopSpaceMAUI.DAL
{
	public interface ISyncable
	{
		string GetEntityName ();



		SyncStatusCode SaveTemp (object typedListOfTEntity);



		bool MoveTempToProd (SQLiteConnection db);
	}



	public abstract class Syncable<TEntity, TEntityTemp> : ISyncable
		where TEntity : class, IDisposable, new()
		where TEntityTemp : class, IDisposable, new()
	{
		public Syncable ()
		{
			DeleteRecordCeasedToExists = true;
		}

		protected bool DeleteRecordCeasedToExists{ set; get;}

		public abstract string GetEntityName ();



		protected abstract TEntity ConvertTempToEntity (TEntityTemp temp);



		protected abstract bool KeyMatch (TEntity local, TEntity remote);



		protected abstract bool HasChanged (TEntity local, TEntity remote);



		protected abstract IOrderedEnumerable<TEntity> OrderBy (IEnumerable<TEntity> source);



		protected virtual void DropTableTemp (SQLiteConnection db)
		{
			db.DropTable<TEntityTemp> ();
		}



		protected virtual void CreateTableTemp (SQLiteConnection db)
		{
			db.CreateTable<TEntityTemp> ();
		}

		protected virtual void BeforeSaveTempInsertAll (List<TEntityTemp> temp)
		{
		}

		protected virtual void AfterSaveTempInsertAll (List<TEntityTemp> temp)
		{
		}



		public SyncStatusCode SaveTemp (object typedListOfTEntity)
		{
			List<TEntityTemp> temp = (List<TEntityTemp>)typedListOfTEntity;

			Model.Sync.LogInfo (GetEntityName () + Localization.TryTranslateText("SaveTempDataStart"));

			SQLiteConnection db = null;
			SyncStatusCode result = SyncStatusCode.SaveError;

			try {
				db = Database.GetNewConnection ();
				db.BeginTransaction ();

				Model.Sync.LogInfo (GetEntityName () + Localization.TryTranslateText("SaveTempDataDeleteTable"));
				DropTableTemp (db);

				Model.Sync.LogInfo (GetEntityName () + Localization.TryTranslateText("SaveTempDataRecreateTable"));
				CreateTableTemp (db);

				BeforeSaveTempInsertAll (temp);

				Model.Sync.LogInfo (GetEntityName () + Localization.TryTranslateText("SaveTempDataInsert"));
				int rowsAdded = db.InsertAll (temp);
				Model.Sync.LogInfo (GetEntityName () + String.Format(Localization.TryTranslateText("SaveTempDataInsertCount"), rowsAdded));

				AfterSaveTempInsertAll (temp);

				Model.Sync.LogInfo (GetEntityName () + Localization.TryTranslateText("SaveTempDataConfirm"));
				db.Commit ();

				result = SyncStatusCode.SaveOK;
			}
			catch (Exception ex) {
				Model.Sync.LogError (GetEntityName () + String.Format(Localization.TryTranslateText("SaveTempDataFail"), ex));
			}
			finally {
				Database.Close (db);
				db = null;
			}

			Model.Sync.LogInfo (GetEntityName () + Localization.TryTranslateText("SaveTempDataFinished"));
			return result;
		}



		public bool MoveTempToProd (SQLiteConnection db)
		{
			Model.Sync.LogInfo (GetEntityName () + Localization.TryTranslateText("SaveProdDataStart"));

			List<TEntity> local = null;
			List<TEntity> remote = null;
			List<TEntity> difference = null;

			int recordsTotal = 0, recordsInserted = 0, recordsUpdated = 0, recordsDeleted = 0;

			try {
				var table_name = GetEntityName();
				if (table_name == "Quiz")
				{
					Model.Sync.LogInfo(GetEntityName() + Localization.TryTranslateText("SaveProdDataGet"));
					local = db.Table<TEntity>().ToList();
					recordsTotal = local.Count;
				}
				else
				{
                    Model.Sync.LogInfo(GetEntityName() + Localization.TryTranslateText("SaveProdDataGet"));
                    local = db.Table<TEntity>().ToList();
                    recordsTotal = local.Count;
                }


                Model.Sync.LogInfo (GetEntityName () + Localization.TryTranslateText("SaveTempDataGet"));
				remote = db.Table<TEntityTemp> ().Select (temp => ConvertTempToEntity (temp)).ToList ();

				// 1) Banco local não está populado, todos registros devem ser inseridos
				if (local.Count == 0) {
					Model.Sync.LogInfo (GetEntityName () + Localization.TryTranslateText("SaveProdDataInsert"));
					recordsTotal = recordsInserted = db.InsertAll (remote);
					Model.Sync.LogInfo (GetEntityName () + String.Format(Localization.TryTranslateText("SaveProdDataInsertCount"), recordsTotal));
				}
				else {
					// 2) Banco já está populado, os registros devem ser analisados
					Model.Sync.LogInfo (GetEntityName () + Localization.TryTranslateText("SaveProdDataSync"));

					// 2.a) DELETA registros que deixaram de existir
					if (DeleteRecordCeasedToExists) {
						difference = local.Where (l => !remote.Any (r => KeyMatch (l, r))).ToList ();
						recordsTotal -= (recordsDeleted = difference.Count);

						if (difference.Count > 0) {
							Model.Sync.LogInfo (GetEntityName () + Localization.TryTranslateText("SaveProdDataDeleteData"));
							foreach (var l in difference) {
								Delete (l, db);
								local.Remove (l);
							}
							Model.Sync.LogInfo (GetEntityName () + String.Format(Localization.TryTranslateText("SaveProdDataDeleteCount"), recordsDeleted));
						}
						difference = null;
					}

					//2.b) INSERE novos registros
					difference = remote.Where (r => !local.Any (l => KeyMatch (l, r))).ToList ();
					recordsTotal += (recordsInserted = difference.Count);

					if (difference.Count > 0) {
						Model.Sync.LogInfo (GetEntityName () + Localization.TryTranslateText("SaveProdDataInsertNew"));
						foreach (var r in difference) {
							Insert (r, db);
							remote.Remove (r);
						}
						Model.Sync.LogInfo (GetEntityName () + String.Format(Localization.TryTranslateText("SaveProdDataInsertNewCount") , recordsInserted));
					}
					difference = null;

					//2.c) ATUALIZA os registros que já existiam
					if (remote.Count > 0) {
						TEntity l = null;

						Model.Sync.LogInfo (GetEntityName () + Localization.TryTranslateText("SaveProdDataUpdate"));
						foreach (var r in remote) {
							l = local.Where (l_ => KeyMatch (l_, r)).FirstOrDefault ();

							if (HasChanged (l, r)) {
								Update (r, db); // r contains new info
								recordsUpdated++;
							}

							local.Remove (l); // can't remove from remote, only from local
							l.Dispose ();
							r.Dispose ();
						}
						Model.Sync.LogInfo (GetEntityName () + String.Format(Localization.TryTranslateText("SaveProdDataUpdateCount"), recordsUpdated));

						l = null;
					}
				}

				Model.Sync.LogInfo (GetEntityName () + Localization.TryTranslateText("SaveTempDataDeleteTable"));
				DropTableTemp (db);

				Model.Sync.LogInfo (string.Format (GetEntityName () + String.Format(Localization.TryTranslateText("SaveProdDataFinished"), recordsTotal, recordsInserted, recordsUpdated, recordsDeleted)));
				return true;
			}
			catch (Exception ex) {
				Model.Sync.LogError (GetEntityName () + String.Format(Localization.TryTranslateText("SaveProdDataFail"), ex));
			}
			finally {
				remote = local = difference = null;
			}

			return false;
		}



		public virtual List<TEntity> GetAll (SQLiteConnection db)
		{
			return OrderBy (db.Table<TEntity> ()).ToList ();
		}



		public virtual List<TEntity> GetAll ()
		{
			SQLiteConnection db = Database.GetNewConnection ();
			List<TEntity> result = GetAll (db);
			Database.Close (db);
			db = null;
			return result;
		}



		public virtual void OpenConnectionAndDoAction (Action<TEntity> action, TEntity e)
		{
			SQLiteConnection db = Database.GetNewConnection ();
			action (e);
			Database.Close (db);
			db = null;
		}



		public virtual void Insert (TEntity e, SQLiteConnection db)
		{
			db.Insert (e);
		}



		public virtual void Insert (TEntity e)
		{
			OpenConnectionAndDoAction (Insert, e);
		}



		public virtual void Update (TEntity e, SQLiteConnection db)
		{
			db.Update (e);
		}



		public virtual void Update (TEntity e)
		{
			OpenConnectionAndDoAction (Update, e);
		}



		public virtual void Delete (TEntity e, SQLiteConnection db)
		{
			db.Delete (e);
		}



		public virtual void Delete (TEntity e)
		{
			OpenConnectionAndDoAction (Delete, e);
		}
	}
}