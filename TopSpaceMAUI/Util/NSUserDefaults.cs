
#if IOS
using IOSDefaults = Foundation.NSUserDefaults;
#endif

namespace TopSpaceMAUI.Util
{
    internal class NSUserDefaults
    {
        private Dictionary<string, NSString> Data = new Dictionary<string, NSString>();

        public static NSUserDefaults StandardUserDefaults { get; internal set; } = new NSUserDefaults();

        public NSUserDefaults()
        {
            if (StandardUserDefaults == null)
            {
#if IOS
                var dafaults = IOSDefaults.StandardUserDefaults;
#else

#endif
            }
        }

        internal bool? BoolForKey(string key)
        {
            bool value = false;
            NSString? record = this.Data.GetValueOrDefault(key);
            if (record != null)
            {
                value = record.boolValue;
            }

            return value;
        }

        internal int IntForKey(string key)
        {
            int value = 0;
            NSString? record = this.Data.GetValueOrDefault(key);
            if (record != null)
            {
                value = record.intValue;
            }

            return value;
        }

        internal void RemoveObject(string key)
        {
            Data.Remove(key);
        }

        internal void SetBool(bool value, string key)
        {
            NSString? record = this.Data.GetValueOrDefault(key);
            if (record == null)
            {
                record = new NSString(key);
                Data.Add(key, record);
            }
            record.boolValue = value;
        }

        internal void SetInt(int value, string key)
        {
            NSString? record = this.Data.GetValueOrDefault(key);
            if (record == null)
            {
                record = new NSString(key);
                Data.Add(key, record);
            }
            record.intValue = value;
        }

        internal void SetString(string value, string key)
        {
            NSString? record = this.Data.GetValueOrDefault(key);
            if (record == null)
            {
                record = new NSString(key);
                Data.Add(key, record);
            }
            record.stringValue = value;
        }

        internal string StringForKey(string key)
        {
            string value = "";
            NSString? record = this.Data.GetValueOrDefault(key);
            if (record != null)
            {
                value = record.stringValue;
            }

            return value;
        }

        internal void Synchronize()
        {
            throw new NotImplementedException();
        }

        internal object ValueForKey(NSString nSString)
        {
            string key = nSString.key;
            object value = null;
            NSString? record = this.Data.GetValueOrDefault(key);
            if (record != null)
            {
                value = record.objectValue;
            }

            return value;
        }

        internal void SetValue(object value, string key)
        {
            NSString? record = this.Data.GetValueOrDefault(key);
            if (record == null)
            {
                record = new NSString(key);
                Data.Add(key, record);
            }
            record.objectValue = value;
        }
    }
}