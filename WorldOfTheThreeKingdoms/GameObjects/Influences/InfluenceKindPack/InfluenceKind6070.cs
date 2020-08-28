﻿using GameObjects;
using GameObjects.Influences;
using System;


using System.Runtime.Serialization;namespace GameObjects.Influences.InfluenceKindPack
{

    [DataContract]public class InfluenceKind6070 : InfluenceKind
    {
        private float rate;

        public override void ApplyInfluenceKind(Architecture architecture)
        {
            architecture.ExperienceRate += rate;
        }

        public override void PurifyInfluenceKind(Architecture architecture)
        {
            architecture.ExperienceRate -= rate;
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

        public override double AIFacilityValue(Architecture a)
        {
            return this.rate * a.PersonCount * 3;
        }
    }
}

