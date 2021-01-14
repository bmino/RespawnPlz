using BepInEx;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;

namespace RespawnPlz {
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.bmino.RespawnPlz", "RespawnPlz", "1.0.0")]
    public class RespawnPlz : BaseUnityPlugin {

        public static string CHAT_KEYWORD = "respawnplz";

        public void Awake() {

            Logger.LogMessage("Loaded RespawnPlz!");

            On.RoR2.Networking.GameNetworkManager.OnClientConnect += (On.RoR2.Networking.GameNetworkManager.orig_OnClientConnect orig, RoR2.Networking.GameNetworkManager self, NetworkConnection conn) => {

            };

            On.RoR2.Console.RunCmd += (On.RoR2.Console.orig_RunCmd orig, RoR2.Console self, RoR2.Console.CmdSender sender, string concommandName, List<string> userArgs) => {
                orig(self, sender, concommandName, userArgs);

                // Only handle our specific trigger
                if (concommandName != "say" || userArgs.FirstOrDefault() != RespawnPlz.CHAT_KEYWORD) {
                    return;
                };

                // Server-only logic
                if (!NetworkServer.active || Stage.instance == null) {
                    return;
                }

                CharacterMaster senderMaster = sender.networkUser.master;
                string senderUsername = sender.networkUser.userName;

                if (senderMaster.IsDeadAndOutOfLivesServer()) {
                    ChatHelper.sendBroadcastChat($"Won't respawn {senderUsername} because they are dead!");
                    return;
                }

                ChatHelper.sendBroadcastChat($"That idiot, {senderUsername}, got stuck again...");
                Stage.instance.RespawnCharacter(senderMaster);
            };
        }
    }
}