namespace EXLRT.Kentico.Mvc.Widgets.FullCalendar.Models.Widgets.FullCalendarWidget
{
    using Kentico.Mvc.Widgets.FullCalendar.Models.FullCalendar;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class FullCalendarWidgetConfiguration
    {
        public string WidgetTypeDisplayName { get; set; }

        public string SiteCodeName { get; set; }

        public FullCalendarConfigurationType ConfigurationType { get; set; }

        public string ClassOrObjectName { get; set; }

        public string TitleColumnName { get; set; }

        public string StartDateColumnName { get; set; }

        public string EndDateColumnName { get; set; }

        public string DescriptionColumnName { get; set; }

        public UrlConfiguration Url { get; set; }

        public string AllDayColumnName { get; set; }

        public IEnumerable<string> Columns => GetColumns();

        private IEnumerable<string> GetColumns()
        {
            List<string> columns = new List<string>()
            {
                TitleColumnName, StartDateColumnName, EndDateColumnName
            };

            if (!String.IsNullOrEmpty(DescriptionColumnName))
            {
                columns.Add(DescriptionColumnName);
            }

            if (Url != null)
            {
                columns.AddRange(Url.Columns);
            }

            if (!String.IsNullOrEmpty(AllDayColumnName))
            {
                columns.Add(AllDayColumnName);
            }

            return columns.Distinct();
        }
    }
}
