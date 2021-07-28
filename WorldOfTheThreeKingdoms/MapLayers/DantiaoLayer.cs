using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameGlobal;
using GameObjects;
using Microsoft.Xna.Framework;
using WorldOfTheThreeKingdoms;
using Microsoft.Xna.Framework.Graphics;
using GameManager;
using GamePanels;
using Tools;
using Platforms;

namespace WorldOfTheThreeKingdoms.GameScreens.ScreenLayers
{

    public class General
    {
        public Person Person { get; set; }

        public int Force { get; set; }

        public float Life { get; set; }

        public float Skill { get; set; }
        public int WeaponA { get; set; }
        public int ArmorA { get; set; }
        public Vector2 Position { get; set; }

        public Vector2 LandPosition { get; set; }

        public int LimiteWidth = 0;

        public bool LimiteOrder = true;

        AnimatedTexture atCurrent = null;

        public string Style { get; set; }

        public string Status { get; set; }

        Dictionary<string, AnimatedTexture> atGeneralStatus = new Dictionary<string, AnimatedTexture>();

        string[] styles = new string[] { "General01A", "General01B" };

        string[] status = new string[] { "WalkLeft", "WalkRight", "AttackLeft", "AttackRight", "Failure" };

        public string Direction = "";  //Up Down Left Right

        public float Duration = 0f;

        public float Delay = 0f;

        public Vector2 StartPos;

        public float ActionTime = 0f;

        public float Speed = 20f;

        public float SpeedExt = 0f;

        public float SpeedPlus = 0f;

        public float SpeedNow = 0f;

        public float SayTimeTotal = 0f;

        public float SayTime = 0f;

        public bool SayDisappear = true;

        public string SayWords = "";

        public bool IsPaused
        {
            get
            {
                return atCurrent == null ? true : atCurrent.Paused;
            }
        }
        
        public General(Person p)
        {
            Person = p;

            foreach (var style in styles)
            {
                foreach (var gen in status)
                {
                    var genStatus = new AnimatedTexture(@"Content\Textures\Resources\Dantiao\" + style, gen, "", true, 5)
                    {
                        Depth = DantiaoLayer.depth - 0.035f
                    };

                    atGeneralStatus.Add(style + "-" + gen, genStatus);
                }
            }

        }

        public void ChangeStatus(string style, string status)
        {
            Style = style;

            Status = status;

            atCurrent = atGeneralStatus[Style + "-" + Status];
        }

        public void ChangeWalkToAttack()
        {
            Status = Status.Replace("Walk", "Attack");

            ChangeStatus(Style, Status);
        }

        public void ChangeAttackToWalk()
        {
            Status = Status.Replace("Attack", "Walk");

            ChangeStatus(Style, Status);
        }

        public void ChangeStatusDirection()
        {
            if (Status.Contains("Left"))
            {
                Status = Status.Replace("Left", "Right");
                Direction = "Right";
            }
            else if (Status.Contains("Right"))
            {
                Status = Status.Replace("Right", "Left");
                Direction = "Left";
            }

            ChangeStatus(Style, Status);
        }

        public void Pause()
        {
            Direction = "";
            atCurrent.Paused = true;
        }

        public void Start()
        {
            StartPos = LandPosition;
            ActionTime = 0f;
            atCurrent.Paused = false;

            atCurrent.ChangeFrame(Convert.ToInt32(5 * (Speed + SpeedExt + SpeedPlus) / Speed));
        }

        public void ChangePosition(Vector2 landPos)
        {
            LandPosition = landPos;
        }

        public void Update(float gameTime, Vector2 screenPos)
        {
            if (atCurrent == null)
            {

            }
            else
            {
                if (SayTime > 0f)
                {
                    SayTime -= gameTime;

                    if (SayTime < 0f)
                    {
                        SayTime = 0f;
                        //SayWords = "";
                    }
                }

                if (Delay > 0)
                {
                    Delay -= gameTime;

                    if (Delay < 0)
                    {
                        Delay = 0;
                    }

                }

                if (Duration > 0)
                {
                    Duration -= gameTime;

                    if (Duration <= 0)
                    {
                        Duration = 0;

                        Direction = "";

                        if (Status.Contains("Walk"))
                        {
                            Pause();
                        }
                    }
                }

                if (Delay == 0)
                {

                    ActionTime += gameTime;

                    if (String.IsNullOrEmpty(Direction))
                    {

                    }
                    else
                    {
                        SpeedNow = Speed + SpeedExt + ActionTime * SpeedPlus;

                        if (SpeedNow <= 0)
                        {
                            SpeedNow = 0;
                        }

                        //位移s＝Vot + at²/ 2

                        var moveDis = (Speed + SpeedExt) * ActionTime + SpeedPlus * ActionTime * ActionTime / 2;

                        //ActionTime * realSpeed;

                        Vector2 movePos = Vector2.Zero;

                        if (Direction == "Left")
                        {
                            movePos = new Vector2(-moveDis, 0);

                            if (StartPos.X + movePos.X - DantiaoLayer.basePos.X > 0)
                            {
                                LandPosition = StartPos + movePos;
                            }
                        }
                        else if (Direction == "Right")
                        {
                            movePos = new Vector2(moveDis, 0);

                            if (StartPos.X + movePos.X - DantiaoLayer.basePos.X < 2200 - 120)
                            {
                                LandPosition = StartPos + movePos;
                            }
                        }
                        else if (Direction == "Up")
                        {
                            movePos = new Vector2(0, -moveDis);

                            if (StartPos.Y + movePos.Y - DantiaoLayer.basePos.Y > 150)
                            {
                                LandPosition = StartPos + movePos;
                            }
                        }
                        else if (Direction == "Down")
                        {
                            movePos = new Vector2(0, moveDis);

                            if (StartPos.Y + movePos.Y - DantiaoLayer.basePos.Y < 450)
                            {
                                LandPosition = StartPos + movePos;
                            }
                        }


                    }
                }

                Position = LandPosition - screenPos;

                atCurrent.Position = Position;

                atCurrent.LimiteWidth = LimiteWidth;

                atCurrent.LimiteOrder = LimiteOrder;

                atCurrent.UpdateFrame(gameTime);
            }
        }

        public void Draw()
        {
            if (atCurrent == null)
            {

            }
            else
            {
                atCurrent.DrawFrame(null);
            }

        }

    }

    public class DantiaoLayer
    {
        public static List<Person> Persons = null;

        float totalTime = 0f;

        float elapsedTime = 0f;

        float fightTime = 0f;

        Vector2 scale = Vector2.One;

        public float Alpha = 0f;

        public static float depth = 0.1f;

        public bool IsVisible = true;

        public bool IsStart = true;

        public static Vector2 basePos = Vector2.Zero;

        Vector2 cloudPos = new Vector2(15, 10);

        Rectangle cloudRec = new Rectangle(0, 0, 1000 - 22, 222);

        Vector2 treePos = new Vector2(15, 150);

        Rectangle treeRec = new Rectangle(0, 0, 1000 - 22, 80);

        Vector2 landPos = new Vector2(15, 225);

        Rectangle landRec = new Rectangle(0, 0, 1000 - 22, 530);

