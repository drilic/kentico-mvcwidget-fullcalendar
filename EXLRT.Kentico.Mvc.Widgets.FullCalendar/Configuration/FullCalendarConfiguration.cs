namespace EXLRT.Kentico.Mvc.Widgets.FullCalendar
{
    using CMS.Core;
    using CMS.DataEngine;
    using Kentico.Mvc.Widgets.FullCalendar.Models.FullCalendar;
    using Kentico.Mvc.Widgets.FullCalendar.Models.Widgets.FullCalendarWidget;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class FullCalendarConfiguration
    {
        private const string LogEventCodeName = "Full Calendar";
        private const string RequiredFieldsMissingMessage = "Required fields in full calendar widget configuration can't be null";
        private const string WidgetAlreadyExistsMessage = "Widget type already registerd. Key must be unique for all sites.";
        private const string MissingConfigurationMessage = "Missing widget configuration";
        internal static bool IsMultiCultureSite = false;
        private static IEventLogService eventLogService = Service.Resolve<IEventLogService>();

        private static Dictionary<string, FullCalendarWidgetConfiguration> WidgetConfiguration { get; set; }

        public static Dictionary<string, FullCalendarWidgetConfiguration> InitializeFullCalendarForSingleCultureSites()
        {
            IsMultiCultureSite = false;
            return WidgetConfiguration = new Dictionary<string, FullCalendarWidgetConfiguration>();
        }

        public static Dictionary<string, FullCalendarWidgetConfiguration> InitializeFullCalendarForMultiCultureSites()
        {
            IsMultiCultureSite = true;
            return WidgetConfiguration = new Dictionary<string, FullCalendarWidgetConfiguration>();
        }

        public static Dictionary<string, FullCalendarWidgetConfiguration> AddConfiguration(this Dictionary<string, FullCalendarWidgetConfiguration> _, string widgetType, FullCalendarWidgetConfiguration widgetTypeConfiguration)
        {
            if (!widgetTypeConfiguration.IsConfigurationValid())
            {
                throw new Exception(RequiredFieldsMissingMessage);
            }

            if (!WidgetConfiguration.ContainsKey(widgetType))
            {
                WidgetConfiguration.Add(widgetType, widgetTypeConfiguration);
            }
            else
            {
                eventLogService.LogInformation(nameof(FullCalendarConfiguration), LogEventCodeName, eventDescription: WidgetAlreadyExistsMessage);
            }

            return WidgetConfiguration;
        }

        public static Dictionary<string, FullCalendarWidgetConfiguration> AddConfiguration(this Dictionary<string, FullCalendarWidgetConfiguration> _, string widgetType, FullCalendarConfigurationType configurationType, string classOrObjectName, string widgetTypeDisplayName, string titleColumnName, string startDateColumnName, string endDateColumnName, UrlConfiguration url = null, string descriptionColumnName = null, string allDayColumnName = null, string siteCodeName = null)
        {
            if (!WidgetConfiguration.ContainsKey(widgetType))
            {
                FullCalendarWidgetConfiguration widgetTypeConfiguration = new FullCalendarWidgetConfiguration
                {
                    TitleColumnName = titleColumnName,
                    WidgetTypeDisplayName = widgetTypeDisplayName,
                    ClassOrObjectName = classOrObjectName,
                    ConfigurationType = configurationType,
                    StartDateColumnName = startDateColumnName,
                    EndDateColumnName = endDateColumnName,
                    Url = url,
                    DescriptionColumnName = descriptionColumnName,
                    AllDayColumnName = allDayColumnName,
                    SiteCodeName = siteCodeName
                };

                if (!widgetTypeConfiguration.IsConfigurationValid())
                {
                    throw new Exception(RequiredFieldsMissingMessage);
                }

                WidgetConfiguration.Add(widgetType, widgetTypeConfiguration);
            }
            else
            {
                eventLogService.LogInformation(nameof(FullCalendarConfiguration), LogEventCodeName, eventDescription: WidgetAlreadyExistsMessage);
            }

            return WidgetConfiguration;
        }

        public static FullCalendarWidgetConfiguration GetWidgetConfiguration(string widgetType)
        {
            if (WidgetConfiguration == null || !WidgetConfiguration.ContainsKey(widgetType))
            {
                eventLogService.LogError(nameof(FullCalendarConfiguration), "Full Calendar", eventDescription: "Missing widget configuration");
                return null;
            }

            return WidgetConfiguration[widgetType];
        }

        public static Dictionary<string, FullCalendarWidgetConfiguration> GetWidgetConfigurations()
        {
            if (WidgetConfiguration == null)
            {
                eventLogService.LogError(nameof(FullCalendarConfiguration), LogEventCodeName, eventDescription: MissingConfigurationMessage);
                return null;
            }

            return WidgetConfiguration;
        }

        private static bool IsConfigurationValid(this FullCalendarWidgetConfiguration configuration)
        {
            string[] requiredColumnsValue = new string[] { configuration.TitleColumnName, configuration.StartDateColumnName, configuration.ClassOrObjectName, configuration.ConfigurationType.ToString(), configuration.WidgetTypeDisplayName };
            return !requiredColumnsValue.Any(columnValue => String.IsNullOrEmpty(columnValue));
        }

        #region "Full Calendar Mapper"

        public static FullCalendarEvent ToFullCalendarEvent(this BaseInfo item, FullCalendarWidgetConfiguration configuration)
        {
            FullCalendarEvent fullCalendarEvent = new FullCalendarEvent()
            {
                Title = item.GetStringValue(configuration.TitleColumnName, string.Empty),
                StartDate = item.GetDateTimeValue(configuration.StartDateColumnName, DateTime.MinValue).ToString("yyyy-MM-dd HH:mm:ss")
            };

            if (!String.IsNullOrEmpty(configuration.EndDateColumnName))
            {
                fullCalendarEvent.EndDate = item.GetDateTimeValue(configuration.EndDateColumnName, DateTime.MinValue).ToString("yyyy-MM-dd HH:mm:ss");
            }

            if (configuration.Url != null)
            {
                string url = configuration.Url.Pattern;
                foreach (string columnName in configuration.Url.Columns)
                {
                    url = url.Replace($"#{columnName}#", item.GetStringValue(columnName, string.Empty));
                }

                fullCalendarEvent.Url = url;
            }

            if (!String.IsNullOrEmpty(configuration.DescriptionColumnName))
            {
                fullCalendarEvent.Description = item.GetStringValue(configuration.DescriptionColumnName, null);
            }

            if (!String.IsNullOrEmpty(configuration.AllDayColumnName))
            {
                fullCalendarEvent.AllDay = item.GetBooleanValue(configuration.AllDayColumnName, false);
            }

            return fullCalendarEvent;
        }

        #endregion
    }
}
