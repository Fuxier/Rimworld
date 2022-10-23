using System;
using System.Text;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x0200017D RID: 381
	public struct NamedArgument
	{
		// Token: 0x06000A71 RID: 2673 RVA: 0x00036C0D File Offset: 0x00034E0D
		public NamedArgument(object arg, string label)
		{
			this.arg = arg;
			this.label = label;
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x00036C1D File Offset: 0x00034E1D
		public static implicit operator NamedArgument(int value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x00036C2B File Offset: 0x00034E2B
		public static implicit operator NamedArgument(char value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x00036C39 File Offset: 0x00034E39
		public static implicit operator NamedArgument(float value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x00036C47 File Offset: 0x00034E47
		public static implicit operator NamedArgument(double value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x00036C55 File Offset: 0x00034E55
		public static implicit operator NamedArgument(long value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x00036C63 File Offset: 0x00034E63
		public static implicit operator NamedArgument(string value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x00036C6C File Offset: 0x00034E6C
		public static implicit operator NamedArgument(uint value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x00036C7A File Offset: 0x00034E7A
		public static implicit operator NamedArgument(byte value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x00036C88 File Offset: 0x00034E88
		public static implicit operator NamedArgument(ulong value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x00036C63 File Offset: 0x00034E63
		public static implicit operator NamedArgument(StringBuilder value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x00036C63 File Offset: 0x00034E63
		public static implicit operator NamedArgument(Thing value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x00036C63 File Offset: 0x00034E63
		public static implicit operator NamedArgument(Def value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x00036C63 File Offset: 0x00034E63
		public static implicit operator NamedArgument(WorldObject value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x00036C63 File Offset: 0x00034E63
		public static implicit operator NamedArgument(Faction value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x00036C96 File Offset: 0x00034E96
		public static implicit operator NamedArgument(IntVec3 value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x00036CA4 File Offset: 0x00034EA4
		public static implicit operator NamedArgument(LocalTargetInfo value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x00036CB2 File Offset: 0x00034EB2
		public static implicit operator NamedArgument(TargetInfo value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x00036CC0 File Offset: 0x00034EC0
		public static implicit operator NamedArgument(GlobalTargetInfo value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x00036C63 File Offset: 0x00034E63
		public static implicit operator NamedArgument(Map value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x00036CCE File Offset: 0x00034ECE
		public static implicit operator NamedArgument(TaggedString value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x00036C63 File Offset: 0x00034E63
		public static implicit operator NamedArgument(Ideo value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x00036CDC File Offset: 0x00034EDC
		public override string ToString()
		{
			return (this.label.NullOrEmpty() ? "unnamed" : this.label) + "->" + this.arg.ToStringSafe<object>();
		}

		// Token: 0x04000A61 RID: 2657
		public object arg;

		// Token: 0x04000A62 RID: 2658
		public string label;
	}
}
