using System;
using System.IO;

using System.Security.Cryptography;
using System.Text;
using System.Linq;
namespace TopSpaceMAUI.Util
{
	public class FileSystem
	{



		public static bool CheckFile (string file)
		{

            //Model.Sync.LogInfo("Nome da foto: " + Environment.GetFolderPath(Environment.SpecialFolder.Personal) + Path.DirectorySeparatorChar + file);
            return File.Exists (Environment.GetFolderPath (Environment.SpecialFolder.Personal) + Path.DirectorySeparatorChar + file);
		}

        public static bool CheckFileQualityCheck(string file)
        {
            return File.Exists(file);
        }

		public static bool SaveFile(string path, byte[] fileServer)
		{
			string completePath = System.IO.Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), path);
			File.WriteAllBytes (completePath, fileServer);
				
			if (File.Exists (completePath)) {
				FileInfo fileClient = new FileInfo (completePath); 
				Model.Sync.LogInfo (String.Format (Localization.TranslateText ("ReceiveFilesClientLength"), fileServer.Length) + " / " + String.Format (Localization.TranslateText ("ReceiveFilesSavedLength"), fileClient.Length));
				fileClient = null;
				return true;
			}

			return false;
		}


		public static string GetMd5File(string path)
		{
			FileInfo file = new FileInfo (System.IO.Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), path));

			using (MD5 md5Hash = MD5.Create())
			{
				byte[] data;

				using (var stream = file.OpenRead())
				{
					file = null;
					data = md5Hash.ComputeHash(stream);
				}

				StringBuilder sBuilder = new StringBuilder();

				for (int i = 0; i < data.Length; i++)
				{
					sBuilder.Append(data[i].ToString("x2"));
				}

				data = null;

				return sBuilder.ToString().ToUpper();
			}
		}

		public static bool CheckMD5File(string path, string md5Server)
		{
			string md5Client = GetMd5File (path);

			Model.Sync.LogInfo (String.Concat(Localization.TryTranslateText("EntityNews"), String.Format (Localization.TryTranslateText("ReceiveFilesMD5Server"), md5Server)));
			Model.Sync.LogInfo (String.Concat(Localization.TryTranslateText("EntityNews"), String.Format (Localization.TryTranslateText("ReceiveFilesMD5Client"), md5Client)));

			if (string.IsNullOrEmpty (md5Server) || string.IsNullOrEmpty (md5Client) || !md5Client.Equals (md5Server.ToUpper())) {
				return false;
			}

			return true;
		}

		public static bool DeleteFile (string path)
		{
			string completePath = System.IO.Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), path);
			if (File.Exists (completePath)) {
				File.Delete (completePath);
				if (!File.Exists (completePath))
					return true;
			}
			return false;
		}

		public static void DeleteFile (string file, string directory)
		{
			File.Delete (Environment.GetFolderPath (Environment.SpecialFolder.Personal) + Path.DirectorySeparatorChar + file);
			// Apagar o diretÃ³rio caso fique vazio
			if (ListFiles (directory).Length == 0) {
				DeleteDirectory (directory);
			}
		}



		public static void CreateDirectory (string name)
		{
			var documentsDirectory = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			var directoryName = Path.Combine (documentsDirectory, name);
			Directory.CreateDirectory (directoryName);
		}



		public static void DeleteDirectory (string name)
		{
			string directory = (Environment.GetFolderPath (Environment.SpecialFolder.Personal) + Path.DirectorySeparatorChar + name);

			string[] files = Directory.GetFiles (directory);
			string[] dirs = Directory.GetDirectories (directory);

			foreach (string file in files) {
				File.SetAttributes (file, FileAttributes.Normal);
				File.Delete (file);
			}

			foreach (string dir in dirs) {
				DeleteDirectory (dir);
			}

			Directory.Delete (directory, false);
		}


		public static void MoveDirectory(string path, string pathSource, string pathDestination)
		{
			string pathAux = null;

			if (System.IO.Directory.Exists (path)) {
				foreach (string pasta in System.IO.Directory.GetDirectories(path)) {
					pathAux = pasta.Replace (pathSource, pathDestination);
					System.IO.Directory.CreateDirectory (pathAux);

					foreach (string pathFile in System.IO.Directory.GetFiles(pasta)) {
						pathAux = pathFile.Replace (pathSource, pathDestination);
						System.IO.File.Delete (pathAux);
						System.IO.File.Move (pathFile, pathAux);
					}

					MoveDirectory (pasta, pathSource, pathDestination);

					System.IO.Directory.Delete (pasta);
				}

				pathAux = path.Replace (pathSource, pathDestination);
				System.IO.Directory.CreateDirectory (pathAux);

				foreach (string pathFile in System.IO.Directory.GetFiles(path)) {
					pathAux = pathFile.Replace (pathSource, pathDestination);
					System.IO.File.Delete (pathAux);
					System.IO.File.Move (pathFile, pathAux);
				}
			}
		}


		public static string[] ListFiles (string name)
		{
			string directory = (Environment.GetFolderPath (Environment.SpecialFolder.Personal) + Path.DirectorySeparatorChar + name);
			return Directory.GetFiles (directory);
		}

		public static void DeleteEmptyDirectory(string path)
		{
			foreach(string itemDirectory in System.IO.Directory.GetDirectories(path))
			{
				if (!FileSystem.IsEmptyDirectory(itemDirectory)){
					System.IO.Directory.Delete (itemDirectory);
				}

				DeleteEmptyDirectory (itemDirectory);
			}

			if (!FileSystem.IsEmptyDirectory(path)){
				System.IO.Directory.Delete (path);
			}		
		}

		public static bool IsEmptyDirectory(string path)
		{
			int qtdFiles = Directory.GetFiles (path).Where (f => !f.EndsWith (".DS_Store")).Count ();
			int qtdDirectory = Directory.GetDirectories (path).Length;
			return (qtdFiles == 0 && qtdDirectory == 0);
		}

		public static byte[] SelectPhoto(string file)
		{            
           return File.ReadAllBytes (Environment.GetFolderPath (Environment.SpecialFolder.Personal) + Path.DirectorySeparatorChar + file);
        }

        public static byte[] ResizePhoto(string file)
        {
            string completePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), file);

            using (var stream = File.OpenRead(completePath))
            {
                Microsoft.Maui.Graphics.IImage image = Microsoft.Maui.Graphics.Platform.PlatformImage.FromStream(stream);

                if (image.Width > Config.PHOTO_MAX_WIDTH + 10)
                {
                    var resizedImage = image.Resize(Config.PHOTO_MAX_WIDTH, Config.PHOTO_MAX_HEIGHT, ResizeMode.Bleed);

                    using (var memoryStream = new MemoryStream())
                    {
                        resizedImage.Save(memoryStream, ImageFormat.Png); // Adjust the format as needed
                        File.WriteAllBytes(completePath, memoryStream.ToArray());
                    }
                }
            }

            return SelectPhoto(file);
        }
    }
}

