﻿namespace LAB14_TINOCO_DAEA.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LasttName { get; set; }
        public string DocumentNumber { get; set; }
        public string Email { get; set; } = string.Empty;
        public bool Active { get; set; } = true;
    }
}
