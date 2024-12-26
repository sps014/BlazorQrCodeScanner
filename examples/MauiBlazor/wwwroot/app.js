window.calculateBoundingBox= function (viewfinderWidth, viewfinderHeight) {
    let minEdgePercentage = 0.999; 
    let minEdgeSize = Math.min(viewfinderWidth, viewfinderHeight);
    let qrboxSize = Math.floor(minEdgeSize * minEdgePercentage);
    return {
        width: qrboxSize,
        height: 150,
    };
};