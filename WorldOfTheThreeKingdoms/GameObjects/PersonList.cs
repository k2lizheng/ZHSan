using GameManager;
using GameObjects.Influences;
using GameObjects.PersonDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace GameObjects
{
    [DataContract]
    public class PersonList : GameObjectList
    {
        // 构造函数
        public PersonList()
        {
            // 初始化特定于PersonList的内容
        }
        public void Add(Person person)
        {
            base.Add(person);
        }
        public void AddRange(IEnumerable<Person> persons)
        {
            foreach (var person in persons)
            {
                Add(person);
            }
        }
        public Person GetPerson(int id)
        {
            return base.GetGameObject(id) as Person;
        }
        public List<Person> GetPersons(IEnumerable<int> ids)
        {
            var result = new List<Person>();
            foreach (int id in ids)
            {
                var person = GetPerson(id);
                if (person != null)
                {
                    result.Add(person);
                }
            }
            return result;
        }
        public void AddPersonWithEvent(Person person, bool add = true)
        {
            if (person == null) return;
            if (add)
            {
                base.Add(person);
            }
            //if (Session.MainGame.mainGameScreen != null)
            //{
                person.OnJailBreakSuccess += new Person.JailBreakSuccess(person_OnJailBreakSuccess);
                person.OnJailBreakFailed += new Person.JailBreakFailed(person_OnJailBreakFailed);
                person.OnConvinceSuccess += new Person.ConvinceSuccess(this.person_OnConvinceSuccess);
                person.OnConvinceFailed += new Person.ConvinceFailed(this.person_OnConvinceFailed);
                person.OnInformationObtained += new Person.InformationObtained(this.person_OnInformationObtained);
                person.qingbaoshibaishijian +=new Person.qingbaoshibai(this.person_qingbaoshibai);
               // person.OnSpySuccess += new Person.SpySuccess(this.person_OnSpySuccess);
               // person.OnSpyFailed += new Person.SpyFailed(this.person_OnSpyFailed);
                person.OnDestroySuccess += new Person.DestroySuccess(this.person_OnDestroySuccess);
                person.OnDestroyFailed += new Person.DestroyFailed(this.person_OnDestroyFailed);
                person.OnInstigateSuccess += new Person.InstigateSuccess(this.person_OnInstigateSuccess);
                person.OnInstigateFailed += new Person.InstigateFailed(this.person_OnInstigateFailed);
                person.OnGossipSuccess += new Person.GossipSuccess(this.person_OnGossipSuccess);
                person.OnGossipFailed += new Person.GossipFailed(this.person_OnGossipFailed);
                person.OnSearchFinished += new Person.SearchFinished(this.person_OnSearchFinished);
                //person.OnSpyFound += new Person.SpyFound(this.person_OnSpyFound);
                person.OnTreasureFound += new Person.TreasureFound(this.person_OnTreasureFound);
               // person.OnShowMessage += new Person.ShowMessage(this.person_OnShowMessage);
                person.OnDeath += new Person.Death(this.person_OnDeath);
                person.OnLeave += new Person.Leave(this.person_OnLeave);
                person.OnBeKilled += new Person.BeKilled(this.person_OnBeKilled);
                person.OnChangeLeader += new Person.ChangeLeader(this.person_OnChangeLeader);
                person.OnDeathChangeFaction += new Person.DeathChangeFaction(this.person_OnDeathChangeFaction);
                person.OnStudyTitleFinished += new Person.StudyTitleFinished(this.person_OnStudyTitleFinished);
                person.OnStudySkillFinished += new Person.StudySkillFinished(this.person_OnStudySkillFinished);
                person.OnStudyStuntFinished += new Person.StudyStuntFinished(this.person_OnStudyStuntFinished);
                person.OnBeAwardedTreasure += new Person.BeAwardedTreasure(this.person_OnBeAwardedTreasure);
                person.OnBeConfiscatedTreasure += new Person.BeConfiscatedTreasure(this.person_OnBeConfiscatedTreasure);
                person.OnCapturedByArchitecture += new Person.CapturedByArchitecture(this.person_OnCapturedByArchitecture);
                person.OnCreateBrother += new Person.CreateBrother(this.person_OnCreateBrother);
                person.OnCreateSister += new Person.CreateSister(person_OnCreateSister);
                person.OnCreateSpouse += new Person.CreateSpouse(person_OnCreateSpouse);
            //}
        }

        public void PurifyInfluences()
        {
            //foreach (Person person in base.GameObjects)
            //{
            //    if (!Session.Current.Scenario.Preparing)
            //    {
            //        foreach (Title t in person.Titles)
            //        {
            //            t.Influences.PurifyInfluence(person, Applier.Title, t.ID, false);
            //        }
            //        foreach (Skill s in person.Skills.GetSkillList())
            //        {
            //            s.Influences.PurifyInfluence(person, Applier.Skill, s.ID, false);
            //        }
            //        foreach (Stunt s in person.Stunts.GetStuntList())
            //        {
            //            s.Influences.PurifyInfluence(person, Applier.Stunt, 0, false);
            //        }
            //        person.PurifyAllTreasures(false);
            //    }
            //}
            bool isPreparing = Session.Current?.Scenario?.Preparing ?? false;
            if (isPreparing) return;

            // 使用for循环，避免foreach的枚举器开销
            for (int i = 0; i < base.Count; i++)
            {
                Person person = base[i] as Person;
                if (person == null) continue;

                // 优化Title处理
                var titles = person.Titles;
                for (int j = 0; j < titles.Count; j++)
                {
                    var title = titles[j] as Title;
                    title.Influences?.PurifyInfluence(person, Applier.Title, title.ID, false);
                }

                // 优化Skill处理
                var skills = person.Skills?.GetSkillList();
                if (skills != null)
                {
                    for (int j = 0; j < skills.Count; j++)
                    {
                        var skill = skills[j] as Skill;
                        skill.Influences?.PurifyInfluence(person, Applier.Skill, skill.ID, false);
                    }
                }

                // 优化Stunt处理
                var stunts = person.Stunts?.GetStuntList();
                if (stunts != null)
                {
                    for (int j = 0; j < stunts.Count; j++)
                    {
                        var stunt = stunts[j] as Stunt;
                        stunt.Influences?.PurifyInfluence(person, Applier.Stunt, 0, false);
                    }
                }

                person.PurifyAllTreasures(false);
            }
        }

        public void ApplyInfluences()
        {           
            // 使用并行处理（如果线程安全）
            if (base.GameObjects.Count > 100)
            {
                Parallel.ForEach(base.GameObjects.OfType<Person>(), person =>
                {
                    person.ApplyTitles(false);
                    person.ApplySkills(false);
                    person.ApplyStunts();
                    person.ApplyAllTreasures(false);
                });
            }
            else
            {
                foreach (Person person in base.GameObjects)
                {
                    person.ApplyTitles(false);
                    person.ApplySkills(false);
                    person.ApplyStunts();
                    person.ApplyAllTreasures(false);
                }
            }

        }
        // 条件筛选（返回PersonList）
        public PersonList Where(Func<Person, bool> predicate)
        {
            PersonList result = new PersonList();
            for (int i = 0; i < base.Count; i++)
            {
                if (base[i] is Person person && predicate(person))
                {
                    result.Add(person);
                }
            }
            return result;
        }
        // 第一个满足条件的Person
        public Person FirstOrDefault(Func<Person, bool> predicate = null)
        {
            if (predicate == null)
            {
                for (int i = 0; i < base.Count; i++)
                {
                    if (base[i] is Person person)
                    {
                        return person;
                    }
                }
            }
            else
            {
                for (int i = 0; i < base.Count; i++)
                {
                    if (base[i] is Person person && predicate(person))
                    {
                        return person;
                    }
                }
            }
            return null;
        }
        // 检查是否存在满足条件的Person
        public bool Any(Func<Person, bool> predicate = null)
        {
            if (predicate == null)
            {
                for (int i = 0; i < base.Count; i++)
                {
                    if (base[i] is Person)
                    {
                        return true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < base.Count; i++)
                {
                    if (base[i] is Person person && predicate(person))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // 统计满足条件的Person数量
        public int CountByPredicate(Func<Person, bool> predicate = null)
        {
            if (predicate == null)
            {
                // 统计所有Person数量
                int count = 0;
                for (int i = 0; i < base.Count; i++)
                {
                    if (base[i] is Person)
                    {
                        count++;
                    }
                }
                return count;
            }
            else
            {
                int count = 0;
                for (int i = 0; i < base.Count; i++)
                {
                    if (base[i] is Person person && predicate(person))
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        // 对每个Person执行操作
        public void ForEach(Action<Person> action)
        {
            for (int i = 0; i < base.Count; i++)
            {
                if (base[i] is Person person)
                {
                    action(person);
                }
            }
        }
        
        // 通用方法：获取某项属性最大的人物
        public Person GetMaxBy(Func<Person, int> selector)
        {
            Person maxPerson = null;
            int maxValue = int.MinValue;

            for (int i = 0; i < base.Count; i++)
            {
                Person person = base[i] as Person;
                if (person != null)
                {
                    int value = selector(person);
                    if (value > maxValue)
                    {
                        maxValue = value;
                        maxPerson = person;
                    }
                }
            }

            return maxPerson;
        }
        // 使用通用方法重写原有方法
        //public Person GetMaxStrengthPerson() => GetMaxBy(p => p.Strength);
        //public Person GetMaxMeritPerson() => GetMaxBy(p => p.Merit);
        //public Person GetMaxUntiredMeritPerson() => GetMaxBy(p => p.UntiredMerit);
        //public Person GetMaxControversyAbilityPerson() => GetMaxBy(p => p.ControversyAbility); 

        public Person GetMaxCommandPerson()
        {
            int command = -1;
            Person person = null;
            foreach (Person person2 in base.GameObjects)
            {
                if (person2.Command > command)
                {
                    command = person2.Command;
                    person = person2;
                }
            }
            return person;
        }

        public Person GetMaxControversyAbilityPerson()
        {
            int controversyAbility = -1;
            Person person = null;
            foreach (Person person2 in base.GameObjects)
            {
                if (person2.ControversyAbility > controversyAbility)
                {
                    controversyAbility = person2.ControversyAbility;
                    person = person2;
                }
            }
            return person;
        }

        public Person GetMaxIntelligencePerson()
        {
            int intelligence = -1;
            Person person = null;
            foreach (Person person2 in base.GameObjects)
            {
                if (person2.Intelligence > intelligence)
                {
                    intelligence = person2.Intelligence;
                    person = person2;
                }
            }
            return person;
        }

        public Person GetMaxMeritPerson()
        {
            int merit = -1;
            Person person = null;
            foreach (Person person2 in base.GameObjects)
            {
                if (person2.Merit > merit)
                {
                    merit = person2.Merit;
                    person = person2;
                }
            }
            return person;
        }

        public Person GetMaxUntiredMeritPerson()
        {
            int merit = -1;
            Person person = null;
            foreach (Person person2 in base.GameObjects)
            {
                if (person2.UntiredMerit > merit)
                {
                    merit = person2.Merit;
                    person = person2;
                }
            }
            return person;
        }

        public Person GetMaxStrengthPerson()
        {
            int strength = -1;
            Person person = null;
            foreach (Person person2 in base.GameObjects)
            {
                if (person2.Strength > strength)
                {
                    strength = person2.Strength;
                    person = person2;
                }
            }
            return person;
        }

        /// <summary>
        /// 人物授予宝物事件
        /// </summary>
        /// <param name="person"></param>
        /// <param name="t"></param>
        private void person_OnBeAwardedTreasure(Person person, Treasure t)
        {
            Session.MainGame.mainGameScreen.PersonBeAwardedTreasure(person, t);
        }

        /// <summary>
        /// 人物没收宝物事件
        /// </summary>
        /// <param name="person"></param>
        /// <param name="t"></param>
        private void person_OnBeConfiscatedTreasure(Person person, Treasure t)
        {
            Session.MainGame.mainGameScreen.PersonBeConfiscatedTreasure(person, t);
        }

        private void person_OnBeKilled(Person person, Architecture location)
        {
            Session.MainGame.mainGameScreen.PersonBeKilled(person, location);
        }

        private void person_OnChangeLeader(Faction faction, Person leader, bool changeName, string oldName)
        {
            Session.MainGame.mainGameScreen.PersonChangeLeader(faction, leader, changeName, oldName);
        }
        /*
        private void person_OnQuanXiangFailed(Person source, Faction targetFaction)
        {
            Session.MainGame.mainGameScreen.QuanXiangFailed(source, targetFaction);
        }
        */
        private void person_OnConvinceFailed(Person source, Person destination)
        {
            Session.MainGame.mainGameScreen.PersonConvinceFailed(source, destination);
        }

        private void person_OnConvinceSuccess(Person source, Person destination, Faction oldFaction)
        {
            Session.MainGame.mainGameScreen.PersonConvinceSuccess(source, destination, oldFaction);
        }

        private void person_OnDeath(Person person, Person killer, Architecture location, Troop locationTroop)
        {
            Session.MainGame.mainGameScreen.PersonDeath(person, killer, location, locationTroop);
        }

        private void person_OnDeathChangeFaction(Person dead, Person leader, string oldName)
        {
            Session.MainGame.mainGameScreen.PersonDeathChangeFaction(dead, leader, oldName);
        }

        private void person_OnDestroyFailed(Person person, Architecture architecture)
        {
            Session.MainGame.mainGameScreen.PersonDestroyFailed(person, architecture);
        }

        private void person_OnDestroySuccess(Person person, Architecture architecture, int down)
        {
            Session.MainGame.mainGameScreen.PersonDestroySuccess(person, architecture, down);
        }

        private void person_OnGossipFailed(Person person, Architecture architecture)
        {
            Session.MainGame.mainGameScreen.PersonGossipFailed(person, architecture);
        }

        private void person_OnGossipSuccess(Person person, Architecture architecture)
        {
            Session.MainGame.mainGameScreen.PersonGossipSuccess(person, architecture);
        }

        private void person_OnInformationObtained(Person person, Information information)
        {
            Session.MainGame.mainGameScreen.PersonInformationObtained(person, information);
        }
        
        private void person_qingbaoshibai(Person person)
        {
            Session.MainGame.mainGameScreen.qingbaoshibai(person);
        }

        private void person_OnInstigateFailed(Person person, Architecture architecture)
        {
            Session.MainGame.mainGameScreen.PersonInstigateFailed(person, architecture);
        }

        private void person_OnInstigateSuccess(Person person, Architecture architecture, int down)
        {
            Session.MainGame.mainGameScreen.PersonInstigateSuccess(person, architecture, down);
        }

        private void person_OnLeave(Person person, Architecture location)
        {
            Session.MainGame.mainGameScreen.PersonLeave(person, location);
        }

        private void person_OnSearchFinished(Person person, Architecture architecture, SearchResultPack resultPack)
        {
            Session.MainGame.mainGameScreen.PersonSearchFinished(person, architecture, resultPack);
        }
        /*
        private void person_OnShowMessage(Person person, PersonMessage personMessage)
        {
            Session.MainGame.mainGameScreen.PersonShowMessage(person, personMessage);
        }
        
        private void person_OnSpyFailed(Person person, Architecture architecture)
        {
            Session.MainGame.mainGameScreen.PersonSpyFailed(person, architecture);
        }

        private void person_OnSpyFound(Person person, Person spy)
        {
            Session.MainGame.mainGameScreen.PersonSpyFound(person, spy);
        }

        private void person_OnSpySuccess(Person person, Architecture architecture)
        {
            Session.MainGame.mainGameScreen.PersonSpySuccess(person, architecture);
        }
        */
        private void person_OnStudySkillFinished(Person person, string skillString, bool success)
        {
            Session.MainGame.mainGameScreen.PersonStudySkillFinished(person, skillString, success);
        }

        private void person_OnStudyStuntFinished(Person person, Stunt stunt, bool success)
        {
            Session.MainGame.mainGameScreen.PersonStudyStuntFinished(person, stunt, success);
        }

        private void person_OnStudyTitleFinished(Person person, Title title, bool success)
        {
            Session.MainGame.mainGameScreen.PersonStudyTitleFinished(person, title, success);
        }

        private void person_OnTreasureFound(Person person, Treasure treasure)
        {
            Session.MainGame.mainGameScreen.PersonTreasureFound(person, treasure);
        }

        private void person_OnCapturedByArchitecture(Person person, Architecture architecture)
        {
            Session.MainGame.mainGameScreen.PersonCapturedByArchitecture(person, architecture);
        }

        void person_OnJailBreakSuccess(Person source, Captive destination)
        {
            Session.MainGame.mainGameScreen.PersonJailBreak(source, destination);
        }

        void person_OnJailBreakFailed(Person source, Architecture destination)
        {
            Session.MainGame.mainGameScreen.PersonJailBreakFailed(source, destination);
        }

        void person_OnCreateBrother(Person p, Person q)
        {
            Session.MainGame.mainGameScreen.CreateBrother(p, q);
        }

        void person_OnCreateSister(Person p, Person q)
        {
            Session.MainGame.mainGameScreen.CreateSister(p, q);
        }

        void person_OnCreateSpouse(Person p, Person q)
        {
            Session.MainGame.mainGameScreen.CreateSpouse(p, q);
        }
    }
}
