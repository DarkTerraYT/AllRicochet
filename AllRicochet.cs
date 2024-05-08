using MelonLoader;
using BTD_Mod_Helper;
using AllRicochet;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Unity;
using BTD_Mod_Helper.Api.ModOptions;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;

[assembly: MelonInfo(typeof(AllRicochet.AllRicochet), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace AllRicochet;

public class AllRicochet : BloonsTD6Mod
{
    public override void OnApplicationStart()
    {
        ModHelper.Msg<AllRicochet>("AllRicochet loaded!");
    }

    string[] BlockedIds = ["bombshooter", "mortar", "spikefactory"];

    public static void ApplyRichochet(AttackModel attackModel, RetargetOnContactModel model)
    {
        foreach(var proj in attackModel.GetDescendants<ProjectileModel>().ToList())
        {
            proj.AddBehavior(model);
        }
    }

    public static readonly ModSettingString TowerID = new("BoomerangMonkey-500")
    {
        displayName = "TowerID",
        description = "Which tower to get the ricochet from (format: NameWithoutSpaces-Crosspath)"
    };

    public static readonly ModSettingBool ApplyToAll = new(false)
    { 
        requiresRestart = true
    };


    public override void OnTitleScreen()
    {
        RetargetOnContactModel model = Game.instance.model.GetTowerFromId(TowerID).GetDescendant<RetargetOnContactModel>();

        foreach(var tower in Game.instance.model.towers)
        {
            foreach(var id in BlockedIds)
            {
                if (tower.baseId.ToLower().Contains(id) & ApplyToAll)
                {
                    return;
                }
            }

            foreach(var atck in tower.GetAttackModels())
            {
                ApplyRichochet(atck, model);
            }
            foreach(var abilityAtck in tower.GetBehaviors<ActivateAttackModel>())
            {
                foreach(var atck in abilityAtck.attacks)
                {
                    ApplyRichochet(atck, model);
                }
            }
        }
    }
}