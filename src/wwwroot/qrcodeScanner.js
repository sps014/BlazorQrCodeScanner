
window.qrScanners = {};
export function showPrompt(message) {
  return prompt(message, 'Type anything here');
}

export function createScanner(id)
{
    window.qrScanners[id] = new Html5Qrcode("reader");
}

export function start(hash, idOrCOntraintsConfig,config,typeOfQrBox,dotnetObjectReference)
{

    window.qrScanners[hash]
        .start(idOrCOntraintsConfig, config, qrCodeSuccessCallback.bind(dotnetObjectReference))
        .then(() => {
            dotnetObjectReference.invokeMethodAsync("qrStarted");
        })
        .catch((e)=>
        {
            dotnetObjectReference.invokeMethodAsync("qrStartFailed",e);
        });
}

function qrCodeSuccessCallback(decodedText, decodedResult) {
    dotnet.invokeMethodAsync("qrSuccess", decodedText);
}

export function dispose(hash) {
    window.qrScanners[hash].stop();
    delete window.qrScanners[hash];
}

