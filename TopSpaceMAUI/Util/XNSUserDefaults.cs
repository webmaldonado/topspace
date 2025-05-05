using Microsoft.Maui.Storage;

namespace TopSpaceMAUI.Util
{
    public static class XNSUserDefaults
    {
        public static void RemoveObject(string key)
        {
            Preferences.Remove(key);
        }

        public static string GetStringForKey(string key)
        {
            return Preferences.Get(key, string.Empty);
        }

        public static void SetStringValueForKey(string key, string value)
        {
            Preferences.Set(key, value);
        }

        public static bool? GetBoolValueForKey(string key)
        {
            if (Preferences.ContainsKey(key))
            {
                return Preferences.Get(key, false);
            }
            return null;
        }

        public static void SetBoolValueForKey(string key, bool value)
        {
            Preferences.Set(key, value);
        }

        public static int GetIntValueForKey(string key)
        {
            return Preferences.Get(key, 0);
        }

        public static void SetIntValueForKey(string key, int value)
        {
            Preferences.Set(key, value);
        }
    }
}


//using System;


//namespace TopSpaceMAUI.Util
//{
//	public static class XNSUserDefaults
//	{
//		public static void RemoveObject (string key) 
//		{
//			NSUserDefaults defaults = NSUserDefaults.StandardUserDefaults;
//			defaults.RemoveObject (key);
//			defaults.Synchronize ();
//		}

//		public static string GetStringForKey (string key) 
//		{
//			NSUserDefaults defaults = NSUserDefaults.StandardUserDefaults;
//			return defaults.StringForKey (key);
//		}

//		public static void SetStringValueForKey (string key, string value)
//		{
//			NSUserDefaults defaults = NSUserDefaults.StandardUserDefaults;
//			defaults.SetString (value, key);
//			defaults.Synchronize ();
//		}

//		public static bool? GetBoolValueForKey (string key)
//		{
//			NSUserDefaults defaults = NSUserDefaults.StandardUserDefaults;
//			if (defaults.ValueForKey(new NSString(key)) != null) {
//				return defaults.BoolForKey (key);
//			}
//			return null;
//		}

//		public static void SetBoolValueForKey (string key, bool value)
//		{
//			NSUserDefaults defaults = NSUserDefaults.StandardUserDefaults;
//			defaults.SetBool (value, key);
//			defaults.Synchronize ();
//		}

//		public static nint GetIntValueForKey (string key) 
//		{
//			NSUserDefaults defaults = NSUserDefaults.StandardUserDefaults;
//			return defaults.IntForKey (key);
//		}

//		public static void SetIntValueForKey (string key, int value)
//		{
//			NSUserDefaults defaults = NSUserDefaults.StandardUserDefaults;
//			defaults.SetInt (value, key);
//			defaults.Synchronize ();
//		}
//	}
//}

