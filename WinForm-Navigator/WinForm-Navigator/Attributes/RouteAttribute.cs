using System;

namespace Navigators.Attributes
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =true)]
    public class RouteAttribute:Attribute
    {
        public string RoutePath { get; set; }
        public string RouteName { get; set; }

        public RouteAttribute(string routePath)
        {
            RoutePath = routePath;
        }

        public RouteAttribute(string routePath, string routeName)
        {
            RoutePath = routePath;
            RouteName = routeName;
        }
    }
}
