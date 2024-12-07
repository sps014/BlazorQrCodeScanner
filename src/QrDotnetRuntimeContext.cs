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
    internal Func<SizeF, SizeF>? QrBoxFunction;

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
    [JSInvokable("qrBoxFunc")]
    public SizeF QrBoxFunc(double width,double height)
    {
        if (QrBoxFunction == null)
            return SizeF.Empty;

        return QrBoxFunction.Invoke(new SizeF((float)width, (float)height));
    }


}
