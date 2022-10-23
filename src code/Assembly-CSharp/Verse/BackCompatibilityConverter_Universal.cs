using System;
using System.Collections.Generic;
using System.Xml;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000511 RID: 1297
	public class BackCompatibilityConverter_Universal : BackCompatibilityConverter
	{
		// Token: 0x060027A6 RID: 10150 RVA: 0x00002662 File Offset: 0x00000862
		public override bool AppliesToVersion(int majorVer, int minorVer)
		{
			return true;
		}

		// Token: 0x060027A7 RID: 10151 RVA: 0x0010156C File Offset: 0x000FF76C
		public override string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null)
		{
			if (defType == typeof(ThingDef))
			{
				if (defName == "WoolYak")
				{
					return "WoolSheep";
				}
				if (defName == "Plant_TreeAnimus" || defName == "Plant_TreeAnimusSmall" || defName == "Plant_TreeAnimaSmall" || defName == "Plant_TreeAnimaNormal" || defName == "Plant_TreeAnimaHardy")
				{
					return "Plant_TreeAnima";
				}
				if (defName == "Psytrainer_EntropyLink")
				{
					return "Psytrainer_EntropyDump";
				}
				if (defName == "PsylinkNeuroformer")
				{
					return "PsychicAmplifier";
				}
				if (defName == "PsychicShockLance")
				{
					return "Apparel_PsychicShockLance";
				}
				if (defName == "PsychicInsanityLance")
				{
					return "Apparel_PsychicInsanityLance";
				}
				if (defName == "Nutrifungus")
				{
					return "Plant_Nutrifungus";
				}
				if (defName == "Mech_Centipede")
				{
					return "Mech_CentipedeBlaster";
				}
				if (defName == "Corpse_Mech_Centipede")
				{
					return "Corpse_Mech_CentipedeBlaster";
				}
				if (defName == "AncientDiabolusRemains")
				{
					return "AncientUltraDiabolusRemains";
				}
				if (defName == "AncientUltraDiabolusRemains")
				{
					return "AncientExostriderRemains";
				}
				if (defName == "MegaspiderCocoon")
				{
					return "CocoonMegaspider";
				}
				if (defName == "MegascarabCocoon")
				{
					return "CocoonMegascarab";
				}
				if (defName == "SpelopedeCocoon")
				{
					return "CocoonSpelopede";
				}
				if (defName == "AncientCentipedeShell")
				{
					return "AncientMechDetritus";
				}
				if (defName == "BasicSubcore")
				{
					return "SubcoreBasic";
				}
				if (defName == "RegularSubcore")
				{
					return "SubcoreRegular";
				}
				if (defName == "HighSubcore")
				{
					return "SubcoreHigh";
				}
				if (defName == "AncientMechanoidShell")
				{
					return "AncientMechDetritus";
				}
				if (defName == "XenogermExtractor")
				{
					return "GeneExtractor";
				}
				if (defName == "Mech_Purger")
				{
					return "Mech_Tunneler";
				}
				if (defName == "RemoteCharger")
				{
					return "MechBooster";
				}
				if (defName == "StandingLamp_Red")
				{
					return "StandingLamp";
				}
				if (defName == "StandingLamp_Green")
				{
					return "StandingLamp";
				}
				if (defName == "StandingLamp_Blue")
				{
					return "StandingLamp";
				}
				if (defName == "Darklamp")
				{
					return "StandingLamp";
				}
				if (defName == "MechanitorComplexMap")
				{
					return "MechanoidTransponder";
				}
			}
			if (defType == typeof(HediffDef))
			{
				if (defName == "Psylink")
				{
					return "PsychicAmplifier";
				}
				if (defName == "RemoteCharge")
				{
					return "MechBoost";
				}
			}
			if (defType == typeof(PreceptDef) && defName == "FuneralDestroyed")
			{
				return "FuneralNoCorpse";
			}
			if (defType == typeof(RitualOutcomeEffectDef) && defName == "AttendedFuneralDestroyed")
			{
				return "AttendedFuneralNoCorpse";
			}
			if (defType == typeof(AbilityDef))
			{
				if (defName == "PreachingOfHealing")
				{
					return "PreachHealth";
				}
				if (defName == "HeartenHealth")
				{
					return "PreachHealth";
				}
			}
			if (defType == typeof(IdeoIconDef))
			{
				if (defName == "PoliticalA")
				{
					return "Eagle";
				}
				if (defName == "NatureA")
				{
					return "Treeflat";
				}
				if (defName == "PirateA")
				{
					return "Steer";
				}
				if (defName == "PirateB")
				{
					return "Skull";
				}
				if (defName == "PoliticalB")
				{
					return "OliveBranches";
				}
				if (defName == "ReligionA")
				{
					return "DownBurst";
				}
				if (defName == "ReligionB")
				{
					return "TripleCross";
				}
			}
			if (defType == typeof(PawnKindDef))
			{
				if (defName == "Mech_Centipede")
				{
					return "Mech_CentipedeBlaster";
				}
				if (defName == "Mech_Purger")
				{
					return "Mech_Tunneler";
				}
			}
			if (defType == typeof(ThoughtDef))
			{
				if (defName == "AteFungus_Prefered")
				{
					return "AteFungus_Preferred";
				}
				if (defName == "AteFungusAsIngredient_Prefered")
				{
					return "AteFungusAsIngredient_Preferred";
				}
			}
			if (defType == typeof(JobDef) && defName == "StudyThing")
			{
				return "StudyBuilding";
			}
			if (defType == typeof(ThingStyleDef))
			{
				if (defName.EndsWith("StandingLamp_Red") || defName.EndsWith("StandingLamp_Green") || defName.EndsWith("StandingLamp_Blue"))
				{
					return defName.Substring(0, defName.IndexOf("StandingLamp") + "StandingLamp".Length);
				}
				if (defName.EndsWith("DarklampStanding"))
				{
					return defName.Substring(0, defName.IndexOf("DarklampStanding")) + "StandingLamp";
				}
			}
			return null;
		}

		// Token: 0x060027A8 RID: 10152 RVA: 0x00101A30 File Offset: 0x000FFC30
		public override Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node)
		{
			if (providedClassName == "Hediff_PsychicAmplifier")
			{
				return typeof(Hediff_Psylink);
			}
			if (providedClassName == "Graphic_MotePulse")
			{
				return typeof(Graphic_MoteWithAgeSecs);
			}
			if (node != null && (providedClassName == "ThingWithComps" || providedClassName == "Verse.ThingWithComps"))
			{
				XmlElement xmlElement = node["def"];
				if (xmlElement != null)
				{
					if (xmlElement.InnerText == "PsychicShockLance")
					{
						return typeof(Apparel);
					}
					if (xmlElement.InnerText == "PsychicInsanityLance")
					{
						return typeof(Apparel);
					}
					if (xmlElement.InnerText == "OrbitalTargeterBombardment")
					{
						return typeof(Apparel);
					}
					if (xmlElement.InnerText == "OrbitalTargeterPowerBeam")
					{
						return typeof(Apparel);
					}
					if (xmlElement.InnerText == "OrbitalTargeterMechCluster")
					{
						return typeof(Apparel);
					}
					if (xmlElement.InnerText == "TornadoGenerator")
					{
						return typeof(Apparel);
					}
				}
			}
			if (providedClassName == "Building_AncientUltraDiabolusRemains")
			{
				return typeof(Building_AncientMechRemains);
			}
			return null;
		}

		// Token: 0x060027A9 RID: 10153 RVA: 0x00101B6C File Offset: 0x000FFD6C
		public override void PostExposeData(object obj)
		{
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				int loadedGameVersionBuild = ScribeMetaHeaderUtility.loadedGameVersionBuild;
				Pawn_RoyaltyTracker pawn_RoyaltyTracker;
				if ((pawn_RoyaltyTracker = (obj as Pawn_RoyaltyTracker)) != null && loadedGameVersionBuild <= 2575)
				{
					foreach (RoyalTitle royalTitle in pawn_RoyaltyTracker.AllTitlesForReading)
					{
						royalTitle.conceited = RoyalTitleUtility.ShouldBecomeConceitedOnNewTitle(pawn_RoyaltyTracker.pawn);
					}
				}
				if (loadedGameVersionBuild < 3167)
				{
					this.MealRestrictionsReworkBackCompat(obj);
				}
				if (loadedGameVersionBuild < 3156)
				{
					this.BiosculpterReworkBackCompat(obj);
				}
				this.ApplyLampColor(obj);
				Pawn_NeedsTracker pawn_NeedsTracker;
				if ((pawn_NeedsTracker = (obj as Pawn_NeedsTracker)) != null)
				{
					pawn_NeedsTracker.AllNeeds.RemoveAll((Need n) => n.def.defName == "Authority");
				}
				History history;
				if ((history = (obj as History)) != null && history.historyEventsManager == null)
				{
					history.historyEventsManager = new HistoryEventsManager();
				}
			}
			Pawn pawn;
			if ((pawn = (obj as Pawn)) != null)
			{
				if (pawn.abilities == null)
				{
					pawn.abilities = new Pawn_AbilityTracker(pawn);
				}
				Ability ability = pawn.abilities.abilities.FirstOrFallback((Ability x) => x.def.defName == "AnimaTreeLinking", null);
				if (ability != null)
				{
					pawn.abilities.RemoveAbility(ability.def);
				}
				if (pawn.RaceProps.Humanlike)
				{
					if (pawn.surroundings == null)
					{
						pawn.surroundings = new Pawn_SurroundingsTracker(pawn);
					}
					if (pawn.styleObserver == null)
					{
						pawn.styleObserver = new Pawn_StyleObserverTracker(pawn);
					}
					if (pawn.connections == null)
					{
						pawn.connections = new Pawn_ConnectionsTracker(pawn);
					}
				}
				if (pawn.health != null)
				{
					if (pawn.health.hediffSet.hediffs.RemoveAll((Hediff x) => x == null) != 0)
					{
						Log.Error(pawn.ToStringSafe<Pawn>() + " had some null hediffs.");
					}
					Pawn_HealthTracker health = pawn.health;
					Hediff hediff;
					if (health == null)
					{
						hediff = null;
					}
					else
					{
						HediffSet hediffSet = health.hediffSet;
						hediff = ((hediffSet != null) ? hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicHangover, false) : null);
					}
					Hediff hediff2 = hediff;
					if (hediff2 != null)
					{
						pawn.health.hediffSet.hediffs.Remove(hediff2);
					}
					Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.WakeUpTolerance, false);
					if (firstHediffOfDef != null)
					{
						pawn.health.hediffSet.hediffs.Remove(firstHediffOfDef);
					}
					Hediff firstHediffOfDef2 = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.GoJuiceTolerance, false);
					if (firstHediffOfDef2 != null)
					{
						pawn.health.hediffSet.hediffs.Remove(firstHediffOfDef2);
					}
					if (pawn.mechanitor != null)
					{
						Hediff firstHediffOfDef3 = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BandNode, false);
						if (firstHediffOfDef3 != null && !(firstHediffOfDef3 is Hediff_BandNode))
						{
							pawn.health.RemoveHediff(firstHediffOfDef3);
							pawn.health.AddHediff(HediffDefOf.BandNode, pawn.health.hediffSet.GetBrain(), null, null);
						}
					}
					if (!pawn.Dead)
					{
						if (pawn.thinker == null)
						{
							pawn.thinker = new Pawn_Thinker(pawn);
						}
						if (pawn.jobs == null)
						{
							pawn.jobs = new Pawn_JobTracker(pawn);
						}
						if (pawn.stances == null)
						{
							pawn.stances = new Pawn_StanceTracker(pawn);
						}
					}
				}
				if (pawn.equipment != null && pawn.apparel != null && pawn.inventory != null)
				{
					List<ThingWithComps> list = null;
					for (int i = 0; i < pawn.equipment.AllEquipmentListForReading.Count; i++)
					{
						ThingWithComps thingWithComps = pawn.equipment.AllEquipmentListForReading[i];
						if (thingWithComps.def.defName == "OrbitalTargeterBombardment" || thingWithComps.def.defName == "OrbitalTargeterPowerBeam" || thingWithComps.def.defName == "OrbitalTargeterMechCluster" || thingWithComps.def.defName == "TornadoGenerator")
						{
							list = (list ?? new List<ThingWithComps>());
							list.Add(thingWithComps);
						}
					}
					if (list != null)
					{
						foreach (ThingWithComps thingWithComps2 in list)
						{
							Apparel apparel = (Apparel)thingWithComps2;
							pawn.equipment.Remove(apparel);
							this.ResetVerbs(apparel);
							if (pawn.apparel.CanWearWithoutDroppingAnything(apparel.def))
							{
								pawn.apparel.Wear(apparel, true, false);
							}
							else
							{
								pawn.inventory.innerContainer.TryAdd(apparel, true);
							}
						}
					}
				}
				if (pawn.RaceProps.IsMechanoid && pawn.Faction == Faction.OfPlayer && pawn.Name == null && Scribe.mode == LoadSaveMode.PostLoadInit)
				{
					pawn.GenerateNecessaryName();
					return;
				}
			}
			else if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.SaveLampColor(obj);
				Map map;
				Game game;
				if ((map = (obj as Map)) != null)
				{
					if (map.temporaryThingDrawer == null)
					{
						map.temporaryThingDrawer = new TemporaryThingDrawer();
					}
					if (map.flecks == null)
					{
						map.flecks = new FleckManager(map);
					}
					if (map.autoSlaughterManager == null)
					{
						map.autoSlaughterManager = new AutoSlaughterManager(map);
					}
					if (map.treeDestructionTracker == null)
					{
						map.treeDestructionTracker = new TreeDestructionTracker(map);
					}
					if (map.gasGrid == null)
					{
						map.gasGrid = new GasGrid(map);
					}
					if (map.pollutionGrid == null)
					{
						map.pollutionGrid = new PollutionGrid(map);
					}
					if (map.storageGroups == null)
					{
						map.storageGroups = new StorageGroupManager(map);
					}
					if (map.terrainGrid.colorGrid == null)
					{
						map.terrainGrid.colorGrid = new ColorDef[map.cellIndices.NumGridCells];
						return;
					}
				}
				else if ((game = (obj as Game)) != null)
				{
					if (game.transportShipManager == null)
					{
						game.transportShipManager = new TransportShipManager();
					}
					if (game.studyManager == null)
					{
						game.studyManager = new StudyManager();
					}
					if (ModsConfig.BiotechActive && game.customXenogermDatabase == null)
					{
						game.customXenogermDatabase = new CustomXenogermDatabase();
					}
				}
			}
		}

		// Token: 0x060027AA RID: 10154 RVA: 0x001021A0 File Offset: 0x001003A0
		private void ResetVerbs(ThingWithComps t)
		{
			IVerbOwner verbOwner = t as IVerbOwner;
			if (verbOwner != null)
			{
				VerbTracker verbTracker = verbOwner.VerbTracker;
				if (verbTracker != null)
				{
					verbTracker.VerbsNeedReinitOnLoad();
				}
			}
			foreach (ThingComp thingComp in t.AllComps)
			{
				IVerbOwner verbOwner2 = thingComp as IVerbOwner;
				if (verbOwner2 != null)
				{
					VerbTracker verbTracker2 = verbOwner2.VerbTracker;
					if (verbTracker2 != null)
					{
						verbTracker2.VerbsNeedReinitOnLoad();
					}
				}
			}
		}

		// Token: 0x060027AB RID: 10155 RVA: 0x00102224 File Offset: 0x00100424
		public override int GetBackCompatibleBodyPartIndex(BodyDef body, int index)
		{
			if (body == BodyDefOf.Human && ScribeMetaHeaderUtility.loadedGameVersionBuild <= 3094 && index >= 22)
			{
				return index + 1;
			}
			return index;
		}

		// Token: 0x060027AC RID: 10156 RVA: 0x00102244 File Offset: 0x00100444
		private void MealRestrictionsReworkBackCompat(object obj)
		{
			FoodRestrictionDatabase foodRestrictionDatabase;
			if ((foodRestrictionDatabase = (obj as FoodRestrictionDatabase)) != null)
			{
				foodRestrictionDatabase.CreateIdeologyFoodRestrictions();
			}
		}

		// Token: 0x060027AD RID: 10157 RVA: 0x00102264 File Offset: 0x00100464
		private void BiosculpterReworkBackCompat(object obj)
		{
			JobDriver_CarryToBiosculpterPod jobDriver_CarryToBiosculpterPod;
			if ((jobDriver_CarryToBiosculpterPod = (obj as JobDriver_CarryToBiosculpterPod)) != null)
			{
				jobDriver_CarryToBiosculpterPod.EndJobWith(JobCondition.Incompletable);
			}
			JobDriver_EnterBiosculpterPod jobDriver_EnterBiosculpterPod;
			if ((jobDriver_EnterBiosculpterPod = (obj as JobDriver_EnterBiosculpterPod)) != null)
			{
				jobDriver_EnterBiosculpterPod.EndJobWith(JobCondition.Incompletable);
			}
			Building thing;
			if ((thing = (obj as Building)) != null)
			{
				CompBiosculpterPod compBiosculpterPod = thing.TryGetComp<CompBiosculpterPod>();
				if (compBiosculpterPod != null)
				{
					if (compBiosculpterPod.Occupant == null)
					{
						compBiosculpterPod.ClearCycle();
					}
					compBiosculpterPod.autoLoadNutrition = true;
				}
			}
		}

		// Token: 0x060027AE RID: 10158 RVA: 0x001022BC File Offset: 0x001004BC
		private void SaveLampColor(object obj)
		{
			Building key;
			if ((key = (obj as Building)) == null)
			{
				return;
			}
			ColorInt? colorInt = null;
			ScribeLoader loader = Scribe.loader;
			string text;
			if (loader == null)
			{
				text = null;
			}
			else
			{
				XmlNode curXmlParent = loader.curXmlParent;
				if (curXmlParent == null)
				{
					text = null;
				}
				else
				{
					XmlElement xmlElement = curXmlParent["def"];
					text = ((xmlElement != null) ? xmlElement.InnerText : null);
				}
			}
			string a = text;
			if (a == "StandingLamp_Red")
			{
				colorInt = new ColorInt?(new ColorInt(217, 80, 80));
			}
			else if (a == "StandingLamp_Green")
			{
				colorInt = new ColorInt?(new ColorInt(80, 217, 80));
			}
			else if (a == "StandingLamp_Blue")
			{
				colorInt = new ColorInt?(new ColorInt(80, 80, 217));
			}
			else if (a == "Darklamp")
			{
				colorInt = new ColorInt?(new ColorInt(78, 226, 229));
			}
			if (colorInt != null)
			{
				this.lampsToColors[key] = colorInt.Value;
			}
		}

		// Token: 0x060027AF RID: 10159 RVA: 0x001023B8 File Offset: 0x001005B8
		private void ApplyLampColor(object obj)
		{
			Building building;
			if ((building = (obj as Building)) == null)
			{
				return;
			}
			CompGlower comp;
			if ((comp = building.GetComp<CompGlower>()) == null)
			{
				return;
			}
			ColorInt colorInt;
			if (!this.lampsToColors.TryGetValue(building, out colorInt))
			{
				return;
			}
			float h;
			float s;
			float num;
			Color.RGBToHSV(colorInt.ToColor, out h, out s, out num);
			float num2;
			float v;
			Color.RGBToHSV(comp.GlowColor.ToColor, out num, out num2, out v);
			comp.GlowColor = new ColorInt(Color.HSVToRGB(h, s, v));
		}

		// Token: 0x060027B0 RID: 10160 RVA: 0x0010242D File Offset: 0x0010062D
		public override void PreLoadSavegame(string loadingVersion)
		{
			this.lampsToColors.Clear();
		}

		// Token: 0x04001A00 RID: 6656
		private Dictionary<Building, ColorInt> lampsToColors = new Dictionary<Building, ColorInt>(128);
	}
}
