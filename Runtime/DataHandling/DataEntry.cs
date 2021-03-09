namespace oculog
{
    public struct DataEntry
    {
        public string id;
        public string description;
        
        public ELogLevel logLevel;

        private float timeStamp;

        public DataEntry(string id, string description, float timeStamp, ELogLevel logLevel)
        {
            this.id = id;
            this.description = description;
            this.logLevel = logLevel;
            this.timeStamp = timeStamp;
        }

        public DataEntry(string id, string description, float timeStamp)
        {
            this.id = id;
            this.description = description;
            this.timeStamp = timeStamp;
            logLevel = ELogLevel.Default;
        }

        public string GetTimeStamp()
        {
            throw new System.NotImplementedException();
        }
    }
}