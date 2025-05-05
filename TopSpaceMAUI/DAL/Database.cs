using System;
using SQLite;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using TopSpaceMAUI;


namespace TopSpaceMAUI.DAL
{
    public static class Database
    {
        private static string? _dbName = null;

        public static string DbName
        {
            get
            {
                if (_dbName == null)
                {
#if ANDROID
                    _dbName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "SoSDB_2024.db");
#else
                    _dbName = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "SoSDB_2024.db");
                    //_dbName = Path.Combine("/Users/ronaldmaldonado/Desktop/TopSpaceDB", "SoSDB_2024.db");
#endif
                }

                return _dbName;
            }
        }

        public static SQLiteConnection GetNewConnection()
        {
            string dbPath = DbName;
            Directory.CreateDirectory(Path.GetDirectoryName(dbPath));
            return new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
        }

        public static void Close(SQLiteConnection connection)
        {
            if (connection == null)
                return;

            try
            {
                if (connection.IsInTransaction)
                    connection.Rollback();
            }
            catch
            {
            }
            finally
            {
                connection.Close();
                connection.Dispose();
                GC.Collect();
            }
        }

        public static void CreateDatabase()
        {
            SQLiteConnection db = GetNewConnection();

            // Criar tabelas com SQLite
            db.CreateTable<Model.POS>();
            db.CreateTable<Model.Brand>();
            db.CreateTable<Model.SKU>();
            db.CreateTable<Model.SKUCompetitor>();
            db.CreateTable<Model.MetricType>();
            db.CreateTable<Model.Metric>();
            db.CreateTable<Model.VisitCount>();
            db.CreateTable<Model.FileDepot>();
            db.CreateTable<Model.Message>();
            db.CreateTable<Model.SyncHistory>();
            db.CreateTable<Model.ExecutionContext>();
            db.CreateTable<Model.LPScoreBrand> ();
            db.CreateTable<Model.LPScoreSKU> ();
            db.CreateTable<Model.LogApp>();
            db.CreateTable<Model.TagType>();
            db.CreateTable<Model.Tag>();
            db.CreateTable<Model.TagBrandPontoNatural>();
            db.CreateTable<Model.TagMerchandisingAcao>();
            db.CreateTable<Model.TagPresenca>();
            db.CreateTable<Model.Quiz>();
            db.CreateTable<Model.QuizType>();
            db.CreateTable<Model.QuizOption>();
            db.CreateTable<Model.QuizAnswer>();
            db.CreateTable<Model.QuizPOS>();
            db.CreateTable<Model.Promotion>();
            db.CreateTable<Model.PromotionPOS>();
            db.CreateTable<Model.POSGps>();
            db.CreateTable<Model.LPGrade>();


            // Excluir as tabelas relacionadas com a tag.
            db.Execute("DROP TABLE IF EXISTS LPGrade");
            //db.Execute("DROP TABLE IF EXISTS LPMetricType");
            //db.Execute("DROP TABLE IF EXISTS LPScoreBrand");
            //db.Execute("DROP TABLE IF EXISTS LPScoreSKU");

            // Tabelas com chave primária concatenada, sem suporte a criação automática com SQLite
            db.Execute("CREATE TABLE IF NOT EXISTS ObjectiveBrand (POSCode varchar(20), MetricID integer, BrandID integer, Objective float, DueDate text, PRIMARY KEY (POSCode, MetricID, BrandID));");
            db.Execute("CREATE TABLE IF NOT EXISTS Visit (POSCode varchar(20), VisitDate text, Latitude float, Longitude float, Precision float, Score integer, PhotosDirectory varchar(36), DatabaseVersion text, PhotoTaken integer, Status varchar(1), PRIMARY KEY (POSCode, VisitDate))");
            if (!Util.DatabaseExtensions.ColumnExists(db, "Visit", "Category"))
            {
                db.Execute("ALTER TABLE Visit ADD COLUMN Category integer DEFAULT NULL");
            }

            db.Execute("CREATE TABLE IF NOT EXISTS VisitDataBrand (POSCode varchar(20), VisitDate text, MetricID integer, BrandID integer, Value float, CompetitorValue float, Score integer, PRIMARY KEY (POSCode, VisitDate, MetricID, BrandID))");
            db.Execute("CREATE TABLE IF NOT EXISTS VisitDataSKU (POSCode varchar(20), VisitDate text, MetricID integer, SKUID integer, Value float, Score integer, PRIMARY KEY (POSCode, VisitDate, MetricID, SKUID))");
            db.Execute("CREATE TABLE IF NOT EXISTS VisitDataTrackPrice (POSCode varchar(20), VisitDate text, SKUID integer, Value float, PRIMARY KEY (POSCode, VisitDate, SKUID))");
            db.Execute("CREATE TABLE IF NOT EXISTS VisitPhotoQueue (POSCode varchar(20), VisitDate text, MetricID integer, BrandID integer, SKUID integer, PhotoID integer, PhotoDirectory varchar(36), Photo varchar(28), Category integer, SampleCategory integer, SampleVisit integer, PRIMARY KEY (POSCode, VisitDate, MetricID, BrandID, SKUID, PhotoID))");
            db.Execute("CREATE TABLE IF NOT EXISTS VisitPhotoQualityCheckQueue (POSCode varchar(20), VisitDate text, MetricID integer, BrandID integer, SKUID integer, PhotoID integer, PhotoDirectory varchar(36), Photo varchar(28), Category integer, SampleCategory integer, SampleVisit integer, PRIMARY KEY (POSCode, VisitDate, MetricID, BrandID, SKUID, PhotoID))");
            db.Execute("CREATE TABLE IF NOT EXISTS VisitDataQuiz (QuizAnswerID integer,POSCode varchar(20), VisitDate text, QuizID integer, Type integer, Question varchar(500), AnswerValue integer, Answer varchar(500), REP varchar(500))");


            db.Execute("CREATE TABLE IF NOT EXISTS LPGrade (Name varchar(20), ScoreMin integer, ScoreMax integer, StartPeriod text, EndPeriod text, TagID integer, PRIMARY KEY (Name, StartPeriod, TagID))");
            db.Execute("CREATE TABLE IF NOT EXISTS LPMetricType (MetricTypeCode varchar(50), Weight float, StartPeriod text, EndPeriod text, TagID integer, PRIMARY KEY (MetricTypeCode, StartPeriod, TagID))");
            //db.Execute("CREATE TABLE IF NOT EXISTS LPScoreBrand (LPScoreBrandID varchar(50), BrandID integer, MetricID integer, StartPeriod text, EndPeriod text, Score integer, TagID integer, PRIMARY KEY (LPScoreBrandID, StartPeriod, TagID))");
            //db.Execute("CREATE TABLE IF NOT EXISTS LPScoreSKU (LPScoreSKUID varchar(50), SKUID integer, MetricID integer, StartPeriod text, EndPeriod text, Score integer, TagID integer, PRIMARY KEY (LPScoreSKUID, StartPeriod, TagID))");

            db.Execute("CREATE TABLE IF NOT EXISTS LastVisit (POSCode varchar(20), VisitDate text, Score integer, PRIMARY KEY (POSCode, VisitDate))");
            db.Execute("CREATE TABLE IF NOT EXISTS LastVisitDataTrackPrice (POSCode varchar(20), VisitDate text, SKUID integer, Value float, PRIMARY KEY (POSCode, VisitDate, SKUID))");

            db.Execute("CREATE TABLE IF NOT EXISTS ImgLib (ItemID varchar(140), Title text, Tags text, Brand text, FileID varchar(140), URLDownload varchar(140), URLThumb varchar(140), ActionCode varchar(100), CreationDate text, PRIMARY KEY (ItemID))");
            if (!Util.DatabaseExtensions.ColumnExists(db, "ImgLib", "LibCode"))
            {
                db.Execute("ALTER TABLE ImgLib ADD COLUMN LibCode varchar(50) NULL DEFAULT('" + Config.URL_API_MODULO_IMG_LIB + "')");
            }

            db.Execute("CREATE TABLE IF NOT EXISTS POSMaterial (ItemID varchar(140), Month text, Quantity integer, PRIMARY KEY (ItemID, Month))");

            if (!Util.DatabaseExtensions.ColumnExists(db, "LogException", "CurrentScreen"))
            {
                db.Execute("ALTER TABLE LogException ADD COLUMN CurrentScreen varchar(140) DEFAULT NULL");
            }
            ////TODO: Remover colunas StockNormalMin e StockNormalMax de SKU

            if (!Util.DatabaseExtensions.ColumnExists(db, "SKU", "TrackPrice"))
            {
                db.Execute("ALTER TABLE SKU ADD COLUMN TrackPrice integer DEFAULT NULL");
            }

            db.Execute("DROP TABLE IF EXISTS ObjectiveSKU");
            db.Execute("DROP TABLE IF EXISTS LastVisitData");

            if (!Util.DatabaseExtensions.ColumnExists(db, "VisitDataBrand", "ExecutionContextID"))
            {
                db.Execute("ALTER TABLE VisitDataBrand ADD COLUMN ExecutionContextID integer DEFAULT NULL");
            }

            db.Execute("DROP TABLE IF EXISTS Log");

            //db.Execute("CREATE INDEX IF NOT EXISTS IX_TAG_NOME ON Tag (Name)");
            //db.Execute("CREATE INDEX IF NOT EXISTS IX_TAGBRANDPONTONATURAL_TAGID_BRANDID ON TagBrandPontoNatural (TagID, BrandID)");
            //db.Execute("CREATE INDEX IF NOT EXISTS IX_TAGMERCHANDISINGACAO_TAGID_BRANDID ON TagMerchandisingAcao (TagID, BrandID)");
            //db.Execute("CREATE INDEX IF NOT EXISTS IX_OBJECTIVE_BRAND_POS ON ObjectiveBrand (PosCode)");
            //db.Execute("CREATE INDEX IF NOT EXISTS IX_OBJECTIVE_BRAND_POS_BRAND_METRIC ON ObjectiveBrand (PosCode, BrandID, MetricID)");
            //db.Execute("CREATE INDEX IF NOT EXISTS IX_OBJECTIVE_BRAND_BRAND_METRIC ON ObjectiveBrand (BrandID, MetricID)");

            if (!Util.DatabaseExtensions.ColumnExists(db, "POS", "UnitVariation"))
            {
                db.Execute("ALTER TABLE POS ADD COLUMN UnitVariation varchar(250) DEFAULT NULL");
            }

            PopulateDatabase(db);

            Close(db);
        }

        public static void CheckDatabaseIntegrity()
        {
            SQLiteConnection? db = null;
            SQLiteCommand? commandCheck = null;
            try
            {
                db = GetNewConnection();
                commandCheck = db.CreateCommand("PRAGMA integrity_check;");
                string resultCheck = commandCheck.ExecuteScalar<string>();
                if (resultCheck.ToUpperInvariant() != "OK")
                {
                    FixDatabase(db);
                }
            }
            catch
            {
            }
            finally
            {
                commandCheck = null;
                if (db != null) 
                    Close(db);
                db = null;
            }
        }

        public static void FixDatabase(SQLiteConnection db)
        {
            SQLiteCommand? commandFix = db.CreateCommand("VACUUM;");
            commandFix.ExecuteNonQuery();
            commandFix = null;
        }

        private static void PopulateDatabase(SQLiteConnection db)
        {
            db.InsertOrReplace(new Model.ExecutionContext { ExecutionContextID = 1, Description = "Não existe o ponto negociado na loja" });
            db.InsertOrReplace(new Model.ExecutionContext { ExecutionContextID = 2, Description = "Não possuo material de Trade" });
            db.InsertOrReplace(new Model.ExecutionContext { ExecutionContextID = 3, Description = "Loja sem VB adequado para negociação" });
            db.InsertOrReplace(new Model.ExecutionContext { ExecutionContextID = 4, Description = "Gerente proibiu a implementação" });
            db.InsertOrReplace(new Model.ExecutionContext { ExecutionContextID = 5, Description = "Loja não recebeu essa negociação" });
            db.InsertOrReplace(new Model.ExecutionContext { ExecutionContextID = 6, Description = "Loja está em Reforma ou em Balanço" });
        }
    }
}