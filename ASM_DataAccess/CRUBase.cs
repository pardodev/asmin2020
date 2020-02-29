using System;
using System.Reflection;
using System.Collections;
using System.Data;

namespace ASM.DataAccess
{
    /// <summary>
    /// CRUBase Class
    /// </summary>
    public class CRUBase
    {
        public CRUBase()
        {
        }

        //Generate Primary Key for WHERE CLAUSE ...
        protected static PrimaryKey[] GeneratePrimaryKeys(string[] paramNames, Object[] paramValues, DbFieldType[] paramTypes, DbSearchType[] paramSearch)
        {
            if (paramNames == null) return null;
            if (paramValues == null) return null;
            if (paramTypes == null) return null;

            if (paramNames.Length != paramValues.Length) return null;
            if (paramNames.Length != paramTypes.Length) return null;
            if (paramSearch != null)
                if (paramNames.Length != paramSearch.Length) return null;

            PrimaryKey[] pks = new PrimaryKey[paramNames.Length];
            Int16 idx = 0;
            foreach (Object param in paramNames)
            {
                //Names
                if (string.IsNullOrEmpty(paramNames[idx])) return null;

                //Values
                if (string.IsNullOrEmpty(Convert.ToString(paramValues[idx]))) paramValues[idx] = "null";
                if (paramValues[idx].ToString().Contains("1/1/0001")) paramValues[idx] = "null";
                if (paramValues[idx].ToString().Contains("1/1/1900")) paramValues[idx] = "null";

                if (paramSearch == null)
                    pks[idx] = new PrimaryKey(paramNames[idx], Convert.ToString(paramValues[idx]), paramTypes[idx]);
                else
                    pks[idx] = new PrimaryKey(paramNames[idx], Convert.ToString(paramValues[idx]), paramTypes[idx], paramSearch[idx]);

                idx++;
            }

            return pks;
        }

        protected static PrimaryKey[] GeneratePrimaryKeys(string[] paramNames, Object[] paramValues, DbFieldType[] paramTypes)
        {
            return GeneratePrimaryKeys(paramNames, paramValues, paramTypes, null);
        }

        //This function returns where clause string
        private static string BuildSqlWhere(PrimaryKey[] keys)
        {
            string whereClause = "";
            if (keys != null)
            {
                string swc = "";
                for (Int32 i = 0; i < keys.Length; i++)
                {
                    if (keys[i] != null)
                    {
                        if (keys[i].Value == "null") keys[i].SearchTypeKey = DbSearchType.Equal;
                        string opr = "=";
                        bool _LikeOrNotLike = false;
                        bool _InOrNotIn = false;
                        switch (keys[i].SearchTypeKey)
                        {
                            case DbSearchType.Equal: opr = "="; break;
                            case DbSearchType.NotEqual: opr = "<>"; break;
                            case DbSearchType.LessThan: opr = "<"; break;
                            case DbSearchType.GreaterThan: opr = ">"; break;
                            case DbSearchType.LessThanOrEqual: opr = "<="; break;
                            case DbSearchType.GreaterThanOrEqual: opr = ">="; break;
                            case DbSearchType.In: opr = "IN"; _InOrNotIn = true; break;
                            case DbSearchType.NotIn: opr = "NOT IN"; _InOrNotIn = true; break;
                            case DbSearchType.Like:
                                opr = "LIKE";
                                _LikeOrNotLike = true;
                                break;
                            case DbSearchType.NotLike:
                                opr = "NOT LIKE";
                                _LikeOrNotLike = true;
                                break;

                        }

                        if (_LikeOrNotLike)
                        {                            
                            if(keys[i].Type == DbFieldType.Text)
                                swc = "(" + keys[i].Name + " " + opr + " '" + keys[i].Value + "%')";
                            else
                            {
                                if(opr == "LIKE")
                                    opr = "=";
                                if(opr == "NOT LIKE")
                                    opr = "<>";
                                swc = "(" + keys[i].Name + " " + opr + " " + keys[i].Value + " or " + keys[i].Value + " is null)";
                            }                            
                        }
                        else if(_InOrNotIn)
                        {
                            if(keys[i].Type == DbFieldType.NonText)
                            {
                                if(!string.IsNullOrEmpty(keys[i].Value ))
                                    swc = "(" + keys[i].Name + " " + opr + " (" + keys[i].Value + "))";
                            }
                            else                                
                                swc = "(" + keys[i].Name + " " + opr + " ('" + keys[i].Value.Replace(",", "','") + "'))";
                        }
                        else
                        {
                            if(keys[i].Type == DbFieldType.Text)
                                swc = "(" + keys[i].Name + " " + opr + " '" + keys[i].Value + "' or " + (keys[i].Value == "null" ? "null" : "'" + keys[i].Value + "'") + " is null)";
                            else
                                swc = "(" + keys[i].Name + " " + opr + " " + keys[i].Value + " or " + keys[i].Value + " is null)";
                        }

                        if(i > 0)
                        {
                            if(!String.IsNullOrEmpty(swc))
                            {
                                if(!String.IsNullOrEmpty(whereClause))
                                    whereClause = whereClause + " AND " + swc;
                                else
                                    whereClause = whereClause + swc;
                            }
                        }
                        else
                            whereClause = swc;
                    }
                }
                if (whereClause != "") whereClause = " WHERE " + whereClause;
            }

            return whereClause;
        }

