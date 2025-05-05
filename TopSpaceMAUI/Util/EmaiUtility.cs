using System.Globalization;
using System.Web;
using FileSystemHelper = Microsoft.Maui.Storage.FileSystem;

namespace TopSpaceMAUI.Util
{
    public class EmaiUtility
	{
		public static string PrepareEmailCrash(List<Model.LogApp> log)
		{
			string message = String.Empty;
			foreach (var line in log) {
				message += Localization.TryTranslateText("EmailLogLogID") + line.AppID + "<br/ >";
				message += Localization.TryTranslateText ("EmailLogCreationDate") + line.DeviceDate + "<br/ >";
				if (!String.IsNullOrEmpty(line.Operation)) {
					message += Localization.TryTranslateText ("EmailLogCurrentScreen") + line.Operation + "<br/ >";
				} else {
					message += Localization.TryTranslateText ("EmailLogCurrentScreen") + Localization.TryTranslateText ("EmailLogCurrentScreenNoInfo") + "<br/ >";
				}
				message += Localization.TryTranslateText("EmailLogDescription") + HttpUtility.HtmlEncode(line.Description) + "<br/ >";
				message += Localization.TryTranslateText("EmailLogComments") + HttpUtility.HtmlEncode(line.Comments) + "<br/ ><br/ >";
			}
			return message;
		}

        //public static string PrepareEmailSync(List<Model.SyncLog> log)
        //{
        //	string message = String.Empty;
        //	foreach (var line in log) {
        //		message += line.Message;
        //		if (!String.IsNullOrEmpty(line.Detail)) {
        //			message += Localization.TryTranslateText("EmailLogDetails") + line.Detail + "<br/ >";
        //		}
        //		message += "<br/ >";
        //	}
        //	message += "<br/ >";
        //	return message;
        //}
        public static string PrepareEmailSync(System.Collections.ObjectModel.ObservableCollection<Model.SyncLog> log)
        {
            string message = String.Empty;
            foreach (var line in log)
            {
                message += line.Message;
                if (!String.IsNullOrEmpty(line.Detail))
                {
                    message += Localization.TryTranslateText("EmailLogDetails") + line.Detail + "<br/ >";
                }
                message += "<br/ >";
            }
            message += "<br/ >";
            return message;
        }

        private static string GetAppInfo ()
		{
			string appInfo = "<b>"+ Localization.TryTranslateText("EmailLogUserInfo") +"</b>" + "<br/ >";
			appInfo += Localization.TryTranslateText("EmailLogUsername") + DAL.Token.Current.Username + "<br/ >";
			appInfo += Localization.TryTranslateText("EmailLogAppVersion") + AppInfo.VersionString + "<br/ >";
			appInfo += Localization.TryTranslateText ("EmailLogServerName") + Config.URL_API_BASE + "<br/ >";

			return appInfo;   
		}

		public static string GetDeviceInfo ()
		{
			string deviceInfo = "<br/ ><b>"+ Localization.TryTranslateText("EmailLogDeviceInfo") +"</b>" + "<br/ >";
			deviceInfo += Localization.TryTranslateText("EmailLogDeviceModel") + Util.DeviceHardware.Version.Description () + "<br/ >";
			deviceInfo += Localization.TryTranslateText("EmailLogSOVersion") + DeviceInfo.Current.VersionString + "<br/ >";
			deviceInfo += Localization.TryTranslateText("EmailLogLanguage") + CultureInfo.CurrentUICulture.DisplayName + "<br/ >";
			deviceInfo += Localization.TryTranslateText("EmailLogLocationServices");
			//deviceInfo += CLLocationManager.Status == CLAuthorizationStatus.Authorized? Localization.TryTranslateText ("EmailLogLSEnabled") + "<br/ >" : Localization.TryTranslateText("EmailLogLSDisabled") + " (" + CLLocationManager.Status + ") <br/ >";
			deviceInfo += Localization.TryTranslateText("EmailLogCameraAccess");
			//deviceInfo += AVCaptureDevice.GetAuthorizationStatus(new NSString (AVMediaType.Video)) == AVAuthorizationStatus.Authorized? Localization.TryTranslateText ("EmailLogLSEnabled") + "<br/ >" : Localization.TryTranslateText("EmailLogLSDisabled") + " (" + AVCaptureDevice.GetAuthorizationStatus(new NSString (AVMediaType.Video)) + ") <br/ >";
			deviceInfo += Localization.TryTranslateText("EmailLogBackgroundRefresh");
			//deviceInfo += UIApplication.SharedApplication.BackgroundRefreshStatus == UIBackgroundRefreshStatus.Available? Localization.TryTranslateText ("EmailLogBREnabled") + "<br/ >" : Localization.TryTranslateText("EmailLogBRDisabled") + " (" + UIApplication.SharedApplication.BackgroundRefreshStatus + ") <br/ >";
			deviceInfo += Localization.TryTranslateText ("EmailLogInternetConnectionStatus") + Connectivity.Current.NetworkAccess.ToString() + "<br/ >";
		
			return deviceInfo;
		}

