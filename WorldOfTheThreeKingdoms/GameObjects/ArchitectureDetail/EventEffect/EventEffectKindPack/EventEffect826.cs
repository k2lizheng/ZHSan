﻿using GameObjects;
using System;


using System.Runtime.Serialization;namespace GameObjects.ArchitectureDetail.EventEffect
{

    [DataContract]public class EventEffect826 : EventEffectKind
    {

        public override void ApplyEffectKind(Person person, Event e)
        {
            person.RemoveClose(markedPerson);
        }
    }
}

