
window.qrScanners = {};

window.createScanner = (id) => {
    window.qrScanners[id] = new Html5Qrcode("reader");
};

window.startScanner = (hash, idOrContraintsConfig, config, qrBoxValue, typeOfQrBox, dotnetObjectReference) => {

    console.log(idOrContraintsConfig);
    if (typeOfQrBox != 0)
        config['qrbox'] = processQrBox(qrBoxValue, typeOfQrBox, dotnetObjectReference);

    window.qrScanners[hash]
        .start(idOrContraintsConfig, config, qrCodeSuccessCallback.bind(dotnetObjectReference))
        .then(() => {
            dotnetObjectReference.invokeMethodAsync("qrStarted");
        })
        .catch((e) => {
            dotnetObjectReference.invokeMethodAsync("qrStartFailed", e);
        });
};

function processQrBox(qrBoxValue, type, dotnet) {
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

    return undefined;
}

function qrBoxFunction(viewportWidth,viewportHeight)
{
    let result = {};
    this.invokeMethodAsync("qrBoxFunc", viewportWidth, viewportHeight).then(e => {
        result = e;
    });

    if (result.height == undefined || result.width == undefined)
        return {};

    return { height: result.height, width: result.width };
}

window.setWidthHeightOfVideo = (idRoot, w, h, bgColor) => {
    const video = document.querySelector(`#${idRoot} video`);

    if (video == undefined || video==null)
        return;

    video.style.width = w;
    video.style.height = h;

    console.log(w,h);

    video.style.setProperty('background-color', bgColor);

};

function qrCodeSuccessCallback(decodedText, decodedResult) {
    dotnet.invokeMethodAsync("qrSuccess", decodedText);
}

window.disposeScanner = (hash) => {
    window.qrScanners[hash].stop();
    delete window.qrScanners[hash];
};

