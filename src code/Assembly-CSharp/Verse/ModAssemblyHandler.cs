using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Verse
{
	// Token: 0x02000277 RID: 631
	public class ModAssemblyHandler
	{
		// Token: 0x06001205 RID: 4613 RVA: 0x0006953D File Offset: 0x0006773D
		public ModAssemblyHandler(ModContentPack mod)
		{
			this.mod = mod;
		}

		// Token: 0x06001206 RID: 4614 RVA: 0x00069558 File Offset: 0x00067758
		public void ReloadAll()
		{
			if (!ModAssemblyHandler.globalResolverIsSet)
			{
				ResolveEventHandler @object = (object obj, ResolveEventArgs args) => Assembly.GetExecutingAssembly();
				AppDomain.CurrentDomain.AssemblyResolve += @object.Invoke;
				ModAssemblyHandler.globalResolverIsSet = true;
			}
			foreach (FileInfo fileInfo in from f in ModContentPack.GetAllFilesForModPreserveOrder(this.mod, "Assemblies/", (string e) => e.ToLower() == ".dll", null)
			select f.Item2)
			{
				Assembly assembly = null;
				try
				{
					byte[] rawAssembly = File.ReadAllBytes(fileInfo.FullName);
					FileInfo fileInfo2 = new FileInfo(Path.Combine(fileInfo.DirectoryName, Path.GetFileNameWithoutExtension(fileInfo.FullName)) + ".pdb");
					if (fileInfo2.Exists)
					{
						byte[] rawSymbolStore = File.ReadAllBytes(fileInfo2.FullName);
						assembly = AppDomain.CurrentDomain.Load(rawAssembly, rawSymbolStore);
					}
					else
					{
						assembly = AppDomain.CurrentDomain.Load(rawAssembly);
					}
				}
				catch (Exception ex)
				{
					Log.Error("Exception loading " + fileInfo.Name + ": " + ex.ToString());
					break;
				}
				if (!(assembly == null) && this.AssemblyIsUsable(assembly))
				{
					GenTypes.ClearCache();
					this.loadedAssemblies.Add(assembly);
				}
			}
		}

		// Token: 0x06001207 RID: 4615 RVA: 0x000696F8 File Offset: 0x000678F8
		private bool AssemblyIsUsable(Assembly asm)
		{
			try
			{
				asm.GetTypes();
			}
			catch (ReflectionTypeLoadException ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"ReflectionTypeLoadException getting types in assembly ",
					asm.GetName().Name,
					": ",
					ex
				}));
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Loader exceptions:");
				if (ex.LoaderExceptions != null)
				{
					foreach (Exception ex2 in ex.LoaderExceptions)
					{
						stringBuilder.AppendLine("   => " + ex2.ToString());
					}
				}
				Log.Error(stringBuilder.ToString());
				return false;
			}
			catch (Exception ex3)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception getting types in assembly ",
					asm.GetName().Name,
					": ",
					ex3
				}));
				return false;
			}
			return true;
		}

		// Token: 0x04000F2F RID: 3887
		private ModContentPack mod;

		// Token: 0x04000F30 RID: 3888
		public List<Assembly> loadedAssemblies = new List<Assembly>();

		// Token: 0x04000F31 RID: 3889
		private static bool globalResolverIsSet;
	}
}
