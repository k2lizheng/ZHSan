﻿using GameObjects;
using GameObjects.Influences;
using System;


using System.Runtime.Serialization;namespace GameObjects.Influences.InfluenceKindPack
{

    [DataContract]public class InfluenceKind50 : InfluenceKind
    {
        private float rate = 1f;

        public override void ApplyInfluenceKind(Architecture person)
        {
            person.DayRateIncrementOfInternal += this.rate;
        }

        public override void PurifyInfluenceKind(Architecture person)
        {
            person.DayRateIncrementOfInternal -= this.rate;
        }

        public override void InitializeParameter(string parameter)
        {
            try
            {
                this.rate = float.Parse(parameter);
            }
            catch
            {
            }
        }
    }
}

