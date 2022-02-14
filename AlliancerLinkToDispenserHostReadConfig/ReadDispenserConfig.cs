using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data.SqlClient;
using System.Text;

namespace AlliancerLinkToDispenserHostReadConfig
{
    /// <summary>
    /// 读取配置文件类
    /// </summary>
    public class ReadDispenserConfig
    {
        private string fieldContent = "";
        /// <summary>
        /// 字段配置文件内容
        /// </summary>
        public string FieldContent
        {
            get { return fieldContent; }
            set
            {
                if (value != fieldContent)
                {
                    FieldDatas = GetFieldDatasByContent(value);
                }
                fieldContent = value;
            }
        }

        /// <summary>
        /// 事件触发器文件路径
        /// </summary>
        public string EventTriggerFile;

        /// <summary>
        /// 字段配置文件类
        /// </summary>
        public List<FieldData> FieldDatas { get; set; }
        /// <summary>
        /// 所有事件触发器数据集合
        /// </summary>
        private List<EventTriggerData> EventTriggerDatas { get; set; } = new List<EventTriggerData>();

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="FiledPath">字段配置文件地址</param>
        /// <param name="Con">SQL连接</param>
        /// <param name="TriggerFile">事件触发器文件路径</param>
        public ReadDispenserConfig(string FiledPath,SqlConnection Con,string TriggerFile) 
        {
            if (!File.Exists(FiledPath))
            {
                throw new FileNotFoundException(FiledPath);
            }
            if (!File.Exists(TriggerFile))
            {
                throw new FileNotFoundException(TriggerFile);
            }
            EventTriggerFile = TriggerFile;
            //读取配置字段数据
            FieldContent = File.ReadAllText(FiledPath);
            FieldDatas = GetFieldDatasByContent(FieldContent);
            //动态添加字段数据与修正字段名称
            SetFiledDataName(Con);
            //生成事件触发实体类数据
            GetEventTriggerDatas(Con);
        }
        /// <summary>
        /// 字符串数组转实体类
        /// </summary>
        /// <param name="FilePath">Bin文件地址</param>
        /// <returns></returns>
        public InputOutputSet_Host ArrayToEntityClass(string FilePath)
        {
            int index = 0;
            if (FilePath == null)
            {
                throw new ArgumentNullException(FilePath);
            }
            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException(FilePath);
            }
            //分割Bin文件数据
            string[] Array = GetBinTOHexadecimal(FilePath).Split(new[] { "0B 00 FF FE FF" }, StringSplitOptions.None);
            //生成每个连接数据实体类
            InputOutputSet_Host inputOutputSet_Host = new InputOutputSet_Host
            {
                InputOutputDatas = new List<InputOutputData>(),
                Information = Array[0]
            };
            for (int i = 1; i < Array.Length; i++)
            {
                string[] Data = Array[i].Split(new[] { "FF FE FF" }, StringSplitOptions.None);
                InputOutputData inputOutputData = GenerateEntityClass(Data);
                inputOutputSet_Host.InputOutputDatas.Add(inputOutputData);
                //将导出数据添加进事件触发数据
                if (!inputOutputData.ImportExport)
                {
                    EventTriggerData eventTriggerData = new EventTriggerData();
                    eventTriggerData.FirstName = inputOutputData.ListName;
                    eventTriggerData.SecondName = inputOutputData.ListName;
                    eventTriggerData.Index = "5";
                    eventTriggerData.Num = index != 0 ? Math.Pow(2, index).ToString() : "1";
                    eventTriggerData.Sign = "1";
                    EventTriggerDatas.Add(eventTriggerData);
                    index++;
                }
            }
            //设置连接排序号
            SetDataIndex(inputOutputSet_Host.InputOutputDatas);
            for (int i = 0; i < inputOutputSet_Host.InputOutputDatas.Count; i++)
            {
                if (inputOutputSet_Host.InputOutputDatas[i].ImportExport)
                {
                    inputOutputSet_Host.InputOutputDatas[i].TrigerFourData = GetEventTriggerDatas(inputOutputSet_Host.InputOutputDatas[i].TrigerFourHexadecimal, EventTriggerDatas, true);
                }
                else
                {
                    inputOutputSet_Host.InputOutputDatas[i].TrigerOneData = GetEventTriggerDatas(inputOutputSet_Host.InputOutputDatas[i].TrigerOneHexadecimal, EventTriggerDatas, false);
                    inputOutputSet_Host.InputOutputDatas[i].TrigerTwoData = GetEventTriggerDatas(inputOutputSet_Host.InputOutputDatas[i].TrigerTwoHexadecimal, EventTriggerDatas, false);
                    inputOutputSet_Host.InputOutputDatas[i].TrigerThreeData = GetEventTrigger3Datas(inputOutputSet_Host.InputOutputDatas[i].TrigerThreeHexadecimal, EventTriggerDatas);
                }
            }
            return inputOutputSet_Host;
        }
        /// <summary>
        /// 生成导入导出数据实体类
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        private InputOutputData GenerateEntityClass(string[] Data)
        {
            int FieldNum = 0;
            InputOutputData inputOutputData = new InputOutputData();
            inputOutputData.Index = "";
            inputOutputData.FileID = Data[0].Substring(70);
            inputOutputData.ListName = HexadecimalToString(Data[1]);
            inputOutputData.FolderPath = HexadecimalToString(Data[2]);
            inputOutputData.FileName = HexadecimalToString(Data[3].Substring(0, Data[3].Length - 6));
            inputOutputData.PathSelectSubscript = HexadecimalToDecimal(Data[3].Substring(Data[3].Length - 6));
            inputOutputData.XmlExtendedFormat = HexadecimalToString(Data[4]);
            inputOutputData.LogFolderPath = HexadecimalToString(Data[5]);
            #region 拼接数据1
            inputOutputData.LogFileName = GetLogFilename(Data[6]);
            string[] FirstSpliceData;
            if (Data.Length == 11)
            {
                FirstSpliceData = GetFirstSpliceData(Data[6]);
            }
            else
            {
                FirstSpliceData = GetFirstSpliceDataHasField(Data);

                inputOutputData.DatabaseSecondFieldData = GetDatabaseFieldData(FirstSpliceData[12]);
                inputOutputData.SecondStringLength = FirstSpliceData[13];
                inputOutputData.SecondTableName = FirstSpliceData[14];
                inputOutputData.SecondLabel = FirstSpliceData[20];
                inputOutputData.SecondFieldSeparator1 = FirstSpliceData[21];
                inputOutputData.SecondFieldSeparator2 = FirstSpliceData[22];
                inputOutputData.SecondRecordSeparator = FirstSpliceData[23];

                inputOutputData.DatabaseFirstFieldData = GetDatabaseFieldData(FirstSpliceData[16]);
                inputOutputData.FirstStringLength = FirstSpliceData[17];
                inputOutputData.FirstTableName = FirstSpliceData[18];
                inputOutputData.FirstLabel = FirstSpliceData[24];
                inputOutputData.FirstFieldSeparator1 = FirstSpliceData[25];
                inputOutputData.FirstFieldSeparator2 = FirstSpliceData[26];
                inputOutputData.FirstRecordSeparator = FirstSpliceData[27];

                if (FirstSpliceData[11] != string.Empty && FirstSpliceData[15] != string.Empty)
                {
                    FieldNum = int.Parse(FirstSpliceData[19]) + 4;
                }
                else
                {
                    FieldNum = int.Parse(FirstSpliceData[19]);
                }
            }
            inputOutputData.LoginToFile = FirstSpliceData[0];
            inputOutputData.LoginDebug = FirstSpliceData[1];
            inputOutputData.Activation = FirstSpliceData[2];
            inputOutputData.MachineCount = FirstSpliceData[3];
            inputOutputData.MachineSelects = new List<string>();
            if (FirstSpliceData[3] != "0")
            {
                string Temporary = "";
                for (int j = 0; j < FirstSpliceData[4].Length; j++)
                {
                    Temporary += FirstSpliceData[4][j];
                    if (j != 0 && (j + 1) % 8 == 0)
                    {
                        inputOutputData.MachineSelects.Add(HexadecimalToDecimal(Temporary.Substring(0, 4)));
                        Temporary = "";
                    }
                }
            }
            inputOutputData.TrigerOneHexadecimal = FirstSpliceData[5];
            inputOutputData.DelaySeconds = FirstSpliceData[6];
            inputOutputData.TrigerFourHexadecimal = FirstSpliceData[7];
            inputOutputData.TrigerTwoHexadecimal = FirstSpliceData[8];
            inputOutputData.TypeCheck = FirstSpliceData[9];
            inputOutputData.DatabaseType = FirstSpliceData[10];
            #endregion

            if (int.Parse(inputOutputData.TypeCheck) % 2 != 0)
            {
                inputOutputData.ImportExport = true;
            }
            else
            {
                inputOutputData.ImportExport = false;
            }
            inputOutputData.DatabaseName = HexadecimalToString(Data[7 + FieldNum]);
            inputOutputData.ServerName = HexadecimalToString(Data[8 + FieldNum]);
            inputOutputData.DatabaseLoginName = HexadecimalToString(Data[9 + FieldNum]);
            #region 拼接数据2
            string[] SecondSpliceData = GetSecondSpliceData(Data[10 + FieldNum]);
            inputOutputData.DatabasePassword = SecondSpliceData[0];
            inputOutputData.TextFormat = SecondSpliceData[1];
            inputOutputData.EventTrigerThreeCount = SecondSpliceData[2];
            inputOutputData.TimeBased = SecondSpliceData[3];
            inputOutputData.FritEventTrigerThreeSwitch = SecondSpliceData[4];
            inputOutputData.StartInterval = SecondSpliceData[5];
            inputOutputData.EndInterval = SecondSpliceData[6];
            inputOutputData.FixedTime = SecondSpliceData[7];
            inputOutputData.MakeInterval = SecondSpliceData[8];
            inputOutputData.WeekCheckData = GetWeekData(int.Parse(SecondSpliceData[9]));
            inputOutputData.Priority = SecondSpliceData[11];
            inputOutputData.TrigerThreeHexadecimal = new List<string>();
            if (SecondSpliceData[2] != "0")
            {
                string Temporary = "";
                inputOutputData.TrigerThreeHexadecimal.Add("0A000000" + SecondSpliceData[10] + "0000000" + SecondSpliceData[4] + "000000");

                string NewArray = Data[10 + FieldNum].Replace(" ", "").Substring(70, (int.Parse(SecondSpliceData[2]) - 1) * 24);
                for (int j = 0; j < NewArray.Length; j++)
                {
                    Temporary += NewArray[j];
                    if ((j + 1) % 24 == 0)
                    {
                        inputOutputData.TrigerThreeHexadecimal.Add(Temporary);
                        Temporary = "";
                    }

                }
            }
            #endregion
            return inputOutputData;
        }
        /// <summary>
        /// 字符串转数据库字段数据集合
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        private List<DatabaseFieldData> GetDatabaseFieldData(string Data)
        {
            List<DatabaseFieldData> databaseFieldDatas = new List<DatabaseFieldData>();
            string[] Array = Data.Split(new[] { "FFFEFF" }, StringSplitOptions.None);
            for (int i = 0; i < Array.Length / 3; i++)
            {
                DatabaseFieldData databaseFieldData = new DatabaseFieldData();
                string FirstString;
                if (i == 0)
                {
                    FirstString = Array[i].Substring(6 + int.Parse(Array[i].Substring(0, 2)) * 4);
                }
                else
                {
                    FirstString = Array[i + i * 2].Substring(int.Parse(HexadecimalToDecimal(Array[i + i * 2].Substring(0, 2) + "00")) * 4 + 2);
                }
                databaseFieldData.SortNum = HexadecimalToDecimal(FirstString.Substring(0, 4));
                databaseFieldData.FieldSign = FirstString.Substring(8, 4);
                FieldData fieldData = FieldDatas.FirstOrDefault(T => T.FieldHexadecimal == databaseFieldData.FieldSign);
                if (fieldData != null)
                {
                    databaseFieldData.FieldName = fieldData.FieldName;
                    switch (HexadecimalToDecimal(FirstString.Substring(12, 4)))
                    {
                        case "1":
                            databaseFieldData.Type = "Numerical";
                            break;
                        case "2":
                            databaseFieldData.Type = "String";
                            break;
                        case "3":
                            databaseFieldData.Type = "DateTime";
                            break;
                        case "4":
                            databaseFieldData.Type = "XML Parent Node";
                            break;
                        case "5":
                            databaseFieldData.Type = "XML Parent Node End";
                            break;
                        case "6":
                            databaseFieldData.Type = "XML Nodes List";
                            break;
                        case "7":
                            databaseFieldData.Type = "XML Nodes List End";
                            break;
                    }
                    databaseFieldData.FormatIndex = HexadecimalToDecimal(FirstString.Substring(16, 4));
                    databaseFieldData.Alignment = databaseFieldData.Type != "DateTime" ? HexadecimalToDecimal(FirstString.Substring(20, 4)) : "";
                    databaseFieldData.StringLenght = HexadecimalToDecimal(FirstString.Substring(24, 4));
                    databaseFieldData.LeadingChar = databaseFieldData.Type == "Numerical" ? HexadecimalToDecimal(FirstString.Substring(28, 4)) : "";
                    databaseFieldData.DivisionData = FirstString.Substring(36, 20);
                    databaseFieldData.MultiplicationData = FirstString.Substring(60, 20);
                    databaseFieldData.FixedValue = HexadecimalToString("0000" + Array[i + 1 + i * 2]
                        .Substring(2, int.Parse(Array[i + 1 + i * 2].Substring(0, 2)) * 4));
                    databaseFieldData.Unit = HexadecimalToString("0000" + Array[i + 2 + i * 2].Substring(2));
                    databaseFieldData.ColumnName = HexadecimalToString("0000" + Array[i + 3 + i * 2].Substring(2, int.Parse(HexadecimalToDecimal(Array[i + 3 + i * 2].Substring(0, 2) + "00")) * 4));
                    databaseFieldData.DatabaseField = fieldData.DatabaseField;
                    databaseFieldData.DatabaseSign = fieldData.FirstOrSecond;
                    databaseFieldDatas.Add(databaseFieldData);
                }
            }
            return databaseFieldDatas;
        }

