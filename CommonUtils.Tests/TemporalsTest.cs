using System;
using Xunit;

namespace CommonUtils.Temporal.Tests
{
    public class TemporalTest
    {
        DateTime dateTime;
        DateTime localDateTime;
        DateTime utcDateTime;
        TimeZoneInfo pstTz;
        TimeZoneInfo localTz;
        TimeZoneInfo utcTz;

        public TemporalTest()
        {
            dateTime = new DateTime(2021, 02, 10, 12, 30, 15);
            localDateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
            utcDateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            pstTz = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            localTz = TimeZoneInfo.Local;
            utcTz = TimeZoneInfo.Utc;
        }

        [Fact]
        public void Ctor_DateTime_LocalKind()
        {
            var temporal = new Temporal(localDateTime);

            Assert.Equal(localTz, temporal.TzInfo);
            Assert.Equal(localTz.GetUtcOffset(dateTime), temporal.OffsetFromUtc);
            Assert.Equal(dateTime, temporal.UnspecifiedDateTime);
            Assert.Equal(localDateTime.ToUniversalTime(), temporal.UtcDateTime);
            Assert.Equal(new DateTimeOffset(localDateTime, localTz.GetUtcOffset(dateTime)), temporal.WithOffset);
        }

        [Fact]
        public void Ctor_DateTime_UtcKind()
        {
            var temporal = new Temporal(utcDateTime);

            Assert.Equal(utcTz, temporal.TzInfo);
            Assert.Equal(TimeSpan.Zero, temporal.OffsetFromUtc);
            Assert.Equal(dateTime, temporal.UnspecifiedDateTime);
            Assert.Equal(utcDateTime, temporal.UtcDateTime);
            Assert.Equal(new DateTimeOffset(dateTime, TimeSpan.Zero), temporal.WithOffset);
        }

        [Fact]
        public void Ctor_DateTime_UnspecifiedKind()
        {
            var temporal = new Temporal(dateTime);

            Assert.Equal(utcTz, temporal.TzInfo);
            Assert.Equal(TimeSpan.Zero, temporal.OffsetFromUtc);
            Assert.Equal(dateTime, temporal.UnspecifiedDateTime);
            Assert.Equal(utcDateTime, temporal.UtcDateTime);
            Assert.Equal(new DateTimeOffset(dateTime, TimeSpan.Zero), temporal.WithOffset);
        }

        [Fact]
        public void Ctor_DateTime_TimeZoneInfo_LocalKind()
        {
            var pstDateTime = TimeZoneInfo.ConvertTime(localDateTime, pstTz);
            var temporal = new Temporal(localDateTime, pstTz);

            Assert.Equal(pstTz, temporal.TzInfo);
            Assert.Equal(pstTz.GetUtcOffset(pstDateTime), temporal.OffsetFromUtc);
            Assert.Equal(pstDateTime, temporal.UnspecifiedDateTime);
            Assert.Equal(TimeZoneInfo.ConvertTimeToUtc(pstDateTime, pstTz), temporal.UtcDateTime);
            Assert.Equal(new DateTimeOffset(pstDateTime, pstTz.GetUtcOffset(pstDateTime)), temporal.WithOffset);
        }

        [Fact]
        public void Ctor_DateTime_TimeZoneInfo_UtcKind()
        {
            var pstDateTime = TimeZoneInfo.ConvertTime(utcDateTime, pstTz);
            var temporal = new Temporal(utcDateTime, pstTz);

            Assert.Equal(pstTz, temporal.TzInfo);
            Assert.Equal(pstTz.GetUtcOffset(pstDateTime), temporal.OffsetFromUtc);
            Assert.Equal(pstDateTime, temporal.UnspecifiedDateTime);
            Assert.Equal(utcDateTime, temporal.UtcDateTime);
            Assert.Equal(new DateTimeOffset(pstDateTime, pstTz.GetUtcOffset(pstDateTime)), temporal.WithOffset);
        }

        [Fact]
        public void Ctor_DateTime_TimeZoneInfo_UnspecifiedKind()
        {
            var temporal = new Temporal(dateTime, pstTz);

            Assert.Equal(pstTz, temporal.TzInfo);
            Assert.Equal(pstTz.GetUtcOffset(dateTime), temporal.OffsetFromUtc);
            Assert.Equal(dateTime, temporal.UnspecifiedDateTime);
            Assert.Equal(TimeZoneInfo.ConvertTimeToUtc(dateTime, pstTz), temporal.UtcDateTime);
            Assert.Equal(new DateTimeOffset(dateTime, pstTz.GetUtcOffset(dateTime)), temporal.WithOffset);
        }

        [Fact]
        public void Ctor_DateTime_TimeSpan_LocalKind()
        {
            // Per DateTimeOffset's constraints: DateTime with Local kind requires Local TimeZone's Offset.
            var temporal = new Temporal(localDateTime, localTz.GetUtcOffset(dateTime));

            Assert.Null(temporal.TzInfo);
            Assert.Equal(localTz.GetUtcOffset(dateTime), temporal.OffsetFromUtc);
            Assert.Equal(dateTime, temporal.UnspecifiedDateTime);
            Assert.Equal(localDateTime.ToUniversalTime(), temporal.UtcDateTime);
            Assert.Equal(new DateTimeOffset(dateTime, localTz.GetUtcOffset(dateTime)), temporal.WithOffset);
        }

        [Fact]
        public void Ctor_DateTime_TimeSpan_UtcKind()
        {
            // Per DateTimeOffset's constraints: DateTime with Utc kind requires Utc TimeZone's Offset.
            var temporal = new Temporal(utcDateTime, TimeSpan.Zero);

            Assert.Null(temporal.TzInfo);
            Assert.Equal(TimeSpan.Zero, temporal.OffsetFromUtc);
            Assert.Equal(dateTime, temporal.UnspecifiedDateTime);
            Assert.Equal(utcDateTime, temporal.UtcDateTime);
            Assert.Equal(new DateTimeOffset(dateTime, TimeSpan.Zero), temporal.WithOffset);
        }

        [Fact]
        public void Ctor_DateTime_TimeSpan_UnspecifiedKind()
        {
            // Per DateTimeOffset's constraints: DateTime with Unspecified kind allows for any Offset within the valid range.
            var temporal = new Temporal(dateTime, pstTz.GetUtcOffset(dateTime));
            
            Assert.Null(temporal.TzInfo);
            Assert.Equal(pstTz.GetUtcOffset(dateTime), temporal.OffsetFromUtc);
            Assert.Equal(dateTime, temporal.UnspecifiedDateTime);
            Assert.Equal(TimeZoneInfo.ConvertTimeToUtc(dateTime, pstTz), temporal.UtcDateTime);
            Assert.Equal(new DateTimeOffset(dateTime, pstTz.GetUtcOffset(dateTime)), temporal.WithOffset);
        }
    }
}