        Vector2 general1Pos = Vector2.Zero;

        Vector2 general2Pos = Vector2.Zero;

        Vector2 screenPosPre = Vector2.Zero;

        Vector2 screenPos = Vector2.Zero;

        public int round = 0;

        int Speed = 1;

        ButtonTexture btnStory, btnPagePre, btnPageNext, btnSpeed, btnSpeedUp, btnSpeedDown;
        ButtonTexture btnQianzhi, btnGongji, btnQuanli, btnQuanxiang, btnTaopao, btnTouxiang, btnAnqi;

        General genLeft, genRight;

        int moveDistance = 0;

        float moveTime = 0f;

        public string Stage = "Cloud";

        public int Result = 0;

        string Title = "";

        bool ViewExit = false;

        public TroopDamage damage = null;
        private DuelCommand command;

        private DuelCommand PlayerCommand;

        bool LIsPlayer = true;

        public DantiaoLayer(Person left, Person right, bool LeftIsPlayer)
        {
            if(!Session.Current.Scenario.IsPlayer(left.BelongedFaction) && !Session.Current.Scenario.IsPlayer(left.BelongedFaction))
            { LeftIsPlayer = true; }
            //scale = new Vector2(Convert.ToSingle(Session.ResolutionX) / 800f, Convert.ToSingle(Session.ResolutionY) / 480f);
            LIsPlayer = LeftIsPlayer;

            basePos = new Vector2((Session.ResolutionX - 1000) / 2, (Session.ResolutionY - 620) / 2);

            btnStory = new ButtonTexture(@"Content\Textures\Resources\Dantiao\Story", "Story", basePos + new Vector2(25, 545))
            {
                Visible = true
            };
            btnStory.OnButtonPress += (sender, e) =>
            {
                btnStory.Selected = !btnStory.Selected;
            };


            btnQianzhi = new ButtonTexture(@"Content\Textures\Resources\Dantiao\C1", "C1", basePos + new Vector2(25, 495))
            {
                Visible = true,
                //ViewText = " 牵制"
            };
            btnQianzhi.OnButtonPress += (sender, e) =>
            {
                
                btnQianzhi.Selected = !btnQianzhi.Selected;
            };


            btnGongji = new ButtonTexture(@"Content\Textures\Resources\Dantiao\C2", "C2", basePos + new Vector2(25, 445))
            {
                Visible = true,//ViewText = " 攻击"
            };
            btnGongji.OnButtonPress += (sender, e) =>
            {
                btnGongji.Selected = !btnGongji.Selected;
            };


            btnQuanli = new ButtonTexture(@"Content\Textures\Resources\Dantiao\C3", "C3", basePos + new Vector2(25, 395))
            {
                Visible = true, //ViewText = " 全力"
            };
            btnQuanli.OnButtonPress += (sender, e) =>
            {
               btnQuanli.Selected = !btnQuanli.Selected;
            };


            btnQuanxiang = new ButtonTexture(@"Content\Textures\Resources\Dantiao\C4", "C4", basePos + new Vector2(25, 345))
            {
                Visible = true,//ViewText = " 劝降"
            };
            btnQuanxiang.OnButtonPress += (sender, e) =>
            {
                btnQuanxiang.Selected = !btnQuanxiang.Selected;
            };


            btnTaopao = new ButtonTexture(@"Content\Textures\Resources\Dantiao\C5", "C5", basePos + new Vector2(25, 295))
            {
                Visible = true,
                //ViewText = " 逃跑"
            };
            btnTaopao.OnButtonPress += (sender, e) =>
            {
           btnTaopao.Selected = !btnTaopao.Selected;
            };


            btnTouxiang = new ButtonTexture(@"Content\Textures\Resources\Dantiao\C6", "C6", basePos + new Vector2(25, 245))
            {
                Visible = true,
                //ViewText = " 投降"
            }; 
            btnTouxiang.OnButtonPress += (sender, e) =>
            {
                btnTouxiang.Selected = !btnTouxiang.Selected;
            };


            btnAnqi = new ButtonTexture(@"Content\Textures\Resources\Dantiao\C7", "C7", basePos + new Vector2(125, 495))
            {
                Visible = true,
                //ViewText = "擒拿"
            }; 
            btnAnqi.OnButtonPress += (sender, e) =>
            {
                btnAnqi.Selected = !btnAnqi.Selected;
            };


            btnPagePre = new ButtonTexture(@"Content\Textures\Resources\Dantiao\Page", "Left", basePos + new Vector2(30 + 110, 555));

            btnPagePre.OnButtonPress += (sender, e) =>
            {

            };

            btnPageNext = new ButtonTexture(@"Content\Textures\Resources\Dantiao\Page", "Right", basePos + new Vector2(38 + 228, 555));

            btnPageNext.OnButtonPress += (sender, e) =>
            {

            };

            btnSpeed = new ButtonTexture(@"Content\Textures\Resources\Start\Setting", "Setting", basePos + new Vector2(750, 30))
            {
                Scale = 0.5f,
                Visible = false
            };

            btnSpeed.OnButtonPress += (sender, e) =>
            {

            };

            btnSpeedDown = new ButtonTexture(@"Content\Textures\Resources\Dantiao\Page", "Left", basePos + new Vector2(750 + 110, 35))
            {
                Enable = false,
                Visible = false
            };

            btnSpeedDown.OnButtonPress += (sender, e) =>
            {
                Speed--;
                btnSpeedUp.Enable = true;
                if (Speed == 1)
                {
                    btnSpeedDown.Enable = false;
                }
            };

            btnSpeedUp = new ButtonTexture(@"Content\Textures\Resources\Dantiao\Page", "Right", basePos + new Vector2(750 + 170, 35))
            {
                Visible = false
            };

            btnSpeedUp.OnButtonPress += (sender, e) =>
            {
                Speed++;
                btnSpeedDown.Enable = true;
                if (Speed == 5)
                {
                    btnSpeedUp.Enable = false;
                }
            };

            genLeft = new General(left)
            {
                Force = left.ChallengeStrength,
                WeaponA = (int)left.TreasureWorthforGroup(1100)/10,
                ArmorA = (int)(left.TreasureWorthforGroup(4000) / 5 + left.TreasureWorthforGroup(4100)/5),
                Life = 100 - (left.Tiredness < 80 ? left.Tiredness : 20),
                Skill = 100
            };

            genLeft.ChangeStatus("General01A", "WalkRight");

            //genLeft.ChangePosition(basePos + new Vector2(500-100, 250));
            genLeft.ChangePosition(basePos + new Vector2(250, 250));

            genLeft.Pause();

            genRight = new General(right)
            {
                Force = right.ChallengeStrength,
                WeaponA = (int)right.TreasureWorthforGroup(1100)/10,
                ArmorA = (int)(right.TreasureWorthforGroup(4000)/5 + right.TreasureWorthforGroup(4100) / 5),
                Life = 100 - (right.Tiredness < 80 ? left.Tiredness : 20),
                Skill = 100
            };

            genRight.ChangeStatus("General01B", "WalkLeft");

            //genRight.ChangePosition(basePos + new Vector2(2200-1000+500-80, 250));
            genRight.ChangePosition(basePos + new Vector2(750, 250));

            genRight.Pause();

            Session.PlayMusic("Battle");
            
           
        }
        public void Go(GameTime gameTime)
        {
            Update(gameTime);
            //Draw();        
        }
        public void Update(GameTime gameTime)
        {        
            Session.MainGame.mainGameScreen.Update(gameTime);
            Session.MainGame.mainGameScreen.Draw(gameTime);
        }
        public void Start()
        {
            elapsedTime = 0f;
            IsVisible = true;
            while (Session.MainGame.mainGameScreen.dantiaoLayer != null)
            {              
                Go(new GameTime());
            }
        }

