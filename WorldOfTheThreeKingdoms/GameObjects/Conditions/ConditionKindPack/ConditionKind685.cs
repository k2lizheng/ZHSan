﻿using GameObjects;
using GameObjects.Conditions;
using GameObjects.PersonDetail;
using System;


using System.Runtime.Serialization;
namespace GameObjects.Conditions.ConditionKindPack
{

    [DataContract]
    public class ConditionKind685 : ConditionKind
    {
        private int number = 0;

        public override bool CheckConditionKind(Person person)
        {
            foreach (Title t in person.RealTitles)
            {
                if (t.Kind.ID == this.number)
                {
                    return false ;

                }
            }
            return true;
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
