using System;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace TopSpaceMAUI.Util
{
	public class DeviceHardware
	{
		public const string HardwareProperty = "hw.machine";

		public enum HardwareVersion
		{
			iPhone1G,
			iPhone2G,
			iPhone3G,
			iPhone4,
			iPhone5,
			iPod1G,
			iPod2G,
			iPod3G,
			iPod4G,
			iPod5G,
			iPad,
			[Description("iPad 2")]
			iPad2,
			[Description("iPad Mini")]
			iPadMini,
			[Description("iPad 3")]
			iPad3,
			[Description("iPad 4")]
			iPad4,
			[Description("iPad Air")]
			iPadAir,
			[Description("iPad Mini 2")]
			iPadMini2,
			[Description("iPad Mini 3")]
			iPadMini3,
			[Description("iPad Air 2")]
			iPadAir2,
			[Description("iOS Simulator")]
			Simulator,
			Unknown
		}

		//[DllImport (Constants.SystemLibrary)]
		internal static extern int sysctlbyname ([MarshalAs (UnmanagedType.LPStr)] string property, // name of the property
			IntPtr output, // output
			IntPtr oldLen, // IntPtr.Zero
			IntPtr newp, // IntPtr.Zero
			uint newlen // 0
		);

		public static HardwareVersion Version {
			get {
                // get the length of the string that will be returned
                var ret = HardwareVersion.Unknown;
#if IOS
                var pLen = Marshal.AllocHGlobal (sizeof(int));
				sysctlbyname (DeviceHardware.HardwareProperty, IntPtr.Zero, pLen, IntPtr.Zero, 0);

				var length = Marshal.ReadInt32 (pLen);

				// check to see if we got a length
				if (length == 0) {
					Marshal.FreeHGlobal (pLen);
					return HardwareVersion.Unknown;
				}

				// get the hardware string
				var pStr = Marshal.AllocHGlobal (length);
				sysctlbyname (DeviceHardware.HardwareProperty, pStr, pLen, IntPtr.Zero, 0);

				// convert the native string into a C# string
				var hardwareStr = Marshal.PtrToStringAnsi (pStr);

				// determine which hardware we are running
				if (hardwareStr == "iPhone1,1")
					ret = HardwareVersion.iPhone1G;
				else if (hardwareStr == "iPhone1,2")
					ret = HardwareVersion.iPhone2G;
				else if (hardwareStr == "iPhone2,1")
					ret = HardwareVersion.iPhone3G;
				else if (hardwareStr == "iPhone3,1")
					ret = HardwareVersion.iPhone4;
				else if (hardwareStr == "iPhone4,1")
					ret = HardwareVersion.iPhone5;
				else if (hardwareStr == "iPod1,1")
					ret = HardwareVersion.iPod1G;
				else if (hardwareStr == "iPod2,1")
					ret = HardwareVersion.iPod2G;
				else if (hardwareStr == "iPod3,1")
					ret = HardwareVersion.iPod3G;
				else if (hardwareStr == "iPod4,1")
					ret = HardwareVersion.iPod4G;
				// iPad
				else if (hardwareStr == "iPad1,1")
					ret = HardwareVersion.iPad;

				else if (hardwareStr == "iPad2,1")
					ret = HardwareVersion.iPad2;
				else if (hardwareStr == "iPad2,2")
					ret = HardwareVersion.iPad2;
				else if (hardwareStr == "iPad2,3")
					ret = HardwareVersion.iPad2;
				else if (hardwareStr == "iPad2,4")
					ret = HardwareVersion.iPad2;

				else if (hardwareStr == "iPad2,5")
					ret = HardwareVersion.iPadMini;
				else if (hardwareStr == "iPad2,6")
					ret = HardwareVersion.iPadMini;
				else if (hardwareStr == "iPad2,7")
					ret = HardwareVersion.iPadMini;

				else if (hardwareStr == "iPad3,1") 
					ret = HardwareVersion.iPad3;
				else if (hardwareStr == "iPad3,2")
					ret = HardwareVersion.iPad3;
				else if (hardwareStr == "iPad3,3")
					ret = HardwareVersion.iPad3;


				else if (hardwareStr == "iPad3,4") 
					ret = HardwareVersion.iPad4;
				else if (hardwareStr == "iPad3,5")
					ret = HardwareVersion.iPad4;
				else if (hardwareStr == "iPad3,6")
					ret = HardwareVersion.iPad4;

				else if (hardwareStr == "iPad4,1") 
					ret = HardwareVersion.iPadAir;
				else if (hardwareStr == "iPad4,2")
					ret = HardwareVersion.iPadAir;
				else if (hardwareStr == "iPad4,3")
					ret = HardwareVersion.iPadAir;

				else if (hardwareStr == "iPad4,4")
					ret = HardwareVersion.iPadMini2;
				else if (hardwareStr == "iPad4,5")
					ret = HardwareVersion.iPadMini2;
				else if (hardwareStr == "iPad4,6")
					ret = HardwareVersion.iPadMini2;

				else if (hardwareStr == "iPad4,7")
					ret = HardwareVersion.iPadMini3;
				else if (hardwareStr == "iPad4,8")
					ret = HardwareVersion.iPadMini3;
				else if (hardwareStr == "iPad4,9")
					ret = HardwareVersion.iPadMini3;

				else if (hardwareStr == "iPad5,3")
					ret = HardwareVersion.iPadAir2;
				else if (hardwareStr == "iPad5,4")
					ret = HardwareVersion.iPadAir2;

				else if (hardwareStr == "i386")
					ret = HardwareVersion.Simulator;
				else if (hardwareStr == "x86_64")
					ret = HardwareVersion.Simulator;

				// cleanup
				Marshal.FreeHGlobal (pLen);
				Marshal.FreeHGlobal (pStr);
#endif
				return ret;
			}
		}
	}
}