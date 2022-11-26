﻿using GameObjects;
using GameObjects.Conditions;
using System;


using System.Runtime.Serialization;namespace GameObjects.Conditions.ConditionKindPack
{

    [DataContract]public class ConditionKind4921 : ConditionKind
    {
        
        public override bool CheckConditionKind(Person person)
        {
            return !person.Hates(ConditionKind.markedPerson);
        }

    }
}

