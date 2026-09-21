using GameManager;
using GameObjects.Influences;
using GameObjects.PersonDetail;
using Microsoft.Xna.Framework.Graphics;
using Platforms;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WorldOfTheThreeKingdoms;

namespace GameObjects
{
    [DataContract]
    public class Treasure : GameObject
    {
        // 工厂类 - 负责创建逻辑
        private static class TreasureFactory
        {
            // 线程安全的缓存
            private static readonly ConcurrentDictionary<string, List<int>> _fileCache =
                new ConcurrentDictionary<string, List<int>>();

            // 默认影响列表缓存
            private static readonly Dictionary<int, List<int>> _defaultInfluenceGroups =
                new Lazy<Dictionary<int, List<int>>>(CreateDefaultInfluenceGroups, true).Value;

            // 宝物类型配置
            private static readonly Dictionary<string, TreasureTypeConfig> _treasureTypeConfigs =
                new Lazy<Dictionary<string, TreasureTypeConfig>>(CreateTreasureTypeConfigs, true).Value;

            // 预过滤的特殊影响列表（用于全能类型）
            private static List<Influence> _specialInfluences;
            private static readonly object _specialInfluencesLock = new object();

            private static Dictionary<int, List<int>> CreateDefaultInfluenceGroups()
            {
                return new Dictionary<int, List<int>>
                {
                    [3214] = new List<int> { 6190, 6411, 6253, 6252, 6251, 6250, 6180, 6182, 6183, 6191 },
                    [1100] = new List<int> { 6010, 6011, 6012, 6013, 6014, 6015, 6016, 6017, 6018, 6019 },
                    [300] = new List<int> { 7532, 250, 7083, 220, 7082, 7080, 7079, 7081, 251, 7084, 207, 252, 261, 262, 263 },
                    [1300] = new List<int> { 53, 124, 120, 7292, 7293, 7294, 7295, 7296, 0, 1, 2, 3, 4, 5, 10, 11, 12, 13, 14, 15, 121, 122, 123 },
                    [2900] = new List<int> { 610, 623, 622, 661, 621, 620, 630, 631, 660, 650, 651, 609, 611 },
                    [2001] = new List<int> { 6823, 6822, 6821, 6801, 6800, 6820, 6820, 6810, 6811, 6821, 6822, 6823 },
                    [3200] = new List<int> { 7265, 6024, 6014, 6004, 6000, 6010, 6020, 6030, 6040, 6001, 6011, 6021, 6031, 6041, 6044, 6034, 7263 },
                    [4100] = new List<int> { 7239, 5080, 5091, 5061, 6002, 5060, 5070, 6001, 5071, 5090, 5081, 7255 },
                    [4000] = new List<int> { 604, 7179, 511, 530, 401, 405, 420, 421, 531, 510, 7179, 600 },
                    [1200] = new List<int> { 572, 6220, 6570, 6905, 6990, 6705, 6540, 6515, 6715, 6995, 6945, 6595, 6210, 573 }
                };
            }

            private static Dictionary<string, TreasureTypeConfig> CreateTreasureTypeConfigs()
            {
                return new Dictionary<string, TreasureTypeConfig>
                {
                    ["全能"] = new TreasureTypeConfig(3214, 1376, "玉玺", true, 281, null),
                    ["将军"] = new TreasureTypeConfig(1100, 1, "武器", false, 0, null),
                    ["君主"] = new TreasureTypeConfig(300, 1001, "名马", false, 281, null),
                    ["智将"] = new TreasureTypeConfig(1300, 1211, "政书", false, 0, null),
                    ["女官"] = new TreasureTypeConfig(2900, 1352, "乐器", false, 0, null),
                    ["识者"] = new TreasureTypeConfig(2001, 1312, "药", false, 0, null),
                    ["文官"] = new TreasureTypeConfig(3200, 1332, "器具", false, 0, null),
                    ["武官"] = new TreasureTypeConfig(4100, 43, "头盔", false, 0, null),
                    ["猛将"] = new TreasureTypeConfig(4000, 3048, "铠甲", false, 0, null),
                    ["军师"] = new TreasureTypeConfig(1200, 1205, "兵书", false, 0, null)
                };
            }

            public static Treasure CreateTreasure(PersonGenerateParam param, bool isAI)
            {
                var treasure = new Treasure();

                // 分配ID
                AllocateNewID(treasure);

                // 初始化名称
                treasure.Name = GenerateTreasureName(treasure.ID);

                // 根据参数配置宝物
                ConfigureTreasure(treasure, param);

                // 设置基本属性
                SetBasicProperties(treasure, param);

                // 处理特殊影响
                HandleSpecialInfluences(treasure, param, isAI);

                // 添加到场景
                Session.Current.Scenario.Treasures.AddTreasure(treasure);

                return treasure;
            }

