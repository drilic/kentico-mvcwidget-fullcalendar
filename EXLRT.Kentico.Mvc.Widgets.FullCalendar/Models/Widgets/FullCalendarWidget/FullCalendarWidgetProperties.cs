using EXLRT.Kentico.Mvc.Widgets.FullCalendar.Models.Widgets.FullCalendarWidget;
using Kentico.PageBuilder.Web.Mvc;

[assembly: RegisterWidget("EXLRT.FullCalendarWidget", "Full calendar", typeof(FullCalendarWidgetProperties), customViewName: "Widgets/FullCalendarWidget/_FullCalendarWidget", Description = "Full calendar allows you to render various content as calendar events.", IconClass = "icon-calendar")]
namespace EXLRT.Kentico.Mvc.Widgets.FullCalendar.Models.Widgets.FullCalendarWidget
{
    using global::Kentico.Forms.Web.Mvc;
    using Kentico.Mvc.Widgets.FullCalendar.Models.FormComponents.FullCalendarTypeSelector;

    public class FullCalendarWidgetProperties : IWidgetProperties
    {
        /// <summary>
        /// Theme of the widget.
        /// </summary>
        [EditingComponent(FullCalendarTypeSelectorComponent.IDENTIFIER, Label = "Calendar type", Order = 1)]
        [EditingComponentProperty(nameof(DropDownProperties.OptionLabel), "Select calendar")]
        public string CalendarType { get; set; }

        [EditingComponent(DropDownComponent.IDENTIFIER, Label = "Hour format", Order = 2)]
        [EditingComponentProperty(nameof(DropDownProperties.DataSource), "12\r\n24")]
        public string HourFormat { get; set; } = "12";


        [EditingComponent(CheckBoxComponent.IDENTIFIER, Label = "Use tooltip", Order = 3)]
        public bool UseTooltip { get; set; } = true;
    }
}
