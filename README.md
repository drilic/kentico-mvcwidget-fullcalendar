# Kentico MVC Full Calendar widget
[![MIT](https://camo.githubusercontent.com/52ec9e2dfec7264e254fb7af5ac87f301ced9180/68747470733a2f2f696d672e736869656c64732e696f2f707970692f6c2f417270656767696f2e737667)](https://raw.githubusercontent.com/hyperium/hyper/master/LICENSE)

Full calendar allows you to render various content as calendar events.

## Requirements
* **Kentico 12.0.41** or later version is required to use this component.
* **Autofac 3.5.2** for resolving dependency injection

## Download & instalation
1. Download and install widget:
    * Install widget through nuget package
        * Download nuget package from this [link](https://github.com/drilic/kentico-mvcwidget-fullcalendar/blob/master/EXLRT.Kentico.Mvc.Widgets.FullCalendar/EXLRT.Kentico.Mvc.Widgets.FullCalendar.1.0.0.nupkg)
        * Setup local nugetfeed and copy nuget package **EXLRT.Kentico.Mvc.Widgets.FullCalendar.1.0.0.nupkg** to newly created feed.
        * Install package to your MVC solution
    * Install widget by source code
        * Clone this repostiory to your file system: https://github.com/drilic/kentico-mvcwidget-fullcalendar
        * Link **EXLRT.Kentico.Mvc.Widgets.FullCalendar.csproj** as existing project
        * Copy files to exact same structure in MVC project:
            * Views\Shared\Widgets\FullCalendarWidget\\_FullCalendarWidget.cshtml
            * Views\Shared\FormComponents\FullCalendarTypeSelector\\_FormComponents.FullCalendarTypeSelector.cshtml
            * Content\Widgets\FullCalendar\css\fullCalendar.css
            * Content\Widgets\FullCalendar\js\fullCalendar.js
        * Add project as reference to the MVC project
        * Build solution
4. [Configure routing for widget](#configure-routing)
5. [Register dependency injection](#register-dependency-injection)
6. [Setup predefined calendars](#setup-predefined-calendars)
	* Calendars can be configured per site, but name must be unique globally
7. [Register necessary styles and scripts for full calendar in layout](#register-layout-scripts)

For more information check the blog on this [link](https://exlrt.com).

### Configure routing

Method definition:
```csharp
void MapFullCalendarRoutes(this RouteCollection routes, MvcRouteHandler routeHandler = null, object constraint = null, CultureInfo defaultCulture = null);
```
Example of configuration for DancingGoat site (RouteConfig.cs):
```csharp
using EXLRT.Kentico.Mvc.Widgets.FullCalendar;
...
routes.MapFullCalendarRoutes(new MultiCultureMvcRouteHandler(defaultCulture), new { culture = new SiteCultureConstraint() }, defaultCulture);
```

### Register dependency injection

Method definition:
```csharp
void RegisterFullCalendarDependencies(this ContainerBuilder builder);
```
Example of configuration for DancingGoat site (DependencyResolverConfig.cs):
```csharp
using EXLRT.Kentico.Mvc.Widgets.FullCalendar;
...
builder.RegisterFullCalendarDependencies();
```

### Setup predefined calendars
Method definition:
```csharp
Dictionary<string, FullCalendarWidgetConfiguration> AddConfiguration(this Dictionary<string, FullCalendarWidgetConfiguration> _, string widgetType, FullCalendarConfigurationType configurationType, string classOrObjectName, string widgetTypeDisplayName, string titleColumnName, string startDateColumnName, string endDateColumnName, UrlConfiguration url = null, string descriptionColumnName = null, string allDayColumnName = null, string siteCodeName = null);
```
Example of configuration for DancingGoat site (ApplicationConfig.cs):
```csharp
using EXLRT.Kentico.Mvc.Widgets.FullCalendar;
...
FullCalendarConfiguration.InitializeFullCalendarForMultiCultureSites()
                    .AddConfiguration("articles", FullCalendarConfigurationType.Pages, "DancingGoatMvc.Article",
                                      "Articles (Pages)", "DocumentName", "DocumentPublishFrom", "DocumentPublishTo", new UrlConfiguration()
                                      {
                                          Pattern = "/#DocumentCulture#/Articles/#NodeGUID#/#NodeAlias#",
                                          Columns = new string[] { "DocumentCulture", "NodeAlias", "NodeGUID" }
                                      })
									  
                    // Objects bellow does not exist in DancingGoat sample. They must be created in Kentico CMS before using
                    .AddConfiguration("events", FullCalendarConfigurationType.CustomTables, "customtable.Events",
                                      "Events (Custom Tables)", "ItemName", "StartDate", "EndDate", descriptionColumnName: "Description")
									  
                    .AddConfiguration("classes", FullCalendarConfigurationType.Classes, "dancinggoat.Events",
                                      "Events (Module Classes)", "EventsName", "EventsStartDate", "EventsEndDate");
```

### Register layout scripts

Example of configuration for DancingGoat site (_Layout.cshtml):
```csharp
@using EXLRT.Kentico.Mvc.Widgets.FullCalendar.Extensions;
...
<head>
	...
	 @Html.RenderFullCalendarStyles()
</head>
<body>
	...
	@Html.RenderFullCalendarScripts();
</body>

```
