using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace File_Manager
{
    class Program
    {
        public static string conString = "Data Source=DESKTOP-NG0VQ9J\\SQLEXPRESS;Initial Catalog=LocationEditsDB;Integrated Security=True";
        
        static void Main(string[] args)
        {
            
            string root = @"D:\CSharp Progs\Files";
            string dest = @"D:\CSharp Progs\FilesSorted";
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            if (!Directory.Exists(dest))
            {
                Directory.CreateDirectory(dest);
            }
            string[] fils = Directory.GetFiles(root);
            int i = 1;
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            SqlCommand delCmd = new SqlCommand("delete from FileManager", con);
            delCmd.ExecuteNonQuery();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "FileTracker";
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (string fil in fils)
            {
                int indexOfDot = fil.LastIndexOf('.');
                string ext = fil.Substring(indexOfDot + 1);
                string fileName = fil.Substring(fil.LastIndexOf('\\'));
               
                string extFol = @"D:\CSharp Progs\FilesSorted\" + ext;
                
                if (con.State == System.Data.ConnectionState.Open)
                {
                    //string q = "insert into FileManager values("+i+",'" + fil + "','" + dest + "\\" + ext + fil.Substring(fil.LastIndexOf('\\')) + "','" + fileName + "')";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new SqlParameter("@Idparam",i));
                    cmd.Parameters.Add(new SqlParameter("@LocBeforeparam",fil));
                    cmd.Parameters.Add(new SqlParameter("@LocAfterparam", dest + "\\" + ext + fil.Substring(fil.LastIndexOf('\\'))));
                    cmd.Parameters.Add(new SqlParameter("@fileNameparam", fileName));
                    if (Directory.Exists(extFol))
                    {
                        File.Move(@fil, dest + "\\" + ext + fil.Substring(fil.LastIndexOf('\\')));
                    }
                    else
                    {
                        Directory.CreateDirectory(extFol);
                        File.Move(@fil, dest + "\\" + ext + fil.Substring(fil.LastIndexOf('\\')));
                    }
                    
                    cmd.ExecuteNonQuery();
                }
                i++;
                
                

            }
            con.Close();
            Console.ReadLine();
        }
    }
}
