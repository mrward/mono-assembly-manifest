using System;
using Mono.Options;
using System.Collections.Generic;
using System.Linq;

namespace MonoAssemblyManifest
{
	class ManifestApp
	{
		string assemblyFileName;
		bool showHelp;
		bool showAll;
		OptionSet options;

		public static void Main (string[] args)
		{
			try {
				var app = new ManifestApp ();
				app.Run (args);
			} catch (Exception ex) {
				Console.WriteLine (ex.Message);
			}
		}

		void Run (string[] args)
		{
			if (!ParseOptions (args))
				return;

			var viewer = new ManifestViewer ();
			if (showAll) {
				viewer.Level = ManifestLevel.Full;
			}
			viewer.DisplayManifest (assemblyFileName, Console.Out);
		}

		bool ParseOptions (string[] args)
		{
			options = new OptionSet ();
			options.Add (
				"h|help",
				"show this message and exit",
				v => showHelp = true);
			options.Add (
				"a|all",
				"show all assembly metadata",
				v => showAll = true);

			List<string> fileNames = options.Parse (args);

			if (fileNames.Count > 0) {
				assemblyFileName = fileNames [0];
			}

			if (showHelp || !fileNames.Any ()) {
				ShowHelp ();
				return false;
			}

			return true;
		}

		void ShowHelp ()
		{
			Console.WriteLine ("Usage: monomanifest [options]+ filename");
			Console.WriteLine ("Displays metadata for a .NET assembly.");
			Console.WriteLine ();
			Console.WriteLine ("Options:");
			options.WriteOptionDescriptions (Console.Out);
		}
	}
}
