

## Wprowadzenie

W jednym z projektów postanowiłem ustawiać wartości w sposób dynamiczny, czyli po nazwie właściwości w Runtimie.

Zainspirowany konstrukcją z języka Javascript postanowiłem zaimplementować to w C#

Czyli zamiast:

~~~ csharp
customer.FirstName = "John";

Console.WriteLine(customer.FirstName);
~~~

chciałem uzyskać coś takiego:

~~~ csharp

customer["FirstName"] = "John";

Console.WriteLine(customer["FirstName"]);

~~~

## Rozwiązanie z użyciem System.Reflection

Utworzyłem własny indekser i użyłem refleksji.

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


## Rozwiązanie z użyciem FastMember

Utworzyłem własny indekser i użyłem biblioteki **FastMember**.

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

Porównanie rozwiązań:


|                   Method |      Mean |    Error |   StdDev | Rank |
|------------------------- |----------:|---------:|---------:|-----:|
|   FastMemberTypeAccessor |  62.96 ns | 0.927 ns | 0.822 ns |    1 |
| FastMemberObjectAccessor |  82.74 ns | 1.141 ns | 1.068 ns |    2 |
|               Reflection | 243.04 ns | 4.884 ns | 8.294 ns |    3 |

## Podsumowanie

Jak widać zwyciezcą został FastMember, zwłaszcza metoda FastMemberTypeAccessor i takie rozwiązanie polecam.
