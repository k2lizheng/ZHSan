﻿using GameObjects;
using GameObjects.Influences;
using System;


using System.Runtime.Serialization;namespace GameObjects.Influences.InfluenceKindPack
{

    [DataContract]public class InfluenceKind6780 : InfluenceKind
    {
        private int rate;

        public override void ApplyInfluenceKind(Troop troop)
        {
            troop.IntelligenceExperienceIncrease += rate;
        }

        public override void PurifyInfluenceKind(Troop troop)
        {
            troop.IntelligenceExperienceIncrease -= rate;
        }

        public override void InitializeParameter(string parameter)
        {
            try
            {
                this.rate = int.Parse(parameter);
            }
            catch
            {
            }
        }
    }
}

