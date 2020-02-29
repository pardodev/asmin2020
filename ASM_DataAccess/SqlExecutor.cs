using System;
using System.Data;
using System.Data.SqlClient;

namespace ASM.DataAccess
{
    /// <summary>
    /// SqlExecutor Class for SQL Server
    /// </summary>
    public class SqlExecutor : IDisposable
    {
        public string ConnectionString;
        SqlConnection _connection;
        SqlCommand _command;
        SqlDataAdapter _dataAdapter;
        int m_time_out = 1800;

        /// <summary>
        /// Executor for SQL Server
        /// </summary>
        /// <param name="conString">Connection String to target Database</param>
        public SqlExecutor(string conString)
        {
            this.ConnectionString = conString;
            this.m_time_out = 1800;
        }

        /// <summary>
        /// Executor for SQL Server with define TimeOut
        /// </summary>
        /// <param name="conString">Connection String to target Database</param>
        /// <param name="pTimeOut">TimeOut</param>
        public SqlExecutor(string conString, int pTimeOut)
        {
            this.ConnectionString = conString;
            this.m_time_out = pTimeOut;
        }

        ~SqlExecutor()
        {
            _connection = null;
            _command = null;
            _dataAdapter = null;
        }
        /// <summary>
        /// Execute the SQL Query and return in DataTable, then Close the connection automatically
        /// </summary>
        /// <param name="sql">SQLString</param>
        /// <returns>Data Table</returns>
        public DataTable Read(string sql)
        {
            DataTable dt = new DataTable();
            try
            {
                if (_connection == null)
                    _connection = new SqlConnection(this.ConnectionString);
                _command = new SqlCommand(sql);
                _command.Connection = _connection;
                _command.CommandTimeout = this.m_time_out;
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();
                DataAdapter.SelectCommand = _command;
                DataAdapter.Fill(dt);
            }
            catch (Exception e)
            {
                throw new Exception("Err in Read():" + e.Message);
            }
            finally
            {
                this.Close();
            }
            return dt;
        }

        /// <summary>
        /// Execute the SQL Query and return in DataTable
        /// </summary>
        /// <param name="sql">SQLQuery</param>
        /// <returns>Data Table</returns>
        public DataTable ReadAlive(string sql)
        {
            DataTable dt = new DataTable();
            try
            {
                this.Command.CommandText = sql;
                DataAdapter.SelectCommand = this.Command;
                DataAdapter.Fill(dt);
            }
            catch (Exception e)
            {
                throw new Exception("Err in ReadAlive():" + e.Message);
            }
            finally { }
            return dt;
        }

        /// <summary>
        /// Execute NonQuery and return the record Affected
        /// </summary>
        /// <param name="sql">SQL String</param>
        /// <returns>Record Affected</returns>
        public Int32 ExecuteNonQuery(string sql)
        {
            int recAff = -1;
            try
            {
                if (_connection == null)
                    _connection = new SqlConnection(this.ConnectionString);
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                _command = new SqlCommand(sql);
                _command.CommandTimeout = this.m_time_out;
                _command.Connection = _connection;
                _command.CommandType = CommandType.Text;

                recAff = (Int32)_command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception("Err in ExecuteNonQuery():" + e.Message);
            }
            finally
            {
                this.Close();
            }
            return recAff;
        }

        /// <summary>
        /// Execute NonQuery but leave the connection opened
        /// </summary>
        /// <param name="sql">SQL String</param>
        /// <returns>Record Affected</returns>
        public Int32 ExecuteNonQueryAlive(string sql)
        {
            int recAff = -1;
            try
            {
                this.Command.CommandText = sql;
                recAff = (Int32)this.Command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception("Err in ExecuteNonQueryAlive():" + e.Message);
            }
            finally { }
            return recAff;
        }


        /// <summary>
        /// Execute Query and return in a SQL Data Reader
        /// </summary>
        /// <param name="sql">SQL Query</param>
        /// <returns>SQL DataReader</returns>
        public SqlDataReader ExecuteReader(string sql)
        {
            try
            {
                if (_connection == null)
                    _connection = new SqlConnection(this.ConnectionString);
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                _command = new SqlCommand(sql);
                _command.CommandTimeout = this.m_time_out;
                _command.Connection = _connection;

                return _command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception e)
            {
                throw new Exception("Err in ExecuteReader():" + e.Message);
            }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// Close connection and release all resources
        /// </summary>
        public void Close()
        {
            if (_command != null)
            {
                _command.Dispose();
                _command = null;
            }
            if (_dataAdapter != null)
            {
                _dataAdapter.Dispose();
                _dataAdapter = null;
            }
            if (_connection != null)
            {
                if (_connection.State != ConnectionState.Closed)                
                    _connection.Close();                
                _connection.Dispose();
                _connection = null;
            }
            
        }

        /// <summary>
        /// Close connection and release all resources
        /// </summary>
        public void Dispose()
        {
            this.Close();
        }

        /// <summary>
        /// Get or set time out of SQLCommand
        /// </summary>
        public int TimeOut
        {
            get { return this.m_time_out; }
            set { this.m_time_out = value; }
        }

        protected SqlConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    if (String.IsNullOrEmpty(ConnectionString))
                        throw new Exception("Connection string must be defined before using connection, command or data adapter");

                    _connection = new SqlConnection(ConnectionString);
                    _connection.Open();
                }

                return _connection;
            }
            set
            {
                _connection = value;
            }
        }

        public SqlCommand Command
        {
            get
            {
                if (_command == null)
                {
                    _command = new SqlCommand();
                    _command.Connection = Connection;
                }
                if (_command.Connection.ConnectionString == string.Empty)
                {
                    _command.Connection.ConnectionString = this.ConnectionString;
                }
                _command.CommandTimeout = this.m_time_out;
                return _command;
            }
            set
            {
                _command = value;
            }
        }

        protected SqlDataAdapter DataAdapter
        {
            get
            {
                if (_dataAdapter == null)
                {
                    _dataAdapter = new SqlDataAdapter();
                }

                return _dataAdapter;
            }
            set
            {
                _dataAdapter = value;
            }
        }

        /// <summary>
        /// Execute Scalar and Close connection automatically. Return object must be casting to your own datatype
        /// </summary>
        /// <param name="sql">SQL Query</param>
        /// <returns>return Object, must be casting/convert to your own data type</returns>
        public Object ExecuteScalar(String sql)
        {
            Object ret_obj = null;
            try
            {
                if (_connection == null)
                    _connection = new SqlConnection(this.ConnectionString);
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                _command = new SqlCommand(sql);
                _command.CommandTimeout = this.m_time_out;
                _command.Connection = _connection;

                ret_obj = (Object)_command.ExecuteScalar();
            }
            catch (Exception e)
            {
                throw new Exception("Err in ExecuteScalar():" + e.Message);
            }
            finally
            {
                this.Close();
            }
            return ret_obj;
        }

        /// <summary>
        /// Execute Scalar but leave connection opened. Return object must be casting to your own datatype
        /// </summary>
        /// <param name="sql">SQL String</param>
        /// <returns>Return object must be casting to your own datatype</returns>
        public Object ExecuteScalarAlive(String sql)
        {
            Object ret_obj = null;
            try
            {
                this.Command.CommandText = sql;
                ret_obj = (Object)this.Command.ExecuteScalar();
            }
            catch (Exception e)
            {
                throw new Exception("Err in ExecuteScalarAlive():" + e.Message);
            }
            return ret_obj;
        }
    }
}