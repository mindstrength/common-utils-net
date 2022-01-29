using System;

namespace CommonUtils.Temporal
{
    /// <summary>
    /// A temporal value representing a date, time, offset from UTC, and an optional time zone.
    /// </summary>
    public struct Temporal
    {
        /// <value>
        /// The <c>DateTime</c> as <c>DateTimeKind.Unspecified</c>.
        /// </value>
        public DateTime UnspecifiedDateTime { get; private set; }

        /// <value>
        /// The offset from UTC to use with the <c>UnspecifiedDateTime</c>.
        /// </value>
        public TimeSpan OffsetFromUtc { get; private set; }

        /// <value>
        /// The time zone [optional] to use with the <c>UnspecifiedDateTime</c>.
        /// </value>
        public TimeZoneInfo? TzInfo { get; private set; }

        /// <value>
        /// The <c>UnspecifiedDateTime</c> converted to UTC.
        /// </value>
        public DateTime UtcDateTime => WithOffset.UtcDateTime;

        /// <value>
        /// The <c>UnspecifiedDateTime</c> paired with the <c>OffsetFromUtc</c>.
        /// </value>
        public DateTimeOffset WithOffset => new DateTimeOffset(UnspecifiedDateTime, OffsetFromUtc);

        /// <summary>
        /// Constructs a temporal object with a <c>dateTime</c>.
        /// </summary>
        /// <param name="dateTime">The date and time to use.</param>
        /// <remarks>
        /// If the <c>dateTime</c> kind is <c>DateTimeKind.Local</c>, sets the <c>TzInfo</c> to
        /// <c>TimeZoneInfo.Local</c>; otherwise, it defaults to <c>DateTimeKind.Utc</c>.
        /// To specify the time zone, use a constructor with a <c>TimeZoneInfo</c> parameter.
        /// </remarks>
        public Temporal(DateTime dateTime)
        {
            TzInfo = dateTime.Kind == DateTimeKind.Local ? TimeZoneInfo.Local : TimeZoneInfo.Utc;
            UnspecifiedDateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
            OffsetFromUtc = TzInfo.GetUtcOffset(UnspecifiedDateTime);
        }

        /// <summary>
        /// Constructs a temporal object with a <c>dateTime</c> and <c>timeZoneInfo</c>.
        /// </summary>
        /// <param name="dateTime">The date and time to use.</param>
        /// <param name="timeZoneInfo">The time zone to use.</param>
        /// <remarks>
        /// If the <c>dateTime</c> kind is <c>DateTimeKind.Unspecified</c>, <c>UnspecifiedDateTime</c> is
        /// set to it; otherwise, the <c>dateTime</c> is first converted to the date and time in the given
        /// <c>timeZoneInfo</c>, and then that value is set as the <c>UnspecifiedDateTime</c>.
        /// </remarks>
        public Temporal(DateTime dateTime, TimeZoneInfo timeZoneInfo)
        {
            TzInfo = timeZoneInfo;
            UnspecifiedDateTime = dateTime.Kind == DateTimeKind.Unspecified ?
                dateTime :
                DateTime.SpecifyKind(TimeZoneInfo.ConvertTime(dateTime, TzInfo), DateTimeKind.Unspecified);
            OffsetFromUtc = TzInfo.GetUtcOffset(UnspecifiedDateTime);
        }

        /// <summary>
        /// Constructs a temporal object with a <c>dateTime</c> and <c>offsetFromUtc</c>.
        /// </summary>
        /// <param name="dateTime">The date and time to use.</param>
        /// <param name="offsetFromUtc">The offset from UTC to use.</param>
        /// <remarks>
        /// The constraints of the <c>DateTimeOffset(DateTime, TimeSpan)</c> constructor apply.
        /// </remarks>
        public Temporal(DateTime dateTime, TimeSpan offsetFromUtc)
        {
            OffsetFromUtc = new DateTimeOffset(dateTime, offsetFromUtc).Offset;
            UnspecifiedDateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
            TzInfo = default;
        }
    }
}
