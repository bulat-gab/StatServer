using System;
using Contracts;
using FluentAssertions;
using NUnit.Framework;
using StatServerCore.Extensions;

namespace StatServerTests
{
    public class ExtentionsTests
    {
        [Test]
        public void ServerStats_IsEmpty_ShouldReturnTrue_ForNewlyCreatedStats()
        {
            var stats = ServerStats.CreateEmpty();
            stats.IsEmpty().Should().BeTrue();
        }
        
        [Test]
        public void ServerStats_IsEmpty_ShouldReturnFalse_WhenStatsNotEmpty()
        {
            var stats = ServerStats.CreateEmpty();
            stats.MaximumPopulation = 1;
            stats.IsEmpty().Should().BeFalse();
        }

        [Test]
        public void DateTime_CompareWith_ShouldReturnTrue_ForSameDates()
        {
            var t1 = new DateTime(2018, 10, 10, 15, 15, 15);
            var t2 = new DateTime(2018, 10, 10, 15, 15, 15);

            t1.CompareWith(t2).Should().BeTrue();
            t2.CompareWith(t1).Should().BeTrue();
        }
        
        [Test]
        public void DateTime_CompareWith_ShouldReturnFalse_WhenDifferentSeconds()
        {
            var t1 = new DateTime(2018, 10, 10, 15, 15, 59);
            var t2 = new DateTime(2018, 10, 10, 15, 15, 15);

            t1.CompareWith(t2).Should().BeFalse();
            t2.CompareWith(t1).Should().BeFalse();
        }
    }
}