﻿using PaschoalottoDemo.Models;

namespace PaschoalottoDemo.RandomUserGenerator
{
    public class Location
    {
        public Street Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public Coordinates Coordinates { get; set; }
        public Timezone Timezone { get; set; }
    }
}
