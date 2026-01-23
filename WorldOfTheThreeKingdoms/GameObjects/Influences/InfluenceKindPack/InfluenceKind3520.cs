using GameObjects;
using GameObjects.Influences;
using System;


using System.Runtime.Serialization;namespace GameObjects.Influences.InfluenceKindPack
{

    [DataContract]public class InfluenceKind3520 : InfluenceKind
    {
        private int cost;
        
        public override void ApplyInfluenceKind(Architecture architecture)
        {
           
        }

        public override void InitializeParameter(string parameter)
        {
            try
            {
                this.cost = int.Parse(parameter);
            }
            catch
            {
            }
        }

        public override void PurifyInfluenceKind(Architecture architecture)
        {
            
        }

        public override double AIFacilityValue(Architecture a)
        {
            return a.IsFundAbundant && a.IsFoodAbundant ? a.PersonCount : 0;
        }
    }
}

