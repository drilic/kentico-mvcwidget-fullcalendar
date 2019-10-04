function initFullCalendar(config) {
    var calendarEl = document.getElementById(config.calendarId);
    if (calendarEl) {
        fetch(config.eventsApi + config.widgetType)
            .then(response => response.json())
            .then(data => {
                var calendar = new FullCalendar.Calendar(calendarEl, {
                    plugins: ['dayGrid'],
                    contentHeight: 'auto',
                    header:
                    {
                        left: 'title',
                        center: '',
                        right: 'today prev,next'
                    },
                    eventTimeFormat: {
                        hour: '2-digit',
                        minute: '2-digit',
                        hour12: (config.use12hFormat === 'True')
                    },
                    locale: config.locale,
                    eventLimit: true, // for all non-TimeGrid views
                    events: JSON.parse(data),
                    eventRender: function (info) {
                        customizeCalendarEvent(config, info);
                    }
                });

                calendar.render();
            }).catch(error => console.error(error));
    }
}

function customizeCalendarEvent(config, info) {
    if (info.event.extendedProps.description) {
        let h = '<br/><span class="fc-description"> ' + info.event.extendedProps.description + '</span>';
        var contentElement = info.el.getElementsByClassName('fc-content')[0];
        contentElement.insertAdjacentHTML('beforeend', h);

        if (config.useTooltip === 'True') {
            var tooltip = new Tooltip(info.el, {
                title: info.event.extendedProps.description,
                placement: 'top',
                trigger: 'hover',
                container: 'body',
                delay: {
                    show: 300,
                    hide: 0
                }
            });
        }
    }
}