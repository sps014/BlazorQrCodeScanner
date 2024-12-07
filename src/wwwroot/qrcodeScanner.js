
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
    return window.qrScanners[hash].getRunningTrackCapabilities();
}
window.getRunningTrackCameraCapabilitiesScanner = (hash) => {
    let r = window.qrScanners[hash].getRunningTrackCameraCapabilities();

    let torch  = r.torchFeature();
    let zoom  = r.zoomFeature();
   

    if (torch.isSupported() && zoom.isSupported())
    return {
        torch:
        {
            isSupported: torch.isSupported(),
            value: torch.value()
        }, zoom:
        {
            isSupported: zoom.isSupported(),
            min: zoom.min(),
            max: zoom.max(),
            step: zoom.step()
        }
        };
    else if (zoom.isSupported())
        return {
            zoom:
            {
                isSupported: zoom.isSupported(),
                min: zoom.min(),
                max: zoom.max(),
                step: zoom.step()
            }
        };
    if (torch.isSupported())
        return {
            torch:
            {
                isSupported: torch.isSupported(),
                value: torch.value()
            }
        };


    return {};
;
}

function processQrBox(qrBoxValue, type) {
    if (type == 1) {
        return qrBoxValue.ratio;
    }
    //2 dimensional size
    else if (type == 2) {
        return qrBoxValue;
    }

    //contains js function name on window
    else if (type == 3) {
        return window[qrBoxValue.jSFunctionName];
    }

    return {};
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

