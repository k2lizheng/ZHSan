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

        public DantiaoLayer(Person left, Person right)
        {
            //scale = new Vector2(Convert.ToSingle(Session.ResolutionX) / 800f, Convert.ToSingle(Session.ResolutionY) / 480f);

            basePos = new Vector2((Session.ResolutionX - 1000) / 2, (Session.ResolutionY - 620) / 2);

            btnStory = new ButtonTexture(@"Content\Textures\Resources\Dantiao\Story", "Story", basePos + new Vector2(25, 545))
            {
                Visible = true
            };
            btnStory.OnButtonPress += (sender, e) =>
            {
                btnStory.Selected = !btnStory.Selected;
            };


            btnQianzhi = new ButtonTexture(@"Content\Textures\Resources\Dantiao\Button", "Button", basePos + new Vector2(25, 495))
            {
                Visible = true,
                ViewText = " 牵制"
            };
            btnQianzhi.OnButtonPress += (sender, e) =>
            {
                
                btnQianzhi.Selected = !btnQianzhi.Selected;
            };


            btnGongji = new ButtonTexture(@"Content\Textures\Resources\Dantiao\Button", "Button", basePos + new Vector2(25, 445))
            {
                Visible = true,ViewText = " 攻击"
            };
            btnGongji.OnButtonPress += (sender, e) =>
            {
                btnGongji.Selected = !btnGongji.Selected;
            };


            btnQuanli = new ButtonTexture(@"Content\Textures\Resources\Dantiao\Button", "Button", basePos + new Vector2(25, 395))
            {
                Visible = true, ViewText = " 全力"
            };
            btnQuanli.OnButtonPress += (sender, e) =>
            {
               btnQuanli.Selected = !btnQuanli.Selected;
            };


            btnQuanxiang = new ButtonTexture(@"Content\Textures\Resources\Dantiao\Button", "Button", basePos + new Vector2(25, 345))
            {
                Visible = false,ViewText = " 劝降"
            };
            btnQuanxiang.OnButtonPress += (sender, e) =>
            {
                //btnQuanxiang.Selected = !btnQuanxiang.Selected;
            };


            btnTaopao = new ButtonTexture(@"Content\Textures\Resources\Dantiao\Button", "Button", basePos + new Vector2(25, 295))
            {
                Visible = false,
                ViewText = " 逃跑"
            };
            btnTaopao.OnButtonPress += (sender, e) =>
            {
               // btnTaopao.Selected = !btnTaopao.Selected;
            };


            btnTouxiang = new ButtonTexture(@"Content\Textures\Resources\Dantiao\Button", "Button", basePos + new Vector2(25, 245))
            {
                Visible = false,
                ViewText = " 投降"
            }; 
            btnTouxiang.OnButtonPress += (sender, e) =>
            {
               // btnTouxiang.Selected = !btnTouxiang.Selected;
            };


            btnAnqi = new ButtonTexture(@"Content\Textures\Resources\Dantiao\Button", "Button", basePos + new Vector2(125, 495))
            {
                Visible = false,
                ViewText = "暗器"
            }; 
            btnAnqi.OnButtonPress += (sender, e) =>
            {
               // btnAnqi.Selected = !btnAnqi.Selected;
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
                Life = 100,
                Skill = 100
            };

            genLeft.ChangeStatus("General01A", "WalkRight");

            //genLeft.ChangePosition(basePos + new Vector2(500-100, 250));
            genLeft.ChangePosition(basePos + new Vector2(250, 250));

            genLeft.Pause();

            genRight = new General(right)
            {
                Force = right.ChallengeStrength,
                Life = 100,
                Skill = 100
            };

            genRight.ChangeStatus("General01B", "WalkLeft");

            //genRight.ChangePosition(basePos + new Vector2(2200-1000+500-80, 250));
            genRight.ChangePosition(basePos + new Vector2(750, 250));

            genRight.Pause();

            Session.PlayMusic("Battle");
        }

        public void Start()
        {
            elapsedTime = 0f;
            IsVisible = true;
           
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
                    if (totalTime >= 120f && Stage != "Over" && Stage != "OverOut")
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
                        genLeft.SayTimeTotal = 2f;
                        genLeft.SayTime = 2f;
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
                        PerformCommand(command, genLeft);
                        genLeft.SpeedExt = 15+30;
                        genLeft.SpeedPlus = 10;
                        genLeft.Direction = "Right";
                        genLeft.Start();
                        Stage = "Gen1Running";
                        elapsedTime = 0f;
                        screenPosPre = screenPos;
                        Platform.Current.PlayEffect(@"Content\Sound\Dantiao\Moving");
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
                        genRight.SayTimeTotal = 2f;
                        genRight.SayTime = 2f;
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
                            Stage = "P1";
                        }
                    }
                    else if (Stage == "P1")
                    {
                        if (btnQianzhi.Selected)
                        {
                            btnQianzhi.Selected = !btnQianzhi.Selected;
                            Stage = "Gen1Run";
                            elapsedTime = 0f;
                            command = DuelCommand.Attack;

                        }
                        if (btnGongji.Selected)
                        {
                            btnGongji.Selected = !btnGongji.Selected;
                            Stage = "Gen1Run";
                            elapsedTime = 0f;
                            command = DuelCommand.Heavy;

                        }
                        if (btnQuanli.Selected)
                        {
                            btnQuanli.Selected = !btnQuanli.Selected;
                            Stage = "Gen1Run";
                            elapsedTime = 0f;
                            command = DuelCommand.Ctritical;

                        }

                    }
                    else if (Stage == "Gen2Run")
                    {
                        command = Duel_Command(genLeft, genRight);
                        PerformCommand(command, genRight);               
                        genRight.SpeedExt = 45;
                        genRight.SpeedPlus = 25;
                        genRight.Direction = "Left";
                        genRight.Start();
                        Stage = "Gen2Running";
                        elapsedTime = 0f;
                        screenPosPre = screenPos;
                        Platform.Current.PlayEffect(@"Content\Sound\Dantiao\Moving");
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
                        round++;
                       // genLeft.ChangeWalkToAttack();
                       // genLeft.Start();
                       // genRight.ChangeWalkToAttack();
                       // genRight.Start();
                        elapsedTime = 0f;
                        Stage = "FightRun";
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
                    }
                    else if (Stage == "FightRun")
                    {
                        SetScreenPos();
                        //交匯情況下的武力傷害
                        if (genLeft.Status== "AttackRight")
                        {
                            Fight(gameTime, true,command, genLeft,genRight);
                        }
                        else
                        {
                            Fight(gameTime, true, command, genRight,genLeft);
                        }
                        if (elapsedTime > 0.4f)
                        {
                            fightTime = 0f;
                            if (genLeft.Status == "AttackRight")
                            {
                                genLeft.ChangeAttackToWalk();
                            }
                            // genLeft.SpeedPlus = -new Random().Next(3, 7);
                            // genLeft.Start();
                            else {
                                genRight.ChangeAttackToWalk();
                            }                          
                           // genRight.SpeedPlus = -new Random().Next(3, 7);
                           // genRight.Start();
                            Stage = "FightRunStop";
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
                            if (genLeft.IsPaused) {
                                genRight.Pause();//此处让被攻击后特效消失
                                genLeft.ChangeStatusDirection();

                                genLeft.SpeedPlus = new Random().Next(8, 8);
                        
                                 genLeft.Start();
                            }
                            else
                            {
                                genLeft.Pause();
                                genRight.ChangeStatusDirection();

                                genRight.SpeedPlus = new Random().Next(8, 8);

                                genRight.Start();

                            }
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
                            if (distance >= 300&& elapsedTime > 0.3f)
                            {
                                if (genLeft.Status.Contains("Left"))
                                {
                                    genLeft.ChangeStatusDirection();
                                    genLeft.Pause();Stage = "P2";
                                }
                                else
                                {
                                    genRight.ChangeStatusDirection();
                                    genRight.Pause();
                                    Stage = "P1";
                                }
                                elapsedTime = 0f;
                            }
                        }
                    }
                    else if (Stage == "P2")
                    {
                        Stage = "Gen2Run";
                    }
                    //else if (Stage == "Fighting")
                    //{
                    //    //對打情況下的傷害
                    //    Fight(gameTime, false);

                    //    if (elapsedTime >= 5f)
                    //    {
                    //        General gen1, gen2;

                    //        var ran = new Random().Next(0, 10);

                    //        if (ran == 0 || ran == 2 || ran == 4 || ran == 6 || ran == 8)
                    //        {
                    //            gen1 = genLeft;
                    //            gen2 = genRight;
                    //        }
                    //        else
                    //        {
                    //            gen1 = genRight;
                    //            gen2 = genLeft;
                    //        }

                    //        if (0 <= ran && ran <= 2)
                    //        {
                    //            //保持不動

                    //        }
                    //        else if (3 <= ran && ran <= 5)
                    //        {
                    //            //後退、跟進
                    //            if (gen1.LandPosition.X <= gen2.LandPosition.X)
                    //            {
                    //                if (gen1.LandPosition.X - basePos.X > 60)
                    //                {
                    //                    gen1.Direction = "Left";
                    //                    gen1.ChangeStatus(gen1.Style, "WalkRight");
                    //                    gen1.Duration = 2f;

                    //                    gen2.Direction = "Left";
                    //                    gen2.ChangeStatus(gen2.Style, "WalkLeft");
                    //                    gen2.Delay = 1f;

                    //                    gen1.Start();

                    //                    gen2.Start();

                    //                    Stage = "WaitRush";
                    //                }
                    //            }
                    //            else
                    //            {
                    //                if (gen1.LandPosition.X - basePos.X < 2200 - 200)
                    //                {
                    //                    gen1.Direction = "Right";
                    //                    gen1.ChangeStatus(gen1.Style, "WalkLeft");
                    //                    gen1.Duration = 2f;

                    //                    gen2.Direction = "Right";
                    //                    gen2.ChangeStatus(gen2.Style, "WalkRight");
                    //                    gen2.Delay = 1f;

                    //                    gen1.Start();

                    //                    gen2.Start();

                    //                    Stage = "WaitRush";
                    //                }
                    //            }

                    //        }
                    //        else if (6 <= ran && ran <= 7)
                    //        {
                    //            if (gen1.LandPosition.Y - basePos.Y > 200 && gen2.LandPosition.Y - basePos.Y > 150)
                    //            {
                    //                //向上
                    //                gen1.Direction = "Up";
                    //                gen1.ChangeAttackToWalk();
                    //                gen1.Duration = 2f;

                    //                gen2.Direction = "Up";
                    //                gen2.ChangeAttackToWalk();
                    //                gen2.Delay = 1f;

                    //                gen1.Start();

                    //                gen2.Start();

                    //                Stage = "WaitRush";

                    //                Platform.Current.PlayEffect(@"Content\Sound\Dantiao\Moving");
                    //            }
                    //        }
                    //        else if (8 <= ran && ran <= 10)
                    //        {
                    //            if (gen1.LandPosition.Y - basePos.Y < 460 && gen2.LandPosition.Y - basePos.Y < 460)
                    //            {
                    //                //向下
                    //                gen1.Direction = "Down";
                    //                gen1.ChangeAttackToWalk();
                    //                gen1.Duration = 2f;

                    //                gen2.Direction = "Down";
                    //                gen2.ChangeAttackToWalk();
                    //                gen2.Delay = 1f;

                    //                gen1.Start();

                    //                gen2.Start();

                    //                Stage = "WaitRush";


                    //                Platform.Current.PlayEffect(@"Content\Sound\Dantiao\Moving");
                    //            }
                    //        }

                    //        elapsedTime = 0f;
                    //    }
                    //}
                    else if (Stage == "Over")
                    {
                        if (InputManager.IsDown)
                        {
                            Stage = "OverOut";
                            elapsedTime = 0f;
                        }
                    }
                    else if (Stage == "OverOut")
                    {
                        if (elapsedTime >= 1f)
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
            Surround = 6
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
            fightTime += gameTime;
            int A = 45;
            HandleA(gen.Force,A);
            if (fightTime >= 0.8f || rush && fightTime >= 0.3f)
            {
                fightTime -= (rush ? 0.3f : 0.8f);
                int basedamage = A / 2 + (int)(GameObject.Chance(50) ? 1 : -1) * 5;
                switch (command)
                {
                    case DuelCommand.Attack:
                        gen2.Life -=basedamage/2;rush = (GameObject.Chance(95) ? true : false);
                        break;

                    case DuelCommand.Heavy:
                        rush = (GameObject.Chance(70*gen.Force/gen2.Force) ? true : false); gen2.Life -= basedamage*(rush?1:0);
                        //if (!rush) { gen2.Pause(); }
                        break;

                    case DuelCommand.Ctritical:
                         rush = (GameObject.Chance(10 * gen.Force / gen2.Force) ? true : false);gen2.Life -= basedamage*2 * (rush ? 1 : 0);
                        if (!rush) { gen.Life -= basedamage / 2; }
                        break;

                    case DuelCommand.Induce:
                        gen2.Life -= basedamage / 2; rush = (GameObject.Chance(95) ? true : false);
                        break;

                    case DuelCommand.Flee:
                        gen2.Life -= basedamage / 2; rush = (GameObject.Chance(95) ? true : false);
                        break;

                    case DuelCommand.Surround:
                        gen2.Life -= basedamage / 2; rush = (GameObject.Chance(95) ? true : false);
                        break;
                }
                if (rush)
                {
                    Platform.Current.PlayEffect(@"Content\Sound\Dantiao\NormalAttack");              
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
            if (IsVisible && Stage != "Cloud")
            {

                CacheManager.Draw(@"Content\Textures\Resources\Dantiao\Cloud.png", basePos + cloudPos, cloudRec, Color.White * Alpha, SpriteEffects.None, scale, depth - 0.01f);

                CacheManager.Draw(@"Content\Textures\Resources\Dantiao\Tree.png", basePos + treePos, treeRec, Color.White * Alpha, SpriteEffects.None, scale, depth - 0.02f);

                CacheManager.Draw(@"Content\Textures\Resources\Dantiao\Land.png", basePos + landPos, landRec, Color.White * Alpha, SpriteEffects.None, scale, depth - 0.03f);

                CacheManager.Draw(@"Content\Textures\Resources\Dantiao\Avatar.png", basePos + new Vector2(15 + 10, 10 + 10), null, Color.White * Alpha, SpriteEffects.None, scale, depth - 0.04f);

                CacheManager.DrawZhsanAvatar(genLeft.Person, genLeft.Person.PictureIndex, 9999, "", new Rectangle(new Point(Convert.ToInt32(basePos.X + 15 + 10), Convert.ToInt32(basePos.Y + 10 + 10)), new Point(150, 150)), Color.White * Alpha, depth - 0.035f);

                CacheManager.DrawString(null, genLeft.Person.Name, basePos + new Vector2(15 + 10 + 10, 10 + 10 + 10), Color.Red * Alpha, 0f, Vector2.Zero, scale.X * 0.8f, SpriteEffects.None, depth - 0.045f);

                CacheManager.DrawString(null, "武力：" + genLeft.Force, basePos + new Vector2(25, 175), Color.DarkRed * Alpha, 0f, Vector2.Zero, scale.X * 0.8f, SpriteEffects.None, depth - 0.036f);

                CacheManager.Draw(@"Content\Textures\Resources\Dantiao\SwordBlackLeft.png", basePos + new Vector2(210, 25), null, Color.White * Alpha, SpriteEffects.None, scale, depth - 0.04f);

                CacheManager.Draw(@"Content\Textures\Resources\Dantiao\SwordBlackLeft.png", basePos + new Vector2(210, 55), null, Color.White * Alpha, SpriteEffects.None, scale, depth - 0.04f);

                CacheManager.Draw(@"Content\Textures\Resources\Dantiao\SwordCyanLeft.png", basePos + new Vector2(210, 25), new Rectangle(0, 0, Convert.ToInt32(288 * Convert.ToSingle(genLeft.Skill) / 100f), 25), Color.White * Alpha, SpriteEffects.None, scale, depth - 0.045f);

                CacheManager.Draw(@"Content\Textures\Resources\Dantiao\SwordRedLeft.png", basePos + new Vector2(210, 55), new Rectangle(0, 0, Convert.ToInt32(288 * Convert.ToSingle(genLeft.Life) / 100f), 25), Color.White * Alpha, SpriteEffects.None, scale, depth - 0.045f);



                CacheManager.Draw(@"Content\Textures\Resources\Dantiao\Avatar.png", basePos + new Vector2(1000 - 15 - 15 - 150, 620 - 20 - 15 - 150), null, Color.White * Alpha, SpriteEffects.None, scale, depth - 0.04f);

                CacheManager.DrawZhsanAvatar(genRight.Person,genRight.Person.PictureIndex, 9999, "", new Rectangle(new Point(Convert.ToInt32(basePos.X + 1000 - 15 - 15 - 150), Convert.ToInt32(basePos.Y + 620 - 20 - 15 - 150)), new Point(150, 150)), Color.White * Alpha, depth - 0.035f);

                CacheManager.DrawString(null, genRight.Person.Name, basePos + new Vector2(1000 - 15 - 15 - 140, 620 - 20 - 15 - 140), Color.Red * Alpha, 0f, Vector2.Zero, scale.X * 0.8f, SpriteEffects.None, depth - 0.045f);

                CacheManager.DrawString(null, "武力：" + genRight.Force, basePos + new Vector2(1000 - 140, 620 - 218), Color.DarkRed * Alpha, 0f, Vector2.Zero, scale.X * 0.8f, SpriteEffects.None, depth - 0.036f);             

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
        public void PerformCommand(DuelCommand command,General genRight)
        {
            genRight.SayTimeTotal = 2f;
            genRight.SayTime = 2f;          
            switch (command)
            {
                case DuelCommand.Attack:
                    genRight.SayWords= $"今日就让你见识一下我{genRight.Person.Name}之武艺,吃我一击!";
                    break;

                case DuelCommand.Heavy:
                    genRight.SayWords = $"今日不施展下我之绝招,不知道我{genRight.Person.Name}的厉害!";
                    break;

                case DuelCommand.Ctritical:
                    genRight.SayWords = $"今日你我势不两立,吃我{genRight.Person.Name}的最后一招!";
                    break;

                case DuelCommand.Induce:
                    genRight.SayWords =$"稍安勿躁,听我{genRight.Person.Name}一言......";
                    break;

                case DuelCommand.Flee:
                    genRight.SayWords = $"{genLeft.Person.Name}果然名不虚传,我今日非你敌手,后会有期!";
                    break;

                case DuelCommand.Surround:
                    genRight.SayWords = $"且慢动手，{genRight.Person.Name}有要事相商......!";
                    break;
            }
        }

    }

}