        //This function returns sql string: SELECT ... FROM ... 
        protected static string BuildSqlSelect(Object item, string tableName, bool IsSelectAll)
        {
            return BuildSqlSelect(item, tableName, new PrimaryKey[1] { null }, IsSelectAll);
        }

        //This function returns sql string: SELECT ... FROM ... WHERE ...
        protected static string BuildSqlSelect(Object item, string tableName, PrimaryKey key, bool IsSelectAll)
        {
            return BuildSqlSelect(item, tableName, new PrimaryKey[1] { key }, IsSelectAll);
        }

        //This function returns sql string: SELECT ... FROM ... WHERE ...
        protected static string BuildSqlSelect(Object item, string tableName, PrimaryKey[] keys, bool IsSelectAll)
        {
            DbField attr;
            ArrayList fldList;
            string fieldClause = "";
            Int32 p = 1;
            string f;

            //Create the Field Clause as "*" OR "field1, field2, field3, ...." 
            fldList = GetSelectableFields(item);
            if (IsSelectAll)
            {
                fieldClause = tableName + ".*";
                for (Int32 i = 0; i < fldList.Count; i++)
                {
                    attr = (DbField)fldList[i];
                    fieldClause = fieldClause + ", " + GetFieldSelectClause(attr, tableName);
                }
            }
            else
            {
                for (Int32 i = 0; i < fldList.Count; i++)
                {
                    attr = (DbField)fldList[i];
                    f = GetFieldSelectClause(attr, tableName);
                    if (p > 1)
                        fieldClause = fieldClause + ", " + f;
                    else
                        fieldClause = f;

                    p += 1;
                }
            }

            if (fieldClause == "")
                throw new Exception("There is no field to select, please define the attribute on the class");


            //Create the Where Clause as "WHERE field1=value1 AND field2='value2' AND ...."
            string whereClause = "";
            whereClause = BuildSqlWhere(keys);


            //Create the SQL SELECT Statement
            string sql;
            sql = "SELECT " + fieldClause + " FROM " + tableName + whereClause;

            return sql;
        }

