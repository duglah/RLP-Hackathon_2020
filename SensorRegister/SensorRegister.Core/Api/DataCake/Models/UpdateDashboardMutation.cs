namespace SensorRegister.Core.Api.DataCake.Models
{
    public class UpdateDashboardMutation
    {
        public string WorkspaceId { get; set; } = "2966c4d8-7f98-4e89-8d7a-60028e1b75a2";
        public string DashboardId { get; set; } = "1eef9d8c-f4b1-4675-87f6-fa4bcee56b82";
        public string Name { get; set; } = "RLP Hackathon";
        public bool Public { get; set; } = false;
        public string Icon { get; set; } = "chart-line";

        public DataCakeDashboard Dashboard { get; set; }

        //quick and dirty 👺
        public override string ToString()
            => $@"
            mutation {{
                updateDashboard(input: {{
                    workspace: ""{WorkspaceId}"",
                    dashboard: ""{DashboardId}"",
                    name: ""{Name}"",
                    public: {(Public ? "true" : "false")},
                    icon: ""{Icon}"", 
                    dashboards: """"{{Dashboard}}""""
                }}) {{
                ok,
                dashboard{{name, dashboards}}
                }}
            }}
            "
                .Replace("\\\"","\"")
                .Replace($@"""""{{Dashboard}}""""", $"\"{Dashboard}\"");
    }
}