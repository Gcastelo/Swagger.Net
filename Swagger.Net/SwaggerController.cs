﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json.Linq;

namespace Swagger.Net
{
    public class SwaggerController : ApiController
    {
        /// <summary>
        /// Get the resource description of the api for swagger documentation
        /// </summary>
        /// <remarks>It is very convenient to have this information available for generating clients
        /// </remarks>
        /// <returns>JSON document representing structure of API</returns>
        public ResourceListing Get()
        {
            var docProvider = (XmlCommentDocumentationProvider)GlobalConfiguration.Configuration.Services.GetDocumentationProvider();

            ResourceListing r = SwaggerGen.CreateResourceListing(ControllerContext);
            List<string> uniqueControllers = new List<string>();

            foreach (var api in GlobalConfiguration.Configuration.Services.GetApiExplorer().ApiDescriptions)
            {
                string controllerName = api.ActionDescriptor.ControllerDescriptor.ControllerName;
                if (uniqueControllers.Contains(controllerName) ||
                      controllerName.ToUpper().Equals(SwaggerGen.SWAGGER.ToUpper())) continue;

                uniqueControllers.Add(controllerName);

                ResourceApi rApi = SwaggerGen.CreateResourceApi(api);
                r.apis.Add(rApi);
            }
            
            return r;
        }
    }
}