        public void SetScreenPos()
        {
            screenPos.X = (genLeft.LandPosition.X + genRight.LandPosition.X) / 2 + 64 - 500 - basePos.X;
            if (screenPos.X < 0)
            {
                screenPos.X = 0;
            }
            else if (screenPos.X > 2200 - 1000)
            {
                screenPos.X = 2200 - 1000;
            }
            screenPos.Y = (genLeft.LandPosition.Y + genRight.LandPosition.Y) / 2 + 64 - 350 - basePos.Y;
            if (screenPos.Y < 0)
            {
                screenPos.Y = 0;
            }
            //else if (screenPos.Y > 300)
            //{
            //    screenPos.Y = 300;
            //}
        }

        public void Update(float gameTime)
        {           
            if (IsVisible && IsStart)
            {
                totalTime += gameTime;

                elapsedTime += gameTime;

                if (Stage == "Cloud")
                {
                    if (elapsedTime <= 1f)
                    {
                        Alpha = 0f;
                    }
                    else
                    {
                        Stage = "Start";
                        elapsedTime = 0f;
                       
                    }
                }
                else
                {
                    if (totalTime >= 200f && Stage != "Over" && Stage != "OverOut")
                    {
                        Stage = "Over";
                        genLeft.SayDisappear = false;
                        genRight.SayDisappear = false;

                        Result = -1;
                        genLeft.SayWords = "棋逢对手！";
                        genLeft.SayTime = 2f;
                        genRight.SayWords = "痛快痛快！";
                        genRight.SayTime = 2f;

                        Title = "双方平局";
                    }

                    if (Stage == "Start")
                    {
                        if (elapsedTime <= 2f)
                        {
                            Alpha = elapsedTime / 2f;
                        }
                        else
                        {
                            Alpha = 1f;
                            Stage = "Gen1Move";
                            elapsedTime = 0f;
                            Platform.Current.PlayEffect(@"Content\Sound\Dantiao\Moving");
                        }
                    }
                    else if (Stage == "Gen1Move")
                    {
                        genLeft.SpeedExt = 30;
                        genLeft.Direction = "Right";
                        genLeft.Start();
                        Stage = "Gen1Moving";
                        elapsedTime = 0f;
                        screenPosPre = screenPos;
                    }
                    else if (Stage == "Gen1Moving")
                    {
                        screenPos = screenPosPre + (genLeft.LandPosition - genLeft.StartPos);

                        if (elapsedTime >= 2f)
                        {
                            Stage = "Gen1Speak";
                            elapsedTime = 0f;
                        }
                    }
                    else if (Stage == "Gen1Speak")
                    {
                        genLeft.Pause();
                        genLeft.SayTimeTotal = 1f;
                        genLeft.SayTime = 1f;
                        genLeft.SayWords = $"吾乃{genLeft.Person.Name}，哪个敢来应战？";

                        if (genLeft.Person.Name == "曹操")
                        {
                            genLeft.SayWords = $"吾乃{genLeft.Person.Name}，将军别来无恙？";
                        }

                        Stage = "Gen1Speaking";
                        elapsedTime = 0f;
                    }
                    else if (Stage == "Gen1Speaking")
                    {
                        if (elapsedTime >= 2f)
                        {
                           // Stage = "Gen1Run";
                            elapsedTime = 0f;
                            Stage = "Gen2Speak";//03/
                        }
                    }
                    else if (Stage == "Gen1Run")
                    {
                        //PerformCommand(command, genLeft,true);
                       
                        if (Stage != "Over")
                        {
                            
                            Stage = "Gen1Running";
                            elapsedTime = 0f;
                            screenPosPre = screenPos;
                            Platform.Current.PlayEffect(@"Content\Sound\Dantiao\Moving");
                        }                                             
                    }
                    else if (Stage == "Gen1Running")
                    {
                        //if (elapsedTime <= 2f)
                        //{
                        //    screenPos = screenPosPre + new Vector2(genRight.LandPosition.X + 64 - 530 - basePos.X - screenPosPre.X, 0) * elapsedTime / 2f;
                        //}
                        //else if (elapsedTime >= 3f)
                        //{
                        //    Stage = "Gen2Speak";
                        //}
                        var centerLeft = genLeft.LandPosition + new Vector2(64, 64);

                        var centerRight = genRight.LandPosition + new Vector2(64, 64);

                        var distance = Math.Sqrt((centerRight.X - centerLeft.X) * (centerRight.X - centerLeft.X) + (centerRight.Y - centerLeft.Y));

                        if (distance <= 100)
                        {
                            genLeft.Pause();
                            genRight.Start();//此处让被攻击时有特效
                            Stage = "FightRush";
                            genLeft.ChangeWalkToAttack();
                        }
                    }
                    else if (Stage == "Gen2Speak")
                    {
                        genRight.SayTimeTotal = 1f;
                        genRight.SayTime = 1f;
                        genRight.SayWords = $"上将{genRight.Person.Name}在此，贼将休得猖狂！";
                        Stage = "Gen2Speaking";
                        elapsedTime = 0f;
                    }
                    else if (Stage == "Gen2Speaking")
                    {
                        if (elapsedTime >= 2f)
                        {
                                   // Stage = "Gen2Run";
                            elapsedTime = 0f;
                            //Stage = "P1";
                            Stage = "Command";
                        }
                    }
                    else if (Stage == "Command")
                    {
                        if (btnQianzhi.Selected)
                        {
                            btnQianzhi.Selected = !btnQianzhi.Selected;
                            PlayerCommand = DuelCommand.Attack;
                           
                            Stage = "WaitAi";
                        }
                        else if (btnGongji.Selected)
                        {
                            btnGongji.Selected = !btnGongji.Selected;
                            PlayerCommand = DuelCommand.Heavy;
                            Stage = "WaitAi";

                        }
                        else if(btnQuanli.Selected)
                        {
                            btnQuanli.Selected = !btnQuanli.Selected;
                            PlayerCommand = DuelCommand.Ctritical;
                            Stage = "WaitAi";

                        }
                        else if(btnTaopao.Selected)
                        {
                            btnTaopao.Selected = !btnTaopao.Selected;
                            PlayerCommand = DuelCommand.Flee;
                            Stage = "WaitAi";
                        }
                        else if(btnQuanxiang.Selected)
                        {
                            btnQuanxiang.Selected = !btnQuanxiang.Selected;
                            PlayerCommand = DuelCommand.Induce;
                            Stage = "WaitAi";

                        }
                        else if(btnTouxiang.Selected)
                        {
                            btnTouxiang.Selected = !btnTouxiang.Selected;
                            PlayerCommand = DuelCommand.Surround;
                            Stage = "WaitAi";
                        }
                        else if(btnAnqi.Selected)
                        {
                            btnAnqi.Selected = !btnAnqi.Selected;
                            PlayerCommand = DuelCommand.Anqi;
                            Stage = "WaitAi";
                        }
                        if (LIsPlayer) {
                            PerformCommand(PlayerCommand, genLeft, genRight, true);
                        }
                        else {
                            PerformCommand(PlayerCommand, genRight, genLeft, true);
                        }
                        
                        elapsedTime = 0f;
                    }
                    else if (Stage == "WaitAi")
                    {
                        if (LIsPlayer)
                        {
                            command = Duel_Command(genLeft, genRight);
                            PerformCommand(command, genRight, genLeft,false);
                        }
                        else
                        {
                            command = Duel_Command(genRight, genLeft);
                            PerformCommand(command, genLeft, genRight,false);
                        }              
                       if(Stage != "Over")
                        {
                            Stage = "FightRush";
                        }
                        
                        elapsedTime = 0f;

                    }
                    else if (Stage == "Gen2Running")
                    {
                        screenPos = new Vector2(genRight.LandPosition.X + 64 - 530 - basePos.X, screenPos.Y);

                        var centerLeft = genLeft.LandPosition + new Vector2(64, 64);

                        var centerRight = genRight.LandPosition + new Vector2(64, 64);

                        var distance = Math.Sqrt((centerRight.X - centerLeft.X) * (centerRight.X - centerLeft.X) + (centerRight.Y - centerLeft.Y));

                        if (distance <= 100)
                        {
                            genRight.Pause();
                            genLeft.Start();
                            Stage = "FightRush";
                            genRight.ChangeWalkToAttack();
                        }
                    }

                    else if (Stage == "FightRush")
                    {
                        SetScreenPos();

                        genLeft.SpeedExt = 15 + 30;
                        genLeft.SpeedPlus = 25;
                        if (genLeft.Status.Contains("Right"))
                        {
                            genLeft.Direction = "Right";
                        }
                        else {
                            genLeft.Direction = "Left";
                        }
                        //genLeft.ChangeStatusDirection();
                        genLeft.Start();
                        genRight.SpeedExt = 45;
                        genRight.SpeedPlus = 25;
                        if (genRight.Status.Contains("Left"))
                        { 
                            genRight.Direction = "Left";
                        }
                        else
                        {
                            genRight.Direction = "Right";
                        }
                        //genAi.ChangeStatusDirection();
                        genRight.Start();
                        screenPosPre = screenPos;
                        Platform.Current.PlayEffect(@"Content\Sound\Dantiao\Moving");

                        
                        round++;
                       // genLeft.ChangeWalkToAttack();
                       // genLeft.Start();
                       // genRight.ChangeWalkToAttack();
                       // genRight.Start();
                        
                        Stage = "FightRun";
                        elapsedTime = 0f;
                    }
                    else if (Stage == "FightRun")
                    { 
                        var centerLeft = genLeft.LandPosition + new Vector2(64, 64);

                        var centerRight = genRight.LandPosition + new Vector2(64, 64);

                        var distance = Math.Sqrt((centerRight.X - centerLeft.X) * (centerRight.X - centerLeft.X) + (centerRight.Y - centerLeft.Y));

                        if (distance <= 120)
                        {
                            genRight.ChangeWalkToAttack();
                            genLeft.ChangeWalkToAttack();
                            Stage = "Fighting";
                            elapsedTime = 0f;
                        }
                     }
                        //if (!genLeft.IsPaused && !genRight.IsPaused && (genLeft.SpeedNow > 68 || genRight.SpeedNow > 68))
                        //{
                        //    //速度超過一定值，則對沖過去，開始減速
                        //    Stage = "FightRun";

                        //    Platform.Current.PlayEffect(@"Content\Sound\Dantiao\Moving");
                        //}
                        //else
                        //{
                        //    //速度不到一定值，則開始對打
                        //    genLeft.Direction = "";
                        //    genRight.Direction = "";

                        //    Stage = "Fighting";
                        //}
                    
                    else if (Stage == "Fighting")
                    {
                        SetScreenPos();
                        //交匯情況下的武力傷害
                        if (fightTime == 0f)
                        {
                            Fight(gameTime, true, PlayerCommand, genLeft, genRight);
                        }
                        else
                        {
                            Fight(gameTime, true, command, genRight, genLeft);
                        }
                        if (fightTime > 1 && Stage != "Over")
                        {
                            fightTime = 0f;
                             
                                //if (genLeft.Status == "AttackRight")
                                //{
                                
                            //}
                            //// genLeft.SpeedPlus = -new Random().Next(3, 7);
                            //// genLeft.Start();
                            //else {
                                
                            //}                          
                           // genRight.SpeedPlus = -new Random().Next(3, 7);
                           // genRight.Start();
                                Stage = "FightRunBack";
                            
                        }
                    }
                    else if (Stage == "FightRunStop")
                    {
                        SetScreenPos();
                       // if (genLeft.SpeedNow <= 20 || genRight.SpeedNow <= 20)
                       // {
                            //genLeft.Pause();
                            //genRight.Pause();
                            elapsedTime = 0f;
                            Stage = "FightRunBack";
                        //}
                    }
                    else if (Stage == "FightRunBack")
                    {
                        SetScreenPos();

                        if (elapsedTime > 0.3f)
                        {
                            genLeft.ChangeAttackToWalk();
                            genRight.ChangeAttackToWalk();
                            //if (genLeft.IsPaused) {
                            //genAi.Pause();//此处让被攻击后特效消失
                            //genLeft.ChangeStatusDirection();

                            //genLeft.SpeedPlus = new Random().Next(25, 25);

                            //genLeft.Start();
                            //}
                            //else
                            //{
                            //genLeft.Pause();
                            //genAi.ChangeStatusDirection();

                            // genAi.SpeedPlus = new Random().Next(25, 25);

                            //genAi.Start();

                            //}
                            Stage = "WaitRush";

                            Platform.Current.PlayEffect(@"Content\Sound\Dantiao\Moving");
                        }
                    }
                    else if (Stage == "WaitRush")
                    {


                        SetScreenPos();

                        var centerLeft = genLeft.LandPosition + new Vector2(64, 64);

                        var centerRight = genRight.LandPosition + new Vector2(64, 64);

                        if (Math.Abs(genLeft.LandPosition.Y - genRight.LandPosition.Y) <= 10f)
                        {
                            var distance = Math.Sqrt((centerRight.X - centerLeft.X) * (centerRight.X - centerLeft.X) + (centerRight.Y - centerLeft.Y));

                            //if (distance <= 100)
                            //{
                            //    if (genLeft.Duration == 0 && genRight.Duration == 0)
                            //    {
                            //        Stage = "FightRush";
                            //    }
                            //}
                            if (distance >= 500)
                            {
                                //if (genLeft.Status.Contains("Left"))
                                //{
                                    genLeft.ChangeStatusDirection();
                                    genLeft.Pause();//Stage = "P2";
                                //}
                                //else
                                //{
                                    genRight.ChangeStatusDirection();
                                    genRight.Pause();
                                //    //Stage = "P1";
                                //}
                                Stage = "Command";
                                elapsedTime = 0f;
                            }
                        }
                    }
                    else if (Stage == "P2")
                    {
                        Stage = "Gen2Run";
                    }
                   
                    else if (Stage == "Over")
                    {
                        genRight.Direction = "";
                        genLeft.Direction = "";
                        if (InputManager.IsDown && elapsedTime >= 1f)//此处加elapsedTime防止作出命令立即退出
                        {
                            Stage = "OverOut";
                            elapsedTime = 0f;
                        }
                    }
                    else if (Stage == "OverOut")
                    {
                        if (elapsedTime >= 1f &&(genLeft.SayTime ==0 && genRight.SayTime == 0))//此处加saytime防止作出命令立即退出
                        {
                            Session.MainGame.mainGameScreen.dantiaoLayer = null;

                            if (damage == null)
                            {
                                Session.MainGame.mainGameScreen.ReturnToMainMenu();
                            }
                            else
                            {
                                Session.MainGame.mainGameScreen.cloudLayer.IsStart = false;
                                Session.MainGame.mainGameScreen.cloudLayer.IsVisible = false;
                                Session.MainGame.mainGameScreen.cloudLayer.Reverse = false;

                                damage.ChallengeHappened = true;

                                damage.ChallengeStarted = false;

                                damage.ChallengeResult = Result;
                                damage.ChallengeSourcePerson = DantiaoLayer.Persons[0]; //maxStrengthPerson;
                                damage.ChallengeDestinationPerson = DantiaoLayer.Persons[1];
                                //destination;
                                //if (returnValue >= -4 && returnValue <= 10 && returnValue != 0)
                                //{
                                //    flag = returnValue;
                                //}
                                //else   //返回值出错时避免跳出
                                //{
                                //    flag = (GameObject.Chance(chance) ? 1 : 2);
                                //}

                                Session.MainGame.mainGameScreen.EnableUpdate = true;
                            }

                            DantiaoLayer.Persons = null;

                        }
                        Alpha = 1 - elapsedTime;
                    }

                    float elapsedTime2 = elapsedTime < 1f ? elapsedTime : 1f;

                    btnQianzhi.Update();
                    btnGongji.Update(); btnQuanli.Update(); btnQuanxiang.Update(); btnTaopao.Update(); btnTouxiang.Update(); btnAnqi.Update();

                   
                    btnStory.Update();

                    if (btnStory.Selected)
                    {
                        btnPagePre.Update();

                        btnPageNext.Update();
                       
                    }
                     btnSpeed.Update();

                    btnSpeedUp.Update();

                    btnSpeedDown.Update();

                    landRec.Height = 620 - 10 - Convert.ToInt32(landPos.Y);

                    cloudRec.X = Convert.ToInt32(screenPos.X * 0.2f);

                    treeRec.X = Convert.ToInt32(screenPos.X * 0.5f);

                    landRec.X = Convert.ToInt32(screenPos.X);

                    genLeft.Update(gameTime, screenPos);

                    genRight.Update(gameTime, screenPos);


                    landPos = new Vector2(15, 225 - screenPos.Y);

                    if (String.IsNullOrEmpty(Title))
                    {

                    }
                    else
                    {
                        var time = float.Parse("0." + totalTime.ToString().Split(new string[] { "." }, StringSplitOptions.None)[0]);

                        if (time <= 0.5f)
                        {
                            ViewExit = true;
                        }
                        else
                        {
                            ViewExit = false;
                        }

                    }

                }

            }
        }
        public  DuelCommand Duel_Command(General player, General ai)
        {
            General general = ai;
            General wrapper2 = player;
            int num = ai.Force - player.Force;
            //int num2 = general.Life - wrapper2.CurrentStamina;
            float num2 = 100 - genLeft.Life;
            int num3 = 50;
            int num4 = (int)GameMath.Clamp((float)(((num3 + (num * 0.7f)) + (num2 * 0.3f)) + GameObject.Random(-10, 10)), (float)0f, (float)100f);
            int num5 = 100 - num4;
            int num6 = 0;
            int num7 = 0;
            int num8 = 0;
            int num9 = 0;
            if (genRight.Life < (100 / 2))
            {
                num4 -= 10;
                num5 -= 10;
                num6 = 20;
            }
            else if (genRight.Life < (100 / 3))
            {
                num4 -= 20;
                num5 -= 10;
                num6 = 30;
            }
            int num10 = (int)GameMath.Clamp((num * 0.6f) + (num2 * 0.4f), 0f, (float)num6);
            if (num10 > 0)
            {
                num6 -= num10;
                if (num2 >= 0)
                {
                    num5 += num10 / 3;
                    num4 += (num10 * 2) / 3;
                }
                else
                {
                    num5 += (num10 * 2) / 3;
                    num4 += num10 / 3;
                }
            }
            float capturePercent = 0f;
            //CoreMethod.Duel_Induce(ai, player, out capturePercent);
            if (capturePercent >= 0.25f)
            {
                num7 = 20 + num6;
                num4 -= 10;
                num5 -= 10;
                num6 = 0;
            }
            if ((genRight.Life <= (100 / 3)) && (genLeft.Life >= (100 / 2)))
            {
                num8 = 20;
                num4 -= 10;
                num5 -= 10;
            }
            if (genRight.Life < 10)
            {
                num9 = num8;
                num8 = 0;
            }
            //if ((general.Data.Form == GeneralForm.Ghost) || (general.Data.Form == GeneralForm.Boss))
            //{
            //    num6 += num9;
            //    num9 = 0;
            //}
            //if (general.Data.Status != GeneralStatus.Employed)
            //{
            //    num8 += num9;
            //    num6 += num7;
            //    num9 = num7 = 0;
            //}
            int[] rates = new int[] { num4, num5, num6, num7, num8, num9 };
            int rates2 = GameMath.Possibility(rates) + 1;
            return (DuelCommand)rates2;
        }
        public enum DuelCommand
        {
            Attack = 1,
            Ctritical = 3,
            Flee = 5,
            Heavy = 2,
            Induce = 4,
            Surround = 6,
            Anqi=7
        }
        private void HandleA(int force ,int A)
        {
            if (force <= 64) { A = 45; }
            if (force>64 && force <= 79) { A = 50; }
            if (force > 79 && force <= 86) { A = 55; }
            if (force > 86 && force <= 89) { A = 57; }
            if (force > 89 && force <= 91) { A = 60; }
            if (force > 91 && force <= 95) { A = 62; }
            if (force > 95 && force <= 96) { A = 65; }
            if (force > 96 && force <= 98) { A = 70; }
            if (force > 98 && force <= 99) { A = 70; }
            if (force >  99) { A = 80; }
        }
        public void Fight(float gameTime, bool rush, DuelCommand command, General gen,General gen2)
        {
            fightTime += 1;
            int A = 45;
            HandleA(gen.Force,A);
            if (rush)
            {
                //fightTime -= (rush ? 0.3f : 0.8f);
                int basedamage = A / 2 + gen.WeaponA / 2 - gen2.ArmorA + (int)(GameObject.Chance(50) ? 1 : -1) * 5;
                switch (command)
                {
                    case DuelCommand.Attack:
                        basedamage = basedamage/2;rush = true ;
                        break;

                    case DuelCommand.Heavy:
                        rush = (GameObject.Chance(70 * gen.Force/gen2.Force) ? true : false); //gen2.Life -= basedamage*(rush?1:0);
                        //if (!rush) { gen2.Pause(); }
                        break;

                    case DuelCommand.Ctritical:
                        rush = (GameObject.Chance(30) ? true : false);//10 * gen.Force / gen2.Force
                        basedamage = basedamage * 2 ;
                        if (!rush) {
                            basedamage = new Random().Next(10, 30);
                            gen.SayWords += "     -" + basedamage + "      ";
                            gen.SayTime = 2f;
                            gen.Life -= basedamage;
                        }
                        break;

                    case DuelCommand.Induce:
                        basedamage = basedamage / 3; rush = (GameObject.Chance(95) ? true : false);
                        break;

                    case DuelCommand.Flee:
                        basedamage = basedamage / 4; rush = (GameObject.Chance(95) ? true : false);
                        break;

                    case DuelCommand.Surround:
                        basedamage = basedamage / 3; rush = (GameObject.Chance(95) ? true : false);
                        break;
                }
                if (rush)
                {
                    Platform.Current.PlayEffect(@"Content\Sound\Dantiao\NormalAttack");
                    gen2.Life -= basedamage;
                    gen2.SayWords = "-" + basedamage +"      ";
                    gen2.SayTime = 2f;
                }               
                //gen2.Life -= A / 2 + (int)(GameObject.Chance(50) ? 1 : -1)*5;
             
                //genLeft.Life -= Convert.ToSingle(new Random().Next(0, genRight.Force / 5) * 8) / 10f;

                //genRight.Life -= Convert.ToSingle(new Random().Next(0, genLeft.Force / 5) * 8) / 10f;

                if (genLeft.Life <= 0 || genRight.Life <= 0)
                {
                    Stage = "Over";
                    if (genLeft.Life <= 0 && genRight.Life <= 0)
                    {
                        Result = -1;
                        genLeft.SayWords = "棋逢对手！";
                        genLeft.SayTime = 2f;
                        genRight.SayWords = "痛快痛快！";
                        genRight.SayTime = 2f;

                        genLeft.SayDisappear = false;
                        genRight.SayDisappear = false;

                        Title = "双方平局";
                    }
                    else if (genRight.Life <= 0)
                    {
                        Result = 1;

                        genRight.ChangeStatus("General01B", "Failure");

                        genLeft.SayWords = "谁敢再战！";
                        genLeft.SayTime = 2f;
                        genLeft.SayDisappear = false;

                        Title = genLeft.Person.Name + "获胜！";
                    }
                    else if (genLeft.Life <= 0)
                    {
                        Result = 2;

                        genLeft.ChangeStatus("General01A", "Failure");

                        genRight.SayWords = "不过如此！";
                        genRight.SayTime = 2f;
                        genRight.SayDisappear = false;

                        Title = genRight.Person.Name + "获胜！";
                    }

                }
            }
        }

