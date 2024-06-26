﻿using System.Collections.Generic;

namespace JoshMkhariPROG7312Game.Logic.FindCallNumbers
{
    public class DeweyObject
    {
        public int _number { get; }
        public string _description { get; }
        public List<DeweyObject> _leaves { get; set; }

        public DeweyObject(int number, string description)
        {
            _number = number;
            _description = description;
            _leaves = new List<DeweyObject>();
        }
        
    }
}