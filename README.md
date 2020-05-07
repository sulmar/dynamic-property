

## Wprowadzenie

W jednym z projekt�w potrzebowa�em ustawienia warto�ci w�a�ciwo�ci w spos�b dynamiczny, czyli nazwa pola by�a zmienn�.

Zainspirowany konstrukcj� z j�zyka Javascript postanowi�em zaimplementowa� to w C#

Czyli zamiast pisa�:

~~~ csharp
customer.FirstName = "John";

Console.WriteLine(customer.FirstName);
~~~

chcia�em uzyska� co� takiego:

~~~ csharp

customer["FirstName"] = "John";

Console.WriteLine(customer["FirstName"]);

~~~

## Rozwi�zanie z u�yciem System.Reflection

Utworzy�em w�asny indekser i u�y�em refleksji.

~~~ csharp
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
~~~


## Rozwi�zanie z u�yciem FastMember

Utworzy�em w�asny indekser i u�y�em biblioteki **FastMember**.

~~~
 dotnet add package FastMember
~~~


### ObjectAccessor
~~~ csharp

  public class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        private readonly ObjectAccessor wrapper;

        public Customer()
        {
            wrapper = ObjectAccessor.Create(this);
        }

        public object this[string propertyName]
        {
            get => wrapper[propertyName];
            set => wrapper[propertyName] = value;
        }
    }

~~~


### TypeAccessor

~~~ csharp

 public class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        private readonly TypeAccessor accessor;

        public Customer()
        {
            accessor = TypeAccessor.Create(GetType());
        }

        public object this[string propertyName]
        {
            get => accessor[this, propertyName];
            set => accessor[this, propertyName] = value;
        }
    }

~~~

## Benchmarks

Por�wnanie rozwi�za�:


|                   Method |      Mean |    Error |   StdDev | Rank |
|------------------------- |----------:|---------:|---------:|-----:|
|   FastMemberTypeAccessor |  62.96 ns | 0.927 ns | 0.822 ns |    1 |
| FastMemberObjectAccessor |  82.74 ns | 1.141 ns | 1.068 ns |    2 |
|               Reflection | 243.04 ns | 4.884 ns | 8.294 ns |    3 |

## Podsumowanie

Jak wida� zwyciezc� zosta� FastMember, zw�aszcza metoda FastMemberTypeAccessor i takie rozwi�zanie polecam.