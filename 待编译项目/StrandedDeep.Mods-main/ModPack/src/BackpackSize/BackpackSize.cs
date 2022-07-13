using Beam;
using Beam.Crafting;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mod.BackpackSize
{
    public class BackpackSize : SubPlugin
    {
        public override string Name => "背包-修改器";
        private static Dictionary<InteractiveType, int> originalStackSizes;
        public static ConfigEntry<int> DefaultStackSize;
        public static ConfigEntry<int> ContainerStackSize;
        public static ConfigEntry<int> LeavesStackSize;
        public static ConfigEntry<int> LogStackSize;
        public static ConfigEntry<int> ArrowStackSize;
        public static ConfigEntry<int> SpeargunAmmoStackSize;
        public static ConfigEntry<int> FishStackSize;
        public static ConfigEntry<int> MeatStackSize;
        public static ConfigEntry<int> FruitStackSize;

        public override void Initialize()
        {
            originalStackSizes = new();
            foreach (var entry in SlotStorage.STACK_SIZES)
            originalStackSizes.Add(entry.Key, entry.Value);
            DefaultStackSize = Bind("背包一格可以放多少个物品", 40, "", SettingChanged);
            ContainerStackSize = Bind("游戏箱子", 1, "", SettingChanged);
            LeavesStackSize = Bind("纤维叶子", 40, "", SettingChanged);
            LogStackSize = Bind("原木", 40, "", SettingChanged);
            ArrowStackSize = Bind("弓的箭", 40, "", SettingChanged);
            SpeargunAmmoStackSize = Bind("气枪的箭", 40, "", SettingChanged);
            FishStackSize = Bind("鱼类", 40, "", SettingChanged);
            MeatStackSize = Bind("肉类", 40, "", SettingChanged);
            FruitStackSize = Bind("水果", 40, "", SettingChanged);
            base.Initialize();
        }

        private void SettingChanged(SettingChangedEventArgs args)
        {
            if (Enabled)
                SetBackpackSize();
        }

        public override void OnEnabled()
        {
            base.OnEnabled();
            SetBackpackSize();
        }

        public override void OnDisabled()
        {
            base.OnDisabled();

            foreach (var entry in originalStackSizes)
                SlotStorage.STACK_SIZES[entry.Key] = entry.Value;
        }

        private static void SetBackpackSize()
        {
            SlotStorage.STACK_SIZES[InteractiveType.CONTAINER] = ContainerStackSize.Value;
            SlotStorage.STACK_SIZES[InteractiveType.CRAFTING_LEAVES] = LeavesStackSize.Value;
            SlotStorage.STACK_SIZES[InteractiveType.CRAFTING_LOG] = LogStackSize.Value;
            SlotStorage.STACK_SIZES[InteractiveType.TOOLS_ARROW] = ArrowStackSize.Value;
            SlotStorage.STACK_SIZES[InteractiveType.TOOLS_SPEARGUN_ARROW] = SpeargunAmmoStackSize.Value;
            SlotStorage.STACK_SIZES[InteractiveType.ANIMALS_FISH] = FishStackSize.Value;
            SlotStorage.STACK_SIZES[InteractiveType.FOOD_MEAT] = MeatStackSize.Value;
            SlotStorage.STACK_SIZES[InteractiveType.FOOD_FRUIT] = FruitStackSize.Value;
        }

        [HarmonyPatch(typeof(SlotStorage), nameof(SlotStorage.GetStackSize))]
        public class GetBackpackSize_Patch
        {
            [HarmonyPostfix]
            static void Postfix(ref int __result, CraftingType type)
            {
                if (!SlotStorage.STACK_SIZES.TryGetValue(type.InteractiveType, out int value))
                    value = DefaultStackSize.Value;
                __result = value;
            }
        }
    }
}
