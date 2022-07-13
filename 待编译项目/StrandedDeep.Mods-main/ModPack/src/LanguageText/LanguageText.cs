using Beam;
using Beam.Language;
using Beam.AccountServices;
using UnityEngine;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steamworks;


namespace Mod.LanguageText
{
    public class LanguageText : SubPlugin
    {
        public override string Name => "语言文本读取";
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

        [HarmonyPatch(typeof(LocalizationHandler), nameof(LocalizationHandler.LoadDefaultAsset))]

        public class LanguageText_Patch
        {
            [HarmonyPrefix]
            static bool Prefix(ref string path, ref string[] __result)
            {
			AssetBundle assetBundle = AssetBundle.LoadFromFile(Application.dataPath + "/BepInEx//Fonts//chinese_text");
			TextAsset textAsset = assetBundle.LoadAsset<TextAsset>(path);
			__result = textAsset.text.Split(new char[]
			{
				'\n'
			});
            assetBundle.Unload(assetBundle);
            return false;
            }
        }
    }
}
