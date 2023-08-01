using System.Collections.Generic;

namespace EXLRT.Kentico.Mvc.Widgets.FullCalendar
{
    public class UrlConfiguration
    {
        /// <summary>
        /// Available only for Pages Calendars
        /// </summary>
        public bool UseDefaultKenticoRetriever { get; set; } = false;

        public IEnumerable<string> Columns { get; set; }

        public string Pattern { get; set; }
    }
}
