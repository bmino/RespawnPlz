using RoR2;

namespace RespawnPlz {
    public static class ChatHelper {
        public static void sendBroadcastChat(string message) {
            Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = message });
        }
    }
}