        public void Draw()
        {
            if (IsVisible && Stage != "Cloud")//
            {

                CacheManager.Draw(@"Content\Textures\Resources\Dantiao\Cloud.png", basePos + cloudPos, cloudRec, Color.White * Alpha, SpriteEffects.None, scale, depth - 0.01f);

                CacheManager.Draw(@"Content\Textures\Resources\Dantiao\Tree.png", basePos + treePos, treeRec, Color.White * Alpha, SpriteEffects.None, scale, depth - 0.02f);

                CacheManager.Draw(@"Content\Textures\Resources\Dantiao\Land.png", basePos + landPos, landRec, Color.White * Alpha, SpriteEffects.None, scale, depth - 0.03f);

                CacheManager.Draw(@"Content\Textures\Resources\Dantiao\Avatar.png", basePos + new Vector2(15 + 10, 10 + 10), null, Color.White * Alpha, SpriteEffects.None, scale, depth - 0.04f);

                CacheManager.DrawZhsanAvatar(genLeft.Person, "", new Rectangle(new Point(Convert.ToInt32(basePos.X + 15 + 10), Convert.ToInt32(basePos.Y + 10 + 10)), new Point(150, 150)), Color.White * Alpha, depth - 0.035f);

                CacheManager.DrawString(null, genLeft.Person.Name, basePos + new Vector2(15 + 10 + 10, 10 + 10 + 10), Color.Red * Alpha, 0f, Vector2.Zero, scale.X * 0.8f, SpriteEffects.None, depth - 0.045f);

                CacheManager.DrawString(null, "武力" + genLeft.Force, basePos + new Vector2(25, 175), Color.DarkRed * Alpha, 0f, Vector2.Zero, scale.X * 0.8f, SpriteEffects.None, depth - 0.036f);
                CacheManager.DrawString(null, " 武器" + genLeft.WeaponA, basePos + new Vector2(25 + 70, 175), Color.DarkRed * Alpha, 0f, Vector2.Zero, scale.X * 0.8f, SpriteEffects.None, depth - 0.036f);
                CacheManager.DrawString(null, " 防具" + genLeft.ArmorA, basePos + new Vector2(25 + 140, 175), Color.DarkRed * Alpha, 0f, Vector2.Zero, scale.X * 0.8f, SpriteEffects.None, depth - 0.036f);

                CacheManager.Draw(genLeft.Person.TreasurePictureforGroup(1100), new Rectangle((int)basePos.X + 30 + 70, (int)basePos.Y + 175 + 20, 40, 40), null, Color.White * Alpha, 0f, Vector2.Zero, SpriteEffects.None, 0.0355f);
                CacheManager.Draw(genLeft.Person.TreasurePictureforGroup(4000), new Rectangle((int)basePos.X + 75 + 70, (int)basePos.Y + 175 + 20, 40, 40), null, Color.White * Alpha, 0f, Vector2.Zero, SpriteEffects.None, 0.0355f);
                CacheManager.Draw(genLeft.Person.TreasurePictureforGroup(4100), new Rectangle((int)basePos.X + 120 + 70, (int)basePos.Y + 175 + 20, 40, 40), null, Color.White * Alpha, 0f, Vector2.Zero, SpriteEffects.None, 0.0355f);
                //CacheManager.Draw(genLeft.Person.TreasurePictureforGroup(1100).Name, basePos + new Vector2(25 + 70, 175), null, Color.White * Alpha, SpriteEffects.None, scale, 0.037f);

                CacheManager.Draw(@"Content\Textures\Resources\Dantiao\SwordBlackLeft.png", basePos + new Vector2(210, 25), null, Color.White * Alpha, SpriteEffects.None, scale, depth - 0.04f);

                CacheManager.Draw(@"Content\Textures\Resources\Dantiao\SwordBlackLeft.png", basePos + new Vector2(210, 55), null, Color.White * Alpha, SpriteEffects.None, scale, depth - 0.04f);

                CacheManager.Draw(@"Content\Textures\Resources\Dantiao\SwordCyanLeft.png", basePos + new Vector2(210, 25), new Rectangle(0, 0, Convert.ToInt32(288 * Convert.ToSingle(genLeft.Skill) / 100f), 25), Color.White * Alpha, SpriteEffects.None, scale, depth - 0.045f);

                CacheManager.Draw(@"Content\Textures\Resources\Dantiao\SwordRedLeft.png", basePos + new Vector2(210, 55), new Rectangle(0, 0, Convert.ToInt32(288 * Convert.ToSingle(genLeft.Life) / 100f), 25), Color.White * Alpha, SpriteEffects.None, scale, depth - 0.045f);



                CacheManager.Draw(@"Content\Textures\Resources\Dantiao\Avatar.png", basePos + new Vector2(1000 - 15 - 15 - 150, 620 - 20 - 15 - 150), null, Color.White * Alpha, SpriteEffects.None, scale, depth - 0.04f);

                CacheManager.DrawZhsanAvatar(genRight.Person, "", new Rectangle(new Point(Convert.ToInt32(basePos.X + 1000 - 15 - 15 - 150), Convert.ToInt32(basePos.Y + 620 - 20 - 15 - 150)), new Point(150, 150)), Color.White * Alpha, depth - 0.035f);

                CacheManager.DrawString(null, genRight.Person.Name, basePos + new Vector2(1000 - 15 - 15 - 140, 620 - 20 - 15 - 140), Color.Blue * Alpha, 0f, Vector2.Zero, scale.X * 0.8f, SpriteEffects.None, depth - 0.045f);

                CacheManager.DrawString(null, "武力" + genRight.Force, basePos + new Vector2(1000 - 280, 620 - 218), Color.DarkBlue * Alpha, 0f, Vector2.Zero, scale.X * 0.8f, SpriteEffects.None, depth - 0.036f);
                CacheManager.DrawString(null, " 武器" + genRight.WeaponA, basePos + new Vector2(1000 -210, 620 - 218), Color.DarkBlue * Alpha, 0f, Vector2.Zero, scale.X * 0.8f, SpriteEffects.None, depth - 0.036f);
                CacheManager.DrawString(null, " 防具" + genRight.ArmorA, basePos + new Vector2(1000 - 140, 620 - 218), Color.DarkBlue * Alpha, 0f, Vector2.Zero, scale.X * 0.8f, SpriteEffects.None, depth - 0.036f);
                CacheManager.Draw(genRight.Person.TreasurePictureforGroup(1100), new Rectangle((int)basePos.X + 1000 - 280 + 75, (int)basePos.Y + 620 - 218 - 40, 40, 40), null, Color.White * Alpha, 0f, Vector2.Zero, SpriteEffects.None, 0.0355f);
                CacheManager.Draw(genRight.Person.TreasurePictureforGroup(4000), new Rectangle((int)basePos.X + 1000 - 280 + 120, (int)basePos.Y + 620 - 218 - 40, 40, 40), null, Color.White * Alpha, 0f, Vector2.Zero, SpriteEffects.None, 0.0355f);
                CacheManager.Draw(genRight.Person.TreasurePictureforGroup(4100), new Rectangle((int)basePos.X + 1000 - 280 + 165, (int)basePos.Y + 620 - 218 - 40, 40, 40), null, Color.White * Alpha, 0f, Vector2.Zero, SpriteEffects.None, 0.0355f);

                CacheManager.Draw(@"Content\Textures\Resources\Dantiao\SwordBlackRight.png", basePos + new Vector2(510, 520), null, Color.White * Alpha, SpriteEffects.None, scale, depth - 0.04f);

                CacheManager.Draw(@"Content\Textures\Resources\Dantiao\SwordBlackRight.png", basePos + new Vector2(510, 550), null, Color.White * Alpha, SpriteEffects.None, scale, depth - 0.04f);

                CacheManager.Draw(@"Content\Textures\Resources\Dantiao\SwordCyanRight.png", basePos + new Vector2(510 + Convert.ToInt32(288 * (1 - Convert.ToSingle(genRight.Skill) / 100f)), 520), new Rectangle(Convert.ToInt32(288 * (1 - Convert.ToSingle(genRight.Skill) / 100f)), 0, Convert.ToInt32(288 * Convert.ToSingle(genRight.Skill) / 100), 25), Color.White * Alpha, SpriteEffects.None, scale, depth - 0.045f);

                CacheManager.Draw(@"Content\Textures\Resources\Dantiao\SwordRedRight.png", basePos + new Vector2(510 + Convert.ToInt32(288 * (1 - Convert.ToSingle(genRight.Life) / 100f)), 550), new Rectangle(Convert.ToInt32(288 * (1 - Convert.ToSingle(genRight.Life) / 100f)), 0, Convert.ToInt32(288 * Convert.ToSingle(genRight.Life) / 100), 25), Color.White * Alpha, SpriteEffects.None, scale, depth - 0.045f);

                CacheManager.DrawString(null, "第 " + round + " 回合", basePos + new Vector2(550, 35), Color.Red * Alpha, 0f, Vector2.Zero, scale.X, SpriteEffects.None, depth - 0.036f);

               // CacheManager.DrawString(null, "牵制", basePos + new Vector2(25, 495), Color.DarkRed * Alpha, 0f, Vector2.Zero, scale.X * 0.8f, SpriteEffects.None, depth - 0.045f);
                btnStory.Draw();

                if (btnStory.Selected)
                {
                    btnPagePre.Draw();

                    btnPageNext.Draw();

                    CacheManager.DrawString(null, "1/2 回合", basePos + new Vector2(35 + 138, 557), Color.Blue * Alpha, 0f, Vector2.Zero, scale.X * 0.7f, SpriteEffects.None, depth - 0.036f);
                }
                btnQianzhi.Draw();
                btnGongji.Draw(); btnQuanli.Draw(); btnQuanxiang.Draw(); btnTaopao.Draw(); btnTouxiang.Draw(); btnAnqi.Draw();
                btnSpeed.Draw();

                //CacheManager.DrawString(null, "X" + Speed, basePos + new Vector2(750 + 50, 35), Color.Black * Alpha, 0f, Vector2.Zero, scale.X, SpriteEffects.None, depth - 0.036f);

                btnSpeedDown.Draw();

                btnSpeedUp.Draw();

                if (basePos.X - 128 + 30 <= genLeft.Position.X && genLeft.Position.X < basePos.X + 1000 - 70)
                {
                    if (basePos.X > genLeft.Position.X)
                    {
                        genLeft.LimiteWidth = Convert.ToInt32(128 - (basePos.X - genLeft.Position.X));

                        genLeft.LimiteOrder = true;
                    }
                    else if (genLeft.Position.X > basePos.X + 1000 - 128)
                    {
                        genLeft.LimiteWidth = Convert.ToInt32(1000 - (genLeft.Position.X - basePos.X));

                        genLeft.LimiteOrder = false;
                    }
                    else
                    {
                        genLeft.LimiteWidth = 0;
                    }

                    genLeft.Draw();
                }

                if (basePos.X - 128 + 30 <= genRight.Position.X && genRight.Position.X < basePos.X + 1000 - 65)
                {
                    if (basePos.X > genRight.Position.X)
                    {
                        genRight.LimiteWidth = Convert.ToInt32(128 - (basePos.X - genRight.Position.X));

                        genRight.LimiteOrder = true;
                    }
                    else if (genRight.Position.X > basePos.X + 1000 - 128)
                    {
                        genRight.LimiteWidth = Convert.ToInt32(1000 - (genRight.Position.X - basePos.X));

                        genRight.LimiteOrder = false;
                    }
                    else
                    {
                        genRight.LimiteWidth = 0;
                    }

                    genRight.Draw();
                }

                if (genLeft.SayTime > 0f || !genLeft.SayDisappear)
                {
                    CacheManager.Draw(@"Content\Textures\Resources\Dantiao\StarLeft.png", basePos + new Vector2(185, 82), null, Color.White * Alpha, SpriteEffects.None, scale, depth - 0.035f);

                    CacheManager.DrawString(null, genLeft.SayWords.WordsSubString(Convert.ToInt32((1 - genLeft.SayTime / genLeft.SayTimeTotal) * genLeft.SayWords.Length), 0).SplitLineString(12), basePos + new Vector2(185 + 45, 82 + 38), Color.Black * Alpha, 0f, Vector2.Zero, scale.X * 0.8f, SpriteEffects.None, depth - 0.036f);
                }

                if (genRight.SayTime > 0f || !genRight.SayDisappear)
                {
                    CacheManager.Draw(@"Content\Textures\Resources\Dantiao\StarRight.png", basePos + new Vector2(470, 405), null, Color.White * Alpha, SpriteEffects.None, scale, depth - 0.035f);

                    CacheManager.DrawString(null, genRight.SayWords.WordsSubString(Convert.ToInt32((1 - genRight.SayTime / genRight.SayTimeTotal) * genRight.SayWords.Length), 0).SplitLineString(12), basePos + new Vector2(470 + 35, 405 + 38), Color.Black * Alpha, 0f, Vector2.Zero, scale.X * 0.8f, SpriteEffects.None, depth - 0.036f);
                }

                if (String.IsNullOrEmpty(Title))
                {
                    
                }
                else
                {
                    CacheManager.DrawString(null, Title, basePos + new Vector2(350, 150), Color.Red * Alpha, 0f, Vector2.Zero, scale.X * 3f, SpriteEffects.None, depth - 0.036f);
                }

                if (ViewExit)
                {
                    string exitWords = "点击任意处以退出。";

                    CacheManager.DrawString(null, exitWords, basePos + new Vector2(300, 450), Color.Black * Alpha, 0f, Vector2.Zero, scale.X * 1.5f, SpriteEffects.None, depth - 0.036f);
                }

                CacheManager.Draw(@"Content\Textures\Resources\Dantiao\Ground.png", basePos, null, Color.White * Alpha, SpriteEffects.None, scale, depth - 0.09f);

            }
        }

