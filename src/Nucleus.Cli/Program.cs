using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using CommandLine;
using Nucleus.Core;
using Nucleus.Core.Logging;
using Nucleus.Core.Settings;

namespace Nucleus.Cli
{
	class Program
	{
		static Task<int> Main(string[] args) => Parser.Default.ParseArguments<BuildOptions, object>(args)
			.MapResult(
				(BuildOptions options) => BuildEntryPoint(options),
				errors => Task.FromResult(1));

		private static async Task<int> BuildEntryPoint(BuildOptions options)
		{
			var log = new AsyncOutLog();
			log.Info("Begin.");
			var fullInputPath = Path.GetFullPath(options.InputFolder);
			var fullOutputPath = Path.GetFullPath(options.OutputFolder);
			var fullConfigFilePath = Path.GetFullPath(options.ConfigFileName ?? Path.Combine(options.InputFolder, "nucleus.json"));
			try
			{
				var settings = await LoadSettings(fullConfigFilePath);
				settings.InputDir = fullInputPath;
				settings.OutputDir = fullOutputPath;
				var generator = await Startup.InitGenerator(settings, log);
				await generator.GenerateSite();
				log.Info("Completed.");
				return 0;
			}
			catch(Exception e)
			{
				log.Fatal($"{e.Message}{Environment.NewLine}{e.StackTrace}");
				return -1;
			}
		}

		private static async Task<NucleusSettings> LoadSettings(string fullConfigFilePath)
		{
			await using var openSettingsStream = File.OpenRead(fullConfigFilePath);
			var settings = await JsonSerializer.DeserializeAsync<NucleusSettings>(openSettingsStream, new JsonSerializerOptions()
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			});
			if(settings == null)
				throw new ArgumentException($"File {fullConfigFilePath} is corrupted.");

			return settings;
		}
	}
}
