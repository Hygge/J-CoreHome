using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CoreHome.Utils
{
    public class JwtHelper
    {
        private readonly IConfiguration _configuration;

        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 创建token
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public string CreateToken(string Name, int id)
        {
            // 1. 定义需要使用到的Claims
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, "u_admin"), //HttpContext.User.Identity.Name       
            new Claim("Id", id.ToString()),
            new Claim("Name", Name)
        };

            // 2. 从 appsettings.json 中读取SecretKey
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

            // 3. 选择加密算法
            var algorithm = SecurityAlgorithms.HmacSha256;

            // 4. 生成Credentials
            var signingCredentials = new SigningCredentials(secretKey, algorithm);

            // 5. 根据以上，生成token
            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],     //Issuer
                _configuration["Jwt:Audience"],   //Audience
                claims,                          //Claims,
                DateTime.Now,                    //notBefore
                DateTime.Now.AddSeconds(Convert.ToInt32(_configuration["Jwt:Expire"])),    //expires
                signingCredentials               //Credentials
            );

            // 6. 将token变为string
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return token;
        }

        /// <summary>
        /// 校验解析是否token或是否过期
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool IsToken(string token)
        {
            try
            {
                JwtSecurityToken jwt = new JwtSecurityToken(token);   
                
                var jwtHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken securityToken = jwtHandler.ReadJwtToken(token);
                // jwt.Payload[JwtRegisteredClaimNames.Exp]

                return true;
            }
            catch (Exception ex)
            {
                return false;
            } 
     
        }

    }
}
