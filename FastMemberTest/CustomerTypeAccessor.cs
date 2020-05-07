using FastMember;

namespace DynamicProperty.FastMemberTest
{
    public class CustomerTypeAccessor
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        private readonly TypeAccessor accessor;

        public CustomerTypeAccessor()
        {
            accessor = TypeAccessor.Create(GetType());
        }

        public object this[string propertyName]
        {
            get => accessor[this, propertyName];
            set => accessor[this, propertyName] = value;
        }
    }
}
