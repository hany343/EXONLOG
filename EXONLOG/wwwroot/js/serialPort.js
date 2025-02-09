export async function initializeScale(dotNetRef, portName, baudRate) {
    try {
        // Check if Web Serial API is supported
        if (!('serial' in navigator)) {
            throw new Error('Web Serial API not supported in this browser.');
        }

        // Request access to the serial port
        const port = await navigator.serial.requestPort();
        await port.open({ baudRate: baudRate });

        // Start reading data from the serial port
        readScaleData(port, dotNetRef);
    } catch (err) {
        console.error('Serial port error:', err);
        dotNetRef.invokeMethodAsync('HandleSerialError', err.message);
    }
}

async function readScaleData(port, dotNetRef) {
    const decoder = new TextDecoder();
    let reader;

    try {
        while (port.readable) {
            reader = port.readable.getReader();
            while (true) {
                const { value, done } = await reader.read();
                if (done) break;

                // Decode and parse the weight data
                const text = decoder.decode(value);
                const weight = parseWeight(text);

                // Send the weight data to Blazor
                dotNetRef.invokeMethodAsync('UpdateWeight', weight);
            }
        }
    } catch (err) {
        console.error('Error reading from serial port:', err);
        dotNetRef.invokeMethodAsync('HandleSerialError', err.message);
    } finally {
        if (reader) {
            reader.releaseLock();
        }
    }
}

function parseWeight(data) {
    // Implement scale-specific protocol parsing
    // Example: Extract weight from "Weight: 123.45 kg"
    const match = data.match(/Weight:\s*([\d.]+)\s*kg/);
    return match ? parseFloat(match[1]) : 0;
}

export async function resetScale(port) {
    if (port && port.readable) {
        const reader = port.readable.getReader();
        await reader.cancel();
        reader.releaseLock();
    }
}