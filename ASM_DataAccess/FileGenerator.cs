using System;
using System.Collections;
using System.Text;
using System.Data;
using System.IO;

namespace ASM.DataAccess
{
    public static class FileGenerator
    {
        #region CollectionBase Source
        public static Boolean Generate(CollectionBase busineesObject, Object item, Boolean writeHeader, String columnDemiliter, String textQualifier, QualifierType qType, String outputFile)
        {
            return Generate(busineesObject, item, writeHeader, null, columnDemiliter, textQualifier, qType, outputFile, false);
        }
        public static Boolean Generate(CollectionBase busineesObject, Object item, String[] Headers, String columnDemiliter, String textQualifier, QualifierType qType, String outputFile)
        {
            return Generate(busineesObject, item, true, Headers, columnDemiliter, textQualifier, qType, outputFile, false);
        }
        public static Boolean Generate(CollectionBase busineesObject, Object item, Boolean writeHeader, String columnDemiliter, String textQualifier, QualifierType qType, String outputFile, Boolean isAppend)
        {
            return Generate(busineesObject, item, writeHeader, null, columnDemiliter, textQualifier, qType, outputFile, isAppend);
        }
        public static Boolean Generate(CollectionBase busineesObject, Object item, String[] Headers, String columnDemiliter, String textQualifier, QualifierType qType, String outputFile, Boolean isAppend)
        {
            return Generate(busineesObject, item, true, Headers, columnDemiliter, textQualifier, qType, outputFile, isAppend);
        }
        #endregion

        #region DictionaryBase Source
        public static Boolean Generate(DictionaryBase busineesObject, Object item, Boolean writeHeader, String columnDemiliter, String textQualifier, QualifierType qType, String outputFile)
        {
            return Generate(busineesObject, item, writeHeader, null, columnDemiliter, textQualifier, qType, outputFile, false);
        }
        public static Boolean Generate(DictionaryBase busineesObject, Object item, String[] Headers, String columnDemiliter, String textQualifier, QualifierType qType, String outputFile)
        {
            return Generate(busineesObject, item, true, Headers, columnDemiliter, textQualifier, qType, outputFile, false);
        }
        public static Boolean Generate(DictionaryBase busineesObject, Object item, Boolean writeHeader, String columnDemiliter, String textQualifier, QualifierType qType, String outputFile, Boolean isAppend)
        {
            return Generate(busineesObject, item, writeHeader, null, columnDemiliter, textQualifier, qType, outputFile, isAppend);
        }
        public static Boolean Generate(DictionaryBase busineesObject, Object item, String[] Headers, String columnDemiliter, String textQualifier, QualifierType qType, String outputFile, Boolean isAppend)
        {
            return Generate(busineesObject, item, true, Headers, columnDemiliter, textQualifier, qType, outputFile, isAppend);
        }
        #endregion

        #region DataTable Source
        public static Boolean Generate(DataTable dTable, Boolean writeHeader, String columnDemiliter, String textQualifier, QualifierType qType, String outputFile)
        {
            return Generate(dTable.DefaultView, writeHeader, null, columnDemiliter, textQualifier, qType, outputFile, false);
        }
        public static Boolean Generate(DataTable dTable, String[] Headers, String columnDemiliter, String textQualifier, QualifierType qType, String outputFile)
        {
            return Generate(dTable.DefaultView, true, Headers, columnDemiliter, textQualifier, qType, outputFile, false);
        }
        public static Boolean Generate(DataTable dTable, Boolean writeHeader, String columnDemiliter, String textQualifier, QualifierType qType, String outputFile, Boolean isAppend)
        {
            return Generate(dTable.DefaultView, writeHeader, null, columnDemiliter, textQualifier, qType, outputFile, isAppend);
        }
        public static Boolean Generate(DataTable dTable, String[] Headers, String columnDemiliter, String textQualifier, QualifierType qType, String outputFile, Boolean isAppend)
        {
            return Generate(dTable.DefaultView, true, Headers, columnDemiliter, textQualifier, qType, outputFile, isAppend);
        }
        #endregion

