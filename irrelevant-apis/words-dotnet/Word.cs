using System;

namespace Words
{
    public class Word
    {
        public string Term { get; set; }
        public string Description { get; set; }
        
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
    }
}
