namespace TopoMojo.Core.Models
{
    public class NewGamespace
    {
        public string Id { get; set; }
        public WorkspaceSpec Workspace { get; set; }
    }

    public class WorkspaceSpec
    {
        public int Id { get; set; }
        public NetworkSpec Network { get; set; }
        public VmSpec[] Vms { get; set; } = new VmSpec[] {};
        public bool CustomizeTemplates { get; set; }
        public string Templates { get; set; }
        public string Iso { get; set; }
        public bool HostAffinity { get; set; }
    }

    public class VmSpec
    {
        public string Name { get; set; }
        public int Replicas { get; set; }
    }

    public class NetworkSpec
    {
        public string[] Hosts { get; set; } = new string[] {};
        public string NewIp { get; set; }
        public string[] Dnsmasq { get; set; } = new string[] {};
    }

    public class VmAction
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
    }
}
