using System.Data;

namespace CPC.DbComponent
{
    public abstract class FetchSql
    {
        // Methods
        internal FetchSql()
        {
        }

        public abstract string FetchAddedSql(DataRowView drv);
        public abstract string FetchDeleteSql(DataRowView drv, bool interCurrent, string[] obj);
        public abstract string FetchModifySql(DataRowView drv, bool interCurrent, string[] obj);
        public abstract void Init();
        public abstract string ReFillRowItem(DataRowView drv);
    }
}