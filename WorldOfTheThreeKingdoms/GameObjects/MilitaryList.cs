using System;
using System.Runtime.Serialization;

namespace GameObjects
{
    [DataContract]
    public class MilitaryList : GameObjectList
    {
        public void AddMilitary(Military military)
        {
            Add(military);
        }
    }
}

