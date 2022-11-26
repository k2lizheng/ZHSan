﻿using GameObjects;
using GameObjects.Conditions;
using System;


using System.Runtime.Serialization;namespace GameObjects.Conditions.ConditionKindPack
{

    [DataContract]public class ConditionKind4560 : ConditionKind
    {
        private int number = 0;

        public override bool CheckConditionKind(Person person)
        {
            return person.Reputation - ConditionKind.markedPerson.Reputation >= number;
        }

        public override void InitializeParameter(string parameter)
        {
            try
            {
                this.number = int.Parse(parameter);
            }
            catch
            {
            }
        }
    }
}

