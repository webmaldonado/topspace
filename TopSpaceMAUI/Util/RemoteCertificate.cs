using System;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace TopSpaceMAUI.Util
{
	public class RemoteCertificate
	{
		public static bool ValidateRemoteCertificate( object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors policyErrors )
		{
			if (cert == null || chain == null)
				return false;

			if (policyErrors != SslPolicyErrors.None)
				return false;

			DateTime dateFrom = DateTime.Parse(cert.GetEffectiveDateString());
			DateTime dateTo = DateTime.Parse(cert.GetExpirationDateString());

			if (DateTime.UtcNow.CompareTo(dateFrom) < 0 || DateTime.UtcNow.CompareTo(dateTo) > 0)
				return false;

			if (Config.BAYER_CERTIFICATE_PUB_KEY_CURRENT.Equals(cert.GetPublicKeyString()) || Config.BAYER_CERTIFICATE_PUB_KEY.Equals(cert.GetPublicKeyString()))
				return true;

			return false;
		}
	}
}

