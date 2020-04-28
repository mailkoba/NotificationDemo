using System;
using Microsoft.AspNetCore.Mvc;

namespace NotificationDemo.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class NoCacheAttribute : ResponseCacheAttribute
    {
        public NoCacheAttribute()
        {
            NoStore = true;
            Location = ResponseCacheLocation.None;
        }
    }
}
