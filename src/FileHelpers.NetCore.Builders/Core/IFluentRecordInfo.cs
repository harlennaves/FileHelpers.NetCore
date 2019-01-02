using System.Text.RegularExpressions;

namespace FileHelpers.NetCore.Builders.Core
{
    /// <summary>
    /// Fluet record information,  whether it is delimited or other details
    /// </summary>
    public interface IFluentRecordInfo
    {
        /// <summary>
        /// Is the input delimited or fixed length
        /// </summary>
        bool IsDelimited { get; }

        /// <summary>
        /// Number of fields defined
        /// </summary>
        int FieldCount { get; }

        /// <summary>
        /// Number of records to skip before starting processing (header records)
        /// </summary>
        int IgnoreFirst { get; set; }

        /// <summary>
        /// Number of records to skip when processing end of file (Trailer records)
        /// </summary>
        int IgnoreLast { get; set; }

        /// <summary>
        /// Whether the Notify read event is set or something else needs to be notified of change
        /// </summary>
        bool NotifyRead { get; }

        /// <summary>
        /// Whether the notify write event is hooked or something else needs to be notified on write
        /// </summary>
        bool NotifyWrite { get; }

        /// <summary>
        /// Buffer beginning size hint
        /// </summary>
        int SizeHint { get; }

        /// <summary>
        /// Do we skip empty lines?
        /// </summary>
        bool IgnoreEmptyLines { get; set; }

        /// <summary>
        /// Do we skip lines that are visually empty,  spaces only
        /// </summary>
        bool IgnoreEmptySpaces { get; }

        /// <summary>
        /// String that prefixes a comment,  eg a # in shell script
        /// </summary>
        string CommentMarker { get; set; }

        /// <summary>
        /// Can the comment marker be proceeded with spaces.
        /// </summary>
        bool CommentAnyPlace { get; set; }
        /// <summary>
        /// Selection condition when reading records, allows skipping unused data
        /// </summary>
        RecordCondition RecordCondition { get; set; }

        /// <summary>
        /// Selection of records to process by regex
        /// </summary>
        Regex RecordConditionRegEx { get; }

        /// <summary>
        /// Selection of records to process by prefix
        /// </summary>
        string RecordConditionSelector { get; set; }
    }
}