            private static void AllocateNewID(Treasure treasure)
            {
                var treasures = Session.Current.Scenario.Treasures as TreasureList;
                if (treasures == null)
                {
                    treasure.ID = 25000;
                    return;
                }

                // 使用现有方法或改进的方法分配ID
                int newId = FindAvailableID(treasures);
                treasure.ID = newId;
            }

            private static int FindAvailableID(TreasureList treasures)
            {
                
                int candidateId = Session.Current.Scenario.Treasures.GetFreeGameObjectID();

                return candidateId;
            }

            private static void ConfigureTreasure(Treasure treasure, PersonGenerateParam param)
            {
                string preferredType = param.PreferredType.Name;

                if (_treasureTypeConfigs.TryGetValue(preferredType, out var config))
                {
                    treasure.TreasureGroup = config.GroupId;
                    treasure.Pic = config.PictureId;
                    treasure.Name += config.NameSuffix;

                    // 处理特殊类型
                    if (config.HasSpecialInfluence)
                    {
                        HandleSpecialTypeConfiguration(treasure, config, preferredType);
                    }

                    // 添加配置的影响
                    if (config.FixedInfluenceId > 0)
                    {
                        AddFixedInfluence(treasure, config.FixedInfluenceId);
                    }

                    // 从文件或默认列表添加随机影响
                    AddRandomInfluence(treasure, config.GroupId);
                }
            }

            private static void HandleSpecialTypeConfiguration(Treasure treasure, TreasureTypeConfig config, string preferredType)
            {
                if (preferredType == "全能")
                {
                    // 全能类型有特殊处理逻辑
                    AddRandomSpecialInfluence(treasure);
                }
            }

            private static void SetBasicProperties(Treasure treasure, PersonGenerateParam param)
            {
                treasure.AppearYear = 0;
                treasure.Description = $"{Session.Current.Scenario.Date}委托{param.PreferredType}打造于{param.FoundLocation.Name}";
                treasure.HidePlaceIDString = -1;
                treasure.Available = true;
            }

            private static void HandleSpecialInfluences(Treasure treasure, PersonGenerateParam param, bool isAI)
            {
                if (treasure.Worth < 5)
                {
                    treasure.Description += "（催生啦》》》》》》）";
                    treasure.Worth = 1;
                    treasure.TreasureGroup = 1312;
                }
            }

            private static void AddFixedInfluence(Treasure treasure, int influenceId)
            {
                var influence = Session.Current.Scenario.GameCommonData.AllInfluences.GetInfluence(influenceId);
                if (influence != null)
                {
                    treasure.Influences.AddInfluence(influence);
                }
            }

            private static void AddRandomInfluence(Treasure treasure, int groupId)
            {
                List<int> influenceList = GetInfluenceListForGroup(groupId);
                if (influenceList == null || influenceList.Count == 0)
                    return;

                AddRandomInfluenceFromList(treasure, influenceList);
            }

            private static List<int> GetInfluenceListForGroup(int groupId)
            {
                string filePath = $"Content/Data/Treasure/{groupId}.txt";

                // 尝试从缓存获取
                if (_fileCache.TryGetValue(filePath, out var cachedList))
                {
                    return cachedList;
                }

                List<int> influenceList;

                if (Platform.Current.FileExists(filePath))
                {
                    influenceList = Person.readNumberList(filePath);
                }
                else
                {
                    // 使用预定义的默认列表
                    _defaultInfluenceGroups.TryGetValue(groupId, out influenceList);
                }

                // 缓存结果（如果找到）
                if (influenceList != null)
                {
                    _fileCache[filePath] = influenceList;
                }

                return influenceList;
            }

            private static void AddRandomInfluenceFromList(Treasure treasure, List<int> influenceList)
            {
                int count = influenceList.Count;
                if (count == 0) return;
                
                int a = Random(0, count - 1);
                int b = Random(0, count - 1);
                int c = (a + b) / 2;

                // 计算价值
                treasure.Worth = Math.Abs(c - count / 2) * 160 / Math.Max(count, 1);

                // 添加影响
                int influenceId = influenceList[c];
                try
                {
                    var influence = Session.Current.Scenario.GameCommonData.AllInfluences.GetInfluence(influenceId);
                    if (influence != null)
                    {
                        treasure.Influences.AddInfluence(influence);
                    }
                }
                catch
                {
                    // 记录日志或忽略
                }
            }

