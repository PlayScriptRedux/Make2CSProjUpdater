using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;
using Mono.Options;

namespace Make2CSProjUpdater
{

	class MainClass
	{
		static int verbosity;

		static void ShowHelp (OptionSet p)
		{
			Console.WriteLine ("Make2CSProjectUpdate / ©SushiHangover 2015");
			Console.WriteLine ("Update the .csproj ItemGroup/Compile elements from a sourcefile list.");
			Console.WriteLine ();
			Console.WriteLine ("Options:");
			p.WriteOptionDescriptions (Console.Out);
		}

		static void Log (string format, params object[] args)
		{
			if (verbosity > 0) {
				Console.Write ("# ");
				Console.WriteLine (format, args);
			}
		}

		static void FileTest(string fileName) {
			if (!File.Exists(fileName))
				throw(new FileNotFoundException(fileName));
		}

		public static int Main (string[] args)
		{
			String csprojName = "";
			String makeSourcesFile = "";
			bool prompt = true;
			bool showHelp = false;
//			var names = new List<string> ();

			var p = new OptionSet () { { "p=|proj=", "The project filename (.csproj) to update.",
					v => { 
						if (v == null)
							throw(new OptionException("project file required","p=|proj="));
						FileTest(v); 
						csprojName = v;
					}
				}, { "s=|sources=", "The filename of the Makefile source list.",
								v => {
						if (v == null)
							throw(new OptionException("project file required","s=|sources="));
						FileTest(v); 
						makeSourcesFile = v;
					}
				}, { "i|interactive", "Confirmation prompt to update/save the project file.",
					v => {
						prompt = v != null;
					}
				}, { "v|verbose", "Increase message verbosity.",
					v => {
						if (v != null)
							++verbosity;
					}
				}, { "h|help",  "Show this message and exit", 
					v => showHelp = v != null
				},
			};

			List<string> extra;
			try {
				extra = p.Parse (args);
			} catch (OptionException e) {
				Console.Write ("make2csprojupdater: ");
				Console.WriteLine (e.Message);
				Console.WriteLine ("Try `make2csprojupdater --help' for more information.");
				return 1;
			} catch (FileNotFoundException e) {
				Console.Write ("make2csprojupdater: ");
				Console.WriteLine("File not found: {0}", e.Message);
				return 1;
			}

			if (showHelp || extra.Count > 0) {
				ShowHelp (p);
				return 0;
			}

			var makeSources = File.ReadAllLines (makeSourcesFile);

			var dirty = false;
			Log ("Make source files\t: {0}", makeSources.Count ());

			var xProject = XDocument.Load (csprojName);
			XNamespace ns = xProject.Root.LastAttribute.Value;
			var nodes = xProject.Root.Elements ();
			var itemGroups = nodes.Where (i => i.Name == ns + "ItemGroup");
			// Note: Source Compile/Includes can exist in mutliple ItemGroups, need to scan them all           
			var compiles = itemGroups.Elements ().Where (i => i.Name == ns + "Compile");
			Log ("Project source files\t: {0}\n", compiles.Count ());

			Log ("Project needs the following changes\t : {0}", csprojName);
			foreach (var compile in compiles) {
				if (!makeSources.Contains (compile.Attribute ("Include").Value.Replace ('\\', '/'))) {
					dirty = true;
					Log ("Delete:\t{0}", compile.Attribute ("Include").Value);
				}                    
			}
			foreach (var makeSource in makeSources) {
				if (!compiles.Any (s => (s.Attribute ("Include").Value.Replace ('\\', '/') == makeSource))) {
					dirty = true;
					Log ("Add:\t{0}", makeSource);
				}
			}
            
			if (!dirty) {
				Log ("Project source files matches the file sources...");
				return 0;
			}
			var yes = new ConsoleKeyInfo ();
			if (prompt) {
				Console.Write ("\nMake changes? [y/n]:"); 
				yes = Console.ReadKey (false);
				Console.WriteLine ("");
				if (yes.KeyChar != 'y') {
					return 0;
				}
			}
            
			var backupFile = Path.ChangeExtension (csprojName, Path.GetExtension (csprojName) + ".bak");
			Log ("Saving backup:\t{0}", backupFile);
			xProject.Save (backupFile);
			foreach (var compile in compiles) {
				if (!makeSources.Contains (compile.Attribute ("Include").Value.Replace ('\\', '/'))) {
					Log ("Deleting:\t{0}", compile.Attribute ("Include").Value);
					compile.Remove ();
				}                    
			}
                
			var itemGroup = xProject.Root.Elements ().First (g => (g.Name == ns + "ItemGroup") && (g.Descendants ().Any (i => i.Name == ns + "Compile")));
			foreach (var makeSource in makeSources) {
				if (!compiles.Any (s => (s.Attribute ("Include").Value.Replace ('\\', '/') == makeSource))) {
					Log ("Adding:\t{0}", makeSource);
					var compileItem = new XElement (ns + "Compile", new XAttribute ("Include", makeSource.Replace ('/', '\\')), 
						                  new XElement (ns + "Link", Path.GetFileName (makeSource)));
					itemGroup.Add (compileItem);
				}
			}
			Log ("Saving project:\t{0}", csprojName);
			xProject.Save (csprojName);
			return 0;
		}
	}
}
