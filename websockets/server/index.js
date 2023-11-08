const WebSocket = require("ws");

const wss = new WebSocket.Server({ port: 8080 });

const clientIds = new Set();

wss.on("connection", (ws) => {
    console.log("New client attempting to connect");

    ws.isFirstMessage = true;

    ws.on("message", data => {
        const messageStr = data.toString();

        if (ws.isFirstMessage) {
            const proposedId = messageStr;  // Le client envoie son ID au premier message
            
            // Vérifier si l'ID est déjà utilisé
            if (clientIds.has(proposedId)) {
                ws.send("ID already in use");
                console.log(`Connection refused, ID ${proposedId} already in use`);
                return;
            } 
            
            // Ajouter le nouvel ID au registre et au WebSocket client
            else {
            clientIds.add(proposedId);
            ws.clientId = proposedId;
            ws.send("OK");
            console.log(`Client ${ws.clientId} connected with unique ID`);
            ws.isFirstMessage = false;
            return;
            }
        }

        // Traitement des messages suivants
        const [targetClientId, messageContent] = messageStr.split(':', 2);

        if (!targetClientId || !messageContent) {
            return;
        }

        console.log(`${ws.clientId} sends to ${targetClientId}: ${messageContent}`);

        // Trouver le client cible et envoyer le message
        let targetClientFound = false;
        wss.clients.forEach(client => {
            if (client.clientId === targetClientId && client.readyState === WebSocket.OPEN) {
                client.send(`${ws.clientId}:${messageContent}`);
                targetClientFound = true;
            }
        });

        if (!targetClientFound) {
            ws.send(`Client ID ${targetClientId} not found`);
        }
    });

    ws.on("close", () => {
        // Supprimer l'ID du client de l'ensemble lorsqu'il se déconnecte
        if (ws.clientId) {
            clientIds.delete(ws.clientId);
            console.log(`Client ID ${ws.clientId} disconnected`);
        }
    });
});
