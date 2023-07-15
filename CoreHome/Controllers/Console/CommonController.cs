
using Common.Emu;
using Common.Result;
using CoreHome.Filters;
using CoreHome.Utils;
using Data.Db;
using Data.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;


namespace CoreHome.Controllers.Console
{

    //[EnableCors("Policy1")] //允许跨域
    //[TypeFilter(typeof(ConsoleFilter))]
    [Route("[controller]/[action]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly ILogger<CommonController> _logger;
        private readonly BlogDbContext _blogDbContext;
        private readonly IWebHostEnvironment environment;
        /// <summary>
        /// 域名
        /// </summary>
        private readonly string ym;
   

        private readonly string filePath;

        public CommonController(ILogger<CommonController> logger, BlogDbContext blogDbContext, IWebHostEnvironment environment, IConfiguration configuration)
        {
            _logger = logger;
            _blogDbContext = blogDbContext;
            this.environment = environment;
            this.filePath = Path.Combine(environment.ContentRootPath, "MyStaticFiles");
            this.ym = configuration["ym"];


        }
        [TypeFilter(typeof(ConsoleFilter))]
        [HttpPost]
        public R UpFile(IFormFile file)
        {
            try
            {
                FileInfom fileInfom = new FileInfom();
                fileInfom.local = true;
                fileInfom.CreatedTime = DateTime.Now;
                fileInfom.IsDelete = false;
                fileInfom.Type = Convert.ToInt32(FileUtil.GetFileType(file).Value);
                //获得到文件名
                fileInfom.FileName = System.IO.Path.GetFileName(file.FileName.ToString());
                //获得文件扩展名
                fileInfom.Suffix = System.IO.Path.GetExtension(fileInfom.FileName);

                //设置文件上传路径 默认当前磁盘下 FileUpload服务器映射文件夹
                string fileHead = FileUtil.CreatedDirPath(null);
                string fullFileName = fileHead + Path.DirectorySeparatorChar + Path.GetFileName(file.FileName);

                fileInfom.url = fullFileName.Replace("\\", "/");

                FileUtil.SaveFileLocal(filePath, fullFileName, file);

                fileInfom.size = file.Length;
                // 计算文件大小
                /* var math = Math.DivRem(file.Length, 1024 * 1024);
                 string y = math.Remainder.ToString().Length > 2 ? math.Remainder.ToString().Substring(0, 1) : math.Remainder.ToString();
                 string size = math.Quotient.ToString() + "." + y;*/

                //string baseUrl = ("https://" + Request.HttpContext.Connection.LocalIpAddress.MapToIPv4().ToString() + ":" + Request.HttpContext.Connection.LocalPort); 
                //string baseUrl = ("https://localhost:" + Request.HttpContext.Connection.LocalPort) + fileInfom.url;
                string baseUrl = ym + fileInfom.url;
                _logger.LogInformation(ym + fileInfom.url);

                _blogDbContext.FileInfos.Add(fileInfom);
                _blogDbContext.SaveChanges();

                return R.Ok(baseUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError("文件保存失败：" + ex.Message);
                return R.Fail("文件保存失败");
            }
            

        }

        [TypeFilter(typeof(ConsoleFilter))]
        [HttpGet]
        public R GetList(string? name, int current = 1, int pageSize = 6)
        {
            int v = PageUtils<FileInfom>.GetPage(current, pageSize);

            var query = _blogDbContext.FileInfos.Where(f => f.IsDelete == false);
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(f => f.FileName.StartsWith(name));
            }
            query.OrderByDescending(f => f.CreatedTime);
            var couant = query.Count();
            var p = new PageUtils<FileInfom>(query.Skip(v).Take(pageSize).ToList(), pageSize, current, couant);
            return R.Ok(p);    
        }

        [TypeFilter(typeof(ConsoleFilter))]     
        [HttpGet]
        public R Delete(int id)
        {
            FileInfom friend = _blogDbContext.FileInfos.Where(f => f.Id == id).First();
            if (friend != null)
            {
                friend.IsDelete = true;
                _blogDbContext.SaveChanges();
            }
            return R.Ok();
        }

        [TypeFilter(typeof(ConsoleFilter))]
        [HttpGet]
        public R DelByName(string name)
        {
            FileInfom friend = _blogDbContext.FileInfos.Where(f => f.FileName == name).First();
            if (friend != null)
            {
                friend.IsDelete = true;
                _blogDbContext.SaveChanges();
            }
            if (friend.local)
            {
                string path = filePath + friend.url;
                System.IO.File.Delete(path);
            }

            return R.Ok();
        }

        [TypeFilter(typeof(ConsoleFilter))]
        [HttpPost]
        public object UploadFile()
        {
     
            #region Api处理模块
            try
            {
                //获取上传文件
                IFormFileCollection files = HttpContext.Request.Form.Files;
                //判断是否有文件上传
                if (files == null ||files.Count == 0)
                {
                    return new
                    {
                        errno = 1, // 注意：值是数字，不能是字符串
                        message = "未上传文件！！！"
                    };
                }
                string url = string.Empty;
                for (int i = 0; i < files.Count; i++)
                {
                    //设置文件上传路径 默认当前磁盘下 FileUpload服务器映射文件夹
                    string fileHead = FileUtil.CreatedDirPath(null);
                    string fullFileName =  fileHead + Path.DirectorySeparatorChar + Path.GetFileName(files[i].FileName);

                    
                    ////创建文件夹，保存文件
                    string path = Path.GetDirectoryName(filePath + fullFileName);
                    if(i == 0)
                    {
                        url = fullFileName;
                    }
                    fullFileName =  filePath + fullFileName;
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
                    using (Stream stream = files[i].OpenReadStream())
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
                    _logger.LogInformation("保存文件：{}", fullFileName);                  
                }

                //string baseUrl = ("https://" + Request.HttpContext.Connection.LocalIpAddress.MapToIPv4().ToString() + ":" + Request.HttpContext.Connection.LocalPort); 
                string baseUrl = ("https://localhost:" + Request.HttpContext.Connection.LocalPort); 
                return new
                {
                    errno = 0, // 注意：值是数字，不能是字符串
                    data = new
                    {
                        url = ym + @url.Replace("\\", "/"), // 图片 src ，必须
                        alt = @url, // 图片描述文字，非必须
                        href = @url // 图片的链接，非必须
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("图片上传失败：{}", ex);
                return new
                {
                    errno = 1, // 注意：值是数字，不能是字符串
                    message = "上传失败：" + ex.Message
                };
            }        
            
            #endregion

        }


    }
}
