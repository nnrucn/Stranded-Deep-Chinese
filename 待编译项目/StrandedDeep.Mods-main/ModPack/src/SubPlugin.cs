using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mod
{
    public abstract class SubPlugin
    {
        public abstract string Name { get; }

        public ConfigEntry<bool> ToggleSetting;
        public bool Enabled => ToggleSetting.Value;

        protected Harmony Harmony;

        public virtual void Initialize()
        {
            Harmony = new Harmony($"{StrandedDeep_Mod.GUID}.{this.GetType().Name}");

            if (ToggleSetting.Value)
                OnEnabled();
        }

        public virtual void OnEnabled()
        {
            Harmony.PatchAll(this.GetType());
            foreach (var type in this.GetType().GetNestedTypes())
                Harmony.PatchAll(type);

            Log($"开启:{Name},深海搁浅贴吧吧务组");
        }

        public virtual void OnDisabled()
        {
            Harmony.UnpatchSelf();

            Log($"禁用:{Name},深海搁浅贴吧吧务组");
        }

        public virtual void Update() { }

        public virtual void FixedUpdate() { }

        public virtual void OnGUI() { }

        public void Toggle_SettingsChanges(object sender, EventArgs e)
        {
            if ((bool)(e as SettingChangedEventArgs).ChangedSetting.BoxedValue)
                OnEnabled();
            else
                OnDisabled();
        }

        protected ConfigEntry<T> Bind<T>(string name, 
            T defaultValue, 
            string description, 
            Action<SettingChangedEventArgs> onSettingChanged = null)
        {
            return Bind(name, defaultValue, new ConfigDescription(description), onSettingChanged);
        }

        protected ConfigEntry<T> Bind<T>(string name, 
            T defaultValue, 
            ConfigDescription description, 
            Action<SettingChangedEventArgs> onSettingChanged = null)
        {
            var config = StrandedDeep_Mod.Instance.Config.Bind(new ConfigDefinition(this.Name, name), defaultValue, description);

            if (onSettingChanged != null)
            {
                config.SettingChanged += (obj, arg) =>
                {
                    onSettingChanged(arg as SettingChangedEventArgs);
                };
            }

            return config;
        }

        protected Coroutine StartCoroutine(IEnumerator coroutine)
        {
            return StrandedDeep_Mod.Instance.StartCoroutine(coroutine);
        }

        public void Log(object o) => StrandedDeep_Mod.Logging.LogMessage(o?.ToString());
        public void LogWarning(object o) => StrandedDeep_Mod.Logging.LogWarning(o?.ToString());
        public void LogError(object o) => StrandedDeep_Mod.Logging.LogError(o?.ToString());
    }
}
