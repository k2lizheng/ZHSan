using GameFreeText;
using GameGlobal;
using GameManager;
using GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platforms;
using PluginInterface;
using PluginInterface.BaseInterface;
using System;
using System.Xml;
using WorldOfTheThreeKingdoms;
using GameObjects.PersonDetail;
namespace PersonDetailPlugin
{

    public class PersonDetailPlugin : GameObject, IPersonDetail, IBasePlugin, IPluginXML, IPluginGraphics
    {
        // Fields
        private string author = "clip_on";
        private const string DataPath = @"Content\Textures\GameComponents\PersonDetail\Data\";
        private string description = "人物细节显示";

        private const string Path = @"Content\Textures\GameComponents\PersonDetail\";
        private PersonDetail personDetail = new PersonDetail();
        private string pluginName = "PersonDetailPlugin";
        private string version = "1.0.0";
        private const string XMLFilename = "PersonDetailData.xml";


        // Methods
        public void Dispose()
        {
        }

        public void Draw()
        {
            if (this.personDetail.IsShowing)
            {
                this.personDetail.Draw();
            }
        }

        public void Initialize()
        {
        }

        public void LoadDataFromXMLDocument(string filename)
        {
            Font font;
            Color color;
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            XmlNode nextSibling = document.FirstChild.NextSibling;
            XmlNode node3 = nextSibling.ChildNodes.Item(0);
            this.personDetail.BackgroundSize.X = int.Parse(node3.Attributes.GetNamedItem("Width").Value);
            this.personDetail.BackgroundSize.Y = int.Parse(node3.Attributes.GetNamedItem("Height").Value);
            this.personDetail.BackgroundTexture = CacheManager.GetTempTexture(@"Content\Textures\GameComponents\PersonDetail\Data\" + node3.Attributes.GetNamedItem("FileName").Value);
            node3 = nextSibling.ChildNodes.Item(1);
            Rectangle rectangle = StaticMethods.LoadRectangleFromXMLNode(node3);
            StaticMethods.LoadFontAndColorFromXMLNode(node3, out font, out color);
            this.personDetail.SurNameText = new FreeText(font, color);
            this.personDetail.SurNameText.Position = rectangle;
            this.personDetail.SurNameText.Align = (TextAlign)Enum.Parse(typeof(TextAlign), node3.Attributes.GetNamedItem("Align").Value);
            node3 = nextSibling.ChildNodes.Item(2);
            rectangle = StaticMethods.LoadRectangleFromXMLNode(node3);
            StaticMethods.LoadFontAndColorFromXMLNode(node3, out font, out color);
            this.personDetail.SurNameText = new FreeText(font, color);
            this.personDetail.GivenNameText.Position = rectangle;
            this.personDetail.GivenNameText.Align = (TextAlign)Enum.Parse(typeof(TextAlign), node3.Attributes.GetNamedItem("Align").Value);
            node3 = nextSibling.ChildNodes.Item(3);
            rectangle = StaticMethods.LoadRectangleFromXMLNode(node3);
            StaticMethods.LoadFontAndColorFromXMLNode(node3, out font, out color);
            this.personDetail.SurNameText = new FreeText(font, color);
            this.personDetail.CalledNameText.Position = rectangle;
            this.personDetail.CalledNameText.Align = (TextAlign)Enum.Parse(typeof(TextAlign), node3.Attributes.GetNamedItem("Align").Value);
            node3 = nextSibling.ChildNodes.Item(4);
            this.personDetail.PortraitClient = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(5);
            for (int i = 0; i < node3.ChildNodes.Count; i += 2)
            {
                LabelText item = new LabelText();
                XmlNode node = node3.ChildNodes.Item(i);
                rectangle = StaticMethods.LoadRectangleFromXMLNode(node);
                StaticMethods.LoadFontAndColorFromXMLNode(node, out font, out color);
                item.Label = new FreeText(font, color);
                item.Label.Position = rectangle;
                item.Label.Align = (TextAlign)Enum.Parse(typeof(TextAlign), node.Attributes.GetNamedItem("Align").Value);
                item.Label.Text = node.Attributes.GetNamedItem("Label").Value;
                node = node3.ChildNodes.Item(i + 1);
                rectangle = StaticMethods.LoadRectangleFromXMLNode(node);
                StaticMethods.LoadFontAndColorFromXMLNode(node, out font, out color);
                item.Label = new FreeText(font, color);
                item.Text.Position = rectangle;
                item.Text.Align = (TextAlign)Enum.Parse(typeof(TextAlign), node.Attributes.GetNamedItem("Align").Value);
                item.PropertyName = node.Attributes.GetNamedItem("PropertyName").Value;
                this.personDetail.LabelTexts.Add(item);
            }
            node3 = nextSibling.ChildNodes.Item(9);
            this.personDetail.InfluenceClient = StaticMethods.LoadRectangleFromXMLNode(node3);
            this.personDetail.InfluenceText.ClientWidth = this.personDetail.InfluenceClient.Width;
            this.personDetail.InfluenceText.ClientHeight = this.personDetail.InfluenceClient.Height;
            this.personDetail.InfluenceText.RowMargin = int.Parse(node3.Attributes.GetNamedItem("RowMargin").Value);
            this.personDetail.InfluenceText.TitleColor = StaticMethods.LoadColor(node3.Attributes.GetNamedItem("TitleColor").Value);
            this.personDetail.InfluenceText.SubTitleColor = StaticMethods.LoadColor(node3.Attributes.GetNamedItem("SubTitleColor").Value);
            this.personDetail.InfluenceText.SubTitleColor2 = StaticMethods.LoadColor(node3.Attributes.GetNamedItem("SubTitleColor2").Value);
            this.personDetail.InfluenceText.SubTitleColor3 = StaticMethods.LoadColor(node3.Attributes.GetNamedItem("SubTitleColor3").Value);
            StaticMethods.LoadFontAndColorFromXMLNode(node3, out font, out color);
            this.personDetail.InfluenceText.Builder.SetFreeTextBuilder(font);
            this.personDetail.InfluenceText.DefaultColor = color;
            node3 = nextSibling.ChildNodes.Item(10);
            this.personDetail.ConditionClient = StaticMethods.LoadRectangleFromXMLNode(node3);
            this.personDetail.ConditionText.ClientWidth = this.personDetail.ConditionClient.Width;
            this.personDetail.ConditionText.ClientHeight = this.personDetail.ConditionClient.Height;
            this.personDetail.ConditionText.RowMargin = int.Parse(node3.Attributes.GetNamedItem("RowMargin").Value);
            this.personDetail.ConditionText.TitleColor = StaticMethods.LoadColor(node3.Attributes.GetNamedItem("TitleColor").Value);
            this.personDetail.ConditionText.SubTitleColor = StaticMethods.LoadColor(node3.Attributes.GetNamedItem("SubTitleColor").Value);
            this.personDetail.ConditionText.PositiveColor = StaticMethods.LoadColor(node3.Attributes.GetNamedItem("PositiveColor").Value);
            this.personDetail.ConditionText.NegativeColor = StaticMethods.LoadColor(node3.Attributes.GetNamedItem("NegativeColor").Value);
            StaticMethods.LoadFontAndColorFromXMLNode(node3, out font, out color);
            this.personDetail.ConditionText.Builder.SetFreeTextBuilder(font);
            this.personDetail.ConditionText.DefaultColor = color;
            node3 = nextSibling.ChildNodes.Item(11);
            this.personDetail.BiographyClient = StaticMethods.LoadRectangleFromXMLNode(node3);
            this.personDetail.BiographyText.ClientWidth = this.personDetail.BiographyClient.Width;
            this.personDetail.BiographyText.ClientHeight = this.personDetail.BiographyClient.Height;
            this.personDetail.BiographyText.RowMargin = int.Parse(node3.Attributes.GetNamedItem("RowMargin").Value);
            this.personDetail.BiographyText.TitleColor = StaticMethods.LoadColor(node3.Attributes.GetNamedItem("TitleColor").Value);
            this.personDetail.BiographyText.SubTitleColor = StaticMethods.LoadColor(node3.Attributes.GetNamedItem("SubTitleColor").Value);
            this.personDetail.BiographyText.SubTitleColor2 = StaticMethods.LoadColor(node3.Attributes.GetNamedItem("SubTitleColor2").Value);
            StaticMethods.LoadFontAndColorFromXMLNode(node3, out font, out color);
            this.personDetail.BiographyText.Builder.SetFreeTextBuilder(font);
            this.personDetail.BiographyText.DefaultColor = color;
            node3 = nextSibling.ChildNodes.Item(14);
            this.personDetail.MoreMessageClient = StaticMethods.LoadRectangleFromXMLNode(node3);
            this.personDetail.MoreMessageText.ClientWidth = this.personDetail.MoreMessageClient.Width;
            this.personDetail.MoreMessageText.ClientHeight = this.personDetail.MoreMessageClient.Height;
            this.personDetail.MoreMessageText.RowMargin = int.Parse(node3.Attributes.GetNamedItem("RowMargin").Value);
            this.personDetail.MoreMessageText.TitleColor = StaticMethods.LoadColor(node3.Attributes.GetNamedItem("TitleColor").Value);
            StaticMethods.LoadFontAndColorFromXMLNode(node3, out font, out color);
            this.personDetail.MoreMessageText.Builder.SetFreeTextBuilder(font);
            this.personDetail.MoreMessageText.DefaultColor = color;
            node3 = nextSibling.ChildNodes.Item(15);
            this.personDetail.MoreMessageBGClient = StaticMethods.LoadRectangleFromXMLNode(node3);
            this.personDetail.MoreMessageBGSize.X = int.Parse(node3.Attributes.GetNamedItem("Width").Value);
            this.personDetail.MoreMessageBGSize.Y = int.Parse(node3.Attributes.GetNamedItem("Height").Value);
            this.personDetail.MoreMessageBGTexture = CacheManager.GetTempTexture(@"Content\Textures\GameComponents\PersonDetail\Data\" + node3.Attributes.GetNamedItem("FileName").Value);
            node3 = nextSibling.ChildNodes.Item(0x10);
            this.personDetail.Switch_MoreMessage = node3.Attributes.GetNamedItem("MoreMessage").Value;
            this.personDetail.Switch_DisplayFamily = node3.Attributes.GetNamedItem("DisplayFamily").Value;
            node3 = nextSibling.ChildNodes.Item(0x15);
            this.personDetail.Treasure1group10Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x16);
            this.personDetail.Treasure2group15Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x17);
            this.personDetail.Treasure3group20Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x18);
            this.personDetail.Treasure4group25Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x19);
            this.personDetail.Treasure5group30Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x1a);
            this.personDetail.Treasure6group40Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x1b);
            this.personDetail.Treasure7group50Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x1c);
            this.personDetail.Treasure8group55Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x1d);
            this.personDetail.Treasure9group60Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(30);
            this.personDetail.Treasure10group70Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x1f);
            this.personDetail.Treasure11group90Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x20);
            this.personDetail.Treasure12group100Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x21);
            this.personDetail.TreasureBGClient = StaticMethods.LoadRectangleFromXMLNode(node3);
            this.personDetail.TreasureBGSize.X = int.Parse(node3.Attributes.GetNamedItem("Width").Value);
            this.personDetail.TreasureBGSize.Y = int.Parse(node3.Attributes.GetNamedItem("Height").Value);
            this.personDetail.TreasureBG = CacheManager.GetTempTexture(@"Content\Textures\GameComponents\PersonDetail\Data\" + node3.Attributes.GetNamedItem("FileName").Value);
            node3 = nextSibling.ChildNodes.Item(0x22);
            this.personDetail.TitleBGClient = StaticMethods.LoadRectangleFromXMLNode(node3);
            this.personDetail.TitleBGSize.X = int.Parse(node3.Attributes.GetNamedItem("Width").Value);
            this.personDetail.TitleBGSize.Y = int.Parse(node3.Attributes.GetNamedItem("Height").Value);
            this.personDetail.TitleBG = CacheManager.GetTempTexture(@"Content\Textures\GameComponents\PersonDetail\Data\" + node3.Attributes.GetNamedItem("FileName").Value);
            node3 = nextSibling.ChildNodes.Item(0x23);
            this.personDetail.StuntBGClient = StaticMethods.LoadRectangleFromXMLNode(node3);
            this.personDetail.StuntBGSize.X = int.Parse(node3.Attributes.GetNamedItem("Width").Value);
            this.personDetail.StuntBGSize.Y = int.Parse(node3.Attributes.GetNamedItem("Height").Value);
            this.personDetail.StuntBG = CacheManager.GetTempTexture(@"Content\Textures\GameComponents\PersonDetail\Data\" + node3.Attributes.GetNamedItem("FileName").Value);
            node3 = nextSibling.ChildNodes.Item(0x24);
            this.personDetail.SkillBGClient = StaticMethods.LoadRectangleFromXMLNode(node3);
            this.personDetail.SkillBGSize.X = int.Parse(node3.Attributes.GetNamedItem("Width").Value);
            this.personDetail.SkillBGSize.Y = int.Parse(node3.Attributes.GetNamedItem("Height").Value);
            this.personDetail.SkillBG = CacheManager.GetTempTexture(@"Content\Textures\GameComponents\PersonDetail\Data\" + node3.Attributes.GetNamedItem("FileName").Value);
            node3 = nextSibling.ChildNodes.Item(0x29);
            this.personDetail.Title1kind1Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x2a);
            this.personDetail.Title2kind200Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x2b);
            this.personDetail.Title3kind10Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x2c);
            this.personDetail.Title4kind2Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x2d);
            this.personDetail.Title5kind21Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x2e);
            this.personDetail.Title6kind7Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x2f);
            this.personDetail.Title7kind3Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x30);
            this.personDetail.Title8kind4Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x31);
            this.personDetail.Title9kind50Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(50);
            this.personDetail.Title10kind5Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x33);
            this.personDetail.Title11kind600Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x34);
            this.personDetail.Title12kind700Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x3d);
            this.personDetail.Stunt0ID0Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x3e);
            this.personDetail.Stunt1ID1Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x3f);
            this.personDetail.Stunt2ID2Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x40);
            this.personDetail.Stunt3ID3Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x41);
            this.personDetail.Stunt4ID4Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x42);
            this.personDetail.Stunt5ID5Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x43);
            this.personDetail.Stunt6ID6Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x44);
            this.personDetail.Stunt7ID7Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x45);
            this.personDetail.Stunt8ID8Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(70);
            this.personDetail.Stunt9ID9Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x47);
            this.personDetail.Stunt10ID10Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x48);
            this.personDetail.Stunt11ID11Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x49);
            this.personDetail.Stunt12ID12Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x4a);
            this.personDetail.Stunt13ID13Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x4b);
            this.personDetail.Stunt14ID14Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x4c);
            this.personDetail.Stunt15ID15Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x4d);
            this.personDetail.Stunt16ID16Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x4e);
            this.personDetail.Stunt17ID17Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x4f);
            this.personDetail.Stunt18ID18Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(80);
            this.personDetail.Stunt19ID19Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x51);
            this.personDetail.Stunt20ID20Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x52);
            this.personDetail.Stunt21ID21Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x53);
            this.personDetail.Stunt22ID22Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x54);
            this.personDetail.Stunt23ID23Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x55);
            this.personDetail.Stunt24ID24Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x56);
            this.personDetail.Stunt25ID25Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x57);
            this.personDetail.Stunt26ID26Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x58);
            this.personDetail.Stunt27ID27Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x59);
            this.personDetail.Stunt28ID28Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(90);
            this.personDetail.Stunt29ID29Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x5b);
            this.personDetail.Skill0ID0Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x5c);
            this.personDetail.Skill1ID1Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x5d);
            this.personDetail.Skill2ID2Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x5e);
            this.personDetail.Skill3ID3Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x5f);
            this.personDetail.Skill4ID4Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x60);
            this.personDetail.Skill5ID5Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x61);
            this.personDetail.Skill6ID6Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x62);
            this.personDetail.Skill7ID7Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x63);
            this.personDetail.Skill8ID8Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(100);
            this.personDetail.Skill9ID9Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x65);
            this.personDetail.Skill10ID10Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x66);
            this.personDetail.Skill11ID11Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x67);
            this.personDetail.Skill12ID12Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x68);
            this.personDetail.Skill13ID13Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x69);
            this.personDetail.Skill14ID14Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x6a);
            this.personDetail.Skill15ID15Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x6b);
            this.personDetail.Skill16ID16Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x6c);
            this.personDetail.Skill17ID17Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x6d);
            this.personDetail.Skill18ID18Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(110);
            this.personDetail.Skill19ID19Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x6f);
            this.personDetail.Skill20ID20Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x70);
            this.personDetail.Skill21ID21Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x71);
            this.personDetail.Skill22ID22Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x72);
            this.personDetail.Skill23ID23Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x73);
            this.personDetail.Skill24ID24Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x74);
            this.personDetail.Skill25ID25Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x75);
            this.personDetail.Skill26ID26Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x76);
            this.personDetail.Skill27ID27Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(0x77);
            this.personDetail.Skill28ID28Client = StaticMethods.LoadRectangleFromXMLNode(node3);
            node3 = nextSibling.ChildNodes.Item(120);
            this.personDetail.Skill29ID29Client = StaticMethods.LoadRectangleFromXMLNode(node3);
        }



        public void SetGraphicsDevice()
        {
            this.LoadDataFromXMLDocument(@"Content\Data\Plugins\PersonDetailData.xml");
        }

        public void SetPerson(object person)
        {
            this.personDetail.SetPerson(person as Person);
        }

        public void SetPosition(ShowPosition showPosition)
        {
            this.personDetail.SetPosition(showPosition);
        }

        public void SetScreen(Screen screen)
        {
            this.personDetail.Initialize(screen);
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Initialize(Screen screen)
        {
            throw new NotImplementedException();
        }

        public string Author
        {
            get
            {
                return this.author;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
        }

        public object Instance
        {
            get
            {
                return this;
            }
        }

        public bool IsShowing
        {
            get
            {
                return this.personDetail.IsShowing;
            }
            set
            {
                this.personDetail.IsShowing = value;
            }
        }

        public string PluginName
        {
            get
            {
                return this.pluginName;
            }
        }

        public string Version
        {
            get
            {
                return this.version;
            }
        }
    }
}