        //This function returns sql string: INSERT INTO ... VALUES ...
        //INSERT INTO Table1(column1, column2) VALUES ('value1', 'value2')
        protected static string BuildSqlInsert(Object businessObject, string tableName)
        {
            Hashtable map;
            string fieldClause = "";
            Int32 p = 1;
            string fn;
            string fv;
            string fldNames = "";
            string fldValues = "";

            //Create the Field Clause
            map = GetInsertableFields(businessObject);
            foreach (DictionaryEntry entry in map)
            {
                fn = Convert.ToString(entry.Key);
                fv = Convert.ToString(entry.Value);
                if (fv.Contains("1/1/0001")) fv = "null";
                if (p > 1)
                {
                    fldNames = fldNames + ", " + fn;
                    fldValues = fldValues + ", " + fv;
                }
                else
                {
                    fldNames = fn;
                    fldValues = fv;
                }
                p += 1;
            }

            if (fldNames != "")
            {
                fldNames = "(" + fldNames + ")";
                fldValues = "(" + fldValues + ")";

                //Create the SQL INSERT Statement
                string sql;
                sql = "INSERT INTO " + tableName + fldNames + " VALUES " + fldValues;

                return sql;
            }
            else
                return "";

        }

        //This function returns sql string: UPDATE ... WHERE ...
        protected static string BuildSqlUpdate(Object businessObject, string tableName, PrimaryKey key)
        {
            return BuildSqlUpdate(businessObject, tableName, new PrimaryKey[1] { key });
        }

        //This function returns sql string: UPDATE ... WHERE ...
        protected static string BuildSqlUpdate(Object businessObject, string tableName, PrimaryKey[] keys)
        {
            ArrayList list;
            string fieldClause = "";
            Int32 p = 1;
            string f;

            //Create the Field Clause "field1=value1, field2=value2, field3=value3, ...."
            list = GetUpdatableFields(businessObject);
            for (Int32 i = 0; i < list.Count; i++)
            {
                f = (string)list[i];
                if (p > 1)
                {
                    if (f != "") fieldClause = fieldClause + ", " + f;
                }
                else
                    fieldClause = f;

                p += 1;
            }

            if (fieldClause == "")
                throw new Exception("There is no field to update, please define the attribute on the class");


            //Create the Where Clause "WHERE field1=value1 AND field2='value2' ...."
            string whereClause = "";
            whereClause = BuildSqlWhere(keys);

            //Create the SQL UPDATE Statement
            string sql;
            sql = "UPDATE " + tableName + " SET " + fieldClause + " " + whereClause;

            return sql;
        }

        //This function returns sql string: DELETE ... WHERE
        protected static string BuildSqlDelete(string tableName, PrimaryKey key)
        {
            return BuildSqlDelete(tableName, new PrimaryKey[1] { key });
        }

        //This function returns sql string: DELETE ... WHERE
        protected static string BuildSqlDelete(string tableName, PrimaryKey[] keys)
        {
            //Create the Where Clause
            string whereClause = "";
            whereClause = BuildSqlWhere(keys);

            //Create the SQL UPDATE Statement
            string sql;
            sql = "DELETE " + tableName + whereClause;

            return sql;
        }

        //This function fills the properties/fields of businessObject with datarow values
        protected static void FillProperties(Object businessObject, DataRow dataRow)
        {
            FieldInfo[] fieldInfos = businessObject.GetType().GetFields();
            PropertyInfo[] propInfos = businessObject.GetType().GetProperties();
            FieldInfo errField = null;
            PropertyInfo errProp = null;
            DbField fieldAttr = null;
            string fieldName = "";

            //pasang exception di sini kalo tipe property dr class tdk compatible dgn tipe field db
            try
            {
                for (Int32 i = 0; i < fieldInfos.Length; i++)
                {
                    if (fieldInfos[i].IsDefined((new DbField()).GetType(), false))
                    {
                        errField = fieldInfos[i];
                        fieldAttr = (DbField)fieldInfos[i].GetCustomAttributes((new DbField()).GetType(), false)[0];

                        if (string.IsNullOrEmpty(fieldAttr.FieldNameAlias))
                            fieldName = fieldAttr.FieldName;
                        else
                            fieldName = fieldAttr.FieldNameAlias;

                        if (dataRow[fieldName] != DBNull.Value)
                            fieldInfos[i].SetValue(businessObject, dataRow[fieldName]);
                    }
                }
            }
            catch
            {
                throw new Exception("Error: Class field '" + errField.Name.ToString() + "' of type " + errField.FieldType.ToString() + " is incompatible with database field '" + fieldName.ToString() + "' of type " + dataRow[fieldName].GetType().ToString());
            }
            finally
            { }

            try
            {
                for (Int32 i = 0; i < propInfos.Length; i++)
                {
                    if (propInfos[i].IsDefined((new DbField()).GetType(), false))
                    {
                        errProp = propInfos[i];
                        fieldAttr = (DbField)propInfos[i].GetCustomAttributes((new DbField()).GetType(), false)[0];
                        fieldName = fieldAttr.FieldName;
                        if (dataRow[fieldName] != DBNull.Value)
                            propInfos[i].SetValue(businessObject, dataRow[fieldName], null);
                    }
                }
            }
            catch
            {
                throw new Exception("Error: Class property '" + errProp.Name + "' of type " + errProp.PropertyType.ToString() + " is incompatible with database field '" + fieldName.ToString() + "' of type " + dataRow[fieldName].GetType().ToString());
            }
            finally
            { }
        }

