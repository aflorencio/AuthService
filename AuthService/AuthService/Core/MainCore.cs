using Microsoft.IdentityModel.Tokens;
using Mongo.CRUD;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

using DMModel = AuthService.MAuth;


namespace AuthService
{
    class MainCore
    {
        static string key = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed0";
        public IMongoCRUD<DMModel> db;
        public MainCore(string server, string database)
        {
            try
            {
                db = new MongoCRUD<DMModel>(server, database);
            }
            catch (Exception e)
            {
                var algo = e;

            }
        }

        public void Create(DMModel data)
        {
            db.Create(data);
        }


        private static string GenerateToken(string id)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var secToken = new JwtSecurityToken(
                signingCredentials: credentials,
                issuer: "Sample",
                audience: "Sample",
                claims: new[]
                {
                new Claim(JwtRegisteredClaimNames.NameId, id),
                },
                expires: DateTime.UtcNow.AddDays(1));

            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(secToken);
        }

        private static bool ValidateToken(string authToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();

            SecurityToken validatedToken;
            try
            {
                IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private static TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = false, // Because there is no expiration in the generated token
                ValidateAudience = false, // Because there is no audiance in the generated token
                ValidateIssuer = false,   // Because there is no issuer in the generated token
                ValidIssuer = "Sample",
                ValidAudience = "Sample",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)) // The same key as the one that generate the token
            };
        }

        private static JwtPayload ReadToken(string tokenString)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenString);

            return token.Payload;

        }

        public string Login(string user, string pass) {
            //Buscamos si existe en Auth el usuario y la pass
            BsonDocument filter = new BsonDocument();
            filter.Add("user", user);
            filter.Add("$and", new BsonArray()
                    .Add(new BsonDocument()
                            .Add("pass", pass)
                    )
            );
            var data = db.Search(filter).Documents.FirstOrDefault();

            //comprobamos si tiene permitido el inicio de sesion.

            if (data.awollAccess == true)
            {
                data.tokenActivo = GenerateToken(data._id.ToString());
                data.cadTokenTime = DateTime.Now.AddDays(1);
                data.ip = "0.0.0.0";
                data.loginDate = DateTime.Now;
                db.Update(data);
                return data.tokenActivo;
            }
            else {
                return "err.err.err";
            }

        }
        public bool Logout(string token) {
            if (ValidateToken(token) == true)
            {
                var dataToken = ReadToken(token).ToList();
                var id = dataToken.ElementAt(0).Value.ToString();
                var dbTokenInfo = db.Get(ObjectId.Parse(id));

                if (dbTokenInfo.cadTokenTime > DateTime.Now)
                {
                    dbTokenInfo.cadTokenTime = DateTime.Parse("2000/01/01");
                    db.Update(dbTokenInfo);
                    return true;
                }

            }
            return false;
        }

        public bool Status(string token) {

            if (ValidateToken(token) == true)
            {
                var dataToken = ReadToken(token).ToList();
                var id = dataToken.ElementAt(0).Value.ToString();
                var dbTokenInfo = db.Get(ObjectId.Parse(id));

                if (dbTokenInfo.awollAccess == true) {
                    if (dbTokenInfo.cadTokenTime > DateTime.Now) {
                        var hora = Convert.ToInt16(DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString());
                        if (hora >= dbTokenInfo.openAccess && hora <= dbTokenInfo.closeAccess)
                        {
                            return true;
                        }
                    }
                }

            }
            
            return false;
        }


    }
}
