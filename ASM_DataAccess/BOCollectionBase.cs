using System;
using System.Collections;
using System.Data;

namespace ASM.DataAccess
{
    /// <summary>
    /// BOCollectionBase Class
    /// </summary>
    /// 
    [Serializable()]
    public class BOCollectionBase : CollectionBase
    {
        public ArrayList DeletedList;

        public BOCollectionBase()
        {
            DeletedList = new ArrayList();
        }

        public void RemoveAtIndex(Int32 index)
        {
            DeletedList.Add(this.InnerList[index]);
            base.RemoveAt(index);
        }

        protected DataTable GetDataSource(ArrayList data, Object item)
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
                        if (fieldInfos[j].FieldType.IsGenericType && fieldInfos[j].FieldType.GetGenericTypeDefinition() == typeof(System.Nullable<>))
                            tbl.Columns.Add(fieldInfos[j].Name, fieldInfos[j].FieldType.GetGenericArguments()[0]);
                        else
                            tbl.Columns.Add(fieldInfos[j].Name, fieldInfos[j].FieldType);
                    }
                }
                for (j = 0; j < propInfos.Length; j++)
                {
                    if (propInfos[j].IsDefined((new DbField()).GetType(), false))
                    {
                        if (propInfos[j].PropertyType.IsGenericType && propInfos[j].PropertyType.GetGenericTypeDefinition() == typeof(System.Nullable<>))
                            tbl.Columns.Add(propInfos[j].Name, propInfos[j].PropertyType.GetGenericArguments()[0]);
                        else
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
                        if (fieldInfos[j].FieldType.IsGenericType && fieldInfos[j].FieldType.GetGenericTypeDefinition() == typeof(System.Nullable<>))
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
                        if (propInfos[j].PropertyType.IsGenericType && propInfos[j].PropertyType.GetGenericTypeDefinition() == typeof(System.Nullable<>))
                            tbl.Columns.Add(propInfos[j].Name, propInfos[j].PropertyType.GetGenericArguments()[0]);
                        else
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

        internal DataTable FriendGetDataSource(ArrayList cols, Object item)
        {
            return GetDataSource(cols, item);
        }
    }

    /// <summary>
    /// BODictionaryBase Class
    /// </summary>
    /// 
    [Serializable()]
    public class BODictionaryBase : DictionaryBase
    {
        public ArrayList DeletedList;

        public BODictionaryBase()
        {
            DeletedList = new ArrayList();
        }

        public void Remove(string key)
        {
            DeletedList.Add(this.InnerHashtable[key]);
            base.InnerHashtable.Remove(key);
        }

        protected DataTable GetDataSource(IDictionary dics, Object item)
        {
            IDictionaryEnumerator enm = dics.GetEnumerator();
            ArrayList arr = new ArrayList();
            while (enm.MoveNext())
            {
                arr.Add(enm.Value);
            }

            BOCollectionBase adaptee = new BOCollectionBase();
            adaptee.DeletedList = this.DeletedList;

            return adaptee.FriendGetDataSource(arr, item);
        }
    }

}