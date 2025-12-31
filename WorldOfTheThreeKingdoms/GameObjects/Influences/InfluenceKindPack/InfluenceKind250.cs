using GameObjects;
using GameObjects.Influences;
using System;


using System.Runtime.Serialization;namespace GameObjects.Influences.InfluenceKindPack
{

    [DataContract]public class InfluenceKind250 : InfluenceKind
    {
        private float rate = 1f;

        public override void ApplyInfluenceKind(Troop troop)
        {
            if (troop != null) // && troop.RateOfMovability < this.rate) //数据里10%是1.1
            {
                //troop.RateOfMovability = this.rate;
                troop.RateOfMovability += this.rate - 1;
            }
            
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
                troop.RateOfMovability -= (this.rate - 1);
            }
        }
    }
}

