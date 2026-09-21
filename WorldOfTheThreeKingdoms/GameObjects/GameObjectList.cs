using GameGlobal;
using GameObjects.FactionDetail;
using GameObjects.PersonDetail;
using GameObjects.TroopDetail;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
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
            }
        }

        public void SetImmutable()
        {
            immutable = true;
        }

        public void Add(GameObject t)
        {
            if (immutable)
                throw new Exception("Trying to add things to an immutable list");
            this.gameObjects.Add(t);
        }
        public void Add(GameObject t, bool IDrepeat = false)
        {
            if (immutable)
                throw new Exception("Trying to add things to an immutable list");
            this.gameObjects.Add(t);
        }

        public void AddRange(GameObjectList t)
        {
            if (immutable)
                throw new Exception("Trying to add things to an immutable list");
            this.gameObjects.AddRange(t.GameObjects);
        }

        public void Clear()
        {
            if (immutable)
                throw new Exception("Trying to clear an immutable list");
            this.gameObjects.Clear();
        }

        public void ClearSelected()
        {
            foreach (GameObject obj2 in this.gameObjects)
            {
                obj2.Selected = false;
            }
        }

        public List<int> GenerateRandomIndexList()
        {
            int num;
            List<int> list = new List<int>();
            for (num = 0; num < this.Count; num++)
            {
                list.Add(num);
            }
            for (num = 0; num < this.Count; num++)
            {
                int num2 = num + GameObject.Random(this.Count - num);
                int num3 = list[num];
                list[num] = list[num2];
                list[num2] = num3;
            }
            return list;
        }

        public IEnumerator GetEnumerator()
        {
            return this.GetRealEnumerator();
        }

        public int GetFreeGameObjectID()
        {
            for (int i = this.Count; i >= 0; i--)
            {
                if (!this.HasGameObject(i))
                {
                    return i;
                }
            }
            throw new Exception("GetFreeGameObjectID Error.");
        }

        public GameObject GetGameObject(int ID)
        {
            if (ID >= 0)
            {
                return this.gameObjects.FirstOrDefault(ga => ga != null && ga.ID == ID);
                //foreach (GameObject obj2 in this.gameObjects)
                //{
                //    if (obj2.ID == ID)
                //    {
                //        return obj2;
                //    }
                //}
            }
            return null;
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
            foreach (GameObject obj2 in this.gameObjects)
            {
                bool flag = true;
                for (int i = 0; i < conditions.Length; i++)
                {
                    if (conditions[i].LEG == 0)
                    {
                        if (!StaticMethods.GetPropertyValue(obj2, conditions[i].PropertyName).Equals(conditions[i].PropertyValue))
                        {
                            flag = false;
                            break;
                        }
                    }
                    else if (conditions[i].LEG > 0)
                    {
                        if (((int)StaticMethods.GetPropertyValue(obj2, conditions[i].PropertyName)) <= ((int)conditions[i].PropertyValue))
                        {
                            flag = false;
                            break;
                        }
                    }
                    else if ((conditions[i].LEG < 0) && (((int)StaticMethods.GetPropertyValue(obj2, conditions[i].PropertyName)) >= ((int)conditions[i].PropertyValue)))
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
            return list;
        }

        public bool HasGameObject(GameObject t)
        {
            return (this.gameObjects.IndexOf(t) >= 0);
        }

        public bool HasGameObject(int ID)
        {
            if (ID >= 0)
            {
                foreach (GameObject obj2 in this.gameObjects)
                {
                    if (obj2.ID == ID)
                    {
                        return true;
                    }
                }
            }
            return false;
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
            this.gameObjects.RemoveAll(delegate(GameObject o)
            {
                return o == gameObject;
            }
            );
            //this.gameObjects.Remove(gameObject);
        }

        public void RemoveAt(int index)
        {
            if (immutable)
                throw new Exception("Trying to remove things to an immutable list");
            this.gameObjects.RemoveAt(index);
        }

        public void ReSort()
        {
            PropertyComparer comparer = new PropertyComparer(this.PropertyName, this.IsNumber, this.SmallToBig);
            this.gameObjects.Sort(comparer);
        }

        public string SaveToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (GameObject obj2 in this.gameObjects)
            {
                builder.Append(obj2.ID.ToString() + " ");
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
            foreach (GameObject gameObject in this.gameObjects)
            {
                if (gameObjectList.HasGameObject(gameObject))
                {
                    gameObject.Selected = true;
                }
            }
        }

        public void Sort(IComparer<GameObject> comparer)
        {
            this.gameObjects.Sort(comparer);
        }

        public void StableSort(IComparer<GameObject> comparer)
        {
            this.gameObjects = this.gameObjects.OrderBy<GameObject, GameObject>(x => x, comparer).ToList<GameObject>();
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

        public GameObject this[int index]
        {
            get
            {
                return this.gameObjects[index];
            }
            set
            {
                this.gameObjects[index] = value;
            }
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

