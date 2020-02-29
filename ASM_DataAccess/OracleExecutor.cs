using System;
using System.Data;
using System.Data.OracleClient;

namespace ASM.DataAccess
{
    /// <summary>
    /// SQL Executor for Oracle
    /// </summary>
    public class OracleExecutor : IDisposable
    {       
        public String ConnectionString;
        private OracleConnection _connection;
        private OracleCommand _command;
        private OracleDataAdapter _dataAdapter;
        private int m_time_out = 1800;

        /// <summary>
        /// Executor for Oracle, with default time out 7200
        /// </summary>
        /// <param name="conString">Connection String to Oracle</param>
        public OracleExecutor(string conString)
        {
            this.ConnectionString = conString;
            this.m_time_out = 7200;
        }

        /// <summary>
        /// Executor for Oracle with define TimeOut
        /// </summary>
        /// <param name="conString">Connection String to Oracle</param>
        /// <param name="pTimeOut">Time Out</param>
        public OracleExecutor(string conString, int pTimeOut)
        {
            this.ConnectionString = conString;
            this.m_time_out = pTimeOut;
        }

        ~OracleExecutor()
        {
            _connection = null;
            _command = null;
            _dataAdapter = null;
        }

        /// <summary>
        /// Get or set time out of OracleSQLCommand
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
                this._connection = new OracleConnection(this.ConnectionString);
                this._command = new OracleCommand(sql);
                this._command.CommandTimeout = this.TimeOut;
                this._command.Connection = this._connection;
                if (this._connection.State != ConnectionState.Open)
                    this._connection.Open();
                this.DataAdapter.SelectCommand = this._command;
                this.DataAdapter.Fill(dt);
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
                this.Command.CommandText = sql;
                this.DataAdapter.SelectCommand = this.Command;
                this.DataAdapter.Fill(dt);
            }
            catch (Exception e)
            {
                throw new Exception("Err in ReadAlive():" + e.Message);
            }
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
                this._connection = new OracleConnection(this.ConnectionString);
                this._command = new OracleCommand(sql);
                this._command.CommandTimeout = this.TimeOut;
                this._command.Connection = this._connection;
                if (this._connection.State != ConnectionState.Open)
                    this._connection.Open();

                recAff = (Int32)this._command.ExecuteNonQuery();
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
        /// <returns>OracleReader</returns>
        public OracleDataReader ExecuteReader(string sql)
        {
            try
            {
                this._connection = new OracleConnection(this.ConnectionString);
                this._command = new OracleCommand(sql);
                this._command.CommandTimeout = this.TimeOut;
                this._command.Connection = _connection;
                this._connection.Open();

                return this._command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception e)
            {
                throw new Exception("Err in ExecuteReader():" + e.Message);
            }
            finally
            {
                if (this._connection.State != ConnectionState.Closed) 
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
                this.Command.CommandText = sql;
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
            if (this._command != null)
            {
                this._command.Dispose();
                this._command = null;
            }
            if (this._dataAdapter != null)
            {
                this._dataAdapter.Dispose();
                this._dataAdapter = null;
            }
            if (this._connection != null)
            {
                if (this._connection.State != ConnectionState.Closed)                
                    this._connection.Close();
                this._connection.Dispose();                
                this._connection = null;
            }
            
        }

        /// <summary>
        /// Close connection and release all resources
        /// </summary>
        public void Dispose()
        {
            this.Close();
            this.Connection.Dispose();
        }

        protected OracleConnection Connection
        {
            get
            {
                if (this._connection == null)
                {
                    if (String.IsNullOrEmpty(ConnectionString))
                        throw new Exception("Connection string must be defined before using connection, command or data adapter");

                    this._connection = new OracleConnection(ConnectionString);
                }
                if (this._connection.State != ConnectionState.Open)
                    this._connection.Open();

                return this._connection;
            }
            set
            {
                this._connection = value;
            }
        }

        public OracleCommand Command
        {
            get
            {
                if (this._command == null)
                {
                    this._command = new OracleCommand();
                    this._command.Connection = Connection;
                }
                if (this._command.Connection.ConnectionString == string.Empty)
                {
                    this._command.Connection.ConnectionString = this.ConnectionString;
                }
                this._command.CommandTimeout = this.TimeOut;
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }

        protected OracleDataAdapter DataAdapter
        {
            get
            {
                if (this._dataAdapter == null)
                {
                    this._dataAdapter = new OracleDataAdapter();
                }

                return this._dataAdapter;
            }
            set
            {
                this._dataAdapter = value;
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
                this._connection = new OracleConnection(this.ConnectionString);
                this._command = new OracleCommand(sql);
                this._command.CommandTimeout = this.TimeOut;
                this._command.Connection = this._connection;
                if (this._connection.State != ConnectionState.Open)
                    this._connection.Open();

                return (Object)this._command.ExecuteScalar();
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
                this.Command.CommandText = sql;
                return (Object)Command.ExecuteScalar();
            }
            catch (Exception e)
            {
                throw new Exception("Err in ExecuteScalarAlive():" + e.Message);
            }
        }
    }
}
