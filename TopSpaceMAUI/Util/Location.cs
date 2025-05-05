using System.Timers;

namespace TopSpaceMAUI.Util
{
    public class Location
	{
		public Microsoft.Maui.Devices.Sensors.Location? position;

		protected decimal precision = 1000;

		protected const int TIMEOUT = 2 * 60 * 1000;
		protected System.Timers.Timer timer;

        private CancellationTokenSource _cancelTokenSource;
        private bool _isCheckingLocation;

        public Location()
		{
            try
            {
                position = Geolocation.Default.GetLastKnownLocationAsync().GetAwaiter().GetResult();
            } catch (Exception e)
            {
                position = null;
            }

            timer = new System.Timers.Timer(TIMEOUT);
            timer.Elapsed += (object sender, ElapsedEventArgs e) => {
                Dispose();
                GetLocation();
            };
            timer.Start();
        }


		public void Dispose()
		{
			if (timer != null) {
				timer.Stop();
				timer.Dispose();
				timer = null;
			}
		}



		public void Stop()
		{
			Dispose ();
		}



		public void GetLocation()
		{
            if (!_isCheckingLocation)
    			this.GetCurrentLocation().GetAwaiter().GetResult();
		}

        public async Task GetCurrentLocation()
        {
            try
            {
                _isCheckingLocation = true;

                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                _cancelTokenSource = new CancellationTokenSource();

                position = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

                //if (position != null)
                //    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
            }
            // Catch one of the following exceptions:
            //   FeatureNotSupportedException
            //   FeatureNotEnabledException
            //   PermissionException
            catch (Exception ex)
            {
                // Unable to get location
                position = null;
            }
            finally
            {
                _isCheckingLocation = false;
            }
        }

        public void CancelRequest()
        {
            if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
                _cancelTokenSource.Cancel();
        }
    }
}