        #region 收集拼接数据
        /// <summary>
        /// 获取第一个拼接数据
        /// </summary>
        /// <param name="Array"></param>
        /// <returns></returns>
        private string[] GetFirstSpliceData(string Array)
        {
            int Length = int.Parse(HexadecimalToDecimal(Array.Substring(0, 4).Trim() + "00"));
            Array = Array.Replace(" ", "").Substring(Length * 4).Substring(2);
            string[] SpliceData = new string[11];
            SpliceData[0] = Array.Substring(0, 4) == "0100" ? "true" : "false";
            SpliceData[1] = Array.Substring(16, 4) == "0100" ? "true" : "false";
            SpliceData[2] = Array.Substring(24, 4) == "0100" ? "true" : "false";
            SpliceData[3] = HexadecimalToDecimal(Array.Substring(32, 4));
            int MahcineCountLength = int.Parse(SpliceData[3]) * 8;
            SpliceData[4] = Array.Substring(40, MahcineCountLength);
            SpliceData[5] = Array.Substring(40 + MahcineCountLength, 12);
            SpliceData[6] = HexadecimalToDecimal(Array.Substring(56 + MahcineCountLength, 4));
            SpliceData[7] = Array.Substring(64 + MahcineCountLength, 12);
            SpliceData[8] = Array.Substring(88 + MahcineCountLength, 8);
            SpliceData[9] = HexadecimalToDecimal(Array.Substring(96 + MahcineCountLength, 4));
            SpliceData[10] = HexadecimalToDecimal(Array.Substring(104 + MahcineCountLength, 4));
            
            return SpliceData;
        }
        /// <summary>
        /// 获取第二个拼接数据
        /// </summary>
        /// <param name="Array"></param>
        /// <returns></returns>
        private string[] GetSecondSpliceData(string Array)
        {
            Array = Array.Replace(" ", "");
            string[] SpliceData = new string[12];
            SpliceData[0] = GetDatabasePassword(Array.Substring(0, int.Parse(Array.Substring(0, 2)) * 4 + 2));
            Array = Array.Substring(int.Parse(Array.Substring(0, 2)) * 4 + 2);
            SpliceData[1] = HexadecimalToDecimal(Array.Substring(0, 4));
            SpliceData[2] = HexadecimalToDecimal(Array.Substring(8, 4));
            SpliceData[3] = Array.Substring(16, 4) == "0100" ? "true" : "false";
            SpliceData[4] = HexadecimalToDecimal(Array.Substring(24, 4));
            int EventTrigerLength = int.Parse(SpliceData[2]) * 24;
            if (SpliceData[2] != "0")
            {
                SpliceData[3] = Array.Substring(16 + EventTrigerLength, 4) == "0100" ? "true" : "false";
                SpliceData[10] = Array.Substring(16, 2);
            }
            SpliceData[5] = Get8BitData(Array.Substring(32 + EventTrigerLength, 16));
            SpliceData[6] = Get8BitData(Array.Substring(56 + EventTrigerLength, 16));
            SpliceData[7] = Get8BitData(Array.Substring(80 + EventTrigerLength, 16));
            SpliceData[8] = Array.Substring(96 + EventTrigerLength, 4) == "0100" ? "true" : "false";
            SpliceData[9] = HexadecimalToDecimal(Array.Substring(104 + EventTrigerLength, 4));
            if (Array.Substring(112).Length > 8)
            {
                SpliceData[11] = HexadecimalToDecimal(Array.Substring(112, 4));
            }
            return SpliceData;
        }
        /// <summary>
        /// 获取第一段拼接数据（含有字段数据）
        /// </summary>
        /// <param name="Array"></param>
        /// <returns></returns>
        private string[] GetFirstSpliceDataHasField(string[] Array)
        {
            int Length = int.Parse(HexadecimalToDecimal(Array[6].Substring(0, 4).Trim() + "00"));
            Array[6] = Array[6].Replace(" ", "").Substring(Length * 4).Substring(2);
            string[] SpliceData = new string[28];
            SpliceData[0] = Array[6].Substring(0, 4) == "0100" ? "true" : "false";
            SpliceData[1] = Array[6].Substring(16, 4) == "0100" ? "true" : "false";
            SpliceData[2] = Array[6].Substring(24, 4) == "0100" ? "true" : "false";
            SpliceData[3] = HexadecimalToDecimal(Array[6].Substring(32, 4));
            int MahcineCountLength = int.Parse(SpliceData[3]) * 8;
            SpliceData[4]= Array[6].Substring(40, MahcineCountLength);
            SpliceData[5] = Array[6].Substring(40 + MahcineCountLength, 12);
            SpliceData[6] = HexadecimalToDecimal(Array[6].Substring(56 + MahcineCountLength, 4));
            SpliceData[7] = Array[6].Substring(64 + MahcineCountLength, 12);

            SpliceData[11] = "";
            SpliceData[12] = "";
            SpliceData[13] = "";
            SpliceData[14] = "";
            SpliceData[15] = "";
            SpliceData[16] = "";
            SpliceData[17] = "";
            SpliceData[18] = "";

            SpliceData[20] = "";
            SpliceData[21] = "";
            SpliceData[22] = "";
            SpliceData[23] = "";

            SpliceData[24] = "";
            SpliceData[25] = "";
            SpliceData[26] = "";
            SpliceData[27] = "";

            int FieldNum = 4;
            if (Array[6].Substring(Array[6].Length - 16, 4) == "0200")
            {
                if (Array[6].Substring(Array[6].Length - 8, 4) == "0200")
                {
                    Array[10] = Array[10].Replace(" ", "");
                    SpliceData[20] = Array[7].Replace(" ", "").Substring(2) != "" ? HexadecimalToString("0000" + Array[7].Replace(" ", "").Substring(2)) : "";
                    SpliceData[21] = HexadecimalToString("0000" + Array[8].Replace(" ", "").Substring(2));
                    SpliceData[22] = Array[9].Replace(" ", "").Substring(2) != "" ? HexadecimalToString("0000" + Array[9].Replace(" ", "").Substring(2)) : "";
                    SpliceData[23] = HexadecimalToString("0000" + Array[10].Substring(2, int.Parse(HexadecimalToDecimal(Array[10].Substring(0, 2) + "00")) * 4));

                    SpliceData[11] = HexadecimalToDecimal(Array[10].Substring(int.Parse(Array[10].Substring(0, 2)) * 4 + 2, 4));
                    if (int.Parse(SpliceData[11]) > 0)
                    {
                        FieldNum += int.Parse(SpliceData[11]) * 3 + 1;
                        for (int i = 1; i < FieldNum - 3; i++)
                        {
                            SpliceData[12] += Array[9 + i].Replace(" ", "") + "FFFEFF";
                        }
                        SpliceData[12] = SpliceData[12].Substring(0, SpliceData[12].Length - 6);
                        SpliceData[13] = HexadecimalToDecimal(Array[6 + FieldNum].Replace(" ", "").Substring(Array[6 + FieldNum].Replace(" ", "").Length - 16, 4));
                        SpliceData[14] = HexadecimalToString("0000" + Array[6 + FieldNum].Replace(" ", "").Substring(2, int.Parse(Array[6 + FieldNum].Replace(" ", "").Substring(0, 2)) * 4));
                    }
                    else
                    {
                        SpliceData[13] = HexadecimalToDecimal(Array[11].Replace(" ", "").Substring(Array[11].Replace(" ", "").Length - 16, 4));
                        SpliceData[14] = HexadecimalToString("0000" + Array[11].Replace(" ", "").Substring(2, int.Parse(Array[11].Replace(" ", "").Substring(0, 2)) * 4));
                        FieldNum++;
                    }


                    Array[10 + FieldNum] = Array[10 + FieldNum].Replace(" ", "");
                    SpliceData[24] = Array[7 + FieldNum].Replace(" ", "").Substring(2) != "" ? HexadecimalToString("0000" + Array[7 + FieldNum].Replace(" ", "").Substring(2)) : "";
                    SpliceData[25] = HexadecimalToString("0000" + Array[8 + FieldNum].Replace(" ", "").Substring(2));
                    SpliceData[26] = Array[9 + FieldNum].Replace(" ", "").Substring(2) != "" ? HexadecimalToString("0000" + Array[9 + FieldNum].Replace(" ", "").Substring(2)) : "";
                    SpliceData[27] = HexadecimalToString("0000" + Array[10 + FieldNum].Replace(" ", "").Substring(2, int.Parse(HexadecimalToDecimal(Array[10 + FieldNum].Substring(0, 2) + "00")) * 4));

                    SpliceData[15] = HexadecimalToDecimal(Array[10 + FieldNum].Substring(int.Parse(Array[10 + FieldNum].Substring(0, 2)) * 4 + 2, 4));
                    if (Array[10 + FieldNum].Substring(16, 5) != "00 00")
                    {
                        SpliceData[16] = "";
                        if (int.Parse(SpliceData[15]) > 0)
                        {
                            for (int i = 1; i < int.Parse(SpliceData[15]) * 3 + 5 - 3; i++)
                            {
                                SpliceData[16] += Array[9 + FieldNum + i].Replace(" ", "") + "FFFEFF";
                            }
                            FieldNum += int.Parse(SpliceData[15]) * 3 + 1;
                            SpliceData[16] = SpliceData[16].Substring(0, SpliceData[16].Length - 6);
                        }
                        else
                        {
                            FieldNum++;
                        }
                    }
                    else
                    {
                        FieldNum++;
                    }
                    SpliceData[17] = HexadecimalToDecimal(Array[10 + FieldNum].Replace(" ", "").Substring(Array[10 + FieldNum].Replace(" ", "").Length - 32, 4));
                    SpliceData[18] = HexadecimalToString("0000" + Array[10 + FieldNum].Replace(" ", "").Substring(2, int.Parse(Array[10 + FieldNum].Replace(" ", "").Substring(0, 2)) * 4));
                }
                else
                {
                    Array[10] = Array[10].Replace(" ", "");
                    SpliceData[24] = Array[7].Replace(" ", "").Substring(2) != "" ? HexadecimalToString("0000" + Array[7].Replace(" ", "").Substring(2)) : "";
                    SpliceData[25] = HexadecimalToString("0000" + Array[8].Replace(" ", "").Substring(2));
                    SpliceData[26] = Array[9].Replace(" ", "").Substring(2) != "" ? HexadecimalToString("0000" + Array[9].Replace(" ", "").Substring(2)) : "";
                    SpliceData[27] = HexadecimalToString("0000" + Array[10].Replace(" ", "").Substring(2, int.Parse(HexadecimalToDecimal(Array[10].Substring(0, 2) + "00")) * 4));

                    SpliceData[15] = HexadecimalToDecimal(Array[10].Substring(int.Parse(Array[10].Substring(0, 2)) * 4 + 2, 4));
                    if (int.Parse(SpliceData[15]) > 0)
                    {
                        FieldNum += int.Parse(SpliceData[15]) * 3 + 1;
                        for (int i = 1; i < FieldNum - 3; i++)
                        {
                            SpliceData[16] += Array[9 + i].Replace(" ", "") + "FFFEFF";
                        }
                        SpliceData[16] = SpliceData[16].Substring(0, SpliceData[16].Length - 6);
                        SpliceData[17] = HexadecimalToDecimal(Array[6 + FieldNum].Replace(" ", "").Substring(Array[6 + FieldNum].Replace(" ", "").Length - 16, 4));
                        SpliceData[18] = HexadecimalToString("0000" + Array[6 + FieldNum].Replace(" ", "").Substring(2, int.Parse(Array[6 + FieldNum].Replace(" ", "").Substring(0, 2)) * 4));
                    }
                    else
                    {
                        SpliceData[17] = HexadecimalToDecimal(Array[11].Replace(" ", "").Substring(Array[11].Replace(" ", "").Length - 16, 4));
                        SpliceData[18] = HexadecimalToString("0000" + Array[11].Replace(" ", "").Substring(2, int.Parse(Array[11].Replace(" ", "").Substring(0, 2)) * 4));
                        FieldNum++;
                    }

                    Array[10 + FieldNum] = Array[10 + FieldNum].Replace(" ", "");
                    SpliceData[20] = Array[7 + FieldNum].Replace(" ", "").Substring(2) != "" ? HexadecimalToString("0000" + Array[7 + FieldNum].Replace(" ", "").Substring(2)) : "";
                    SpliceData[21] = HexadecimalToString("0000" + Array[8 + FieldNum].Replace(" ", "").Substring(2));
                    SpliceData[22] = Array[9 + FieldNum].Replace(" ", "").Substring(2) != "" ? HexadecimalToString("0000" + Array[9 + FieldNum].Replace(" ", "").Substring(2)) : "";
                    SpliceData[23] = HexadecimalToString("0000" + Array[10 + FieldNum].Replace(" ", "").Substring(2, int.Parse(HexadecimalToDecimal(Array[10 + FieldNum].Substring(0, 2) + "00")) * 4));

                    SpliceData[11] = HexadecimalToDecimal(Array[10 + FieldNum].Substring(int.Parse(Array[10 + FieldNum].Substring(0, 2)) * 4 + 2, 4));
                    if (Array[10 + FieldNum].Substring(16, 5) != "00 00")
                    {
                        SpliceData[12] = "";
                        if (int.Parse(SpliceData[11]) > 0)
                        {
                            for (int i = 1; i < int.Parse(SpliceData[11]) * 3 + 5 - 3; i++)
                            {
                                SpliceData[12] += Array[9 + FieldNum + i].Replace(" ", "") + "FFFEFF";
                            }
                            FieldNum += int.Parse(SpliceData[11]) * 3 + 1;
                            SpliceData[12] = SpliceData[12].Substring(0, SpliceData[12].Length - 6);
                        }
                        else
                        {
                            FieldNum++;
                        }
                    }
                    else
                    {
                        FieldNum++;
                    }
                    SpliceData[13] = HexadecimalToDecimal(Array[10 + FieldNum].Replace(" ", "").Substring(Array[10 + FieldNum].Replace(" ", "").Length - 32, 4));
                    SpliceData[14] = HexadecimalToString("0000" + Array[10 + FieldNum].Replace(" ", "").Substring(2, int.Parse(Array[10 + FieldNum].Replace(" ", "").Substring(0, 2)) * 4));
                }

                SpliceData[8] = Array[10 + FieldNum].Replace(" ", "").Substring(Array[10 + FieldNum].Replace(" ", "").Length - 24, 8);
                SpliceData[9] = HexadecimalToDecimal(Array[10 + FieldNum].Replace(" ", "").Substring(Array[10 + FieldNum].Replace(" ", "").Length - 16, 4));
                SpliceData[10] = HexadecimalToDecimal(Array[10 + FieldNum].Replace(" ", "").Substring(Array[10 + FieldNum].Replace(" ", "").Length - 8, 4));
            }
            else
            {
                Array[10] = Array[10].Replace(" ", "");
                if (Array[6].Substring(Array[6].Length - 8, 4) == "0200")
                {
                    SpliceData[11] = HexadecimalToDecimal(Array[6 + FieldNum].Substring(int.Parse(Array[6 + FieldNum].Substring(0, 2)) * 4 + 2, 4));
                    SpliceData[12] = "";
                    if (int.Parse(SpliceData[11]) > 0)
                    {
                        FieldNum += int.Parse(SpliceData[11]) * 3 + 1;
                        for (int i = 1; i < FieldNum - 3; i++)
                        {
                            SpliceData[12] += Array[9 + i].Replace(" ", "") + "FFFEFF";
                        }

                        SpliceData[12] = SpliceData[12].Substring(0, SpliceData[12].Length - 6);
                    }
                    else
                    {
                        FieldNum++;
                    }
                    SpliceData[13] = HexadecimalToDecimal(Array[6 + FieldNum].Replace(" ", "").Substring(Array[6 + FieldNum].Replace(" ", "").Length - 32, 4));
                    SpliceData[14] = HexadecimalToString("0000" + Array[6 + FieldNum].Replace(" ", "").Substring(2, int.Parse(Array[6 + FieldNum].Replace(" ", "").Substring(0, 2)) * 4));

                    SpliceData[8] = Array[6 + FieldNum].Replace(" ", "").Substring(Array[6 + FieldNum].Replace(" ", "").Length - 24, 8);
                    SpliceData[9] = HexadecimalToDecimal(Array[6 + FieldNum].Replace(" ", "").Substring(Array[6 + FieldNum].Replace(" ", "").Length - 16, 4));
                    SpliceData[10] = HexadecimalToDecimal(Array[6 + FieldNum].Replace(" ", "").Substring(Array[6 + FieldNum].Replace(" ", "").Length - 8, 4));

                    SpliceData[20] = Array[7].Replace(" ", "").Substring(2) != "" ? HexadecimalToString("0000" + Array[7].Replace(" ", "").Substring(2)) : "";
                    SpliceData[21] = HexadecimalToString("0000" + Array[8].Replace(" ", "").Substring(2));
                    SpliceData[22] = Array[9].Replace(" ", "").Substring(2) != "" ? HexadecimalToString("0000" + Array[9].Replace(" ", "").Substring(2)) : "";
                    SpliceData[23] = HexadecimalToString("0000" + Array[10].Substring(2, int.Parse(HexadecimalToDecimal(Array[10].Substring(0, 2) + "00")) * 4));

                }
                else
                {
                    SpliceData[15] = HexadecimalToDecimal(Array[6 + FieldNum].Substring(int.Parse(Array[6 + FieldNum].Substring(0, 2)) * 4 + 2, 4));
                    SpliceData[16] = "";
                    if (int.Parse(SpliceData[15]) > 0)
                    {
                        FieldNum += int.Parse(SpliceData[15]) * 3 + 1;
                        for (int i = 1; i < FieldNum - 3; i++)
                        {
                            SpliceData[16] += Array[9 + i].Replace(" ", "") + "FFFEFF";
                        }

                        SpliceData[16] = SpliceData[16].Substring(0, SpliceData[16].Length - 6);
                    }
                    else
                    {
                        FieldNum++;
                    }
                    SpliceData[17] = HexadecimalToDecimal(Array[6 + FieldNum].Replace(" ", "").Substring(Array[6 + FieldNum].Replace(" ", "").Length - 32, 4));
                    SpliceData[18] = HexadecimalToString("0000" + Array[6 + FieldNum].Replace(" ", "").Substring(2, int.Parse(HexadecimalToDecimal(Array[6 + FieldNum].Replace(" ", "").Substring(0, 2) + "00")) * 4));

                    SpliceData[8] = Array[6 + FieldNum].Replace(" ", "").Substring(Array[6 + FieldNum].Replace(" ", "").Length - 24, 8);
                    SpliceData[9] = HexadecimalToDecimal(Array[6 + FieldNum].Replace(" ", "").Substring(Array[6 + FieldNum].Replace(" ", "").Length - 16, 4));
                    SpliceData[10] = HexadecimalToDecimal(Array[6 + FieldNum].Replace(" ", "").Substring(Array[6 + FieldNum].Replace(" ", "").Length - 8, 4));

                    SpliceData[24] = Array[7].Replace(" ", "").Substring(2) != "" ? HexadecimalToString("0000" + Array[7].Replace(" ", "").Substring(2)) : "";
                    SpliceData[25] = HexadecimalToString("0000" + Array[8].Replace(" ", "").Substring(2));
                    SpliceData[26] = Array[9].Replace(" ", "").Substring(2) != "" ? HexadecimalToString("0000" + Array[9].Replace(" ", "").Substring(2)) : "";
                    SpliceData[27] = HexadecimalToString("0000" + Array[10].Substring(2, int.Parse(HexadecimalToDecimal(Array[10].Substring(0, 2) + "00")) * 4));
                }
            }

            SpliceData[19] = FieldNum.ToString();
            return SpliceData;
        }
        #endregion

