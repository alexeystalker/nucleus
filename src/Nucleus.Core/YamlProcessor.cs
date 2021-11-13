using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading;
using Nucleus.Core.Extensions;
using Nucleus.Core.Interfaces;
using Nucleus.Core.Models;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Nucleus.Core;

public class YamlProcessor : IEntryProcessor
{
	private const string YamlDocumentStart = "---";
	private readonly ILog _log;

	private readonly Lazy<IDeserializer> YamlDeserializerLazy =
		new Lazy<IDeserializer>(() => new DeserializerBuilder()
				.IgnoreUnmatchedProperties()
				.WithNamingConvention(CamelCaseNamingConvention.Instance)
				.Build(),
			LazyThreadSafetyMode.ExecutionAndPublication);

	public YamlProcessor(ILog log)
	{
		_log = log;
	}

	public SiteEntry ProcessEntry(SiteEntry entry)
	{
		if(!entry.StringContent.IsSignificant())
			return entry;

		var (stringMetadata, listMetadata) = GetYamlMetadata(entry.StringContent);
		return entry with
		{
			StringMetadata = entry.StringMetadata.Merge(stringMetadata),
			ListMetadata = entry.ListMetadata.Merge(listMetadata)
		};
	}

	private (Dictionary<string, string> stringMetadata, Dictionary<string, List<string>> listMetadata) GetYamlMetadata(
		string str)
	{
		if(!str.StartsWith(YamlDocumentStart))
			return (new Dictionary<string, string>(), new Dictionary<string, List<string>>());

		try
		{
			using var input = new StringReader(str);
			var parser = new Parser(input);
			parser.Consume<StreamStart>();
			parser.Consume<DocumentStart>();
			var untyped = YamlDeserializerLazy.Value.Deserialize<Dictionary<string, object>>(parser);
			parser.Consume<DocumentEnd>();

			var stringMetadata = untyped
				.Select(kvp => new KeyValuePair<string, string>(kvp.Key, kvp.Value as string))
				.Where(kvp => kvp.Value.IsSignificant())
				.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

			var listMetadata = untyped
				.Select(kvp =>
					new KeyValuePair<string, List<string>>(kvp.Key,
						(kvp.Value as List<object>)?.OfType<string>().ToList()))
				.Where(kvp => kvp.Value != null)
				.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
			return (stringMetadata, listMetadata);
		} catch(Exception e)
		{
			_log.Error($"Error while deserializing yaml {e.Message}");
		}

		return (new Dictionary<string, string>(), new Dictionary<string, List<string>>());
	}
}
