namespace EXLRT.Kentico.Mvc.Widgets.FullCalendar.Extensions
{
    using CMS.Helpers;
    using global::Kentico.PageBuilder.Web.Mvc;
    using global::Kentico.Web.Mvc;
    using Kentico.Mvc.Widgets.FullCalendar;
    using Kentico.Mvc.Widgets.FullCalendar.Models.Widgets.FullCalendarWidget;
    using System;
    using System.Globalization;
    using System.Web;
    using System.Web.Mvc;

    public static class FullCalendarHtmlExtensions
    {
        private const string FullCalendarVersion = "4.2.0";
        private const string PopperjsVersion = "1.15.0";
        private const string TooltipjsVersion = "1.3.2";

        public static MvcHtmlString RenderFullCalendarStyles(this HtmlHelper _)
        {
            string styles = $@"<link rel='stylesheet' href='//cdnjs.cloudflare.com/ajax/libs/fullcalendar/{FullCalendarVersion}/core/main.min.css' />
                              <link rel='stylesheet' href='//cdnjs.cloudflare.com/ajax/libs/fullcalendar/{FullCalendarVersion}/daygrid/main.min.css' /> ";

            return MvcHtmlString.Create(styles);
        }

        public static MvcHtmlString InitializeFullCalendar(this HtmlHelper _, FullCalendarWidgetProperties widgetProperties)
        {
            string resultHtml = string.Empty;

            if (widgetProperties != null && !String.IsNullOrEmpty(widgetProperties.CalendarType))
            {
                string calendarId = $"fullCalendar-{Guid.NewGuid()}";
                string locale = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                bool use12hFormat = widgetProperties.HourFormat.Equals("12") ? true : false;
                string apiUrl = $"~/fullcalendarapi/getcalendardata/";

                if (FullCalendarConfiguration.IsMultiCultureSite)
                {
                    apiUrl = $"~/{CultureInfo.CurrentUICulture.Name}/fullcalendarapi/getcalendardata/";
                }

                resultHtml = $@"<div id='{calendarId}'></div>
                                 <script>
                                     (function () {{
                                         var config = {{
                                             calendarId: '{calendarId}',
                                             eventsApi: '{URLHelper.ResolveUrl(apiUrl)}',
                                             locale: '{locale}',
                                             useTooltip: '{widgetProperties.UseTooltip}',
                                             widgetType: '{widgetProperties.CalendarType}',
                                             use12hFormat: '{use12hFormat}'
                                         }};

                                        if (document.readyState === 'loading') {{
                                            document.addEventListener('DOMContentLoaded', function () {{
                                                initFullCalendar(config);
                                            }});
                                        }} else {{
                                            initFullCalendar(config);
                                        }}
                                    }})();
                                    </script>";
            }
            else
            {
                if (HttpContext.Current.Kentico().PageBuilder().EditMode)
                {
                    resultHtml = @"<p> Full calendar widget does not have proper configuration!</p>";
                }
            }

            return MvcHtmlString.Create(resultHtml);
        }

        public static MvcHtmlString RenderFullCalendarScripts(this HtmlHelper _)
        {
            string scripts = $@"<script type='text/javascript' src='//cdnjs.cloudflare.com/ajax/libs/fullcalendar/{FullCalendarVersion}/core/main.min.js'></script>
                               <script type='text/javascript' src='//cdnjs.cloudflare.com/ajax/libs/fullcalendar/{FullCalendarVersion}/daygrid/main.min.js'></script>
                               <script type='text/javascript' src='//cdnjs.cloudflare.com/ajax/libs/fullcalendar/{FullCalendarVersion}/core/locales-all.js'></script>
                               <script type='text/javascript' src='//cdnjs.cloudflare.com/ajax/libs/popper.js/{PopperjsVersion}/umd/popper.min.js'></script>
                               <script type='text/javascript' src='//cdnjs.cloudflare.com/ajax/libs/tooltip.js/{TooltipjsVersion}/umd/tooltip.min.js'></script>
                               <script type='text/javascript' src='~/content/widgets/fullcalendar/js/fullcalendar.js'></script>";

            return MvcHtmlString.Create(scripts);
        }
    }
}
