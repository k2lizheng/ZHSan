﻿using GameObjects;
using GameObjects.Influences;
using System;


using System.Runtime.Serialization;namespace GameObjects.Influences.InfluenceKindPack
{

    [DataContract]public class InfluenceKind160 : InfluenceKind
    {
        private int increment = 0;

        public override void ApplyInfluenceKind(Person person)
        {
            person.MonthIncrementOfFactionReputation += this.increment;
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

        public override void PurifyInfluenceKind(Person person)
        {
            person.MonthIncrementOfFactionReputation -= this.increment;
        }
    }
}

