using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;


namespace PrayFileSync
{
  public  class exclude
    {
        public exclude()
        {
            GetExcludeDir();
            GetExcludeFile();
            GetTrustDir();
           
        }
        /// <summary>
        /// 排除目录
        /// </summary>
        public List<string> ExcludeDir = new List<string>(); // "outlook" 目录过滤
        public List<string> TrustDir = new List<string>(); // "outlook" 信任目录
        /// <summary>
        /// 过滤文件
        /// </summary>
        public List<string[]> FiltrationFile = new List<string[]> { };//{"*.doc","2m"} 文件过滤
        private string Expath = AppDomain.CurrentDomain.BaseDirectory;
        /// <summary>
        /// 添加过滤的目录
        /// </summary>
        /// <param name="path"></param>
        public void AddExcludeDir(string path)
        {
            GetExcludeDir();           
            string e = path.Substring(path.LastIndexOf('\\') + 1, path.Length- (path.LastIndexOf('\\') + 1));
            Console.WriteLine(e);
            if (!ExcludeDir.Contains(e)) {
                    var path1 = Expath + "ExcludeDir.ini";
                    FileStream File = new FileStream(path1, FileMode.Append, FileAccess.Write);
                    StreamWriter Sfile = new StreamWriter(File, Encoding.GetEncoding("gb2312"));  
                    Sfile.WriteLine(e);
                    Sfile.Close();
                    File.Close();              
                    //添加到过滤文件中
                    Console.WriteLine("已成功添加到排除目录中!");                
            }
         }
        /// <summary>
        /// 添加信任的目录
        /// </summary>
        /// <param name="path"></param>
        public void AddTrustDir(string path)
        {
            GetExcludeDir();
            string e = path.Substring(path.LastIndexOf('\\') + 1, path.Length - (path.LastIndexOf('\\') + 1));
            Console.WriteLine(e);
            if (!ExcludeDir.Contains(e))
            {
                var path1 = Expath + "TrustDir.ini";
                FileStream File = new FileStream(path1, FileMode.Append, FileAccess.Write);
                StreamWriter Sfile = new StreamWriter(File, Encoding.GetEncoding("gb2312"));
                Sfile.WriteLine(e);
                Sfile.Close();
                File.Close();
                //添加到过滤文件中
                Console.WriteLine("已成功添加到信任目录中!");
            }
        }

        public void GetTrustDir()
        {
            //读取配置文件返回过滤目录列表
            try
            {
                var path1 = Expath + "TrustDir.ini";
                //  if (File.Exists(path1))
                // {
                using (FileStream File = new FileStream(path1, FileMode.OpenOrCreate, FileAccess.Read))
                {
                    using (StreamReader Sfile = new StreamReader(File, Encoding.GetEncoding("gb2312")))
                    {
                        string s;
                        while ((s = Sfile.ReadLine()) != null)
                        {
                            TrustDir.Add(s.Trim());
                        }
                        Sfile.Close();
                    }
                    File.Close();
                }
                // }
            }
            catch
            {

            }

        }

        public void GetExcludeDir()
        {
            //读取配置文件返回过滤目录列表
           try
            {
               var path1 = Expath + "ExcludeDir.ini";
                //  if (File.Exists(path1))
                // {
                using (FileStream File = new FileStream(path1, FileMode.OpenOrCreate, FileAccess.Read))
                {
                    using (StreamReader Sfile = new StreamReader(File, Encoding.GetEncoding("gb2312")))
                    {
                        string s;
                        while ((s = Sfile.ReadLine()) != null)
                        {
                            ExcludeDir.Add(s.Trim());
                        }
                        Sfile.Close();
                    }
                    File.Close();
                }
               // }
            }
            catch
            {

            }
        
        }
        public void GetExcludeFile()
        {
            //读取配置文件返回过滤文件列表
            try
            {
                var path2 = Expath + "FiltrationFile.ini";
                //  if (File.Exists(path2))
                // {
                using (FileStream File = new FileStream(path2, FileMode.OpenOrCreate, FileAccess.Read))
                {
                    using (StreamReader Sfile = new StreamReader(File, Encoding.GetEncoding("gb2312")))
                    { 
                        string s;
                        while ((s = Sfile.ReadLine()) != null)
                        {
                            string[] fie = s.Split('|');

                            if (fie.Length >= 2)
                            {
                                long zist = long.Parse(System.Text.RegularExpressions.Regex.Replace(fie[1], @"[^0-9]+", ""));
                                if (fie[1].IndexOf("G", StringComparison.OrdinalIgnoreCase) >= 0)
                                    fie[1] = (zist * 1024 * 1024 * 1024).ToString();
                                if (fie[1].IndexOf("M", StringComparison.OrdinalIgnoreCase) >= 0)
                                    fie[1] = (zist * 1024 * 1024).ToString();
                                if (fie[1].IndexOf("kb", StringComparison.OrdinalIgnoreCase) >= 0)
                                    fie[1] = (zist * 1024).ToString();
                            }


                            FiltrationFile.Add(fie);
                        }
                        Sfile.Close();
                       // ExcludeFile.Add();
                    }
                    File.Close();
               }
            }
            catch
            {

            }

        }
        /// <summary>
        /// 目录是否包含在排除目录中
        /// </summary>
        /// <param name="path">目录</param>
        /// <param name="layernum">排除根目录下最前几层的目录,0表示全目录</param>
        /// <returns>
        /// 目录数,即如目录\\192.168.1.192\up\\dfe\xc\software,
        /// 排除包含software的目录如是目录数为6则排除,如果为1则不排除 1表示根目录下面的目录
        /// </returns>
        public bool ExDirInclude (string path,int layernum)
        {
            if (layernum > 0)
            {
                layernum += 1;
                var pathnew = path.Replace("\\\\", "\\");//共享目录
                var layer = path.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                if (layer.Length > layernum)
                {
                    //index为过滤起始位
                    int index = path.Length - pathnew.Length;//计算共享目录与本地目录的不同
                    for (var je = 0; je <= layernum; je++)
                    {
                        if (je > 1)
                            index += 2;
                        index += layer[je].Length;
                    }

                    path = path.Substring(0, index);
                }
            }
            foreach (var e in ExcludeDir)
            {
                if (path.IndexOf(e) >= 0)
                    return true;
            }
            return false;

            //foreach (var e in ExcludeDir)
            //{
            //    if (path.IndexOf(e)>=0)
            //     return true;
            //}
            //return false;
        }

        public bool ExDirTrust (string path)
        {
           // Console.WriteLine("test1" + path);
            foreach (var e in TrustDir)
            {
                
                if (path.IndexOf(e) >= 0)
                {
                //    Console.WriteLine("test" + e);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 是否符合过滤
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
        public bool ExFileInclude (string filepath)
        {
            //取得文件扩展名
            string FileExpanded = filepath.Substring(filepath.LastIndexOf(".") + 1, filepath.Length - (filepath.LastIndexOf(".") + 1));

            foreach (var e in FiltrationFile)
            {
                //如果包含
                if (e[0].IndexOf(FileExpanded,StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    //如果有限制大小的话
                    if (e.Length > 1)
                    {
                        //计算大小
                        FileInfo f = new FileInfo(filepath);
                        if (f.Length > long.Parse(e[1]))
                        {
                            //符合扩展但超过大小
                            return false;
                        }

                    }
                    //符合扩展,无大小过滤
                    return true;
                }
                   
            }
            //不符合扩展
            return false;
        }


    }

}
