namespace IAMRoleService.WebApi.Models
{
    public class KubernetesClusterName
    {
        private readonly string _name;

        public KubernetesClusterName(string name)
        {
            _name = name;
        }
        
        public static implicit operator KubernetesClusterName (string name)
        {
            return new KubernetesClusterName(name);
        }
        
        public override string ToString()
        {
            return _name;
        }
    }
}