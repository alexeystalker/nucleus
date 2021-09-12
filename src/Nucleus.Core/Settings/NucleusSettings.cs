using System.IO;
using System.Text.Json.Serialization;

namespace Nucleus.Core.Settings
{
	public class NucleusSettings
	{
		[JsonIgnore]
		public string InputDir { get; set; }
		[JsonIgnore]
		public string OutputDir { get; set; }
		public InputFolderSettings PagesFolder { get; set; }
		[JsonIgnore]
		public string PagesFolderPath => Path.GetFullPath(Path.Combine(InputDir, PagesFolder.Path));
		//[JsonPropertyName("notesFolder")]
		public InputFolderSettings NotesFolder { get; set; }
		[JsonIgnore]
		public string NotesFolderPath => Path.GetFullPath(Path.Combine(InputDir, NotesFolder.Path));
		public InputFolderSettings LayoutFolder { get; set; }
		public string LayoutFolderPath => Path.GetFullPath(Path.Combine(InputDir, LayoutFolder.Path));
		public string SiteTitle { get; set; }
	}
}
