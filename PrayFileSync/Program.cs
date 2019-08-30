
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace PrayFileSync
{
    class Program
    {

        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                switch (args[0].ToLower())
                {
                    //说明解释
                    case "/?":
                    case "-?":
                        Console.WriteLine("/addtrust [信任目录]  \r\n  解释:生成规则添加信任目录,复制此目录下的所有文件,过滤文件规则不生效 \r\n");
                        Console.WriteLine("/addexclude [排除目录]  \r\n  解释:生成规则添加排除目录,不复制目录下的所有文件 \r\n");
                        break;
                    //安装至
                    case "/install":
                    case "-install":
                        //安装
                       // RegHelper.SetRegistryValue("HKEY_CLASSES_ROOT", @"Folder\shell\SyncExcludeDir", "Icon", "REG_SZ", "D:\\我的工具\\bak\\Exclude.ico");
                        break;
                    default:
                        break;
                }
            }

            if (args.Length >= 2)
            {
                exclude exc1 = new exclude();
                switch (args[0].ToLower())
                {
                    //说明解释
                    case "/?":
                    case "-?":
                        Console.WriteLine("/addtrust [信任目录]  \r\n  解释:生成规则添加信任目录,复制此目录下的所有文件,过滤文件规则不生效 \r\n");
                        Console.WriteLine("/addexclude [排除目录]  \r\n  解释:生成规则添加排除目录,不复制目录下的所有文件 \r\n");
                        break;
                     //添加信任
                    case "/addtrust":
                    case "-addtrust":
                       
                        exc1.AddTrustDir(args[1]);
                        break;
                    //添加到排除目录
                    case "/addexclude":
                    case "-addexclude":                   
                        exc1.AddExcludeDir(args[1]);
                        break;
                    default:
                        #region 复制文件
                        //过滤类
                        if (Directory.Exists(args[0]))
                        {
                            Console.WriteLine("源文件夹存在,正开始复制");
                            //exclude exc1 = new exclude();
                            FileCopy fileCopy = new FileCopy();
                            fileCopy.SrcDir = args[0];
                            fileCopy.DstDir = args[1];
                            fileCopy.getfile(exc1);
                            Console.WriteLine("已复制完成");
                        }
                        else
                        {
                            Console.WriteLine("源文件夹不存在,或无法访问");
                        }
                        #endregion
                        break;
                }    
            }



        }
       
    }
}
