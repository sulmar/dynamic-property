using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicProperty.PerformanceTests
{
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class ReflectionVsFastMemberBenchmarks
    {

        [Benchmark]
        public void Reflection()
        {
            ReflectionTest.Customer customer = new ReflectionTest.Customer();
            customer["FirstName"] = "John";
        }

        [Benchmark]
        public void FastMember()
        {
            FastMemberTest.Customer customer = new FastMemberTest.Customer();
            customer["FirstName"] = "John";
        }
    }
}
