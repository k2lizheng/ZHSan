﻿using GameObjects;
using GameObjects.Influences;
using System;


using System.Runtime.Serialization;namespace GameObjects.Influences.InfluenceKindPack
{

    [DataContract]public class InfluenceKind6500 : InfluenceKind
    {
        private int increment;

        public override void ApplyInfluenceKind(Troop t)
        {
            t.stealTreasureRate += this.increment;
        }

        public override void PurifyInfluenceKind(Troop t)
        {
            t.stealTreasureRate -= this.increment;
        }

        public override void InitializeParameter(string parameter)
        {
            try
            {
                this.increment = int.Parse(parameter);
            }
            catch
            {
            }
        }
    }
}

