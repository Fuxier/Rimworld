using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200017C RID: 380
	public static class GrammarResolverSimpleStringExtensions
	{
		// Token: 0x06000A5D RID: 2653 RVA: 0x000363E4 File Offset: 0x000345E4
		public static TaggedString Formatted(this string str, NamedArgument arg1)
		{
			GrammarResolverSimpleStringExtensions.argsLabels.Clear();
			GrammarResolverSimpleStringExtensions.argsObjects.Clear();
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg1.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg1.arg);
			return GrammarResolverSimple.Formatted(str, GrammarResolverSimpleStringExtensions.argsLabels, GrammarResolverSimpleStringExtensions.argsObjects);
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x0003643A File Offset: 0x0003463A
		public static TaggedString Formatted(this TaggedString str, NamedArgument arg1)
		{
			return str.RawText.Formatted(arg1);
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x0003644C File Offset: 0x0003464C
		public static TaggedString Formatted(this string str, NamedArgument arg1, NamedArgument arg2)
		{
			GrammarResolverSimpleStringExtensions.argsLabels.Clear();
			GrammarResolverSimpleStringExtensions.argsObjects.Clear();
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg1.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg1.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg2.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg2.arg);
			return GrammarResolverSimple.Formatted(str, GrammarResolverSimpleStringExtensions.argsLabels, GrammarResolverSimpleStringExtensions.argsObjects);
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x000364C2 File Offset: 0x000346C2
		public static TaggedString Formatted(this TaggedString str, NamedArgument arg1, NamedArgument arg2)
		{
			return str.RawText.Formatted(arg1, arg2);
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x000364D4 File Offset: 0x000346D4
		public static TaggedString Formatted(this string str, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			GrammarResolverSimpleStringExtensions.argsLabels.Clear();
			GrammarResolverSimpleStringExtensions.argsObjects.Clear();
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg1.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg1.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg2.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg2.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg3.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg3.arg);
			return GrammarResolverSimple.Formatted(str, GrammarResolverSimpleStringExtensions.argsLabels, GrammarResolverSimpleStringExtensions.argsObjects);
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x0003656A File Offset: 0x0003476A
		public static TaggedString Formatted(this TaggedString str, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			return str.RawText.Formatted(arg1, arg2, arg3);
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x0003657C File Offset: 0x0003477C
		public static TaggedString Formatted(this string str, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
		{
			GrammarResolverSimpleStringExtensions.argsLabels.Clear();
			GrammarResolverSimpleStringExtensions.argsObjects.Clear();
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg1.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg1.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg2.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg2.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg3.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg3.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg4.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg4.arg);
			return GrammarResolverSimple.Formatted(str, GrammarResolverSimpleStringExtensions.argsLabels, GrammarResolverSimpleStringExtensions.argsObjects);
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x00036634 File Offset: 0x00034834
		public static TaggedString Formatted(this TaggedString str, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
		{
			return str.RawText.Formatted(arg1, arg2, arg3, arg4);
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x00036648 File Offset: 0x00034848
		public static TaggedString Formatted(this string str, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5)
		{
			GrammarResolverSimpleStringExtensions.argsLabels.Clear();
			GrammarResolverSimpleStringExtensions.argsObjects.Clear();
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg1.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg1.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg2.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg2.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg3.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg3.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg4.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg4.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg5.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg5.arg);
			return GrammarResolverSimple.Formatted(str, GrammarResolverSimpleStringExtensions.argsLabels, GrammarResolverSimpleStringExtensions.argsObjects);
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x00036722 File Offset: 0x00034922
		public static TaggedString Formatted(this TaggedString str, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5)
		{
			return str.RawText.Formatted(arg1, arg2, arg3, arg4, arg5);
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x00036738 File Offset: 0x00034938
		public static TaggedString Formatted(this string str, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5, NamedArgument arg6)
		{
			GrammarResolverSimpleStringExtensions.argsLabels.Clear();
			GrammarResolverSimpleStringExtensions.argsObjects.Clear();
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg1.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg1.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg2.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg2.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg3.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg3.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg4.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg4.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg5.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg5.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg6.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg6.arg);
			return GrammarResolverSimple.Formatted(str, GrammarResolverSimpleStringExtensions.argsLabels, GrammarResolverSimpleStringExtensions.argsObjects);
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x00036834 File Offset: 0x00034A34
		public static TaggedString Formatted(this TaggedString str, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5, NamedArgument arg6)
		{
			return str.RawText.Formatted(arg1, arg2, arg3, arg4, arg5, arg6);
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x0003684C File Offset: 0x00034A4C
		public static TaggedString Formatted(this string str, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5, NamedArgument arg6, NamedArgument arg7)
		{
			GrammarResolverSimpleStringExtensions.argsLabels.Clear();
			GrammarResolverSimpleStringExtensions.argsObjects.Clear();
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg1.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg1.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg2.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg2.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg3.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg3.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg4.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg4.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg5.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg5.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg6.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg6.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg7.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg7.arg);
			return GrammarResolverSimple.Formatted(str, GrammarResolverSimpleStringExtensions.argsLabels, GrammarResolverSimpleStringExtensions.argsObjects);
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x0003696A File Offset: 0x00034B6A
		public static TaggedString Formatted(this TaggedString str, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5, NamedArgument arg6, NamedArgument arg7)
		{
			return str.RawText.Formatted(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x00036984 File Offset: 0x00034B84
		public static TaggedString Formatted(this string str, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5, NamedArgument arg6, NamedArgument arg7, NamedArgument arg8)
		{
			GrammarResolverSimpleStringExtensions.argsLabels.Clear();
			GrammarResolverSimpleStringExtensions.argsObjects.Clear();
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg1.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg1.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg2.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg2.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg3.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg3.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg4.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg4.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg5.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg5.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg6.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg6.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg7.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg7.arg);
			GrammarResolverSimpleStringExtensions.argsLabels.Add(arg8.label);
			GrammarResolverSimpleStringExtensions.argsObjects.Add(arg8.arg);
			return GrammarResolverSimple.Formatted(str, GrammarResolverSimpleStringExtensions.argsLabels, GrammarResolverSimpleStringExtensions.argsObjects);
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x00036AC4 File Offset: 0x00034CC4
		public static TaggedString Formatted(this TaggedString str, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5, NamedArgument arg6, NamedArgument arg7, NamedArgument arg8)
		{
			return str.RawText.Formatted(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x00036AEC File Offset: 0x00034CEC
		public static TaggedString Formatted(this string str, params NamedArgument[] args)
		{
			GrammarResolverSimpleStringExtensions.argsLabels.Clear();
			GrammarResolverSimpleStringExtensions.argsObjects.Clear();
			for (int i = 0; i < args.Length; i++)
			{
				GrammarResolverSimpleStringExtensions.argsLabels.Add(args[i].label);
				GrammarResolverSimpleStringExtensions.argsObjects.Add(args[i].arg);
			}
			return GrammarResolverSimple.Formatted(str, GrammarResolverSimpleStringExtensions.argsLabels, GrammarResolverSimpleStringExtensions.argsObjects);
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x00036B5C File Offset: 0x00034D5C
		public static TaggedString Formatted(this string str, IEnumerable<NamedArgument> args)
		{
			GrammarResolverSimpleStringExtensions.argsLabels.Clear();
			GrammarResolverSimpleStringExtensions.argsObjects.Clear();
			foreach (NamedArgument namedArgument in args)
			{
				GrammarResolverSimpleStringExtensions.argsLabels.Add(namedArgument.label);
				GrammarResolverSimpleStringExtensions.argsObjects.Add(namedArgument.arg);
			}
			return GrammarResolverSimple.Formatted(str, GrammarResolverSimpleStringExtensions.argsLabels, GrammarResolverSimpleStringExtensions.argsObjects);
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x00036BE8 File Offset: 0x00034DE8
		public static TaggedString Formatted(this TaggedString str, params NamedArgument[] args)
		{
			return str.RawText.Formatted(args);
		}

		// Token: 0x04000A5F RID: 2655
		private static List<string> argsLabels = new List<string>();

		// Token: 0x04000A60 RID: 2656
		private static List<object> argsObjects = new List<object>();
	}
}
