using GameManager;
using GameObjects.Influences;
using GameObjects.PersonDetail;
using Microsoft.Xna.Framework.Graphics;
using Platforms;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using WorldOfTheThreeKingdoms;



namespace GameObjects
{
    [DataContract]
    public class Treasure : GameObject
    {
        private int appearYear;
        private bool available;

        [DataMember]
        public int BelongedPersonIDString { get; set; }

        public Person BelongedPerson;
        private string description;

        [DataMember]
        public int HidePlaceIDString { get; set; }

        public Architecture HidePlace;

        [DataMember]
        public string InfluencesString { get; set; }

        public InfluenceTable Influences = new InfluenceTable();
        private int pic;
        private PlatformTexture picture;
        private int worth;

        public void Init()
        {
            Influences = new InfluenceTable();
        }

        [DataMember]
        public int TreasureGroup
        {
            get;
            set;
        }

        [DataMember]
        public int AppearYear
        {
            get
            {
                return this.appearYear;
            }
            set
            {
                this.appearYear = value;
            }
        }

        [DataMember]
        public bool Available
        {
            get
            {
                return this.available;
            }
            set
            {
                this.available = value;
            }
        }

        public string BelongedPersonString
        {
            get
            {
                return ((this.BelongedPerson != null) ? this.BelongedPerson.Name : "----");
            }
        }

        [DataMember]
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        public string HidePlaceString
        {
            get
            {
                return ((this.HidePlace != null) ? this.HidePlace.Name : "----");
            }
        }

        public string InfluenceString
        {
            get
            {
                string str = "";
                foreach (Influence influence in this.Influences.Influences.Values)
                {
                    str = str + "•" + influence.Description;
                }
                return str;
            }
        }

        [DataMember]
        public int Pic
        {
            get
            {
                return this.pic;
            }
            set
            {
                this.pic = value;
            }
        }

        //public void disposeTexture()
        //{
        //    if (this.picture != null)
        //    {
        //        this.picture.Dispose();
        //        this.picture = null;
        //    }
        //}

        public PlatformTexture Picture
        {
            get
            {
                if (this.picture == null)
                {
                    try
                    {
                        this.picture = CacheManager.GetTempTexture("Content/Textures/Resources/Treasure/" + this.Pic.ToString() + ".png");                        
                    }
                    catch
                    {                       
                        this.picture = null;
                    }
                }
                return this.picture;
            }
        }

        [DataMember]
        public int Worth
        {
            get
            {
                return this.worth;
            }
            set
            {
                this.worth = value;
            }
        }

        private static Treasure HandleID()
        {
            Treasure r = new Treasure();

            //look for empty id
            int id = 25000;
            TreasureList pl = Session.Current.Scenario.Treasures as TreasureList;
            pl.SmallToBig = true;
            pl.IsNumber = true;
            pl.PropertyName = "ID";
            pl.ReSort();
            foreach (Treasure p in pl)
            {
                if (p.ID == id)
                {
                    id++;

                }
                else if (p.ID > id)
                {
                    break;
                }
            }
            r.ID = id;
            return r;
        }

