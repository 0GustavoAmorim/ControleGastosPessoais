using System.Reflection;

namespace ControleGastosPessoais.API.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterRepositories(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var repositories = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Repository"))
                .Select(repoType => new
                {
                    Interface = repoType.GetInterfaces().FirstOrDefault(i => i.Name.EndsWith("Repository")),
                    Implementation = repoType
                })
                .Where(x => x.Interface != null);

            foreach(var repo in repositories)
            {
                services.AddScoped(repo.Interface, repo.Implementation);
            }
        }
    }
}
