const WebSocket = require("ws");

const wss = new WebSocket.Server({ port: 8080 });

wss.on("connection", (ws, request) => {
    console.log("New client connected");

    ws.isFirstMessage = true;

    ws.on("message", data => {
        if (ws.isFirstMessage) {
            ws.clientId = data;  // Le client envoie sont ID au premier message
            ws.isFirstMessage = false;
        }

        console.log(`Client ${ws.clientId} has sent us: ${data}`);
        ws.send(`Client ${ws.clientId}. Voici la réponse`);
    });

    ws.on("close", () => {
        console.log("Client disconnected");
    });

});
