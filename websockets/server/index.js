const WebSocket = require("ws");

const wss = new WebSocket.Server({ port: 8080 });

wss.on("connection", (ws, request, client) => {
    console.log("New client connected");

    ws.on("message", data => {
        console.log(`Client ${client} has sent us: ${data}`);
        ws.send(`Client ${client}. Voici la réponse`);
    });
    ws.on("close", () => {
        console.log("Client disconnected");
    });

});

