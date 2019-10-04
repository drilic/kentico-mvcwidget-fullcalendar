namespace EXLRT.Kentico.Mvc.Widgets.FullCalendar
{
    using System;
    using System.Globalization;
    using System.Web.Mvc;
    using System.Web.Routing;

    public static class FullCalendarRoutes
    {
        public static void MapFullCalendarRoutes(this RouteCollection routes, MvcRouteHandler routeHandler = null, object constraint = null, CultureInfo defaultCulture = null)
        {
            Route route = null;

            if (!FullCalendarConfiguration.IsMultiCultureSite)
            {
                route = routes.MapRoute(
                   name: "FullCalendarApi",
                   url: "fullcalendarapi/getcalendardata/{widgetType}",
                   defaults: new { controller = "FullCalendarApi", action = "GetCalendarData" },
                   constraints: constraint
               );
            }
            else
            {
                if (defaultCulture == null)
                {
                    throw new Exception("Default culture can't be null for multilingual configuration. Please check FullCalendar routes intialization.");
                }

                route = routes.MapRoute(
                    name: "FullCalendarApi",
                    url: "{culture}/fullcalendarapi/getcalendardata/{widgetType}",
                    defaults: new { culture = defaultCulture.Name, controller = "FullCalendarApi", action = "GetCalendarData" },
                    constraints: constraint
                );
            }

            if (routeHandler != null)
            {
                route.RouteHandler = routeHandler;
            }
        }
    }
}
