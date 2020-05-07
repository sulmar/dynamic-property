using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicProperty.ReflectionTest
{
    public class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public object this[string propertyName]
        {
            get => GetType().GetProperty(propertyName).GetValue(this);
            set => GetType().GetProperty(propertyName).SetValue(this, value, null);
        }
    }

}
