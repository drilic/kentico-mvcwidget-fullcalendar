using EXLRT.Kentico.Mvc.Widgets.FullCalendar.Models.FormComponents.FullCalendarTypeSelector;
using Kentico.Forms.Web.Mvc;

[assembly: RegisterFormComponent(FullCalendarTypeSelectorComponent.IDENTIFIER, typeof(FullCalendarTypeSelectorComponent), "Full calendar selector", Description = "Full calendar type selector used to select between different calendar type from predefined list of calendars.", IconClass = "icon-calendar", ViewName = "FormComponents/FullCalendarTypeSelector/_FormComponents.FullCalendarTypeSelector")]
namespace EXLRT.Kentico.Mvc.Widgets.FullCalendar.Models.FormComponents.FullCalendarTypeSelector
{
    using CMS.SiteProvider;
    using Kentico.Mvc.Widgets.FullCalendar;
    using Kentico.Mvc.Widgets.FullCalendar.Models.Widgets.FullCalendarWidget;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class FullCalendarTypeSelectorComponent : SelectorFormComponent<DropDownProperties>
    {

        public const string IDENTIFIER = "EXLRT.FormComponents.FullCalendarTypeSelector";

        protected override IEnumerable<SelectListItem> GetItems()
        {
            Dictionary<string, FullCalendarWidgetConfiguration> widgetConfigurations = FullCalendarConfiguration.GetWidgetConfigurations();
            if (widgetConfigurations == null || widgetConfigurations.Count == 0)
            {
                yield break;
            }

            if (!string.IsNullOrEmpty(Properties.OptionLabel))
            {
                yield return new SelectListItem()
                {
                    Value = String.Empty,
                    Text = Properties.OptionLabel
                };
            }

            foreach (KeyValuePair<string, FullCalendarWidgetConfiguration> widgetConfiguration in widgetConfigurations)
            {
                FullCalendarWidgetConfiguration currentConfig = widgetConfiguration.Value;

                if (String.IsNullOrEmpty(currentConfig.SiteCodeName) || currentConfig.SiteCodeName.Equals(SiteContext.CurrentSiteName))
                {
                    yield return new SelectListItem()
                    {
                        Value = widgetConfiguration.Key,
                        Text = widgetConfiguration.Value.WidgetTypeDisplayName
                    };
                }
            }
        }
    }
}
