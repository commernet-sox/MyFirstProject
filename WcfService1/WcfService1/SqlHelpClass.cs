using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WcfService1
{
    /// <summary>
    /// 数据库帮助类
    /// </summary>
    public class SqlHelpClass
    {
        static private string strServer;//数据库服务器地址，就是计算机名称
        static private string strUid;//SQLserver用户名称
        static private string strPwd;//密码
        static private string strDatabaseName;//数据库名称

        //属性限制外部只能设置数据库的链接信息
        static public string StrServer { set => strServer = value; }
        static public string StrUid { set => strUid = value; }
        static public string StrPwd { set => strPwd = value; }
        static public string StrDatabaseName { set => strDatabaseName = value; }

        static public string strSql
        {
            //这个字符串是连接字符串，包含着数据库的信息，对外部只提供信息不能修改
            get
            {
                return "Server="+strServer + ";User=" + strUid + ";Pwd=" + strPwd + ";Database=" + strDatabaseName;
            }
        }
        static SqlConnection sqlConnection;
        static SqlCommand cmd;

        static SqlHelpClass()
        {
            //静态构造函数，如果你需要给数据库信息设置默认值
            strServer = "47.98.229.13";
            strUid = "sa";
            strPwd = "123qwe!@#";
            strDatabaseName = "ZJWebsite";
        }

        /// <summary>
        /// 返回一个数据库连接
        /// </summary>
        /// <returns></returns>
        public static SqlConnection GetSqlConnection()
        {

            sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = strSql;
            return sqlConnection;
        }



        #region 对连接执行SQL语句或存储过程并返回受影响行数
        /// <summary>
        /// 对连接执行SQL语句并返回受影响行数
        /// </summary>
        /// <param name="sql">数据库操作命令语句</param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteNonQueryTypeText(string sql)
        {

            sqlConnection = new SqlConnection();//创建一个数据库连接实例（实例就是对象）
                                                //对这个类SqlConnection作用不明白的可以把鼠标放到类名可以显示微软对类给出的说明

            try
            {
                sqlConnection.ConnectionString = strSql;//数据库的信息给到这个连接，

                sqlConnection.Open();//打开连接

                cmd = new SqlCommand();/*创建一个查询实例
                                         *SqlConnection就像路，我们创建SqlConnection实例时就打通了连通数据库的一条路
                                         *SqlCommand就是信使，
                                         *我们每次要对数据库发出指令时，SqlCommand对象就可以帮我们把这个指令送到数据库
                                         * 数据库处理完后，再通过SqlCommand把我们需要的数据传回来。*/

                cmd.CommandText = sql;//设置要执行的数据库命令，就是你得把信给信使

                cmd.Connection = sqlConnection;//设置查询

                return cmd.ExecuteNonQuery();//返回受影响的行数，cmd.ExecuteNonQuery()在这里相当于执行命令：你给我把信送过去。
                                             //如果没有cmd.ExecuteNonQuery()，那么命令是不会执行的。
            }
            catch (Exception e)
            {

                throw e;//抛出异常
                //如果不想抛出异常影响程序进行，这里可以不写
                //或者返回个-1，然后在使用类里做判断
            }
            finally
            {
                sqlConnection.Close();//关闭数据库连接
                                      //数据库连接一般都是一次查询打开一个连接，查询结束关闭当前连接。
                                      //都已经把主要代码写在帮助类里了就不要想着强行省代码，该写的还得写
                                      //不要想着一个SqlConnection多次共用,防止出现不必要的异常或错误

            }

        }

        /// <summary>
        /// 对连接执行有参数的数据库的存储过程并返回受影响行数
        /// </summary>
        /// <param name="StoredProcedureName">数据库的存储过程的名称</param>
        /// <param name="sqlParameters">SqlParameter集合</param>
        /// <returns>受影响行数</returns>
        private static int ExecuteNonQueryTypeStoredProcedure(string StoredProcedureName, SqlParameter[] sqlParameters)
        {
            /*SqlParameter:
             *例： SqlParameter("@ID",ID)
             * @ID:就是数据库存储过程的参数
             * ID：就是你给这个参数赋的值
             * 
             * 要执行存储过程，首先就要给存储过程里边的参数赋值，
             * 一个SqlParameter可以给一个参数赋值，
             * 而一个SqlParameter数组可以包含给所有参数赋值的SqlParameter
             */
            sqlConnection = new SqlConnection();
            try
            {
                sqlConnection.ConnectionString = strSql;//数据库的信息给到这个连接，

                sqlConnection.Open();//打开连接
                cmd = new SqlCommand();//创建一个查询实例
                cmd.CommandType = CommandType.StoredProcedure;//设置cmd的行为类型
                cmd.CommandText = StoredProcedureName;//设置要执行的数据库存储过程的名称
                cmd.Connection = sqlConnection;//设置连接
                cmd.Parameters.AddRange(sqlParameters);//AddRange方法给cmd的参数添加sqlParameters集合
                return cmd.ExecuteNonQuery();//返回结果


            }
            catch (Exception e)
            {

                throw e;//抛出异常

            }
            finally
            {
                sqlConnection.Close();//关闭数据库连接

            }
        }

        /// <summary>
        /// 连接执行无参数的数据库的存储过程并返回受影响行数
        /// </summary>
        /// <param name="StoredProcedureName">数据库的存储过程的名称</param>
        /// <returns>受影响行数</returns>
        private static int ExecuteNonQueryTypeStoredProcedure(string StoredProcedureName)
        {
            sqlConnection = new SqlConnection();
            try
            {
                sqlConnection.ConnectionString = strSql;//数据库的信息给到这个连接，

                sqlConnection.Open();//打开连接
                cmd = new SqlCommand();//创建一个查询实例
                cmd.CommandType = CommandType.StoredProcedure;//设置cmd的行为类型
                cmd.CommandText = StoredProcedureName;//设置要执行的数据库存储过程的名称
                cmd.Connection = sqlConnection;//设置连接

                return cmd.ExecuteNonQuery();//返回结果


            }
            catch (Exception e)
            {

                throw e;//抛出异常

            }
            finally
            {
                sqlConnection.Close();//关闭数据库连接

            }
        }

        /// <summary>
        /// 对连接执行SQL语句或无参数存储过程并返回受影响行数
        /// </summary>
        /// <param name="sqlOrSPname">SQL语句或者无参数存储过程名称</param>
        /// <param name="commandType">解释命令字符串类型</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sqlOrSPname, CommandType commandType)
        {
            switch (commandType)
            {
                case CommandType.StoredProcedure:
                    return ExecuteNonQueryTypeStoredProcedure(sqlOrSPname);

                case CommandType.Text:
                    return ExecuteNonQueryTypeText(sqlOrSPname);

                default:
                    return ExecuteNonQueryTypeText("select * from " + sqlOrSPname);

            }

        }
        /// <summary>
        /// 连接执行有参数存储过程并返回受影响行数
        /// </summary>
        /// <param name="sqlOrSPname">有参数存储过程名称</param>
        /// <param name="sqlParameters">SqlParametes数组</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sqlOrSPname, SqlParameter[] sqlParameters)
        {
            return ExecuteNonQueryTypeStoredProcedure(sqlOrSPname, sqlParameters);
        }
        #endregion


        /// <summary>
        /// 返回查询结果集的第一行第一列的值
        /// </summary>
        /// <returns>查询结果第一行第一列的值</returns>
        public static object ExecuteScalar(string sql)
        {
            sqlConnection = new SqlConnection();
            try
            {
                sqlConnection.ConnectionString = strSql;//数据库的信息给到这个连接，

                sqlConnection.Open();//打开连接
                cmd = new SqlCommand();//创建一个查询实例

                cmd.CommandText = sql.ToString();//设置要执行的数据库命令
                cmd.Connection = sqlConnection;//设置查询

                return cmd.ExecuteScalar();//返回结果


            }
            catch (Exception e)
            {

                throw e;//抛出异常

            }
            finally
            {
                sqlConnection.Close();//关闭数据库连接

            }

        }

        #region 查询数据库返回一个DataTable
        /// <summary>
        /// 查询数据库表返回一个只读结果集
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <returns>DataTable</returns>
        public static DataTable GetReadOnlyDataTable(string sql)
        {
            sqlConnection = new SqlConnection();
            try
            {
                sqlConnection.ConnectionString = strSql;
                sqlConnection.Open();
                cmd = new SqlCommand(sql, sqlConnection);//利用SqlCommand构造函数的重载，可以在实例化时就给相对应得属性赋值
                SqlDataReader reader = cmd.ExecuteReader();//根据sql读取数据库表，返回一个只读结果集
                DataTable dt = new DataTable();//创建一个DataTable对象
                dt.Load(reader);//将读到的结果集填充到DataTable对象    
                return dt;
            }
            catch (Exception e)
            {

                throw e;//抛出异常

            }
            finally
            {
                sqlConnection.Close();//关闭数据库连接

            }
        }
        /// <summary>
        /// 查询数据库表返回一个可读写可操作的结果集
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetReadWwriteDataTable(string sql)
        {
            sqlConnection = new SqlConnection();
            try
            {
                sqlConnection.ConnectionString = strSql;

                SqlDataAdapter adapter = new SqlDataAdapter(sql, sqlConnection);//返回一个可读写可操作的结果集
                DataTable dt = new DataTable();//创建一个DataTable对象
                adapter.Fill(dt);//将结果集填充到DataTable对象 
                return dt;
            }
            catch (Exception e)
            {

                throw e;//抛出异常

            }
            finally
            {
                sqlConnection.Close();//关闭数据库连接

            }
        }
        #endregion







    }
}