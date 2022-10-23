using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000525 RID: 1317
	public struct DamageInfo
	{
		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x06002806 RID: 10246 RVA: 0x001042BC File Offset: 0x001024BC
		// (set) Token: 0x06002807 RID: 10247 RVA: 0x001042C4 File Offset: 0x001024C4
		public DamageDef Def
		{
			get
			{
				return this.defInt;
			}
			set
			{
				this.defInt = value;
			}
		}

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x06002808 RID: 10248 RVA: 0x001042CD File Offset: 0x001024CD
		public float Amount
		{
			get
			{
				if (!DebugSettings.enableDamage)
				{
					return 0f;
				}
				return this.amountInt;
			}
		}

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x06002809 RID: 10249 RVA: 0x001042E2 File Offset: 0x001024E2
		public float ArmorPenetrationInt
		{
			get
			{
				return this.armorPenetrationInt;
			}
		}

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x0600280A RID: 10250 RVA: 0x001042EA File Offset: 0x001024EA
		public Thing Instigator
		{
			get
			{
				return this.instigatorInt;
			}
		}

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x0600280B RID: 10251 RVA: 0x001042F2 File Offset: 0x001024F2
		public DamageInfo.SourceCategory Category
		{
			get
			{
				return this.categoryInt;
			}
		}

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x0600280C RID: 10252 RVA: 0x001042FA File Offset: 0x001024FA
		public Thing IntendedTarget
		{
			get
			{
				return this.intendedTargetInt;
			}
		}

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x0600280D RID: 10253 RVA: 0x00104302 File Offset: 0x00102502
		public float Angle
		{
			get
			{
				return this.angleInt;
			}
		}

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x0600280E RID: 10254 RVA: 0x0010430A File Offset: 0x0010250A
		public BodyPartRecord HitPart
		{
			get
			{
				return this.hitPartInt;
			}
		}

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x0600280F RID: 10255 RVA: 0x00104312 File Offset: 0x00102512
		public BodyPartHeight Height
		{
			get
			{
				return this.heightInt;
			}
		}

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x06002810 RID: 10256 RVA: 0x0010431A File Offset: 0x0010251A
		public BodyPartDepth Depth
		{
			get
			{
				return this.depthInt;
			}
		}

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x06002811 RID: 10257 RVA: 0x00104322 File Offset: 0x00102522
		public ThingDef Weapon
		{
			get
			{
				return this.weaponInt;
			}
		}

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x06002812 RID: 10258 RVA: 0x0010432A File Offset: 0x0010252A
		public BodyPartGroupDef WeaponBodyPartGroup
		{
			get
			{
				return this.weaponBodyPartGroupInt;
			}
		}

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x06002813 RID: 10259 RVA: 0x00104332 File Offset: 0x00102532
		public HediffDef WeaponLinkedHediff
		{
			get
			{
				return this.weaponHediffInt;
			}
		}

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x06002814 RID: 10260 RVA: 0x0010433A File Offset: 0x0010253A
		public bool InstantPermanentInjury
		{
			get
			{
				return this.instantPermanentInjuryInt;
			}
		}

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x06002815 RID: 10261 RVA: 0x00104342 File Offset: 0x00102542
		public bool InstigatorGuilty
		{
			get
			{
				return this.instigatorGuilty;
			}
		}

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x06002816 RID: 10262 RVA: 0x0010434A File Offset: 0x0010254A
		public bool SpawnFilth
		{
			get
			{
				return this.spawnFilth;
			}
		}

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x06002817 RID: 10263 RVA: 0x00104352 File Offset: 0x00102552
		public bool AllowDamagePropagation
		{
			get
			{
				return !this.InstantPermanentInjury && this.allowDamagePropagationInt;
			}
		}

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x06002818 RID: 10264 RVA: 0x00104364 File Offset: 0x00102564
		public bool IgnoreArmor
		{
			get
			{
				return this.ignoreArmorInt;
			}
		}

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x06002819 RID: 10265 RVA: 0x0010436C File Offset: 0x0010256C
		public bool IgnoreInstantKillProtection
		{
			get
			{
				return this.ignoreInstantKillProtectionInt;
			}
		}

		// Token: 0x0600281A RID: 10266 RVA: 0x00104374 File Offset: 0x00102574
		public DamageInfo(DamageDef def, float amount, float armorPenetration = 0f, float angle = -1f, Thing instigator = null, BodyPartRecord hitPart = null, ThingDef weapon = null, DamageInfo.SourceCategory category = DamageInfo.SourceCategory.ThingOrUnknown, Thing intendedTarget = null, bool instigatorGuilty = true, bool spawnFilth = true)
		{
			this.defInt = def;
			this.amountInt = amount;
			this.armorPenetrationInt = armorPenetration;
			if (angle < 0f)
			{
				this.angleInt = (float)Rand.RangeInclusive(0, 359);
			}
			else
			{
				this.angleInt = angle;
			}
			this.instigatorInt = instigator;
			this.categoryInt = category;
			this.hitPartInt = hitPart;
			this.heightInt = BodyPartHeight.Undefined;
			this.depthInt = BodyPartDepth.Undefined;
			this.weaponInt = weapon;
			this.weaponBodyPartGroupInt = null;
			this.weaponHediffInt = null;
			this.instantPermanentInjuryInt = false;
			this.allowDamagePropagationInt = true;
			this.ignoreArmorInt = false;
			this.ignoreInstantKillProtectionInt = false;
			this.instigatorGuilty = instigatorGuilty;
			this.intendedTargetInt = intendedTarget;
			this.spawnFilth = spawnFilth;
		}

		// Token: 0x0600281B RID: 10267 RVA: 0x0010442C File Offset: 0x0010262C
		public DamageInfo(DamageInfo cloneSource)
		{
			this.defInt = cloneSource.defInt;
			this.amountInt = cloneSource.amountInt;
			this.armorPenetrationInt = cloneSource.armorPenetrationInt;
			this.angleInt = cloneSource.angleInt;
			this.instigatorInt = cloneSource.instigatorInt;
			this.categoryInt = cloneSource.categoryInt;
			this.hitPartInt = cloneSource.hitPartInt;
			this.heightInt = cloneSource.heightInt;
			this.depthInt = cloneSource.depthInt;
			this.weaponInt = cloneSource.weaponInt;
			this.weaponBodyPartGroupInt = cloneSource.weaponBodyPartGroupInt;
			this.weaponHediffInt = cloneSource.weaponHediffInt;
			this.instantPermanentInjuryInt = cloneSource.instantPermanentInjuryInt;
			this.allowDamagePropagationInt = cloneSource.allowDamagePropagationInt;
			this.intendedTargetInt = cloneSource.intendedTargetInt;
			this.ignoreArmorInt = cloneSource.ignoreArmorInt;
			this.ignoreInstantKillProtectionInt = cloneSource.ignoreInstantKillProtectionInt;
			this.instigatorGuilty = cloneSource.instigatorGuilty;
			this.spawnFilth = cloneSource.spawnFilth;
		}

		// Token: 0x0600281C RID: 10268 RVA: 0x0010451D File Offset: 0x0010271D
		public void SetAmount(float newAmount)
		{
			this.amountInt = newAmount;
		}

		// Token: 0x0600281D RID: 10269 RVA: 0x00104526 File Offset: 0x00102726
		public void SetIgnoreArmor(bool ignoreArmor)
		{
			this.ignoreArmorInt = ignoreArmor;
		}

		// Token: 0x0600281E RID: 10270 RVA: 0x0010452F File Offset: 0x0010272F
		public void SetIgnoreInstantKillProtection(bool ignore)
		{
			this.ignoreInstantKillProtectionInt = ignore;
		}

		// Token: 0x0600281F RID: 10271 RVA: 0x00104538 File Offset: 0x00102738
		public void SetBodyRegion(BodyPartHeight height = BodyPartHeight.Undefined, BodyPartDepth depth = BodyPartDepth.Undefined)
		{
			this.heightInt = height;
			this.depthInt = depth;
		}

		// Token: 0x06002820 RID: 10272 RVA: 0x00104548 File Offset: 0x00102748
		public void SetHitPart(BodyPartRecord forceHitPart)
		{
			this.hitPartInt = forceHitPart;
		}

		// Token: 0x06002821 RID: 10273 RVA: 0x00104551 File Offset: 0x00102751
		public void SetInstantPermanentInjury(bool val)
		{
			this.instantPermanentInjuryInt = val;
		}

		// Token: 0x06002822 RID: 10274 RVA: 0x0010455A File Offset: 0x0010275A
		public void SetWeaponBodyPartGroup(BodyPartGroupDef gr)
		{
			this.weaponBodyPartGroupInt = gr;
		}

		// Token: 0x06002823 RID: 10275 RVA: 0x00104563 File Offset: 0x00102763
		public void SetWeaponHediff(HediffDef hd)
		{
			this.weaponHediffInt = hd;
		}

		// Token: 0x06002824 RID: 10276 RVA: 0x0010456C File Offset: 0x0010276C
		public void SetAllowDamagePropagation(bool val)
		{
			this.allowDamagePropagationInt = val;
		}

		// Token: 0x06002825 RID: 10277 RVA: 0x00104578 File Offset: 0x00102778
		public void SetAngle(Vector3 vec)
		{
			if (vec.x != 0f || vec.z != 0f)
			{
				this.angleInt = Quaternion.LookRotation(vec).eulerAngles.y;
				return;
			}
			this.angleInt = (float)Rand.RangeInclusive(0, 359);
		}

		// Token: 0x06002826 RID: 10278 RVA: 0x001045CC File Offset: 0x001027CC
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(def=",
				this.defInt,
				", amount= ",
				this.amountInt,
				", instigator=",
				(this.instigatorInt != null) ? this.instigatorInt.ToString() : this.categoryInt.ToString(),
				", angle=",
				this.angleInt.ToString("F1"),
				")"
			});
		}

		// Token: 0x04001A6E RID: 6766
		private DamageDef defInt;

		// Token: 0x04001A6F RID: 6767
		private float amountInt;

		// Token: 0x04001A70 RID: 6768
		private float armorPenetrationInt;

		// Token: 0x04001A71 RID: 6769
		private float angleInt;

		// Token: 0x04001A72 RID: 6770
		private Thing instigatorInt;

		// Token: 0x04001A73 RID: 6771
		private DamageInfo.SourceCategory categoryInt;

		// Token: 0x04001A74 RID: 6772
		public Thing intendedTargetInt;

		// Token: 0x04001A75 RID: 6773
		private bool ignoreArmorInt;

		// Token: 0x04001A76 RID: 6774
		private bool ignoreInstantKillProtectionInt;

		// Token: 0x04001A77 RID: 6775
		private BodyPartRecord hitPartInt;

		// Token: 0x04001A78 RID: 6776
		private BodyPartHeight heightInt;

		// Token: 0x04001A79 RID: 6777
		private BodyPartDepth depthInt;

		// Token: 0x04001A7A RID: 6778
		private ThingDef weaponInt;

		// Token: 0x04001A7B RID: 6779
		private BodyPartGroupDef weaponBodyPartGroupInt;

		// Token: 0x04001A7C RID: 6780
		private HediffDef weaponHediffInt;

		// Token: 0x04001A7D RID: 6781
		private bool instantPermanentInjuryInt;

		// Token: 0x04001A7E RID: 6782
		private bool allowDamagePropagationInt;

		// Token: 0x04001A7F RID: 6783
		private bool instigatorGuilty;

		// Token: 0x04001A80 RID: 6784
		private bool spawnFilth;

		// Token: 0x020020F3 RID: 8435
		public enum SourceCategory
		{
			// Token: 0x040082AF RID: 33455
			ThingOrUnknown,
			// Token: 0x040082B0 RID: 33456
			Collapse
		}
	}
}
