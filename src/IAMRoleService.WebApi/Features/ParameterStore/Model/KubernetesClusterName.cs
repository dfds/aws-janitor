namespace IAMRoleService.WebApi.Models
{
    public class KubernetesClusterName
    {
        private readonly string _name;

        public KubernetesClusterName(string name)
        {
            _name = name;
        }

        public static implicit operator KubernetesClusterName(string name)
        {
            if (name == null)
            {
                return null;
            }

            return new KubernetesClusterName(name);
        }
        public static implicit operator string(KubernetesClusterName kubernetesClusterName)
        {
            if (kubernetesClusterName == null)
            {
                return null;
            }

            return kubernetesClusterName._name;
        }
        public override string ToString()
        {
            return _name;
        }

        public override bool Equals(object obj)
        {
            if (obj is KubernetesClusterName kubernetesClusterName)
            {
                return _name.Equals(kubernetesClusterName._name);
            }

            if (obj is string item)
            {
                return _name.Equals(item);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return _name.GetHashCode();
        }
    }
}