        public class GameMath
        {
            // Fields
            private const int INFINITE = 0x1869f;
   
            public static float Clamp(float value, float min, float max)
            {
                if (value < min)
                {
                    value = min;
                    return value;
                }
                if (value > max)
                {
                    value = max;
                }
                return value;
            }
            // Methods      
            public static int Clamp(int value, int min = 0, int max = 0x1869f) =>
                Clamp(value, min, max);
            public static int Possibility(params int[] rates)
            {
                int num = rates.Sum();
                int num3 = 0;
                int num4 = GameObject.Random(0, num - 1);
                for (int i = 0; i < rates.Length; i++)
                {
                    if (rates[i] < 0)
                    {
                        rates[i] = 0;
                    }
                    if ((num4 >= num3) && (num4 < (num3 + rates[i])))
                    {
                        return i;
                    }
                    num3 += rates[i];
                }
                return 0;
            }  
        }
        public void PerformCommand(DuelCommand command, General gen, General destin, bool player)
        {
            gen.SayTimeTotal = 2f;
            gen.SayTime = 2f;
            string Name = gen.Person.Name;
            string Name2 = destin.Person.Name;
            //if (LIsPlayer && player||(!LIsPlayer && !player)) { Name = genLeft.Person.Name; Name2 = genAi.Person.Name; }
            switch (command)
            {
                case DuelCommand.Attack:
                    gen.SayWords= $"今日就让你见识一下我{Name}之武艺,吃我一击!";
                    break;

                case DuelCommand.Heavy:
                    gen.SayWords = $"今日不施展下我之绝招,不知道我{Name}的厉害!";
                    break;

                case DuelCommand.Ctritical:
                    gen.SayWords = $"今日你我势不两立,吃我{Name}的最后一招!";                  
                    break;

                case DuelCommand.Induce:
                    gen.SayWords =$"稍安勿躁,听我{Name}一言......";
                    if((gen.Life - destin.Life) > 50 && destin.Person.Loyalty < 100 || gen.Person.CanConvince(destin.Person))
                    {
                        destin.SayWords = $"{Name}将军言之有理！";
                        Title = Name + "成功说服"+ Name2;
                        if (LIsPlayer && player || (!LIsPlayer && !player))
                        {
                            Result = 10; //10、P2武将被说服
                        }
                        else
                        {
                            Result = 9;
                        }
                        gen.SayDisappear = false;
                        destin.SayDisappear = false;
                        Stage = "Over";
                    }
                    break;

                case DuelCommand.Flee:
                    gen.SayWords = $"{Name2}果然名不虚传,我今日非你敌手,后会有期!";
                    if (Math.Abs(gen.Life - destin.Life) < 50 && Math.Abs(gen.Person.BaseStrength - destin.Person.BaseStrength) < 25) {
                        Title = Name + "逃跑";
                        if (LIsPlayer && player || (!LIsPlayer && !player))
                        {
                            Result = 5; //5、P1武将逃跑
                        }
                        else
                        {
                            Result = 6;
                        }
                        Stage = "Over";
                        gen.ChangeStatusDirection();
                    }
                    break;

                case DuelCommand.Surround:
                    gen.SayWords = $"且慢动手，{Name}有要事相商......!";
                    
                    Title = Name + "被俘";
                    if (LIsPlayer && player || (!LIsPlayer && !player))
                    {
                        Result = 7;//7、P1武将被俘虏
                    }
                    else
                    {
                        Result = 8;
                    }
                    Stage = "Over";
                    gen.ChangeStatus(gen.Style, "Failure");
                    gen.SayDisappear = false;
                    break;

                case DuelCommand.Anqi:
                    gen.SayWords = $"{Name2}贼将休走!!!";
                    if ((gen.Life - destin.Life) > 50 && (gen.Person.BaseStrength - destin.Person.BaseStrength) > 25)
                    {
                        Title = Name2 + "被俘";
                        if (LIsPlayer && player || (!LIsPlayer && !player))
                        {
                            Result = 8; 
                            
                        }
                        else
                        {
                            Result = 7;//
                        }
                        Stage = "Over";
                        destin.ChangeStatus(destin.Style, "Failure");
                        gen.SayDisappear = false;
                        
                    }
                    break;
            }
        }

    }

}
