using Common.Emu;
using System.Security.Policy;

namespace CoreHome.Utils
{
    /// <summary>
    /// 文件工具类
    /// </summary>
    public class FileUtil
    {
        static string[] img = { ".jpg", ".jpeg", ".png", ".xbm", ".tif", ".pjp", ".svgz", ".jpg",
            ".jpeg", ".ico", ".tiff", ".gif", ".svg", ".jfif", ".webp", ".png", ".bmp", ".pjpeg", ".avif" };
        static string[] txt = { ".txt", ".text",  };



        /// <summary>
        /// 创建文件夹路径默认 FileUpload文件夹路径下
        /// </summary>
        /// <returns></returns>
        public static string CreatedDirPath(string? basePath)
        {
            if (basePath == null)
            {
                return $"{Path.DirectorySeparatorChar}FileUpload{Path.DirectorySeparatorChar}{DateTime.Now:yyyy-MM-dd}";
            }
            return $"{Path.DirectorySeparatorChar}{basePath}{Path.DirectorySeparatorChar}{DateTime.Now:yyyy-MM-dd}";
        }

        /// <summary>
        /// 根据后缀名获取文件类型
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static FileType? GetFileType(IFormFile file)
        {    
            //获得到文件名
            string fileName = System.IO.Path.GetFileName(file.FileName.ToString());
            //获得文件扩展名
            string fileNameEx = System.IO.Path.GetExtension(fileName);
            //没有扩展名的文件名
            string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(fileName);
            
            bool isFile = false;
            for (int i = 0; i < img.Length; i++)
            {
                if (fileNameEx.Equals(img[i]))
                {
                    isFile = true;
                    break;
                }
            }
            if (isFile)
            {
                return FileType.Img;
            }
            for (int i = 0; i < txt.Length; i++)
            {
                if (fileNameEx.Equals(txt[i]))
                {
                    isFile = true;
                    break;
                }
            }
            if (isFile)
            {
                return FileType.Txt;
            }
            return FileType.Other;
        }

        public static void SaveFileLocal(string filePath, string fullFileName, IFormFile file)
        {
            ////创建文件夹，保存文件
            string path = Path.GetDirectoryName(filePath + fullFileName);
     
            fullFileName = filePath + fullFileName;
            #region 检查上传的物理路径是否存在，不存在则创建
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            #endregion
            //保存文件  文件存在则先删除原来的文件
            if (System.IO.File.Exists(fullFileName))
            {
                System.IO.File.Delete(fullFileName);
            }
            //将流写入文件
            using (Stream stream = file.OpenReadStream())
            {
                // 把 Stream 转换成 byte[]
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                // 设置当前流的位置为流的开始
                stream.Seek(0, SeekOrigin.Begin);
                // 把 byte[] 写入文件
                FileStream fs = new FileStream(fullFileName, FileMode.Create, FileAccess.Write);
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(bytes);
                bw.Close();
                fs.Close();
            }
        }

    }
}