        //This function returns the dictionary of: property_name & property_attribute
        private static ArrayList GetSelectableFields(Object businessObject)
        {
            FieldInfo[] fieldInfos = businessObject.GetType().GetFields();
            PropertyInfo[] propInfos = businessObject.GetType().GetProperties();
            ArrayList list = new ArrayList();
            FieldInfo fi;
            PropertyInfo pi;
            DbField attr;

            for (Int32 i = 0; i < fieldInfos.Length; i++)
            {
                fi = fieldInfos[i];
                if (fi.IsDefined((new DbField()).GetType(), false))
                {
                    attr = (DbField)fi.GetCustomAttributes((new DbField()).GetType(), false)[0];
                    list.Add(attr);
                }
            }

            for (Int32 i = 0; i < propInfos.Length; i++)
            {
                pi = propInfos[i];
                if (pi.IsDefined((new DbField()).GetType(), false))
                {
                    attr = (DbField)pi.GetCustomAttributes((new DbField()).GetType(), false)[0];
                    list.Add(attr);
                }
            }
            return list;
        }

        //This function returns the dictionary of: property_name & property_attribute that can be inserted to database
        private static Hashtable GetInsertableFields(Object businessObject)
        {
            FieldInfo[] fieldInfos = businessObject.GetType().GetFields();
            PropertyInfo[] propInfos = businessObject.GetType().GetProperties();
            FieldInfo fi;
            PropertyInfo pi;
            DbField attr;
            Hashtable map = new Hashtable();
            Object v;
            string value;
            for (Int32 i = 0; i < fieldInfos.Length; i++)
            {
                fi = fieldInfos[i];
                if (fi.IsDefined((new DbField()).GetType(), false) && !fi.IsDefined((new DbReadOnly()).GetType(), false))
                {
                    attr = (DbField)fi.GetCustomAttributes((new DbField()).GetType(), false)[0];
                    if (string.IsNullOrEmpty(attr.LookupTable))
                    {
                        if (fi.IsDefined((new DbCreatableAutoIncrement()).GetType(), false))
                        {
                            if (fi.FieldType.UnderlyingSystemType == typeof(Int64))
                            {
                                Int64 uniqVal = BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0);
                                map.Add(attr.FieldName, uniqVal);
                                fi.SetValue(businessObject, uniqVal);
                            }
                            else
                            {
                                Int32 uniqVal = Guid.NewGuid().GetHashCode();
                                map.Add(attr.FieldName, uniqVal);
                                fi.SetValue(businessObject, uniqVal);
                            }
                        }
                        else
                        {
                            v = fi.GetValue(businessObject);
                            if (v != null)
                            {
                                value = Convert.ToString(v);
                                if (value != string.Empty)
                                {
                                    if (attr.FieldType == DbFieldType.Text) value = "'" + value.Replace("'", "''") + "'";

                                    map.Add(attr.FieldName, value);
                                }
                            }
                        }
                    }
                }
            }

