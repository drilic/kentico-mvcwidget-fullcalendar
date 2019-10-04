namespace EXLRT.Kentico.Mvc.Widgets.FullCalendar.Repositories
{
    using Kentico.Mvc.Widgets.FullCalendar.Models.FullCalendar;
    using Kentico.Mvc.Widgets.FullCalendar.Models.Widgets.FullCalendarWidget;
    using System.Collections.Generic;

    public interface IFullCalendarEventsRepository
    {
        IEnumerable<FullCalendarEvent> GetPageTypeEvents(FullCalendarWidgetConfiguration fullCalendarWidgetConfiguration);

        IEnumerable<FullCalendarEvent> GetCustomTableEvents(FullCalendarWidgetConfiguration fullCalendarWidgetConfiguration);

        IEnumerable<FullCalendarEvent> GetModuleClassEvents(FullCalendarWidgetConfiguration fullCalendarWidgetConfiguration);
    }
}
