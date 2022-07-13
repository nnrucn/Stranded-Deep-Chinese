using Beam;
using Beam.UI;
using Beam.Crafting;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mod.BedSaveTips
{
    public class BedSaveTips : SubPlugin
    {
        public override string Name => "使用床或者睡袋的存档保存提示";
        public override void Initialize()
        {
            base.Initialize();
        }

        private void SettingChanged(SettingChangedEventArgs args)
        {
            base.OnEnabled();
        }

        public override void OnEnabled()
        {
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            base.OnDisabled();
        }

        [HarmonyPatch(typeof(ConstructionObject_BED), nameof(ConstructionObject_BED.DoSave))]

        public class BedSaveTips_Patch
        {
            [HarmonyPostfix]
             static void Postfix()
            {
                LocalizedNotification localizedNotification = new LocalizedNotification(new Notification());
                localizedNotification.Priority = NotificationPriority.Immediate;
                localizedNotification.Duration = 2f;
                localizedNotification.TitleText.SetTerm("<size=23>存档</size>");
                localizedNotification.MessageText.SetTerm("<size=23>已保存</size>");
                localizedNotification.Raise();
            }
        }
    }
}
