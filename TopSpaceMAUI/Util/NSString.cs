namespace TopSpaceMAUI.Util
{
    internal class NSString
    {
        public string key;
        public string stringValue {get;set;}
        public int intValue { get; set; }
        public bool boolValue { get; set; }
        public object objectValue { get; set; }

        public NSString(string key)
        {
            this.key = key;
        }
    }
}