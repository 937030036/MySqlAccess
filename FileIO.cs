using System;
using System.IO;
namespace MySqlAccess
{
    public static class FileIO
    {
        public static bool SaveFile(in string ip, in string port,
            in string username, in string psw, in string dbname, out string msg)
        {
            string ipstr = $"ip={ip}";
            string portstr = $"port={port}";
            string usernamestr = $"username={username}";
            string pswstr = $"psw={psw}";
            string dbnamestr = $"dbname={dbname}";
            try
            {
                using (StreamWriter streamWriter = new StreamWriter("../../Info.txt"))
                {
                    streamWriter.WriteLine(ipstr);
                    streamWriter.WriteLine(portstr);
                    streamWriter.WriteLine(usernamestr);
                    streamWriter.WriteLine(pswstr);
                    streamWriter.WriteLine(dbnamestr);
                }
                msg = "Succ";
                return true;
            }
            catch (Exception e)
            {
                msg = e.StackTrace;
                return false;
            }
        }

        public static bool ReadFile(out string ip, out string port,
            out string username, out string psw, out string dbname, out string msg)
        {
            ip = null;
            port = null;
            username = null;
            psw = null;
            dbname = null;
            msg = null;
            try
            {
                using (StreamReader streamReader = new StreamReader("../../Info.txt"))
                {
                    string line = "";
                    char[] sep = { '=' };
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        string[] strings = line.Split(sep, 2);
                        switch (strings[0])
                        {
                            case "ip":
                                ip = strings[1];
                                break;
                            case "port":
                                port= strings[1];
                                break;
                            case "username":
                                username = strings[1];
                                break;
                            case "psw":
                                psw = strings[1];
                                break;
                            case "dbname":
                                dbname = strings[1];
                                break;
                            default:
                                ip = null;
                                port = null;
                                username = null;
                                psw = null;
                                dbname = null;
                                msg = "File data exception";
                                return false;
                        }
                    }
                }
                return true;
            }
            catch(FileNotFoundException)
            {
                ip = "localhost";
                port = "3306";
                username = "";
                psw = "";
                dbname = "";
                msg = "FileNotFound";
                return true;
            }
            catch(Exception e)
            {
                msg = e.StackTrace;
                return false;
            }
        }
    }
}