            private static void AddRandomSpecialInfluence(Treasure treasure)
            {
                // 延迟加载并缓存特殊影响列表
                if (_specialInfluences == null)
                {
                    lock (_specialInfluencesLock)
                    {
                        if (_specialInfluences == null)
                        {
                            _specialInfluences = LoadSpecialInfluences();
                        }
                    }
                }

                if (_specialInfluences.Count == 0) return;

                int randomIndex = Random(0, _specialInfluences.Count - 1);
                treasure.Influences.AddInfluence(_specialInfluences[randomIndex]);
            }

            private static List<Influence> LoadSpecialInfluences()
            {
                var allInfluences = Session.Current.Scenario.GameCommonData.AllInfluences;
                if (allInfluences?.Influences == null)
                    return new List<Influence>();

                // 筛选条件：Kind.ID >= 320 且 Description 不为空
                return allInfluences.Influences.Values
                    .Where(influence => influence.Kind.ID >= 320 && !string.IsNullOrEmpty(influence.Description))
                    .ToList();
            }

            private static string GenerateTreasureName(int id)
            {
                string idStr = id.ToString();
                return idStr.Length >= 3
                    ? idStr.Substring(idStr.Length - 3)
                    : idStr.PadLeft(3, '0');
            }           

            // 配置数据结构
            private class TreasureTypeConfig
            {
                public int GroupId { get; }
                public int PictureId { get; }
                public string NameSuffix { get; }
                public bool HasSpecialInfluence { get; }
                public int FixedInfluenceId { get; }
                public Func<Treasure, bool> CustomLogic { get; }

                public TreasureTypeConfig(
                    int groupId,
                    int pictureId,
                    string nameSuffix,
                    bool hasSpecialInfluence = false,
                    int fixedInfluenceId = 0,
                    Func<Treasure, bool> customLogic = null)
                {
                    GroupId = groupId;
                    PictureId = pictureId;
                    NameSuffix = nameSuffix;
                    HasSpecialInfluence = hasSpecialInfluence;
                    FixedInfluenceId = fixedInfluenceId;
                    CustomLogic = customLogic;
                }
            }
        }

        // ==================== Treasure 类实例部分 ====================

        [DataMember]
        public int BelongedPersonIDString { get; set; }

        public Person BelongedPerson { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int HidePlaceIDString { get; set; }

        public Architecture HidePlace { get; set; }

        [DataMember]
        public string InfluencesString { get; set; }

        public InfluenceTable Influences { get; private set; } = new InfluenceTable();

        [DataMember]
        public int Pic { get; set; }

        private PlatformTexture _picture;
        private bool _pictureLoaded;
        private bool _pictureLoadFailed;

        [DataMember]
        public int TreasureGroup { get; set; }

        [DataMember]
        public int AppearYear { get; set; }

        [DataMember]
        public bool Available { get; set; }

        [DataMember]
        public int Worth { get; set; }

        public string BelongedPersonString => BelongedPerson?.Name ?? "----";

        public string HidePlaceString => HidePlace?.Name ?? "----";

        public string InfluenceString
        {
            get
            {
                if (Influences == null || Influences.Influences.Count == 0)
                    return string.Empty;

                var influences = Influences.Influences.Values;
                var result = new System.Text.StringBuilder(influences.Count * 32);

                foreach (Influence influence in influences)
                {
                    result.Append('•').Append(influence.Description);
                }

                return result.ToString();
            }
        }

        public PlatformTexture Picture
        {
            get
            {
                if (_picture == null && !_pictureLoadFailed)
                {
                    if (!_pictureLoaded)
                    {
                        try
                        {
                            string texturePath = $"Content/Textures/Resources/Treasure/{Pic}.png";
                            _picture = CacheManager.GetTempTexture(texturePath);
                            _pictureLoaded = true;
                        }
                        catch
                        {
                            _pictureLoadFailed = true;
                            _picture = null;
                        }
                    }
                }
                return _picture;
            }
        }

        public void Init()
        {
            Influences = new InfluenceTable();
            _picture = null;
            _pictureLoaded = false;
            _pictureLoadFailed = false;
        }

        /// <summary>
        /// 预加载图片纹理
        /// </summary>
        public void PreloadPicture()
        {
            _ = Picture; // 触发加载
        }

        /// <summary>
        /// 创建宝物的公共接口
        /// </summary>
        public static Treasure CreateTreasure(PersonGenerateParam param, bool isAI)
        {
            return TreasureFactory.CreateTreasure(param, isAI);
        }
    }
}