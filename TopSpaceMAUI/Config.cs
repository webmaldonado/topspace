using System;

namespace TopSpaceMAUI
{
    public static class Config
    {
        public const string HOST_TEST_CONNECTION = "google.com";

        // E-mails para envio de logs da aplicação
        public static string[] LOG_RECIPIENTS = new string[] { "application.support@bayer.com" };
        public static string[] LOG_CC_RECIPIENTS = new string[] { };

        // URL base do Web Service
        public static string URL_API_BASE = "https://topspacews.bayer.com.br/api/"; // Servidor de produção
        //public static string URL_API_BASE = "http://10.27.114.8:8096/api/"; // Servidor QA windows
        //public static string URL_API_BASE = "http://BY0V0N:8096/api/"; // Servidor DEV windows

        // Public key certificado SSL
        public const string BAYER_CERTIFICATE_PUB_KEY_CURRENT = "3082010A02820101009D9AF40A2331F0EF2574737D5111FEAB466CEA96DC5FAECD96D2BEDA862E84238B41F237DE6957393A9CF3C7758F29489FC3F06893A3FD545DD214B3F678B967F43D8FAE0AFAD37C0996D2D3AFC76FDF431E18BD46A744303823E9323C89EF31F9F22877DE59C7ACF2CE3994E924AB4D38AB97ABEFE7292DFFC4AA8289905103B737F2A0F79022C62C046920D9E6F2556942C7E4C007A6530AB0D98A0C47946A258ED1E435A2D154BF359F49E55B5AB353CD804C93F2240C5825DA660F16263548C3F83E573A743ABF39CE6D3CA6F4C0638EE0114AEF730655F004596ECFB590C9955A6FF20F671CD4361D6A22ED3131FA8ADC1B67A8CD213AD98C86A6104FEF0203010001";
        public const string BAYER_CERTIFICATE_PUB_KEY = "3082010A02820101009D9AF40A2331F0EF2574737D5111FEAB466CEA96DC5FAECD96D2BEDA862E84238B41F237DE6957393A9CF3C7758F29489FC3F06893A3FD545DD214B3F678B967F43D8FAE0AFAD37C0996D2D3AFC76FDF431E18BD46A744303823E9323C89EF31F9F22877DE59C7ACF2CE3994E924AB4D38AB97ABEFE7292DFFC4AA8289905103B737F2A0F79022C62C046920D9E6F2556942C7E4C007A6530AB0D98A0C47946A258ED1E435A2D154BF359F49E55B5AB353CD804C93F2240C5825DA660F16263548C3F83E573A743ABF39CE6D3CA6F4C0638EE0114AEF730655F004596ECFB590C9955A6FF20F671CD4361D6A22ED3131FA8ADC1B67A8CD213AD98C86A6104FEF0203010001";

