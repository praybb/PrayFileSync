using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrayFileSync;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Console.WriteLine("源文件夹存在,正开始复制");
            exclude exc1 = new exclude();
            FileCopy fileCopy = new FileCopy();
            fileCopy.SrcDir = @"\\192.168.1.217\d$\53207325e0d39fe3a7cdbcd7";
            fileCopy.DstDir = @"D:\test";
            fileCopy.getfile(exc1);
            Console.WriteLine("已复制完成");
            // RegHelper.SetRegistryValue("HKEY_CLASSES_ROOT", @"Folder\shell\SyncExcludeDir", "Icon", "REG_SZ", "D:\\我的工具\\bak\\Exclude.ico");
        }
    }
}
