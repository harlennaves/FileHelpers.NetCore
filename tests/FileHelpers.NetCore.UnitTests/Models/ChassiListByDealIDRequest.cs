using System;

using FileHelpers.Events;

namespace FileHelpers.NetCore.UnitTests.Models
{
    [FixedLengthRecord(FixedMode.ExactLength)]
    public class ChassiListByDealIDRequest : MainframeMessageRequest
    {
        [FieldHidden]
        private const int ARRAY_MAX_SIZE = 500;

        [FieldFixedLength(3)]
        public string Function;

        [FieldFixedLength(4)]
        [FieldAlign(AlignMode.Right, '0')]
        public int ArraySize;

        [FieldFixedLength(10)]
        [FieldArrayLength(500)]
        [FieldConverter(typeof(ArrayDataConverter<DealID>))]
        public DealID[] ArrayData;

        public ChassiListByDealIDRequest()
        {
            MessageIdentificator = "AB_PQS_STATUS_REQUEST";
            Function = "001";

            ArraySize = 0;

            ArrayData = new DealID[ARRAY_MAX_SIZE];
            for (int i = 0; i < ArrayData.Length; i++)
            {
                ArrayData[i] = new DealID();
            }
        }

        public static void BeforeEvent(EngineBase engine, BeforeWriteEventArgs<ChassiListByDealIDRequest> e)
        {
            e.Record.ArraySize = e.Record.ArrayData.Length;
            Array.Resize(ref e.Record.ArrayData, ARRAY_MAX_SIZE);
        }

        [IgnoreEmptyLines()]
        [FixedLengthRecord(FixedMode.ExactLength)]
        public class DealID
        {
            [FieldNullValue("")]
            [FieldFixedLength(10)]
            [FieldAlign(AlignMode.Right, '0')]
            public string DealId;
        }
    }
}
