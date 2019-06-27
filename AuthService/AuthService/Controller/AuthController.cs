using Grapevine.Interfaces.Server;
using Grapevine.Server;
using Grapevine.Server.Attributes;
using Grapevine.Shared;
using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AuthService
{
    [RestResource]
    class AuthController
    {
        MainCore _ = new MainCore("mongodb://51.83.73.69:27017", "AuthService");

        #region POST

        [RestRoute(HttpMethod = HttpMethod.POST, PathInfo = "/api/auth/add")]
        public IHttpContext AddContacto(IHttpContext context)
        {

            string jsonRAW = context.Request.Payload;
            dynamic dataId = JsonConvert.DeserializeObject<object>(jsonRAW);

            MAuth data = new MAuth();

            data.typeService = dataId?.typeService.Value;
            data.serviceID = new Regex(@"^[0-9a-fA-F]{24}$").Match(dataId?.serviceID.ToString()).Success == true ? ObjectId.Parse(dataId?.serviceID.ToString()) : null;

            data.awollAccess = true;
            data.rolAccess = dataId?.rolAccess.Value;
            data.openAccess = dataId?.openAccess;

            data.closeAccess = dataId?.closeAccess;

            data.user = dataId?.user.Value;
            data.pass = dataId?.pass.Value;

            _.Create(data);

            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            context.Response.AppendHeader("Content-Type", "application/json");
            context.Response.SendResponse(json);
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.POST, PathInfo = "/api/auth/login")]
        public IHttpContext Login(IHttpContext context)
        {

            string jsonRAW = context.Request.Payload;
            dynamic dataId = JsonConvert.DeserializeObject<object>(jsonRAW);

            var user = dataId?.user;
            var data = _.Login(dataId?.user.Value, dataId.pass.Value);
         
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            context.Response.AppendHeader("Content-Type", "application/json");
            context.Response.SendResponse(json);
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.POST, PathInfo = "/api/auth/logout")]
        public IHttpContext Logout(IHttpContext context)
        {

            string jsonRAW = context.Request.Payload;
            dynamic dataId = JsonConvert.DeserializeObject<object>(jsonRAW);

            var data = _.Logout(dataId?.token.Value);

            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            context.Response.AppendHeader("Content-Type", "application/json");
            context.Response.SendResponse(json);
            return context;
        }


        [RestRoute(HttpMethod = HttpMethod.POST, PathInfo = "/api/auth/status")]
        public IHttpContext Status(IHttpContext context)
        {

            string jsonRAW = context.Request.Payload;
            dynamic dataId = JsonConvert.DeserializeObject<object>(jsonRAW);

            var data = _.Status(dataId?.token.Value);

            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            context.Response.AppendHeader("Content-Type", "application/json");
            context.Response.SendResponse(json);
            return context;
        }

        #endregion
    }
}
