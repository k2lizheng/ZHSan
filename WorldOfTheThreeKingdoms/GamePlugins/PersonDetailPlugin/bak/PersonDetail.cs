namespace PersonDetailPlugin
{
    using GameFreeText;
    using GameGlobal;
    using GameManager;
    using GameObjects;
    using GameObjects.Conditions;
    using GameObjects.Influences;
    using GameObjects.PersonDetail;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System;
    using PluginInterface;
    using System.Xml;
    //using System.Drawing;
    using System.Collections.Generic;

    internal class PersonDetail
    {
        // Fields
        internal FreeTextList AllSkillTexts;
        internal Rectangle TitleClient;
        internal FreeTextList LearnableSkillTexts;
        internal FreeTextList PersonSkillTexts;
        internal Rectangle PortraitClient;
        internal Screen screen;
        internal Person ShowingPerson;
        internal Point SkillBlockSize;
        internal Point SkillDisplayOffset;
        internal Rectangle StuntClient;
        internal FreeRichText StuntText = new FreeRichText();
        internal FreeText SurNameText;
        internal FreeRichText TitleText = new FreeRichText();
        internal Point BackgroundSize;
        internal PlatformTexture BackgroundTexture;
        internal Rectangle BiographyClient;
        internal FreeRichText BiographyText = new FreeRichText();
        internal FreeText CalledNameText;
        internal Rectangle ConditionClient;
        internal FreeRichText ConditionText = new FreeRichText();
        private object current;
        private Point DisplayOffset;
        internal FreeText GivenNameText;
        internal Rectangle InfluenceClient;
        internal FreeRichText InfluenceText = new FreeRichText();
        private bool isShowing;
        internal List<LabelText> LabelTexts = new List<LabelText>();
        internal Rectangle MoreMessageBGClient;
        internal Point MoreMessageBGSize;
        internal PlatformTexture MoreMessageBGTexture;
        internal Rectangle MoreMessageClient;
        internal FreeRichText MoreMessageText = new FreeRichText();
        internal Rectangle Skill0ID0Client;
        internal Rectangle Skill10ID10Client;
        internal Rectangle Skill11ID11Client;
        internal Rectangle Skill12ID12Client;
        internal Rectangle Skill13ID13Client;
        internal Rectangle Skill14ID14Client;
        internal Rectangle Skill15ID15Client;
        internal Rectangle Skill16ID16Client;
        internal Rectangle Skill17ID17Client;
        internal Rectangle Skill18ID18Client;
        internal Rectangle Skill19ID19Client;
        internal Rectangle Skill1ID1Client;
        internal Rectangle Skill20ID20Client;
        internal Rectangle Skill21ID21Client;
        internal Rectangle Skill22ID22Client;
        internal Rectangle Skill23ID23Client;
        internal Rectangle Skill24ID24Client;
        internal Rectangle Skill25ID25Client;
        internal Rectangle Skill26ID26Client;
        internal Rectangle Skill27ID27Client;
        internal Rectangle Skill28ID28Client;
        internal Rectangle Skill29ID29Client;
        internal Rectangle Skill2ID2Client;
        internal Rectangle Skill3ID3Client;
        internal Rectangle Skill4ID4Client;
        internal Rectangle Skill5ID5Client;
        internal Rectangle Skill6ID6Client;
        internal Rectangle Skill7ID7Client;
        internal Rectangle Skill8ID8Client;
        internal Rectangle Skill9ID9Client;
        internal PlatformTexture SkillBG;
        internal Rectangle SkillBGClient;
        internal Point SkillBGSize;
        internal Rectangle Stunt0ID0Client;
        internal Rectangle Stunt10ID10Client;
        internal Rectangle Stunt11ID11Client;
        internal Rectangle Stunt12ID12Client;
        internal Rectangle Stunt13ID13Client;
        internal Rectangle Stunt14ID14Client;
        internal Rectangle Stunt15ID15Client;
        internal Rectangle Stunt16ID16Client;
        internal Rectangle Stunt17ID17Client;
        internal Rectangle Stunt18ID18Client;
        internal Rectangle Stunt19ID19Client;
        internal Rectangle Stunt1ID1Client;
        internal Rectangle Stunt20ID20Client;
        internal Rectangle Stunt21ID21Client;
        internal Rectangle Stunt22ID22Client;
        internal Rectangle Stunt23ID23Client;
        internal Rectangle Stunt24ID24Client;
        internal Rectangle Stunt25ID25Client;
        internal Rectangle Stunt26ID26Client;
        internal Rectangle Stunt27ID27Client;
        internal Rectangle Stunt28ID28Client;
        internal Rectangle Stunt29ID29Client;
        internal Rectangle Stunt2ID2Client;
        internal Rectangle Stunt3ID3Client;
        internal Rectangle Stunt4ID4Client;
        internal Rectangle Stunt5ID5Client;
        internal Rectangle Stunt6ID6Client;
        internal Rectangle Stunt7ID7Client;
        internal Rectangle Stunt8ID8Client;
        internal Rectangle Stunt9ID9Client;
        internal PlatformTexture StuntBG;
        internal Rectangle StuntBGClient;
        internal Point StuntBGSize;
        public string Switch_DisplayFamily = "";
        public string Switch_MoreMessage = "";
        internal Rectangle Title10kind5Client;
        internal Rectangle Title11kind600Client;
        internal Rectangle Title12kind700Client;
        internal Rectangle Title1kind1Client;
        internal Rectangle Title2kind200Client;
        internal Rectangle Title3kind10Client;
        internal Rectangle Title4kind2Client;
        internal Rectangle Title5kind21Client;
        internal Rectangle Title6kind7Client;
        internal Rectangle Title7kind3Client;
        internal Rectangle Title8kind4Client;
        internal Rectangle Title9kind50Client;
        internal PlatformTexture TitleBG;
        internal Rectangle TitleBGClient;
        internal Point TitleBGSize;
        private List<int> TitleKind;
        internal Rectangle Treasure10group70Client;
        internal Rectangle Treasure11group90Client;
        internal Rectangle Treasure12group100Client;
        internal Rectangle Treasure1group10Client;
        internal Rectangle Treasure2group15Client;
        internal Rectangle Treasure3group20Client;
        internal Rectangle Treasure4group25Client;
        internal Rectangle Treasure5group30Client;
        internal Rectangle Treasure6group40Client;
        internal Rectangle Treasure7group50Client;
        internal Rectangle Treasure8group55Client;
        internal Rectangle Treasure9group60Client;
        internal PlatformTexture TreasureBG;
        internal Rectangle TreasureBGClient;
        internal Point TreasureBGSize;
        private List<int> TreasureGroup;

        // Methods
        public PersonDetail()
        {
            List<int> list = new List<int> {
            10,
            15,
            20,
            0x19,
            30,
            40,
            50,
            0x37,
            60,
            70,
            90,
            100
        };
            this.TreasureGroup = list;
            List<int> list2 = new List<int> {
            1,
            2,
            3,
            4,
            5,
            7,
            10,
            0x15,
            50,
            200,
            600,
            700
        };
            this.TitleKind = list2;
        }

        private Color DetailColor(int DetailLevel)
        {
            switch (DetailLevel)
            {
                case 1:
                    return Color.WhiteSmoke;

                case 2:
                    return Color.FloralWhite;

                case 3:
                    return Color.LightGray;

                case 4:
                    return Color.Aquamarine;

                case 5:
                    return Color.Turquoise;

                case 6:
                    return Color.DeepSkyBlue;

                case 7:
                    return Color.Orchid;

                case 8:
                    return Color.Orange;

                case 9:
                    return Color.Tomato;
            }
            return Color.WhiteSmoke;
        }

        internal void Draw()
        {
            if (this.ShowingPerson != null)
            {
                int current;
                List<int>.Enumerator enumerator3;
                Rectangle? sourceRectangle = null;
                CacheManager.Draw(this.BackgroundTexture, this.BackgroundDisplayPosition, sourceRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.2f);
                try
                {
                    CacheManager.DrawZhsanAvatar(this.ShowingPerson, "", this.PortraitDisplayPosition, Color.White, 0.199f);
                }
                catch
                {
                }
                this.SurNameText.Draw(0.1999f);
                this.GivenNameText.Draw(0.1999f);
                this.CalledNameText.Draw(0.1999f);
                foreach (LabelText text in this.LabelTexts)
                {
                    text.Label.Draw(0.1999f);
                    text.Text.Draw(0.1999f);
                }
                foreach (Treasure treasure in this.ShowingPerson.AllEffectiveTreasures)
                {
                    using (enumerator3 = this.TreasureGroup.GetEnumerator())
                    {
                        while (enumerator3.MoveNext())
                        {
                            current = enumerator3.Current;
                            if (treasure.TreasureGroup == current)
                            {
                                try
                                {
                                    CacheManager.Draw(treasure.Picture, this.TreasuresClientDisplayPosition(current), sourceRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.19f);
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                }



                CacheManager.Draw(this.TreasureBG, this.TreasureBGClientDisplayPosition, sourceRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.199f);
                CacheManager.Draw(this.TitleBG, this.TitleBGClientDisplayPosition, sourceRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.199f);
                CacheManager.Draw(this.StuntBG, this.StuntBGClientDisplayPosition, sourceRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.199f);
                CacheManager.Draw(this.SkillBG, this.SkillBGClientDisplayPosition, sourceRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.199f);
                this.InfluenceText.Draw(0.1999f);
                this.ConditionText.Draw(0.1999f);
                this.BiographyText.Draw(0.1999f);
                if (this.Switch_MoreMessage == "on")
                {
                    this.MoreMessageText.Draw(0.199f);
                    CacheManager.Draw(this.MoreMessageBGTexture, this.MoreMessageBGDisplayPosition, sourceRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.1997f);
                }
            }
        }


        internal void Initialize(Screen screen)
        {
            this.screen = screen;
        }

        private void screen_OnMouseLeftDown(Point position)
        {
            if (StaticMethods.PointInRectangle(position, new Rectangle(this.BiographyText.DisplayOffset.X, this.BiographyText.DisplayOffset.Y, this.BiographyClient.Width, this.BiographyClient.Height)))
            {
                if (this.BiographyText.CurrentPageIndex < (this.BiographyText.PageCount - 1))
                {
                    this.BiographyText.NextPage();
                }
                else if (this.BiographyText.CurrentPageIndex == (this.BiographyText.PageCount - 1))
                {
                    this.BiographyText.FirstPage();
                }
            }
            if ((this.Switch_MoreMessage == "off") && StaticMethods.PointInRectangle(position, this.PortraitDisplayPosition))
            {
                this.MoreMessageText.Clear();
                this.MoreMessageText.AddText("兵科经验", this.MoreMessageText.TitleColor);
                this.MoreMessageText.AddNewLine();
                this.MoreMessageText.AddText("步兵：", Color.WhiteSmoke);
                this.MoreMessageText.AddText(this.ShowingPerson.BubingExperience.ToString(), Color.WhiteSmoke);
                this.MoreMessageText.AddNewLine();
                this.MoreMessageText.AddText("弓弩：", Color.WhiteSmoke);
                this.MoreMessageText.AddText(this.ShowingPerson.NubingExperience.ToString(), Color.WhiteSmoke);
                this.MoreMessageText.AddNewLine();
                this.MoreMessageText.AddText("骑兵：", Color.WhiteSmoke);
                this.MoreMessageText.AddText(this.ShowingPerson.QibingExperience.ToString(), Color.WhiteSmoke);
                this.MoreMessageText.AddNewLine();
                this.MoreMessageText.AddText("水军：", Color.WhiteSmoke);
                this.MoreMessageText.AddText(this.ShowingPerson.ShuijunExperience.ToString(), Color.WhiteSmoke);
                this.MoreMessageText.AddNewLine();
                this.MoreMessageText.AddText("器械：", Color.WhiteSmoke);
                this.MoreMessageText.AddText(this.ShowingPerson.QixieExperience.ToString(), Color.WhiteSmoke);
                this.MoreMessageText.AddNewLine();
                this.MoreMessageText.AddText("其他经验", this.MoreMessageText.TitleColor);
                this.MoreMessageText.AddNewLine();
                this.MoreMessageText.AddText("计略：", Color.WhiteSmoke);
                this.MoreMessageText.AddText(this.ShowingPerson.StratagemExperience.ToString(), Color.WhiteSmoke);
                this.MoreMessageText.AddNewLine();
                this.MoreMessageText.AddText("策略：", Color.WhiteSmoke);
                this.MoreMessageText.AddText(this.ShowingPerson.TacticsExperience.ToString(), Color.WhiteSmoke);
                this.MoreMessageText.AddNewLine();
                this.MoreMessageText.AddText("内政：", Color.WhiteSmoke);
                this.MoreMessageText.AddText(this.ShowingPerson.InternalExperience.ToString(), Color.WhiteSmoke);
                this.MoreMessageText.AddNewLine();
                this.MoreMessageText.AddText("", this.MoreMessageText.TitleColor);
                this.MoreMessageText.AddNewLine();
                if (this.Switch_DisplayFamily == "on")
                {
                    this.MoreMessageText.AddText("生卒年：", this.MoreMessageText.TitleColor);
                    this.MoreMessageText.AddText(this.ShowingPerson.YearBorn + "-" + this.ShowingPerson.YearDead, Color.WhiteSmoke);
                    this.MoreMessageText.AddNewLine();
                }
                this.MoreMessageText.AddText("家族 ", this.MoreMessageText.TitleColor);
                //if (this.Switch_DisplayFamily == "on")
                //{
                //    this.MoreMessageText.AddText(this.ShowingPerson.PersonFamily, Color.Orange);
                //}
                this.MoreMessageText.AddNewLine();
                this.MoreMessageText.AddText("父：", Color.WhiteSmoke);
                this.MoreMessageText.AddText(this.ShowingPerson.FatherName, Color.SkyBlue);
                this.MoreMessageText.AddText(" 母：", Color.WhiteSmoke);
                this.MoreMessageText.AddText(this.ShowingPerson.MotherName, Color.SandyBrown);
                this.MoreMessageText.AddNewLine();
                this.MoreMessageText.AddText("配偶：", Color.WhiteSmoke);
                this.MoreMessageText.AddText(this.ShowingPerson.SpouseName, Color.Turquoise);
                this.MoreMessageText.AddNewLine();
                this.MoreMessageText.AddText("兄弟姐妹：", Color.WhiteSmoke);
                //this.MoreMessageText.AddText(this.ShowingPerson.FamilyName[0], Color.SkyBlue);
                //this.MoreMessageText.AddText(this.ShowingPerson.FamilyName[1], Color.SandyBrown);
                this.MoreMessageText.AddNewLine();
                this.MoreMessageText.AddText("子女：", Color.WhiteSmoke);
                //this.MoreMessageText.AddText(this.ShowingPerson.FamilyName[2], Color.SkyBlue);
                //this.MoreMessageText.AddText(this.ShowingPerson.FamilyName[3], Color.SandyBrown);
                this.MoreMessageText.AddNewLine();
                this.MoreMessageText.AddText("孙：", Color.WhiteSmoke);
                //this.MoreMessageText.AddText(this.ShowingPerson.FamilyName[4], Color.SkyBlue);
                //this.MoreMessageText.AddText(this.ShowingPerson.FamilyName[5], Color.SandyBrown);
                this.MoreMessageText.AddNewLine();
                this.MoreMessageText.AddText("亲近：", this.MoreMessageText.TitleColor);
                //this.MoreMessageText.AddText(this.ShowingPerson.ClosePersonsName, Color.Turquoise);
                this.MoreMessageText.AddNewLine();
                this.MoreMessageText.ResortTexts();
                this.Switch_MoreMessage = "on";
            }
            else if ((this.Switch_MoreMessage == "on") && StaticMethods.PointInRectangle(position, this.PortraitDisplayPosition))
            {
                this.MoreMessageText.Clear();
                this.Switch_MoreMessage = "off";
            }
        }

        private void screen_OnMouseMove(Point position, bool leftDown)
        {
            int num;
            bool flag = false;
            if (!flag)
            {
                for (num = 0; num < this.TreasureGroup.Count; num++)
                {
                    if (StaticMethods.PointInRectangle(position, this.TreasuresClientDisplayPosition(this.TreasureGroup[num])))
                    {
                        foreach (Treasure treasure in this.ShowingPerson.AllEffectiveTreasures)
                        {
                            if (treasure.TreasureGroup == this.TreasureGroup[num])
                            {
                                if (this.current != treasure)
                                {
                                    this.BiographyText.Clear();
                                    this.InfluenceText.Clear();
                                    this.InfluenceText.AddText(treasure.Name, Color.WhiteSmoke);
                                    this.InfluenceText.AddNewLine();
                                    this.InfluenceText.AddText("价值：" + treasure.Worth, Color.Orange);
                                    this.InfluenceText.AddNewLine();
                                    //this.InfluenceText.AddText("类别：" + treasure.TreasureGroupName, Color.Orange);
                                    this.InfluenceText.AddNewLine();
                                    this.InfluenceText.AddText(treasure.Description, Color.Turquoise);
                                    this.InfluenceText.AddNewLine();
                                    foreach (Influence influence in treasure.Influences.Influences.Values)
                                    {
                                        if ((((influence.Kind.ID == 280) || (influence.Kind.ID == 0x119)) || ((influence.Kind.ID == 0x11d) || (influence.Kind.ID == 290))) || (influence.Kind.ID == 300))
                                        {
                                            this.InfluenceText.AddText(influence.Description, Color.Moccasin);
                                        }
                                        else
                                        {
                                            this.InfluenceText.AddText(influence.Description, Color.DeepSkyBlue);
                                        }
                                        this.InfluenceText.AddNewLine();
                                    }
                                    this.InfluenceText.ResortTexts();
                                    this.ConditionText.Clear();
                                    this.ConditionText.ResortTexts();
                                    this.current = treasure;
                                }
                                flag = true;
                            }
                        }
                    }
                }
            }
            if (!flag)
            {
                for (num = 0; num < this.TitleKind.Count; num++)
                {
                    if (StaticMethods.PointInRectangle(position, this.TitleClientDisplayPosition(this.TitleKind[num])))
                    {
                        foreach (Title title in this.ShowingPerson.RealTitles)
                        {
                            if (title.Kind.ID == this.TitleKind[num])
                            {
                                if (this.current != title)
                                {
                                    this.BiographyText.Clear();
                                    this.InfluenceText.Clear();
                                    this.InfluenceText.AddText(title.Name, Color.Orange);
                                    this.InfluenceText.AddNewLine();
                                    this.InfluenceText.AddText("类别：" + title.KindName, Color.Orchid);
                                    this.InfluenceText.AddNewLine();
                                    //this.InfluenceText.AddText("品级：" + title.LevelInChinese, this.DetailColor(title.Level));
                                    this.InfluenceText.AddNewLine();
                                    this.InfluenceText.AddText("", Color.WhiteSmoke);
                                    this.InfluenceText.AddNewLine();
                                    foreach (Influence influence in title.Influences.Influences.Values)
                                    {
                                        if ((((influence.Kind.ID == 280) || (influence.Kind.ID == 0x119)) || ((influence.Kind.ID == 0x11d) || (influence.Kind.ID == 290))) || (influence.Kind.ID == 300))
                                        {
                                            this.InfluenceText.AddText(influence.Description, Color.Moccasin);
                                        }
                                        else
                                        {
                                            this.InfluenceText.AddText(influence.Description, Color.DeepSkyBlue);
                                        }
                                        this.InfluenceText.AddNewLine();
                                    }
                                    this.InfluenceText.ResortTexts();
                                    this.ConditionText.Clear();
                                    this.ConditionText.AddText("修习条件", Color.Orchid);
                                    this.ConditionText.AddNewLine();
                                    foreach (Condition condition in title.Conditions.Conditions.Values)
                                    {
                                        if (condition.CheckCondition(this.ShowingPerson))
                                        {
                                            this.ConditionText.AddText(condition.Name, this.ConditionText.PositiveColor);
                                        }
                                        else
                                        {
                                            this.ConditionText.AddText(condition.Name, this.ConditionText.NegativeColor);
                                        }
                                        this.ConditionText.AddNewLine();
                                    }
                                    foreach (Condition condition in title.ArchitectureConditions.Conditions.Values)
                                    {
                                        if ((this.ShowingPerson.LocationArchitecture != null) && condition.CheckCondition(this.ShowingPerson.LocationArchitecture))
                                        {
                                            this.ConditionText.AddText(condition.Name, this.ConditionText.PositiveColor);
                                        }
                                        else
                                        {
                                            this.ConditionText.AddText(condition.Name, this.ConditionText.NegativeColor);
                                        }
                                        this.ConditionText.AddNewLine();
                                    }
                                    foreach (Condition condition in title.FactionConditions.Conditions.Values)
                                    {
                                        if ((this.ShowingPerson.BelongedFaction != null) && condition.CheckCondition(this.ShowingPerson.BelongedFaction))
                                        {
                                            this.ConditionText.AddText(condition.Name, this.ConditionText.PositiveColor);
                                        }
                                        else
                                        {
                                            this.ConditionText.AddText(condition.Name, this.ConditionText.NegativeColor);
                                        }
                                        this.ConditionText.AddNewLine();
                                    }
                                    this.ConditionText.ResortTexts();
                                    this.current = title;
                                }
                                flag = true;
                            }
                        }
                    }
                }
            }
            if (!flag)
            {
                for (num = 0; num < this.SkillID.Count; num++)
                {
                    if (StaticMethods.PointInRectangle(position, this.SkillClientDisplayPosition(this.SkillID[num])) && this.ShowingPerson.HasSkill(this.SkillID[num]))
                    {
                        Skill skill = this.ShowingPerson.Skills.GetSkill(this.SkillID[num]);
                        if ((skill != null) && (this.current != skill))
                        {
                            this.BiographyText.Clear();
                            this.InfluenceText.AddText("技能", Color.Orchid);
                            this.InfluenceText.AddText(skill.Name, Color.Orange);
                            this.InfluenceText.AddNewLine();
                            foreach (Influence influence in skill.Influences.Influences.Values)
                            {
                                if ((((influence.Kind.ID == 280) || (influence.Kind.ID == 0x119)) || ((influence.Kind.ID == 0x11d) || (influence.Kind.ID == 290))) || (influence.Kind.ID == 300))
                                {
                                    this.InfluenceText.AddText(influence.Description, Color.Moccasin);
                                }
                                else
                                {
                                    this.InfluenceText.AddText(influence.Description, Color.DeepSkyBlue);
                                }
                                this.InfluenceText.AddNewLine();
                            }
                            this.InfluenceText.ResortTexts();
                            this.ConditionText.Clear();
                            this.ConditionText.AddText("修习条件", this.ConditionText.TitleColor);
                            this.ConditionText.AddNewLine();
                            foreach (Condition condition in skill.Conditions.Conditions.Values)
                            {
                                if (condition.CheckCondition(this.ShowingPerson))
                                {
                                    this.ConditionText.AddText(condition.Name, this.ConditionText.PositiveColor);
                                }
                                else
                                {
                                    this.ConditionText.AddText(condition.Name, this.ConditionText.NegativeColor);
                                }
                                this.ConditionText.AddNewLine();
                            }
                            this.ConditionText.ResortTexts();
                            this.current = skill;
                        }
                        flag = true;
                    }
                }
            }
            if (!flag)
            {
                for (num = 0; num < this.StuntID.Count; num++)
                {
                    if (StaticMethods.PointInRectangle(position, this.StuntClientDisplayPosition(this.StuntID[num])) && this.ShowingPerson.HasStunt(this.StuntID[num]))
                    {
                        Stunt stunt = this.ShowingPerson.Stunts.GetStunt(this.StuntID[num]);
                        if ((stunt != null) && (this.current != stunt))
                        {
                            this.BiographyText.Clear();
                            this.InfluenceText.Clear();
                            this.InfluenceText.AddText(stunt.Name, Color.Orange);
                            this.InfluenceText.AddNewLine();
                            //this.InfluenceText.AddText("品级：" + stunt.LevelinChinese, Color.Aquamarine);
                            this.InfluenceText.AddNewLine();
                            this.InfluenceText.AddText("战意消耗：" + stunt.Combativity, Color.Orchid);
                            this.InfluenceText.AddNewLine();
                            this.InfluenceText.AddText("持续天数", this.InfluenceText.SubTitleColor2);
                            this.InfluenceText.AddText(stunt.Period.ToString(), this.InfluenceText.SubTitleColor3);
                            this.InfluenceText.AddText("天", this.InfluenceText.SubTitleColor2);
                            this.InfluenceText.AddNewLine();
                            this.InfluenceText.AddText("", Color.WhiteSmoke);
                            this.InfluenceText.AddNewLine();
                            foreach (Influence influence in stunt.Influences.Influences.Values)
                            {
                                if ((((influence.Kind.ID == 280) || (influence.Kind.ID == 0x119)) || ((influence.Kind.ID == 0x11d) || (influence.Kind.ID == 290))) || (influence.Kind.ID == 300))
                                {
                                    this.InfluenceText.AddText(influence.Description, Color.Moccasin);
                                }
                                else
                                {
                                    this.InfluenceText.AddText(influence.Description, Color.DeepSkyBlue);
                                }
                                this.InfluenceText.AddNewLine();
                            }
                            this.InfluenceText.ResortTexts();
                            this.ConditionText.Clear();
                            this.ConditionText.AddText("使用条件", Color.Orange);
                            this.ConditionText.AddNewLine();
                            if ((this.ShowingPerson.LocationTroop != null) && (this.ShowingPerson == this.ShowingPerson.LocationTroop.Leader))
                            {
                                foreach (Condition condition in stunt.CastConditions.Conditions.Values)
                                {
                                    if (condition.CheckCondition(this.ShowingPerson.LocationTroop))
                                    {
                                        this.ConditionText.AddText(condition.Name, this.ConditionText.PositiveColor);
                                    }
                                    else
                                    {
                                        this.ConditionText.AddText(condition.Name, this.ConditionText.NegativeColor);
                                    }
                                    this.ConditionText.AddNewLine();
                                }
                            }
                            else
                            {
                                foreach (Condition condition in stunt.CastConditions.Conditions.Values)
                                {
                                    this.ConditionText.AddText(condition.Name);
                                    this.ConditionText.AddNewLine();
                                }
                            }
                            this.ConditionText.AddNewLine();
                            this.ConditionText.AddText("修习条件", Color.Orange);
                            this.ConditionText.AddNewLine();
                            foreach (Condition condition in stunt.LearnConditions.Conditions.Values)
                            {
                                if (condition.CheckCondition(this.ShowingPerson))
                                {
                                    this.ConditionText.AddText(condition.Name, this.ConditionText.PositiveColor);
                                }
                                else
                                {
                                    this.ConditionText.AddText(condition.Name, this.ConditionText.NegativeColor);
                                }
                                this.ConditionText.AddNewLine();
                            }
                            this.ConditionText.ResortTexts();
                            this.current = stunt;
                        }
                        flag = true;
                    }
                }
            }
            if (!flag && (this.current != null))
            {
                this.current = null;
                this.InfluenceText.Clear();
                this.ConditionText.Clear();
                if (this.ShowingPerson.PersonBiography != null)
                {
                    this.BiographyText.Clear();
                    this.BiographyText.AddText("列传", this.BiographyText.TitleColor);
                    this.BiographyText.AddNewLine();
                    this.BiographyText.AddText(this.ShowingPerson.PersonBiography.Brief);
                    this.BiographyText.AddNewLine();
                    this.BiographyText.AddText("演义", this.BiographyText.SubTitleColor);
                    this.BiographyText.AddNewLine();
                    this.BiographyText.AddText(this.ShowingPerson.PersonBiography.Romance);
                    this.BiographyText.AddNewLine();
                    this.BiographyText.AddText("历史", this.BiographyText.SubTitleColor2);
                    this.BiographyText.AddNewLine();
                    this.BiographyText.AddText(this.ShowingPerson.PersonBiography.History);
                    this.BiographyText.AddNewLine();
                    this.BiographyText.AddText("剧本", this.BiographyText.SubTitleColor2);
                    this.BiographyText.AddText("：");
                    string[] strArray = this.ShowingPerson.PersonBiography.InGame.Split(new char[] { '\n' });
                    foreach (string str in strArray)
                    {
                        this.BiographyText.AddText(str);
                        this.BiographyText.AddNewLine();
                    }
                    this.BiographyText.ResortTexts();
                }
            }
        }

        private void screen_OnMouseRightUp(Point position)
        {
            this.IsShowing = false;
        }

        internal void SetPerson(Person person)
        {
            this.ShowingPerson = person;
            this.SurNameText.Text = person.SurName;
            this.GivenNameText.Text = person.GivenName;
            this.CalledNameText.Text = person.CalledName;
            foreach (LabelText text in this.LabelTexts)
            {
                text.Text.Text = StaticMethods.GetPropertyValue(person, text.PropertyName).ToString();
            }
            this.BiographyText.Clear();
            foreach (LabelText text in this.LabelTexts)
            {
                text.Text.Text = StaticMethods.GetPropertyValue(person, text.PropertyName).ToString();
            }
            this.TitleText.Clear();
            foreach (Title title in person.Titles)
            {
                if (title != null)
                {
                    //阿柒:根据称号等级设定不同颜色
                    if (title.Level < 4)
                    {
                        this.TitleText.AddText("  " + title.DetailedName, Color.AliceBlue);
                    }
                    else if (title.Level >= 4 && title.Level < 7)
                    {
                        this.TitleText.AddText("  " + title.DetailedName, Color.YellowGreen);
                    }
                    else if (title.Level >= 7 && title.Level < 10)
                    {
                        this.TitleText.AddText("  " + title.DetailedName, Color.LightSkyBlue);
                    }
                    else if (title.Level >= 10 && title.Level < 13)
                    {
                        this.TitleText.AddText(title.DetailedName, Color.Violet);
                    }
                    else
                    {
                        this.TitleText.AddText(title.DetailedName, Color.Orange);
                    }

                }
                //this.TitleText.AddText(title.DetailedName, Color.DarkSlateBlue);
                this.TitleText.AddNewLine();
            }
            this.TitleText.ResortTexts();

            this.PersonSkillTexts.SimpleClear();
            this.LearnableSkillTexts.SimpleClear();
            foreach (Skill skill in Session.Current.Scenario.GameCommonData.AllSkills.Skills.Values)
            {
                Rectangle position = new Rectangle(this.SkillDisplayOffset.X + (skill.DisplayCol * this.SkillBlockSize.X), this.SkillDisplayOffset.Y + (skill.DisplayRow * this.SkillBlockSize.Y), this.SkillBlockSize.X, this.SkillBlockSize.Y);
                if (person.Skills.GetSkill(skill.ID) != null)
                {
                    this.PersonSkillTexts.AddText(skill.Name, position);
                }
                else if (skill.CanLearn(person))
                {
                    this.LearnableSkillTexts.AddText(skill.Name, position);
                }
            }
            this.PersonSkillTexts.ResetAllAlignedPositions();
            this.LearnableSkillTexts.ResetAllAlignedPositions();
            this.StuntText.Clear();
            //阿柒:特技显示效果修改,去掉多余的字
            //this.StuntText.AddText("战斗特技", Color.Yellow);
            //this.StuntText.AddNewLine();
            //this.StuntText.AddText(person.Stunts.Count.ToString() + "种", Color.Lime);
            //this.StuntText.AddNewLine();
            foreach (Stunt stunt in person.Stunts.Stunts.Values)
            {
                this.StuntText.AddText(stunt.Name, Color.Khaki);
                this.StuntText.AddText(" 战意消耗" + stunt.Combativity.ToString(), Color.SkyBlue);
                this.StuntText.AddNewLine();
            }
            this.StuntText.ResortTexts();

            if (person.PersonBiography != null)
            {
                this.BiographyText.Clear();
                this.BiographyText.AddText("列传", this.BiographyText.TitleColor);
                this.BiographyText.AddNewLine();
                this.BiographyText.AddText(this.ShowingPerson.PersonBiography.Brief);
                this.BiographyText.AddNewLine();
                this.BiographyText.AddText("演义", this.BiographyText.SubTitleColor);
                this.BiographyText.AddNewLine();
                this.BiographyText.AddText(this.ShowingPerson.PersonBiography.Romance);
                this.BiographyText.AddNewLine();
                this.BiographyText.AddText("历史", this.BiographyText.SubTitleColor2);
                this.BiographyText.AddNewLine();
                this.BiographyText.AddText(this.ShowingPerson.PersonBiography.History);
                this.BiographyText.AddNewLine();
                this.BiographyText.AddText("剧本", this.BiographyText.SubTitleColor2);
                this.BiographyText.AddText("：");
                string[] strArray = this.ShowingPerson.PersonBiography.InGame.Split(new char[] { '\n' });
                foreach (string str in strArray)
                {
                    this.BiographyText.AddText(str);
                    this.BiographyText.AddNewLine();
                }
                this.BiographyText.ResortTexts();
                this.MoreMessageText.Clear();
                this.MoreMessageText.AddText("兵科经验", this.MoreMessageText.TitleColor);
                this.MoreMessageText.AddNewLine();
                this.MoreMessageText.AddText("步兵：", Color.WhiteSmoke);
                this.MoreMessageText.AddText(this.ShowingPerson.BubingExperience.ToString(), Color.WhiteSmoke);
                this.MoreMessageText.AddNewLine();
                this.MoreMessageText.AddText("弓弩：", Color.WhiteSmoke);
                this.MoreMessageText.AddText(this.ShowingPerson.NubingExperience.ToString(), Color.WhiteSmoke);
                this.MoreMessageText.AddNewLine();
                this.MoreMessageText.AddText("骑兵：", Color.WhiteSmoke);
                this.MoreMessageText.AddText(this.ShowingPerson.QibingExperience.ToString(), Color.WhiteSmoke);
                this.MoreMessageText.AddNewLine();
                this.MoreMessageText.AddText("水军：", Color.WhiteSmoke);
                this.MoreMessageText.AddText(this.ShowingPerson.ShuijunExperience.ToString(), Color.WhiteSmoke);
                this.MoreMessageText.AddNewLine();
                this.MoreMessageText.AddText("器械：", Color.WhiteSmoke);
                this.MoreMessageText.AddText(this.ShowingPerson.QixieExperience.ToString(), Color.WhiteSmoke);
                this.MoreMessageText.AddNewLine();
                this.MoreMessageText.AddText("其他经验", this.MoreMessageText.TitleColor);
                this.MoreMessageText.AddNewLine();
                this.MoreMessageText.AddText("计略：", Color.WhiteSmoke);
                this.MoreMessageText.AddText(this.ShowingPerson.StratagemExperience.ToString(), Color.WhiteSmoke);
                this.MoreMessageText.AddNewLine();
                this.MoreMessageText.AddText("策略：", Color.WhiteSmoke);
                this.MoreMessageText.AddText(this.ShowingPerson.TacticsExperience.ToString(), Color.WhiteSmoke);
                this.MoreMessageText.AddNewLine();
                this.MoreMessageText.AddText("内政：", Color.WhiteSmoke);
                this.MoreMessageText.AddText(this.ShowingPerson.InternalExperience.ToString(), Color.WhiteSmoke);
                this.MoreMessageText.AddNewLine();
                this.MoreMessageText.AddText("", this.MoreMessageText.TitleColor);
                this.MoreMessageText.AddNewLine();
                if (this.Switch_DisplayFamily == "on")
                {
                    this.MoreMessageText.AddText("生卒年：", this.MoreMessageText.TitleColor);
                    this.MoreMessageText.AddText(this.ShowingPerson.YearBorn + "-" + this.ShowingPerson.YearDead, Color.WhiteSmoke);
                    this.MoreMessageText.AddNewLine();
                }
                this.MoreMessageText.AddText("家族 ", this.MoreMessageText.TitleColor);
                //if (this.Switch_DisplayFamily == "on")
                //{
                //    this.MoreMessageText.AddText(this.ShowingPerson.PersonFamily, Color.Orange);
                //}
                this.MoreMessageText.AddNewLine();
                this.MoreMessageText.AddText("父：", Color.WhiteSmoke);
                this.MoreMessageText.AddText(this.ShowingPerson.FatherName, Color.SkyBlue);
                this.MoreMessageText.AddText(" 母：", Color.WhiteSmoke);
                this.MoreMessageText.AddText(this.ShowingPerson.MotherName, Color.SandyBrown);
                this.MoreMessageText.AddNewLine();
                this.MoreMessageText.AddText("配偶：", Color.WhiteSmoke);
                this.MoreMessageText.AddText(this.ShowingPerson.SpouseName, Color.Turquoise);
                this.MoreMessageText.AddNewLine();
                this.MoreMessageText.AddNewLine();
                this.MoreMessageText.ResortTexts();
            }
        }

        internal void SetPosition(ShowPosition showPosition)
        {
            Rectangle rectDes = new Rectangle(0, 0, this.screen.viewportSize.X, this.screen.viewportSize.Y);
            Rectangle rect = new Rectangle(0, 0, this.BackgroundSize.X, this.BackgroundSize.Y);
            switch (showPosition)
            {
                case ShowPosition.Center:
                    rect = StaticMethods.GetCenterRectangle(rectDes, rect);
                    break;

                case ShowPosition.Top:
                    rect = StaticMethods.GetTopRectangle(rectDes, rect);
                    break;

                case ShowPosition.Left:
                    rect = StaticMethods.GetLeftRectangle(rectDes, rect);
                    break;

                case ShowPosition.Right:
                    rect = StaticMethods.GetRightRectangle(rectDes, rect);
                    break;

                case ShowPosition.Bottom:
                    rect = StaticMethods.GetBottomRectangle(rectDes, rect);
                    break;

                case ShowPosition.TopLeft:
                    rect = StaticMethods.GetTopLeftRectangle(rectDes, rect);
                    break;

                case ShowPosition.TopRight:
                    rect = StaticMethods.GetTopRightRectangle(rectDes, rect);
                    break;

                case ShowPosition.BottomLeft:
                    rect = StaticMethods.GetBottomLeftRectangle(rectDes, rect);
                    break;

                case ShowPosition.BottomRight:
                    rect = StaticMethods.GetBottomRightRectangle(rectDes, rect);
                    break;
            }
            this.DisplayOffset = new Point(rect.X, rect.Y);
            this.SurNameText.DisplayOffset = this.DisplayOffset;
            this.GivenNameText.DisplayOffset = this.DisplayOffset;
            this.CalledNameText.DisplayOffset = this.DisplayOffset;
            foreach (LabelText text in this.LabelTexts)
            {
                text.Label.DisplayOffset = this.DisplayOffset;
                text.Text.DisplayOffset = this.DisplayOffset;
            }
            this.InfluenceText.DisplayOffset = new Point(this.DisplayOffset.X + this.InfluenceClient.X, this.DisplayOffset.Y + this.InfluenceClient.Y);
            this.ConditionText.DisplayOffset = new Point(this.DisplayOffset.X + this.ConditionClient.X, this.DisplayOffset.Y + this.ConditionClient.Y);
            this.BiographyText.DisplayOffset = new Point(this.DisplayOffset.X + this.BiographyClient.X, this.DisplayOffset.Y + this.BiographyClient.Y);
            this.MoreMessageText.DisplayOffset = new Point(this.DisplayOffset.X + this.MoreMessageClient.X, this.DisplayOffset.Y + this.MoreMessageClient.Y);
        }

        private Rectangle SkillClientDisplayPosition(int SkillID)
        {
            switch (SkillID)
            {
                case 0:
                    return new Rectangle(this.Skill0ID0Client.X + this.DisplayOffset.X, this.Skill0ID0Client.Y + this.DisplayOffset.Y, this.Skill0ID0Client.Width, this.Skill0ID0Client.Height);

                case 1:
                    return new Rectangle(this.Skill1ID1Client.X + this.DisplayOffset.X, this.Skill1ID1Client.Y + this.DisplayOffset.Y, this.Skill1ID1Client.Width, this.Skill1ID1Client.Height);

                case 2:
                    return new Rectangle(this.Skill2ID2Client.X + this.DisplayOffset.X, this.Skill2ID2Client.Y + this.DisplayOffset.Y, this.Skill2ID2Client.Width, this.Skill2ID2Client.Height);

                case 3:
                    return new Rectangle(this.Skill3ID3Client.X + this.DisplayOffset.X, this.Skill3ID3Client.Y + this.DisplayOffset.Y, this.Skill3ID3Client.Width, this.Skill3ID3Client.Height);

                case 4:
                    return new Rectangle(this.Skill4ID4Client.X + this.DisplayOffset.X, this.Skill4ID4Client.Y + this.DisplayOffset.Y, this.Skill4ID4Client.Width, this.Skill4ID4Client.Height);

                case 5:
                    return new Rectangle(this.Skill5ID5Client.X + this.DisplayOffset.X, this.Skill5ID5Client.Y + this.DisplayOffset.Y, this.Skill5ID5Client.Width, this.Skill5ID5Client.Height);

                case 6:
                    return new Rectangle(this.Skill6ID6Client.X + this.DisplayOffset.X, this.Skill6ID6Client.Y + this.DisplayOffset.Y, this.Skill6ID6Client.Width, this.Skill6ID6Client.Height);

                case 7:
                    return new Rectangle(this.Skill7ID7Client.X + this.DisplayOffset.X, this.Skill7ID7Client.Y + this.DisplayOffset.Y, this.Skill7ID7Client.Width, this.Skill7ID7Client.Height);

                case 8:
                    return new Rectangle(this.Skill8ID8Client.X + this.DisplayOffset.X, this.Skill8ID8Client.Y + this.DisplayOffset.Y, this.Skill8ID8Client.Width, this.Skill8ID8Client.Height);

                case 9:
                    return new Rectangle(this.Skill9ID9Client.X + this.DisplayOffset.X, this.Skill9ID9Client.Y + this.DisplayOffset.Y, this.Skill9ID9Client.Width, this.Skill9ID9Client.Height);

                case 10:
                    return new Rectangle(this.Skill10ID10Client.X + this.DisplayOffset.X, this.Skill10ID10Client.Y + this.DisplayOffset.Y, this.Skill10ID10Client.Width, this.Skill10ID10Client.Height);

                case 11:
                    return new Rectangle(this.Skill11ID11Client.X + this.DisplayOffset.X, this.Skill11ID11Client.Y + this.DisplayOffset.Y, this.Skill11ID11Client.Width, this.Skill11ID11Client.Height);

                case 12:
                    return new Rectangle(this.Skill12ID12Client.X + this.DisplayOffset.X, this.Skill12ID12Client.Y + this.DisplayOffset.Y, this.Skill12ID12Client.Width, this.Skill12ID12Client.Height);

                case 13:
                    return new Rectangle(this.Skill13ID13Client.X + this.DisplayOffset.X, this.Skill13ID13Client.Y + this.DisplayOffset.Y, this.Skill13ID13Client.Width, this.Skill13ID13Client.Height);

                case 14:
                    return new Rectangle(this.Skill14ID14Client.X + this.DisplayOffset.X, this.Skill14ID14Client.Y + this.DisplayOffset.Y, this.Skill14ID14Client.Width, this.Skill14ID14Client.Height);

                case 15:
                    return new Rectangle(this.Skill15ID15Client.X + this.DisplayOffset.X, this.Skill15ID15Client.Y + this.DisplayOffset.Y, this.Skill15ID15Client.Width, this.Skill15ID15Client.Height);

                case 0x10:
                    return new Rectangle(this.Skill16ID16Client.X + this.DisplayOffset.X, this.Skill16ID16Client.Y + this.DisplayOffset.Y, this.Skill16ID16Client.Width, this.Skill16ID16Client.Height);

                case 0x11:
                    return new Rectangle(this.Skill17ID17Client.X + this.DisplayOffset.X, this.Skill17ID17Client.Y + this.DisplayOffset.Y, this.Skill17ID17Client.Width, this.Skill17ID17Client.Height);

                case 0x12:
                    return new Rectangle(this.Skill18ID18Client.X + this.DisplayOffset.X, this.Skill18ID18Client.Y + this.DisplayOffset.Y, this.Skill18ID18Client.Width, this.Skill18ID18Client.Height);

                case 0x13:
                    return new Rectangle(this.Skill19ID19Client.X + this.DisplayOffset.X, this.Skill19ID19Client.Y + this.DisplayOffset.Y, this.Skill19ID19Client.Width, this.Skill19ID19Client.Height);

                case 20:
                    return new Rectangle(this.Skill20ID20Client.X + this.DisplayOffset.X, this.Skill20ID20Client.Y + this.DisplayOffset.Y, this.Skill20ID20Client.Width, this.Skill20ID20Client.Height);

                case 0x15:
                    return new Rectangle(this.Skill21ID21Client.X + this.DisplayOffset.X, this.Skill21ID21Client.Y + this.DisplayOffset.Y, this.Skill21ID21Client.Width, this.Skill21ID21Client.Height);

                case 0x16:
                    return new Rectangle(this.Skill22ID22Client.X + this.DisplayOffset.X, this.Skill22ID22Client.Y + this.DisplayOffset.Y, this.Skill22ID22Client.Width, this.Skill22ID22Client.Height);

                case 0x17:
                    return new Rectangle(this.Skill23ID23Client.X + this.DisplayOffset.X, this.Skill23ID23Client.Y + this.DisplayOffset.Y, this.Skill23ID23Client.Width, this.Skill23ID23Client.Height);

                case 0x18:
                    return new Rectangle(this.Skill24ID24Client.X + this.DisplayOffset.X, this.Skill24ID24Client.Y + this.DisplayOffset.Y, this.Skill24ID24Client.Width, this.Skill24ID24Client.Height);

                case 0x19:
                    return new Rectangle(this.Skill25ID25Client.X + this.DisplayOffset.X, this.Skill25ID25Client.Y + this.DisplayOffset.Y, this.Skill25ID25Client.Width, this.Skill25ID25Client.Height);

                case 0x1a:
                    return new Rectangle(this.Skill26ID26Client.X + this.DisplayOffset.X, this.Skill26ID26Client.Y + this.DisplayOffset.Y, this.Skill26ID26Client.Width, this.Skill26ID26Client.Height);

                case 0x1b:
                    return new Rectangle(this.Skill27ID27Client.X + this.DisplayOffset.X, this.Skill27ID27Client.Y + this.DisplayOffset.Y, this.Skill27ID27Client.Width, this.Skill27ID27Client.Height);

                case 0x1c:
                    return new Rectangle(this.Skill28ID28Client.X + this.DisplayOffset.X, this.Skill28ID28Client.Y + this.DisplayOffset.Y, this.Skill28ID28Client.Width, this.Skill28ID28Client.Height);

                case 0x1d:
                    return new Rectangle(this.Skill29ID29Client.X + this.DisplayOffset.X, this.Skill29ID29Client.Y + this.DisplayOffset.Y, this.Skill29ID29Client.Width, this.Skill29ID29Client.Height);
            }
            return new Rectangle(this.Skill29ID29Client.X + this.DisplayOffset.X, this.Skill29ID29Client.Y + this.DisplayOffset.Y, this.Skill29ID29Client.Width, this.Skill29ID29Client.Height);
        }

        private Rectangle StuntClientDisplayPosition(int StuntID)
        {
            switch (StuntID)
            {
                case 0:
                    return new Rectangle(this.Stunt0ID0Client.X + this.DisplayOffset.X, this.Stunt0ID0Client.Y + this.DisplayOffset.Y, this.Stunt0ID0Client.Width, this.Stunt0ID0Client.Height);

                case 1:
                    return new Rectangle(this.Stunt1ID1Client.X + this.DisplayOffset.X, this.Stunt1ID1Client.Y + this.DisplayOffset.Y, this.Stunt1ID1Client.Width, this.Stunt1ID1Client.Height);

                case 2:
                    return new Rectangle(this.Stunt2ID2Client.X + this.DisplayOffset.X, this.Stunt2ID2Client.Y + this.DisplayOffset.Y, this.Stunt2ID2Client.Width, this.Stunt2ID2Client.Height);

                case 3:
                    return new Rectangle(this.Stunt3ID3Client.X + this.DisplayOffset.X, this.Stunt3ID3Client.Y + this.DisplayOffset.Y, this.Stunt3ID3Client.Width, this.Stunt3ID3Client.Height);

                case 4:
                    return new Rectangle(this.Stunt4ID4Client.X + this.DisplayOffset.X, this.Stunt4ID4Client.Y + this.DisplayOffset.Y, this.Stunt4ID4Client.Width, this.Stunt4ID4Client.Height);

                case 5:
                    return new Rectangle(this.Stunt5ID5Client.X + this.DisplayOffset.X, this.Stunt5ID5Client.Y + this.DisplayOffset.Y, this.Stunt5ID5Client.Width, this.Stunt5ID5Client.Height);

                case 6:
                    return new Rectangle(this.Stunt6ID6Client.X + this.DisplayOffset.X, this.Stunt6ID6Client.Y + this.DisplayOffset.Y, this.Stunt6ID6Client.Width, this.Stunt6ID6Client.Height);

                case 7:
                    return new Rectangle(this.Stunt7ID7Client.X + this.DisplayOffset.X, this.Stunt7ID7Client.Y + this.DisplayOffset.Y, this.Stunt7ID7Client.Width, this.Stunt7ID7Client.Height);

                case 8:
                    return new Rectangle(this.Stunt8ID8Client.X + this.DisplayOffset.X, this.Stunt8ID8Client.Y + this.DisplayOffset.Y, this.Stunt8ID8Client.Width, this.Stunt8ID8Client.Height);

                case 9:
                    return new Rectangle(this.Stunt9ID9Client.X + this.DisplayOffset.X, this.Stunt9ID9Client.Y + this.DisplayOffset.Y, this.Stunt9ID9Client.Width, this.Stunt9ID9Client.Height);

                case 10:
                    return new Rectangle(this.Stunt10ID10Client.X + this.DisplayOffset.X, this.Stunt10ID10Client.Y + this.DisplayOffset.Y, this.Stunt10ID10Client.Width, this.Stunt10ID10Client.Height);

                case 11:
                    return new Rectangle(this.Stunt11ID11Client.X + this.DisplayOffset.X, this.Stunt11ID11Client.Y + this.DisplayOffset.Y, this.Stunt11ID11Client.Width, this.Stunt11ID11Client.Height);

                case 12:
                    return new Rectangle(this.Stunt12ID12Client.X + this.DisplayOffset.X, this.Stunt12ID12Client.Y + this.DisplayOffset.Y, this.Stunt12ID12Client.Width, this.Stunt12ID12Client.Height);

                case 13:
                    return new Rectangle(this.Stunt13ID13Client.X + this.DisplayOffset.X, this.Stunt13ID13Client.Y + this.DisplayOffset.Y, this.Stunt13ID13Client.Width, this.Stunt13ID13Client.Height);

                case 14:
                    return new Rectangle(this.Stunt14ID14Client.X + this.DisplayOffset.X, this.Stunt14ID14Client.Y + this.DisplayOffset.Y, this.Stunt14ID14Client.Width, this.Stunt14ID14Client.Height);

                case 15:
                    return new Rectangle(this.Stunt15ID15Client.X + this.DisplayOffset.X, this.Stunt15ID15Client.Y + this.DisplayOffset.Y, this.Stunt15ID15Client.Width, this.Stunt15ID15Client.Height);

                case 0x10:
                    return new Rectangle(this.Stunt16ID16Client.X + this.DisplayOffset.X, this.Stunt16ID16Client.Y + this.DisplayOffset.Y, this.Stunt16ID16Client.Width, this.Stunt16ID16Client.Height);

                case 0x11:
                    return new Rectangle(this.Stunt17ID17Client.X + this.DisplayOffset.X, this.Stunt17ID17Client.Y + this.DisplayOffset.Y, this.Stunt17ID17Client.Width, this.Stunt17ID17Client.Height);

                case 0x12:
                    return new Rectangle(this.Stunt18ID18Client.X + this.DisplayOffset.X, this.Stunt18ID18Client.Y + this.DisplayOffset.Y, this.Stunt18ID18Client.Width, this.Stunt18ID18Client.Height);

                case 0x13:
                    return new Rectangle(this.Stunt19ID19Client.X + this.DisplayOffset.X, this.Stunt19ID19Client.Y + this.DisplayOffset.Y, this.Stunt19ID19Client.Width, this.Stunt19ID19Client.Height);

                case 20:
                    return new Rectangle(this.Stunt20ID20Client.X + this.DisplayOffset.X, this.Stunt20ID20Client.Y + this.DisplayOffset.Y, this.Stunt20ID20Client.Width, this.Stunt20ID20Client.Height);

                case 0x15:
                    return new Rectangle(this.Stunt21ID21Client.X + this.DisplayOffset.X, this.Stunt21ID21Client.Y + this.DisplayOffset.Y, this.Stunt21ID21Client.Width, this.Stunt21ID21Client.Height);

                case 0x16:
                    return new Rectangle(this.Stunt22ID22Client.X + this.DisplayOffset.X, this.Stunt22ID22Client.Y + this.DisplayOffset.Y, this.Stunt22ID22Client.Width, this.Stunt22ID22Client.Height);

                case 0x17:
                    return new Rectangle(this.Stunt23ID23Client.X + this.DisplayOffset.X, this.Stunt23ID23Client.Y + this.DisplayOffset.Y, this.Stunt23ID23Client.Width, this.Stunt23ID23Client.Height);

                case 0x18:
                    return new Rectangle(this.Stunt24ID24Client.X + this.DisplayOffset.X, this.Stunt24ID24Client.Y + this.DisplayOffset.Y, this.Stunt24ID24Client.Width, this.Stunt24ID24Client.Height);

                case 0x19:
                    return new Rectangle(this.Stunt25ID25Client.X + this.DisplayOffset.X, this.Stunt25ID25Client.Y + this.DisplayOffset.Y, this.Stunt25ID25Client.Width, this.Stunt25ID25Client.Height);

                case 0x1a:
                    return new Rectangle(this.Stunt26ID26Client.X + this.DisplayOffset.X, this.Stunt26ID26Client.Y + this.DisplayOffset.Y, this.Stunt26ID26Client.Width, this.Stunt26ID26Client.Height);

                case 0x1b:
                    return new Rectangle(this.Stunt27ID27Client.X + this.DisplayOffset.X, this.Stunt27ID27Client.Y + this.DisplayOffset.Y, this.Stunt27ID27Client.Width, this.Stunt27ID27Client.Height);

                case 0x1c:
                    return new Rectangle(this.Stunt28ID28Client.X + this.DisplayOffset.X, this.Stunt28ID28Client.Y + this.DisplayOffset.Y, this.Stunt28ID28Client.Width, this.Stunt28ID28Client.Height);

                case 0x1d:
                    return new Rectangle(this.Stunt29ID29Client.X + this.DisplayOffset.X, this.Stunt29ID29Client.Y + this.DisplayOffset.Y, this.Stunt29ID29Client.Width, this.Stunt29ID29Client.Height);
            }
            return new Rectangle(this.Stunt29ID29Client.X + this.DisplayOffset.X, this.Stunt29ID29Client.Y + this.DisplayOffset.Y, this.Stunt29ID29Client.Width, this.Stunt29ID29Client.Height);
        }

        private Rectangle TitleClientDisplayPosition(int TitleKindID)
        {
            switch (TitleKindID)
            {
                case 200:
                    return new Rectangle(this.Title2kind200Client.X + this.DisplayOffset.X, this.Title2kind200Client.Y + this.DisplayOffset.Y, this.Title2kind200Client.Width, this.Title2kind200Client.Height);

                case 600:
                    return new Rectangle(this.Title11kind600Client.X + this.DisplayOffset.X, this.Title11kind600Client.Y + this.DisplayOffset.Y, this.Title11kind600Client.Width, this.Title11kind600Client.Height);

                case 700:
                    return new Rectangle(this.Title12kind700Client.X + this.DisplayOffset.X, this.Title12kind700Client.Y + this.DisplayOffset.Y, this.Title12kind700Client.Width, this.Title12kind700Client.Height);

                case 1:
                    return new Rectangle(this.Title1kind1Client.X + this.DisplayOffset.X, this.Title1kind1Client.Y + this.DisplayOffset.Y, this.Title1kind1Client.Width, this.Title1kind1Client.Height);

                case 2:
                    return new Rectangle(this.Title4kind2Client.X + this.DisplayOffset.X, this.Title4kind2Client.Y + this.DisplayOffset.Y, this.Title4kind2Client.Width, this.Title4kind2Client.Height);

                case 3:
                    return new Rectangle(this.Title7kind3Client.X + this.DisplayOffset.X, this.Title7kind3Client.Y + this.DisplayOffset.Y, this.Title7kind3Client.Width, this.Title7kind3Client.Height);

                case 4:
                    return new Rectangle(this.Title8kind4Client.X + this.DisplayOffset.X, this.Title8kind4Client.Y + this.DisplayOffset.Y, this.Title8kind4Client.Width, this.Title8kind4Client.Height);

                case 5:
                    return new Rectangle(this.Title10kind5Client.X + this.DisplayOffset.X, this.Title10kind5Client.Y + this.DisplayOffset.Y, this.Title10kind5Client.Width, this.Title10kind5Client.Height);

                case 7:
                    return new Rectangle(this.Title6kind7Client.X + this.DisplayOffset.X, this.Title6kind7Client.Y + this.DisplayOffset.Y, this.Title6kind7Client.Width, this.Title6kind7Client.Height);

                case 10:
                    return new Rectangle(this.Title3kind10Client.X + this.DisplayOffset.X, this.Title3kind10Client.Y + this.DisplayOffset.Y, this.Title3kind10Client.Width, this.Title3kind10Client.Height);

                case 0x15:
                    return new Rectangle(this.Title5kind21Client.X + this.DisplayOffset.X, this.Title5kind21Client.Y + this.DisplayOffset.Y, this.Title5kind21Client.Width, this.Title5kind21Client.Height);

                case 50:
                    return new Rectangle(this.Title9kind50Client.X + this.DisplayOffset.X, this.Title9kind50Client.Y + this.DisplayOffset.Y, this.Title9kind50Client.Width, this.Title9kind50Client.Height);
            }
            return new Rectangle(this.Title12kind700Client.X + this.DisplayOffset.X, this.Title12kind700Client.Y + this.DisplayOffset.Y, this.Title12kind700Client.Width, this.Title12kind700Client.Height);
        }

        private Rectangle TreasuresClientDisplayPosition(int TreasureGroup)
        {
            switch (TreasureGroup)
            {
                case 0x19:
                    return new Rectangle(this.Treasure4group25Client.X + this.DisplayOffset.X, this.Treasure4group25Client.Y + this.DisplayOffset.Y, this.Treasure4group25Client.Width, this.Treasure4group25Client.Height);

                case 30:
                    return new Rectangle(this.Treasure5group30Client.X + this.DisplayOffset.X, this.Treasure5group30Client.Y + this.DisplayOffset.Y, this.Treasure5group30Client.Width, this.Treasure5group30Client.Height);

                case 40:
                    return new Rectangle(this.Treasure6group40Client.X + this.DisplayOffset.X, this.Treasure6group40Client.Y + this.DisplayOffset.Y, this.Treasure6group40Client.Width, this.Treasure6group40Client.Height);

                case 10:
                    return new Rectangle(this.Treasure1group10Client.X + this.DisplayOffset.X, this.Treasure1group10Client.Y + this.DisplayOffset.Y, this.Treasure1group10Client.Width, this.Treasure1group10Client.Height);

                case 15:
                    return new Rectangle(this.Treasure2group15Client.X + this.DisplayOffset.X, this.Treasure2group15Client.Y + this.DisplayOffset.Y, this.Treasure2group15Client.Width, this.Treasure2group15Client.Height);

                case 20:
                    return new Rectangle(this.Treasure3group20Client.X + this.DisplayOffset.X, this.Treasure3group20Client.Y + this.DisplayOffset.Y, this.Treasure3group20Client.Width, this.Treasure3group20Client.Height);

                case 50:
                    return new Rectangle(this.Treasure7group50Client.X + this.DisplayOffset.X, this.Treasure7group50Client.Y + this.DisplayOffset.Y, this.Treasure7group50Client.Width, this.Treasure7group50Client.Height);

                case 0x37:
                    return new Rectangle(this.Treasure8group55Client.X + this.DisplayOffset.X, this.Treasure8group55Client.Y + this.DisplayOffset.Y, this.Treasure8group55Client.Width, this.Treasure8group55Client.Height);

                case 60:
                    return new Rectangle(this.Treasure9group60Client.X + this.DisplayOffset.X, this.Treasure9group60Client.Y + this.DisplayOffset.Y, this.Treasure9group60Client.Width, this.Treasure9group60Client.Height);

                case 70:
                    return new Rectangle(this.Treasure10group70Client.X + this.DisplayOffset.X, this.Treasure10group70Client.Y + this.DisplayOffset.Y, this.Treasure10group70Client.Width, this.Treasure10group70Client.Height);

                case 90:
                    return new Rectangle(this.Treasure11group90Client.X + this.DisplayOffset.X, this.Treasure11group90Client.Y + this.DisplayOffset.Y, this.Treasure11group90Client.Width, this.Treasure11group90Client.Height);

                case 100:
                    return new Rectangle(this.Treasure12group100Client.X + this.DisplayOffset.X, this.Treasure12group100Client.Y + this.DisplayOffset.Y, this.Treasure12group100Client.Width, this.Treasure12group100Client.Height);
            }
            return new Rectangle(this.Treasure12group100Client.X + this.DisplayOffset.X, this.Treasure12group100Client.Y + this.DisplayOffset.Y, this.Treasure12group100Client.Width, this.Treasure12group100Client.Height);
        }

        // Properties
        private Rectangle BackgroundDisplayPosition =>
            new Rectangle(this.DisplayOffset.X, this.DisplayOffset.Y, this.BackgroundSize.X, this.BackgroundSize.Y);

        public bool IsShowing
        {
            get =>
                this.isShowing;
            set
            {
                this.isShowing = value;
                if (value)
                {
                    this.screen.PushUndoneWork(new UndoneWorkItem(UndoneWorkKind.SubDialog, DialogKind.PersonDetail));
                    this.screen.OnMouseMove += new Screen.MouseMove(this.screen_OnMouseMove);
                    this.screen.OnMouseLeftDown += new Screen.MouseLeftDown(this.screen_OnMouseLeftDown);
                    this.screen.OnMouseRightUp += new Screen.MouseRightUp(this.screen_OnMouseRightUp);
                }
                else
                {
                    if (this.screen.PopUndoneWork().Kind != UndoneWorkKind.SubDialog)
                    {
                        throw new Exception("The UndoneWork is not a SubDialog.");
                    }
                    this.screen.OnMouseMove -= new Screen.MouseMove(this.screen_OnMouseMove);
                    this.screen.OnMouseLeftDown -= new Screen.MouseLeftDown(this.screen_OnMouseLeftDown);
                    this.screen.OnMouseRightUp -= new Screen.MouseRightUp(this.screen_OnMouseRightUp);
                    this.current = null;
                    this.InfluenceText.Clear();
                    this.ConditionText.Clear();
                }
            }
        }

        private Rectangle MoreMessageBGDisplayPosition =>
            new Rectangle(this.MoreMessageBGClient.X + this.DisplayOffset.X, this.MoreMessageBGClient.Y + this.DisplayOffset.Y, this.MoreMessageBGSize.X, this.MoreMessageBGSize.Y);

        private Rectangle PortraitDisplayPosition =>
            new Rectangle(this.PortraitClient.X + this.DisplayOffset.X, this.PortraitClient.Y + this.DisplayOffset.Y, this.PortraitClient.Width, this.PortraitClient.Height);

        private Rectangle SkillBGClientDisplayPosition =>
            new Rectangle(this.SkillBGClient.X + this.DisplayOffset.X, this.SkillBGClient.Y + this.DisplayOffset.Y, this.SkillBGClient.Width, this.SkillBGClient.Height);

        public List<int> SkillID
        {
            get
            {
                List<int> list = new List<int>();
                for (int i = 0; i < 30; i++)
                {
                    list.Add(i);
                }
                return list;
            }
        }

        private Rectangle StuntBGClientDisplayPosition =>
            new Rectangle(this.StuntBGClient.X + this.DisplayOffset.X, this.StuntBGClient.Y + this.DisplayOffset.Y, this.StuntBGClient.Width, this.StuntBGClient.Height);

        public List<int> StuntID
        {
            get
            {
                List<int> list = new List<int>();
                for (int i = 0; i < 30; i++)
                {
                    list.Add(i);
                }
                return list;
            }
        }

        private Rectangle TitleBGClientDisplayPosition =>
            new Rectangle(this.TitleBGClient.X + this.DisplayOffset.X, this.TitleBGClient.Y + this.DisplayOffset.Y, this.TitleBGClient.Width, this.TitleBGClient.Height);

        private Rectangle TreasureBGClientDisplayPosition =>
            new Rectangle(this.TreasureBGClient.X + this.DisplayOffset.X, this.TreasureBGClient.Y + this.DisplayOffset.Y, this.TreasureBGClient.Width, this.TreasureBGClient.Height);
    }
}






