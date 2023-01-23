namespace EXLRT.Kentico.Mvc.Widgets.FullCalendar.Controllers
{
    using global::Kentico.Content.Web.Mvc;
    using Kentico.Mvc.Widgets.FullCalendar;
    using Kentico.Mvc.Widgets.FullCalendar.Models.FullCalendar;
    using Kentico.Mvc.Widgets.FullCalendar.Models.Widgets.FullCalendarWidget;
    using Kentico.Mvc.Widgets.FullCalendar.Repositories;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class FullCalendarApiController : Controller
    {
        public IFullCalendarEventsRepository FullCalendarEventsRepository { get; }
        private readonly IPageUrlRetriever pageUrlRetriever;

        public FullCalendarApiController(IPageUrlRetriever pageUrlRetriever, IFullCalendarEventsRepository fullCalendarEventsRepository)
        {
            this.FullCalendarEventsRepository = fullCalendarEventsRepository;
            this.pageUrlRetriever = pageUrlRetriever;
        }

        public ActionResult GetCalendarData(string widgetType)
        {
            List<FullCalendarEvent> data = new List<FullCalendarEvent>();
            FullCalendarWidgetConfiguration widgetConfiguration = FullCalendarConfiguration.GetWidgetConfiguration(widgetType);

            if (widgetConfiguration != null)
            {
                switch (widgetConfiguration.ConfigurationType)
                {
                    case FullCalendarConfigurationType.Pages:
                        data = this.FullCalendarEventsRepository.GetPageTypeEvents(widgetConfiguration).ToList();

                        if (pageUrlRetriever != null)
                        {
                            foreach (var d in data)
                            {
                                d.Url = pageUrlRetriever.Retrieve(d.Url).RelativePath.Replace("~", "");
                            }
                        }
                        break;

                    case FullCalendarConfigurationType.CustomTables:
                        data = this.FullCalendarEventsRepository.GetCustomTableEvents(widgetConfiguration).ToList();
                        break;

                    case FullCalendarConfigurationType.Classes:
                        data = this.FullCalendarEventsRepository.GetModuleClassEvents(widgetConfiguration).ToList();
                        break;
                }
            }

            return Json(JsonConvert.SerializeObject(data, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            }), JsonRequestBehavior.AllowGet);
        }
    }
}
