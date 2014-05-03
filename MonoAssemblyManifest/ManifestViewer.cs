using System;
using System.IO;
using Mono.Cecil;

namespace MonoAssemblyManifest
{
	class ManifestViewer
	{
		string fullPath;
		TextWriter writer;

		public ManifestViewer ()
		{
			Level = ManifestLevel.Minimal;
		}

		public ManifestLevel Level { get; set; }

		public void DisplayManifest (string fileName, TextWriter writer)
		{
			fullPath = Path.GetFullPath (fileName);
			if (!File.Exists (fullPath))
				throw new FileNotFoundException ("File does not exist.", fullPath);

			this.writer = writer;

			ModuleDefinition module = ModuleDefinition.ReadModule (fullPath);

			DisplayModule (module);
		}

		void DisplayModule (ModuleDefinition module)
		{
			WriteLine ("Name: {0}", module.Assembly.Name);

			if (Level == ManifestLevel.Minimal)
				return;

			WriteLine ("Target Framework: {0}", module.Runtime);

			foreach (CustomAttribute attribute in module.Assembly.CustomAttributes) {
				DisplayAttribute (attribute);
			}
		}

		void WriteLine (string format, params object [] arg)
		{
			writer.WriteLine (format, arg);
		}

		void DisplayAttribute (CustomAttribute attribute)
		{
			switch (attribute.AttributeType.Name) {

			case "AssemblyTitleAttribute":
				WriteFirstConstructorArgument ("Title", attribute);
				break;

			case "AssemblyDescriptionAttribute":
				WriteFirstConstructorArgument ("Description", attribute);
				break;

			case "AssemblyCopyrightAttribute":
				WriteFirstConstructorArgument ("Copyright", attribute);
				break;

			case "AssemblyCompanyAttribute":
				WriteFirstConstructorArgument ("Company", attribute);
				break;

			case "AssemblyFileVersionAttribute":
				WriteFirstConstructorArgument ("File Version", attribute);
				break;

			case "AssemblyInformationalVersionAttribute":
				WriteFirstConstructorArgument ("Informational Version", attribute);
				break;

			case "AssemblyProductAttribute":
				WriteFirstConstructorArgument ("Product", attribute);
				break;
			}
		}

		void WriteFirstConstructorArgument (string name, CustomAttribute attribute)
		{
			WriteLine ("{0}: {1}", name, GetFirstConstructorArgument (attribute));
		}

		string GetFirstConstructorArgument (CustomAttribute attribute)
		{
			if (attribute.ConstructorArguments.Count > 0) {
				return attribute.ConstructorArguments [0].Value as string;
			}
			return "";
		}
	}
}
