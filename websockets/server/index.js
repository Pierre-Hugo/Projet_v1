const WebSocket = require("ws");

const wss = new WebSocket.Server({ port: 8080 });

wss.on("connection", (ws, request) => {
    console.log("New client connected");

    ws.isFirstMessage = true;

    ws.on("message", data => {
        const messageStr = data.toString();

        if (ws.isFirstMessage) {
            ws.clientId = messageStr;  // Le client envoie son ID au premier message
            console.log(`Client ${ws.clientId} sent us: ${data}`);
            ws.isFirstMessage = false;
            return;
        }

        const [targetClientId, messageContent] = messageStr.split(':', 2);

        console.log(`${ws.clientId} sends to ${targetClientId}: ${messageContent}`);

        // Trouver le client et envoyer le message (data)
        wss.clients.forEach(client => {
            if (client.clientId === targetClientId && client.readyState === WebSocket.OPEN) {
                client.send(`${ws.clientId}:${messageContent}`);
            }
        });
    });

    ws.on("close", () => {
        console.log("Client disconnected");
    });

});