		public static void SendEmailLog(IViewController view, string subject, string title, string detail, Action sent = null)
		{
			try {
				if (Email.Default.IsComposeSupported) {

					string appInfo = GetAppInfo();
					string deviceInfo = GetDeviceInfo ();

					string log = "<br/ ><b>"+ title +"</b><br/ >";
					log += "<br/ >" + detail;
					log += String.Format(Localization.TryTranslateText("EmailLogGeneratedAt"), DateTime.UtcNow.ToString(Localization.TryTranslateText("EmailLogDateFormat")));
					EmailMessage mailController = new EmailMessage();

					mailController.To = new List<string>();
					foreach(string item in Config.LOG_RECIPIENTS)
					{
						mailController.To.Add(item);
					}
					if (Config.LOG_CC_RECIPIENTS.Length > 0)
					{
                        mailController.Cc = new List<string>();
                        foreach (string item in Config.LOG_CC_RECIPIENTS)
                        {
                            mailController.Cc.Add(item);
                        }
                    }
                    mailController.Subject = subject;
                    string message = appInfo + deviceInfo + log;
                    mailController.Body = message;
					mailController.BodyFormat = EmailBodyFormat.Html;

					try
					{
                        Email.Default.ComposeAsync(mailController).GetAwaiter().GetResult();
                        XUIAlertView.ShowTranslated("SendLogSucessTitle", "SendLogSucessMessage", null, "SendLogSucessPositiveButton");
                        if (sent != null)
                        {
                            sent();
                        }
                    }
                    catch (Exception e)
					{
                        XUIAlertView.ShowTranslated("SendLogErrorTitle", "SendLogErrorMessage", null, "SendLogErrorPositiveButton");
                    }

                } else {
					XUIAlertView.ShowTranslated("SendLogErrorTitle", "SendLogErrorMessage", null, "SendLogErrorPositiveButton");
				}

			} catch (Exception ex) {
				XUIAlertView.ShowTranslated("SendLogErrorTitle", "SendLogErrorMessage", null, "SendLogErrorPositiveButton");
			}
		}

		public static void SendEmailImgLib(IViewController view, string subject, string title, List<Model.ImgLib> images, Action sent = null)
		{
			try {
				if (Email.Default.IsComposeSupported) {
					string templateFileName = System.IO.Path.DirectorySeparatorChar + Config.IMG_LIB_EMAIL_TEMPLATE;;
					string templateFilePath = System.IO.Path.Combine (FileSystemHelper.AppDataDirectory, Config.EMAIL_TEMPLATE_FOLDER + templateFileName);
					string message =  System.IO.File.ReadAllText(templateFilePath);
					string table = string.Empty;
					int count = 1, limit = 4;

					for (int i = 0; i < images.Count; i++) {
						if (count == 1) {
							table  += "<tr>";
						}

						table += "<td align=\"center\" valign=\"center\">";
						table += "<div style=\"height: 60px; width: 120px;\">";
						table += "<span style=\"word-break:normal; font-size:17px; color:#FE7A46\">" + images[i].Title + "</span> <br/>";
						table += "</div>";
						table += "<div style=\"height: 120px\">";
						table += "<img align=\"bottom\" valign=\"bottom\" src=\"" + images[i].URLThumb + "\"/> <br/>";
						table += "</div>";
						table += "<a href=\""+ images[i].URLDownload + "\" style=\"text-decoration:none;\"/><img src=\"http://bayer-sos-sync.azurewebsites.net/template-email/btn-download-file.png\"></a>";
						table += "</td>";

						count++;

						if (count == limit) {
							table  += "</tr>";
							count = 1;
						}
					}
					message = message.Replace("//EMAIL_IMGLIB_TITLE//", title);
					message = message.Replace("//EMAIL_IMGLIB_IMAGES//", table);

                    EmailMessage mailController = new EmailMessage();
					mailController.Subject = subject;
					mailController.Body = message;
					mailController.BodyFormat = EmailBodyFormat.Html;

					try
					{
						Email.Default.ComposeAsync(mailController).GetAwaiter().GetResult();
                        if (sent != null)
                        {
                            sent();
                        }
                        XUIAlertView.ShowTranslated("SendLogSucessTitle", "SendLogSucessMessage", null, "SendLogSucessPositiveButton");
                    } catch (Exception e)
					{
                        XUIAlertView.ShowTranslated("SendLogErrorTitle", "SendLogErrorMessage", null, "SendLogErrorPositiveButton");
                    }
				} else {
					XUIAlertView.ShowTranslated("SendLogErrorTitle", "SendLogErrorMessage", null, "SendLogErrorPositiveButton");
				}

			} catch (Exception ex) {
				XUIAlertView.ShowTranslated("SendLogErrorTitle", "SendLogErrorMessage", null, "SendLogErrorPositiveButton");
			}
		}



        public static void SendEmailWithAttachment(IViewController view, string recipient, string subject, string messageBody, string attachmentFilePath, Action sent = null)
        {
            if (Email.Default.IsComposeSupported)
            {
                EmailMessage mailComposer = new EmailMessage();
                (mailComposer.To = new List<string>()).Add(recipient);
                mailComposer.Subject = subject;
                mailComposer.Body = messageBody;
				mailComposer.BodyFormat = EmailBodyFormat.Html;

                if (!string.IsNullOrEmpty(attachmentFilePath))
                {
                    var fileData = new EmailAttachment(attachmentFilePath);
                    (mailComposer.Attachments = new List<EmailAttachment>()).Add(fileData);
                }

				try
				{
					Email.Default.ComposeAsync(mailComposer).GetAwaiter().GetResult();
                    if (sent != null)
                    {
                        sent();
                    }
                    XUIAlertView.ShowTranslated("SendLogSucessTitle", "SendLogSucessMessage", null, "SendLogSucessPositiveButton");
                }
                catch (Exception e)
				{
                    XUIAlertView.ShowTranslated("SendLogErrorTitle", "SendLogErrorMessage", null, "SendLogErrorPositiveButton");
                }
            }
            else
            {
                // Handle the case where the device cannot send email
                Console.WriteLine("Email not configured or supported on this device.");
            }
        }

    }
}