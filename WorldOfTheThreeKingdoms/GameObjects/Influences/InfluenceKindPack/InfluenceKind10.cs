﻿using GameObjects;
using GameObjects.Influences;
using System;


using System.Runtime.Serialization;namespace GameObjects.Influences.InfluenceKindPack
{

    [DataContract]public class InfluenceKind10 : InfluenceKind
    {
        private float rate;

        public override void ApplyInfluenceKind(Person person)
        {
            person.RateIncrementOfAgricultureAbility += this.rate;
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

        public override void PurifyInfluenceKind(Person person)
        {
            person.RateIncrementOfAgricultureAbility -= this.rate;
        }
    }
}

