﻿using GameObjects;
using System;


using System.Runtime.Serialization;namespace GameObjects.ArchitectureDetail.EventEffect
{

    [DataContract]public class EventEffect450 : EventEffectKind
    {
        private int type;

        public override void ApplyEffectKind(Person person, Event e)
        {
            person.ValuationOnGovernment = (PersonValuationOnGovernment) type;
        }

        public override void InitializeParameter(string parameter)
        {
            try
            {
                this.type = int.Parse(parameter);
            }
            catch
            {
            }
        }
    }
}

