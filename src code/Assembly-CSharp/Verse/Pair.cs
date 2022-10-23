using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200053B RID: 1339
	public struct Pair<T1, T2> : IEquatable<Pair<T1, T2>>
	{
		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x06002932 RID: 10546 RVA: 0x00107980 File Offset: 0x00105B80
		public T1 First
		{
			get
			{
				return this.first;
			}
		}

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x06002933 RID: 10547 RVA: 0x00107988 File Offset: 0x00105B88
		public T2 Second
		{
			get
			{
				return this.second;
			}
		}

		// Token: 0x06002934 RID: 10548 RVA: 0x00107990 File Offset: 0x00105B90
		public Pair(T1 first, T2 second)
		{
			this.first = first;
			this.second = second;
		}

		// Token: 0x06002935 RID: 10549 RVA: 0x001079A0 File Offset: 0x00105BA0
		public override string ToString()
		{
			string[] array = new string[5];
			array[0] = "(";
			int num = 1;
			T1 t = this.First;
			array[num] = t.ToString();
			array[2] = ", ";
			int num2 = 3;
			T2 t2 = this.Second;
			array[num2] = t2.ToString();
			array[4] = ")";
			return string.Concat(array);
		}

		// Token: 0x06002936 RID: 10550 RVA: 0x001079FE File Offset: 0x00105BFE
		public override int GetHashCode()
		{
			return Gen.HashCombine<T2>(Gen.HashCombine<T1>(0, this.first), this.second);
		}

		// Token: 0x06002937 RID: 10551 RVA: 0x00107A17 File Offset: 0x00105C17
		public override bool Equals(object other)
		{
			return other is Pair<T1, T2> && this.Equals((Pair<T1, T2>)other);
		}

		// Token: 0x06002938 RID: 10552 RVA: 0x00107A2F File Offset: 0x00105C2F
		public bool Equals(Pair<T1, T2> other)
		{
			return EqualityComparer<T1>.Default.Equals(this.first, other.first) && EqualityComparer<T2>.Default.Equals(this.second, other.second);
		}

		// Token: 0x06002939 RID: 10553 RVA: 0x00107A61 File Offset: 0x00105C61
		public static bool operator ==(Pair<T1, T2> lhs, Pair<T1, T2> rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x0600293A RID: 10554 RVA: 0x00107A6B File Offset: 0x00105C6B
		public static bool operator !=(Pair<T1, T2> lhs, Pair<T1, T2> rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x04001AC1 RID: 6849
		private T1 first;

		// Token: 0x04001AC2 RID: 6850
		private T2 second;
	}
}
