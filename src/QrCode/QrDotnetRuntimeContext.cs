using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
    public EventHandler<string?>? OnScannerStartFailed;

    public EventHandler<QrCodeScanResult>? OnQrSuccess;
    public EventHandler<string?>? OnQrScanFailed;
    
    [DynamicDependency(nameof(QrSuccess))]
    [DynamicDependency(nameof(QrSuccessV1))]
    [DynamicDependency(nameof(QrScanFailed))]
    [DynamicDependency(nameof(QrStartFailed))]
    [DynamicDependency(nameof(QrStarted))]
    public QrDotnetRuntimeContext()
    {
        QrDotNetObjectReference = DotNetObjectReference.Create(this);
    }

    public void Dispose()
    {
        QrDotNetObjectReference.Dispose();
    }

    [JSInvokable("qrSuccessV1")]
    public void QrSuccessV1(string scannedResult,string imageUrl)
    {
        OnQrSuccess?.Invoke(this, new QrCodeScanResult()
        {
            DecodedText = scannedResult,
            ImageUrl = imageUrl
        });
    }
    [JSInvokable("qrSuccess")]
    public void QrSuccess(QrCodeScanResult scannedResult)
    {
        OnQrSuccess?.Invoke(this, scannedResult);
    }

    [JSInvokable("qrScanFailed")]
    public void QrScanFailed(string? message)
    {
        OnQrScanFailed?.Invoke(this, message);
    }

    [JSInvokable("qrStartFailed")]
    public void QrStartFailed(string? e)
    {
        OnScannerStartFailed?.Invoke(this, e);
    }

    [JSInvokable("qrStarted")]
    public void QrStarted()
    {
        OnScannerStarted?.Invoke(this,EventArgs.Empty);
    }
}
