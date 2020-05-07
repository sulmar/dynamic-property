

## Wprowadzenie

W jednym z projektów potrzebowa³em ustawienia wartoœci w³aœciwoœci w sposób dynamiczny, czyli nazwa pola by³a zmienn¹.

Zainspirowany konstrukcj¹ z jêzyka Javascript postanowi³em zaimplementowaæ to w C#

Czyli zamiast pisaæ:

~~~ csharp
customer.FirstName = "John";

Console.WriteLine(customer.FirstName);
~~~

chcia³em uzyskaæ coœ takiego:

~~~ csharp

customer["FirstName"] = "John";

Console.WriteLine(customer["FirstName"]);

~~~

## Rozwi¹zanie z u¿yciem System.Reflection

Utworzy³em w³asny indekser i u¿y³em refleksji.

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


## Rozwi¹zanie z u¿yciem FastMember

Utworzy³em w³asny indekser i u¿y³em biblioteki **FastMember**.

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

Porównanie rozwi¹zañ:


|                   Method |      Mean |    Error |   StdDev | Rank |
|------------------------- |----------:|---------:|---------:|-----:|
|   FastMemberTypeAccessor |  62.96 ns | 0.927 ns | 0.822 ns |    1 |
| FastMemberObjectAccessor |  82.74 ns | 1.141 ns | 1.068 ns |    2 |
|               Reflection | 243.04 ns | 4.884 ns | 8.294 ns |    3 |

## Podsumowanie

Jak widaæ zwyciezc¹ zosta³ FastMember, zw³aszcza metoda FastMemberTypeAccessor i takie rozwi¹zanie polecam.