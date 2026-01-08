using GameGlobal;
using GameObjects.FactionDetail;
using GameObjects.PersonDetail;
using GameObjects.TroopDetail;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects
{
    [DataContract]
    [KnownType(typeof(AttackDefaultKindList))]
    [KnownType(typeof(AttackTargetKindList))]
    [KnownType(typeof(CastDefaultKindList))]
    [KnownType(typeof(CastTargetKindList))]
    [KnownType(typeof(InformationKindList))]
    [KnownType(typeof(PersonGeneratorTypeList))]
    [KnownType(typeof(TrainPolicyList))]
    

    public class GameObjectList : IEnumerable
    {
        private List<GameObject> gameObjects = new List<GameObject>();
        private static ConcurrentDictionary<string, PropertyComparer> _comparerCache = new ConcurrentDictionary<string, PropertyComparer>();
        private Dictionary<int, GameObject> idIndex = new Dictionary<int, GameObject>();// ID查找索引
        // 序列化回调方法
        [OnDeserializing]
        private void OnDeserializing(StreamingContext context)
        {
            //在反序列化开始前初始化字典
            if (idIndex == null)
            {
                idIndex = new Dictionary<int, GameObject>();
            }
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            //反序列化完成后重建索引
            RebuildIdIndex();
        }
        [DataMember]
        public bool IsNumber;
        [DataMember]
        public string PropertyName;
        [DataMember]
        public bool SmallToBig;

        private bool immutable = false;

        [DataMember]
        public List<GameObject> GameObjects
        {
            get
            {
                return this.gameObjects;
            }
            set
            {
                this.gameObjects = value;
                RebuildIdIndex();
            }
        }
        // 构造函数中初始化查找字典
        public GameObjectList()
        {
            RebuildIdIndex();
        }
        private void RebuildIdIndex()
        {
            if (idIndex == null)
            {
                idIndex = new Dictionary<int, GameObject>();
            }
            else
            {
                idIndex.Clear();
            }
            if(gameObjects != null)
            {
                foreach (var obj in gameObjects)
                {
                    if (obj != null && obj.ID >= 0)
                    {
                        idIndex[obj.ID] = obj;
                    }
                }
            }
            
        }

        private void AddToIdIndex(GameObject obj)
        {
            if (obj != null && obj.ID >= 0)
            {
                if (idIndex == null)
                {
                    return;
                    //idIndex = new Dictionary<int, GameObject>();
                }
                // 检查是否已存在相同ID的对象，避免冲突
                if (!idIndex.ContainsKey(obj.ID))
                {
                    idIndex[obj.ID] = obj;
                }
                else if (obj.ID==0)
                {
                    obj.ID = gameObjects.Count;
                    if (!idIndex.ContainsKey(obj.ID))
                    {
                        idIndex[obj.ID] = obj;
                    }
                }
                else
                {
                    // 处理ID冲突：记录错误或使用新对象替换
                    // 这里可以根据需求决定策略，比如使用Debug输出警告
#if DEBUG
                    Debug.WriteLine($"警告: ID冲突 - ID:{obj.ID} 已存在于索引中");
#endif
                }
            }
        }
        // 添加一个方法验证索引一致性
        public bool ValidateIndex()
        {
            if (gameObjects == null || idIndex == null)
                return false;

            // 检查每个对象的ID是否都在索引中
            foreach (var obj in gameObjects)
            {
                if (obj != null && obj.ID >= 0)
                {
                    if (!idIndex.ContainsKey(obj.ID))
                    {
                        return false;
                    }
                    if (idIndex[obj.ID] != obj)
                    {
                        return false;
                    }
                }
            }

            // 检查索引中的每个对象是否都在列表中
            foreach (var kvp in idIndex)
            {
                if (!gameObjects.Contains(kvp.Value))
                {
                    return false;
                }
            }

            return true;
        }
        private void RemoveFromIdIndex(GameObject obj)
        {
            if (obj != null && obj.ID >= 0 && idIndex != null)
            {
                idIndex.Remove(obj.ID);
            }
        }
        public void SetImmutable()
        {
            immutable = true;
        }

        public void Add(GameObject t, bool IDrepeat = false)
        {
            if (immutable)
                throw new Exception("Trying to add things to an immutable list");
            // 检查是否存在相同ID的对象
            if (!IDrepeat && t != null && t.ID >= 0 && HasGameObject(t.ID))
            {
                // 可以选择抛出异常或者静默处理
                // 这里选择不添加重复ID的对象，根据需求可以改为抛出异常
#if DEBUG
                  Debug.WriteLine($"警告: ID冲突 - ID:{t.Name}{t.ID} 已存在");
#endif
                // throw new ArgumentException($"已存在ID为{t.ID}的对象");
                return; // 不添加重复ID的对象
            }
            this.gameObjects.Add(t);
            if(!IDrepeat) AddToIdIndex(t); // 更新ID索引
        }

        public void AddRange(GameObjectList t)
        {
            if (immutable)
                throw new Exception("Trying to add things to an immutable list");
            //this.gameObjects.AddRange(t.GameObjects);
            foreach (var obj in t.gameObjects)
            {
                this.gameObjects.Add(obj);
                AddToIdIndex(obj); // 更新ID索引
            }
        }

        public void Clear()
        {
            if (immutable)
                throw new Exception("Trying to clear an immutable list");
            this.gameObjects.Clear();
            idIndex?.Clear();// 清空ID索引
        }

        public void ClearSelected()
        {
            //foreach (GameObject obj2 in this.gameObjects)
            //{
            //    obj2.Selected = false;
            //}
            //使用for循环比foreach稍快
            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Selected = false;
            }
            //// 使用LINQ简化
            //foreach (var obj in gameObjects.Where(obj => obj.Selected))
            //{
            //    obj.Selected = false;
            //}
        }

        public List<int> GenerateRandomIndexList()
        {
            //int num;
            //List<int> list = new List<int>();
            //for (num = 0; num < this.Count; num++)
            //{
            //    list.Add(num);
            //}
            //for (num = 0; num < this.Count; num++)
            //{
            //    int num2 = num + GameObject.Random(this.Count - num);
            //    int num3 = list[num];
            //    list[num] = list[num2];
            //    list[num2] = num3;
            //}
            var list = Enumerable.Range(0, Count).ToList();

            // 使用更好的随机算法
            var random = new Random();
            int n = list.Count;

            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]); // 交换元素
            }
            return list;
        }

        public IEnumerator GetEnumerator()
        {
            return this.GetRealEnumerator();
        }

        public int GetFreeGameObjectID()
        {
            //for (int i = this.Count; i >= 0; i--)
            //{
            //    if (!this.HasGameObject(i))
            //    {
            //        return i;
            //    }
            //}
            //throw new Exception("GetFreeGameObjectID Error.");
            if (idIndex == null || idIndex.Count == 0)
            {
                for (int i = this.Count; i >= 0; i--)
                {
                    if (!this.HasGameObject(i))
                    {
                        RebuildIdIndex();
                        return i;
                    }
                }
            }           

            // 如果有索引，可以更快查找
            //if (idIndex.Count == 0) return 0;

            // 从当前最大ID+1开始，向上查找空闲ID
            int maxId = idIndex.Keys.Max();

            // 最多查找1000个ID，避免无限循环
            for (int i = maxId + 1; i <= maxId + 1000; i++)
            {
                if (!idIndex.ContainsKey(i))

                    return i;

            }
            throw new Exception("GetFreeGameObjectID Error.");
        }
        public GameObject this[int index]
        {
            get
            {
                return this.gameObjects[index];
            }
            set
            {
                if (immutable)
                    throw new Exception("Trying to modify an immutable list");

                if (index < 0 || index >= gameObjects.Count)
                    throw new IndexOutOfRangeException();

                if (idIndex == null) RebuildIdIndex();
                // 更新索引
                var oldObj = gameObjects[index];
                if (oldObj != null && oldObj.ID >= 0)
                {
                    idIndex.Remove(oldObj.ID);
                }

                gameObjects[index] = value;

                if (value != null && value.ID >= 0)
                {
                    if (idIndex.ContainsKey(value.ID))
                    {
                        // ID冲突处理
#if DEBUG
                        Debug.WriteLine($"警告: ID冲突 - 索引器设置时ID:{value.ID}已存在");
#endif
                    }
                    else
                    {
                        idIndex[value.ID] = value;
                    }
                }
            }
        }
        public GameObject GetGameObject(int ID)
        {
            if (ID < 0) return null;

            // 确保idIndex不为null
            if (idIndex == null || (gameObjects.Count > 0 && idIndex.Count == 0))
            {
                //idIndex = new Dictionary<int, GameObject>();
                RebuildIdIndex();
            }

            if (idIndex.TryGetValue(ID, out GameObject gameObject))
            {
                return gameObject;
            }

            // 如果在索引中没找到，但从逻辑上应该存在，可能是索引不一致
            var obj = this.gameObjects.FirstOrDefault(ga => ga != null && ga.ID == ID);
            if (obj != null)
            {
                // 自动修复索引不一致
                AddToIdIndex(obj);
            }

            return obj;

        }

        public GameObject GetGameObject(string Name)
        {
            foreach (GameObject obj2 in this.gameObjects)
            {
                if (obj2.Name == Name)
                {
                    return obj2;
                }
            }
            return null;
        }

        public GameObjectList GetList()
        {
            GameObjectList list = new GameObjectList();
            //foreach (GameObject obj2 in this.gameObjects)
            //{
            //    list.Add(obj2);
            //}
            list.gameObjects.AddRange(this.gameObjects); // 直接添加整个列表
            
            return list;
        }

        public GameObjectList GetList(params GameObjectCondition[] conditions)
        {
            GameObjectList list = new GameObjectList();
            //foreach (GameObject obj2 in this.gameObjects)
            //{
            //    bool flag = true;
            //    for (int i = 0; i < conditions.Length; i++)
            //    {
            //        if (conditions[i].LEG == 0)
            //        {
            //            if (!StaticMethods.GetPropertyValue(obj2, conditions[i].PropertyName).Equals(conditions[i].PropertyValue))
            //            {
            //                flag = false;
            //                break;
            //            }
            //        }
            //        else if (conditions[i].LEG > 0)
            //        {
            //            if (((int)StaticMethods.GetPropertyValue(obj2, conditions[i].PropertyName)) <= ((int)conditions[i].PropertyValue))
            //            {
            //                flag = false;
            //                break;
            //            }
            //        }
            //        else if ((conditions[i].LEG < 0) && (((int)StaticMethods.GetPropertyValue(obj2, conditions[i].PropertyName)) >= ((int)conditions[i].PropertyValue)))
            //        {
            //            flag = false;
            //            break;
            //        }
            //    }
            //    if (flag)
            //    {
            //        list.Add(obj2);
            //    }
            //}
            // 使用for循环比foreach稍快
            for (int i = 0; i < gameObjects.Count; i++)
            {
                GameObject obj2 = gameObjects[i];
                bool flag = true;

                // 展开循环，减少数组访问开销
                for (int j = 0; j < conditions.Length; j++)
                {
                    var condition = conditions[j];
                    object propertyValue = StaticMethods.GetPropertyValue(obj2, condition.PropertyName);

                    if (condition.LEG == 0)
                    {
                        if (!propertyValue.Equals(condition.PropertyValue))
                        {
                            flag = false;
                            break;
                        }
                    }
                    else if (condition.LEG > 0)
                    {
                        if (((int)propertyValue) <= ((int)condition.PropertyValue))
                        {
                            flag = false;
                            break;
                        }
                    }
                    else if (((int)propertyValue) >= ((int)condition.PropertyValue))
                    {
                        flag = false;
                        break;
                    }
                }

                if (flag)
                {
                    list.Add(obj2);
                }
            }
            return list;
        }

        public GameObjectList GetMaxObjects(int count)
        {
            int num;
            if (count > this.Count)
            {
                count = this.Count;
            }
            GameObjectList list = new GameObjectList();
            if (!this.SmallToBig)
            {
                for (num = 0; num < count; num++)
                {
                    list.Add(this[num]);
                }
                return list;
            }
            for (num = count - 1; num >= 0; num--)
            {
                list.Add(this[num]);
            }
            return list;
        }

        public GameObjectList GetMinObjects(int count)
        {
            int num;
            if (count > this.Count)
            {
                count = this.Count;
            }
            GameObjectList list = new GameObjectList();
            if (this.SmallToBig)
            {
                for (num = 0; num < count; num++)
                {
                    list.Add(this[num]);
                }
                return list;
            }
            for (num = count - 1; num >= 0; num--)
            {
                list.Add(this[num]);
            }
            return list;
        }

        public GameObjectList GetRandomList()
        {
            GameObjectList list = new GameObjectList();
            foreach (int num in this.GenerateRandomIndexList())
            {
                list.Add(this.gameObjects[num]);
            }
            list.RebuildIdIndex();
            return list;
        }

        public GameObject GetRandomObject()
        {
            return this.GameObjects[GameObject.Random(this.GameObjects.Count)];
        }

        public IEnumerator GetRealEnumerator()
        {
            foreach (GameObject iteratorVariable0 in this.gameObjects)
            {
                yield return iteratorVariable0;
            }
        }

        public GameObjectList GetSelectedList()
        {
            GameObjectList list = new GameObjectList();
            foreach (GameObject obj2 in this.gameObjects)
            {
                if (obj2.Selected)
                {
                    list.Add(obj2);
                }
            }
            // 使用LINQ简化
            //var selectedObjects = gameObjects.Where(obj => obj.Selected).ToList();
            //list.AddRange(new GameObjectList { GameObjects = selectedObjects });
            return list;
        }

        public bool HasGameObject(GameObject t)
        {
            if (t == null) return false;
            if ((idIndex == null || idIndex.Count == 0))
            {
                // 回退到线性查找（对于没有ID或ID为负的情况）
                return (this.gameObjects.IndexOf(t) >= 0);
            }
            // 优先通过ID索引查找
            else
            {
                // 通过ID查找，然后比较引用相等性
                if (idIndex.TryGetValue(t.ID, out GameObject storedObj))
                {
                    return ReferenceEquals(storedObj, t);
                }
                return false;
            }            
        }

        public bool HasGameObject(int ID)
        {
            if ((idIndex == null || idIndex.Count == 0) && ID >= 0)
            {
                foreach (GameObject obj2 in this.gameObjects)
                {
                    if (obj2.ID == ID)
                    {
                        return true;
                    }
                }
            }
            //if (ID < 0) return false;

            //// 确保idIndex不为null
            //if (idIndex == null)
            //{
            //    idIndex = new Dictionary<int, GameObject>();
            //    RebuildIdIndex();
            //}

            return idIndex.ContainsKey(ID);
           
            //return false;
        }

        public bool HasGameObject(string Name)
        {
            foreach (GameObject obj2 in this.gameObjects)
            {
                if (obj2.Name == Name)
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasSelectedItem()
        {
            foreach (GameObject obj2 in this.gameObjects)
            {
                if (obj2.Selected)
                {
                    return true;
                }
            }
            return false;
            // 使用LINQ Any方法
            //return gameObjects.Any(obj => obj.Selected);
        }

        public int IndexOf(GameObject t)
        {
            return this.gameObjects.IndexOf(t);
        }

        public List<string> LoadFromString(GameObjectList list, string dataString)
        {
            List<string> errorMsg = new List<string>();
            char[] separator = new char[] { ' ', '\n', '\r', '\t' };
            string[] strArray = dataString.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            this.Clear();
            try
            {
                foreach (string str in strArray)
                {
                    GameObject gameObject = list.GetGameObject(int.Parse(str));
                    if (gameObject != null)
                    {
                        this.Add(gameObject);
                    }
                    else
                    {
                        errorMsg.Add("人物ID" + str + "不存在");
                    }
                }
            }
            catch
            {
                errorMsg.Add("多项人物一栏应为半型空格分隔的称号ID");
            }
            return errorMsg;
        }

        public void Remove(GameObject gameObject)
        {
            if (immutable)
                throw new Exception("Trying to remove things to an immutable list");
            //if(idIndex == null || idIndex.Count == 0) 
            //{ 
            //this.gameObjects.RemoveAll(delegate (GameObject o)
            //{
            //    return o == gameObject;
            //}
            //);
            //}
            // 如果列表中没有该对象，直接返回
            //if (!gameObjects.Contains(gameObject))
            //{
            //    return;
            //}
            if ((idIndex != null || idIndex.Count > 0) && gameObjects.Remove(gameObject))
            {
                RemoveFromIdIndex(gameObject); // 从索引中移除
                return;
            }
            // 如果需要移除所有匹配项
            int removedCount = 0;
            for (int i = gameObjects.Count - 1; i >= 0; i--)
            {
                if (gameObjects[i] == gameObject)
                {
                    gameObjects.RemoveAt(i);
                    removedCount++;
                }
            }

            // 如果移除了至少一个对象，更新索引
            if (removedCount > 0)
            {
                RemoveFromIdIndex(gameObject);
            }
        }

        public void RemoveAt(int index)
        {
            if (immutable)
                throw new Exception("Trying to remove things to an immutable list");
            if (index >= 0 && index < gameObjects.Count)
            {
                GameObject obj = gameObjects[index];
                this.gameObjects.RemoveAt(index);
                RemoveFromIdIndex(obj); // 从索引中移除
            }
        }

        public void ReSort()
        {
            //PropertyComparer comparer = new PropertyComparer(this.PropertyName, this.IsNumber, this.SmallToBig);
            string key = $"{PropertyName}_{IsNumber}_{SmallToBig}";
            var comparer = _comparerCache.GetOrAdd(key,
                k => new PropertyComparer(PropertyName, IsNumber, SmallToBig));
            this.gameObjects.Sort(comparer);
        }

        public string SaveToString()
        {
            //StringBuilder builder = new StringBuilder();
            //foreach (GameObject obj2 in this.gameObjects)
            //{
            //    builder.Append(obj2.ID.ToString() + " ");
            //}
            if (gameObjects.Count == 0) return string.Empty;

            // 预分配StringBuilder容量，假设每个ID平均8个字符
            StringBuilder builder = new StringBuilder(gameObjects.Count * 8);

            for (int i = 0; i < gameObjects.Count; i++)
            {
                builder.Append(gameObjects[i].ID);
                builder.Append(' ');
            }

            // 移除最后一个空格
            if (builder.Length > 0)
            {
                builder.Length--; // 移除最后一个字符
            }
            return builder.ToString();
        }

        public void SetOtherUnSelected(GameObject selectedT)
        {
            foreach (GameObject obj2 in this.gameObjects)
            {
                if (obj2 != selectedT)
                {
                    obj2.Selected = false;
                }
            }
        }

        public void SetSelected(GameObjectList gameObjectList)
        {
            //foreach (GameObject gameObject in this.gameObjects)
            //{
            //    if (gameObjectList.HasGameObject(gameObject))
            //    {
            //        gameObject.Selected = true;
            //    }
            //}
            // 先全部取消选中
            ClearSelected();

            // 使用HashSet加速查找
            var targetSet = new HashSet<GameObject>(gameObjectList.GameObjects);

            foreach (var gameObject in gameObjects)
            {
                gameObject.Selected = targetSet.Contains(gameObject);
            }
        }

        public void Sort(IComparer<GameObject> comparer)
        {
            this.gameObjects.Sort(comparer);
        }

        public void StableSort(IComparer<GameObject> comparer)
        {
            //if (immutable)
            //    throw new InvalidOperationException("Cannot sort an immutable list");

            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            if (Count < 2) return;

            // 优化1：使用 ToList() 而不是 ToList<GameObject>()
            this.gameObjects = this.gameObjects.OrderBy(x => x, comparer).ToList();

            //this.gameObjects = this.gameObjects.OrderBy<GameObject, GameObject>(x => x, comparer).ToList<GameObject>();
            //RebuildIdIndex(); // 因为创建了新列表，需要重建索引
        }
        public void SortByID()
        {
            if (immutable)
                throw new Exception("Trying to sort an immutable list");

            if (Count < 2) return;

            // 直接对内部列表排序
            gameObjects.Sort((a, b) => a.ID.CompareTo(b.ID));
            // 注意：不需要重建idIndex，因为字典不受顺序影响
        }

        public override string ToString()
        {
            return (base.GetType().Name + ":Count=" + this.Count);
        }

        public int Count
        {
            get
            {
                return this.gameObjects.Count;
            }
        }

       
        // 批量操作
        public void ForEach(Action<GameObject> action)
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                action(gameObjects[i]);
            }
        }

        // 并行批量操作（对于大量数据）
        public void ParallelForEach(Action<GameObject> action)
        {
            Parallel.ForEach(gameObjects, action);
        }

        // 条件筛选
        public GameObjectList Where(Func<GameObject, bool> predicate)
        {
            GameObjectList result = new GameObjectList();
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (predicate(gameObjects[i]))
                {
                    result.Add(gameObjects[i]);
                }
            }
            return result;
        }
        // 第一个满足条件的
        public GameObject FirstOrDefault(Func<GameObject, bool> predicate)
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (predicate(gameObjects[i]))
                {
                    return gameObjects[i];
                }
            }
            return null;
        }
        // 投影
        public List<TResult> Select<TResult>(Func<GameObject, TResult> selector)
        {
            return gameObjects.Select(selector).ToList();
        }

        // 检查是否所有元素满足条件
        public bool All(Func<GameObject, bool> predicate)
        {
            return gameObjects.All(predicate);
        }

        // 检查是否存在元素满足条件
        public bool Any(Func<GameObject, bool> predicate)
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (predicate(gameObjects[i]))
                {
                    return true;
                }
            }
            return false;
        }

        // 计数满足条件的元素
        public int CountByPredicate(Func<GameObject, bool> predicate)
        {
            int count = 0;
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (predicate(gameObjects[i]))
                {
                    count++;
                }
            }
            return count;
        }
        // 排序
        public GameObjectList OrderBy<TKey>(Func<GameObject, TKey> keySelector, bool descending = false)
        {
            var comparer = Comparer<TKey>.Default;
            GameObjectList result = new GameObjectList();
            result.GameObjects.AddRange(gameObjects);

            result.GameObjects.Sort((a, b) => {
                TKey keyA = keySelector(a);
                TKey keyB = keySelector(b);
                int comparison = comparer.Compare(keyA, keyB);
                return descending ? -comparison : comparison;
            });

            return result;
        }
        // 获取最大值
        public GameObject MaxBy<TKey>(Func<GameObject, TKey> selector) where TKey : IComparable<TKey>
        {
            return gameObjects.OrderByDescending(selector).FirstOrDefault();
        }

        // 获取最小值
        public GameObject MinBy<TKey>(Func<GameObject, TKey> selector) where TKey : IComparable<TKey>
        {
            return gameObjects.OrderBy(selector).FirstOrDefault();
        }
        /// <summary>
        /// 将GameObjectList转换为数组
        /// </summary>
        /// <returns>包含所有GameObject的数组</returns>
        public GameObject[] ToArray()
        {
            // 直接返回内部列表的数组副本
            return gameObjects.ToArray();
        }

        /// <summary>
        /// 将GameObjectList转换为指定类型的数组
        /// </summary>
        /// <typeparam name="T">目标类型（必须是GameObject或其子类）</typeparam>
        /// <returns>包含指定类型对象的数组</returns>
        public T[] ToArray<T>() where T : GameObject
        {
            // 方法1: 使用LINQ进行类型过滤和转换
            return gameObjects.OfType<T>().ToArray();
        }

        /// <summary>
        /// 将GameObjectList转换为数组（高效版本）
        /// </summary>
        /// <returns>包含所有GameObject的数组</returns>
        public GameObject[] ToArrayFast()
        {
            if (gameObjects.Count == 0)
                return Array.Empty<GameObject>();

            // 方法2: 手动创建数组并填充，避免LINQ开销
            var array = new GameObject[gameObjects.Count];
            gameObjects.CopyTo(array, 0);
            return array;
        }

        /// <summary>
        /// 将GameObjectList转换为指定类型的数组（高效版本）
        /// </summary>
        /// <typeparam name="T">目标类型（必须是GameObject或其子类）</typeparam>
        /// <returns>包含指定类型对象的数组</returns>
        public T[] ToArrayFast<T>() where T : GameObject
        {
            if (gameObjects.Count == 0)
                return Array.Empty<T>();

            // 方法1: 预筛选类型
            var list = new List<T>(gameObjects.Count);
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i] is T item)
                    list.Add(item);
            }
            return list.ToArray();

            // 方法2: 使用LINQ但指定容量（对大型集合更友好）
            // return gameObjects.Where(obj => obj is T).Cast<T>().ToArray();
        }

        /// <summary>
        /// 将选中的GameObject转换为数组
        /// </summary>
        /// <returns>包含所有选中GameObject的数组</returns>
        public GameObject[] SelectedToArray()
        {
            // 使用预分配列表以提高性能
            var selectedList = new List<GameObject>();
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i] != null && gameObjects[i].Selected)
                    selectedList.Add(gameObjects[i]);
            }
            return selectedList.ToArray();
        }

        /// <summary>
        /// 将选中的GameObject转换为指定类型的数组
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <returns>包含所有选中指定类型对象的数组</returns>
        public T[] SelectedToArray<T>() where T : GameObject
        {
            var selectedList = new List<T>();
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i] != null && gameObjects[i].Selected && gameObjects[i] is T item)
                    selectedList.Add(item);
            }
            return selectedList.ToArray();
        }

        /// <summary>
        /// 将GameObjectList转换为ID数组
        /// </summary>
        /// <returns>包含所有GameObject ID的数组</returns>
        public int[] ToIdArray()
        {
            if (gameObjects.Count == 0)
                return Array.Empty<int>();

            var idArray = new int[gameObjects.Count];
            for (int i = 0; i < gameObjects.Count; i++)
            {
                idArray[i] = gameObjects[i]?.ID ?? -1;
            }
            return idArray;
        }

        /// <summary>
        /// 将GameObjectList转换为Name数组
        /// </summary>
        /// <returns>包含所有GameObject Name的数组</returns>
        public string[] ToNameArray()
        {
            if (gameObjects.Count == 0)
                return Array.Empty<string>();

            var nameArray = new string[gameObjects.Count];
            for (int i = 0; i < gameObjects.Count; i++)
            {
                nameArray[i] = gameObjects[i]?.Name ?? string.Empty;
            }
            return nameArray;
        }

        /// <summary>
        /// 将GameObjectList转换为二维数组（ID和Name）
        /// </summary>
        /// <returns>包含ID和Name的二维数组</returns>
        public object[,] ToIdNameArray()
        {
            if (gameObjects.Count == 0)
                return new object[0, 2];

            var result = new object[gameObjects.Count, 2];
            for (int i = 0; i < gameObjects.Count; i++)
            {
                result[i, 0] = gameObjects[i]?.ID ?? -1;
                result[i, 1] = gameObjects[i]?.Name ?? string.Empty;
            }
            return result;
        }

        /// <summary>

        /// </summary>
        /*
        [CompilerGenerated]
        private sealed class GetRealEnumerator__0 : IEnumerator<object>, IEnumerator, IDisposable
        {
            private int a1__state;
            private object a2__current;
            public GameObjectList a4__this;
            public List<GameObject>.Enumerator a7__wrap2;
            public GameObject go5__1;

            [DebuggerHidden]
            public GetRealEnumerator__0(int a1__state)
            {
                this.a1__state = a1__state;
            }

            private void am__Finally3()
            {
                this.a1__state = -1;
                this.a7__wrap2.Dispose();
            }

            //private bool MoveNext()
            public bool MoveNext()
            {
                try
                {
                    /*
                    switch (this.a1__state)
                    {
                        case 0:
                            this.a1__state = -1;
                            this.a7__wrap2 = this.a4__this.gameObjects.GetEnumerator();
                            this.a1__state = 1;
                            while (this.a7__wrap2.MoveNext())
                            {
                                this.go5__1 = this.a7__wrap2.Current;
                                this.a2__current = this.go5__1;
                                this.a1__state = 2;
                                return true;
                            Label_0071:
                                this.a1__state = 1;
                            }
                            this.am__Finally3();
                            break;

                        case 2:
                            goto Label_0071;

                    }*/


        /*

                    switch (this.a1__state)
                    {
                        case 0:
                            this.a1__state = -1;
                            this.a7__wrap2 = this.a4__this.gameObjects.GetEnumerator();
                            this.a1__state = 1;
                            while (this.a7__wrap2.MoveNext())
                            {
                                this.go5__1 = this.a7__wrap2.Current;
                                this.a2__current = this.go5__1;
                                this.a1__state = 2;
                                return true;
                            //Label_0071:
                                //this.a1__state = 1;
                            }
                            this.am__Finally3();
                            break;

                        case 2:
                            //goto Label_0071;

                            this.a1__state = 1;
                            while (this.a7__wrap2.MoveNext())
                            {
                                this.go5__1 = this.a7__wrap2.Current;
                                this.a2__current = this.go5__1;
                                this.a1__state = 2;
                                return true;
                            //Label_0071:
                                //this.a1__state = 1;
                            }
                            this.am__Finally3();
                            break;



                    }





                    return false;
                }
                //fault
                catch
                {
                    //this.System.IDisposable.Dispose();
                    throw new Exception("GameObjectList.cs   private bool MoveNext()     error!"); 
                }
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.a1__state)
                {
                    case 1:
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            this.am__Finally3();
                        }
                        break;
                }
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.a2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.a2__current;
                }
            }
        }

        */
        //end 
    }
}

