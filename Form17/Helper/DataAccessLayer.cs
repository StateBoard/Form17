using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
/// <summary>
/// Summary description for DataAccessLayer
/// </summary>
public  class DataAccessLayer
{
     SqlConnection _Con;
    
   public  DataAccessLayer()
    {

        _Con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString);
    }
    
    public  object ExecuteScalar(string Query)
    {
        Object obj = null;
        try
        {
            SqlCommand _Command;
            OpenConnection();
            _Command = new SqlCommand(Query, _Con);
            obj = _Command.ExecuteScalar();
        }
        catch (Exception ex)
        {
            CloseConnection();
        }
        finally
        {
            CloseConnection();
        }
        return obj;
    }
    public  int ExecuteNonQuery(string Query)
    {
        int Success = 0;
        try
        {
            SqlCommand _Command;
            OpenConnection();
            _Command = new SqlCommand(Query, _Con);
            Success = _Command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Success = -1;
            CloseConnection();
        }
        finally
        {
            CloseConnection();
        }
        return Success;
    }
    public  DataSet ExecuteStoredProcedureToRetDataSet(SqlParameter[] ParamArray, string ProcedureName)
    {
        DataSet _Dataset=null;
        try
        {
            SqlCommand _Command;
            _Dataset = new DataSet();
            OpenConnection();
            _Command = new SqlCommand(ProcedureName, _Con);
            _Command.CommandType = CommandType.StoredProcedure;
            _Command.Parameters.AddRange(ParamArray);
            SqlDataAdapter _DataAdapter = new SqlDataAdapter(_Command);
            _DataAdapter.Fill(_Dataset);
        }
        catch (Exception ex)
        { 
        }
        return _Dataset;
    }
    public  DataTable ExecuteStoredProcedureToRetDataTable(SqlParameter[] ParamArray, string ProcedureName)
    {
        DataSet _Dataset = null;
        DataTable _dt = new DataTable();
        try
        {
            SqlCommand _Command;
            _Dataset = new DataSet();
            OpenConnection();
            _Command = new SqlCommand(ProcedureName, _Con);
            _Command.CommandType = CommandType.StoredProcedure;
            _Command.Parameters.AddRange(ParamArray);
            SqlDataAdapter _DataAdapter = new SqlDataAdapter(_Command);
            _DataAdapter.Fill(_Dataset);
        }
        catch (Exception ex)
        {
            CloseConnection();
            using (StreamWriter writer =
            new StreamWriter("important.txt"))
            {
                writer.Write(ex.ToString());
                writer.WriteLine(ex.ToString());
                writer.WriteLine(ex.ToString());
            }
        }
        if (_Dataset != null && _Dataset.Tables.Count > 0)
            _dt = _Dataset.Tables[0];
        return _dt;
    }

    public  IDataReader ExecuteStoredProcedureToReturnDataReader(SqlParameter[] ParamArray, string ProcedureName)
    {
        try
        {
            SqlCommand _Command;
            OpenConnection();
            _Command = new SqlCommand(ProcedureName, _Con);
            _Command.CommandType = CommandType.StoredProcedure;
            _Command.Parameters.AddRange(ParamArray);

            IDataReader dataReader = _Command.ExecuteReader();

            return dataReader;
        }
        catch (Exception ex)
        {
            throw new Exception("EvaluatorMaster::SelectAll::Error occured.", ex);
        }
        finally
        {
           // CloseConnection();
        }
    }

    private  List<object> PopulateObjectsFromReader(IDataReader dataReader)
    {
        throw new NotImplementedException();
    }

    public  int ExecuteStoredProcedure(SqlParameter[] ParamArray, string ProcedureName)
    {
        int Success = 0;
        try
        {
            SqlCommand _Command;
            OpenConnection();
            _Command = new SqlCommand(ProcedureName, _Con);
            _Command.CommandType = CommandType.StoredProcedure;
            _Command.Parameters.AddRange(ParamArray);
            Success = _Command.ExecuteNonQuery();
           
        }
        catch (Exception ex)
        {
            if (_Con.State == ConnectionState.Open)
            {
                CloseConnection();
            }
        }
        finally
        {
            CloseConnection();
        }
        return Success;
    }

    public  int ExecuteStoredProcedure(SqlParameter[] ParamArray, string ProcedureName, string OutputParameterName)
    {
        int Success = 0;
        try
        {
            SqlCommand _Command;
            OpenConnection();
            _Command = new SqlCommand(ProcedureName, _Con);
            _Command.CommandType = CommandType.StoredProcedure;
            _Command.Parameters.AddRange(ParamArray);
            Success = _Command.ExecuteNonQuery();
            Success = Convert.ToInt32(_Command.Parameters[OutputParameterName].Value);
        }
        catch (Exception ex)
        {
            if (_Con.State == ConnectionState.Open)
            {
                CloseConnection();
            }
        }
        finally
        {
            CloseConnection();
        }
        return Success;
    }

    public  double ExecuteStoredProcedureToReturnDouble(SqlParameter[] ParamArray, string ProcedureName, string OutputParameterName)
    {
        double Success = 0;
        try
        {
            SqlCommand _Command;
            OpenConnection();
            _Command = new SqlCommand(ProcedureName, _Con);
            _Command.CommandType = CommandType.StoredProcedure;
            _Command.Parameters.AddRange(ParamArray);
            Success = _Command.ExecuteNonQuery();
            Success = Convert.ToDouble(_Command.Parameters[OutputParameterName].Value);
        }
        catch (Exception ex)
        {
            if (_Con.State == ConnectionState.Open)
            {
                CloseConnection();
            }
        }
        finally
        {
            CloseConnection();
        }
        return Success;
    }

    public  DataTable ExecuteStoredProcedure(string ProcedureName)
    {
        DataTable _DataTable=null;
        try
        {
            SqlCommand _Command;
            OpenConnection();
            _DataTable = new DataTable();
            _Command = new SqlCommand(ProcedureName, _Con);
            _Command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter _DataAdapter = new SqlDataAdapter(_Command);
            _DataAdapter.Fill(_DataTable);
        }
        catch (Exception ex)
        { }
        return _DataTable;
    }

    public  void CloseConnection()
    {
        try
        {
            if (_Con.State == ConnectionState.Open)
            {
                _Con.Close();
            }
        }
        catch (Exception ex)
        { }
    }
    private  void OpenConnection()
    {
        try
        {
            if (_Con.State != ConnectionState.Open)
            {
                _Con.Open();
            }
        }
        catch (Exception ex)
        {
            using (StreamWriter writer =
         new StreamWriter("important.txt"))
            {
                writer.Write(ex.ToString());
                writer.WriteLine(ex.ToString());
                writer.WriteLine(ex.ToString());
            }
            throw;
        }
    }
    public  DataTable ReturnDataTable(string Query)
     {
        DataTable _DataTable = null;
        try
        {
            SqlDataAdapter _dataAdapter = new SqlDataAdapter(Query, _Con);
            _DataTable = new DataTable();
            _dataAdapter.Fill(_DataTable);
        }
        catch (Exception ex)
        {
            throw;
        }
        return _DataTable;
    }

    public  DataSet ReturnDataSet(string Query)
    {
        DataSet _Dataset= null;
        try
        {
            SqlDataAdapter _dataAdapter = new SqlDataAdapter(Query, _Con);
            _Dataset = new DataSet();
            _dataAdapter.Fill(_Dataset);
        }
        catch (Exception ex)
        { }
        return _Dataset;
    }

    public  IDataReader ReturnDataReader(string Query)
    {
        try
        {
            SqlCommand _Command;
            OpenConnection();
            _Command = new SqlCommand(Query, _Con);
            _Command.CommandType = CommandType.Text;            
            IDataReader dataReader = _Command.ExecuteReader();
            return dataReader;
        }
        catch (Exception ex)
        {
            throw new Exception("::SelectAll::Error occured.", ex);
        }
    }
    
}