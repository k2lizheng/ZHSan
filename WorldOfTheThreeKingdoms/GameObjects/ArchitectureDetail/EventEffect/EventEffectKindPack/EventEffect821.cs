﻿using GameObjects;
using System;


using System.Runtime.Serialization;namespace GameObjects.ArchitectureDetail.EventEffect
{

    [DataContract]public class EventEffect821 : EventEffectKind
    {
        private int increment;

        public override void ApplyEffectKind(Person person, Event e)
        {
            person.Brothers.Add(EventEffectKind.markedPerson);
        }
    }
}

