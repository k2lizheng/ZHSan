﻿using GameFreeText;
using GameGlobal;
using GameManager;
using GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TroopSurveyPlugin
{

    internal class TroopSurvey
    {
        public FreeText ArmyText;
        public Point BackgroundSize;
        public PlatformTexture BackgroundTexture;
        public FreeText CombativityText;
        public FreeText CombatTitleText;
        private Point displayOffset;
        public Color FactionColor;
        public Rectangle FactionPosition;
        public FreeText FactionText;
        public PlatformTexture FactionTexture;
        public FreeText KindText;
        public InformationLevel Level;
        public FreeText MoraleText;
        public FreeText NameText;
        public Troop TroopToSurvey;
        public Faction ViewingFaction;

        public void Draw()
        {
            Rectangle? sourceRectangle = null;
            CacheManager.Draw(this.BackgroundTexture, new Rectangle(this.displayOffset.X, this.displayOffset.Y, this.BackgroundSize.X, this.BackgroundSize.Y), sourceRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.05f);
            CacheManager.Draw(this.FactionTexture, new Rectangle(this.displayOffset.X + this.FactionPosition.X, this.displayOffset.Y + this.FactionPosition.Y, this.FactionPosition.Width, this.FactionPosition.Height), null, this.FactionColor, 0f, Vector2.Zero, SpriteEffects.None, 0.049f);
            this.NameText.Draw(0.05f);
            this.KindText.Draw(0.05f);
            this.FactionText.Draw(0.05f);
            this.CombatTitleText.Draw(0.05f);
            this.ArmyText.Draw(0.05f);
            this.MoraleText.Draw(0.05f);
            this.CombativityText.Draw(0.05f);
        }

        private void ResetTextsPosition()
        {
            this.NameText.DisplayOffset = this.displayOffset;
            this.KindText.DisplayOffset = this.displayOffset;
            this.FactionText.DisplayOffset = this.displayOffset;
            this.CombatTitleText.DisplayOffset = this.displayOffset;
            this.ArmyText.DisplayOffset = this.displayOffset;
            this.MoraleText.DisplayOffset = this.displayOffset;
            this.CombativityText.DisplayOffset = this.displayOffset;
        }

        public void Update()
        {
            this.FactionColor = Color.White;
            if (this.TroopToSurvey.BelongedFaction != null)
            {
                this.FactionColor = this.TroopToSurvey.BelongedFaction.FactionColor;
            }
            if (!((this.ViewingFaction == null) || Session.GlobalVariables.SkyEye))
            {
                this.NameText.Text = this.TroopToSurvey.DisplayName;
                this.KindText.Text = this.TroopToSurvey.KindString;
                this.FactionText.Text = this.TroopToSurvey.FactionString;
                this.CombatTitleText.Text = this.TroopToSurvey.CombatTitleString;
                this.ArmyText.Text = this.TroopToSurvey.QuantityInInformationLevel(this.Level);
                this.MoraleText.Text = this.TroopToSurvey.MoraleInInformationLevel(this.Level);
                this.CombativityText.Text = this.TroopToSurvey.CombativityInInformationLevel(this.Level);
            }
            else
            {
                this.NameText.Text = this.TroopToSurvey.DisplayName;
                this.KindText.Text = this.TroopToSurvey.KindString;
                this.FactionText.Text = this.TroopToSurvey.FactionString;
                this.CombatTitleText.Text = this.TroopToSurvey.CombatTitleString;
                this.ArmyText.Text = this.TroopToSurvey.Quantity.ToString();
                this.MoraleText.Text = this.TroopToSurvey.Morale.ToString();
                this.CombativityText.Text = this.TroopToSurvey.Combativity.ToString();
            }
        }

        public Point DisplayOffset
        {
            get
            {
                return this.displayOffset;
            }
            set
            {
                this.displayOffset = value;
                this.ResetTextsPosition();
            }
        }
    }
}

