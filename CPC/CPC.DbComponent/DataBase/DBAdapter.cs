using System;
using System.Collections;
using System.ComponentModel;
using System.Data;

namespace CPC.DbComponent
{
    public class DBAdapter
    {
        private readonly IDbUtil _dbUtil;
        private readonly FetchSql fetchSql;
        private readonly ArrayList sqlArrayList = new ArrayList();
        private bool interCurrent;
        private bool reFillData;

        public DBAdapter(IDbUtil dbUtil)
        {
            if (dbUtil is SQLUtil)
            {
                fetchSql = new SqlFetchSql(dbUtil);
            }
            else
            {
                if (dbUtil is OleDbUtil)
                {
                    fetchSql = new OleDbFetchSql(dbUtil);
                }
                //if (dbUtil is OracleUtil)
                //{
                //    fetchSql = new OracleFetchSql(dbUtil);
                //}
            }
            _dbUtil = dbUtil;
        }

        /// <summary>
        /// 根据主键信息从数据库中找出相关记录
        /// </summary>
        [Description("是否回填数据")]
        public bool ReFillData
        {
            get => reFillData;
            set => reFillData = value;
        }

        private void AddSqlList(DataView addDv)
        {
            foreach (DataRowView view in addDv)
            {
                var num = -1;
                var sqlStr = fetchSql.FetchAddedSql(view);
                if (sqlStr != "")
                {
                    num = _dbUtil.ExecuteNonQuery(sqlStr);
                }
                if (reFillData && (num > 0))
                {
                    view.BeginEdit();
                    sqlArrayList.Add(view);
                    ReFillDataRow(view);
                }
                else if (interCurrent && (num == 0))
                {
                    throw new Exception("发生开放式并发冲突");
                }
            }
        }

        private void AddTableFrame(string tableName)
        {
            if (!TableInfo.ExistTableFrame(_dbUtil, tableName))
            {
                TableInfo.AddTableFrame(_dbUtil, tableName);
            }
        }

        private void Assort(DataView baseDv, string[] obj)
        {
            baseDv.RowStateFilter = DataViewRowState.Deleted;
            if (baseDv.Count > 0)
            {
                DelSqlList(baseDv, obj);
            }
            baseDv.RowStateFilter = DataViewRowState.ModifiedCurrent;
            if (baseDv.Count > 0)
            {
                ModSqlList(baseDv, obj);
            }
            baseDv.RowStateFilter = DataViewRowState.Added;
            if (baseDv.Count > 0)
            {
                AddSqlList(baseDv);
            }
        }

        private void BeginUpdate(DataView baseDv, string[] obj)
        {
            try
            {
                sqlArrayList.Clear();
                fetchSql.Init();
                AddTableFrame(baseDv.Table.TableName);
                Assort(baseDv, obj);
                CommitEdit();
                baseDv.Table.AcceptChanges();
            }
            catch (Exception exception)
            {
                RollBackEdit();
                throw exception;
            }
        }

        private void BeginUpdateAdv(DataView baseDv, string[] obj)
        {
            try
            {
                sqlArrayList.Clear();
                fetchSql.Init();
                AddTableFrame(baseDv.Table.TableName);
                _dbUtil.BeginTrans();
                Assort(baseDv, obj);
                _dbUtil.Commit();
                CommitEdit();
                baseDv.Table.AcceptChanges();
            }
            catch (Exception exception)
            {
                _dbUtil.RollBack();
                RollBackEdit();
                throw exception;
            }
        }

        private void CommitEdit()
        {
            for (var i = 0; i < sqlArrayList.Count; i++)
            {
                ((DataRowView)sqlArrayList[i]).EndEdit();
            }
        }

        private void DelSqlList(DataView delDv, string[] obj)
        {
            foreach (DataRowView view in delDv)
            {
                var num = _dbUtil.ExecuteNonQuery(fetchSql.FetchDeleteSql(view, interCurrent, obj));
            }
        }

        public void Dispose() => GC.SuppressFinalize(this);

        private void ModSqlList(DataView modDv, string[] obj)
        {
            foreach (DataRowView view in modDv)
            {
                var num = -1;
                var sqlStr = fetchSql.FetchModifySql(view, interCurrent, obj);
                if (sqlStr != "")
                {
                    num = _dbUtil.ExecuteNonQuery(sqlStr);
                }
                if (reFillData && (num > 0))
                {
                    view.BeginEdit();
                    sqlArrayList.Add(view);
                    ReFillDataRow(view);
                }
                else if (interCurrent && (num == 0))
                {
                    throw new Exception("发生开放式并发冲突");
                }
            }
        }

        private DataView ObjectToDataView(object baseData)
        {
            var view = new DataView();
            if (baseData is DataTable)
            {
                return new DataView((DataTable)baseData, "", "", DataViewRowState.CurrentRows);
            }
            if (!(baseData is DataView))
            {
                throw new Exception("请传入DataTable或DataView类型");
            }
            return (DataView)baseData;
        }

        private void ReFillDataRow(DataRowView drv)
        {
            var table = _dbUtil.GetDataTable(fetchSql.ReFillRowItem(drv));
            foreach (DataColumn column in table.Columns)
            {
                drv[column.ColumnName] = table.Rows[0][column.ColumnName];
            }
        }

        private void RollBackEdit()
        {
            for (var i = 0; i < sqlArrayList.Count; i++)
            {
                ((DataRowView)sqlArrayList[i]).CancelEdit();
            }
        }

        /// <summary>
        /// 更新DataTable
        /// </summary>
        /// <param name="baseData">数据集</param>
        /// <param name="interCurrent">true为全字段匹配，避免开发式并发冲突 flase为一般更新</param>
        public void Update(object baseData, bool interCurrent)
        {
            try
            {
                this.interCurrent = interCurrent;
                BeginUpdate(ObjectToDataView(baseData), null);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void Update(object baseData, string objstr)
        {
            try
            {
                var strArray = objstr.Split(new[] { ',' });
                interCurrent = true;
                BeginUpdate(ObjectToDataView(baseData), strArray);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void UpdateAdv(object baseData, bool interCurrent)
        {
            try
            {
                this.interCurrent = interCurrent;
                BeginUpdateAdv(ObjectToDataView(baseData), null);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void UpdateAdv(object baseData, string objstr)
        {
            try
            {
                var strArray = objstr.Split(new[] { ',' });
                interCurrent = true;
                BeginUpdateAdv(ObjectToDataView(baseData), strArray);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}