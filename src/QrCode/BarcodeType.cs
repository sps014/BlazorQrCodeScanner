using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorQrCodeScanner;

public enum BarcodeType
{
    AZTEC,
    CODABAR,
    CODE_128,
    CODE_39,
    CODE_93,
    DATA_MATRIX,
    EAN_13,
    EAN_8,
    ITF,
    MAXICODE,
    PDF_417,
    QR_CODE,
    RSS_14,
    RSS_EXPANDED,
    UPC_A,
    UPC_E,
    UPC_EAN_EXTENSION
}