            for (Int32 i = 0; i < propInfos.Length; i++)
            {
                pi = propInfos[i];
                if (pi.IsDefined((new DbField()).GetType(), false) && !pi.IsDefined((new DbReadOnly()).GetType(), false))
                {
                    attr = (DbField)pi.GetCustomAttributes((new DbField()).GetType(), false)[0];
                    if (pi.IsDefined((new DbCreatableAutoIncrement()).GetType(), false))
                    {
                        Int32 uniqVal = Guid.NewGuid().GetHashCode();
                        map.Add(attr.FieldName, uniqVal);
                        pi.SetValue(businessObject, uniqVal, null);
                    }
                    else
                    {
                        v = pi.GetValue(businessObject, null);
                        if (v != null)
                        {
                            value = Convert.ToString(v);
                            if (value != string.Empty)
                            {
                                if (attr.FieldType == DbFieldType.Text) value = "'" + value.Replace("'", "''") + "'";

                                map.Add(attr.FieldName, value);
                            }
                        }
                    }
                }
            }

            return map;
        }

        //This function returns the arraylist of string with the format: "fieldName=fieldValue"
        private static ArrayList GetUpdatableFields(Object businessObject)
        {
            FieldInfo[] fieldInfos = businessObject.GetType().GetFields();
            PropertyInfo[] propInfos = businessObject.GetType().GetProperties();
            FieldInfo fi;
            PropertyInfo pi;
            DbField attr;
            ArrayList list = new ArrayList();
            Object v;
            string value;

            for (Int32 i = 0; i < fieldInfos.Length; i++)
            {
                fi = fieldInfos[i];
                if (fi.IsDefined((new DbField()).GetType(), false) && !(fi.IsDefined((new DbReadOnly()).GetType(), false) || fi.IsDefined((new DbCreatable()).GetType(), false)))
                {
                    attr = (DbField)fi.GetCustomAttributes((new DbField()).GetType(), false)[0];
                    if (string.IsNullOrEmpty(attr.LookupTable))
                    {
                        v = fi.GetValue(businessObject);                        
                        if (v != null)
                        {
                            value = Convert.ToString(v).Trim();
                            if(value.Contains("1/1/0001") || value.Contains("1/1/1900"))
                            {
                                value = string.Empty;
                                //list.Add(attr.FieldName + "=null");
                            }
                            else
                            {
                                if(attr.FieldType == DbFieldType.Text)
                                    value = "'" + value.Replace("'", "''") + "'";
                                list.Add(attr.FieldName + "=" + value);
                            }
                        }
                    }
                }
            }

            for (Int32 i = 0; i < propInfos.Length; i++)
            {
                pi = propInfos[i];
                if (pi.IsDefined((new DbField()).GetType(), false) && !(pi.IsDefined((new DbReadOnly()).GetType(), false) || pi.IsDefined((new DbCreatable()).GetType(), false)))
                {
                    attr = (DbField)pi.GetCustomAttributes((new DbField()).GetType(), false)[0];
                    v = pi.GetValue(businessObject, null);
                    if (v != null)
                    {
                        value = Convert.ToString(v).Trim();
                        if (value != string.Empty)
                        {
                            if (attr.FieldType == DbFieldType.Text) value = "'" + value.Replace("'", "''") + "'";

                            list.Add(attr.FieldName + "=" + value);
                        }
                    }
                }
            }

            return list;
        }

        private static string GetFieldSelectClause(DbField attr, string tableName)
        {
            if (!attr.IsLookupField)
                return tableName + "." + attr.FieldName;
            else
            {
                string whereSubClause = (attr.ParentPK == "") ? attr.LookupTable + "." + attr.ChildPK + "=" + tableName + "." + attr.ChildPK : attr.LookupTable + "." + attr.ChildPK + "=" + tableName + "." + attr.ParentPK;

                string aliasSubClause = (attr.FieldNameAlias == "") ? " AS " + attr.FieldName : " AS " + attr.FieldNameAlias;

                return "(SELECT " + attr.FieldName + " FROM " + attr.LookupTable + " WHERE " + whereSubClause + ")" + aliasSubClause;
            }
        }
    }
}
