## Make2CSProjUpdater

#### Update the .csproj ItemGroup/Compile elements from a sourcefile list.


This project takes a source list of file, usually from an external Makefile source (i.e. Mono.CSharp.dll.sources) and a C# (.csproj) project file and will update/sync the source list into the project file. 

As an author of the [PlayScript Redux](http://github.com/playscriptredux/playscript) project, there are a large number of Mono .csproj files that are not keep up to date with the Makefile source lists. The Makefile source lists are a known good source as they are used to build Mono and the project files are not used in that capicity. But in development and debugging of the Mono Compiler (mcs.exe) and PlayScript Compiler (playc.exe), the use of Xamarian Studio / MonoDevelop IDE makes life so much easier and thus the need for up to date project (.csproj) files. :-)

Note: This project is generic, just a text file containing a list of filenames and a .csproj file.

Note: Found another use; dumping a list of files via find/ls/etc. to a file and updating a blank project file with that source. This provided a quick jumpstart for a few projects that did not include project files or were so outdated that the project files had to be recreated.


### Usage:

	make2csprojupdater.exe -h
	Make2CSProjectUpdate / ©SushiHangover 2015
	Update the .csproj ItemGroup/Compile elements from a sourcefile list.

	Options:
	  -p, --proj=VALUE           The project filename (.csproj) to update.
	  -s, --sources=VALUE        The filename of the Makefile source list.
	  -i, --interactive          Confirmation prompt to update/save the project
	                               file.
	  -v, --verbose              Increase message verbosity.
	  -h, --help                 Show this message and exit
	  
Sample Usage:
  
	  mono make2csprojupdater.exe -p Sample/Mono.CSharp.csproj -s Sample/Mono.CSharp.dll.sources -v -i
	# Make source files	: 68
	# Project source files	: 58

	# Project needs the following changes	 : Sample/Mono.CSharp.csproj
	# Delete:	..\..\mcs\doc-bootstrap.cs
	# Delete:	cs-parser.cs
	# Delete:	..\corlib\Mono.Security.Cryptography\CryptoConvert.cs
	# Delete:	..\Mono.CompilerServices.SymbolWriter\MonoSymbolFile.cs
	# Delete:	..\Mono.CompilerServices.SymbolWriter\MonoSymbolTable.cs
	# Delete:	..\Mono.CompilerServices.SymbolWriter\MonoSymbolWriter.cs
	# Delete:	..\Mono.CompilerServices.SymbolWriter\SourceMethodBuilder.cs
	# Add:	../../mcs/ps-lang.cs
	# Add:	../../mcs/ps-tokenizer.cs
	# Add:	../../mcs/ps-codegen.cs
	# Add:	../../mcs/cxx-emit.cs
	# Add:	../../mcs/cxx-target.cs
	# Add:	../../mcs/inliner.cs
	# Add:	../../mcs/intrinsics.cs
	# Add:	../../mcs/js-emit.cs
	# Add:	../../mcs/js-target.cs
	# Add:	../../class/Mono.CompilerServices.SymbolWriter/MonoSymbolFile.cs
	# Add:	../../class/Mono.CompilerServices.SymbolWriter/MonoSymbolTable.cs
	# Add:	../../class/Mono.CompilerServices.SymbolWriter/SourceMethodBuilder.cs
	# Add:	../../class/Mono.Security/Mono.Security.Cryptography/CryptoConvert.cs
	# Add:	../../build/common/Consts.cs
	# Add:	../../mcs/cs-parser.cs
	# Add:	../../mcs/ps-parser.cs
	# Add:	Assembly/AssemblyInfo.cs

	Make changes? [y/n]:y
	# Saving backup:	Sample/Mono.CSharp.csproj.bak
	# Deleting:	..\..\mcs\doc-bootstrap.cs
	# Deleting:	cs-parser.cs
	# Deleting:	..\corlib\Mono.Security.Cryptography\CryptoConvert.cs
	# Deleting:	..\Mono.CompilerServices.SymbolWriter\MonoSymbolFile.cs
	# Deleting:	..\Mono.CompilerServices.SymbolWriter\MonoSymbolTable.cs
	# Deleting:	..\Mono.CompilerServices.SymbolWriter\MonoSymbolWriter.cs
	# Deleting:	..\Mono.CompilerServices.SymbolWriter\SourceMethodBuilder.cs
	# Adding:	../../mcs/ps-lang.cs
	# Adding:	../../mcs/ps-tokenizer.cs
	# Adding:	../../mcs/ps-codegen.cs
	# Adding:	../../mcs/cxx-emit.cs
	# Adding:	../../mcs/cxx-target.cs
	# Adding:	../../mcs/inliner.cs
	# Adding:	../../mcs/intrinsics.cs
	# Adding:	../../mcs/js-emit.cs
	# Adding:	../../mcs/js-target.cs
	# Adding:	../../class/Mono.CompilerServices.SymbolWriter/MonoSymbolFile.cs
	# Adding:	../../class/Mono.CompilerServices.SymbolWriter/MonoSymbolTable.cs
	# Adding:	../../class/Mono.CompilerServices.SymbolWriter/SourceMethodBuilder.cs
	# Adding:	../../class/Mono.Security/Mono.Security.Cryptography/CryptoConvert.cs
	# Adding:	../../build/common/Consts.cs
	# Adding:	../../mcs/cs-parser.cs
	# Adding:	../../mcs/ps-parser.cs
	# Adding:	Assembly/AssemblyInfo.cs
	# Saving project:	Sample/Mono.CSharp.csproj