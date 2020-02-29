using System;
namespace ASM.DataAccess
{
    /// <summary>
    /// PrimaryKey Class
    /// </summary>
    public class PrimaryKey
    {
        public String Name;
        public String Value;
        public DbFieldType Type;
        public DbSearchType SearchTypeKey;

        public PrimaryKey(string name, string value)
        {
            this.Name = name;
            this.Value = value;
            this.Type = DbFieldType.Text;
            this.SearchTypeKey = DbSearchType.Equal;
        }

        public PrimaryKey(string name, string value, DbFieldType type)
        {
            this.Name = name;
            this.Value = value;
            this.Type = type;
            this.SearchTypeKey = DbSearchType.Equal;
        }

        public PrimaryKey(string name, string value, DbFieldType type, DbSearchType pDbSearchTypeKey)
        {
            this.Name = name;
            this.Value = value;
            this.Type = type;
            this.SearchTypeKey = pDbSearchTypeKey;
        }

    }
}