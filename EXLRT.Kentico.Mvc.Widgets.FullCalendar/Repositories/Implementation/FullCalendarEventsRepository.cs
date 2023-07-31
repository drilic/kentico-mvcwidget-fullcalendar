namespace EXLRT.Kentico.Mvc.Widgets.FullCalendar.Repositories.Implementation
{
    using CMS.CustomTables;
    using CMS.DataEngine;
    using CMS.DocumentEngine;
    using CMS.Helpers;
    using CMS.SiteProvider;
    using global::Kentico.Content.Web.Mvc;
    using Kentico.Mvc.Widgets.FullCalendar;
    using Kentico.Mvc.Widgets.FullCalendar.Models.FullCalendar;
    using Kentico.Mvc.Widgets.FullCalendar.Models.Widgets.FullCalendarWidget;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class FullCalendarEventsRepository : IFullCalendarEventsRepository
    {
        private readonly IPageUrlRetriever PageUrlRetriever;
        private readonly string CultureName;
        private readonly bool LatestVersionEnabled;
        private readonly string SiteName;

        public FullCalendarEventsRepository(IPageUrlRetriever pageUrlRetriever, string cultureName, bool latestVersionEnabled)
        {
            PageUrlRetriever = pageUrlRetriever;
            CultureName = cultureName;
            LatestVersionEnabled = latestVersionEnabled;
            SiteName = SiteContext.CurrentSiteName;
        }

        public IEnumerable<FullCalendarEvent> GetModuleClassEvents(FullCalendarWidgetConfiguration fullCalendarWidgetConfiguration)
        {
            Func<IEnumerable<FullCalendarEvent>> moduleClassesLoadMethod = () => new ObjectQuery(fullCalendarWidgetConfiguration.ClassOrObjectName)
                                      .Columns(fullCalendarWidgetConfiguration.Columns)
                                      .Select(item => item.ToFullCalendarEvent(fullCalendarWidgetConfiguration, this.PageUrlRetriever));

            CacheSettings cacheSettings = new CacheSettings(10, "exlrt|data|eventmoduleclasses", SiteName, CultureName)
            {
                GetCacheDependency = () =>
                {
                    string dependencyCacheKey = String.Format("{0}|all", fullCalendarWidgetConfiguration.ClassOrObjectName.ToLowerInvariant());
                    return CacheHelper.GetCacheDependency(dependencyCacheKey);
                }
            };

            return CacheHelper.Cache(moduleClassesLoadMethod, cacheSettings);
        }

        public IEnumerable<FullCalendarEvent> GetCustomTableEvents(FullCalendarWidgetConfiguration fullCalendarWidgetConfiguration)
        {
            Func<IEnumerable<FullCalendarEvent>> customTablesLoadMethod = () => CustomTableItemProvider.GetItems(fullCalendarWidgetConfiguration.ClassOrObjectName)
                                      .Columns(fullCalendarWidgetConfiguration.Columns)
                                      .Select(item => item.ToFullCalendarEvent(fullCalendarWidgetConfiguration, this.PageUrlRetriever));

            CacheSettings cacheSettings = new CacheSettings(10, "exlrt|data|eventcustomtables", SiteName, CultureName)
            {
                GetCacheDependency = () =>
                {
                    string dependencyCacheKey = String.Format("customtableitem.{0}|all", fullCalendarWidgetConfiguration.ClassOrObjectName.ToLowerInvariant());
                    return CacheHelper.GetCacheDependency(dependencyCacheKey);
                }
            };

            return CacheHelper.Cache(customTablesLoadMethod, cacheSettings);
        }

        public IEnumerable<FullCalendarEvent> GetPageTypeEvents(FullCalendarWidgetConfiguration fullCalendarWidgetConfiguration)
        {
            List<string> columns = new List<string>() { nameof(TreeNode.DocumentCulture), nameof(TreeNode.NodeAliasPath) };
            columns.AddRange(fullCalendarWidgetConfiguration.Columns);
            columns = columns.Distinct().ToList();

            Func<IEnumerable<FullCalendarEvent>> pagesLoadMethod = () => DocumentHelper.GetDocuments()
                                      .Type(fullCalendarWidgetConfiguration.ClassOrObjectName)
                                      .Published(!LatestVersionEnabled)
                                      .LatestVersion(LatestVersionEnabled)
                                      .OnSite(SiteContext.CurrentSiteName)
                                      .Culture(CultureName)
                                      .Columns(columns)
                                      .TypedResult
                                      .Select(node => node.ToFullCalendarEvent(fullCalendarWidgetConfiguration, this.PageUrlRetriever));

            CacheSettings cacheSettings = new CacheSettings(10, "exlrt|data|eventpages", SiteName, CultureName)
            {
                GetCacheDependency = () =>
                {
                    string dependencyCacheKey = String.Format("nodes|{0}|{1}|all", SiteName, fullCalendarWidgetConfiguration.ClassOrObjectName.ToLowerInvariant());
                    return CacheHelper.GetCacheDependency(dependencyCacheKey);
                }
            };

            return CacheHelper.Cache(pagesLoadMethod, cacheSettings);
        }
    }
}
