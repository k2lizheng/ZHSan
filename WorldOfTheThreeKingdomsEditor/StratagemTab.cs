using GameObjects;
using GameObjects.PersonDetail;
using GameObjects.TroopDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WorldOfTheThreeKingdomsEditor
{
    class StratagemTab : BaseTab<Stratagem>
    {
        protected override IItemList GetDataList(GameScenario scen)
        {
            return new GameObjectDictionaryItemList(scen.GameCommonData.AllStratagems.Stratagems);
        }
        protected override Dictionary<string, string> GetDefaultValues()
        {
            return new Dictionary<string, string>()
            {
              
            };
        }

        protected override string[] GetRawItemOrder()
        {
            return new String[]
            {
                "ID",
                "Name",
                    "AIConditionWeightEnemyString",
                "AIConditionWeightSelfString",
            "AnimationKind",
            "ArchitectureTarget",
                "CastConditionsString",
            "CastDefaultString",
            "CastTargetString",
            "Chance",
            "Combativity",
            "Description",
            "Friendly",
            "InfluencesString",
                "RequireInfluenceToUse",
                "Self",
                "TechniquePoint"
            };
        }

        protected override Dictionary<String, String> GetHelpText()
        {
            return new Dictionary<string, string>()
            {
                //{ "Name", "名称" },
                //{ "Combativity", "消耗战意" },
                //{ "Period", "延续天数" },
                //{ "Animation", "动画" },
                //{ "InfluencesString", "影响列表" },
                //{ "CastConditionsString", "使用条件列表" },
                //{ "LearnConditionsString", "修习条件列表" },
                //{ "AIConditionsString", "AI触发条件" },
                //{ "GenerateConditionsString", "生成武将条件" },
                //{ "GenerationChance","不同生成武将类型获得机率" },
                //{ "RelatedAbility", "此技能的相关能力、0-4为武统智政魅" },
            };
        }
        public StratagemTab(GameScenario scen, DataGrid dg, TextBlock helpTextBlock)
        {
            init(scen, dg, helpTextBlock);
        }
    }
}