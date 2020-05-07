

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


## Benchmarks

Porównanie rozwi¹zañ:


|     Method |      Mean |    Error |    StdDev | Rank |
|----------- |----------:|---------:|----------:|-----:|
| FastMember |  88.34 ns | 1.720 ns |  1.841 ns |    1 |
| Reflection | 250.50 ns | 5.008 ns | 11.097 ns |    2 |

## Podsumowanie

Jak widaæ zwyciezc¹ zosta³ FastMember i takie rozwi¹zanie polecam.