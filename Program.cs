using BenchmarkDotNet.Running;
using DynamicProperty.PerformanceTests;
using System;

namespace DynamicProperty
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("dotnet run -c Release");
            var summary = BenchmarkRunner.Run<ReflectionVsFastMemberBenchmarks>();
        }
    }
}
