﻿using GameObjects;
using GameObjects.Influences;
using System;


using System.Runtime.Serialization;namespace GameObjects.Influences.InfluenceKindPack
{

    [DataContract]public class InfluenceKind6755 : InfluenceKind
    {
        private int increment;
        private int prob;

        public override void ApplyInfluenceKind(Person t)
        {
            t.ReputationIncrease.Add(new System.Collections.Generic.KeyValuePair<int, int>(prob, increment));
        }

        public override void PurifyInfluenceKind(Person t)
        {
            t.ReputationIncrease.Remove(new System.Collections.Generic.KeyValuePair<int, int>(prob, increment));
        }

        public override void InitializeParameter(string parameter)
        {
            try
            {
                this.prob = int.Parse(parameter);
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
    }
}

