using System;
using System.Data;
using System.Data.OleDb;

namespace ASM.DataAccess
{
    /// <summary>
    /// SQL Executor for OLEDB
    /// </summary>
    public class OleDbExecutor : IDisposable
    {
        public string ConnectionString;
        OleDbConnection _connection;
        OleDbCommand _command;
        OleDbDataAdapter _dataAdapter;
        int m_time_out = 1800;

        /// <summary>
        /// Executor for OLEDB
        /// </summary>
        /// <param name="conString">Connection String to OLE DB</param>
        public OleDbExecutor(string conString)
        {
            this.ConnectionString = conString;
            this.m_time_out = 1800;
        }

        /// <summary>
        /// Executor for OLE DB with define TimeOut
        /// </summary>
        /// <param name="conString">Connection String to OLE DB</param>
        /// <param name="pTimeOut">Time Out</param>
        public OleDbExecutor(string conString, int pTimeOut)
        {
            this.ConnectionString = conString;
            this.m_time_out = pTimeOut;
        }

        ~OleDbExecutor()
        {
            _connection = null;
            _command = null;
            _dataAdapter = null;
        }

        /// <summary>
        /// Get or set time out of OLEDBSQLCommand
        /// </summary>
        public int TimeOut
        {
            get { return this.m_time_out; }
            set { this.m_time_out = value; }
        }

        /// <summary>
        /// Execute the SQL Query and return in DataTable, then Close the connection automatically
        /// </summary>
        /// <param name="sql">SQL Query</param>
        /// <returns>Data Table</returns>
        public DataTable Read(string sql)
        {
            DataTable dt = null;
            try
            {
                dt = new DataTable();
                _connection = new OleDbConnection(this.ConnectionString);
                _command = new OleDbCommand(sql);
                _command.CommandTimeout = this.m_time_out;
                _command.Connection = _connection;
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
        /// <param name="sql">SQL Query</param>
        /// <returns>Data Table</returns>
        public DataTable ReadAlive(string sql)
        {
            DataTable dt = null;
            try
            {
                dt = new DataTable();
                Command.CommandText = sql;
                DataAdapter.SelectCommand = Command;
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
        /// <param name="sql">SQL Query</param>
        /// <returns>Record Affected</returns>
        public Int32 ExecuteNonQuery(string sql)
        {
            int recAff = -1;
            try
            {
                _connection = new OleDbConnection(this.ConnectionString);
                _command = new OleDbCommand(sql);
                _command.CommandTimeout = this.m_time_out;
                _command.Connection = _connection;
                if (_connection.State == ConnectionState.Closed) _connection.Open();

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
        /// Execute Query and return in a SQL Data Reader
        /// </summary>
        /// <param name="sql">SQL Query</param>
        /// <returns>OleDbReader</returns>
        public OleDbDataReader ExecuteReader(string sql)
        {
            try
            {
                _connection = new OleDbConnection(this.ConnectionString);
                _command = new OleDbCommand(sql);
                _command.CommandTimeout = this.m_time_out;
                _command.Connection = _connection;
                _connection.Open();

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
        /// Execute NonQuery but leave the connection opened
        /// </summary>
        /// <param name="sql">SQL Query</param>
        /// <returns>Record Affected</returns>
        public Int32 ExecuteNonQueryAlive(string sql)
        {
            int recAff = -1;
            try
            {
                Command.CommandText = sql;
                recAff = (Int32)Command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception("Err in ExecuteNonQueryAlive():" + e.Message);
            }
            finally { }
            return recAff;
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

        protected OleDbConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    if (ConnectionString == "")
                        throw new Exception("Connection string must be defined before using connection, command or data adapter");

                    _connection = new OleDbConnection(ConnectionString);
                    _connection.Open();
                }

                return _connection;
            }
            set
            {
                _connection = value;
            }
        }

        public OleDbCommand Command
        {
            get
            {
                if (_command == null)
                {
                    _command = new OleDbCommand();
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

        protected OleDbDataAdapter DataAdapter
        {
            get
            {
                if (_dataAdapter == null)
                {
                    _dataAdapter = new OleDbDataAdapter();
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
            try
            {
                _connection = new OleDbConnection(this.ConnectionString);
                _command = new OleDbCommand(sql);
                _command.CommandTimeout = this.m_time_out;
                _command.Connection = _connection;
                if (_connection.State == ConnectionState.Closed) _connection.Open();

                return (Object)_command.ExecuteScalar();
            }
            catch (Exception e)
            {
                throw new Exception("Err in ExecuteScalar():" + e.Message);
            }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// Execute Scalar but leave connection opened. Return object must be casting to your own datatype
        /// </summary>
        /// <param name="sql">SQL Query</param>
        /// <returns>Return object must be casting to your own datatype</returns>
        public Object ExecuteScalarAlive(String sql)
        {
            try
            {
                Command.CommandText = sql;
                return (Object)Command.ExecuteScalar();
            }
            catch (Exception e)
            {
                throw new Exception("Err in ExecuteScalarAlive():" + e.Message);
            }
            finally { }
            return null;
        }

    }
}
