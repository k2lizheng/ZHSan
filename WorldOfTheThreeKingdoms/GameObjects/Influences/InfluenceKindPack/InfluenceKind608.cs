﻿using GameObjects;
using GameObjects.Influences;
using System;


using System.Runtime.Serialization;namespace GameObjects.Influences.InfluenceKindPack
{

    [DataContract]public class InfluenceKind608 : InfluenceKind
    {
        private float rate = 1f;


        public override void ApplyInfluenceKind(Troop troop)
        {
            troop.MoraleChangeRateOnOutOfFood = this.rate;
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

        public override void PurifyInfluenceKind(Troop troop)
        {
            if (troop != null)
            {
                troop.MoraleChangeRateOnOutOfFood = 1f;
            }
        }
    }
}

