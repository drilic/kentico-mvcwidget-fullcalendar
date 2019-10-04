namespace EXLRT.Kentico.Mvc.Widgets.FullCalendar.Models.FullCalendar
{
    using Newtonsoft.Json;

    public class FullCalendarEvent
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("start")]
        public string StartDate { get; set; }

        [JsonProperty("end")]
        public string EndDate { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("allDay")]
        public bool AllDay { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
