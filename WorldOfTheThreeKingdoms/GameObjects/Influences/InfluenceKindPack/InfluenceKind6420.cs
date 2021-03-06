﻿using GameObjects;
using GameObjects.Influences;
using System;


using System.Runtime.Serialization;namespace GameObjects.Influences.InfluenceKindPack
{

    [DataContract]public class InfluenceKind6420 : InfluenceKind
    {
        private int increment;
        private int disasterId;

        public override void ApplyInfluenceKind(Architecture a)
        {
            if (a.disasterChanceDecrease.ContainsKey(disasterId))
            {
                a.disasterChanceDecrease[disasterId] += this.increment;
            }
            else
            {
                a.disasterChanceDecrease[disasterId] = this.increment;
            }
        }


        public override void PurifyInfluenceKind(Architecture a)
        {
            if (a.disasterChanceDecrease.ContainsKey(disasterId))
            {
                a.disasterChanceDecrease[disasterId] -= this.increment;
            }
        }

        public override void InitializeParameter(string parameter)
        {
            try
            {
                this.disasterId = int.Parse(parameter);
            }
            catch
            {
            }
        }

        public override void InitializeParameter2(string parameter)
        {
            try
            {
                this.increment = int.Parse(parameter);
            }
            catch
            {
            }
        }

        public override double AIFacilityValue(Architecture a)
        {
            return this.increment / 10;
        }
    }
}

