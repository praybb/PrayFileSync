using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace PrayFileSync
{
    public class FileCopy
    {
        /// <summary>
        /// 目标文件夹(绝对路径)
        /// </summary>
        public string DstDir { set; get; }
        /// <summary>
        /// 源文件夹(相对路径)
        /// </summary>
        public string SrcDir { set; get; }
        public void getfile(exclude Exclass)
        {
            getfile(SrcDir, Exclass);
        }

        /// <summary>
        /// 复制全部文件
        /// </summary>
        /// <param name="path"></param>
        public void dircopy(string path)
        {
            try
            {
                string[] dlist = Directory.GetDirectories(path); ;
                if (dlist != null)
                {
                    for (int fileindex = 0; fileindex < dlist.Length; fileindex++)
                    {
                        dircopy(dlist[fileindex]);
                    }
                }
                //Console.WriteLine("trun");
                string[] list = Directory.GetFiles(path);
                if (list != null)
                {
                    foreach (var f in list)
                    {
                        fileCopy(f);
                    }
                }
            }
            catch (System.UnauthorizedAccessException)
            {
                Console.WriteLine("目录文件夹访问被拒绝");
            }
        }
        /// <summary>
        /// 获取路径上的文件名
        /// </summary>
        /// <param name="path"></param>
        private void getfile(string path, exclude ExClass)
        {
            if (ExClass.ExDirTrust(path))
            {

                dircopy(path);
            }
            else
            {

                if (!ExClass.ExDirInclude(path, 0))
                {
                    try
                    {
                        string[] dlist = Directory.GetDirectories(path); ;
                        if (dlist != null)
                        {
                            for (int fileindex = 0; fileindex < dlist.Length; fileindex++)
                            {
                                getfile(dlist[fileindex], ExClass);
                            }
                        }
                        //Console.WriteLine("trun");
                        string[] list = Directory.GetFiles(path);
                        if (list != null)
                        {
                            foreach (var f in list)
                            {
                                if (ExClass.ExFileInclude(f))
                                    fileCopy(f);
                            }
                        }
                    }
                    catch (System.UnauthorizedAccessException)
                    {
                        Console.WriteLine("目录文件夹访问被拒绝");
                    }
                }
                else
                {
                    Console.WriteLine("目录已排除" + path);
                }
            }

        }
        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="filePath"></param>
        public void fileCopy(string filePath)
        {

            if (SrcDir != null && DstDir != null)
            {
                //取目标文件名
                string relativeFile = filePath.Substring(SrcDir.Length);
                string relativeDir = relativeFile.Substring(0, relativeFile.LastIndexOf("\\"));
                Directory.CreateDirectory(DstDir + relativeDir);
                //如果目标存在文件,且修改时间相同则不复制
                if (!(File.Exists(DstDir + relativeFile) && File.GetLastWriteTime(DstDir + relativeFile) == File.GetLastWriteTime(filePath)))
                {

                    File.Copy(filePath, DstDir + relativeFile, true);
                }
                Console.WriteLine("已复制:" + filePath);
            }
        }


    }

}

