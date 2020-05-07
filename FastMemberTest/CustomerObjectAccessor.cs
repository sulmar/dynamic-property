using FastMember;
using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicProperty.FastMemberTest
{
    // dotnet add package FastMember
    public class CustomerObjectAccessor
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        private readonly ObjectAccessor wrapper;

        public CustomerObjectAccessor()
        {
            wrapper = ObjectAccessor.Create(this);
        }

        public object this[string propertyName]
        {
            get => wrapper[propertyName];
            set => wrapper[propertyName] = value;
        }
    }
}
