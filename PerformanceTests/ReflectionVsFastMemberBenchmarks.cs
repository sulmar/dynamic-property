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
        public void FastMemberObjectAccessor()
        {
            FastMemberTest.CustomerObjectAccessor customer = new FastMemberTest.CustomerObjectAccessor();
            customer["FirstName"] = "John";
        }

        [Benchmark]
        public void FastMemberTypeAccessor()
        {
            FastMemberTest.CustomerTypeAccessor customer = new FastMemberTest.CustomerTypeAccessor();
            customer["FirstName"] = "John";
        }
    }
}
