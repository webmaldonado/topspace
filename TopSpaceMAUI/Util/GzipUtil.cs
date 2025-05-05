using System.IO.Compression;
using System.Text;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace TopSpaceMAUI.Util
{
	public class GzipUtil
	{
		public static byte[] Compress(string s)
		{
			var bytes = Encoding.Unicode.GetBytes(s);
			using (var msi = new MemoryStream(bytes))
			using (var mso = new MemoryStream())
			{
				using (var gs = new GZipStream(mso, CompressionMode.Compress))
				{
					msi.CopyTo(gs);
				}
				return mso.ToArray ();
			}
		}



        public static string ZipDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException("O diretório especificado não existe.");
            }

            string zipFileName = Path.Combine(Path.GetTempPath(), "arquivo.zip");

            using (FileStream zipFileStream = new FileStream(zipFileName, FileMode.Create))
            using (ZipOutputStream zipStream = new ZipOutputStream(zipFileStream))
            {
                zipStream.SetLevel(9); // Nível de compressão (0-9)

                ZipDirectory(directoryPath, directoryPath, zipStream);

                zipStream.IsStreamOwner = true;
                zipStream.Close();
            }

            return zipFileName;
        }

        private static void ZipDirectory(string rootDirectory, string sourceDirectory, ZipOutputStream zipStream)
        {
            string[] files = Directory.GetFiles(sourceDirectory);
            foreach (string file in files)
            {
                string relativePath = Path.GetRelativePath(rootDirectory, file).Replace('\\', '/');
                ZipEntry entry = new ZipEntry(relativePath);
                zipStream.PutNextEntry(entry);

                using (FileStream fileStream = File.OpenRead(file))
                {
                    StreamUtils.Copy(fileStream, zipStream, new byte[4096]);
                }

                zipStream.CloseEntry();
            }

            string[] subdirectories = Directory.GetDirectories(sourceDirectory);
            foreach (string subdirectory in subdirectories)
            {
                ZipDirectory(rootDirectory, subdirectory, zipStream);
            }
        }



    }
}