        #region DataView Source
        public static Boolean Generate(DataView dView, Boolean writeHeader, String columnDemiliter, String textQualifier, QualifierType qType, String outputFile)
        {
            return Generate(dView, writeHeader, null, columnDemiliter, textQualifier, qType, outputFile, false);
        }
        public static Boolean Generate(DataView dView, String[] Headers, String columnDemiliter, String textQualifier, QualifierType qType, String outputFile)
        {
            return Generate(dView, true, Headers, columnDemiliter, textQualifier, qType, outputFile, false);
        }
        public static Boolean Generate(DataView dView, Boolean writeHeader, String columnDemiliter, String textQualifier, QualifierType qType, String outputFile, Boolean isAppend)
        {
            return Generate(dView, writeHeader, null, columnDemiliter, textQualifier, qType, outputFile, isAppend);
        }
        public static Boolean Generate(DataView dView, String[] Headers, String columnDemiliter, String textQualifier, QualifierType qType, String outputFile, Boolean isAppend)
        {
            return Generate(dView, true, Headers, columnDemiliter, textQualifier, qType, outputFile, isAppend);
        }
        #endregion

        #region General Function
        private static ArrayList GetList(CollectionBase busineesObject)
        {
            ArrayList items = new ArrayList();
            IEnumerator ie = busineesObject.GetEnumerator();
            while (ie.MoveNext())
                items.Add(ie.Current);
            return items;
        }
        private static ArrayList GetList(DictionaryBase busineesObject)
        {
            ArrayList items = new ArrayList();
            IEnumerator ie = busineesObject.GetEnumerator();
            while (ie.MoveNext())
                items.Add(ie.Current);
            return items;
        }
        private static DataTable GetDataSource(ArrayList data, Object item)
        {
            DataTable tbl = new DataTable();
            System.Reflection.FieldInfo[] fieldInfos;
            System.Reflection.PropertyInfo[] propInfos;
            Object[] rowValues;
            Int32 fieldCount = 0;
            Int32 propCount = 0;
            Int32 columnCount = 0;
            Int32 j;

            if (data.Count == 0)
            {
                fieldInfos = item.GetType().GetFields();
                propInfos = item.GetType().GetProperties();

                //Create table columns
                for (j = 0; j < fieldInfos.Length; j++)
                {
                    if (fieldInfos[j].IsDefined((new DbField()).GetType(), false))
                    {
                        tbl.Columns.Add(fieldInfos[j].Name, fieldInfos[j].FieldType);
                    }
                }
                for (j = 0; j < propInfos.Length; j++)
                {
                    if (propInfos[j].IsDefined((new DbField()).GetType(), false))
                    {
                        tbl.Columns.Add(propInfos[j].Name, propInfos[j].PropertyType);
                    }
                }
            }
            else
            {
                //Get Fields and Properties
                fieldInfos = data[0].GetType().GetFields();
                propInfos = data[0].GetType().GetProperties();

                //Create table columns
                for (j = 0; j < fieldInfos.Length; j++)
                {
                    if (fieldInfos[j].IsDefined((new DbField()).GetType(), false))
                    {
                        tbl.Columns.Add(fieldInfos[j].Name, fieldInfos[j].FieldType);
                        fieldCount += 1;
                        columnCount += 1;
                    }
                }

                for (j = 0; j < propInfos.Length; j++)
                {
                    if (fieldInfos[j].IsDefined((new DbField()).GetType(), false))
                    {
                        tbl.Columns.Add(propInfos[j].Name, propInfos[j].PropertyType);
                        propCount += 1;
                        columnCount += 1;
                    }
                }


                rowValues = new Object[columnCount];


                for (Int32 i = 0; i < data.Count; i++)
                {
                    //Add table rows
                    if (columnCount > 0)
                    {
                        Int16 idx = 0;
                        if (fieldCount > 0)
                        {
                            for (j = 0; j < fieldInfos.Length; j++)
                            {
                                if (fieldInfos[j].IsDefined((new DbField()).GetType(), false))
                                {
                                    rowValues[idx] = fieldInfos[j].GetValue(data[i]);
                                    idx += 1;
                                }
                            }
                        }

                        if (propCount > 0)
                        {
                            for (j = 0; j < propInfos.Length; j++)
                            {
                                if (propInfos[j].IsDefined((new DbField()).GetType(), false))
                                {
                                    rowValues[idx] = propInfos[j].GetValue(data[i], null);
                                    idx += 1;
                                }
                            }
                        }

                        tbl.Rows.Add(rowValues);
                    }
                }
            }

            return tbl;
        }
        private static Boolean Generate(CollectionBase busineesObject, Object item, Boolean writeHeader, String[] Headers, String columnDemiliter, String textQualifier, QualifierType qType, String outputFile, Boolean isAppend)
        {
            ArrayList items = GetList(busineesObject);
            DataTable dt = GetDataSource(items, item);
            return Generate(dt.DefaultView, writeHeader, Headers, columnDemiliter, textQualifier, qType, outputFile, isAppend);
        }
        private static Boolean Generate(DictionaryBase busineesObject, Object item, Boolean writeHeader, String[] Headers, String columnDemiliter, String textQualifier, QualifierType qType, String outputFile, Boolean isAppend)
        {
            ArrayList items = GetList(busineesObject);
            DataTable dt = GetDataSource(items, item);
            return Generate(dt.DefaultView, writeHeader, Headers, columnDemiliter, textQualifier, qType, outputFile, isAppend);
        }
        private static Boolean Generate(DataView dView, Boolean writeHeader, String[] Headers, String columnDemiliter, String textQualifier, QualifierType qType, String outputFile, Boolean isAppend)
        {
            Boolean result = false, hasHeader = false;
            String headerOutput = "", cell = "", head = "";
            Writer _w = null;
            try
            {
                //Validating parameter                
                if (dView == null)
                    throw new Exception("No source data!");
                if (File.Exists(outputFile) && !isAppend)
                    throw new Exception("Output file already exists!");
                if (String.IsNullOrEmpty(columnDemiliter))
                    throw new Exception("Column Demiliter cannot be null or empty!");
                if (columnDemiliter == textQualifier)
                    throw new Exception("Column Demiliter cannot same with Text Qualifier!");

                //if (File.Exists(outputFile) && append)
                //    File.Delete(outputFile);

                if (Headers != null)
                {
                    if (dView.Table.Columns.Count != Headers.Length)
                        throw new Exception("Number of headers not match with source data!");
                    hasHeader = true;
                }

                Type[] columnsType = new Type[dView.Table.Columns.Count];

                _w = new Writer(outputFile, Writer.LogType.Custome, isAppend);
                #region Write Header
                if (writeHeader)
                {
                    headerOutput = "";
                    for (int h = 0; h < dView.Table.Columns.Count; h++)
                    {
                        if (hasHeader)
                            head = Headers[h];
                        else
                            head = dView.Table.Columns[h].ToString();
                        columnsType[h] = dView.Table.Columns[h].DataType;

                        if (qType == QualifierType.All)
                            headerOutput += textQualifier + head + textQualifier;
                        else if (qType == QualifierType.Auto)
                        {
                            if (columnsType[h] == typeof(String) || columnsType[h] == typeof(DateTime) || columnsType[h] == typeof(Char))
                                headerOutput += textQualifier + head + textQualifier;
                            else
                                headerOutput += head;
                        }
                        else
                            headerOutput += head;

                        headerOutput += columnDemiliter.ToString();
                    }
                    if (!String.IsNullOrEmpty(headerOutput))
                    {
                        headerOutput = headerOutput.Substring(0, headerOutput.Length - columnDemiliter.Length);
                        _w.WriteLine(headerOutput);
                    }
                }
                #endregion

                #region Write Detail
                for (int i = 0; i < dView.Count; i++)
                {
                    cell = String.Empty;
                    for (int h = 0; h < dView.Table.Columns.Count; h++)
                    {
                        if (qType == QualifierType.All)
                            cell += textQualifier + dView[i][h].ToString() + textQualifier;
                        else if (qType == QualifierType.Auto)
                        {
                            if (columnsType[h] == typeof(String) || columnsType[h] == typeof(DateTime) || columnsType[h] == typeof(Char))
                                cell += textQualifier + dView[i][h].ToString() + textQualifier;
                            else
                                cell += dView[i][h].ToString();
                        }
                        else
                            cell += dView[i][h].ToString();
                        cell += columnDemiliter.ToString();
                    }
                    if (!String.IsNullOrEmpty(cell))
                    {
                        cell = cell.Substring(0, cell.Length - columnDemiliter.Length);
                        _w.WriteLine(cell);
                    }
                }
                #endregion
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_w != null)
                {
                    _w.Close();
                }
            }
            return result;
        }
        #endregion
    }

    public enum QualifierType
    {
        None = 0,
        All = 1,
        Auto = 2
    }
}
