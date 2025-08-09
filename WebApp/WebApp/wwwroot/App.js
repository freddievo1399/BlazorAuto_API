window.SaveAs = (fileName, base64Data) => {
    const link = document.createElement('a');
    link.download = fileName;
    link.href = "data:application/octet-stream;base64," + base64Data;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};

window.callApi = async function (apiUrl, method, data = null) {
    try {
        let options = {
            method: method.toUpperCase(),
            headers: {
                "Content-Type": "application/json"
            }
        };

        if ((method === "POST" || method === "PUT" || method === "PATCH") && data !== null) {
            try {
                options.body = JSON.stringify(data);
            } catch (e) {
                return {
                    Success: false,
                    Message: e.message
                };
            }
        }

        const response = await fetch(apiUrl, options);

        const result = await response.json();
        return result;

    } catch (e) {
        return {
            Success: false,
            Message: e.message
        };
    }
};