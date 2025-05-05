using System;
using System.Threading;
using System.Threading.Tasks;

namespace TopSpaceMAUI
{
	public class XTask
	{
		public XTask ()
		{
		}



		public static void ThrowIfCancellationRequested(CancellationTokenSource cts)
		{
			if (cts != null)
				cts.Token.ThrowIfCancellationRequested ();
			else
				throw new OperationCanceledException ();
		}



		public static void ThrowOperationCanceled(CancellationTokenSource cts, string message = null)
		{
			if (cts != null)
				cts.Cancel (); // Sinaliza para os demais pontos de controle

			if (string.IsNullOrEmpty(message))
				throw new OperationCanceledException ();

			throw new OperationCanceledException (message);
		}
	}
}