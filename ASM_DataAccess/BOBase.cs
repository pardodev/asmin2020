using System;
using System.Data;

namespace ASM.DataAccess
{
    /// <summary>
    /// BOBase Class
    /// </summary>
    [Serializable()]
    public class BOBase
    {
        public DataRowState RowState;

        public BOBase()
        {
        }

        protected DataTable GetDataSource(Object item)
        {
            DataTable tbl = new DataTable();
            System.Reflection.FieldInfo[] fieldInfos;
            System.Reflection.PropertyInfo[] propInfos;
            Object[] rowValues;
            Int32 fieldCount = 0;
            Int32 propCount = 0;
            Int32 columnCount = 0;
            Int32 j;

            if (item == null) return null;

            //Get Fields and Properties

            fieldInfos = item.GetType().GetFields();
            propInfos = item.GetType().GetProperties();

            //Create table columns
            for (j = 0; j < fieldInfos.Length; j++)
            {
                if (fieldInfos[j].IsDefined((new DbField()).GetType(), false))
                {
                    if(fieldInfos[j].FieldType.IsGenericType && fieldInfos[j].FieldType.GetGenericTypeDefinition() == typeof(System.Nullable<>))
                        tbl.Columns.Add(fieldInfos[j].Name, fieldInfos[j].FieldType.GetGenericArguments()[0]);
                    else
                        tbl.Columns.Add(fieldInfos[j].Name, fieldInfos[j].FieldType);
                    fieldCount += 1;
                    columnCount += 1;
                }
            }

            for (j = 0; j < propInfos.Length; j++)
            {
                if (fieldInfos[j].IsDefined((new DbField()).GetType(), false))
                {
                    if(propInfos[j].PropertyType.IsGenericType && propInfos[j].PropertyType.GetGenericTypeDefinition() == typeof(System.Nullable<>))
                        tbl.Columns.Add(propInfos[j].Name, propInfos[j].PropertyType.GetGenericArguments()[0]);
                    else
                        tbl.Columns.Add(propInfos[j].Name, propInfos[j].PropertyType);
                    propCount += 1;
                    columnCount += 1;
                }
            }


            rowValues = new Object[columnCount];

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
                            rowValues[idx] = fieldInfos[j].GetValue(item);
                            idx += 1;
                        }
                    }
                }

                if (propCount > 0)
                {
                    for (j = 0; j < propInfos.Length; j++)
                    {
                        if (fieldInfos[j].IsDefined((new DbField()).GetType(), false))
                        {
                            rowValues[idx] = propInfos[j].GetValue(item, null);
                            idx += 1;
                        }
                    }
                }

                tbl.Rows.Add(rowValues);
            }

            return tbl;
        }

    }
}