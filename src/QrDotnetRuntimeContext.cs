using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace BlazorQrCodeScanner;

internal class QrDotnetRuntimeContext:IDisposable
{
    internal DotNetObjectReference<QrDotnetRuntimeContext> QrDotNetObjectReference;

    public EventHandler? OnScannerStarted;
    public QrDotnetRuntimeContext()
    {
        QrDotNetObjectReference = DotNetObjectReference.Create(this);
    }

    public void Dispose()
    {
        QrDotNetObjectReference.Dispose();
    }

    [JSInvokable("qrSuccess")]
    public void QrSuccess(string scannedText)
    {

    }

    [JSInvokable("qrStartFailed")]
    public void QrStartFailed(string e)
    {

    }

    [JSInvokable("qrStarted")]
    public void QrStarted()
    {
        OnScannerStarted?.Invoke(this,EventArgs.Empty);
    }
}
