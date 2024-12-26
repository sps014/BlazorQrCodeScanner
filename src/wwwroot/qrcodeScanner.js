
window.qrScanners = {};

window.createScanner = (id) => {
    window.qrScanners[id] = new Html5Qrcode("reader");
};

window.startScanner = (hash, idOrContraintsConfig, config, qrBoxValue, typeOfQrBox, dotnetObjectReference) => {

    if (typeOfQrBox !== 0)
        config['qrbox'] = processQrBox(qrBoxValue, typeOfQrBox, dotnetObjectReference);
    
    let regionHeight = 150;
    if(config['qrbox']!==undefined && config['qrbox'].height!==undefined)
    {
        regionHeight = config['qrbox'].height;
    }

    window.qrScanners[hash]
        .start(idOrContraintsConfig, config, qrCodeSuccessCallback.bind({dotnet:dotnetObjectReference,regionHeight}))
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

window.scanFileScanner = (hash, byteArray,contentType,dotnet) => {
    // Create a Blob from the byte array
    const blob = new Blob([byteArray], { type: contentType });

    // Create a File from the Blob
    const file = new File([blob], "filename.x", { type: contentType });

    window.qrScanners[hash].scanFile(file).then(e => {
        dotnet.invokeMethodAsync("qrSuccessV1", e);
    }).catch(e => {
        dotnet.invokeMethodAsync("qrScanFailed", e);
    });
}

window.scanFileV2Scanner = (hash, byteArray, contentType, dotnet) => {
    // Create a Blob from the byte array
    const blob = new Blob([byteArray], { type: contentType });

    // Create a File from the Blob
    const file = new File([blob], "filename.x", { type: contentType });

    window.qrScanners[hash].scanFileV2(file).then(e => {
        dotnet.invokeMethodAsync("qrSuccess", e);
    }).catch(e => {
        dotnet.invokeMethodAsync("qrScanFailed", e);
    });
}

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
    if (type === 1) {
        return qrBoxValue.ratio;
    }
    //2 dimensional size
    else if (type === 2) {
        return qrBoxValue;
    }

    //contains js function name on window
    else if (type === 3) {
        return window[qrBoxValue.jSFunctionName];
    }

    return {};
}


window.setWidthHeightOfVideo = (idRoot, w, h, bgColor) => {
    const video = document.querySelector(`#${idRoot} video`);

    if (video === undefined || video==null)
        return;

    video.style.width = w;
    video.style.height = h;
    
    video.style.setProperty('background-color', bgColor);

};

function qrCodeSuccessCallback(decodedText, decodedResult) {
    getScannedImageUrl(this.dotnet,decodedText.toString(),this.regionHeight);
}
function getScannedImageUrl(dotnet,decodedText,regionHeight)
{
    let video = document.querySelector("video");
    let canvas = document.createElement("canvas");
    let width = video.videoWidth;
    let height = video.videoHeight;
    let context = canvas.getContext("2d");
    
    if(!regionHeight)
        regionHeight = 150;
    
    canvas.width = width;
    canvas.height = regionHeight;

    let startY = height / 2 - regionHeight / 2;
    
    context.drawImage(
        video,
        0,
        startY,
        width,
        regionHeight,
        0,
        0,
        width,
        regionHeight
    );


    canvas.toBlob(function (blob) {
        const href = URL.createObjectURL(blob);
        dotnet.invokeMethodAsync("qrSuccessV1", decodedText, href);
    }, "image/png");
}



window.disposeScanner = (hash) => {
    try {
        window.qrScanners[hash].stop();
    }
    catch (e){
        console.warn(e);
    }
    delete window.qrScanners[hash];
};

