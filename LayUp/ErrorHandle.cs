using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
namespace fxplc_comm
{
  public  static class ErrorHandle
    {
     public static SQLiteConnection m_dbConnection;
      private static string DB_FileName = "ErrorLog.db";
      static byte[]  MDelayStatus = new byte[16];
      static byte[] M128Status = new byte[16];
     static SQLiteCommand cmd ;
   static   SQLiteDataAdapter da ;
   static DataTable dt;
   //构造函数

   #region 初始化数据库并显示
   public static void Init(ref System.Windows.Forms.DataGridView dgv)
   {
       //创建数据库文件
       if (!System.IO.File.Exists(@DB_FileName))
       {
           SQLiteConnection.CreateFile(@DB_FileName);

       }
       //创建数据库连接
       m_dbConnection = new SQLiteConnection("data source=" + "ErrorLog.db");
       m_dbConnection.Open();
       //创建数据表
       string sqlcmd = "CREATE TABLE IF NOT EXISTS ErrorLog (ErrorType TEXT, TimeStamp TEXT ,DelayName TEXT,Errordescription TEXT )";
       cmd = new SQLiteCommand(sqlcmd, m_dbConnection);
       cmd.ExecuteNonQuery();
       //填充数据表
       string SQL = "SELECT ErrorType, TimeStamp  ,DelayName,Errordescription  FROM ErrorLog";
       cmd = new SQLiteCommand(SQL, m_dbConnection);
       da = new SQLiteDataAdapter(cmd);
       dt = new DataTable();
       da.Fill(dt);
       //将数据库内容填充到DataGridView
       dgv.DataSource = dt;

   }
   #endregion


   #region 更新数据库

   public static void UpdateErrorLog(List<Error> ErrorToUpdate, ref System.Windows.Forms.DataGridView dgv)
   {
       //定义一个数据库事务
       var transaction = m_dbConnection.BeginTransaction();

       foreach (var item in ErrorToUpdate)
       {
          
           string sqlcmd = "INSERT INTO ErrorLog(ErrorType,TimeStamp ,DelayName ,Errordescription ) values(@ErrorType,@TimeStamp ,@DelayName ,@Errordescription)";

           cmd = new SQLiteCommand(sqlcmd, m_dbConnection);
           cmd.Parameters.AddWithValue("@ErrorType", "1");
           cmd.Parameters.AddWithValue("@TimeStamp", item.ErrorTime.ToString("yyyy-MM-dd HH:mm:ss"));
           cmd.Parameters.AddWithValue("@DelayName", item.ID);
           cmd.Parameters.AddWithValue("@Errordescription", item.Description);
           cmd.ExecuteNonQuery();
       }
       //执行事务
       transaction.Commit();
       //使用数据库重新填充DataGridView
       try
       {
           dt = new DataTable();
           da.Fill(dt);
           dgv.DataSource = dt;
       }
       catch (Exception ex)
       {
           System.Windows.Forms.MessageBox.Show(ex.Message);
       }


   }
          
   #endregion
   
      

 
      public static void CloseDatabase()
      {
          m_dbConnection.Close();
      }

      public static void exportToDataView(System.Windows.Forms.DataGridView
dgv )
      {
        
       
       
      }

      #region 清空数据库和表格

      #endregion
      public static void ClearLog(ref System.Windows.Forms.DataGridView dgv)
      {
          //清空数据表
          string sqlcmd = "DELETE FROM ErrorLog ";

          cmd = new SQLiteCommand(sqlcmd, m_dbConnection);
          cmd.ExecuteNonQuery();
          //使用数据表内容填充DataGridView
          try
          {
              dt = new DataTable();
              da.Fill(dt);
              dgv.DataSource = dt;
          }
          catch (Exception ex)
          {
              System.Windows.Forms.MessageBox.Show(ex.Message);
          }
      }
      #region 导出错误日志

     
      public static void ExportTable()
      {
          try
          {
              StreamWriter file = new StreamWriter(@"C:\LayupPLCErrorLog.txt");
              dt = new DataTable();
              da.Fill(dt);
              string txt = string.Empty;
              //  txt += "#";
              foreach (DataRow row in dt.Rows)
              {
                  foreach (DataColumn column in dt.Columns)
                  {
                      txt += row[column.ColumnName].ToString() + "    ";
                  }
                  file.WriteLine(txt.ToString());
                  txt = "";
              }
              file.Close();
          }
          catch (Exception e)
          {

              System.Windows.Forms.MessageBox.Show(e.Message);
          }

      }
      #endregion
      
    }
 
}
