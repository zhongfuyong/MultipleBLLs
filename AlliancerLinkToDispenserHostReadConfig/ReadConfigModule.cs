using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlliancerLinkToDispenserHostReadConfig
{
    /// <summary>
    /// 主机导入导出设置实体类
    /// </summary>
    public class InputOutputSet_Host
    {
        /// <summary>
        /// 基础信息
        /// </summary>
        public string Information { get; set; }
        /// <summary>
        /// 导入导出数据集合
        /// </summary>
        public List<InputOutputData> InputOutputDatas { get; set; }
    }
    /// <summary>
    /// 导入导出数据
    /// </summary>
    public class InputOutputData
    {
        /// <summary>
        /// 数据排序号
        /// </summary>
        public string Index { get; set; }
        /// <summary>
        /// 是否输入输出   true：导入 false：导出
        /// </summary>
        public bool ImportExport { get; set; }
        /// <summary>
        /// FileID
        /// </summary>
        public string FileID { get; set; }
        /// <summary>
        /// 列表名
        /// </summary>
        public string ListName { get; set; }
        /// <summary>
        /// Xml文件夹路径
        /// </summary>
        public string FolderPath { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 路径模块单选下标
        /// </summary>
        public string PathSelectSubscript { get; set; }
        /// <summary>
        /// 扩展文件格式
        /// </summary>
        public string XmlExtendedFormat { get; set; }
        /// <summary>
        /// 日志文件夹路径
        /// </summary>
        public string LogFolderPath { get; set; }
        /// <summary>
        /// 日志文件名称
        /// </summary>
        public string LogFileName { get; set; }
        /// <summary>
        /// 日志到文件勾选
        /// </summary>
        public string LoginToFile { get; set; }
        /// <summary>
        /// 登录调试勾选
        /// </summary>
        public string LoginDebug { get; set; }
        /// <summary>
        /// 激活勾选
        /// </summary>
        public string Activation { get; set; }
        /// <summary>
        /// 机台数
        /// </summary>
        public string MachineCount { get; set; }
        /// <summary>
        /// 已选机台数据集合
        /// </summary>
        public List<string> MachineSelects { get; set; }
        /// <summary>
        /// 事件触发器1数据
        /// </summary>
        public string TrigerOneHexadecimal { get; set; }
        /// <summary>
        /// 事件触发数据1
        /// </summary>
        public List<EventTriggerData> TrigerOneData { get; set; } = new List<EventTriggerData>();
        /// <summary>
        /// 执行延迟秒
        /// </summary>
        public string DelaySeconds { get; set; }
        /// <summary>
        /// 事件触发器4数据
        /// </summary>
        public string TrigerFourHexadecimal { get; set; }
        /// <summary>
        /// 事件触发数据4
        /// </summary>
        public List<EventTriggerData> TrigerFourData { get; set; } = new List<EventTriggerData>();
        /// <summary>
        /// 事件触发器2数据
        /// </summary>
        public string TrigerTwoHexadecimal { get; set; }
        /// <summary>
        /// 事件触发数据2
        /// </summary>
        public List<EventTriggerData> TrigerTwoData { get; set; } = new List<EventTriggerData>();
        /// <summary>
        /// 类型选择
        /// </summary>
        public string TypeCheck { get; set; }
        /// <summary>
        /// 数据库类型选择
        /// </summary>
        public string DatabaseType { get; set; }
        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DatabaseName { get; set; }
        /// <summary>
        /// 服务器名称
        /// </summary>
        public string ServerName { get; set; }
        /// <summary>
        /// 数据库登录名
        /// </summary>
        public string DatabaseLoginName { get; set; }
        /// <summary>
        /// 数据库登录密码
        /// </summary>
        public string DatabasePassword { get; set; }
        /// <summary>
        /// 文本格式勾选
        /// </summary>
        public string TextFormat { get; set; }
        /// <summary>
        /// 事件触发器3总数
        /// </summary>
        public string EventTrigerThreeCount { get; set; }
        /// <summary>
        /// 基于时间勾选
        /// </summary>
        public string TimeBased { get; set; }
        /// <summary>
        /// 事件触发器3第一个开关
        /// </summary>
        public string FritEventTrigerThreeSwitch { get; set; }
        /// <summary>
        /// 启动间隔
        /// </summary>
        public string StartInterval { get; set; }
        /// <summary>
        /// 结束间隔
        /// </summary>
        public string EndInterval { get; set; }
        /// <summary>
        /// 固定时间
        /// </summary>
        public string FixedTime { get; set; }
        /// <summary>
        /// 使间隔勾选
        /// </summary>
        public string MakeInterval { get; set; }
        /// <summary>
        /// 星期勾选
        /// </summary>
        public List<int> WeekCheckData { get; set; }
        /// <summary>
        /// 事件触发器3信息
        /// </summary>
        public List<string> TrigerThreeHexadecimal { get; set; }
        /// <summary>
        /// 事件触发数据3
        /// </summary>
        public List<EventTriggerData> TrigerThreeData { get; set; } = new List<EventTriggerData>();

        #region 数据库字段数据
        /// <summary>
        /// 首条字段数据
        /// </summary>
        public List<DatabaseFieldData> DatabaseFirstFieldData { get; set; }
        /// <summary>
        /// 首条字符串长度
        /// </summary>
        public string FirstStringLength { get; set; }
        /// <summary>
        /// 首条表名称
        /// </summary>
        public string FirstTableName { get; set; }
        /// <summary>
        /// 首条标签
        /// </summary>
        public string FirstLabel { get; set; }
        /// <summary>
        /// 首条字段分隔符1
        /// </summary>
        public string FirstFieldSeparator1 { get; set; }
        /// <summary>
        /// 首条字段分隔符2
        /// </summary>
        public string FirstFieldSeparator2 { get; set; }
        /// <summary>
        /// 首条记录分隔符
        /// </summary>
        public string FirstRecordSeparator { get; set; }

        /// <summary>
        /// 次条字段数据
        /// </summary>
        public List<DatabaseFieldData> DatabaseSecondFieldData { get; set; }
        /// <summary>
        /// 次条字符串长度
        /// </summary>
        public string SecondStringLength { get; set; }
        /// <summary>
        /// 次条表名称
        /// </summary>
        public string SecondTableName { get; set; }
        /// <summary>
        /// 次条标签
        /// </summary>
        public string SecondLabel { get; set; }
        /// <summary>
        /// 次条字段分隔符1
        /// </summary>
        public string SecondFieldSeparator1 { get; set; }
        /// <summary>
        /// 次条字段分隔符
        /// </summary>
        public string SecondFieldSeparator2 { get; set; }
        /// <summary>
        /// 次条记录分隔符
        /// </summary>
        public string SecondRecordSeparator { get; set; }
        #endregion

        /// <summary>
        /// 优先级
        /// </summary>
        public string Priority { get; set; } = "10";
    }
    /// <summary>
    /// 事件触发器数据
    /// </summary>
    public class EventTriggerData
    {
        /// <summary>
        /// 中文名
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// 英文名
        /// </summary>
        public string SecondName { get; set; }
        /// <summary>
        /// 定位
        /// </summary>
        public string Index { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public string Num { get; set; }
        /// <summary>
        /// 导入导出标记
        /// </summary>
        public string Sign { get; set; }
        /// <summary>
        /// 对应OPC监听的项
        /// </summary>
        public string OPCItem { get; set; }
    }
    /// <summary>
    /// 数据库字段数据
    /// </summary>
    public class DatabaseFieldData
    {
        /// <summary>
        /// 排序号
        /// </summary>
        public string SortNum { get; set; }
        /// <summary>
        /// 字段
        /// </summary>
        public string FieldSign { get; set; }
        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 格式选择下标
        /// </summary>
        public string FormatIndex { get; set; }
        /// <summary>
        /// 除-数据
        /// </summary>
        public string DivisionData { get; set; }
        /// <summary>
        /// 乘-数据
        /// </summary>
        public string MultiplicationData { get; set; }
        /// <summary>
        /// 固定值
        /// </summary>
        public string FixedValue { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 字段名
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// 对齐
        /// </summary>
        public string Alignment { get; set; }
        /// <summary>
        /// 前导字符
        /// </summary>
        public string LeadingChar { get; set; }
        /// <summary>
        /// 字符串长度
        /// </summary>
        public string StringLenght { get; set; }
        /// <summary>
        /// 对用数据库的字段数据
        /// </summary>
        public string DatabaseField { get; set; }
        /// <summary>
        /// 导入导出标记
        /// </summary>
        public string DatabaseSign { get; set; }
    }
    /// <summary>
    /// 字段数据
    /// </summary>
    public class FieldData
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        public string FieldType { get; set; }
        /// <summary>
        /// 字段16进制数据
        /// </summary>
        public string FieldHexadecimal { get; set; }
        /// <summary>
        /// 首条或次条记录标记
        /// </summary>
        public string FirstOrSecond { get; set; }
        /// <summary>
        /// 对应的数据库字段
        /// </summary>
        public string DatabaseField { get; set; }
    }
    /// <summary>
    /// TreatmentParams
    /// </summary>
    public class t_CfgTreatmentParams
    {
        #region 私有变量
        private Int32 _c_id = Int32.MinValue;
        private string _c_name = null;
        private Int32 _c_paramtype = Int32.MinValue;
        private string _c_unit = null;
        private Int32 _c_decformat = Int32.MinValue;
        private double _c_minimum = double.MinValue;
        private double _c_maximum = double.MinValue;
        private double _c_default = double.MinValue;
        private Int32 _c_activeforall = Int32.MinValue;
        #endregion

        #region 公共属性
        /// <summary>
        /// 主键 c_Id(NOT NULL)
        /// </summary>
        public Int32 c_Id
        {
            set { _c_id = value; }
            get { return _c_id; }
        }
        /// <summary>
        /// c_Name
        /// </summary>
        public string c_Name
        {
            set { _c_name = value; }
            get { return _c_name; }
        }
        /// <summary>
        /// c_ParamType
        /// </summary>
        public Int32 c_ParamType
        {
            set { _c_paramtype = value; }
            get { return _c_paramtype; }
        }
        /// <summary>
        /// c_Unit
        /// </summary>
        public string c_Unit
        {
            set { _c_unit = value; }
            get { return _c_unit; }
        }
        /// <summary>
        /// c_DecFormat
        /// </summary>
        public Int32 c_DecFormat
        {
            set { _c_decformat = value; }
            get { return _c_decformat; }
        }
        /// <summary>
        /// c_Minimum
        /// </summary>
        public double c_Minimum
        {
            set { _c_minimum = value; }
            get { return _c_minimum; }
        }
        /// <summary>
        /// c_Maximum
        /// </summary>
        public double c_Maximum
        {
            set { _c_maximum = value; }
            get { return _c_maximum; }
        }
        /// <summary>
        /// c_Default
        /// </summary>
        public double c_Default
        {
            set { _c_default = value; }
            get { return _c_default; }
        }
        /// <summary>
        /// c_ActiveForAll
        /// </summary>
        public Int32 c_ActiveForAll
        {
            set { _c_activeforall = value; }
            get { return _c_activeforall; }
        }
        #endregion
    }
    /// <summary>
	/// 实体 t_Machines
	/// </summary>
    public class t_Machines
    {
        #region 构造函数
        /// <summary>
        /// 实体 t_Machines
        /// </summary>
        public t_Machines() { }
        #endregion

        #region 私有变量
        private short _c_id = short.MaxValue;
        private string _c_name = null;
        private string _c_longname = null;
        private short _c_machinetype_id = short.MaxValue;
        private short _c_machineproperty_id = short.MaxValue;
        private short _c_virtualmachinegroup_id = short.MaxValue;
        private string _c_csname = null;
        private short _c_csport = short.MaxValue;
        private short _c_duaddr = short.MaxValue;
        private short _c_duport = short.MaxValue;
        private short _c_praddr = short.MaxValue;
        private short _c_prport = short.MaxValue;
        private short _c_subpraddr = short.MaxValue;
        private string _c_configfilename = null;
        private short _c_canbemaster = short.MaxValue;
        private double _c_minloadweight = double.MinValue;
        private double _c_maxloadweight = double.MinValue;
        private double _c_minloadfluid = double.MinValue;
        private double _c_maxloadfluid = double.MinValue;
        private Int32 _c_yearofconstr = Int32.MinValue;
        private string _c_serialno = null;
        private string _c_manufacturer = null;
        private Int32 _c_ctrlyearofconstr = Int32.MinValue;
        private string _c_ctrlserialno = null;
        private string _c_ctrlmanufacturer = null;
        private double _c_maxvolumetank1 = double.MinValue;
        private double _c_maxvolumetank2 = double.MinValue;
        private double _c_maxvolumetank3 = double.MinValue;
        private short _c_isdosingmachine = short.MaxValue;
        private short _c_showinplanning = short.MaxValue;
        private short _c_isdummymachine = short.MaxValue;
        private short _c_isdissolvingmachine = short.MaxValue;
        private string _c_opcname = null;
        private short _c_istankmonitoringmachine = short.MaxValue;
        private short _c_istankmanagementmachine = short.MaxValue;
        private short _c_linkedbatches = short.MaxValue;
        private short _c_uselinkedbatchessequence = short.MaxValue;
        private short _c_isprintingmachine = short.MaxValue;
        #endregion

        #region 公共属性
        /// <summary>
        /// 主键 c_Id(NOT NULL)
        /// </summary>
        public short c_Id
        {
            set { _c_id = value; }
            get { return _c_id; }
        }
        /// <summary>
        /// c_Name(NOT NULL)
        /// </summary>
        public string c_Name
        {
            set { _c_name = value; }
            get { return _c_name; }
        }
        /// <summary>
        /// c_LongName
        /// </summary>
        public string c_LongName
        {
            set { _c_longname = value; }
            get { return _c_longname; }
        }
        /// <summary>
        /// c_MachineType_Id
        /// </summary>
        public short c_MachineType_Id
        {
            set { _c_machinetype_id = value; }
            get { return _c_machinetype_id; }
        }
        /// <summary>
        /// c_MachineProperty_Id(NOT NULL)
        /// </summary>
        public short c_MachineProperty_Id
        {
            set { _c_machineproperty_id = value; }
            get { return _c_machineproperty_id; }
        }
        /// <summary>
        /// c_VirtualMachineGroup_Id(NOT NULL)
        /// </summary>
        public short c_VirtualMachineGroup_Id
        {
            set { _c_virtualmachinegroup_id = value; }
            get { return _c_virtualmachinegroup_id; }
        }
        /// <summary>
        /// c_CsName(NOT NULL)
        /// </summary>
        public string c_CsName
        {
            set { _c_csname = value; }
            get { return _c_csname; }
        }
        /// <summary>
        /// c_CsPort(NOT NULL)
        /// </summary>
        public short c_CsPort
        {
            set { _c_csport = value; }
            get { return _c_csport; }
        }
        /// <summary>
        /// c_DUAddr(NOT NULL)
        /// </summary>
        public short c_DUAddr
        {
            set { _c_duaddr = value; }
            get { return _c_duaddr; }
        }
        /// <summary>
        /// c_DUPort(NOT NULL)
        /// </summary>
        public short c_DUPort
        {
            set { _c_duport = value; }
            get { return _c_duport; }
        }
        /// <summary>
        /// c_PrAddr(NOT NULL)
        /// </summary>
        public short c_PrAddr
        {
            set { _c_praddr = value; }
            get { return _c_praddr; }
        }
        /// <summary>
        /// c_PrPort(NOT NULL)
        /// </summary>
        public short c_PrPort
        {
            set { _c_prport = value; }
            get { return _c_prport; }
        }
        /// <summary>
        /// c_SubPrAddr(NOT NULL)
        /// </summary>
        public short c_SubPrAddr
        {
            set { _c_subpraddr = value; }
            get { return _c_subpraddr; }
        }
        /// <summary>
        /// c_ConfigFilename
        /// </summary>
        public string c_ConfigFilename
        {
            set { _c_configfilename = value; }
            get { return _c_configfilename; }
        }
        /// <summary>
        /// c_CanBeMaster
        /// </summary>
        public short c_CanBeMaster
        {
            set { _c_canbemaster = value; }
            get { return _c_canbemaster; }
        }
        /// <summary>
        /// c_MinLoadWeight
        /// </summary>
        public double c_MinLoadWeight
        {
            set { _c_minloadweight = value; }
            get { return _c_minloadweight; }
        }
        /// <summary>
        /// c_MaxLoadWeight
        /// </summary>
        public double c_MaxLoadWeight
        {
            set { _c_maxloadweight = value; }
            get { return _c_maxloadweight; }
        }
        /// <summary>
        /// c_MinLoadFluid
        /// </summary>
        public double c_MinLoadFluid
        {
            set { _c_minloadfluid = value; }
            get { return _c_minloadfluid; }
        }
        /// <summary>
        /// c_MaxLoadFluid
        /// </summary>
        public double c_MaxLoadFluid
        {
            set { _c_maxloadfluid = value; }
            get { return _c_maxloadfluid; }
        }
        /// <summary>
        /// c_YearOfConstr
        /// </summary>
        public Int32 c_YearOfConstr
        {
            set { _c_yearofconstr = value; }
            get { return _c_yearofconstr; }
        }
        /// <summary>
        /// c_SerialNo
        /// </summary>
        public string c_SerialNo
        {
            set { _c_serialno = value; }
            get { return _c_serialno; }
        }
        /// <summary>
        /// c_ManuFacturer
        /// </summary>
        public string c_ManuFacturer
        {
            set { _c_manufacturer = value; }
            get { return _c_manufacturer; }
        }
        /// <summary>
        /// c_CtrlYearOfConstr
        /// </summary>
        public Int32 c_CtrlYearOfConstr
        {
            set { _c_ctrlyearofconstr = value; }
            get { return _c_ctrlyearofconstr; }
        }
        /// <summary>
        /// c_CtrlSerialNo
        /// </summary>
        public string c_CtrlSerialNo
        {
            set { _c_ctrlserialno = value; }
            get { return _c_ctrlserialno; }
        }
        /// <summary>
        /// c_CtrlManuFacturer
        /// </summary>
        public string c_CtrlManuFacturer
        {
            set { _c_ctrlmanufacturer = value; }
            get { return _c_ctrlmanufacturer; }
        }
        /// <summary>
        /// c_MaxVolumeTank1
        /// </summary>
        public double c_MaxVolumeTank1
        {
            set { _c_maxvolumetank1 = value; }
            get { return _c_maxvolumetank1; }
        }
        /// <summary>
        /// c_MaxVolumeTank2
        /// </summary>
        public double c_MaxVolumeTank2
        {
            set { _c_maxvolumetank2 = value; }
            get { return _c_maxvolumetank2; }
        }
        /// <summary>
        /// c_MaxVolumeTank3
        /// </summary>
        public double c_MaxVolumeTank3
        {
            set { _c_maxvolumetank3 = value; }
            get { return _c_maxvolumetank3; }
        }
        /// <summary>
        /// c_IsDosingMachine
        /// </summary>
        public short c_IsDosingMachine
        {
            set { _c_isdosingmachine = value; }
            get { return _c_isdosingmachine; }
        }
        /// <summary>
        /// c_ShowInPlanning
        /// </summary>
        public short c_ShowInPlanning
        {
            set { _c_showinplanning = value; }
            get { return _c_showinplanning; }
        }
        /// <summary>
        /// c_IsDummyMachine
        /// </summary>
        public short c_IsDummyMachine
        {
            set { _c_isdummymachine = value; }
            get { return _c_isdummymachine; }
        }
        /// <summary>
        /// c_IsDissolvingMachine
        /// </summary>
        public short c_IsDissolvingMachine
        {
            set { _c_isdissolvingmachine = value; }
            get { return _c_isdissolvingmachine; }
        }
        /// <summary>
        /// c_OPCName(NOT NULL)
        /// </summary>
        public string c_OPCName
        {
            set { _c_opcname = value; }
            get { return _c_opcname; }
        }
        /// <summary>
        /// c_IsTankMonitoringMachine(NOT NULL)
        /// </summary>
        public short c_IsTankMonitoringMachine
        {
            set { _c_istankmonitoringmachine = value; }
            get { return _c_istankmonitoringmachine; }
        }
        /// <summary>
        /// c_IsTankManagementMachine(NOT NULL)
        /// </summary>
        public short c_IsTankManagementMachine
        {
            set { _c_istankmanagementmachine = value; }
            get { return _c_istankmanagementmachine; }
        }
        /// <summary>
        /// c_LinkedBatches(NOT NULL)
        /// </summary>
        public short c_LinkedBatches
        {
            set { _c_linkedbatches = value; }
            get { return _c_linkedbatches; }
        }
        /// <summary>
        /// c_UseLinkedBatchesSequence(NOT NULL)
        /// </summary>
        public short c_UseLinkedBatchesSequence
        {
            set { _c_uselinkedbatchessequence = value; }
            get { return _c_uselinkedbatchessequence; }
        }
        /// <summary>
        /// c_IsPrintingMachine(NOT NULL)
        /// </summary>
        public short c_IsPrintingMachine
        {
            set { _c_isprintingmachine = value; }
            get { return _c_isprintingmachine; }
        }
        #endregion
    }
    /// <summary>
    /// 配置批次文本数据
    /// </summary>
    public class t_CfgBatchTexts
    {
        #region 构造函数
        /// <summary>
        /// 实体 t_CfgBatchTexts
        /// </summary>
        public t_CfgBatchTexts() { }
        #endregion

        #region 私有变量
        private Int32 _c_id = Int32.MinValue;
        private string _c_name = null;
        private Int32 _c_edit = Int32.MinValue;
        private Int32 _c_listidx = Int32.MinValue;
        private Int32 _c_activeforall = Int32.MinValue;
        #endregion

        #region 公共属性
        /// <summary>
        /// 主键 c_Id(NOT NULL)
        /// </summary>
        public Int32 c_Id
        {
            set { _c_id = value; }
            get { return _c_id; }
        }
        /// <summary>
        /// c_Name
        /// </summary>
        public string c_Name
        {
            set { _c_name = value; }
            get { return _c_name; }
        }
        /// <summary>
        /// c_Edit
        /// </summary>
        public Int32 c_Edit
        {
            set { _c_edit = value; }
            get { return _c_edit; }
        }
        /// <summary>
        /// c_ListIdx
        /// </summary>
        public Int32 c_ListIdx
        {
            set { _c_listidx = value; }
            get { return _c_listidx; }
        }
        /// <summary>
        /// c_ActiveForAll
        /// </summary>
        public Int32 c_ActiveForAll
        {
            set { _c_activeforall = value; }
            get { return _c_activeforall; }
        }
        #endregion
    }
    /// <summary>
    /// 配置批次参数数据
    /// </summary>
    public class t_CfgBatchParams
    {

        #region 私有变量
        private Int32 _c_id = Int32.MinValue;
        private string _c_name = null;
        private Int32 _c_paramtype = Int32.MinValue;
        private string _c_unit = null;
        private Int32 _c_decformat = Int32.MinValue;
        private double _c_minimum = double.MinValue;
        private double _c_maximum = double.MinValue;
        private double _c_default = double.MinValue;
        private Int32 _c_activeforall = Int32.MinValue;
        private Int32 _c_paramcategory = Int32.MinValue;
        #endregion

        #region 公共属性
        /// <summary>
        /// 主键 c_Id(NOT NULL)
        /// </summary>
        public Int32 c_Id
        {
            set { _c_id = value; }
            get { return _c_id; }
        }
        /// <summary>
        /// c_Name
        /// </summary>
        public string c_Name
        {
            set { _c_name = value; }
            get { return _c_name; }
        }
        /// <summary>
        /// c_ParamType
        /// </summary>
        public Int32 c_ParamType
        {
            set { _c_paramtype = value; }
            get { return _c_paramtype; }
        }
        /// <summary>
        /// c_Unit
        /// </summary>
        public string c_Unit
        {
            set { _c_unit = value; }
            get { return _c_unit; }
        }
        /// <summary>
        /// c_DecFormat
        /// </summary>
        public Int32 c_DecFormat
        {
            set { _c_decformat = value; }
            get { return _c_decformat; }
        }
        /// <summary>
        /// c_Minimum
        /// </summary>
        public double c_Minimum
        {
            set { _c_minimum = value; }
            get { return _c_minimum; }
        }
        /// <summary>
        /// c_Maximum
        /// </summary>
        public double c_Maximum
        {
            set { _c_maximum = value; }
            get { return _c_maximum; }
        }
        /// <summary>
        /// c_Default
        /// </summary>
        public double c_Default
        {
            set { _c_default = value; }
            get { return _c_default; }
        }
        /// <summary>
        /// c_ActiveForAll
        /// </summary>
        public Int32 c_ActiveForAll
        {
            set { _c_activeforall = value; }
            get { return _c_activeforall; }
        }
        /// <summary>
        /// c_ParamCategory
        /// </summary>
        public Int32 c_ParamCategory
        {
            set { _c_paramcategory = value; }
            get { return _c_paramcategory; }
        }
        #endregion
    }
    /// <summary>
    /// 配置消耗数据
    /// </summary>
    public class t_CfgConsumptions
    {
        #region 构造函数
        /// <summary>
        /// 实体 t_CfgConsumptions
        /// </summary>
        public t_CfgConsumptions() { }
        #endregion

        #region 私有变量
        private Int32 _c_id = Int32.MinValue;
        private string _c_name = null;
        private string _c_unit = null;
        private double _c_costfactor = double.MinValue;
        private string _c_currency = null;
        #endregion

        #region 公共属性
        /// <summary>
        /// 主键 c_Id(NOT NULL)
        /// </summary>
        public Int32 c_Id
        {
            set { _c_id = value; }
            get { return _c_id; }
        }
        /// <summary>
        /// c_Name
        /// </summary>
        public string c_Name
        {
            set { _c_name = value; }
            get { return _c_name; }
        }
        /// <summary>
        /// c_Unit
        /// </summary>
        public string c_Unit
        {
            set { _c_unit = value; }
            get { return _c_unit; }
        }
        /// <summary>
        /// c_CostFactor
        /// </summary>
        public double c_CostFactor
        {
            set { _c_costfactor = value; }
            get { return _c_costfactor; }
        }
        /// <summary>
        /// c_Currency
        /// </summary>
        public string c_Currency
        {
            set { _c_currency = value; }
            get { return _c_currency; }
        }
        #endregion
    }
    /// <summary>
    /// 配置功能停止数据
    /// </summary>
    public class t_CfgFctStops
    {
        #region 构造函数
        /// <summary>
        /// 实体 t_CfgFctStops
        /// </summary>
        public t_CfgFctStops() { }
        #endregion

        #region 私有变量
        private Int32 _c_id = Int32.MinValue;
        private string _c_name = null;
        #endregion

        #region 公共属性
        /// <summary>
        /// 主键 c_Id(NOT NULL)
        /// </summary>
        public Int32 c_Id
        {
            set { _c_id = value; }
            get { return _c_id; }
        }
        /// <summary>
        /// c_Name
        /// </summary>
        public string c_Name
        {
            set { _c_name = value; }
            get { return _c_name; }
        }
        #endregion
    }
    /// <summary>
    /// 配置批次停机数据
    /// </summary>
    public class t_CfgStopDecls
    {
        #region 构造函数
        /// <summary>
        /// 实体 t_CfgStopDecls
        /// </summary>
        public t_CfgStopDecls() { }
        #endregion

        #region 私有变量
        private Int32 _c_id = Int32.MinValue;
        private string _c_name = null;
        private Int32 _c_rgbcolor = Int32.MinValue;
        #endregion

        #region 公共属性
        /// <summary>
        /// 主键 c_Id(NOT NULL)
        /// </summary>
        public Int32 c_Id
        {
            set { _c_id = value; }
            get { return _c_id; }
        }
        /// <summary>
        /// c_Name
        /// </summary>
        public string c_Name
        {
            set { _c_name = value; }
            get { return _c_name; }
        }
        /// <summary>
        /// c_RGBColor
        /// </summary>
        public Int32 c_RGBColor
        {
            set { _c_rgbcolor = value; }
            get { return _c_rgbcolor; }
        }
        #endregion
    }
    /// <summary>
    /// 配置警告数据
    /// </summary>
    public class t_CfgAlarms
    {
        #region 构造函数
        /// <summary>
        /// 实体 t_CfgAlarms
        /// </summary>
        public t_CfgAlarms() { }
        #endregion

        #region 私有变量
        private Int32 _c_id = Int32.MinValue;
        private string _c_name = null;
        private Int32 _c_rgbcolor = Int32.MinValue;
        #endregion

        #region 公共属性
        /// <summary>
        /// 主键 c_Id(NOT NULL)
        /// </summary>
        public Int32 c_Id
        {
            set { _c_id = value; }
            get { return _c_id; }
        }
        /// <summary>
        /// c_Name
        /// </summary>
        public string c_Name
        {
            set { _c_name = value; }
            get { return _c_name; }
        }
        /// <summary>
        /// c_RGBColor
        /// </summary>
        public Int32 c_RGBColor
        {
            set { _c_rgbcolor = value; }
            get { return _c_rgbcolor; }
        }
        #endregion
    }
    /// <summary>
    /// 配置功能组数据
    /// </summary>
    public class t_CfgFunctions
    {
        #region 构造函数
        /// <summary>
        /// 实体 t_CfgFunctions
        /// </summary>
        public t_CfgFunctions() { }
        #endregion

        #region 私有变量
        private Int32 _c_id = Int32.MinValue;
        private string _c_name = null;
        private Int32 _c_rgbcolor = Int32.MinValue;
        private Int32 _c_activeforall = Int32.MinValue;
        #endregion

        #region 公共属性
        /// <summary>
        /// 主键 c_Id(NOT NULL)
        /// </summary>
        public Int32 c_Id
        {
            set { _c_id = value; }
            get { return _c_id; }
        }
        /// <summary>
        /// c_Name
        /// </summary>
        public string c_Name
        {
            set { _c_name = value; }
            get { return _c_name; }
        }
        /// <summary>
        /// c_RGBColor
        /// </summary>
        public Int32 c_RGBColor
        {
            set { _c_rgbcolor = value; }
            get { return _c_rgbcolor; }
        }
        /// <summary>
        /// c_ActiveForAll
        /// </summary>
        public Int32 c_ActiveForAll
        {
            set { _c_activeforall = value; }
            get { return _c_activeforall; }
        }
        #endregion
    }
}