        #region 方法组
        /// <summary>
        /// 设置数据排序号
        /// </summary>
        /// <param name="InputOutputDatas"></param>
        private void SetDataIndex(List<InputOutputData> InputOutputDatas)
        {
            bool Implement = true;
            List<string> ExpTemporary = new List<string>();
            List<string> ImpTemporary = new List<string>();
            for (int i = 0; i < InputOutputDatas.Count; i++)
            {
                if (InputOutputDatas[i].ImportExport)
                {
                    ExpTemporary.Add(InputOutputDatas[i].FileID);
                }
                else
                {
                    ImpTemporary.Add(InputOutputDatas[i].FileID);
                }
            }
            string Temporary = "";
            int Index = 1;
            int TemporaryInt = 0;
            while (Implement)
            {
                if (ExpTemporary.Count == 0)
                {
                    break;
                }
                for (int i = 0; i < ExpTemporary.Count; i++)
                {
                    if (Temporary != string.Empty)
                    {
                        if (ComparativeData(Temporary, ExpTemporary[i].Trim()))
                        {
                            Temporary = ExpTemporary[i].Trim();
                            TemporaryInt = i;
                        }
                    }
                    else
                    {
                        Temporary = ExpTemporary[i].Trim();
                        TemporaryInt = i;
                    }
                    if (i == ExpTemporary.Count - 1)
                    {
                        InputOutputData inputOutputData = InputOutputDatas.FirstOrDefault(t => t.FileID.Trim() == Temporary);
                        inputOutputData.Index = Index.ToString();
                        Index++;
                        Temporary = "";
                        ExpTemporary.RemoveAt(TemporaryInt);
                        if (ExpTemporary.Count == 0)
                        {
                            Implement = false;
                        }
                    }
                }
            }
            Implement = true;
            Index = 1;
            TemporaryInt = 0;
            while (Implement)
            {
                if (ImpTemporary.Count == 0)
                {
                    break;
                }
                for (int i = 0; i < ImpTemporary.Count; i++)
                {
                    if (Temporary != string.Empty)
                    {
                        if (ComparativeData(Temporary, ImpTemporary[i].Trim()))
                        {
                            Temporary = ImpTemporary[i].Trim();
                            TemporaryInt = i;
                        }
                    }
                    else
                    {
                        Temporary = ImpTemporary[i].Trim();
                        TemporaryInt = i;
                    }
                    if (i == ImpTemporary.Count - 1)
                    {
                        InputOutputData inputOutputData = InputOutputDatas.FirstOrDefault(t => t.FileID.Trim() == Temporary);
                        inputOutputData.Index = Index.ToString();
                        Index++;
                        Temporary = "";
                        ImpTemporary.RemoveAt(TemporaryInt);
                        if (ImpTemporary.Count == 0)
                        {
                            Implement = false;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 获取日志文件名字
        /// </summary>
        /// <param name="Array"></param>
        /// <returns></returns>
        private string GetLogFilename(string Array)
        {
            string Temporary = "";
            int Length = int.Parse(HexadecimalToDecimal(Array.Substring(0, 4).Trim() + "00"));
            Array = Array.Replace(" ", "").Substring(2);
            for (int i = 0; i < Array.Length; i++)
            {
                if (i / 4 == Length)
                {
                    break;
                }
                Temporary += Array[i];
                if (i != 0 && i % 2 != 0)
                {
                    Temporary += " ";
                }
            }
            return HexadecimalToString("0000" + Temporary);
        }
        /// <summary>
        /// 对比数据大小
        /// </summary>
        /// <param name="ContrastValue1"></param>
        /// <param name="ContrastValue2"></param>
        /// <returns></returns>
        private bool ComparativeData(string ContrastValue1, string ContrastValue2)
        {
            string[] Array1 = ContrastValue1.Split(' ');
            string[] Array2 = ContrastValue2.Split(' ');
            for (int i = 3; i >= 0; i--)
            {
                if (int.Parse(HexadecimalToDecimal(Array2[i] + "00")) < int.Parse(HexadecimalToDecimal(Array1[i] + "00")))
                {
                    return true;
                }
                if (int.Parse(HexadecimalToDecimal(Array2[i] + "00")) == int.Parse(HexadecimalToDecimal(Array1[i] + "00")))
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取数据库登录密码
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        private string GetDatabasePassword(string Data)
        {
            string DatabasePW = "";
            int Temporary = 0;
            string[] Array = new string[int.Parse(HexadecimalToDecimal(Data.Substring(0, 4)))];
            Data = Data.Substring(2);
            for (int i = 0; i < Data.Length / 4; i++)
            {
                Array[i] = Data[i * 4].ToString() + Data[i * 4 + 1] + Data[i * 4 + 2] + Data[i * 4 + 3];
            }
            for (int i = Array.Length - 1; i >= 0; i--)
            {
                if (i == Array.Length - 1)
                {
                    DatabasePW = HexadecimalToString("0000" + Array[i]);
                    Temporary = int.Parse(HexadecimalToDecimal(Array[i]));
                }
                else
                {
                    Temporary = int.Parse(HexadecimalToDecimal(Array[i])) - Temporary;
                    DatabasePW = (char)(Temporary) + DatabasePW.Trim('\0');
                }
            }
            return DatabasePW;
        }
        /// <summary>
        /// 设置字段数据Name
        /// </summary>
        private void SetFiledDataName(SqlConnection Con)
        {
            DatabaseOperation databaseOperation = new DatabaseOperation();
            List<t_CfgBatchTexts> t_CfgBatchTexts = databaseOperation.GetCfgBatchTexts(Con);
            List<t_CfgBatchParams> t_CfgBatchParams = databaseOperation.GetCfgBatchParams(Con);
            List<t_CfgConsumptions> t_CfgConsumptions = databaseOperation.GetCfgConsumptions(Con);
            List<t_CfgFctStops> t_CfgFctStops = databaseOperation.GetCfgFctStops(Con);
            List<t_CfgTreatmentParams> t_CfgTreatmentParams = databaseOperation.GetT_CfgsTP(Con);
            List<t_CfgStopDecls> t_CfgStopDecls = databaseOperation.GetCfgStopDecls(Con);
            List<t_CfgAlarms> t_CfgAlarms = databaseOperation.GetCfgAlarms(Con);
            List<t_CfgFunctions> t_CfgFunctions = databaseOperation.GetCfgFunctions(Con);

            for (int i = 0; i < t_CfgBatchTexts.Count; i++)
            {
                string BTIndex = t_CfgBatchTexts[i].c_Id.ToString().Length > 1 ? t_CfgBatchTexts[i].c_Id.ToString() : "0" + t_CfgBatchTexts[i].c_Id.ToString();

                //BatchText
                FieldData fieldData = FieldDatas.FirstOrDefault(t => t.FieldName == "BT" + BTIndex + "-BatchText" + BTIndex);
                if (fieldData != null)
                {
                    fieldData.FieldName = "BT" + BTIndex + "-" + t_CfgBatchTexts[i].c_Name;
                }

                //NextBatchText
                fieldData = FieldDatas.FirstOrDefault(t => t.FieldName == "NBT" + BTIndex + "-NextBatchText" + BTIndex);
                if (fieldData != null)
                {
                    fieldData.FieldName = "NBT" + BTIndex + "-" + t_CfgBatchTexts[i].c_Name;
                }

                //OrderBatchText
                fieldData = FieldDatas.FirstOrDefault(t => t.FieldName == "OT" + BTIndex + "-OrderText" + BTIndex);
                if (fieldData != null)
                {
                    fieldData.FieldName = "OT" + BTIndex + "-" + t_CfgBatchTexts[i].c_Name;
                }
            }
            for (int i = 0; i < t_CfgBatchParams.Count; i++)
            {
                string BPIndex = t_CfgBatchParams[i].c_Id.ToString().Length > 1 ? t_CfgBatchParams[i].c_Id.ToString() : "0" + t_CfgBatchParams[i].c_Id.ToString();

                //BatchParam
                FieldData fieldData = FieldDatas.FirstOrDefault(t => t.FieldName == "BP" + BPIndex + "-BatchParam" + BPIndex);
                if (fieldData != null)
                {
                    fieldData.FieldName = "BP" + BPIndex + "-" + t_CfgBatchParams[i].c_Name;
                }

                //NextBatchParam
                fieldData = FieldDatas.FirstOrDefault(t => t.FieldName == "NBP" + BPIndex + "-NextBatchParam" + BPIndex);
                if (fieldData != null)
                {
                    fieldData.FieldName = "NBP" + BPIndex + "-" + t_CfgBatchParams[i].c_Name;
                }

                //OrderBatchParam
                fieldData = FieldDatas.FirstOrDefault(t => t.FieldName == "OP" + BPIndex + "-OrderParam" + BPIndex);
                if (fieldData != null)
                {
                    fieldData.FieldName = "OP" + BPIndex + "-" + t_CfgBatchParams[i].c_Name;
                }
            }
            for (int i = 0; i < t_CfgConsumptions.Count; i++)
            {
                string COIndex = t_CfgConsumptions[i].c_Id.ToString().Length > 1 ? t_CfgConsumptions[i].c_Id.ToString() : "0" + t_CfgConsumptions[i].c_Id.ToString();
                FieldData fieldData = FieldDatas.FirstOrDefault(t => t.FieldName == "CO" + COIndex + "-Consumption" + COIndex);
                if (fieldData != null)
                {
                    fieldData.FieldName = "CO" + COIndex + "-" + t_CfgConsumptions[i].c_Name;
                }
            }
            for (int i = 0; i < t_CfgFctStops.Count; i++)
            {
                string FSIndex = t_CfgFctStops[i].c_Id.ToString().Length > 1 ? t_CfgFctStops[i].c_Id.ToString() : "0" + t_CfgFctStops[i].c_Id.ToString();
                FieldData fieldData = FieldDatas.FirstOrDefault(t => t.FieldName == "FS" + FSIndex + "-FunctionStop" + FSIndex);
                if (fieldData != null)
                {
                    fieldData.FieldName = "FS" + FSIndex + "-" + t_CfgFctStops[i].c_Name;
                }
            }
            for (int i = 0; i < t_CfgTreatmentParams.Count; i++)
            {
                string TPIndex = t_CfgTreatmentParams[i].c_Id.ToString().Length > 1 ? t_CfgTreatmentParams[i].c_Id.ToString() : "0" + t_CfgTreatmentParams[i].c_Id.ToString();
                if (FieldDatas != null)
                {
                    FieldData fieldData = new FieldData();
                    if (i <= 255)
                    {
                        fieldData.FieldHexadecimal = (i + 1).ToString("x2") + "F0";
                    }
                    else if (i > 255 && i <= 510)
                    {
                        fieldData.FieldHexadecimal = (i - 244).ToString("x2") + "F1";
                    }
                    else if (i > 510 && i <= 765)
                    {
                        fieldData.FieldHexadecimal = (i - 509).ToString("x2") + "F2";
                    }
                    else
                    {
                        fieldData.FieldHexadecimal = (i - 765).ToString("x2") + "F3";
                    }
                    fieldData.FieldName = "TP" + TPIndex + t_CfgTreatmentParams[i].c_Name;
                    fieldData.FieldType = "Numerical";
                    fieldData.FirstOrSecond = "1";
                    FieldDatas.Add(fieldData);
                }
            }
            for (int i = 0; i < t_CfgStopDecls.Count; i++)
            {
                string DsIndex = t_CfgStopDecls[i].c_Id.ToString().Length > 1 ? t_CfgStopDecls[i].c_Id.ToString() : "0" + t_CfgStopDecls[i].c_Id.ToString();
                FieldData fieldData = FieldDatas.FirstOrDefault(t => t.FieldName == "DS" + DsIndex + "-DeclStop" + DsIndex);
                if (fieldData != null)
                {
                    fieldData.FieldName = "DS" + DsIndex + "-" + t_CfgStopDecls[i].c_Name;
                }
            }
            for (int i = 0; i < t_CfgAlarms.Count; i++)
            {
                string ALIndex = t_CfgAlarms[i].c_Id.ToString().Length > 1 ? t_CfgAlarms[i].c_Id.ToString() : "0" + t_CfgAlarms[i].c_Id.ToString();
                FieldData fieldData = FieldDatas.FirstOrDefault(t => t.FieldName == "AL" + ALIndex + "-AlarmStop" + ALIndex);
                if (fieldData != null)
                {
                    fieldData.FieldName = "AL" + ALIndex + "-" + t_CfgAlarms[i].c_Name;
                }
            }
            for (int i = 0; i < t_CfgFunctions.Count; i++)
            {
                string FGTIndex = t_CfgFunctions[i].c_Id.ToString().Length > 1 ? t_CfgFunctions[i].c_Id.ToString() : "0" + t_CfgFunctions[i].c_Id.ToString();

                FieldData fieldData = FieldDatas.FirstOrDefault(t => t.FieldName == "FGC" + FGTIndex + "-FunctionGroupCount" + FGTIndex);
                if (fieldData != null)
                {
                    fieldData.FieldName = "FGC" + FGTIndex + "-" + t_CfgFunctions[i].c_Name;
                }

                fieldData = FieldDatas.FirstOrDefault(t => t.FieldName == "FGT" + FGTIndex + "-FunctionGroupTime" + FGTIndex);
                if (fieldData != null)
                {
                    fieldData.FieldName = "FGT" + FGTIndex + "-" + t_CfgFunctions[i].c_Name;
                }
            }
        }
        /// <summary>
        /// 获取所有事件触发器数据
        /// </summary>
        private void GetEventTriggerDatas(SqlConnection Con)
        {
            DatabaseOperation databaseOperation = new DatabaseOperation();
            int index = 1;
            string[] Data = File.ReadAllText(EventTriggerFile).Split('\n');
            for (int i = 0; i < Data.Length; i++)
            {
                EventTriggerData eventTriggerData = new EventTriggerData();
                eventTriggerData.FirstName = Data[i].Split('-')[1];
                eventTriggerData.SecondName = Data[i].Split('-')[0];
                eventTriggerData.Index = Data[i].Split('-')[2];
                eventTriggerData.Num = Data[i].Split('-')[3].Trim('\r');
                eventTriggerData.Sign = Data[i].Split('-')[4].Trim('\r');
                eventTriggerData.OPCItem = Data[i].Split('-')[5].Trim('\r');
                EventTriggerDatas.Add(eventTriggerData);
            }
            List<t_CfgFctStops> t_CfgFctStops = databaseOperation.GetCfgFctStops(Con);
            for (int i = 0; i < t_CfgFctStops.Count; i++)
            {
                string FSIndex = t_CfgFctStops[i].c_Id.ToString().Length > 1 ? t_CfgFctStops[i].c_Id.ToString() : "0" + t_CfgFctStops[i].c_Id.ToString();
                if (i == 15)
                {
                    break;
                }
                EventTriggerData eventTriggerData = new EventTriggerData
                {
                    FirstName = "FS" + FSIndex + " " + t_CfgFctStops[i].c_Name + " ON",
                    SecondName = "FS" + FSIndex + " " + t_CfgFctStops[i].c_Name + " ON",
                    Index = i + 1 > 4 ? (((i + 1) / 4) + 1).ToString() : "1",
                    Num = index != 1 ? Math.Pow(2, index - 1).ToString() : "1",
                    OPCItem = "FS" + FSIndex + " ON",
                    Sign = "3"
                };
                EventTriggerDatas.Add(eventTriggerData);
                index++;
                EventTriggerData eventTriggerData2 = new EventTriggerData
                {
                    FirstName = "FS" + FSIndex + " " + t_CfgFctStops[i].c_Name + " Off",
                    SecondName = "FS" + FSIndex + " " + t_CfgFctStops[i].c_Name + " Off",
                    Index = i + 1 > 4 ? (((i + 1) / 4) + 1).ToString() : "1",
                    Num = index != 2 ? Math.Pow(2, index - 1).ToString() : "2",
                    OPCItem = "FS" + FSIndex + " Off",
                    Sign = "3"
                };
                index++;
                if ((i + 1) % 4 == 0 && i >= 3)
                {
                    index = 1;
                }
                EventTriggerDatas.Add(eventTriggerData2);
            }
            List<t_CfgFunctions> t_CfgFunctions = databaseOperation.GetCfgFunctions(Con);
            for (int i = 0; i < t_CfgFunctions.Count; i++)
            {
                string FGIndex = t_CfgFunctions[i].c_Id.ToString().Length > 1 ? t_CfgFunctions[i].c_Id.ToString() : "0" + t_CfgFunctions[i].c_Id.ToString();
                EventTriggerData eventTriggerData = new EventTriggerData
                {
                    FirstName = "FG" + FGIndex + " " + t_CfgFunctions[i].c_Name + " ON",
                    SecondName = "FG" + FGIndex + " " + t_CfgFunctions[i].c_Name + " ON",
                    Index = "-1",
                    Num = (i + 1).ToString(),
                    OPCItem = "FG" + FGIndex + " ON",
                    Sign = "2"
                };
                EventTriggerDatas.Add(eventTriggerData);
                EventTriggerData eventTriggerData2 = new EventTriggerData
                {
                    FirstName = "FG" + FGIndex + " " + t_CfgFunctions[i].c_Name + " Off",
                    SecondName = "FG" + FGIndex + " " + t_CfgFunctions[i].c_Name + " Off",
                    Index = "-2",
                    Num = (i + 1).ToString(),
                    OPCItem = "FG" + FGIndex + " Off",
                    Sign = "2"
                };
                EventTriggerDatas.Add(eventTriggerData2);
            }
            EventTriggerDatas.Sort((a, b) => a.SecondName.CompareTo(b.SecondName));
        }
        /// <summary>
        /// 获取字段数据
        /// </summary>
        /// <param name="Content">字段配置文件内容</param>
        /// <returns></returns>
        private List<FieldData> GetFieldDatasByContent(string Content)
        {
            List<FieldData> fieldDatas = new List<FieldData>(); string[] Array = Content.Split('\n');
            for (int i = 0; i < Array.Length; i++)
            {
                FieldData fieldData = new FieldData();
                fieldData.FieldName = Array[i].Split('：')[0];
                fieldData.FieldHexadecimal = Array[i].Split('：')[1];
                fieldData.FieldType = Array[i].Split('：')[2];
                fieldData.FirstOrSecond = Array[i].Split('：')[3].TrimEnd('\r');
                fieldData.DatabaseField = Array[i].Split('：')[4].TrimEnd('\r');
                fieldDatas.Add(fieldData);
            }
            try
            {
                
            }
            catch (Exception)
            {
                throw new InvalidCastException("AlliancerLinkToDispenserHostDAL.GetFieldDatas 字段配置文件读取错误！");
            }
            return fieldDatas;
        }
        /// <summary>
        /// 将16进制字符串转换为文本
        /// </summary>
        /// <param name="Hexadecimal">16进制数据</param>
        /// <returns></returns>
        private string HexadecimalToString(string Hexadecimal)
        {
            Hexadecimal = Hexadecimal.Substring(4).Replace(" ", "");
            string[] Array = new string[Hexadecimal.Length / 4];
            for (int i = 0; i < Hexadecimal.Length / 4; i++)
            {
                Array[i] = Hexadecimal[i * 4].ToString() + Hexadecimal[i * 4 + 1];
            }
            byte[] ByteArray = new byte[Hexadecimal.Length / 4];
            for (int i = 0; i < Array.Length; i++)
            {
                if (Array[i].Trim() != string.Empty)
                {
                    ByteArray[i] = Convert.ToByte(Array[i].Trim(), 16);
                }
            }
            return Encoding.GetEncoding(0).GetString(ByteArray).TrimEnd('\0');
        }
        /// <summary>
        /// 16进制转10进制数据
        /// </summary>
        /// <param name="Hexadecimal"></param>
        /// <returns></returns>
        private string HexadecimalToDecimal(string Hexadecimal)
        {
            string DecimalString = "";
            string Temporary = "";
            for (int i = 0; i < Hexadecimal.Replace(" ", "").Length; i++)
            {
                Temporary += Hexadecimal.Replace(" ", "")[i];
                if ((i + 1) % 4 == 0)
                {
                    DecimalString += Int32.Parse(Temporary.Substring(0, 2), System.Globalization.NumberStyles.HexNumber).ToString();
                    Temporary = "";
                }
            }
            return DecimalString;
        }
        /// <summary>
        /// Bin文件转16进制数据
        /// </summary>
        /// <param name="FilePath">文件地址</param>
        /// <returns></returns>
        private string GetBinTOHexadecimal(string FilePath)
        {
            int count = 0;//换行显示计数
            string str = "";
            FileStream Myfile = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            using (BinaryReader binreader = new BinaryReader(Myfile))
            {
                byte[] binchar = binreader.ReadBytes((int)Myfile.Length);
                foreach (byte j in binchar)
                {
                    str += j.ToString("X2") + " ";
                    count++;
                }
            }
            return str;
        }
        /// <summary>
        /// 根据事件触发器1、2、4的16进制数据获取
        /// </summary>
        /// <param name="Data">16进制数据</param>
        /// <param name="EventTriggerDatas">事件数据</param>
        /// <param name="IsImportExport">导入、导出标记</param>
        /// <returns></returns>
        private List<EventTriggerData> GetEventTriggerDatas(string Data, List<EventTriggerData> EventTriggerDatas, bool IsImportExport = true)
        {
            string Sign = IsImportExport ? "1" : "2";
            if (Data.Length != 12)
            {
                Sign = "3";
            }
            List<EventTriggerData> eventTriggerDatas = new List<EventTriggerData>();
            for (int i = 0; i < Data.Length / 2; i++)
            {
                int Num = int.Parse(HexadecimalToDecimal(Data[i * 2].ToString() + Data[i * 2 + 1].ToString() + "00"));
                if (Num != 0)
                {
                    List<EventTriggerData> eventTriggers = EventTriggerDatas.FindAll(t => t.Index == (i + 1).ToString() && t.Sign.Trim('\r') == Sign);
                    eventTriggers.Sort((a, b) => int.Parse(a.Num).CompareTo(int.Parse(b.Num)));
                    for (int j = eventTriggers.Count - 1; j >= 0; j--)
                    {
                        if (Num - int.Parse(eventTriggers[j].Num) >= 0)
                        {
                            eventTriggerDatas.Add(eventTriggers[j]);
                            Num -= int.Parse(eventTriggers[j].Num);
                        }
                    }
                }
            }
            return eventTriggerDatas;
        }
        /// <summary>
        /// 获取事件触发数据3数据
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="EventTriggerDatas"></param>
        /// <returns></returns>
        private List<EventTriggerData> GetEventTrigger3Datas(List<string> Data, List<EventTriggerData> EventTriggerDatas)
        {
            List<EventTriggerData> eventTriggerDatas = new List<EventTriggerData>();
            for (int i = 0; i < Data.Count; i++)
            {
                string Num = HexadecimalToDecimal(Data[i].Substring(8, 2) + "00");
                string Index = Data[i].Substring(17, 1) == "1" ? "-1" : "-2";
                EventTriggerData eventTriggerData = EventTriggerDatas.FirstOrDefault(t => t.Num == Num && t.Index == Index);
                eventTriggerDatas.Add(eventTriggerData);
            }
            return eventTriggerDatas;
        }
        /// <summary>
        /// 获取星期数据
        /// </summary>
        /// <param name="WeekData"></param>
        /// <returns></returns>
        private List<int> GetWeekData(int WeekData)
        {
            int Num = 64;
            List<int> Data = new List<int>();
            for (int i = 7; i > 0; i--)
            {
                if (WeekData - Num >= 0)
                {
                    WeekData -= Num;
                    Data.Add(i);
                }
                Num /= 2;
            }
            return Data;
        }
        /// <summary>
        /// 获取八位数据
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        private string Get8BitData(string Data)
        {
            byte[] ByteArray = new byte[Data.Length / 2];
            for (int i = 0; i < 8; i++)
            {
                ByteArray[i] = (byte)(int.Parse(HexadecimalToDecimal(Data[i * 2].ToString() + Data[i * 2 + 1].ToString() + "00")));
            }
            return DateTime.FromOADate(Calculation(ByteArray)).ToString().Substring(10).Substring(0, 4);
        }
        /// <summary>
        /// 计算8位算法
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        private double Calculation(byte[] values)
        {
            try
            {
                double value1 = values[7];
                double value2 = values[6];
                double value3 = values[5];
                double value4 = values[4];
                double value5 = values[3];
                double value6 = values[2];
                double value7 = values[1];
                double value8 = values[0];
                int square;//平方
                int pow;//次幂
                int up;//上面
                int down;//下面
                double interval;//间隔
                double other;//其他
                double value;//真正的值
                if ((value2 % 16) != 0)
                {
                    square = (int)(value2 / 16d) + 1;
                }
                else
                {
                    square = (int)(value2 / 16d);
                }
                pow = square + 1;
                down = square * 16;
                up = (int)Math.Pow(2, pow);
                if (value2 == down)
                {
                    interval = up / 16d;
                }
                else
                {
                    interval = (up / 2d) / 16d;
                }
                value2 = (up - ((down - value2) * interval));//算出第一个的值
                other = (256d / interval);
                if (value3 != 0)
                {
                    value3 = (value3 / other);
                }
                if (value4 != 0)
                {
                    other = 256d * other;
                    value4 = (value4 / other);
                }
                if (value5 != 0)
                {
                    other = 256d * other;
                    value5 = (value5 / other);
                }
                if (value6 != 0)
                {
                    other = 256d * other;
                    value6 = (value6 / other);
                }
                if (value7 != 0)
                {
                    other = 256d * other;
                    value7 = (value7 / other);
                }
                if (value8 != 0)
                {
                    other = 256d * other;
                    value8 = (value8 / other);
                }
                value = value2 + value3 + value4 + value5 + value6 + value7 + value8;
                if (value1 == 63)
                {
                    return (10d / (65536d / value) * 0.1d);
                }
                else if (value1 == 64)
                {
                    return value;
                }
                return 0;
            }
            catch
            {
                return 0.0000;
            }
        }
        #endregion
    }
}
