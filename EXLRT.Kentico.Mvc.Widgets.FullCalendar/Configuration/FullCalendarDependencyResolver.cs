namespace EXLRT.Kentico.Mvc.Widgets.FullCalendar
{
    using Autofac;
    using global::Kentico.Content.Web.Mvc;
    using global::Kentico.PageBuilder.Web.Mvc;
    using global::Kentico.Web.Mvc;
    using Kentico.Mvc.Widgets.FullCalendar.Controllers;
    using Kentico.Mvc.Widgets.FullCalendar.Repositories;
    using System.Globalization;
    using System.Web;

    public static class FullCalendarDependencyResolver
    {
        public static void RegisterFullCalendarDependencies(this ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(FullCalendarConfiguration).Assembly)
                .Where(x => x.IsClass && !x.IsAbstract && typeof(IFullCalendarEventsRepository).IsAssignableFrom(x))
                .AsImplementedInterfaces()
                .WithParameter((parameter, context) => parameter.Name == "cultureName", (parameter, context) => CultureInfo.CurrentUICulture.Name)
                .WithParameter((parameter, context) => parameter.Name == "latestVersionEnabled", (parameter, context) => HttpContext.Current.Kentico().PageBuilder().EditMode)
                .InstancePerRequest();

            builder.Register(context => new FullCalendarApiController(context.Resolve<IPageUrlRetriever>(), context.Resolve<IFullCalendarEventsRepository>())).InstancePerRequest();
        }
    }
}
