using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Nucleus.Core.FileSystem;
using Nucleus.Core.Interfaces;
using Nucleus.Core.Settings;

namespace Nucleus.Core
{
	public static class Startup
	{
		public static Task<StaticGenerator> InitGenerator(NucleusSettings settings, ILog log)
		{
			var sp = ConfigureServices(settings, log);
			var generator = sp.GetService<StaticGenerator>();
			return Task.FromResult(generator);
		}

		public static ServiceProvider ConfigureServices(NucleusSettings settings, ILog log)
		{
			var services = new ServiceCollection();
			services.AddSingleton(log);
			services.AddSingleton<IFileSystemService, FileSystemService>();
			services.AddSingleton<IEntryProcessor, YamlProcessor>();
			services.AddSingleton<IEntryProcessor, MarkdownProcessor>();
			services.AddSingleton<StaticGenerator>();
			services.AddSingleton(settings);
			return services.BuildServiceProvider();
		}
	}
}