        private static void HandleInfluencesString(Treasure r, bool isAI)
        {
            //Dictionary<InfluenceKind, List<Influence>> influences = Influence.GetKindInfluenceDictionary();
            //foreach (KeyValuePair<InfluenceKind, List<Influence>> kv in influences)
            //{
            //    Dictionary<Influence ,String> chances = new Dictionary<Influence, String>();
            //    foreach (Influence t in kv.Value)
            //    {
            //        if (t.CanBeChosenForGenerated(r))
            //        {
            //            chances.Add(t, t.GenerationChance[(int)officerType]);
            //        }
            //    }

            //    if (chances.Count > 0)
            //    {
            //         r.Influences.AddInfluence(t);
            //    }
            //}  
            
            if (r.Influences.Count < 1)
            {
                if (isAI)
                {
                    r.Influences.AddInfluence(Session.Current.Scenario.GameCommonData.AllInfluences.GetInfluence(262));
                }
                else { 
                // foreach (Influence s in Session.Current.Scenario.GameCommonData.AllInfluences.Influences.Values)
                //{                    
                // if (GameObject.Chance(3))
                // {
                //r.Influences.AddInfluence(s);
                r.Description += "（失败作品-似乎还有其他用处）";
                r.worth = 1;
                r.TreasureGroup = 1312;
                    //break;
                    //}                   
                    //} 
                }
             }                                  
        }
        private static void HandleGroup(Treasure r, PersonGenerateParam param)
        {

            string p = param.PreferredType.Name;
            switch (p)
            {
                case "全能":
                    r.TreasureGroup = 3214; r.Pic = 1376;r.Name += "玉玺";
                    foreach (Influence s in Session.Current.Scenario.GameCommonData.AllInfluences.Influences.Values)
                    {

                        int b = Random(1, 500);
                        if (s.Kind.ID >=320 &&  b == 10)
                        {
                            r.Influences.AddInfluence(s);
                            if (r.Influences.Count > 1)
                                break;
                        }

                    }
                    break;
                case "将军":
                    r.TreasureGroup = 1100; r.Pic = 1; r.Name += "武器";
                    foreach (Influence s in Session.Current.Scenario.GameCommonData.AllInfluences.Influences.Values)
                    {                       
                        if (s.Kind.ID ==6010 && GameObject.Chance(10))
                        {
                            r.Influences.AddInfluence(s);
                            if (r.Influences.Count > 0)
                                break;
                        }                                                  
                    }
                    break;
                case "君主":
                    r.TreasureGroup = 300; r.Pic = 1001; r.Name += "名马";
                    foreach (Influence s in Session.Current.Scenario.GameCommonData.AllInfluences.Influences.Values)
                    {      
                        if(s.Kind.ID == 281) { r.Influences.AddInfluence(s);}
                        if ((s.Kind.ID >= 250 && s.Kind.ID <= 263 || s.Kind.ID == 6270 || s.Kind.ID == 6280 ) && s.ID > 281 && GameObject.Chance(10))
                        {
                            r.Influences.AddInfluence(s);
                            if (r.Influences.Count > 0)
                                break;
                        }
                    }
                    break;
                case "智将":
                    r.TreasureGroup = 1300; r.Pic = 1211; r.Name += "政书";
                    foreach (Influence s in Session.Current.Scenario.GameCommonData.AllInfluences.Influences.Values)
                    {                       
                        if ( s.Kind.ID <= 160 && GameObject.Chance(1))
                        {
                            r.Influences.AddInfluence(s);
                            if (r.Influences.Count > 0)
                                break;
                        }
                    }
                    break;
                case "女官":
                    r.TreasureGroup = 2900; r.Pic = 1352; r.Name += "乐器";
                    foreach (Influence s in Session.Current.Scenario.GameCommonData.AllInfluences.Influences.Values)
                    {             
                        if (s.Kind.ID <= 660 && s.Kind.ID >= 609 && GameObject.Chance(5))
                        {
                            r.Influences.AddInfluence(s);
                            if (r.Influences.Count > 0)
                                break;
                        }
                    }
                    break;
                case "识者":
                    r.TreasureGroup = 2001; r.Pic = 1312; r.Name += "药";
                    foreach (Influence s in Session.Current.Scenario.GameCommonData.AllInfluences.Influences.Values)
                    {
                        if (s.Kind.ID == 6820 && GameObject.Chance(30))
                        {
                            r.Influences.AddInfluence(s);
                            if (r.Influences.Count > 0)
                                break;
                        }
                    }
                    break;
                case "文官":
                    r.TreasureGroup = 3200; r.Pic = 1332; r.Name += "器具";
                    foreach (Influence s in Session.Current.Scenario.GameCommonData.AllInfluences.Influences.Values)
                    {
                        if (s.Kind.ID <= 6040 && s.Kind.ID >= 6020 && GameObject.Chance(5))
                        {
                            r.Influences.AddInfluence(s);
                            if (r.Influences.Count > 0)
                                break;
                        }
                    }
                    break;
                case "武官":
                    r.TreasureGroup = 4100; r.Pic = 43; r.Name += "头盔";
                    foreach (Influence s in Session.Current.Scenario.GameCommonData.AllInfluences.Influences.Values)
                    {
                        if ((s.Kind.ID <= 5090 && s.Kind.ID >= 5060  || s.Kind.ID == 6000)&& GameObject.Chance(5))
                        {
                            r.Influences.AddInfluence(s);
                            if (r.Influences.Count > 0)
                                break;
                        }
                    }
                    break;
                case "猛将":
                    r.TreasureGroup = 4000; r.Pic = 3048; r.Name += "铠甲";
                    foreach (Influence s in Session.Current.Scenario.GameCommonData.AllInfluences.Influences.Values)
                    {
                        if ((s.Kind.ID ==400 || s.Kind.ID ==405  || s.Kind.ID ==420)&& GameObject.Chance(10))
                        {
                            r.Influences.AddInfluence(s);
                            if (r.Influences.Count > 0)
                                break;
                        }
                    }
                    break;
                case "军师":
                    r.TreasureGroup = 1200; r.Pic = 1205; r.Name += "兵书";
                    foreach (Influence s in Session.Current.Scenario.GameCommonData.AllInfluences.Influences.Values)
                    {
                        if ((s.Kind.ID <= 6595 && s.Kind.ID >= 6570 || s.Kind.ID >= 6900)&& GameObject.Chance(4))
                        {
                            r.Influences.AddInfluence(s);
                            if (r.Influences.Count > 0)
                                break;
                        }
                    }
                    break;
            }
                                         
        }

        public static Treasure createTreasure(PersonGenerateParam param, bool isAI)
        {
            
            Treasure r = HandleID();
            r.Name = "z"+r.ID;
            r.Name = r.Name.Remove(0, r.Name.Length - 3);
            HandleGroup(r, param);
                                
            r.AppearYear = 0;
            r.Description =Session.Current.Scenario.Date+ "委托"+param.PreferredType+"打造于"+param.FoundLocation.Name;
            r.HidePlaceIDString = -1;
            r.worth = 39;
            r.Available = true;
            HandleInfluencesString(r,isAI); 
            Session.Current.Scenario.Treasures.AddTreasure(r);
            return r;
        }

    }
}

