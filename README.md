

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

Por�wnanie rozwi�za�:


|     Method |      Mean |    Error |    StdDev | Rank |
|----------- |----------:|---------:|----------:|-----:|
| FastMember |  88.34 ns | 1.720 ns |  1.841 ns |    1 |
| Reflection | 250.50 ns | 5.008 ns | 11.097 ns |    2 |

## Podsumowanie

Jak wida� zwyciezc� zosta� FastMember i takie rozwi�zanie polecam.