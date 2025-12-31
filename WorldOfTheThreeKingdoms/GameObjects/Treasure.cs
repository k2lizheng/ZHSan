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
            
            if (r.Worth < 5)
            {
                //if (isAI && GameObject.Chance(30))
                //{
                //    r.Influences.AddInfluence(Session.Current.Scenario.GameCommonData.AllInfluences.GetInfluence(262));
                //}
                //else { 
                // foreach (Influence s in Session.Current.Scenario.GameCommonData.AllInfluences.Influences.Values)
                //{                    
                // if (GameObject.Chance(3))
                // {
                //r.Influences.AddInfluence(s);
                r.Description += "（催生啦》》》》》》）";
                r.worth = 1;
                r.TreasureGroup = 1312;
                    //break;
                    //}                   
                    //} 
                //}
             }                                  
        }
        private static void HandleGroup(Treasure r, PersonGenerateParam param)
        {

            string p = param.PreferredType.Name;
            List<int> influenceList= new List<int> { };
            switch (p)
            {
                case "全能":
                    r.TreasureGroup = 3214; r.Pic = 1376;r.Name += "玉玺";
                    if (Platform.Current.FileExists("Content/Data/Treasure/3214.txt"))
                    {
                        influenceList = Person.readNumberList("Content/Data/Treasure/3214.txt");
                    }
                    else
                    {
                        influenceList = new List<int> { 6190,6411,6253,6252,6251,6250,6180,6182,6183,6191 };
                    }
                    foreach (Influence i in Session.Current.Scenario.GameCommonData.AllInfluences.Influences.Values)
                    {

                        int ss = Random(1, 500);
                        if (i.Kind.ID >=320 &&  ss == 10 && i.Description != null)
                        {
                            r.Influences.AddInfluence(i);
                            if (r.Influences.Count > 0)
                                break;
                        }

                    }
                    break;
                case "将军":
                    r.TreasureGroup = 1100; r.Pic = 1; r.Name += "武器";
                    if (Platform.Current.FileExists("Content/Data/Treasure/1100.txt"))
                    {
                        influenceList = Person.readNumberList("Content/Data/Treasure/1100.txt");
                    }
                    else
                    {
                        influenceList = new List<int> { 6010, 6011, 6012, 6013, 6014, 6015, 6016, 6017, 6018, 6019, };
                    }
                    break;
                case "君主":
                    r.TreasureGroup = 300; r.Pic = 1001; r.Name += "名马";
                   
                    r.Influences.AddInfluence(Session.Current.Scenario.GameCommonData.AllInfluences.GetInfluence(281));
                    if (Platform.Current.FileExists("Content/Data/Treasure/300.txt"))
                    {
                        influenceList = Person.readNumberList("Content/Data/Treasure/300.txt");
                    }
                    else
                    {
                        influenceList = new List<int> { 7532,250,7083,220,7082,7080,7079,7081,251,7084,207,252,261,262,263};
                    }
                    break;
                case "智将":
                    r.TreasureGroup = 1300; r.Pic = 1211; r.Name += "政书";
                   
                    if (Platform.Current.FileExists("Content/Data/Treasure/1300.txt"))
                    {
                        influenceList = Person.readNumberList("Content/Data/Treasure/1300.txt");
                    }
                    else
                    {
                        influenceList = new List<int> {53,124,120,7292,7293,7294,7295,7296, 0,1,2,3,4,5,10,11,12,13,14,15,121,122,123 };
                    }
                    break;
                case "女官":
                    r.TreasureGroup = 2900; r.Pic = 1352; r.Name += "乐器";
                   
                    if (Platform.Current.FileExists("Content/Data/Treasure/2900.txt"))
                    {
                        influenceList = Person.readNumberList("Content/Data/Treasure/2900.txt");
                    }
                    else
                    {
                        influenceList = new List<int> { 610,623,622,661,621,620,630,631,660,650,651,609,611,};
                    }
                    break;
                case "识者":
                    r.TreasureGroup = 2001; r.Pic = 1312; r.Name += "药";
                    
                    if (Platform.Current.FileExists("Content/Data/Treasure/2001.txt"))
                    {
                        influenceList = Person.readNumberList("Content/Data/Treasure/2001.txt");
                    }
                    else
                    {
                        influenceList = new List<int> { 6823,6822,6821,6801,6800,6820,6820,6810,6811,6821,6822,6823 };
                    }
                    break;
                case "文官":
                    r.TreasureGroup = 3200; r.Pic = 1332; r.Name += "器具";
                    
                    if (Platform.Current.FileExists("Content/Data/Treasure/3200.txt"))
                    {
                        influenceList = Person.readNumberList("Content/Data/Treasure/3200.txt");
                    }
                    else
                    {
                        influenceList = new List<int> { 7265,6024,6014,6004,6000,6010,6020,6030,6040,6001,6011,6021,6031,6041,6044,6034,7263 };
                    }
                    break;
                case "武官":
                    r.TreasureGroup = 4100; r.Pic = 43; r.Name += "头盔";
                    
                    if (Platform.Current.FileExists("Content/Data/Treasure/4100.txt"))
                    {
                        influenceList = Person.readNumberList("Content/Data/Treasure/4100.txt");
                    }
                    else
                    {
                        influenceList = new List<int> { 7239,5080,5091,5061,6002,5060,5070,6001,5071,5090,5081,7255 };
                    }
                    break;
                case "猛将":
                    r.TreasureGroup = 4000; r.Pic = 3048; r.Name += "铠甲";
                   
                    if (Platform.Current.FileExists("Content/Data/Treasure/4000.txt"))
                    {
                        influenceList = Person.readNumberList("Content/Data/Treasure/4000.txt");
                    }
                    else
                    {
                        influenceList = new List<int> { 604,7179,511,530,401,405,420,421,531,510,7179,600 };
                    }
                    break;
                case "军师":
                    r.TreasureGroup = 1200; r.Pic = 1205; r.Name += "兵书";
                   
                    if (Platform.Current.FileExists("Content/Data/Treasure/1200.txt"))
                    {
                        influenceList = Person.readNumberList("Content/Data/Treasure/1200.txt");
                    }
                    else
                    {
                        influenceList = new List<int> { 572,6220,6570,6905,6990,6705,6540,6515,6715,6995,6945,6595,6210,573};
                    }
                    break;

            }
            int a = Random(0, influenceList.Count - 1);
            int b = Random(0, influenceList.Count - 1);
            int c = (a + b) / 2;
            r.Worth = Math.Abs( c - influenceList.Count/2 ) * 160 / influenceList.Count ;
            try
            {
                Influence s = Session.Current.Scenario.GameCommonData.AllInfluences.GetInfluence(influenceList[c]); 
                r.Influences.AddInfluence(s);
            }
            catch { }
           

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
            //r.worth = 39;
            r.Available = true;
            HandleInfluencesString(r,isAI); 
            Session.Current.Scenario.Treasures.AddTreasure(r);
            return r;
        }

    }
}

