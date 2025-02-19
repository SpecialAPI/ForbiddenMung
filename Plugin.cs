using BepInEx;
using Pentacle;
using Pentacle.Builders;
using Pentacle.CustomEvent;
using Pentacle.EffectorConditions;
using Pentacle.Tools;
using Pentacle.TriggerEffect;
using Pentacle.TriggerEffect.BasicTriggerEffects;
using BrutalAPI;
using System;

namespace ForbiddenMung
{
    [BepInPlugin(GUID, "Forbidden Mung", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public const string GUID = "SpecialAPI.ForbiddenMung";
        public const string PREFIX = "ForbiddenMung";

        public void Awake()
        {
            var m = ProfileManager.RegisterMod(GUID, PREFIX);

            var ffM = PassiveBuilder.NewPassive<MultiCustomTriggerEffectPassive>("ForbiddenFruit_Mung_PA", PassiveType_GameIDs.ForbiddenFruit.ToString())
                .SetBasicInformation("Forbidden Fruit", Passives.ForbiddenFruitInHerImage.passiveIcon)
                .SetEnemyDescription("If this enemy is left with a Mud Lung at the end of the turn they will mungle.");

            ffM.SetTriggerEffects(new()
            {
                new()
                {
                    trigger = CustomTriggers.GetForbiddenFruitID,
                    immediate = true,
                    doesPopup = false,

                    effect = new StringReferenceSetterTriggerEffect("Mung")
                },
                new()
                {
                    trigger = CustomTriggers.CanForbiddenFruitMatch,
                    immediate = true,
                    doesPopup = false,

                    effect = new SetForbiddenFruitCanMatchAndPassiveInformationTriggerEffect(),
                    
                    conditions = new()
                    {
                        ScriptableObjectTools.CreateScriptable<StringReferenceMatchesAnyEffectorCondition>(x => x.matchStrings = ["MudLung"])
                    }
                },
                new()
                {
                    trigger = CustomTriggers.TriggerForbiddenFruit,
                    immediate = false,
                    doesPopup = false,

                    effect = new PerformForbiddenFruitEffectsTriggerEffect()
                    {
                        selfDefaultEffects = new()
                        {
                            Effects.GenerateEffect(ScriptableObjectTools.CreateScriptable<RemovePassiveEffect>(x => x.m_PassiveID = PassiveType_GameIDs.Decay.ToString())),
                            Effects.GenerateEffect(ScriptableObjectTools.CreateScriptable<DirectDeathEffect>(), 0, Targeting.Slot_SelfSlot)
                        },
                        otherDefaultEffects = new()
                        {
                            Effects.GenerateEffect(ScriptableObjectTools.CreateScriptable<RemovePassiveEffect>(x => x.m_PassiveID = PassiveType_GameIDs.Decay.ToString())),
                            Effects.GenerateEffect(ScriptableObjectTools.CreateScriptable<DirectDeathEffect>(), 0, Targeting.Slot_SelfSlot),
                            Effects.GenerateEffect(ScriptableObjectTools.CreateScriptable<SpawnEnemyAnywhereEffect>(x =>
                            {
                                x.enemy = LoadedAssetsHandler.GetEnemy("MunglingMudLung_EN");
                                x.givesExperience = true;
                                x._spawnTypeID = CombatType_GameIDs.Spawn_Basic.ToString();
                            }), 1)
                        }
                    }
                }
            });

            var ffML = PassiveBuilder.NewPassive<MultiCustomTriggerEffectPassive>("ForbiddenFruit_MudLung_PA", PassiveType_GameIDs.ForbiddenFruit.ToString())
                .SetBasicInformation("Forbidden Fruit", Passives.ForbiddenFruitInHerImage.passiveIcon)
                .SetEnemyDescription("If this enemy is left with a Mung at the end of the turn they will mungle.");

            ffML.SetTriggerEffects(new()
            {
                new()
                {
                    trigger = CustomTriggers.GetForbiddenFruitID,
                    immediate = true,
                    doesPopup = false,

                    effect = new StringReferenceSetterTriggerEffect("MudLung")
                },
                new()
                {
                    trigger = CustomTriggers.CanForbiddenFruitMatch,
                    immediate = true,
                    doesPopup = false,

                    effect = new SetForbiddenFruitCanMatchAndPassiveInformationTriggerEffect(),
                    
                    conditions = new()
                    {
                        ScriptableObjectTools.CreateScriptable<StringReferenceMatchesAnyEffectorCondition>(x => x.matchStrings = ["Mung"])
                    }
                },
                new()
                {
                    trigger = CustomTriggers.TriggerForbiddenFruit,
                    immediate = false,
                    doesPopup = false,

                    effect = new PerformForbiddenFruitEffectsTriggerEffect()
                    {
                        selfDefaultEffects = new()
                        {
                            Effects.GenerateEffect(ScriptableObjectTools.CreateScriptable<RemovePassiveEffect>(x => x.m_PassiveID = PassiveType_GameIDs.Decay.ToString())),
                            Effects.GenerateEffect(ScriptableObjectTools.CreateScriptable<DirectDeathEffect>(), 0, Targeting.Slot_SelfSlot)
                        },
                        otherDefaultEffects = new()
                        {
                            Effects.GenerateEffect(ScriptableObjectTools.CreateScriptable<RemovePassiveEffect>(x => x.m_PassiveID = PassiveType_GameIDs.Decay.ToString())),
                            Effects.GenerateEffect(ScriptableObjectTools.CreateScriptable<DirectDeathEffect>(), 0, Targeting.Slot_SelfSlot),
                            Effects.GenerateEffect(ScriptableObjectTools.CreateScriptable<SpawnEnemyAnywhereEffect>(x =>
                            {
                                x.enemy = LoadedAssetsHandler.GetEnemy("MunglingMudLung_EN");
                                x.givesExperience = true;
                                x._spawnTypeID = CombatType_GameIDs.Spawn_Basic.ToString();
                            }), 1)
                        }
                    }
                }
            });

            LoadedAssetsHandler.GetEnemy("Mung_EN").passiveAbilities.Add(ffM);
            LoadedAssetsHandler.GetEnemy("MudLung_EN").passiveAbilities.Add(ffML);
        }
    }
}
