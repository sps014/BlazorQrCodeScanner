
window.qrScanners = {};

window.createScanner=(id)=>
{
    window.qrScanners[id] = new Html5Qrcode("reader");
}

window.startScanner=(hash, idOrContraintsConfig,config,qrBoxValue,typeOfQrBox,dotnetObjectReference)=>
{
    config = cleanConfig(config);

    if (typeOfQrBox!=0)
        config['qrbox'] = processQrBox(qrBoxValue, typeOfQrBox, dotnetObjectReference);

    window.qrScanners[hash]
        .start(idOrContraintsConfig, config, qrCodeSuccessCallback.bind(dotnetObjectReference))
        .then(() => {
            dotnetObjectReference.invokeMethodAsync("qrStarted");
        })
        .catch((e)=>
        {
            dotnetObjectReference.invokeMethodAsync("qrStartFailed",e);
        });
}

function processQrBox(qrBoxValue, type, dotnet) {
    console.log(qrBoxValue);

    if (type == 1) {
        return qrBoxValue.ratio;
    }
    //2 dimensional size
    else if (type == 2) {
        return qrBoxValue;
    }

    else if (type == 3) {
        return qrBoxFunction.bind(dotnet);
    }
}

async function qrBoxFunction(viewportWidth,viewportHeight)
{
    let result = await this.invokeMethodAsync("qrBoxFunc", viewportWidth, viewportHeight);
    return result;
}


function cleanConfig(config) {
    let newConfig = {};

    for (let p in config) {
        if (config[p] == undefined || config[p] == null || config[p]=='qrbox') {
            continue;
        }
        newConfig[p] = config[p];
    }


    return newConfig;
}

function qrCodeSuccessCallback(decodedText, decodedResult) {
    dotnet.invokeMethodAsync("qrSuccess", decodedText);
}

window.disposeScanner=(hash) =>{
    window.qrScanners[hash].stop();
    delete window.qrScanners[hash];
}