        // Módulos e métodos do Web Service
        public const string URL_API_MODULO_SYNC = "syncs/";
        public const string URL_API_MODULO_NEWS = "news";
        public const string URL_API_MODULO_IMG_LIB = "BCO_IMG";
        public const string URL_API_MODULO_POS_MAT = "MAT_PDV";
        public const string URL_API_MODULO_WHATS_GOING_ON = "TROMBONE";
        public const string URL_API_REQUEST_TOKEN = "tokens";
        public const string URL_API_REQUEST_START = "{0}/start";
        public const string URL_API_REQUEST_FINISH = "{0}/finish";
        public const string URL_API_REQUEST_NOW = "now";
        public const string URL_API_REQUEST_POSS = "reps/{0}/poss";
        public const string URL_API_REQUEST_BRANDS = "reps/{0}/brands";
        public const string URL_API_REQUEST_SKUS = "reps/{0}/skus";
        public const string URL_API_REQUEST_SKU_COMPETITORS = "reps/{0}/skuCompetitors";
        public const string URL_API_REQUEST_METRIC_TYPES = "reps/{0}/metricTypes";
        public const string URL_API_REQUEST_METRICS = "reps/{0}/metrics?version={1}";
        public const string URL_API_REQUEST_OBJECTIVE_BRANDS = "reps/{0}/objectiveBrands";
        public const string URL_API_REQUEST_LPGRADES = "reps/{0}/LPGrades";
        public const string URL_API_REQUEST_LPMETRICTYPES = "reps/{0}/LPMetricTypes";
        public const string URL_API_REQUEST_LPSCOREBRANDS = "reps/{0}/LPScoreBrands";
        public const string URL_API_REQUEST_LPSCORESKUS = "reps/{0}/LPScoreSKUs";
        public const string URL_API_REQUEST_VISIT_DATA = "reps/{0}/visit";
        public const string URL_API_REQUEST_LOG_DATA = "reps/{0}/log";
        public const string URL_API_REQUEST_VISIT_PHOTO = "reps/{0}/visitPhoto";
        public const string URL_API_REQUEST_VISIT_PHOTO_QUALITY_CHECK = "reps/{0}/visitPhotoQualityCheck";
        public const string URL_API_REQUEST_LAST_VISIT = "reps/{0}/lastVisit";
        public const string URL_API_REQUEST_VISIT_COUNT = "reps/{0}/visitCount";
        public const string URL_API_REQUEST_LAST_VISIT_DATA_TRACK_PRICE = "reps/{0}/lastVisitDataTrackPrice";
        public const string URL_API_REQUEST_FILE_DEPOT_GET_CHANGES = "filedepots/{0}/getchanges?depotVersionLastSynced={1}";
        public const string URL_API_REQUEST_FILE_DEPOT_GET_FILE = "filedepots/{0}/getfile";
        public const string URL_API_REQUEST_IMG_LIB_GET_CHANGES = "imgLib/{0}/getchanges?imgLibLastSynced={1}";
        public const string URL_API_REQUEST_IMG_LIB_GET_FILE = "imgLib/{0}/getFile";
        public const string URL_API_REQUEST_POS_MATERIAL = "reps/{0}/posMaterial";
        public const string URL_API_REQUEST_MESSAGE_GET_CHANGES = "message/{0}/message?lastMessageID={1}";
        public const string URL_API_REQUEST_MESSAGE = "message/{0}/message";
        public const string URL_API_REQUEST_TAG = "reps/{0}/tags";
        public const string URL_API_REQUEST_TAG_BRAND_PONTO_NATURAL = "reps/{0}/tagbrandpontonaturals";
        public const string URL_API_REQUEST_TAG_MERCHANDISING_ACAO = "reps/{0}/tagmerchandisingacaos";
        public const string URL_API_REQUEST_TAG_PRESENCA = "reps/{0}/tagpresencas";
        public const string URL_API_REQUEST_TAG_TYPE = "reps/{0}/tagtypes";
        public const string URL_API_REQUEST_QUIZ = "reps/{0}/quizs";
        public const string URL_API_REQUEST_QUIZ_TYPE = "reps/{0}/quiztypes";
        public const string URL_API_REQUEST_QUIZ_OPTION = "reps/{0}/quizoptions";
        public const string URL_API_REQUEST_QUIZ_ANSWER = "reps/{0}/quizanswers";
        public const string URL_API_REQUEST_QUIZ_POS = "reps/{0}/quizpos";
        public const string URL_API_REQUEST_PROMOTION = "reps/{0}/promotions";
        public const string URL_API_REQUEST_PROMOTION_POS = "reps/{0}/PromotionsPOS";
        public const string URL_API_REQUEST_POSGPS = "reps/{0}/PosGPS";


        // Parâmetros dos métodos do Web Service
        public const string URL_API_PARAMETER_USERNAME = "username";
        public const string URL_API_PARAMETER_PASSWORD = "password";
        public const string URL_API_PARAMETER_ID = "ID";
        public const string URL_API_PARAMETER_UTC = "whenUtc";

        // Parâmetros do header do Web Service
        public const string API_HEADER_TOKEN = "SOS_TOKEN";

        // Keys NSUserDefaults
        public const string KEY_TOKEN = "TokenID";
        public const string KEY_USERNAME = "Username";
        public const string KEY_DATABASE_VERSION = "DateSync";
        public const string KEY_INSTALL_ID = "InstallID";

        public const string KEY_LAST_DATE_SYNC = "LastDateSync";
        public const string KEY_STATUS_SYNC = "StatusSync";
        public const string KEY_NEWS_DATE_SYNC = "DateSyncNews";
        public const string KEY_IMG_LIB_DATE_SYNC = "DateSyncImgLib";
        public const string KEY_POS_MAT_DATE_SYNC = "DateSyncPOSMat";
        public const string KEY_LOG_ERROR = "LogError";
        public const string KEY_APP_VERSION = "Version";
        public const string KEY_OS_VERSION = "OS";
        public const string KEY_CHECK_LOG_SENT = "AlertLogSent";
        public const string KEY_CHECK_LOCALIZATION_ENABLE = "AlertLocalizationShowed";
        public const string KEY_APPLICATION_STATE = "ApplicationState";

        public const string KEY_FOLDER_OLD_IMG_LIB = "imgLib";
        public const string KEY_DELETED_OLD_IMG_LIB = "MIGRATED_IMG_LIB";

        public const string KEY_IMG_LIB_RESETED = "RESETED_";

