using InventoryManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Models
{
    [NotMapped]
    public class JWTManagerRepository
    {
        private readonly InventoryManagementContext _db;
        private readonly IConfiguration iconfiguration;
        public JWTManagerRepository(IConfiguration iconfiguration, InventoryManagementContext db)
        {
            this.iconfiguration = iconfiguration;
            _db = db;
        }
        public string Authenticate(Management management)
        {
            if (!_db.Management.Any(x => x.EmployEmailId == management.EmployEmailId && x.Password == Encrypt_Password(management.Password) && x.Role == management.Role))
            {
                return null;
            }

            var obj = _db.Management.Where(c => c.EmployEmailId == management.EmployEmailId && c.Password == Encrypt_Password(management.Password));
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(iconfiguration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                      new Claim(ClaimTypes.Role, management.Role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(45),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var MainToken = tokenHandler.WriteToken(token);
            return MainToken;
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public string Encrypt_Password(string password)
        {
            string key = "1prt56";
            byte[] SrctArray;
            byte[] EnctArray = UTF8Encoding.UTF8.GetBytes(password);
            SrctArray = UTF8Encoding.UTF8.GetBytes(key);
            TripleDESCryptoServiceProvider objt = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider objcrpt = new MD5CryptoServiceProvider();
            SrctArray = objcrpt.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            objcrpt.Clear();
            objt.Key = SrctArray;
            objt.Mode = CipherMode.ECB;
            objt.Padding = PaddingMode.PKCS7;
            ICryptoTransform crptotrns = objt.CreateEncryptor();
            byte[] resArray = crptotrns.TransformFinalBlock(EnctArray, 0, EnctArray.Length);
            objt.Clear();
            return Convert.ToBase64String(resArray, 0, resArray.Length);
        }
    }
}
