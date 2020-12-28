using System;
using System.Collections.Specialized;

namespace CPC.TaskManager.Plugins
{
    public class ViewPlugin : TaskPlugin
    {
        public string Url { get; set; } = "http://*:18081";

        public Action<ViewOptions> Setup { get; set; }

        protected internal override void OnInitialize(in NameValueCollection props)
        {
            props.Add("quartz.jobStore.type", "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz");
            props.Add("quartz.jobStore.useProperties", "true");
            props.Add("quartz.jobStore.tablePrefix", "QRTZ_");
            props.Add("quartz.jobStore.driverDelegateType", "Quartz.Impl.AdoJobStore.SQLiteDelegate, Quartz");
            props.Add("quartz.jobStore.dataSource", "default");
            props.Add("quartz.dataSource.default.connectionString", "Data Source=store.sqlite.db");
            props.Add("quartz.dataSource.default.provider", "SQLite-Microsoft");
            props.Add("quartz.serializer.type", "json");

            props.Add("quartz.plugin.history.type", "CPC.TaskManager.Plugins.HistoryPlugin, CPC.TaskManager");
        }

        protected internal override void OnStart() => SelfHost.StartAsync(Url, Setup).Wait();

    }
}
