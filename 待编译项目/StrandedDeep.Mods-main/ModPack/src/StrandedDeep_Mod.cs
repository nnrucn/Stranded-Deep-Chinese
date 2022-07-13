using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mod
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class StrandedDeep_Mod : BaseUnityPlugin
    {
        private ConfigEntry<string> Mod_author;
        public const string GUID = "Stranded-Deep-Mod";
        const string NAME = "深海搁浅Mod";
        const string VERSION = "0.90.11";
        const string TOGGLES_CATEGORY = "背包-修改器";

        public static StrandedDeep_Mod Instance { get; private set; }
        public static ManualLogSource Logging => Instance.Logger;

        internal static List<SubPlugin> SubPlugins { get; private set; } = new List<SubPlugin>();

        internal void Awake()
        {
            Mod_author = Config.Bind("Mod作者QQ群:","Mod作者QQ群","QQ群:425934511");
            Instance = this;

            foreach (var type in this.GetType().Assembly.GetTypes())
            {
                if (type.IsSubclassOf(typeof(SubPlugin)))
                {
                    try
                    {
                        var plugin = (SubPlugin)Activator.CreateInstance(type);
                        SubPlugins.Add(plugin);

                        var toggle = Config.Bind(
                            new ConfigDefinition(TOGGLES_CATEGORY, plugin.Name),
                            true,
                            new ConfigDescription($"true为开启,false为关闭!"));

                        toggle.SettingChanged += plugin.Toggle_SettingsChanges;
                        plugin.ToggleSetting = toggle;

                        plugin.Initialize();
                    }
                    catch (Exception ex)
                    {
                        Logger.LogWarning($"程序异常:{type.Name}!");
                        Logger.LogMessage(ex);
                    }
                }
            }
        }

        internal void Update()
        {
            foreach (var plugin in SubPlugins)
            {
                if (plugin.Enabled)
                    plugin.Update();
            }
        }

        internal void FixedUpdate()
        {
            foreach (var plugin in SubPlugins)
            {
                if (plugin.Enabled)
                    plugin.FixedUpdate();
            }
        }

        internal void OnGUI()
        {
            foreach (var plugin in SubPlugins)
            {
                if (plugin.Enabled)
                    plugin.OnGUI();
            }
        }
    }
}
