using BenchmarkDotNet.Running;
using DynamicProperty.PerformanceTests;
using System;

namespace DynamicProperty
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<ReflectionVsFastMemberBenchmarks>();
        }
    }
}
