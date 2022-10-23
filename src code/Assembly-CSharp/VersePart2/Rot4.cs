using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200002A RID: 42
	public struct Rot4 : IEquatable<Rot4>
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001AE RID: 430 RVA: 0x0000954E File Offset: 0x0000774E
		public bool IsValid
		{
			get
			{
				return this.rotInt < 100;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001AF RID: 431 RVA: 0x0000955A File Offset: 0x0000775A
		// (set) Token: 0x060001B0 RID: 432 RVA: 0x00009562 File Offset: 0x00007762
		public byte AsByte
		{
			get
			{
				return this.rotInt;
			}
			set
			{
				this.rotInt = (byte)GenMath.PositiveMod((int)value, 4);
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x0000955A File Offset: 0x0000775A
		// (set) Token: 0x060001B2 RID: 434 RVA: 0x00009562 File Offset: 0x00007762
		public int AsInt
		{
			get
			{
				return (int)this.rotInt;
			}
			set
			{
				this.rotInt = (byte)GenMath.PositiveMod(value, 4);
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x00009574 File Offset: 0x00007774
		public float AsAngle
		{
			get
			{
				switch (this.AsInt)
				{
				case 0:
					return 0f;
				case 1:
					return 90f;
				case 2:
					return 180f;
				case 3:
					return 270f;
				default:
					return 0f;
				}
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x000095C0 File Offset: 0x000077C0
		public SpectateRectSide AsSpectateSide
		{
			get
			{
				switch (this.AsInt)
				{
				case 0:
					return SpectateRectSide.Up;
				case 1:
					return SpectateRectSide.Right;
				case 2:
					return SpectateRectSide.Down;
				case 3:
					return SpectateRectSide.Left;
				default:
					return SpectateRectSide.None;
				}
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x000095F8 File Offset: 0x000077F8
		public Quaternion AsQuat
		{
			get
			{
				switch (this.rotInt)
				{
				case 0:
					return Quaternion.identity;
				case 1:
					return Quaternion.LookRotation(Vector3.right);
				case 2:
					return Quaternion.LookRotation(Vector3.back);
				case 3:
					return Quaternion.LookRotation(Vector3.left);
				default:
					Log.Error("ToQuat with Rot = " + this.AsInt);
					return Quaternion.identity;
				}
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x0000966C File Offset: 0x0000786C
		public Vector2 AsVector2
		{
			get
			{
				switch (this.rotInt)
				{
				case 0:
					return Vector2.up;
				case 1:
					return Vector2.right;
				case 2:
					return Vector2.down;
				case 3:
					return Vector2.left;
				default:
					throw new Exception("rotInt's value cannot be >3 but it is:" + this.rotInt);
				}
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x000096CA File Offset: 0x000078CA
		public bool IsHorizontal
		{
			get
			{
				return this.rotInt == 1 || this.rotInt == 3;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x000096E0 File Offset: 0x000078E0
		public static Rot4 Random
		{
			get
			{
				return new Rot4(Rand.RangeInclusive(0, 3));
			}
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00009562 File Offset: 0x00007762
		public Rot4(byte newRot)
		{
			this.rotInt = (byte)GenMath.PositiveMod((int)newRot, 4);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00009562 File Offset: 0x00007762
		public Rot4(int newRot)
		{
			this.rotInt = (byte)GenMath.PositiveMod(newRot, 4);
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060001BB RID: 443 RVA: 0x000096F0 File Offset: 0x000078F0
		public IntVec3 FacingCell
		{
			get
			{
				switch (this.AsInt)
				{
				case 0:
					return new IntVec3(0, 0, 1);
				case 1:
					return new IntVec3(1, 0, 0);
				case 2:
					return new IntVec3(0, 0, -1);
				case 3:
					return new IntVec3(-1, 0, 0);
				default:
					return default(IntVec3);
				}
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060001BC RID: 444 RVA: 0x0000974C File Offset: 0x0000794C
		public IntVec3 RighthandCell
		{
			get
			{
				switch (this.AsInt)
				{
				case 0:
					return new IntVec3(1, 0, 0);
				case 1:
					return new IntVec3(0, 0, -1);
				case 2:
					return new IntVec3(-1, 0, 0);
				case 3:
					return new IntVec3(0, 0, 1);
				default:
					return default(IntVec3);
				}
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060001BD RID: 445 RVA: 0x000097A8 File Offset: 0x000079A8
		public Rot4 Opposite
		{
			get
			{
				switch (this.AsInt)
				{
				case 0:
					return new Rot4(2);
				case 1:
					return new Rot4(3);
				case 2:
					return new Rot4(0);
				case 3:
					return new Rot4(1);
				default:
					return default(Rot4);
				}
			}
		}

		// Token: 0x060001BE RID: 446 RVA: 0x000097F9 File Offset: 0x000079F9
		public void Rotate(RotationDirection RotDir)
		{
			this.AsByte = (byte)(this.AsByte + RotDir);
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000980C File Offset: 0x00007A0C
		public Rot4 Rotated(RotationDirection RotDir)
		{
			Rot4 result = this;
			result.Rotate(RotDir);
			return result;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000982C File Offset: 0x00007A2C
		public static Rot4 FromAngleFlat(float angle)
		{
			angle = GenMath.PositiveMod(angle, 360f);
			if (angle < 45f)
			{
				return Rot4.North;
			}
			if (angle < 135f)
			{
				return Rot4.East;
			}
			if (angle < 225f)
			{
				return Rot4.South;
			}
			if (angle < 315f)
			{
				return Rot4.West;
			}
			return Rot4.North;
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00009884 File Offset: 0x00007A84
		public static Rot4 FromIntVec3(IntVec3 offset)
		{
			if (offset.x == 1)
			{
				return Rot4.East;
			}
			if (offset.x == -1)
			{
				return Rot4.West;
			}
			if (offset.z == 1)
			{
				return Rot4.North;
			}
			if (offset.z == -1)
			{
				return Rot4.South;
			}
			Log.Error("FromIntVec3 with bad offset " + offset);
			return Rot4.North;
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x000098E7 File Offset: 0x00007AE7
		public static Rot4 FromIntVec2(IntVec2 offset)
		{
			return Rot4.FromIntVec3(offset.ToIntVec3);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x000098F8 File Offset: 0x00007AF8
		public static RotationDirection GetRelativeRotation(Rot4 from, Rot4 to)
		{
			if (!from.IsValid || !to.IsValid)
			{
				Log.Error(string.Format("Both from ({0}) and to ({1}) must be valid.", from, to));
				return RotationDirection.None;
			}
			return (RotationDirection)GenMath.PositiveMod((int)(to.AsByte - from.AsByte), 4);
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000994A File Offset: 0x00007B4A
		public static bool operator ==(Rot4 a, Rot4 b)
		{
			return a.AsInt == b.AsInt;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000995C File Offset: 0x00007B5C
		public static bool operator !=(Rot4 a, Rot4 b)
		{
			return a.AsInt != b.AsInt;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00009974 File Offset: 0x00007B74
		public override int GetHashCode()
		{
			switch (this.rotInt)
			{
			case 0:
				return 235515;
			case 1:
				return 5612938;
			case 2:
				return 1215650;
			case 3:
				return 9231792;
			default:
				return (int)this.rotInt;
			}
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x000099BE File Offset: 0x00007BBE
		public override string ToString()
		{
			return this.rotInt.ToString();
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x000099CC File Offset: 0x00007BCC
		public string ToStringHuman()
		{
			switch (this.rotInt)
			{
			case 0:
				return "North".Translate();
			case 1:
				return "East".Translate();
			case 2:
				return "South".Translate();
			case 3:
				return "West".Translate();
			default:
				return "error";
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00009A40 File Offset: 0x00007C40
		public string ToStringWord()
		{
			switch (this.rotInt)
			{
			case 0:
				return "North";
			case 1:
				return "East";
			case 2:
				return "South";
			case 3:
				return "West";
			default:
				return "error";
			}
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00009A8C File Offset: 0x00007C8C
		public static Rot4 FromString(string str)
		{
			int num;
			byte newRot;
			if (int.TryParse(str, out num))
			{
				newRot = (byte)num;
			}
			else if (!(str == "North"))
			{
				if (!(str == "East"))
				{
					if (!(str == "South"))
					{
						if (!(str == "West"))
						{
							newRot = 0;
							Log.Error("Invalid rotation: " + str);
						}
						else
						{
							newRot = 3;
						}
					}
					else
					{
						newRot = 2;
					}
				}
				else
				{
					newRot = 1;
				}
			}
			else
			{
				newRot = 0;
			}
			return new Rot4(newRot);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00009B06 File Offset: 0x00007D06
		public override bool Equals(object obj)
		{
			return obj is Rot4 && this.Equals((Rot4)obj);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00009B1E File Offset: 0x00007D1E
		public bool Equals(Rot4 other)
		{
			return this.rotInt == other.rotInt;
		}

		// Token: 0x04000073 RID: 115
		public const int NorthInt = 0;

		// Token: 0x04000074 RID: 116
		public const int EastInt = 1;

		// Token: 0x04000075 RID: 117
		public const int SouthInt = 2;

		// Token: 0x04000076 RID: 118
		public const int WestInt = 3;

		// Token: 0x04000077 RID: 119
		public const int RotationCount = 4;

		// Token: 0x04000078 RID: 120
		private byte rotInt;

		// Token: 0x04000079 RID: 121
		public static readonly Rot4 North = new Rot4(0);

		// Token: 0x0400007A RID: 122
		public static readonly Rot4 East = new Rot4(1);

		// Token: 0x0400007B RID: 123
		public static readonly Rot4 South = new Rot4(2);

		// Token: 0x0400007C RID: 124
		public static readonly Rot4 West = new Rot4(3);

		// Token: 0x0400007D RID: 125
		public static readonly Rot4 Invalid = new Rot4
		{
			rotInt = 200
		};
	}
}
