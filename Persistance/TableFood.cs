using System;

namespace Persistance
{
    public class TableFood
    {
        public int TableId{set; get;}
        public string Name{set; get;}
        public int Status{set; get;}
        public const int READY_STATUS = 1;
        public const int  INUSE_STATUS = 2;
    }
}