        public const string KEY_LOCAL_LOG = "LOCAL_LOG";

        // Stages
        public const int RESYNC_TOLERANCE_MINUTES = 0;
        public const string KEY_STAGE_DOWNLOAD_BASE_DATA = "DownloadBaseData";

        // URL base do Mural
        //		public const string URL_WALL_BASE = "https://www.spark.bayer.com.br/"; // Servidor de produção
        public const string URL_WALL_BASE = "https://stage.spark.bayer.com.br/"; // Servidor de homologação
                                                                                 //		public const string URL_WALL_BASE = "http://faceline.bayer.foster.com.br/"; // Servidor de desenvolvimento
                                                                                 //		public const string URL_WALL_BASE = "http://10.46.58.166:8020/"; // Servidor local de desenvolvimento
        public const string URL_WALL_DEFAULT = URL_WALL_BASE + "portal-spark/account/login";

        public const string LOCAL_FOLDER_WWW = "wwwroot";
        public const string LOCAL_DEFAULT_WWW = LOCAL_FOLDER_WWW + "/not-synced";
        public const string LOCAL_DEFAULT_FILE = LOCAL_DEFAULT_WWW + "/index.html";

        public const string NEWS_FOLDER = "news";
        public const string NEWS_DEFAULT_FILE = NEWS_FOLDER + "/index.html";

        public const string EMAIL_TEMPLATE_FOLDER = LOCAL_FOLDER_WWW + "/email-template";
        public const string IMG_LIB_EMAIL_TEMPLATE = "template-imglib.html";

        // Tipos de métricas do BD
        public const string METRIC_TYPE_SHELF_NAME = "Shelf";
        public const string METRIC_TYPE_STOCK_NAME = "Stock";
        public const string METRIC_TYPE_DISPLAY_NAME = "Display";
        public const string METRIC_TYPE_ACTION_NAME = "Action";
        public const string METRIC_STOCK_NAME = "Stock";

        // Configuração da fotografia
        public const string PHOTO_NAME = "BrandID-{0}_MetricID-{1}.jpg";
        public const string PHOTO_NAME_QUALITY_CHECK = "VISIT-{0}-{1}.jpg";
        public const float PHOTO_COMPRESSION = 0.75f;
        public const float PHOTO_MAX_WIDTH = 960;
        public const float PHOTO_MAX_HEIGHT = 720;

        public const string EXTENSION_IMAGES_IMG_LIB = ".png";

        // Status do log
        public const string STATUS_LOG_SENT = "Y";
        public const string STATUS_LOG_NOT_SEND = "N";

        // Status da visita
        public const string STATUS_VISIT_STARTED = "S";
        public const string STATUS_VISIT_COMPLETED = "C";
        public const string STATUS_VISIT_SENT = "T";

        // Status do material PDV
        public const string POS_MATERIAL_NEEDED = "NEEDED";
        public const string POS_MATERIAL_NO_ACTION = "NO_ACTION";
        // Período de entrada de dados
        public const int POS_MATERIAL_NEEDED_FIRST_N_DAYS = 7;
        public const int POS_MATERIAL_EXTRA_DAYS = 2;

        // Intervalo mínimo entre as requisições (em segundos)
        public const int BACKGROUND_FETCH_INTERVAL = 1800; // 30 minutos 

        // Destinatário das mensagens do "O que está rolando?" 
        public const string MESSAGE_WHATS_GOING_ON_ADMIN = "ADMIN";

        // Status das mensagens
        public const string MESSAGE_WHATS_GOING_ON_MESSAGE_SENT = "Y";
        public const string MESSAGE_WHATS_GOING_ON_MESSAGE_NOT_SEND = "N";

        // Local da mensagem
        public const string MESSAGE_WHATS_GOING_ON_MESSAGE_LOCAL = "LOCAL";
        public const string MESSAGE_WHATS_GOING_ON_MESSAGE_SERVER = "SERVER";

        // Constantes
        public const float M_PI = (float)Math.PI;

        // Sincronização, corresponde a 21 etapas
        public const int PROGRESS_TOTAL = 100;
        public const float PROGRESS_PERCENT = 4.7619047619f;

        // Configurações da view
        public const float MODAL_VIEW_WIDTH = 540;
        public const float MODAL_VIEW_HEIGHT = 768;
        public const float STATUS_BAR_HEIGHT = 20;

        // Cores da escala de pontuação
        public static readonly string[] GRADE_COLORS = { "#CC0000", "#FF6600", "#FF9933", "#66CC66", "#33CC99" };

        public enum LogType
        {
            Operation,
            Error,
            Security,
            Debug
        };
    }
}