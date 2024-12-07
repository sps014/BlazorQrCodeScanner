
window.qrScanners = {};

window.createScanner = (id) => {
    window.qrScanners[id] = new Html5Qrcode("reader");
};

window.startScanner = (hash, idOrContraintsConfig, config, qrBoxValue, typeOfQrBox, dotnetObjectReference) => {

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

window.applyVideoConstraintsScanner = async (hash,constraints) => {
    await window.qrScanners[hash].applyVideoConstraints(constraints);
};

window.clearScanner =  (hash) => {
    window.qrScanners[hash].clear();
};

window.getStateScanner = (hash) => {
    return window.qrScanners[hash].getState();
};

window.pauseScanner = (hash, pauseVideo) => {
    return window.qrScanners[hash].pause(pauseVideo);
};

window.resumeScanner = (hash) => {
    return window.qrScanners[hash].resume();
};

window.stopScanner = (hash) => {
    return window.qrScanners[hash].stop();
};

window.getCamerasScanner = () => {
    return Html5Qrcode.getCameras();
};

window.getRunningTrackSettingsScanner = (hash) => {
    return window.qrScanners[hash].getRunningTrackSettings();
};

window.getRunningTrackCapabilitiesScanner=(hash)=>
{
    let r = window.qrScanners[hash].getRunningTrackCapabilities();
    console.log(r);
    return r;
}

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

