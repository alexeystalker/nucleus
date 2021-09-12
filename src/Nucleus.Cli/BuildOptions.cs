using System;
using System.IO;
using CommandLine;

namespace Nucleus.Cli
{
	[Verb("build", isDefault: true, HelpText = "Build site (default)")]
	public class BuildOptions
	{
		[Option('c',"config", HelpText = "Config filename")]
		public string ConfigFileName { get; set; }
		[Option('i',"input", HelpText = "input folder. '.' for default.")]
		public string InputFolder { get; set; } = ".";
		[Option('o', "output", HelpText = "output folder. '_site' for default.")]
		public string OutputFolder { get; set; } = "_site";
	}
}
