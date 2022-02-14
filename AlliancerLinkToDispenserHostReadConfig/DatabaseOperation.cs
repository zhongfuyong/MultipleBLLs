using System;
using System.Collections.Generic;
using Dapper;
using System.Data.SqlClient;

namespace AlliancerLinkToDispenserHostReadConfig
{
    /// <summary>
    /// 数据库操作类
    /// </summary>
    public class DatabaseOperation
    {
        /// <summary>
        /// 获取配置参数数据
        /// </summary>
        /// <param name="Con"></param>
        /// <returns></returns>
        public List<t_CfgTreatmentParams> GetT_CfgsTP(SqlConnection Con) 
        {
            string sql = "SELECT * FROM [dbo].[t_CfgTreatmentParams] ORDER BY [c_Id]";
            return Con.Query<t_CfgTreatmentParams>(sql) as List<t_CfgTreatmentParams>;
        }
        /// <summary>
        /// 获取批次文本数据
        /// </summary>
        /// <returns></returns>
        public List<t_CfgBatchTexts> GetCfgBatchTexts(SqlConnection Con)
        {
            string sql = "SELECT * FROM [dbo].[t_CfgBatchTexts] ORDER BY [c_Id]";
            return Con.Query<t_CfgBatchTexts>(sql) as List<t_CfgBatchTexts>;
        }
        /// <summary>
        /// 获取批次参数数据
        /// </summary>
        /// <returns></returns>
        public List<t_CfgBatchParams> GetCfgBatchParams(SqlConnection Con)
        {
            string sql = "SELECT * FROM [dbo].[t_CfgBatchParams] ORDER BY [c_Id]";
            return Con.Query<t_CfgBatchParams>(sql) as List<t_CfgBatchParams>;
        }
        /// <summary>
        /// 获取批次消耗数据
        /// </summary>
        /// <returns></returns>
        public List<t_CfgConsumptions> GetCfgConsumptions(SqlConnection Con)
        {
            string sql = "SELECT * FROM [dbo].[t_CfgConsumptions] ORDER BY [c_Id]";
            return Con.Query<t_CfgConsumptions>(sql) as List<t_CfgConsumptions>;
        }
        /// <summary>
        /// 获取批次功能停止数据
        /// </summary>
        /// <returns></returns>
        public List<t_CfgFctStops> GetCfgFctStops(SqlConnection Con)
        {
            string sql = "SELECT * FROM [dbo].[t_CfgFctStops] ORDER BY [c_Id]";
            return Con.Query<t_CfgFctStops>(sql) as List<t_CfgFctStops>;
        }
        /// <summary>
        /// 获取批次停机数据
        /// </summary>
        /// <returns></returns>
        public List<t_CfgStopDecls> GetCfgStopDecls(SqlConnection Con)
        {
            string sql = "SELECT * FROM [dbo].[t_CfgStopDecls] ORDER BY [c_Id]";
            return Con.Query<t_CfgStopDecls>(sql) as List<t_CfgStopDecls>;
        }
        /// <summary>
        /// 获取批次警报数据
        /// </summary>
        /// <returns></returns>
        public List<t_CfgAlarms> GetCfgAlarms(SqlConnection Con)
        {
            string sql = "SELECT * FROM [dbo].[t_CfgAlarms] ORDER BY [c_Id]";
            return Con.Query<t_CfgAlarms>(sql) as List<t_CfgAlarms>;
        }
        /// <summary>
        /// 获取批次功能组数据
        /// </summary>
        /// <returns></returns>
        public List<t_CfgFunctions> GetCfgFunctions(SqlConnection Con)
        {
            string sql = "SELECT * FROM [dbo].[t_CfgFunctions] ORDER BY [c_Id]";
            return Con.Query<t_CfgFunctions>(sql) as List<t_CfgFunctions>;
        }
    }
}
