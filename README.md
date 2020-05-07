

## Wprowadzenie
W jednym z projektów miałem sytuację, że edycja danych polegała na modyfikacji pojedynczego pola.
Chciałem uniknąć przesyłania całego obiektu w json, a zamiast tego przesyłać tylko nazwę i wartość pojedynczego pola:

~~~
{"FirstName":"John"}
~~~

Zainspirowany konstrukcją z języka Javascript postanowiłem zaimplementować to w C#

Czyli zamiast pisać:

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

Porównanie rozwiązań:


|     Method |      Mean |    Error |    StdDev | Rank |
|----------- |----------:|---------:|----------:|-----:|
| FastMember |  88.34 ns | 1.720 ns |  1.841 ns |    1 |
| Reflection | 250.50 ns | 5.008 ns | 11.097 ns |    2 |

## Podsumowanie

Jak widać zwyciezcą został FastMember i takie rozwiązanie polecam.
